using KartChrono.Abstractions.Timing;
using KartChrono.Protocol;

namespace KartChrono.Tests.Protocol;

public sealed record SessionStateTests
{
    private static readonly Dictionary<string, string> Fields = new Dictionary<
        string,
        string
    >
    {
        ["1"] = "2",
        ["3"] = "7",
    };

    [Fact]
    public void TakesRunAndCompetitorsFromSnapshot()
    {
        ICompetitor competitor = new CompetitorState(Fields, []);
        IRunState run = new RunState(Fields);
        ISessionSnapshot snapshot = new Snapshot(run, [competitor]);

        Assert.Same(run, snapshot.Run);
        Assert.Same(competitor, Assert.Single(snapshot.Competitors));
    }

    [Fact]
    public void TakesNoLapsWithoutBinaryFrames()
    {
        Assert.Empty(new CompetitorState(Fields, []).Laps);
    }
}
