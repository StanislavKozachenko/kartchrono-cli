using KartChrono.Abstractions.Output;
using KartChrono.Rendering;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Tests.Rendering;

public sealed record VersionOutputTests
{
    [Fact]
    public async Task TakesSingleVersionLine()
    {
        IOutput output = new VersionOutput(new String("1.2.3"));

        List<IString> lines = [];

        await foreach (IString line in output.Lines)
        {
            lines.Add(line);
        }

        Assert.Equal("1.2.3", Assert.Single(lines).TextValue);
    }
}
