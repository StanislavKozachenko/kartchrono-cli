using KartChrono.Abstractions.Output;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Rendering;

public sealed record HelpOutput : IOutput
{
    private static readonly string[] Usage =
    [
        "kartchrono - query and monitor KartChrono live timing",
        "",
        "Usage:",
        "  kartchrono tracks",
        "  kartchrono session --track <slug>",
        "  kartchrono live    --track <slug> [--kart <number>]",
        "  kartchrono laps    --track <slug> --kart <number>",
        "  kartchrono records --track <slug> [--period today|week|month]",
        "",
        "Options:",
        "  --track <slug>      track to read, as listed by 'kartchrono tracks'",
        "  --kart <number>     restrict output to a single kart",
        "  --period <range>    range for 'records' (default today)",
        "  --json              emit JSON instead of a table",
        "  --timeout <seconds> stop after the given time",
        "  --no-color          disable ANSI colour",
        "  --help              show this help",
        "  --version           show the version",
    ];

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        foreach (string line in Usage)
        {
            yield return new String(line);
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
