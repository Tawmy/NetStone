using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetStone.Definitions;

/// <summary>
/// Represents the <c>type</c> field of a <see cref="DefinitionsPack" />. Accepts either a single type or a map from regex
/// capture group name to type.
/// </summary>
[JsonConverter(typeof(DefinitionTypeConverter))]
public sealed class DefinitionType
{
    private readonly string? _singleValue;
    private readonly IReadOnlyDictionary<string, string>? _multiValue;

    internal DefinitionType(string singleValue)
    {
        this._singleValue = singleValue;
    }

    internal DefinitionType(IReadOnlyDictionary<string, string> multiValue)
    {
        this._multiValue = multiValue;
    }

    /// <summary>
    /// Resolves the type for a given regex capture group.
    /// </summary>
    /// <param name="groupName">Name of the regex capture group, or <c>null</c>.</param>
    /// <returns>The type string, or <c>null</c> if no matching entry exists.</returns>
    public string? Get(string? groupName = null)
    {
        if (this._singleValue is not null)
            return this._singleValue;

        if (this._multiValue is not null && groupName is not null &&
            this._multiValue.TryGetValue(groupName, out var type))
            return type;

        return null;
    }
}

internal sealed class DefinitionTypeConverter : JsonConverter<DefinitionType?>
{
    public override DefinitionType? ReadJson(JsonReader reader, Type objectType,
        DefinitionType? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        return token.Type switch
        {
            JTokenType.Null => null,
            JTokenType.String => new DefinitionType((string)token!),
            JTokenType.Object => new DefinitionType(token.ToObject<Dictionary<string, string>>()!),
            _ => throw new JsonSerializationException(
                $"Unexpected token {token.Type} when parsing DefinitionsPack.Type."),
        };
    }
    
    public override void WriteJson(JsonWriter writer, DefinitionType? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
