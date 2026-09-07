using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.String;
using Char = Pure.Primitives.Char.Char;

namespace KartChrono.Rendering.Json;

public sealed record JsonLine : IString
{
    private static readonly JsonWriterOptions Options = new JsonWriterOptions
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Indented = false,
    };

    private readonly Action<Utf8JsonWriter> _content;

    public JsonLine(Action<Utf8JsonWriter> content)
    {
        _content = content;
    }

    public string TextValue
    {
        get
        {
            using MemoryStream buffer = new MemoryStream();

            using (Utf8JsonWriter writer = new Utf8JsonWriter(buffer, Options))
            {
                writer.WriteStartObject();
                _content(writer);
                writer.WriteEndObject();
            }

            return Encoding.UTF8.GetString(buffer.ToArray());
        }
    }

    public IEnumerator<IChar> GetEnumerator()
    {
        return TextValue.Select(symbol => new Char(symbol)).Cast<IChar>().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
