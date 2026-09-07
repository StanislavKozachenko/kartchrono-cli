using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Timing;

public sealed record TracksWithSlug : IAsyncEnumerable<ITrack>
{
    private readonly IAsyncEnumerable<ITrack> _tracks;

    private readonly IString _slug;

    public TracksWithSlug(IAsyncEnumerable<ITrack> tracks, IString slug)
    {
        _tracks = tracks;
        _slug = slug;
    }

    public async IAsyncEnumerator<ITrack> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (ITrack track in _tracks.WithCancellation(cancellationToken))
        {
            if (
                string.Equals(
                    track.Slug.TextValue,
                    _slug.TextValue,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                yield return track;
            }
        }
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
