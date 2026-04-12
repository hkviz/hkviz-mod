using System.Text.Json;
using Mono.Cecil;

if (args.Length != 3) {
    Console.Error.WriteLine("Usage: <assembly-paths-separated-by-semicolon> <type-name> <field-ids-json-path>");
    return 1;
}

string assemblyPathList = args[0];
string typeName = args[1];
string jsonPath = args[2];

string[] assemblyPaths = assemblyPathList
    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Where(File.Exists)
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();

if (assemblyPaths.Length == 0) {
    Console.Error.WriteLine("No valid assembly paths were provided.");
    return 1;
}

TypeDefinition? playerDataType = null;
string sourceAssembly = string.Empty;
foreach (string assemblyPath in assemblyPaths) {
    using AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
    playerDataType = assembly.MainModule.Types.FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);
    if (playerDataType != null) {
        sourceAssembly = assemblyPath;
        break;
    }
}

if (playerDataType == null) {
    Console.Error.WriteLine($"Type '{typeName}' not found in provided references.");
    return 1;
}

IdMaps idMaps = ReadIdMap(jsonPath);
Dictionary<string, FieldSchema> fieldMap = idMaps.Fields;
Dictionary<string, Dictionary<string, ushort>> enumMap = idMaps.Enums;

ushort nextFieldId = fieldMap.Count == 0 ? (ushort)1 : (ushort)(fieldMap.Values.Max(static v => v.Id) + 1);

List<FieldDefinition> supportedFields = playerDataType
    .Fields
    .Where(static f => f.IsPublic && !f.IsStatic)
    .Where(static f => IsSupported(f.FieldType, f.Name))
    .OrderBy(static f => f.Name, StringComparer.Ordinal)
    .ToList();

List<string> enumValidationErrors = [];
Dictionary<string, HashSet<string>> observedEnumMembersByType = new(StringComparer.Ordinal);

foreach (FieldDefinition field in supportedFields) {
    string expectedTypeName = GetTypeName(field.FieldType, field.Name);

    if (fieldMap.TryGetValue(field.Name, out FieldSchema existingField)) {
        string normalizedType = string.IsNullOrWhiteSpace(existingField.Type) ? expectedTypeName : existingField.Type;
        fieldMap[field.Name] = new FieldSchema(existingField.Id, normalizedType);
    } else {
        fieldMap[field.Name] = new FieldSchema(nextFieldId, expectedTypeName);
        nextFieldId++;
    }

    if (!TryResolveEnumType(field.FieldType, out TypeDefinition? enumType) || enumType == null) {
        continue;
    }

    string enumTypeKey = ToEnumTypeKey(enumType);
    if (!observedEnumMembersByType.TryGetValue(enumTypeKey, out HashSet<string>? members)) {
        members = new HashSet<string>(StringComparer.Ordinal);
        observedEnumMembersByType[enumTypeKey] = members;
    }

    foreach (FieldDefinition enumField in enumType.Fields.Where(static ef => ef.IsStatic && ef.HasConstant && ef.Name != "value__")) {
        members.Add(enumField.Name);
        if (!TryFitsIntoByte(enumField.Constant, out string valueText)) {
            enumValidationErrors.Add($"Enum '{enumTypeKey}' member '{enumField.Name}' has value '{valueText}', expected range 0..255.");
        }
    }
}

if (enumValidationErrors.Count > 0) {
    foreach (string error in enumValidationErrors.OrderBy(static e => e, StringComparer.Ordinal)) {
        Console.Error.WriteLine(error);
    }

    return 1;
}

foreach ((string enumTypeKey, HashSet<string> observedMembers) in observedEnumMembersByType.OrderBy(static kv => kv.Key, StringComparer.Ordinal)) {
    if (!enumMap.TryGetValue(enumTypeKey, out Dictionary<string, ushort>? memberMap)) {
        memberMap = new Dictionary<string, ushort>(StringComparer.Ordinal);
        enumMap[enumTypeKey] = memberMap;
    }

    ushort nextMemberId = memberMap.Count == 0 ? (ushort)1 : (ushort)(memberMap.Values.Max() + 1);
    foreach (string memberName in observedMembers.OrderBy(static name => name, StringComparer.Ordinal)) {
        if (memberMap.ContainsKey(memberName)) {
            continue;
        }

        memberMap[memberName] = nextMemberId;
        nextMemberId++;
    }
}

