using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Tests.Fixtures;

public sealed record Fixture : IAsyncEnumerable<IString>
{
    private readonly string _name;

    public Fixture(string name)
    {
        _name = name;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        yield return new String(
            await File.ReadAllTextAsync(
                Path.Combine("Fixtures", _name),
                cancellationToken
            )
        );
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
