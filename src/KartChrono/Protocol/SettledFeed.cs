using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.Number;

namespace KartChrono.Protocol;

public sealed record SettledFeed : IFeed
{
    private readonly IFeed _feed;

    private readonly INumber<int> _snapshots;

    public SettledFeed(IFeed feed, INumber<int> snapshots)
    {
        _feed = feed;
        _snapshots = snapshots;
    }

    public async IAsyncEnumerator<ISessionSnapshot> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        int remaining = _snapshots.NumberValue;
        ISessionSnapshot? settled = null;

        if (remaining > 0)
        {
            await foreach (
                ISessionSnapshot snapshot in _feed.WithCancellation(cancellationToken)
            )
            {
                settled = snapshot;
                remaining -= 1;

                if (remaining <= 0)
                {
                    break;
                }
            }
        }

        if (settled is not null)
        {
            yield return settled;
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
