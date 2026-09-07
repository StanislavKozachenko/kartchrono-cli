using KartChrono.Abstractions.Output;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Rendering;

public sealed record VersionOutput : IOutput
{
    private readonly IString _version;

    public VersionOutput(IString version)
    {
        _version = version;
    }

    public IAsyncEnumerable<IString> Lines => AsyncEnumerable.Repeat(_version, 1);

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
