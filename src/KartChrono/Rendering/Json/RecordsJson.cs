using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Rendering.Json;

public sealed record RecordsJson : IOutput
{
    private readonly IAsyncEnumerable<IRecord> _records;

    public RecordsJson(IAsyncEnumerable<IRecord> records)
    {
        _records = records;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (IRecord record in _records.WithCancellation(cancellationToken))
        {
            yield return new JsonLine(writer =>
            {
                writer.WriteString("kartModel", record.KartModel.TextValue);
                writer.WriteNumber("position", record.Position.NumberValue);
                writer.WriteString("name", record.Name.TextValue);
                writer.WriteNumber("milliseconds", record.Milliseconds.NumberValue);
            });
        }
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
