using KartChrono.Abstractions.Timing;
using KartChrono.Protocol;

namespace KartChrono.Tests.Protocol;

public sealed record BinaryLapsTests
{
    private static ReadOnlyMemory<byte> Recorded =>
        File.ReadAllBytes(Path.Combine("Fixtures", "binlaps.bin"));

    [Fact]
    public void TakesEveryRecordFromFrame()
    {
        Assert.Equal(11, new BinaryLaps(Recorded).Count());
    }

    [Fact]
    public void TakesLapFields()
    {
        ILap second = new BinaryLaps(Recorded).ElementAt(1);

        Assert.Equal("-10002", second.CompetitorId.TextValue);
        Assert.Equal(1, second.Number.NumberValue);
        Assert.Equal(3, second.Position.NumberValue);
        Assert.Equal(82438, second.Milliseconds.NumberValue);
        Assert.Equal(994, second.Flags.NumberValue);
        Assert.Equal(
            [19811, 18441, 15147, 29039],
            second.Sectors.Select(sector => sector.NumberValue)
        );
    }

    [Fact]
    public void TakesTimestampFromSplitDoubleWord()
    {
        ILap second = new BinaryLaps(Recorded).ElementAt(1);

        Assert.Equal(2026, second.Timestamp.Year.NumberValue);
        Assert.Equal(9, second.Timestamp.Month.NumberValue);
        Assert.Equal(7, second.Timestamp.Day.NumberValue);
        Assert.Equal(11, second.Timestamp.Hour.NumberValue);
        Assert.Equal(29, second.Timestamp.Minute.NumberValue);
        Assert.Equal(4, second.Timestamp.Second.NumberValue);
    }

    [Fact]
    public void TakesNoLapsFromFrameWithoutHeader()
    {
        Assert.Empty(new BinaryLaps(new byte[] { 1, 2, 3, 4 }));
    }

    [Fact]
    public void TakesNoLapsFromTruncatedRecord()
    {
        Assert.Empty(new BinaryLaps(Recorded[..40]));
    }

    [Fact]
    public void EnumeratesAsUntypedSequence()
    {
        System.Collections.IEnumerable laps = new BinaryLaps(Recorded);

        int counted = 0;

        foreach (object lap in laps)
        {
            counted += 1;
        }

        Assert.Equal(11, counted);
    }
}
