using System.Globalization;
using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;
using Char = Pure.Primitives.Char.Char;

namespace KartChrono.Rendering;

public sealed record ClockText : IString
{
    private readonly INumber<long> _milliseconds;

    public ClockText(INumber<long> milliseconds)
    {
        _milliseconds = milliseconds;
    }

    public string TextValue
    {
        get
        {
            TimeSpan span = TimeSpan.FromMilliseconds(
                Math.Max(0, _milliseconds.NumberValue)
            );

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:00}:{1:00}:{2:00}",
                (int)span.TotalHours,
                span.Minutes,
                span.Seconds
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
