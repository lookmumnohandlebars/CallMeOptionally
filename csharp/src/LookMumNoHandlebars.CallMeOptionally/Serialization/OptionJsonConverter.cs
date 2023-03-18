using System.Text.Json;
using System.Text.Json.Serialization;

namespace LookMumNoHandlebars.CallMeOptionally.Serialization;

/// <summary>
///     System.Text.Json Serialization Converter.
/// </summary>
/// <code>
///     asd
/// </code>
public class OptionJsonConverter : JsonConverter<Option<object>>
{
    public override Option<object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Option<object> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}