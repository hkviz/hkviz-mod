using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using HKViz.Silk.PlayerDataSchema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace HKViz.Silk.PlayerDataGenerator;

[Generator]
public sealed class PlayerDataWriterGenerator : IIncrementalGenerator {
    private const string FieldIdsFileName = "PlayerDataFieldIds.json";

    private static readonly DiagnosticDescriptor MissingPlayerDataType = new(
        id: "HKVIZSILK001",
        title: "PlayerData type not found",
        messageFormat: "Could not resolve PlayerData type from references. Checked: {0}",
        category: "HKViz.Silk.Generator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor MissingFieldId = new(
        id: "HKVIZSILK002",
        title: "Missing PlayerData field id",
        messageFormat: "PlayerData field '{0}' is missing an id in PlayerDataFieldIds.json",
        category: "HKViz.Silk.Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor UnsupportedPlayerDataField = new(
        id: "HKVIZSILK003",
        title: "Unsupported PlayerData field type",
        messageFormat: "PlayerData field '{0}' of type '{1}' is not supported by PlayerDataWriter generation",
        category: "HKViz.Silk.Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor MissingEnumMemberId = new(
        id: "HKVIZSILK004",
        title: "Missing enum member id",
        messageFormat: "Enum member id for '{0}.{1}' is missing in PlayerDataFieldIds.json",
        category: "HKViz.Silk.Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<string> idMaps = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(FieldIdsFileName, StringComparison.OrdinalIgnoreCase))
            .Select(static (file, ct) => file.GetText(ct)?.ToString() ?? "{}");

        IncrementalValueProvider<(Compilation Compilation, ImmutableArray<string> Maps)> input = context.CompilationProvider.Combine(idMaps.Collect());

        context.RegisterSourceOutput(input, static (spc, data) => {
            Generate(spc, data.Compilation, data.Maps);
        });
    }

