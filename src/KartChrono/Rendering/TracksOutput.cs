using System.Globalization;
using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Rendering;

public sealed record TracksOutput : IOutput
{
    private readonly IAsyncEnumerable<ITrack> _tracks;

    public TracksOutput(IAsyncEnumerable<ITrack> tracks)
    {
        _tracks = tracks;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (ITrack track in _tracks.WithCancellation(cancellationToken))
        {
            yield return new String(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-24} {1}",
                    track.Slug.TextValue,
                    track.Name.TextValue
                )
            );
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
