using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.String;
using Char = Pure.Primitives.Char.Char;

namespace KartChrono.Protocol;

public sealed record FieldText : IString
{
    private readonly IReadOnlyDictionary<string, string> _fields;

    public FieldText(IReadOnlyDictionary<string, string> fields, string key)
    {
        _fields = fields;
        Key = key;
    }

    private string Key { get; }

    public string TextValue =>
        _fields.TryGetValue(Key, out string? value) ? value : string.Empty;

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
