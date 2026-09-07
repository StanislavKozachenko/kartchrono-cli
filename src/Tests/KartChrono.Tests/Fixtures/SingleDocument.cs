using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Tests.Fixtures;

public sealed record SingleDocument : IAsyncEnumerable<IString>
{
    private readonly string _text;

    public SingleDocument(string text)
    {
        _text = text;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;
        yield return new String(_text);
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
