using System.Text.Json;
using KartChrono.Abstractions.Timing;

namespace KartChrono.Protocol;

public sealed record LiveFeed : IFeed
{
    private static readonly string[] CompetitorGroups =
    [
        "results",
        "sessions",
        "pitlane",
    ];

    private readonly IAsyncEnumerable<ReadOnlyMemory<byte>> _frames;

    public LiveFeed(IAsyncEnumerable<ReadOnlyMemory<byte>> frames)
    {
        _frames = frames;
    }

    public async IAsyncEnumerator<ISessionSnapshot> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        Dictionary<string, Dictionary<string, string>> competitors = [];
        Dictionary<string, string> run = [];

        await foreach (
            ReadOnlyMemory<byte> frame in _frames.WithCancellation(cancellationToken)
        )
        {
            if (frame.Span.StartsWith("BINLAPS:"u8))
            {
                continue;
            }

            using JsonDocument document = JsonDocument.Parse(frame);

            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (
                document.RootElement.TryGetProperty("command", out JsonElement command)
                && (command.ValueKind == JsonValueKind.String)
                && (command.GetString() == "clear")
            )
            {
                competitors.Clear();
                run.Clear();
            }

            foreach (JsonProperty property in document.RootElement.EnumerateObject())
            {
                if (
                    CompetitorGroups.Contains(property.Name)
                    && (property.Value.ValueKind == JsonValueKind.Object)
                )
                {
                    foreach (JsonProperty entry in property.Value.EnumerateObject())
                    {
                        if (entry.Value.ValueKind != JsonValueKind.Object)
                        {
                            continue;
                        }

                        if (
                            !competitors.TryGetValue(
                                entry.Name,
                                out Dictionary<string, string>? state
                            )
                        )
                        {
                            state = [];
                            competitors[entry.Name] = state;
                        }

                        foreach (JsonProperty field in entry.Value.EnumerateObject())
                        {
                            state[field.Name] =
                                field.Value.ValueKind == JsonValueKind.String
                                    ? field.Value.GetString() ?? string.Empty
                                    : field.Value.GetRawText();
                        }
                    }
                }
                else if (
                    (property.Name == "removeCompetitors")
                    && (property.Value.ValueKind == JsonValueKind.Array)
                )
                {
                    foreach (JsonElement removed in property.Value.EnumerateArray())
                    {
                        _ = competitors.Remove(
                            removed.ValueKind == JsonValueKind.String
                                ? removed.GetString() ?? string.Empty
                                : removed.GetRawText()
                        );
                    }
                }
                else if (
                    (property.Name != "command")
                    && property.Value.ValueKind
                        is JsonValueKind.Number
                            or JsonValueKind.String
                )
                {
                    run[property.Name] =
                        property.Value.ValueKind == JsonValueKind.String
                            ? property.Value.GetString() ?? string.Empty
                            : property.Value.GetRawText();
                }
            }

            yield return new Snapshot(
                new RunState(run.ToDictionary()),
                [
                    .. competitors
                        .Values.Select(state => new CompetitorState(
                            state.ToDictionary(),
                            []
                        ))
                        .OrderBy(competitor => competitor.Position.NumberValue),
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
