using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.DateTime;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Protocol;

public sealed record Lap : ILap
{
    public Lap(
        IString competitorId,
        INumber<int> number,
        INumber<int> position,
        INumber<int> milliseconds,
        IEnumerable<INumber<int>> sectors,
        IDateTime timestamp,
        INumber<int> flags
    )
    {
        CompetitorId = competitorId;
        Number = number;
        Position = position;
        Milliseconds = milliseconds;
        Sectors = sectors;
        Timestamp = timestamp;
        Flags = flags;
    }

    public IString CompetitorId { get; }

    public INumber<int> Number { get; }

    public INumber<int> Position { get; }

    public INumber<int> Milliseconds { get; }

    public IEnumerable<INumber<int>> Sectors { get; }

    public IDateTime Timestamp { get; }

    public INumber<int> Flags { get; }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
