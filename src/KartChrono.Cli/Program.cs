using System.Diagnostics.CodeAnalysis;
using KartChrono.Cli.Commands;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Cli;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        using HttpClient client = new HttpClient();

        try
        {
            await foreach (IString line in new Command(client, args))
            {
                Console.WriteLine(line.TextValue);
            }

            return 0;
        }
        catch (ArgumentException error)
        {
            await Console.Error.WriteLineAsync(error.Message);
            return 1;
        }
        catch (HttpRequestException error)
        {
            await Console.Error.WriteLineAsync(error.Message);
            return 1;
        }
        catch (OperationCanceledException)
        {
            return 0;
        }
    }
}
