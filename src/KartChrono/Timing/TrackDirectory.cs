using System.Diagnostics.CodeAnalysis;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Timing;

[ExcludeFromCodeCoverage]
public sealed record TrackDirectory : IAsyncEnumerable<IString>
{
    private const string Url = "https://kartchrono.com/racer/app/gettrackslist.php";

    private readonly HttpClient _client;

    public TrackDirectory(HttpClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        using HttpResponseMessage response = await _client.PostAsync(
            Url,
            new FormUrlEncodedContent([]),
            cancellationToken
        );

        _ = response.EnsureSuccessStatusCode();

        yield return new String(
            await response.Content.ReadAsStringAsync(cancellationToken)
        );
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
