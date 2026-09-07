namespace KartChrono.Abstractions.Timing;

public interface ISessionSnapshot
{
    public IRunState Run { get; }

    public IEnumerable<ICompetitor> Competitors { get; }
}
