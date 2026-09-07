using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Abstractions.Timing;

public interface IRecord
{
    public IString KartModel { get; }

    public INumber<int> Position { get; }

    public IString Name { get; }

    public INumber<int> Milliseconds { get; }
}
