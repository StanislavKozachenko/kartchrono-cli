using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Protocol;

public sealed record RunState : IRunState
{
    private const int MillisecondsPerSecond = 1000;

    private readonly IReadOnlyDictionary<string, string> _fields;

    public RunState(IReadOnlyDictionary<string, string> fields)
    {
        _fields = fields;
    }

    public IString RunName => new FieldText(_fields, Field.RunName);

    public IString RaceName => new FieldText(_fields, Field.RaceName);

    public IString TrackConfiguration =>
        new FieldText(_fields, Field.TrackConfigurationName);

    public INumber<int> FlagStatus => new FieldNumber(_fields, Field.FlagStatus);

    public INumber<long> ElapsedMilliseconds =>
        new ScaledNumber(
            new FieldNumber(_fields, Field.RaceTimeSeconds),
            MillisecondsPerSecond
        );

    public INumber<long> RemainingMilliseconds =>
        new ScaledNumber(
            new FieldNumber(_fields, Field.TimeToGoSeconds),
            MillisecondsPerSecond
        );

    public INumber<int> LapsToGo => new FieldNumber(_fields, Field.LapsToGo);

    public INumber<int> BestLapMilliseconds =>
        new FieldNumber(_fields, Field.BestLapTimeOfRun);

    public IString BestLapKartNumber => new FieldText(_fields, Field.BestLapNumber);

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
