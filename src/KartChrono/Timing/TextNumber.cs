using System.Globalization;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Timing;

public sealed record TextNumber : INumber<int>
{
    private readonly IString _text;

    public TextNumber(IString text)
    {
        _text = text;
    }

    public int NumberValue =>
        int.TryParse(
            _text.TextValue,
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
