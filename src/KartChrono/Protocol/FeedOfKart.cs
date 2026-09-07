using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Protocol;

public sealed record FeedOfKart : IFeed
{
    private readonly IFeed _feed;

    private readonly IString _number;

    public FeedOfKart(IFeed feed, IString number)
    {
        _feed = feed;
        _number = number;
    }

    public async IAsyncEnumerator<ISessionSnapshot> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (
            ISessionSnapshot snapshot in _feed.WithCancellation(cancellationToken)
        )
        {
            yield return _number.TextValue.Length == 0
                ? snapshot
                : new Snapshot(
                    snapshot.Run,
                    [
                        .. snapshot.Competitors.Where(competitor =>
                            string.Equals(
                                competitor.Number.TextValue,
                                _number.TextValue,
                                StringComparison.OrdinalIgnoreCase
                            )
                        ),
                    ]
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
