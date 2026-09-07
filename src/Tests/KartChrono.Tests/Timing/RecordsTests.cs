using KartChrono.Abstractions.Timing;
using KartChrono.Tests.Fixtures;
using KartChrono.Timing;
using Pure.Primitives.Abstractions.Number;
using String = Pure.Primitives.String.String;

namespace KartChrono.Tests.Timing;

public sealed record RecordsTests
{
    [Theory]
    [InlineData("1:17.521", 77521)]
    [InlineData("19.119", 19119)]
    [InlineData("0.001", 1)]
    [InlineData("", 0)]
    [InlineData("nonsense", 0)]
    [InlineData("1:2:3.000", 0)]
    [InlineData("x:17.521", 0)]
    public void ParsesLapTimeToMilliseconds(string text, int expected)
    {
        INumber<int> milliseconds = new LapTimeMilliseconds(new String(text));

        Assert.Equal(expected, milliseconds.NumberValue);
    }

    [Theory]
    [InlineData("7", 7)]
    [InlineData("", 0)]
    [InlineData("Staff", 0)]
    public void ParsesPlainNumber(string text, int expected)
    {
        Assert.Equal(expected, new TextNumber(new String(text)).NumberValue);
    }

    [Fact]
    public async Task TakesEveryRecordGroupedByKartClass()
    {
        IReadOnlyList<IRecord> records = await All(new Fixture("records.html"));

        Assert.Equal(4, records.Count);
        Assert.Equal(
            ["Клуб", "Прокат", "Прокат", "Прокат"],
            records.Select(r => r.KartModel.TextValue)
        );
        Assert.Equal([1, 1, 2, 3], records.Select(r => r.Position.NumberValue));
        Assert.Equal(77521, records[0].Milliseconds.NumberValue);
        Assert.Equal("", records[0].Name.TextValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData("<div>nothing here</div>")]
    [InlineData("<div class='hdrModel'>Клуб")]
    [InlineData("<div class='recordRowCell' id='result'")]
    public async Task TakesNoRecordsFromUnusableDocument(string document)
    {
        Assert.Empty(await All(new SingleDocument(document)));
    }

    private static async Task<IReadOnlyList<IRecord>> All(
        IAsyncEnumerable<Pure.Primitives.Abstractions.String.IString> documents
    )
    {
        List<IRecord> records = [];

        await foreach (IRecord record in new Records(documents))
        {
            records.Add(record);
        }

        return records;
    }
}
