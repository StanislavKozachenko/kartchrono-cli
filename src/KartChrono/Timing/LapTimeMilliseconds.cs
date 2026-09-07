using System.Globalization;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Timing;

public sealed record LapTimeMilliseconds : INumber<int>
{
    private const int MillisecondsPerMinute = 60_000;

    private readonly IString _text;

    public LapTimeMilliseconds(IString text)
    {
        _text = text;
    }

    public int NumberValue
    {
        get
        {
            string[] parts = _text.TextValue.Split(':');

            if (
                (parts.Length > 2)
                || !double.TryParse(
                    parts[^1],
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double seconds
                )
            )
            {
                return 0;
            }

            int minutes = 0;

            return
                (parts.Length == 2)
                && !int.TryParse(
                    parts[0],
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out minutes
                )
                ? 0
                : (minutes * MillisecondsPerMinute) + (int)Math.Round(seconds * 1000);
        }
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
