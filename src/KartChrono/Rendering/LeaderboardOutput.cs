using System.Globalization;
using System.Text;
using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Rendering;

public sealed record LeaderboardOutput : IOutput
{
    private static readonly CompositeFormat Row = CompositeFormat.Parse(
        "{0,3}  {1,-6} {2,-20} {3,10} {4,10} {5,5} {6,4}"
    );

    private readonly IAsyncEnumerable<ISessionSnapshot> _snapshots;

    public LeaderboardOutput(IAsyncEnumerable<ISessionSnapshot> snapshots)
    {
        _snapshots = snapshots;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (
            ISessionSnapshot snapshot in _snapshots.WithCancellation(cancellationToken)
        )
        {
            yield return new String(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}  {1}  elapsed {2}  remaining {3}",
                    snapshot.Run.RunName.TextValue,
                    snapshot.Run.TrackConfiguration.TextValue,
                    new ClockText(snapshot.Run.ElapsedMilliseconds).TextValue,
                    new ClockText(snapshot.Run.RemainingMilliseconds).TextValue
                )
            );

            yield return new String(
                string.Format(
                    CultureInfo.InvariantCulture,
                    Row,
                    "POS",
                    "KART",
                    "NAME",
                    "BEST",
                    "LAST",
                    "LAPS",
                    "PIT"
                )
            );

            foreach (ICompetitor competitor in snapshot.Competitors)
            {
                yield return new String(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Row,
                        competitor.Position.NumberValue,
                        competitor.Number.TextValue,
                        competitor.Name.TextValue,
                        new LapTimeText(competitor.BestLapMilliseconds).TextValue,
                        new LapTimeText(competitor.LastLapMilliseconds).TextValue,
                        competitor.LapsCount.NumberValue,
                        competitor.PitsCount.NumberValue
                    )
                );
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
