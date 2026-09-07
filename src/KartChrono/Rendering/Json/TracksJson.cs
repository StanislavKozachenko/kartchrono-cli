using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Rendering.Json;

public sealed record TracksJson : IOutput
{
    private readonly IAsyncEnumerable<ITrack> _tracks;

    public TracksJson(IAsyncEnumerable<ITrack> tracks)
    {
        _tracks = tracks;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (ITrack track in _tracks.WithCancellation(cancellationToken))
        {
            yield return new JsonLine(writer =>
            {
                writer.WriteString("id", track.Id.TextValue);
                writer.WriteString("slug", track.Slug.TextValue);
                writer.WriteString("name", track.Name.TextValue);
            });
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
