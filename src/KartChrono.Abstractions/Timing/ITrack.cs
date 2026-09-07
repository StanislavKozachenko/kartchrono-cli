using Pure.Primitives.Abstractions.String;

namespace KartChrono.Abstractions.Timing;

public interface ITrack
{
    public IString Id { get; }

    public IString Slug { get; }

    public IString Name { get; }
}
