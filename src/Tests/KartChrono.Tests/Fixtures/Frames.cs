namespace KartChrono.Tests.Fixtures;

public sealed record Frames : IAsyncEnumerable<ReadOnlyMemory<byte>>
{
    private readonly IEnumerable<string> _sources;

    public Frames(params IEnumerable<string> sources)
    {
        _sources = sources;
    }

    public async IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        foreach (string source in _sources)
        {
            yield return File.Exists(Path.Combine("Fixtures", source))
                ? await File.ReadAllBytesAsync(
                    Path.Combine("Fixtures", source),
                    cancellationToken
                )
                : System.Text.Encoding.UTF8.GetBytes(source);
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
