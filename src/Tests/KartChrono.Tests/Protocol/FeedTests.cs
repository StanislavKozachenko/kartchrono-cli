using KartChrono.Abstractions.Timing;
using KartChrono.Protocol;
using KartChrono.Tests.Fixtures;
using Pure.Primitives.Number;
using String = Pure.Primitives.String.String;

namespace KartChrono.Tests.Protocol;

public sealed record FeedTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(9, 1)]
    [InlineData(0, 0)]
    [InlineData(-1, 0)]
    public async Task TakesOnlySettledSnapshot(int requested, int expected)
    {
        IFeed feed = new SettledFeed(
            new LiveFeed(new Frames("snapshot.json", "delta.json")),
            new Int(requested)
        );

        Assert.Equal(expected, await Count(feed));
    }

    [Fact]
    public async Task TakesLastSnapshotOfRequestedCount()
    {
        IFeed feed = new SettledFeed(
            new LiveFeed(new Frames("snapshot.json", "delta.json")),
            new Int(2)
        );

        await foreach (ISessionSnapshot snapshot in feed)
        {
            Assert.Equal(335_000, snapshot.Run.ElapsedMilliseconds.NumberValue);
        }
    }

    [Fact]
    public async Task KeepsOnlyRequestedKart()
    {
        IFeed feed = new FeedOfKart(
            new LiveFeed(new Frames("snapshot.json")),
            new String("25")
        );

        await foreach (ISessionSnapshot snapshot in feed)
        {
            Assert.Equal("25", Assert.Single(snapshot.Competitors).Number.TextValue);
            Assert.Equal("Заезд №3", snapshot.Run.RunName.TextValue);
        }
    }

    [Fact]
    public async Task KeepsEveryKartWhenNoneRequested()
    {
        IFeed feed = new FeedOfKart(
            new LiveFeed(new Frames("snapshot.json")),
            new String("")
        );

        await foreach (ISessionSnapshot snapshot in feed)
        {
            Assert.Equal(3, snapshot.Competitors.Count());
        }
    }

    [Fact]
    public async Task KeepsNoKartWhenUnknownRequested()
    {
        IFeed feed = new FeedOfKart(
            new LiveFeed(new Frames("snapshot.json")),
            new String("999")
        );

        await foreach (ISessionSnapshot snapshot in feed)
        {
            Assert.Empty(snapshot.Competitors);
        }
    }

    private static async Task<int> Count(IFeed feed)
    {
        int counted = 0;

        await foreach (ISessionSnapshot snapshot in feed)
        {
            counted += 1;
        }

        return counted;
    }
}
