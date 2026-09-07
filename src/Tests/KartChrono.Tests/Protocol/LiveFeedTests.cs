using KartChrono.Abstractions.Timing;
using KartChrono.Protocol;
using KartChrono.Tests.Fixtures;

namespace KartChrono.Tests.Protocol;

public sealed record LiveFeedTests
{
    [Fact]
    public async Task TakesEveryCompetitorFromSnapshot()
    {
        ISessionSnapshot snapshot = await Last(new Frames("snapshot.json"));

        Assert.Equal(3, snapshot.Competitors.Count());
    }

    [Fact]
    public async Task OrdersCompetitorsByPosition()
    {
        ISessionSnapshot snapshot = await Last(new Frames("snapshot.json"));

        Assert.Equal(
            ["1", "Staff", "25"],
            snapshot.Competitors.Select(competitor => competitor.Number.TextValue)
        );
    }

    [Fact]
    public async Task TakesRunStateFromSnapshot()
    {
        IRunState run = (await Last(new Frames("snapshot.json"))).Run;

        Assert.Equal("Заезд №3", run.RunName.TextValue);
        Assert.Equal("1200 м по часовой", run.TrackConfiguration.TextValue);
        Assert.Equal(0, run.FlagStatus.NumberValue);
        Assert.Equal(76203, run.BestLapMilliseconds.NumberValue);
        Assert.Equal("1", run.BestLapKartNumber.TextValue);
    }

    [Fact]
    public async Task TakesSecondsFieldsAsMilliseconds()
    {
        IRunState run = (await Last(new Frames("snapshot.json"))).Run;

        Assert.Equal(334_000, run.ElapsedMilliseconds.NumberValue);
    }

    [Fact]
    public async Task TakesCompetitorTimings()
    {
        ISessionSnapshot snapshot = await Last(new Frames("snapshot.json"));

        ICompetitor leader = snapshot.Competitors.First();

        Assert.Equal(1, leader.Position.NumberValue);
        Assert.Equal(3, leader.LapsCount.NumberValue);
        Assert.Equal(76203, leader.BestLapMilliseconds.NumberValue);
        Assert.Equal("Клуб", leader.KartModel.TextValue);
        Assert.Equal(
            [18545, 17539, 13603, 0, 0],
            leader.LastSectors.Select(sector => sector.NumberValue)
        );
    }

    [Fact]
    public async Task FoldsDeltaOntoRetainedState()
    {
        ISessionSnapshot snapshot = await Last(new Frames("snapshot.json", "delta.json"));

        Assert.Equal(3, snapshot.Competitors.Count());
        Assert.Equal(335_000, snapshot.Run.ElapsedMilliseconds.NumberValue);
        Assert.Equal(564_000, snapshot.Run.RemainingMilliseconds.NumberValue);
        Assert.Equal("Заезд №3", snapshot.Run.RunName.TextValue);
    }

    [Fact]
    public async Task ResetsStateOnClearCommand()
    {
        ISessionSnapshot snapshot = await Last(new Frames("snapshot.json", "clear.json"));

        Assert.Empty(snapshot.Competitors);
        Assert.Equal("", snapshot.Run.RunName.TextValue);
    }

    [Fact]
    public async Task RemovesCompetitorsOnRequest()
    {
        ISessionSnapshot snapshot = await Last(
            new Frames("snapshot.json", "remove-competitor.json")
        );

        Assert.Equal(
            ["1", "Staff"],
            snapshot.Competitors.Select(competitor => competitor.Number.TextValue)
        );
    }

    [Fact]
    public async Task SkipsBinaryAndUnusableFrames()
    {
        List<ISessionSnapshot> snapshots = [];

        IFeed feed = new LiveFeed(new Frames("binlaps.bin", "[1,2,3]", "snapshot.json"));

        await foreach (ISessionSnapshot snapshot in feed)
        {
            snapshots.Add(snapshot);
        }

        Assert.Equal(3, Assert.Single(snapshots).Competitors.Count());
    }

    [Fact]
    public async Task TakesOneSnapshotPerTextFrame()
    {
        List<ISessionSnapshot> snapshots = [];

        IFeed feed = new LiveFeed(new Frames("snapshot.json", "delta.json"));

        await foreach (ISessionSnapshot snapshot in feed)
        {
            snapshots.Add(snapshot);
        }

        Assert.Equal(2, snapshots.Count);
    }

    private static async Task<ISessionSnapshot> Last(
        IAsyncEnumerable<ReadOnlyMemory<byte>> frames
    )
    {
        ISessionSnapshot? last = null;

        await foreach (ISessionSnapshot snapshot in new LiveFeed(frames))
        {
            last = snapshot;
        }

        return last ?? throw new InvalidOperationException();
    }
}
