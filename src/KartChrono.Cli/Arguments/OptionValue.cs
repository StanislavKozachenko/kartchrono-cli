using Pure.Primitives.Abstractions.Char;
using Pure.Primitives.Abstractions.String;
using Char = Pure.Primitives.Char.Char;

namespace KartChrono.Cli.Arguments;

public sealed record OptionValue : IString
{
    private readonly IString _name;

    private readonly IEnumerable<string> _arguments;

    public OptionValue(IString name, params IEnumerable<string> arguments)
    {
        _name = name;
        _arguments = arguments;
    }

    public string TextValue
    {
        get
        {
            string flag = $"--{_name.TextValue}";
            string[] arguments = [.. _arguments];

            for (int index = 0; index < (arguments.Length - 1); index += 1)
            {
                if (
                    string.Equals(arguments[index], flag, StringComparison.Ordinal)
                    && !arguments[index + 1].StartsWith("--", StringComparison.Ordinal)
                )
                {
                    return arguments[index + 1];
                }
            }

            return string.Empty;
        }
    }

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
