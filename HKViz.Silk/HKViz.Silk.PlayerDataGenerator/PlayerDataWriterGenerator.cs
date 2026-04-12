using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
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

    private static readonly DiagnosticDescriptor AssignedNewIds = new(
        id: "HKVIZSILK002",
        title: "New PlayerData fields received IDs",
        messageFormat: "Assigned new PlayerData field ids: {0}. The sync target should persist these in PlayerDataFieldIds.json",
        category: "HKViz.Silk.Generator",
        defaultSeverity: DiagnosticSeverity.Info,
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

    private static readonly DiagnosticDescriptor AssignedNewEnumMemberIds = new(
        id: "HKVIZSILK004",
        title: "New enum member ids assigned",
        messageFormat: "Assigned new enum member ids: {0}. The sync target should persist these in PlayerDataFieldIds.json",
        category: "HKViz.Silk.Generator",
        defaultSeverity: DiagnosticSeverity.Info,
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

        List<ObservedField> allFields = playerDataType
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Where(static field => !field.IsStatic && !field.IsConst)
            .Select(field => new ObservedField(field, GetFieldKind(field)))
            .OrderBy(static field => field.Symbol.Name, StringComparer.Ordinal)
            .ToList();

        List<ObservedField> unsupportedFields = allFields
            .Where(static field => field.Kind == FieldKind.Unsupported)
            .ToList();

        foreach (ObservedField field in unsupportedFields) {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    UnsupportedPlayerDataField,
                    Location.None,
                    field.Symbol.Name,
                    field.Symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                )
            );
        }

        List<ObservedField> supportedFields = allFields
            .Where(static field => field.Kind != FieldKind.Unsupported)
            .ToList();

        ushort nextFieldId = fieldSchemaMap.Count == 0 ? (ushort)1 : (ushort)(fieldSchemaMap.Values.Max(static value => value.Id) + 1);
        List<string> newFieldAssignments = [];

        foreach (ObservedField field in supportedFields) {
            string expectedTypeName = GetTypeName(field.Symbol);
            if (fieldSchemaMap.TryGetValue(field.Symbol.Name, out FieldSchema existingSchema)) {
                string normalizedType = string.IsNullOrWhiteSpace(existingSchema.Type) ? expectedTypeName : existingSchema.Type;
                fieldSchemaMap[field.Symbol.Name] = new FieldSchema(existingSchema.Id, normalizedType);
                continue;
            }

            fieldSchemaMap[field.Symbol.Name] = new FieldSchema(nextFieldId, expectedTypeName);
            newFieldAssignments.Add($"{field.Symbol.Name}:{nextFieldId}");
            nextFieldId++;
        }

        if (newFieldAssignments.Count > 0) {
            context.ReportDiagnostic(Diagnostic.Create(AssignedNewIds, Location.None, string.Join(", ", newFieldAssignments)));
        }

        Dictionary<string, EnumTypeInfo> enumTypeInfos = BuildEnumTypeInfos(supportedFields);
        List<string> newEnumMemberAssignments = [];

        foreach (KeyValuePair<string, EnumTypeInfo> enumTypeEntry in enumTypeInfos.OrderBy(static kv => kv.Key, StringComparer.Ordinal)) {
            string enumTypeKey = enumTypeEntry.Key;
            EnumTypeInfo enumInfo = enumTypeEntry.Value;

            if (!enumMemberMapByType.TryGetValue(enumTypeKey, out Dictionary<string, ushort>? memberMap)) {
                memberMap = new Dictionary<string, ushort>(StringComparer.Ordinal);
                enumMemberMapByType[enumTypeKey] = memberMap;
            }

            ushort nextMemberId = memberMap.Count == 0 ? (ushort)1 : (ushort)(memberMap.Values.Max() + 1);
            foreach (string memberName in enumInfo.MemberNames.OrderBy(static name => name, StringComparer.Ordinal)) {
                if (memberMap.ContainsKey(memberName)) {
                    continue;
                }

                memberMap[memberName] = nextMemberId;
                newEnumMemberAssignments.Add($"{enumTypeKey}.{memberName}:{nextMemberId}");
                nextMemberId++;
            }
        }

        if (newEnumMemberAssignments.Count > 0) {
            context.ReportDiagnostic(Diagnostic.Create(AssignedNewEnumMemberIds, Location.None, string.Join(", ", newEnumMemberAssignments)));
        }

        HashSet<string> usedIdentifiers = new(StringComparer.Ordinal);
        Dictionary<string, string> enumMapperNamesByType = new(StringComparer.Ordinal);
        foreach (string enumTypeKey in enumTypeInfos.Keys.OrderBy(static key => key, StringComparer.Ordinal)) {
            enumMapperNamesByType[enumTypeKey] = MakeUniqueIdentifier("MapEnum_" + enumTypeKey, usedIdentifiers);
        }

        for (int i = 0; i < supportedFields.Count; i++) {
            ObservedField field = supportedFields[i];
            field.FieldIdName = MakeUniqueIdentifier("FieldId_" + field.Symbol.Name, usedIdentifiers);
            field.PreviousName = MakeUniqueIdentifier("_previous_" + field.Symbol.Name, usedIdentifiers);
            field.CurrentName = MakeUniqueIdentifier("current_" + field.Symbol.Name, usedIdentifiers);
            field.PreviousValueGetterName = MakeUniqueIdentifier("PreviousValue" + field.Symbol.Name, usedIdentifiers);

            if (field.Kind == FieldKind.EnumId && field.Symbol.Type is INamedTypeSymbol enumTypeSymbol) {
                string enumTypeKey = GetEnumTypeKey(enumTypeSymbol);
                field.EnumTypeKey = enumTypeKey;
                field.EnumMapperName = enumMapperNamesByType[enumTypeKey];
            }

            supportedFields[i] = field;
        }

        string playerDataTypeName = playerDataType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        StringBuilder source = new();
        source.AppendLine("// <auto-generated />");
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine("namespace HKViz.Silk.Recording;");
        source.AppendLine();
        source.AppendLine("public partial class PlayerDataWriter {");
        source.AppendLine("    public static class FieldIds {");

        foreach (ObservedField field in supportedFields) {
            source.Append("        public const ushort ");
            source.Append(field.FieldIdName);
            source.Append(" = ");
            source.Append(fieldSchemaMap[field.Symbol.Name].Id);
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
        source.AppendLine();

        foreach (ObservedField field in supportedFields) {
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
                    source.Append("        runFiles.WritePlayerDataIntArrayFullChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
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
                case FieldKind.Guid:
                    source.Append("        runFiles.WritePlayerDataGuidChange(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
                    break;
            }

            if (field.Kind == FieldKind.IntArray) {
                source.Append("        UpdateIntArraySnapshot(ref ");
                source.Append(field.PreviousName);
                source.Append(", ");
                source.Append(field.CurrentName);
                source.AppendLine(");");
            } else {
                source.Append("        ");
                source.Append(field.PreviousName);
                source.Append(" = ");
                source.Append(GetPreviousAssignmentExpression(field, field.CurrentName));
                source.AppendLine(";");
            }
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
        source.AppendLine();
        source.AppendLine("        if (!_hasPreviousPlayerDataValues) {");
        foreach (ObservedField field in supportedFields) {
            if (field.Kind == FieldKind.IntArray) {
                source.Append("            UpdateIntArraySnapshot(ref ");
                source.Append(field.PreviousName);
                source.Append(", ");
                source.Append(GetReadExpression(field, "playerData"));
                source.AppendLine(");");
            } else {
                source.Append("            ");
                source.Append(field.PreviousName);
                source.Append(" = ");
                source.Append(GetPreviousAssignmentExpression(field, GetReadExpression(field, "playerData")));
                source.AppendLine(";");
            }
        }
        source.AppendLine("            _hasPreviousPlayerDataValues = true;");
        source.AppendLine("            return;");
        source.AppendLine("        }");
        source.AppendLine();

        foreach (ObservedField field in supportedFields) {
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
                    source.Append("        WriteIntArrayIfChanged(FieldIds.");
                    source.Append(field.FieldIdName);
                    source.Append(", ref ");
                    source.Append(field.PreviousName);
                    source.Append(", ");
                    source.Append(field.CurrentName);
                    source.AppendLine(");");
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

            source.Append("        ");
            source.Append(field.PreviousName);
            source.Append(" = ");
            source.Append(GetPreviousAssignmentExpression(field, field.CurrentName));
            source.AppendLine(";");
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
        source.AppendLine("}");

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
                map[prop.Name] = new FieldSchema(legacyId, string.Empty);
                continue;
            }

            if (prop.Value.ValueKind != JsonValueKind.Object) {
                continue;
            }

            bool hasId = false;
            ushort id = 0;
            string type = string.Empty;

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
                }
            }

            if (!hasId) {
                continue;
            }

            map[prop.Name] = new FieldSchema(id, type);
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
            if (field.Symbol.Type is not INamedTypeSymbol enumTypeSymbol) {
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

        if (IsGuidByteArray(field.Symbol)) {
            return "global::System.Guid";
        }

        return field.Symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    private static string GetReadExpression(ObservedField field, string instanceName) {
        if (field.Kind == FieldKind.EnumId) {
            return field.EnumMapperName + "(" + instanceName + "." + field.Symbol.Name + ")";
        }

        if (IsGuidByteArray(field.Symbol)) {
            return "GuidFromBytesOrEmpty(" + instanceName + "." + field.Symbol.Name + ")";
        }

        return instanceName + "." + field.Symbol.Name;
    }

    private static string GetPreviousAssignmentExpression(ObservedField field, string valueExpression) {

        return valueExpression;
    }

    private static string GetTypeName(IFieldSymbol field) {
        if (IsGuidByteArray(field)) {
            return "guid";
        }

        if (field.Type.TypeKind == TypeKind.Enum) {
            return GetEnumTypeKey(field.Type);
        }

        return field.Type.SpecialType switch {
            SpecialType.System_Boolean => "bool",
            SpecialType.System_Byte => "byte",
            SpecialType.System_SByte => "sbyte",
            SpecialType.System_Int16 => "short",
            SpecialType.System_UInt16 => "ushort",
            SpecialType.System_Int32 => "int",
            SpecialType.System_UInt64 => "ulong",
            SpecialType.System_Single => "float",
            SpecialType.System_String => "string",
            _ when IsIntArray(field) => "int[]",
            _ when field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::UnityEngine.Vector2" => "vector2",
            _ when field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::UnityEngine.Vector3" => "vector3",
            _ when field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::System.Guid" => "guid",
            _ => field.Type.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
        };
    }

    private static FieldKind GetFieldKind(IFieldSymbol field) {
        if (IsGuidByteArray(field)) {
            return FieldKind.Guid;
        }

        ITypeSymbol symbol = field.Type;
        if (symbol.TypeKind == TypeKind.Enum) {
            return FieldKind.EnumId;
        }

        if (IsIntArray(field)) {
            return FieldKind.IntArray;
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

    private static bool IsGuidByteArray(IFieldSymbol field) {
        return field.Type is IArrayTypeSymbol arrayType
               && arrayType.ElementType.SpecialType == SpecialType.System_Byte
               && field.Name.EndsWith("Guid", StringComparison.Ordinal);
    }

    private static bool IsIntArray(IFieldSymbol field) {
        return field.Type is IArrayTypeSymbol arrayType
               && arrayType.ElementType.SpecialType == SpecialType.System_Int32;
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
    }

    private readonly struct FieldSchema {
        public FieldSchema(ushort id, string type) {
            Id = id;
            Type = type;
        }

        public ushort Id { get; }
        public string Type { get; }
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
        public ObservedField(IFieldSymbol symbol, FieldKind kind) {
            Symbol = symbol;
            Kind = kind;
            FieldIdName = string.Empty;
            PreviousName = string.Empty;
            CurrentName = string.Empty;
            PreviousValueGetterName = string.Empty;
            EnumTypeKey = string.Empty;
            EnumMapperName = string.Empty;
        }

        public IFieldSymbol Symbol;
        public FieldKind Kind;
        public string FieldIdName;
        public string PreviousName;
        public string CurrentName;
        public string PreviousValueGetterName;
        public string EnumTypeKey;
        public string EnumMapperName;
    }
}



