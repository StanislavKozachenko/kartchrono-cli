using KartChrono.Abstractions.Timing;

namespace KartChrono.Protocol;

public sealed record Snapshot : ISessionSnapshot
{
    public Snapshot(IRunState run, IEnumerable<ICompetitor> competitors)
    {
        Run = run;
        Competitors = competitors;
    }

    public IRunState Run { get; }

    public IEnumerable<ICompetitor> Competitors { get; }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
