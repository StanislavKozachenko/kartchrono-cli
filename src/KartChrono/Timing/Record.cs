using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Timing;

public sealed record Record : IRecord
{
    public Record(
        IString kartModel,
        INumber<int> position,
        IString name,
        INumber<int> milliseconds
    )
    {
        KartModel = kartModel;
        Position = position;
        Name = name;
        Milliseconds = milliseconds;
    }

    public IString KartModel { get; }

    public INumber<int> Position { get; }

    public IString Name { get; }

    public INumber<int> Milliseconds { get; }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
