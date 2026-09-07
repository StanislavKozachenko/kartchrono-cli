using Pure.Primitives.Abstractions.DateTime;
using Pure.Primitives.Abstractions.Number;

namespace KartChrono.Abstractions.Timing;

public interface ILap
{
    public INumber<int> Number { get; }

    public INumber<int> Position { get; }

    public INumber<int> Milliseconds { get; }

    public IEnumerable<INumber<int>> Sectors { get; }

    public IDateTime Timestamp { get; }

    public INumber<int> Flags { get; }
}