List<KeyValuePair<string, FieldSchema>> orderedFields = fieldMap
    .OrderBy(static kv => kv.Value.Id)
    .ThenBy(static kv => kv.Key, StringComparer.Ordinal)
    .ToList();

Dictionary<string, object> outputFields = new(StringComparer.Ordinal);
foreach ((string key, FieldSchema value) in orderedFields) {
    outputFields[key] = new Dictionary<string, object>(StringComparer.Ordinal) {
        ["id"] = value.Id,
        ["type"] = value.Type,
    };
}

Dictionary<string, object> outputEnums = new(StringComparer.Ordinal);
foreach ((string enumTypeKey, Dictionary<string, ushort> memberMap) in enumMap.OrderBy(static kv => kv.Key, StringComparer.Ordinal)) {
    Dictionary<string, ushort> orderedMembers = new(StringComparer.Ordinal);
    foreach ((string memberName, ushort memberId) in memberMap
                 .OrderBy(static kv => kv.Value)
                 .ThenBy(static kv => kv.Key, StringComparer.Ordinal)) {
        orderedMembers[memberName] = memberId;
    }

    outputEnums[enumTypeKey] = orderedMembers;
}

Directory.CreateDirectory(Path.GetDirectoryName(jsonPath) ?? ".");
var outputRoot = new Dictionary<string, object>(StringComparer.Ordinal) {
    ["fields"] = outputFields,
    ["enums"] = outputEnums,
};

string json = JsonSerializer.Serialize(outputRoot, new JsonSerializerOptions {
    WriteIndented = true,
});
File.WriteAllText(jsonPath, json + Environment.NewLine);

int enumMemberCount = observedEnumMembersByType.Values.Sum(static members => members.Count);
Console.WriteLine($"Synced {supportedFields.Count} PlayerData fields and {enumMemberCount} enum members from {sourceAssembly} into {jsonPath}");
return 0;

static IdMaps ReadIdMap(string path) {
    if (!File.Exists(path)) {
        return new IdMaps(
            new Dictionary<string, FieldSchema>(StringComparer.Ordinal),
            new Dictionary<string, Dictionary<string, ushort>>(StringComparer.Ordinal)
        );
    }

    try {
        string text = File.ReadAllText(path);
        using JsonDocument doc = JsonDocument.Parse(text);
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

        Dictionary<string, ushort>? legacy = JsonSerializer.Deserialize<Dictionary<string, ushort>>(text);
        Dictionary<string, FieldSchema> legacyFields = new(StringComparer.Ordinal);
        if (legacy != null) {
            foreach ((string key, ushort id) in legacy) {
                legacyFields[key] = new FieldSchema(id, string.Empty);
            }
        }

        return new IdMaps(
            legacyFields,
            new Dictionary<string, Dictionary<string, ushort>>(StringComparer.Ordinal)
        );
    } catch {
        return new IdMaps(
            new Dictionary<string, FieldSchema>(StringComparer.Ordinal),
            new Dictionary<string, Dictionary<string, ushort>>(StringComparer.Ordinal)
        );
    }
}

