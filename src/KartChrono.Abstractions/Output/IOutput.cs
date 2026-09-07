using Pure.Primitives.Abstractions.String;

namespace KartChrono.Abstractions.Output;

public interface IOutput
{
    public IAsyncEnumerable<IString> Lines { get; }
}
