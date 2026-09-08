using KartChrono.Protocol;
using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Tests.Protocol;

public sealed record FieldReaderTests
{
    private static readonly Dictionary<string, string> Fields = new Dictionary<
        string,
        string
    >
    {
        ["1"] = "42",
        ["3"] = "Staff",
        ["9"] = "not a number",
    };

    [Theory]
    [InlineData("1", 42)]
    [InlineData("9", 0)]
    [InlineData("404", 0)]
    public void TakesNumberOrZero(string key, int expected)
    {
        INumber<int> number = new FieldNumber(Fields, key);

        Assert.Equal(expected, number.NumberValue);
    }

    [Theory]
    [InlineData("3", "Staff")]
    [InlineData("404", "")]
    public void TakesTextOrEmpty(string key, string expected)
    {
        IString text = new FieldText(Fields, key);

        Assert.Equal(expected, text.TextValue);
    }

    [Fact]
    public void EnumeratesTextAsCharacters()
    {
        IString text = new FieldText(Fields, "3");

        Assert.Equal(['S', 't', 'a', 'f', 'f'], text.Select(symbol => symbol.CharValue));
    }

    [Fact]
    public void EnumeratesTextAsUntypedSequence()
    {
        System.Collections.IEnumerable text = new FieldText(Fields, "3");

        List<char> symbols = [];

        foreach (object symbol in text)
        {
            symbols.Add(((IChar)symbol).CharValue);
        }

        Assert.Equal(['S', 't', 'a', 'f', 'f'], symbols);
    }

    [Theory]
    [InlineData(1000, 42000)]
    [InlineData(0, 0)]
    public void ScalesNumber(int factor, long expected)
    {
        INumber<long> scaled = new ScaledNumber(new FieldNumber(Fields, "1"), factor);

        Assert.Equal(expected, scaled.NumberValue);
    }
}
