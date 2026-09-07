using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Timing;

public sealed record Records : IAsyncEnumerable<IRecord>
{
    private const string ModelMarker = "'hdrModel'>";

    private const string PositionMarker = "id='pos'>";

    private const string NameMarker = "id='name'>";

    private const string ResultMarker = "id='result'>";

    private readonly IAsyncEnumerable<IString> _documents;

    public Records(IAsyncEnumerable<IString> documents)
    {
        _documents = documents;
    }

    public async IAsyncEnumerator<IRecord> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (IString document in _documents.WithCancellation(cancellationToken))
        {
            string text = document.TextValue;
            string model = string.Empty;
            string position = string.Empty;
            string name = string.Empty;
            int cursor = 0;

            while (cursor < text.Length)
            {
                int marker = text.IndexOf("id='", cursor, StringComparison.Ordinal);
                int heading = text.IndexOf(ModelMarker, cursor, StringComparison.Ordinal);

                if ((heading >= 0) && ((marker < 0) || (heading < marker)))
                {
                    heading += ModelMarker.Length;
                    int end = text.IndexOf('<', heading);

                    if (end < 0)
                    {
                        break;
                    }

                    model = text[heading..end];
                    cursor = end;
                    continue;
                }

                if (marker < 0)
                {
                    break;
                }

                int value = text.IndexOf('>', marker);
                int closing = value < 0 ? -1 : text.IndexOf('<', value);

                if (closing < 0)
                {
                    break;
                }

                string content = text[(value + 1)..closing];
                string kind = text[marker..(value + 1)];
                cursor = closing;

                if (kind.EndsWith(PositionMarker, StringComparison.Ordinal))
                {
                    position = content;
                }
                else if (kind.EndsWith(NameMarker, StringComparison.Ordinal))
                {
                    name = content;
                }
                else if (kind.EndsWith(ResultMarker, StringComparison.Ordinal))
                {
                    yield return new Record(
                        new String(model),
                        new TextNumber(new String(position)),
                        new String(name),
                        new LapTimeMilliseconds(new String(content))
                    );
                }
            }
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
