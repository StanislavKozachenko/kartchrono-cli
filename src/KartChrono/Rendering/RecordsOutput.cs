using System.Globalization;
using System.Text;
using KartChrono.Abstractions.Output;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Rendering;

public sealed record RecordsOutput : IOutput
{
    private static readonly CompositeFormat Row = CompositeFormat.Parse(
        "{0,-16} {1,3}  {2,10}  {3}"
    );

    private readonly IAsyncEnumerable<IRecord> _records;

    public RecordsOutput(IAsyncEnumerable<IRecord> records)
    {
        _records = records;
    }

    public async IAsyncEnumerator<IString> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        yield return new String(
            string.Format(
                CultureInfo.InvariantCulture,
                Row,
                "CLASS",
                "POS",
                "TIME",
                "NAME"
            )
        );

        await foreach (IRecord record in _records.WithCancellation(cancellationToken))
        {
            yield return new String(
                string.Format(
                    CultureInfo.InvariantCulture,
                    Row,
                    record.KartModel.TextValue,
                    record.Position.NumberValue,
                    new LapTimeText(record.Milliseconds).TextValue,
                    record.Name.TextValue
                )
            );
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