    private static void Generate(SourceProductionContext context, Compilation compilation, ImmutableArray<string> maps) {
        INamedTypeSymbol? playerDataType = null;
        string[] candidates = ["PlayerData", "Silksong.DataManager.PlayerData", "GlobalSettings.PlayerData"];

        foreach (string candidate in candidates) {
            playerDataType = compilation.GetTypeByMetadataName(candidate);
            if (playerDataType != null) {
                break;
            }
        }

        if (playerDataType == null) {
            context.ReportDiagnostic(Diagnostic.Create(MissingPlayerDataType, Location.None, string.Join(", ", candidates)));
            return;
        }

        IdMaps idMaps = ParseIdMap(maps);
        Dictionary<string, FieldSchema> fieldSchemaMap = idMaps.Fields;
        Dictionary<string, Dictionary<string, ushort>> enumMemberMapByType = idMaps.Enums;

        List<ObservedField> observedFields = playerDataType
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Where(static field => field.DeclaredAccessibility == Accessibility.Public && !field.IsStatic && !field.IsConst)
            .OrderBy(static field => field.Name, StringComparer.Ordinal)
            .SelectMany(ExpandObservedFields)
            .Where(static field => !PlayerDataSchemaRules.IsIgnoredPlayerDataFieldName(field.ContainingFieldName)
                                   && !PlayerDataSchemaRules.IsIgnoredPlayerDataFieldName(field.FieldName))
            .ToList();

        observedFields.AddRange(GetSyntheticHeroStateFields(compilation));

        List<ObservedField> allFields = observedFields
            .Select(field => {
                field.Kind = GetFieldKind(field);
                return field;
            })
            .OrderBy(static field => field.FieldName, StringComparer.Ordinal)
            .ToList();

        List<ObservedField> unsupportedFields = allFields
            .Where(static field => field.Kind == FieldKind.Unsupported)
            .ToList();

        foreach (ObservedField field in unsupportedFields) {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    UnsupportedPlayerDataField,
                    Location.None,
                    field.FieldName,
                    field.FieldType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                )
            );
        }

        List<ObservedField> supportedFields = allFields
            .Where(static field => field.Kind != FieldKind.Unsupported)
            .ToList();

        List<string> missingFieldIds = [];

        foreach (ObservedField field in supportedFields) {
            bool isEnumField = field.Kind == FieldKind.EnumId;
            string expectedTypeName = isEnumField ? "enum" : GetTypeName(field);
            string expectedEnumType = isEnumField ? GetEnumTypeKey(field.FieldType) : string.Empty;

            if (!fieldSchemaMap.TryGetValue(field.FieldName, out FieldSchema existingSchema) || existingSchema.Id == 0) {
                missingFieldIds.Add(field.FieldName);
                continue;
            }

            if (isEnumField) {
                string normalizedEnumType = !string.IsNullOrWhiteSpace(existingSchema.EnumType)
                    ? existingSchema.EnumType
                    : string.Equals(existingSchema.Type, "enum", StringComparison.OrdinalIgnoreCase)
                        ? expectedEnumType
                        : string.IsNullOrWhiteSpace(existingSchema.Type)
                            ? expectedEnumType
                            : existingSchema.Type;
                fieldSchemaMap[field.FieldName] = new FieldSchema(existingSchema.Id, "enum", normalizedEnumType);
            } else {
                string normalizedType = string.IsNullOrWhiteSpace(existingSchema.Type) ? expectedTypeName : existingSchema.Type;
                fieldSchemaMap[field.FieldName] = new FieldSchema(existingSchema.Id, normalizedType, string.Empty);
            }
        }

        if (missingFieldIds.Count > 0) {
            foreach (string fieldName in missingFieldIds.OrderBy(static name => name, StringComparer.Ordinal)) {
                context.ReportDiagnostic(Diagnostic.Create(MissingFieldId, Location.None, fieldName));
            }

            return;
        }

        Dictionary<string, EnumTypeInfo> enumTypeInfos = BuildEnumTypeInfos(supportedFields);
        List<(string EnumType, string Member)> missingEnumMemberIds = [];

        foreach (KeyValuePair<string, EnumTypeInfo> enumTypeEntry in enumTypeInfos.OrderBy(static kv => kv.Key, StringComparer.Ordinal)) {
            string enumTypeKey = enumTypeEntry.Key;
            EnumTypeInfo enumInfo = enumTypeEntry.Value;

            if (!enumMemberMapByType.TryGetValue(enumTypeKey, out Dictionary<string, ushort>? memberMap)) {
                foreach (string memberName in enumInfo.MemberNames.OrderBy(static name => name, StringComparer.Ordinal)) {
                    missingEnumMemberIds.Add((enumTypeKey, memberName));
                }

                continue;
            }

            foreach (string memberName in enumInfo.MemberNames.OrderBy(static name => name, StringComparer.Ordinal)) {
                if (!memberMap.ContainsKey(memberName)) {
                    missingEnumMemberIds.Add((enumTypeKey, memberName));
                }
            }
        }

        if (missingEnumMemberIds.Count > 0) {
            foreach ((string enumType, string member) in missingEnumMemberIds
                         .OrderBy(static item => item.EnumType, StringComparer.Ordinal)
                         .ThenBy(static item => item.Member, StringComparer.Ordinal)) {
                context.ReportDiagnostic(Diagnostic.Create(MissingEnumMemberId, Location.None, enumType, member));
            }

            return;
        }

        HashSet<string> usedIdentifiers = new(StringComparer.Ordinal);
        Dictionary<string, string> enumMapperNamesByType = new(StringComparer.Ordinal);
        foreach (string enumTypeKey in enumTypeInfos.Keys.OrderBy(static key => key, StringComparer.Ordinal)) {
            enumMapperNamesByType[enumTypeKey] = MakeUniqueIdentifier("MapEnum_" + enumTypeKey, usedIdentifiers);
        }

        for (int i = 0; i < supportedFields.Count; i++) {
            ObservedField field = supportedFields[i];
            field.FieldIdName = MakeUniqueIdentifier("FieldId_" + field.FieldName, usedIdentifiers);
            field.PreviousName = MakeUniqueIdentifier("_previous_" + field.FieldName, usedIdentifiers);
            field.CurrentName = MakeUniqueIdentifier("current_" + field.FieldName, usedIdentifiers);
            field.PreviousValueGetterName = MakeUniqueIdentifier("PreviousValue" + field.FieldName, usedIdentifiers);

            if (field.Kind == FieldKind.EnumId && field.FieldType is INamedTypeSymbol enumTypeSymbol) {
                string enumTypeKey = GetEnumTypeKey(enumTypeSymbol);
                field.EnumTypeKey = enumTypeKey;
                field.EnumMapperName = enumMapperNamesByType[enumTypeKey];
            }

            supportedFields[i] = field;
        }

        bool hasHeroStateFields = supportedFields.Any(static field => string.Equals(field.ContainingFieldName, "heroState", StringComparison.Ordinal));

        string playerDataTypeName = playerDataType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

         StringBuilder source = new();
         source.AppendLine("// <auto-generated />");
         source.AppendLine("#nullable enable");
         source.AppendLine();
         source.AppendLine("using HKViz.Silk.Recording.DataHelpers;");
         source.AppendLine();
         source.AppendLine("namespace HKViz.Silk.Recording;");
        source.AppendLine();
        source.AppendLine("public partial class PlayerDataWriter {");
        source.AppendLine("    public static class FieldIds {");

        foreach (ObservedField field in supportedFields) {
            source.Append("        public const ushort ");
            source.Append(field.FieldIdName);
            source.Append(" = ");
            source.Append(fieldSchemaMap[field.FieldName].Id);
            source.AppendLine(";");
        }

        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private bool _hasPreviousPlayerDataValues;");

        foreach (ObservedField field in supportedFields) {
            source.Append("    private ");
            source.Append(GetStorageTypeDisplay(field));
            source.Append(' ');
            source.Append(field.PreviousName);
            source.AppendLine(" = default!;");
        }

        source.AppendLine();
        foreach (ObservedField field in supportedFields) {
            source.Append("    public ");
            source.Append(GetStorageTypeDisplay(field));
            source.Append(' ');
            source.Append(field.PreviousValueGetterName);
            source.Append(" => ");
            source.Append(field.PreviousName);
            source.AppendLine(";");
        }

        source.AppendLine();
        source.AppendLine("    public partial void WriteAll() {");
        source.Append("        ");
        source.Append(playerDataTypeName);
        source.AppendLine(" playerData = global::PlayerData.instance;");
        source.AppendLine("        if (playerData == null) {");
        source.AppendLine("            return;");
        source.AppendLine("        }");
        if (hasHeroStateFields) {
            source.AppendLine("        global::HeroController heroController = global::HeroController.instance;");
        }
        source.AppendLine();

        foreach (ObservedField field in supportedFields) {
            if (TryGetNamedMapInfo(field.FieldType, out NamedMapInfo namedMapInfo)) {
                source.Append("        var ");
                source.Append(field.CurrentName);
                source.Append(" = ");
                source.Append(GetReadExpression(field, "playerData"));
                source.AppendLine(";");

                source.Append("        WriteNamedMapIfChanged(FieldIds.");
                source.Append(field.FieldIdName);
                source.Append(", ref ");
                source.Append(field.PreviousName);
                source.Append(", ");
                source.Append(field.CurrentName);
                source.Append(", ");
                source.Append(namedMapInfo.EqualsMethodName);
                source.Append(", ");
                source.Append(namedMapInfo.CopyMethodName);
                source.Append(", ");
                source.Append(namedMapInfo.WriteMethodName);
                source.AppendLine(");");
                source.AppendLine();
                continue;
            }

            source.Append("        ");
            source.Append(GetStorageTypeDisplay(field));
            source.Append(' ');
            source.Append(field.CurrentName);
            source.Append(" = ");
            source.Append(GetReadExpression(field, "playerData"));
            source.AppendLine(";");

            switch (field.Kind) {
                case FieldKind.Bool:
                    source.Append("        runFiles.WritePlayerDataBoolChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.IntLike:
                    source.Append("        runFiles.WritePlayerDataIntChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", (int)");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.EnumId:
                    source.Append("        runFiles.WritePlayerDataEnumChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.ULong:
                    source.Append("        runFiles.WritePlayerDataULongChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.Vector3:
                    source.Append("        runFiles.WritePlayerDataVector3Change(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.Vector2:
                    source.Append("        runFiles.WritePlayerDataVector2Change(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.IntArray:
                    if (IsIntList(field.FieldType)) {
                        source.Append("        runFiles.WritePlayerDataIntListFullChange(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    } else {
                        source.Append("        runFiles.WritePlayerDataIntArrayFullChange(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    }
                    break;
                case FieldKind.Float:
                    source.Append("        runFiles.WritePlayerDataFloatChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                 case FieldKind.String:
                    source.Append("        runFiles.WritePlayerDataStringChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.Append(" ?? string.Empty");
                    source.AppendLine(");");
                    break;
                case FieldKind.StringCollection:
                    if (IsStringSet(field.FieldType)) {
                        source.Append("        runFiles.WritePlayerDataStringSetFullChange(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    } else {
                        source.Append("        runFiles.WritePlayerDataStringCollectionFullChange(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.Append(" == null ? new string[0] : ");
                        source.Append(field.CurrentName);
                        source.AppendLine(".ToArray());");
                    }
                    break;
                case FieldKind.StoryEventList:
                    source.Append("        runFiles.WritePlayerDataStoryEventListFullChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.WrappedVector2ListArray:
                    source.Append("        runFiles.WritePlayerDataWrappedVector2ListFullChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.Guid:
                    source.Append("        runFiles.WritePlayerDataGuidChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
            }

            // All fields use the same assignment pattern now
            source.Append("        ");
            source.Append(field.PreviousName);
            source.Append(" = ");
            source.Append(GetPreviousAssignmentExpression(field, field.CurrentName));
            source.AppendLine(";");
            source.AppendLine();
        }

        source.AppendLine("        _hasPreviousPlayerDataValues = true;");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    public partial void WriteChanged() {");
        source.Append("        ");
        source.Append(playerDataTypeName);
        source.AppendLine(" playerData = global::PlayerData.instance;");
        source.AppendLine("        if (playerData == null) {");
        source.AppendLine("            return;");
        source.AppendLine("        }");
        if (hasHeroStateFields) {
            source.AppendLine("        global::HeroController heroController = global::HeroController.instance;");
        }
        source.AppendLine();
        source.AppendLine("        if (!_hasPreviousPlayerDataValues) {");
         foreach (ObservedField field in supportedFields) {
             if (TryGetNamedMapInfo(field.FieldType, out NamedMapInfo namedMapInfo)) {
                 source.Append("            WriteNamedMapIfChanged(FieldIds.");
                 source.Append(field.FieldIdName);
                 source.Append(", ref ");
                 source.Append(field.PreviousName);
                 source.Append(", ");
                 source.Append(GetReadExpression(field, "playerData"));
                 source.Append(", ");
                 source.Append(namedMapInfo.EqualsMethodName);
                 source.Append(", ");
                 source.Append(namedMapInfo.CopyMethodName);
                 source.Append(", ");
                 source.Append(namedMapInfo.WriteMethodName);
                 source.AppendLine(");");
                 continue;
             }

             source.Append("            ");
             source.Append(field.PreviousName);
             source.Append(" = ");
             source.Append(GetPreviousAssignmentExpression(field, GetReadExpression(field, "playerData")));
             source.AppendLine(";");
         }
         source.AppendLine("            _hasPreviousPlayerDataValues = true;");
         source.AppendLine("            return;");
         source.AppendLine("        }");
         source.AppendLine();

        foreach (ObservedField field in supportedFields) {
            if (TryGetNamedMapInfo(field.FieldType, out NamedMapInfo namedMapInfo)) {
                source.Append("        var ");
                source.Append(field.CurrentName);
                source.Append(" = ");
                source.Append(GetReadExpression(field, "playerData"));
                source.AppendLine(";");

                source.Append("        WriteNamedMapIfChanged(FieldIds.");
                source.Append(field.FieldIdName);
                source.Append(", ref ");
                source.Append(field.PreviousName);
                source.Append(", ");
                source.Append(field.CurrentName);
                source.Append(", ");
                source.Append(namedMapInfo.EqualsMethodName);
                source.Append(", ");
                source.Append(namedMapInfo.CopyMethodName);
                source.Append(", ");
                source.Append(namedMapInfo.WriteMethodName);
                source.AppendLine(");");
                source.AppendLine();
                continue;
            }

            source.Append("        ");
            source.Append(GetStorageTypeDisplay(field));
            source.Append(' ');
            source.Append(field.CurrentName);
            source.Append(" = ");
            source.Append(GetReadExpression(field, "playerData"));
            source.AppendLine(";");

            switch (field.Kind) {
                case FieldKind.Bool:
                    source.Append("        WriteBoolIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.IntLike:
                    source.Append("        WriteIntIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", (int)");
                    source.Append(field.PreviousName);
                    source.Append(", (int)");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.EnumId:
                    source.Append("        WriteEnumIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.ULong:
                    source.Append("        WriteULongIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.Vector3:
                    source.Append("        WriteVector3IfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.Vector2:
                    source.Append("        WriteVector2IfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                 case FieldKind.IntArray:
                    if (IsIntList(field.FieldType)) {
                        source.Append("        WriteIntListIfChanged(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append("ref ");
                        source.Append(field.PreviousName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    } else {
                        source.Append("        WriteIntArrayIfChanged(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append("ref ");
                        source.Append(field.PreviousName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    }
                    break;
                case FieldKind.Float:
                    source.Append("        WriteFloatIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                 case FieldKind.String:
                    source.Append("        WriteStringIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.StringCollection:
                    if (IsStringSet(field.FieldType)) {
                        source.Append("        WriteStringSetIfChanged(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append("ref ");
                        source.Append(field.PreviousName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    } else {
                        source.Append("        WriteStringListIfChanged(FieldIds.");
                        source.Append(field.FieldIdName);
                        source.Append(", ");
                        source.Append("ref ");
                        source.Append(field.PreviousName);
                        source.Append(", ");
                        source.Append(field.CurrentName);
                        source.AppendLine(");");
                    }
                    break;
                case FieldKind.StoryEventList:
                    source.Append("        WriteStoryEventListIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ref ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.WrappedVector2ListArray:
                    source.Append("        WritePlacedMarkersIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ref ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
                case FieldKind.Guid:
                    source.Append("        WriteGuidIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
            }

            if (field.Kind != FieldKind.IntArray && field.Kind != FieldKind.StringCollection && field.Kind != FieldKind.NamedMap && field.Kind != FieldKind.StoryEventList && field.Kind != FieldKind.WrappedVector2ListArray) {
                source.Append("        ");
                source.Append(field.PreviousName);
                source.Append(" = ");
                source.Append(GetPreviousAssignmentExpression(field, field.CurrentName));
                source.AppendLine(";");
            }
            source.AppendLine();
        }

        source.AppendLine("    }");
        source.AppendLine();

        foreach (KeyValuePair<string, EnumTypeInfo> enumTypeEntry in enumTypeInfos.OrderBy(static kv => kv.Key, StringComparer.Ordinal)) {
            string enumTypeKey = enumTypeEntry.Key;
            EnumTypeInfo enumInfo = enumTypeEntry.Value;

            if (!enumMapperNamesByType.TryGetValue(enumTypeKey, out string? mapperName)) {
                continue;
            }

            if (!enumMemberMapByType.TryGetValue(enumTypeKey, out Dictionary<string, ushort>? memberIds)) {
                continue;
            }

            source.Append("    private static ushort ");
            source.Append(mapperName);
            source.Append('(');
            source.Append(enumInfo.FullyQualifiedTypeName);
            source.AppendLine(" value) {");
            source.Append("        string? memberName = global::System.Enum.GetName(typeof(");
            source.Append(enumInfo.FullyQualifiedTypeName);
            source.AppendLine("), value);");
            source.AppendLine("        return memberName switch {");

            foreach (IFieldSymbol memberSymbol in enumInfo.MemberSymbols.OrderBy(static m => m.Name, StringComparer.Ordinal)) {
                if (!memberIds.TryGetValue(memberSymbol.Name, out ushort memberId)) {
                    continue;
                }

                source.Append("            \"");
                source.Append(memberSymbol.Name);
                source.Append("\" => ");
                source.Append(memberId);
                source.AppendLine(",");
            }

            source.Append("            _ => throw new global::System.InvalidOperationException(\"Unknown enum value for ");
            source.Append(enumTypeKey);
            source.AppendLine("\"),");
            source.AppendLine("        };");
            source.AppendLine("    }");
            source.AppendLine();
        }

        source.AppendLine("    private static global::System.Guid GuidFromBytesOrEmpty(global::System.Byte[]? value) {");
        source.AppendLine("        return value is { Length: 16 } ? new global::System.Guid(value) : global::System.Guid.Empty;");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static global::System.Collections.Generic.List<int>? CopyIntList(global::System.Collections.Generic.List<int>? value) {");
        source.AppendLine("        if (value is null || value.Count == 0) {");
        source.AppendLine("            return null;");
        source.AppendLine("        }");
        source.AppendLine("        return new global::System.Collections.Generic.List<int>(value);");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static global::System.Collections.Generic.List<string>? CopyStringList(global::System.Collections.Generic.List<string>? value) {");
        source.AppendLine("        if (value is null || value.Count == 0) {");
        source.AppendLine("            return null;");
        source.AppendLine("        }");
        source.AppendLine("        return new global::System.Collections.Generic.List<string>(value);");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static global::System.Collections.Generic.HashSet<string>? CopyStringSet(global::System.Collections.Generic.HashSet<string>? value) {");
        source.AppendLine("        if (value is null || value.Count == 0) {");
        source.AppendLine("            return null;");
        source.AppendLine("        }");
        source.AppendLine("        return new global::System.Collections.Generic.HashSet<string>(value, global::System.StringComparer.Ordinal);");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("    private static global::System.Collections.Generic.List<global::PlayerStory.EventInfo>? CopyStoryEventList(global::System.Collections.Generic.List<global::PlayerStory.EventInfo>? value) {");
        source.AppendLine("        if (value is null || value.Count == 0) {");
        source.AppendLine("            return null;");
        source.AppendLine("        }");
        source.AppendLine("        return new global::System.Collections.Generic.List<global::PlayerStory.EventInfo>(value);");
        source.AppendLine("    }");
        source.AppendLine();
        source.AppendLine("}");
        source.AppendLine();
        context.AddSource("PlayerDataWriter.generated.cs", SourceText.From(source.ToString(), Encoding.UTF8));
    }

    private static IdMaps ParseIdMap(ImmutableArray<string> maps) {
        string json = maps.Length > 0 ? maps[0] : "{}";
        try {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            bool hasFields = TryGetPropertyIgnoreCase(root, "fields", out JsonElement fieldsElement);
            bool hasEnums = TryGetPropertyIgnoreCase(root, "enums", out JsonElement enumsElement);

            if (root.ValueKind == JsonValueKind.Object && (hasFields || hasEnums)) {
                Dictionary<string, FieldSchema> fields = hasFields
                    ? ReadFieldMapObject(fieldsElement)
                    : new Dictionary<string, FieldSchema>(StringComparer.Ordinal);
                Dictionary<string, Dictionary<string, ushort>> enums = hasEnums
                    ? ReadEnumMapObject(enumsElement)
                    : new Dictionary<string, Dictionary<string, ushort>>(StringComparer.Ordinal);
                return new IdMaps(fields, enums);
            }

            Dictionary<string, ushort>? legacy = JsonSerializer.Deserialize<Dictionary<string, ushort>>(json);
            Dictionary<string, FieldSchema> legacyFields = new(StringComparer.Ordinal);
            if (legacy != null) {
                foreach (KeyValuePair<string, ushort> entry in legacy) {
                    legacyFields[entry.Key] = new FieldSchema(entry.Value, string.Empty);
                }
            }

            return new IdMaps(legacyFields, new Dictionary<string, Dictionary<string, ushort>>(StringComparer.Ordinal));
        } catch {
            return new IdMaps(
                new Dictionary<string, FieldSchema>(StringComparer.Ordinal),
                new Dictionary<string, Dictionary<string, ushort>>(StringComparer.Ordinal)
            );
        }
    }

    private static Dictionary<string, FieldSchema> ReadFieldMapObject(JsonElement element) {
        Dictionary<string, FieldSchema> map = new(StringComparer.Ordinal);
        if (element.ValueKind != JsonValueKind.Object) {
            return map;
        }

        foreach (JsonProperty prop in element.EnumerateObject()) {
            if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.TryGetUInt16(out ushort legacyId)) {
                map[prop.Name] = new FieldSchema(legacyId, string.Empty, string.Empty);
                continue;
            }

            if (prop.Value.ValueKind != JsonValueKind.Object) {
                continue;
            }

            bool hasId = false;
            ushort id = 0;
            string type = string.Empty;
            string enumType = string.Empty;

            foreach (JsonProperty child in prop.Value.EnumerateObject()) {
                if (string.Equals(child.Name, "id", StringComparison.OrdinalIgnoreCase)
                    && child.Value.ValueKind == JsonValueKind.Number
                    && child.Value.TryGetUInt16(out ushort parsedId)) {
                    hasId = true;
                    id = parsedId;
                    continue;
                }

                if (string.Equals(child.Name, "type", StringComparison.OrdinalIgnoreCase)
                    && child.Value.ValueKind == JsonValueKind.String) {
                    type = child.Value.GetString() ?? string.Empty;
                    continue;
                }

                if (string.Equals(child.Name, "enumType", StringComparison.OrdinalIgnoreCase)
                    && child.Value.ValueKind == JsonValueKind.String) {
                    enumType = child.Value.GetString() ?? string.Empty;
                }
            }

            if (!hasId) {
                continue;
            }

            map[prop.Name] = new FieldSchema(id, type, enumType);
        }

        return map;
    }

    private static Dictionary<string, Dictionary<string, ushort>> ReadEnumMapObject(JsonElement element) {
        Dictionary<string, Dictionary<string, ushort>> map = new(StringComparer.Ordinal);
        if (element.ValueKind != JsonValueKind.Object) {
            return map;
        }

        foreach (JsonProperty enumType in element.EnumerateObject()) {
            if (enumType.Value.ValueKind != JsonValueKind.Object) {
                continue;
            }

            Dictionary<string, ushort> members = new(StringComparer.Ordinal);
            foreach (JsonProperty member in enumType.Value.EnumerateObject()) {
                if (member.Value.ValueKind == JsonValueKind.Number && member.Value.TryGetUInt16(out ushort id)) {
                    members[member.Name] = id;
                }
            }

            map[enumType.Name] = members;
        }

        return map;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement root, string propertyName, out JsonElement value) {
        if (root.ValueKind != JsonValueKind.Object) {
            value = default;
            return false;
        }

        foreach (JsonProperty property in root.EnumerateObject()) {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase)) {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private static Dictionary<string, EnumTypeInfo> BuildEnumTypeInfos(List<ObservedField> supportedFields) {
        Dictionary<string, EnumTypeInfo> infos = new(StringComparer.Ordinal);

        foreach (ObservedField field in supportedFields.Where(static f => f.Kind == FieldKind.EnumId)) {
            if (field.FieldType is not INamedTypeSymbol enumTypeSymbol) {
                continue;
            }

            string enumTypeKey = GetEnumTypeKey(enumTypeSymbol);
            if (infos.ContainsKey(enumTypeKey)) {
                continue;
            }

            List<IFieldSymbol> memberSymbols = enumTypeSymbol
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Where(static m => m.IsStatic && m.ConstantValue != null && m.Name != "value__")
                .ToList();

            infos[enumTypeKey] = new EnumTypeInfo(
                enumTypeKey,
                enumTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                memberSymbols
            );
        }

        return infos;
    }

    private static string MakeUniqueIdentifier(string name, HashSet<string> usedIdentifiers) {
        string baseName = SanitizeIdentifier(name);
        string candidate = baseName;
        int index = 1;
        while (!usedIdentifiers.Add(candidate)) {
            candidate = baseName + "_" + index;
            index++;
        }

        return candidate;
    }

    private static string SanitizeIdentifier(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return "Field";
        }

        StringBuilder sb = new(name.Length + 1);
        if (!SyntaxFacts.IsIdentifierStartCharacter(name[0])) {
            sb.Append('_');
        }

        foreach (char ch in name) {
            sb.Append(SyntaxFacts.IsIdentifierPartCharacter(ch) ? ch : '_');
        }

        return sb.ToString();
    }

    private static string GetStorageTypeDisplay(ObservedField field) {
        if (field.Kind == FieldKind.EnumId) {
            return "ushort";
        }

        if (field.Kind == FieldKind.NamedMap && TryGetNamedMapInfo(field.FieldType, out NamedMapInfo namedMapInfo)) {
            return "global::System.Collections.Generic.Dictionary<string, " + namedMapInfo.SnapshotValueTypeDisplay + ">";
        }

        if (field.Kind == FieldKind.IntArray && IsIntList(field.FieldType)) {
            return "global::System.Collections.Generic.List<int>";
        }

        if (field.Kind == FieldKind.StringCollection && IsStringList(field.FieldType)) {
            return "global::System.Collections.Generic.List<string>";
        }

        if (field.Kind == FieldKind.StringCollection && IsStringSet(field.FieldType)) {
            return "global::System.Collections.Generic.HashSet<string>";
        }

        if (IsGuidByteArray(field.FieldType, field.FieldName)) {
            return "global::System.Guid";
        }

        return field.FieldType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    private static string GetReadExpression(ObservedField field, string instanceName) {
        string sourceExpression = string.IsNullOrEmpty(field.ReadExpressionOverride)
            ? instanceName + "." + field.ContainingFieldName + field.ReadExpressionSuffix
            : field.ReadExpressionOverride;

        if (field.Kind == FieldKind.EnumId) {
            return field.EnumMapperName + "(" + sourceExpression + ")";
        }

        if (field.Kind == FieldKind.NamedMap && TryGetNamedMapInfo(field.FieldType, out NamedMapInfo namedMapInfo)) {
            return sourceExpression + namedMapInfo.ReadExpressionSuffix;
        }

        if (IsIntList(field.FieldType)) {
            return sourceExpression;
        }

        if (IsStringList(field.FieldType)) {
            return sourceExpression;
        }

        if (IsStringSet(field.FieldType)) {
            return sourceExpression;
        }

        if (IsGuidByteArray(field.FieldType, field.FieldName)) {
            return "GuidFromBytesOrEmpty(" + sourceExpression + ")";
        }

        return sourceExpression;
    }

    private static string GetPreviousAssignmentExpression(ObservedField field, string valueExpression) {
        if (field.Kind == FieldKind.IntArray && IsIntList(field.FieldType)) {
            return "CopyIntList(" + valueExpression + ")";
        }

        if (field.Kind == FieldKind.StringCollection && IsStringList(field.FieldType)) {
            return "CopyStringList(" + valueExpression + ")";
        }

        if (field.Kind == FieldKind.StringCollection && IsStringSet(field.FieldType)) {
            return "CopyStringSet(" + valueExpression + ")";
        }

        if (field.Kind == FieldKind.StoryEventList && IsStoryEventInfoList(field.FieldType)) {
            return "CopyStoryEventList(" + valueExpression + ")";
        }

        if (field.Kind == FieldKind.WrappedVector2ListArray && IsWrappedVector2ListArray(field.FieldType)) {
            return "WrappedVector2ListDataHelper.CopyArray(" + valueExpression + ")";
        }

        return valueExpression;
    }

     private static bool TryGetNamedMapInfo(ITypeSymbol type, out NamedMapInfo info) {
         if (IsSteelQuestSpotArray(type)) {
             info = new NamedMapInfo("bool", ".EnumerateBySceneName()", "BoolDataHelper.Equals", "BoolDataHelper.Copy", "BinaryWriterExtensions.WriteBool");
             return true;
         }

         if (IsStringIntDictionary(type)) {
             info = new NamedMapInfo("int", string.Empty, "IntDataHelper.Equals", "IntDataHelper.Copy", "BinaryWriterExtensions.WriteInt");
             return true;
         }

         if (type is not INamedTypeSymbol namedType) {
             info = default;
             return false;
         }

         string typeName = namedType.Name;
         info = typeName switch {
             "CollectableItemsData" => new NamedMapInfo("global::CollectableItemsData.Data", ".Enumerate()", "CollectableItemsDataHelper.Equals", "CollectableItemsDataHelper.Copy", "BinaryWriterExtensions.WriteCollectableItemsData"),
             "CollectableRelicsData" => new NamedMapInfo("global::CollectableRelicsData.Data", ".Enumerate()", "CollectableRelicsDataHelper.Equals", "CollectableRelicsDataHelper.Copy", "BinaryWriterExtensions.WriteCollectableRelicsData"),
             "CollectableMementosData" => new NamedMapInfo("global::CollectableMementosData.Data", ".Enumerate()", "CollectableMementosDataHelper.Equals", "CollectableMementosDataHelper.Copy", "BinaryWriterExtensions.WriteCollectableMementosData"),
             "MateriumItemsData" => new NamedMapInfo("global::MateriumItemsData.Data", ".Enumerate()", "MateriumItemsDataHelper.Equals", "MateriumItemsDataHelper.Copy", "BinaryWriterExtensions.WriteMateriumItemsData"),
             "QuestCompletionData" => new NamedMapInfo("global::QuestCompletionData.Completion", ".Enumerate()", "QuestCompletionDataHelper.Equals", "QuestCompletionDataHelper.Copy", "BinaryWriterExtensions.WriteQuestCompletionData"),
             "QuestRumourData" => new NamedMapInfo("global::QuestRumourData.Data", ".Enumerate()", "QuestRumourDataHelper.Equals", "QuestRumourDataHelper.Copy", "BinaryWriterExtensions.WriteQuestRumourData"),
             "ToolItemLiquidsData" => new NamedMapInfo("global::ToolItemLiquidsData.Data", ".Enumerate()", "ToolItemLiquidsDataHelper.Equals", "ToolItemLiquidsDataHelper.Copy", "BinaryWriterExtensions.WriteToolItemLiquidsData"),
             "ToolItemsData" => new NamedMapInfo("global::ToolItemsData.Data", ".Enumerate()", "ToolItemsDataHelper.Equals", "ToolItemsDataHelper.Copy", "BinaryWriterExtensions.WriteToolItemsData"),
             "ToolCrestsData" => new NamedMapInfo("global::ToolCrestsData.Data", ".Enumerate()", "ToolCrestsDataHelper.Equals", "ToolCrestsDataHelper.Copy", "BinaryWriterExtensions.WriteToolCrestsData"),
             "FloatingCrestSlotsData" => new NamedMapInfo("global::ToolCrestsData.SlotData", ".Enumerate()", "ToolCrestsDataHelper.EqualsSlotData", "ToolCrestsDataHelper.CopySlotData", "BinaryWriterExtensions.WriteToolCrestsSlotData"),
             "EnemyJournalKillData" => new NamedMapInfo("global::EnemyJournalKillData.KillData", ".Dictionary", "EnemyJournalKillDataHelper.Equals", "EnemyJournalKillDataHelper.Copy", "BinaryWriterExtensions.WriteEnemyJournalKillData"),
             _ => default,
         };

         return !string.IsNullOrEmpty(info.SnapshotValueTypeDisplay);
     }

    private static string GetTypeName(ObservedField field) {
        if (IsGuidByteArray(field.FieldType, field.FieldName)) {
            return "guid";
        }

        if (field.FieldType.TypeKind == TypeKind.Enum) {
            return GetEnumTypeKey(field.FieldType);
        }

        return field.FieldType.SpecialType switch {
            SpecialType.System_Boolean => "bool",
            SpecialType.System_Byte => "byte",
            SpecialType.System_SByte => "sbyte",
            SpecialType.System_Int16 => "short",
            SpecialType.System_UInt16 => "ushort",
            SpecialType.System_Int32 => "int",
            SpecialType.System_UInt64 => "ulong",
            SpecialType.System_Single => "float",
            SpecialType.System_String => "string",
            _ when IsIntList(field.FieldType) => "list<int>",
            _ when IsStringList(field.FieldType) => "list<string>",
            _ when IsStringSet(field.FieldType) => "hashset<string>",
            _ when IsStringIntDictionary(field.FieldType) => "dictionary<string,int>",
            _ when IsStoryEventInfoList(field.FieldType) => "list<playerstory.eventinfo>",
            _ when IsWrappedVector2ListArray(field.FieldType) => "wrappedvector2list[]",
            _ when IsIntArray(field.FieldType) => "int[]",
            _ when field.FieldType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::UnityEngine.Vector2" => "vector2",
            _ when field.FieldType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::UnityEngine.Vector3" => "vector3",
            _ when field.FieldType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::System.Guid" => "guid",
            _ => field.FieldType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
        };
    }

    private static FieldKind GetFieldKind(ObservedField field) {
        if (IsGuidByteArray(field.FieldType, field.FieldName)) {
            return FieldKind.Guid;
        }

        ITypeSymbol symbol = field.FieldType;
        if (symbol.TypeKind == TypeKind.Enum) {
            return FieldKind.EnumId;
        }

        if (IsIntArray(symbol)) {
            return FieldKind.IntArray;
        }

        if (IsIntList(symbol)) {
            return FieldKind.IntArray;
        }

        if (IsStringList(symbol)) {
            return FieldKind.StringCollection;
        }

        if (IsStringSet(symbol)) {
            return FieldKind.StringCollection;
        }

        if (IsStoryEventInfoList(symbol)) {
            return FieldKind.StoryEventList;
        }

        if (IsWrappedVector2ListArray(symbol)) {
            return FieldKind.WrappedVector2ListArray;
        }

        if (TryGetNamedMapInfo(symbol, out _)) {
            return FieldKind.NamedMap;
        }

        return symbol.SpecialType switch {
            SpecialType.System_Boolean => FieldKind.Bool,
            SpecialType.System_Byte => FieldKind.IntLike,
            SpecialType.System_SByte => FieldKind.IntLike,
            SpecialType.System_Int16 => FieldKind.IntLike,
            SpecialType.System_UInt16 => FieldKind.IntLike,
            SpecialType.System_Int32 => FieldKind.IntLike,
            SpecialType.System_UInt64 => FieldKind.ULong,
            SpecialType.System_Single => FieldKind.Float,
            SpecialType.System_String => FieldKind.String,
            _ when symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::UnityEngine.Vector2" => FieldKind.Vector2,
            _ when symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::UnityEngine.Vector3" => FieldKind.Vector3,
            _ when symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::System.Guid" => FieldKind.Guid,
            _ => FieldKind.Unsupported,
        };
    }

    private static bool IsGuidByteArray(ITypeSymbol type, string fieldName) {
        return type is IArrayTypeSymbol arrayType
               && arrayType.ElementType.SpecialType == SpecialType.System_Byte
               && fieldName.EndsWith("Guid", StringComparison.Ordinal);
    }

    private static bool IsIntArray(ITypeSymbol type) {
        return type is IArrayTypeSymbol arrayType
               && arrayType.ElementType.SpecialType == SpecialType.System_Int32;
    }

    private static bool IsIntList(ITypeSymbol type) {
        return type is INamedTypeSymbol {
            IsGenericType: true,
            Name: "List",
            TypeArguments.Length: 1,
        } namedType
               && namedType.ContainingNamespace.ToDisplayString() == "System.Collections.Generic"
               && namedType.TypeArguments[0].SpecialType == SpecialType.System_Int32;
    }

    private static bool IsStringList(ITypeSymbol type) {
        return type is INamedTypeSymbol {
            IsGenericType: true,
            Name: "List",
            TypeArguments.Length: 1,
        } namedType
               && namedType.ContainingNamespace.ToDisplayString() == "System.Collections.Generic"
               && namedType.TypeArguments[0].SpecialType == SpecialType.System_String;
    }

    private static bool IsStringSet(ITypeSymbol type) {
        return type is INamedTypeSymbol {
            IsGenericType: true,
            Name: "HashSet",
            TypeArguments.Length: 1,
        } namedType
               && namedType.ContainingNamespace.ToDisplayString() == "System.Collections.Generic"
               && namedType.TypeArguments[0].SpecialType == SpecialType.System_String;
    }

    private static bool IsStringIntDictionary(ITypeSymbol type) {
        return type is INamedTypeSymbol {
            IsGenericType: true,
            Name: "Dictionary",
            TypeArguments.Length: 2,
        } namedType
               && namedType.ContainingNamespace.ToDisplayString() == "System.Collections.Generic"
               && namedType.TypeArguments[0].SpecialType == SpecialType.System_String
               && namedType.TypeArguments[1].SpecialType == SpecialType.System_Int32;
    }

    private static bool IsStoryEventInfoList(ITypeSymbol type) {
        return type is INamedTypeSymbol {
            IsGenericType: true,
            Name: "List",
            TypeArguments.Length: 1,
        } namedType
               && namedType.ContainingNamespace.ToDisplayString() == "System.Collections.Generic"
               && namedType.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::PlayerStory.EventInfo";
    }

    private static bool IsSteelQuestSpotArray(ITypeSymbol type) {
        return PlayerDataSchemaRules.IsSteelQuestSpotArrayTypeName(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
    }

    private static bool IsWrappedVector2ListArray(ITypeSymbol type) {
        return PlayerDataSchemaRules.IsWrappedVector2ListArrayTypeName(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
    }

    private static IEnumerable<ObservedField> ExpandObservedFields(IFieldSymbol field) {
        foreach (ObservedField observed in ExpandObservedFieldsRecursive(field, field.Type, field.Name, string.Empty)) {
            yield return observed;
        }
    }

    private static IEnumerable<ObservedField> ExpandObservedFieldsRecursive(IFieldSymbol rootField, ITypeSymbol currentType, string fieldNamePrefix, string readSuffixPrefix) {
        if (!IsFlattenedWrapperType(currentType)) {
            yield return new ObservedField(rootField, currentType, fieldNamePrefix, readSuffixPrefix);
            yield break;
        }

        if (currentType is not INamedTypeSymbol wrapperType) {
            yield break;
        }

        foreach (IFieldSymbol nestedField in wrapperType.GetMembers().OfType<IFieldSymbol>().Where(static f => f.DeclaredAccessibility == Accessibility.Public && !f.IsStatic && !f.IsConst).OrderBy(static f => f.Name, StringComparer.Ordinal)) {
            string nestedFieldName = fieldNamePrefix + "_" + nestedField.Name;
            string nestedReadSuffix = readSuffixPrefix + "." + nestedField.Name;

            foreach (ObservedField observed in ExpandObservedFieldsRecursive(rootField, nestedField.Type, nestedFieldName, nestedReadSuffix)) {
                yield return observed;
            }
        }
    }

    private static bool IsFlattenedWrapperType(ITypeSymbol type) {
        return PlayerDataSchemaRules.IsFlattenedWrapperTypeName(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
    }

    private static IEnumerable<ObservedField> GetSyntheticHeroStateFields(Compilation compilation) {
        INamedTypeSymbol? heroStateType = compilation.GetTypeByMetadataName("HeroControllerStates");
        if (heroStateType == null) {
            yield break;
        }

        foreach (IFieldSymbol field in heroStateType.GetMembers().OfType<IFieldSymbol>().Where(static f => !f.IsStatic && !f.IsConst).OrderBy(static f => f.Name, StringComparer.Ordinal)) {
            if (PlayerDataSchemaRules.IsIgnoredHeroStateFieldName(field.Name)) {
                continue;
            }

            string fieldType = field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            string readExpression = "heroController == null ? default(" + fieldType + ") : heroController.cState." + field.Name;
            yield return ObservedField.CreateSynthetic(field.Type, "heroState_" + field.Name, "heroState", readExpression);
        }
    }

    private static string GetEnumTypeKey(ITypeSymbol enumSymbol) {
        return enumSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
    }

    private enum FieldKind {
        Unsupported = 0,
        Bool,
        IntLike,
        EnumId,
        ULong,
        IntArray,
        Vector2,
        Vector3,
        Float,
        String,
        Guid,
        StringCollection,  // List<string> or HashSet<string>
        NamedMap,
        StoryEventList,
        WrappedVector2ListArray,
    }

    private readonly struct NamedMapInfo {
        public NamedMapInfo(string snapshotValueTypeDisplay, string readExpressionSuffix, string equalsMethodName, string copyMethodName, string writeMethodName) {
            SnapshotValueTypeDisplay = snapshotValueTypeDisplay;
            ReadExpressionSuffix = readExpressionSuffix;
            EqualsMethodName = equalsMethodName;
            CopyMethodName = copyMethodName;
            WriteMethodName = writeMethodName;
        }

        public string SnapshotValueTypeDisplay { get; }
        public string ReadExpressionSuffix { get; }
        public string EqualsMethodName { get; }
        public string CopyMethodName { get; }
        public string WriteMethodName { get; }
    }

    private readonly struct FieldSchema {
        public FieldSchema(ushort id, string type, string enumType = "") {
            Id = id;
            Type = type;
            EnumType = enumType;
        }

        public ushort Id { get; }
        public string Type { get; }
        public string EnumType { get; }
    }

    private readonly struct IdMaps {
        public IdMaps(Dictionary<string, FieldSchema> fields, Dictionary<string, Dictionary<string, ushort>> enums) {
            Fields = fields;
            Enums = enums;
        }

        public Dictionary<string, FieldSchema> Fields { get; }
        public Dictionary<string, Dictionary<string, ushort>> Enums { get; }
    }

    private readonly struct EnumTypeInfo {
        public EnumTypeInfo(string key, string fullyQualifiedTypeName, List<IFieldSymbol> memberSymbols) {
            Key = key;
            FullyQualifiedTypeName = fullyQualifiedTypeName;
            MemberSymbols = memberSymbols;
            MemberNames = new HashSet<string>(memberSymbols.Select(static m => m.Name), StringComparer.Ordinal);
        }

        public string Key { get; }
        public string FullyQualifiedTypeName { get; }
        public List<IFieldSymbol> MemberSymbols { get; }
        public HashSet<string> MemberNames { get; }
    }

    private struct ObservedField {
        public ObservedField(IFieldSymbol symbol, ITypeSymbol fieldType, string fieldName, string readExpressionSuffix) {
            Symbol = symbol;
            FieldType = fieldType;
            FieldName = fieldName;
            ReadExpressionSuffix = readExpressionSuffix;
            Kind = FieldKind.Unsupported;
            FieldIdName = string.Empty;
            PreviousName = string.Empty;
            CurrentName = string.Empty;
            PreviousValueGetterName = string.Empty;
            EnumTypeKey = string.Empty;
            EnumMapperName = string.Empty;
            ContainingFieldName = symbol.Name;
            ReadExpressionOverride = string.Empty;
        }

        public static ObservedField CreateSynthetic(ITypeSymbol fieldType, string fieldName, string containingFieldName, string readExpressionOverride) {
            return new ObservedField {
                Symbol = null!,
                FieldType = fieldType,
                FieldName = fieldName,
                ReadExpressionSuffix = string.Empty,
                Kind = FieldKind.Unsupported,
                FieldIdName = string.Empty,
                PreviousName = string.Empty,
                CurrentName = string.Empty,
                PreviousValueGetterName = string.Empty,
                EnumTypeKey = string.Empty,
                EnumMapperName = string.Empty,
                ContainingFieldName = containingFieldName,
                ReadExpressionOverride = readExpressionOverride,
            };
        }

        public IFieldSymbol Symbol;
        public ITypeSymbol FieldType;
        public string FieldName;
        public string ReadExpressionSuffix;
        public FieldKind Kind;
        public string FieldIdName;
        public string PreviousName;
        public string CurrentName;
        public string PreviousValueGetterName;
        public string EnumTypeKey;
        public string EnumMapperName;
        public string ContainingFieldName;
        public string ReadExpressionOverride;
    }
}



