using KartChrono.Abstractions.Output;
using KartChrono.Protocol;
using KartChrono.Rendering;
using KartChrono.Tests.Fixtures;
using KartChrono.Timing;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Tests.Rendering;

public sealed record OutputTests
{
    [Fact]
    public async Task ListsTrackSlugAndName()
    {
        IReadOnlyList<string> lines = await Rendered(
            new TracksOutput(new Tracks(new Fixture("gettrackslist.html")))
        );

        Assert.Equal(54, lines.Count);
        Assert.Equal("mayak                    Маяк", lines[0]);
    }

    [Fact]
    public async Task RendersRunHeaderAndOneRowPerCompetitor()
    {
        IReadOnlyList<string> lines = await Rendered(
            new LeaderboardOutput(new LiveFeed(new Frames("snapshot.json")))
        );

        Assert.Equal(5, lines.Count);
        Assert.Contains("Заезд №3", lines[0]);
        Assert.Contains("1200 м по часовой", lines[0]);
        Assert.Contains("elapsed 00:05:34", lines[0]);
        Assert.Contains("POS", lines[1]);
        Assert.Contains("1:16.203", lines[2]);
    }

    [Fact]
    public async Task RendersLapsWithSectors()
    {
        IReadOnlyList<string> lines = await Rendered(
            new LapsOutput(
                new LiveFeed(new Frames("snapshot.json", "binlaps.bin", "delta.json"))
            )
        );

        Assert.Contains(
            lines,
            line => line.StartsWith("kart 1", StringComparison.Ordinal)
        );
        Assert.Contains(
            lines,
            line => line.Contains("1:16.203", StringComparison.Ordinal)
        );
        Assert.Contains(lines, line => line.Contains("18.545", StringComparison.Ordinal));
    }

    [Fact]
    public async Task RendersUsage()
    {
        IReadOnlyList<string> lines = await Rendered(new HelpOutput());

        Assert.Contains(
            lines,
            line => line.Contains("kartchrono tracks", StringComparison.Ordinal)
        );
        Assert.Contains(
            lines,
            line => line.Contains("--track <slug>", StringComparison.Ordinal)
        );
    }

    private static async Task<IReadOnlyList<string>> Rendered(IOutput output)
    {
        List<string> lines = [];

        await foreach (IString line in output)
        {
            lines.Add(line.TextValue);
        }

        return lines;
    }
}
