using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Abstractions.Timing;

public interface ICompetitor
{
    public INumber<int> Position { get; }

    public IString Number { get; }

    public IString Name { get; }

    public IString KartModel { get; }

    public INumber<int> LapsCount { get; }

    public INumber<int> BestLapMilliseconds { get; }

    public INumber<int> LastLapMilliseconds { get; }

    public INumber<int> Gap { get; }

    public IBool GapIsTime { get; }

    public INumber<int> Difference { get; }

    public IBool DifferenceIsTime { get; }

    public INumber<int> PitsCount { get; }

    public IEnumerable<INumber<int>> LastSectors { get; }

    public IEnumerable<ILap> Laps { get; }
}
