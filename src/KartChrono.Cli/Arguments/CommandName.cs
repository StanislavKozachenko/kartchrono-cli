using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.String;
using Char = Pure.Primitives.Char.Char;

namespace KartChrono.Cli.Arguments;

public sealed record CommandName : IString
{
    private readonly IEnumerable<string> _arguments;

    public CommandName(params IEnumerable<string> arguments)
    {
        _arguments = arguments;
    }

    public string TextValue =>
        _arguments.FirstOrDefault() is string first
        && !first.StartsWith("--", StringComparison.Ordinal)
            ? first
            : string.Empty;

    public IEnumerator<IChar> GetEnumerator()
    {
        return TextValue.Select(symbol => new Char(symbol)).Cast<IChar>().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
