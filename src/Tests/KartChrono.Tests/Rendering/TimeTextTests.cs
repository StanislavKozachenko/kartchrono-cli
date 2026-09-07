using KartChrono.Rendering;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Number;

namespace KartChrono.Tests.Rendering;

public sealed record TimeTextTests
{
    [Theory]
    [InlineData(77521, "1:17.521")]
    [InlineData(19119, "19.119")]
    [InlineData(3661001, "61:01.001")]
    [InlineData(0, "")]
    [InlineData(-5, "")]
    public void FormatsLapTime(int milliseconds, string expected)
    {
        IString text = new LapTimeText(new Int(milliseconds));

        Assert.Equal(expected, text.TextValue);
    }

    [Theory]
    [InlineData(900_000L, "00:15:00")]
    [InlineData(3_723_000L, "01:02:03")]
    [InlineData(0L, "00:00:00")]
    [InlineData(-1000L, "00:00:00")]
    public void FormatsClock(long milliseconds, string expected)
    {
        IString text = new ClockText(new Long(milliseconds));

        Assert.Equal(expected, text.TextValue);
    }

    [Fact]
    public void EnumeratesFormattedTextAsCharacters()
    {
        Assert.Equal(
            ['1', '9', '.', '1', '1', '9'],
            new LapTimeText(new Int(19119)).Select(symbol => symbol.CharValue)
        );
        Assert.Equal(
            ['0', '0', ':', '1', '5', ':', '0', '0'],
            new ClockText(new Long(900_000)).Select(symbol => symbol.CharValue)
        );
    }
}
