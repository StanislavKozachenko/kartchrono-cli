using System.Globalization;
using System.Text;
using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Rendering;

public sealed record LapsOutput : IOutput
{
    private static readonly CompositeFormat Row = CompositeFormat.Parse(
        "{0,4}  {1,10}  {2,3}  {3}"
    );

    private readonly IAsyncEnumerable<ISessionSnapshot> _snapshots;

    public LapsOutput(IAsyncEnumerable<ISessionSnapshot> snapshots)
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
            foreach (ICompetitor competitor in snapshot.Competitors)
            {
                yield return new String(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "kart {0}  {1}",
                        competitor.Number.TextValue,
                        competitor.Name.TextValue
                    )
                );

                yield return new String(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Row,
                        "LAP",
                        "TIME",
                        "POS",
                        "SECTORS"
                    )
                );

                foreach (ILap lap in competitor.Laps)
                {
                    yield return new String(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            Row,
                            lap.Number.NumberValue,
                            new LapTimeText(lap.Milliseconds).TextValue,
                            lap.Position.NumberValue,
                            string.Join(
                                "  ",
                                lap.Sectors.Select(sector =>
                                    new LapTimeText(sector).TextValue
                                )
                            )
                        )
                    );
                }
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
