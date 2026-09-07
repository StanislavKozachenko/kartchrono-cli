using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using KartChrono.Abstractions.Timing;
using KartChrono.Protocol;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Timing;

[ExcludeFromCodeCoverage]
public sealed record TrackFeed : IFeed
{
    private readonly IAsyncEnumerable<ITrack> _tracks;

    private readonly IString _slug;

    public TrackFeed(IAsyncEnumerable<ITrack> tracks, IString slug)
    {
        _tracks = tracks;
        _slug = slug;
    }

    public async IAsyncEnumerator<ISessionSnapshot> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        IString? id = null;

        await foreach (
            ITrack track in new TracksWithSlug(_tracks, _slug).WithCancellation(
                cancellationToken
            )
        )
        {
            id = track.Id;
            break;
        }

        if (id is null)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Unknown track '{0}'. Run 'kartchrono tracks' to list them.",
                    _slug.TextValue
                )
            );
        }

        IFeed feed = new LiveFeed(new WebSocketFrames(id));

        await foreach (
            ISessionSnapshot snapshot in feed.WithCancellation(cancellationToken)
        )
        {
            yield return snapshot;
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