static Dictionary<string, FieldSchema> ReadFieldMapObject(JsonElement element) {
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

        ushort id = 0;
        bool hasId = false;
        string type = string.Empty;

        foreach (JsonProperty child in prop.Value.EnumerateObject()) {
            if (string.Equals(child.Name, "id", StringComparison.OrdinalIgnoreCase)
                && child.Value.ValueKind == JsonValueKind.Number
                && child.Value.TryGetUInt16(out ushort parsedId)) {
                id = parsedId;
                hasId = true;
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

static Dictionary<string, Dictionary<string, ushort>> ReadEnumMapObject(JsonElement element) {
    Dictionary<string, Dictionary<string, ushort>> map = new(StringComparer.Ordinal);
    if (element.ValueKind != JsonValueKind.Object) {
        return map;
    }

    foreach (JsonProperty enumType in element.EnumerateObject()) {
        if (enumType.Value.ValueKind != JsonValueKind.Object) {
            // Legacy enum type id format (number) is intentionally ignored.
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

static bool TryGetPropertyIgnoreCase(JsonElement root, string propertyName, out JsonElement value) {
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

static bool IsSupported(TypeReference type, string fieldName) {
    string fullName = type.FullName;
    if (fullName is "System.Boolean"
        or "System.Byte"
        or "System.SByte"
        or "System.Int16"
        or "System.UInt16"
        or "System.Int32"
        or "System.UInt64"
        or "System.Single"
        or "UnityEngine.Vector2"
        or "UnityEngine.Vector3"
        or "System.String"
        or "System.Guid") {
        return true;
    }

    if (fullName == "System.Int32[]") {
        return true;
    }

    if (IsListOf(type, "System.Int32") || IsListOf(type, "System.String")) {
        return true;
    }

    if (IsSetOf(type, "System.String")) {
        return true;
    }

    if (fullName == "System.Byte[]" && fieldName.EndsWith("Guid", StringComparison.Ordinal)) {
        return true;
    }

    try {
        TypeDefinition? resolved = type.Resolve();
        return resolved?.IsEnum == true;
    } catch {
        return false;
    }
}

static bool TryResolveEnumType(TypeReference type, out TypeDefinition? enumType) {
    try {
        TypeDefinition? resolved = type.Resolve();
        if (resolved?.IsEnum == true) {
            enumType = resolved;
            return true;
        }
    } catch {
        // ignored intentionally
    }

    enumType = null;
    return false;
}

static string ToEnumTypeKey(TypeDefinition enumType) {
    return enumType.FullName.Replace('/', '.');
}

static string GetTypeName(TypeReference type, string fieldName) {
    if (type.FullName == "System.Byte[]" && fieldName.EndsWith("Guid", StringComparison.Ordinal)) {
        return "guid";
    }

    if (TryResolveEnumType(type, out TypeDefinition? enumType) && enumType != null) {
        return ToEnumTypeKey(enumType);
    }

    return type.FullName switch {
        "System.Boolean" => "bool",
        "System.Byte" => "byte",
        "System.SByte" => "sbyte",
        "System.Int16" => "short",
        "System.UInt16" => "ushort",
        "System.Int32" => "int",
        "System.UInt64" => "ulong",
        "System.Single" => "float",
        "UnityEngine.Vector2" => "vector2",
        "UnityEngine.Vector3" => "vector3",
        "System.Int32[]" => "int[]",
        "System.String" => "string",
        "System.Guid" => "guid",
        _ when IsListOf(type, "System.Int32") => "list<int>",
        _ when IsListOf(type, "System.String") => "list<string>",
        _ when IsSetOf(type, "System.String") => "hashset<string>",
        _ => type.FullName,
    };
}

static bool IsListOf(TypeReference type, string elementFullName) {
    if (type is not GenericInstanceType { ElementType.FullName: "System.Collections.Generic.List`1" } genericType) {
        return false;
    }

    return genericType.GenericArguments.Count == 1
           && genericType.GenericArguments[0].FullName == elementFullName;
}

static bool IsSetOf(TypeReference type, string elementFullName) {
    if (type is not GenericInstanceType { ElementType.FullName: "System.Collections.Generic.HashSet`1" } genericType) {
        return false;
    }

    return genericType.GenericArguments.Count == 1
           && genericType.GenericArguments[0].FullName == elementFullName;
}

static bool TryFitsIntoByte(object? value, out string valueText) {
    valueText = value?.ToString() ?? "<null>";
    return value switch {
        byte => true,
        sbyte v => v >= 0,
        short v => v is >= byte.MinValue and <= byte.MaxValue,
        ushort v => v <= byte.MaxValue,
        int v => v is >= byte.MinValue and <= byte.MaxValue,
        uint v => v <= byte.MaxValue,
        long v => v is >= byte.MinValue and <= byte.MaxValue,
        ulong v => v <= byte.MaxValue,
        _ => false,
    };
}

readonly struct FieldSchema {
    public FieldSchema(ushort id, string type) {
        Id = id;
        Type = type;
    }

    public ushort Id { get; }
    public string Type { get; }
}

readonly struct IdMaps {
    public IdMaps(Dictionary<string, FieldSchema> fields, Dictionary<string, Dictionary<string, ushort>> enums) {
        Fields = fields;
        Enums = enums;
    }

    public Dictionary<string, FieldSchema> Fields { get; }
    public Dictionary<string, Dictionary<string, ushort>> Enums { get; }
}
