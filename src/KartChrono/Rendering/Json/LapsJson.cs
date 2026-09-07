using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Rendering.Json;

public sealed record LapsJson : IOutput
{
    private readonly IAsyncEnumerable<ISessionSnapshot> _snapshots;

    public LapsJson(IAsyncEnumerable<ISessionSnapshot> snapshots)
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
                foreach (ILap lap in competitor.Laps)
                {
                    yield return new JsonLine(writer =>
                    {
                        writer.WriteString("kart", competitor.Number.TextValue);
                        writer.WriteString("name", competitor.Name.TextValue);
                        writer.WriteNumber("lap", lap.Number.NumberValue);
                        writer.WriteNumber("position", lap.Position.NumberValue);
                        writer.WriteNumber("milliseconds", lap.Milliseconds.NumberValue);
                        writer.WriteNumber("flags", lap.Flags.NumberValue);
                        writer.WriteStartArray("sectors");

                        foreach (INumber<int> sector in lap.Sectors)
                        {
                            writer.WriteNumberValue(sector.NumberValue);
                        }

                        writer.WriteEndArray();
                    });
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
