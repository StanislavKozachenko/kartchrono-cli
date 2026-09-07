using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Cli.Arguments;

public sealed record OptionPresence : IBool
{
    private readonly IString _name;

    private readonly IEnumerable<string> _arguments;

    public OptionPresence(IString name, params IEnumerable<string> arguments)
    {
        _name = name;
        _arguments = arguments;
    }

    public bool BoolValue =>
        _arguments.Contains($"--{_name.TextValue}", StringComparer.Ordinal);

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
