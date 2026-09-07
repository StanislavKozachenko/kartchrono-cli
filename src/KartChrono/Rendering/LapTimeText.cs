using System.Globalization;
using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;
using Char = Pure.Primitives.Char.Char;

namespace KartChrono.Rendering;

public sealed record LapTimeText : IString
{
    private const int MillisecondsPerMinute = 60_000;

    private const int MillisecondsPerSecond = 1_000;

    private readonly INumber<int> _milliseconds;

    public LapTimeText(INumber<int> milliseconds)
    {
        _milliseconds = milliseconds;
    }

    public string TextValue
    {
        get
        {
            int total = _milliseconds.NumberValue;

            if (total <= 0)
            {
                return string.Empty;
            }

            int minutes = total / MillisecondsPerMinute;
            int seconds = total % MillisecondsPerMinute / MillisecondsPerSecond;
            int fraction = total % MillisecondsPerSecond;

            return minutes > 0
                ? string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}:{1:00}.{2:000}",
                    minutes,
                    seconds,
                    fraction
                )
                : string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}.{1:000}",
                    seconds,
                    fraction
                );
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
