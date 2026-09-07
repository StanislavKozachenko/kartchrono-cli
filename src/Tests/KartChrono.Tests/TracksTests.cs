using KartChrono.Abstractions.Timing;
using KartChrono.Tests.Fixtures;
using KartChrono.Timing;
using String = Pure.Primitives.String.String;

namespace KartChrono.Tests;

public sealed record TracksTests
{
    [Fact]
    public async Task TakesEveryTrackFromDirectory()
    {
        List<ITrack> tracks = [];

        await foreach (ITrack track in new Tracks(new Fixture("gettrackslist.html")))
        {
            tracks.Add(track);
        }

        Assert.Equal(54, tracks.Count);
    }

    [Fact]
    public async Task TakesIdSlugAndName()
    {
        ITrack first = await FirstOf(new Tracks(new Fixture("gettrackslist.html")));

        Assert.Equal("0a7e400059d7cea2bc8504c3788753c0", first.Id.TextValue);
        Assert.Equal("mayak", first.Slug.TextValue);
        Assert.Equal("Маяк", first.Name.TextValue);
    }

    [Fact]
    public async Task TakesNoTracksFromUnrelatedDocument()
    {
        List<ITrack> tracks = [];

        await foreach (ITrack track in new Tracks(new SingleDocument("<div>nope</div>")))
        {
            tracks.Add(track);
        }

        Assert.Empty(tracks);
    }

    [Fact]
    public async Task TakesOnlyTracksMatchingSlug()
    {
        List<ITrack> tracks = [];

        IAsyncEnumerable<ITrack> matching = new TracksWithSlug(
            new Tracks(new Fixture("gettrackslist.html")),
            new String("MAYAK")
        );

        await foreach (ITrack track in matching)
        {
            tracks.Add(track);
        }

        Assert.Equal("mayak", Assert.Single(tracks).Slug.TextValue);
    }

    [Fact]
    public async Task TakesNoTracksForUnknownSlug()
    {
        List<ITrack> tracks = [];

        IAsyncEnumerable<ITrack> matching = new TracksWithSlug(
            new Tracks(new Fixture("gettrackslist.html")),
            new String("nowhere")
        );

        await foreach (ITrack track in matching)
        {
            tracks.Add(track);
        }

        Assert.Empty(tracks);
    }

    [Theory]
    [InlineData("<div")]
    [InlineData("<div track_key='a' url_prefix='b'>no closing tag")]
    [InlineData("<div url_prefix='b' track_key='a>x</div>")]
    [InlineData("plain text, no markup at all")]
    public async Task TakesNoTracksFromMalformedDocument(string document)
    {
        List<ITrack> tracks = [];

        await foreach (ITrack track in new Tracks(new SingleDocument(document)))
        {
            tracks.Add(track);
        }

        Assert.Empty(tracks);
    }

    [Fact]
    public void ThrowsExceptionOnGetHashCode()
    {
        _ = Assert.Throws<NotSupportedException>(() =>
            new Tracks(new SingleDocument("")).GetHashCode()
        );
        _ = Assert.Throws<NotSupportedException>(() =>
            new TracksWithSlug(
                new Tracks(new SingleDocument("")),
                new String("x")
            ).GetHashCode()
        );
        _ = Assert.Throws<NotSupportedException>(() =>
            new Track(new String("a"), new String("b"), new String("c")).GetHashCode()
        );
    }

    [Fact]
    public void ThrowsExceptionOnToString()
    {
        _ = Assert.Throws<NotSupportedException>(() =>
            new Tracks(new SingleDocument("")).ToString()
        );
        _ = Assert.Throws<NotSupportedException>(() =>
            new TracksWithSlug(
                new Tracks(new SingleDocument("")),
                new String("x")
            ).ToString()
        );
        _ = Assert.Throws<NotSupportedException>(() =>
            new Track(new String("a"), new String("b"), new String("c")).ToString()
        );
    }

    private static async Task<ITrack> FirstOf(IAsyncEnumerable<ITrack> tracks)
    {
        await foreach (ITrack track in tracks)
        {
            return track;
        }

        throw new InvalidOperationException();
    }
}
