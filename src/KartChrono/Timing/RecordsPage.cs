using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Timing;

[ExcludeFromCodeCoverage]
public sealed record RecordsPage : IAsyncEnumerable<IString>
{
    private static readonly CompositeFormat Url = CompositeFormat.Parse(
        "https://{0}.kartchrono.com/archive/records.php"
            + "?period={1}&maxCount=100&linesCount=100"
    );

    private readonly HttpClient _client;

    private readonly IString _slug;

    private readonly IString _period;

    public RecordsPage(HttpClient client, IString slug, IString period)
    {
        _client = client;
        _slug = slug;
        _period = period;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        yield return new String(
            await _client.GetStringAsync(
                new Uri(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Url,
                        _slug.TextValue,
                        _period.TextValue
                    )
                ),
                cancellationToken
            )
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
