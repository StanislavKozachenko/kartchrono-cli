using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Rendering.Json;

public sealed record SessionJson : IOutput
{
    private readonly IAsyncEnumerable<ISessionSnapshot> _snapshots;

    public SessionJson(IAsyncEnumerable<ISessionSnapshot> snapshots)
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
            yield return new JsonLine(writer =>
            {
                writer.WriteString("run", snapshot.Run.RunName.TextValue);
                writer.WriteString("race", snapshot.Run.RaceName.TextValue);
                writer.WriteString(
                    "configuration",
                    snapshot.Run.TrackConfiguration.TextValue
                );
                writer.WriteNumber("flag", snapshot.Run.FlagStatus.NumberValue);
                writer.WriteNumber(
                    "elapsedMilliseconds",
                    snapshot.Run.ElapsedMilliseconds.NumberValue
                );
                writer.WriteNumber(
                    "remainingMilliseconds",
                    snapshot.Run.RemainingMilliseconds.NumberValue
                );
                writer.WriteStartArray("competitors");

                foreach (ICompetitor competitor in snapshot.Competitors)
                {
                    writer.WriteStartObject();
                    writer.WriteNumber("position", competitor.Position.NumberValue);
                    writer.WriteString("kart", competitor.Number.TextValue);
                    writer.WriteString("name", competitor.Name.TextValue);
                    writer.WriteString("kartModel", competitor.KartModel.TextValue);
                    writer.WriteNumber("laps", competitor.LapsCount.NumberValue);
                    writer.WriteNumber("pits", competitor.PitsCount.NumberValue);
                    writer.WriteNumber(
                        "bestLapMilliseconds",
                        competitor.BestLapMilliseconds.NumberValue
                    );
                    writer.WriteNumber(
                        "lastLapMilliseconds",
                        competitor.LastLapMilliseconds.NumberValue
                    );
                    writer.WriteStartArray("lastSectors");

                    foreach (INumber<int> sector in competitor.LastSectors)
                    {
                        writer.WriteNumberValue(sector.NumberValue);
                    }

                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
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
