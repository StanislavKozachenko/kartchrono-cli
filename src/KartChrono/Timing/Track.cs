using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Timing;

public sealed record Track : ITrack
{
    public Track(IString id, IString slug, IString name)
    {
        Id = id;
        Slug = slug;
        Name = name;
    }

    public IString Id { get; }

    public IString Slug { get; }

    public IString Name { get; }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
