using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Abstractions.Timing;

public interface IRunState
{
    public IString RunName { get; }

    public IString RaceName { get; }

    public IString TrackConfiguration { get; }

    public INumber<int> FlagStatus { get; }

    public INumber<long> ElapsedMilliseconds { get; }

    public INumber<long> RemainingMilliseconds { get; }

    public INumber<int> LapsToGo { get; }

    public INumber<int> BestLapMilliseconds { get; }

    public IString BestLapKartNumber { get; }
}
