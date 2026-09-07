using System.Diagnostics.CodeAnalysis;
using KartChrono.Abstractions.Output;
using KartChrono.Rendering;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Cli;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static async Task Main()
    {
        IOutput output = new VersionOutput(new String("0.1.0"));

        await foreach (IString line in output)
        {
            Console.WriteLine(line.TextValue);
        }
    }
}
