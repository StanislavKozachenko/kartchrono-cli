using System.Globalization;
using Pure.Primitives.Abstractions.Number;

namespace KartChrono.Protocol;

public sealed record FieldNumber : INumber<int>
{
    private readonly IReadOnlyDictionary<string, string> _fields;

    private readonly string _key;

    public FieldNumber(IReadOnlyDictionary<string, string> fields, string key)
    {
        _fields = fields;
        _key = key;
    }

    public int NumberValue =>
        _fields.TryGetValue(_key, out string? value)
        && int.TryParse(
            value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int parsed
        )
            ? parsed
            : 0;

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
