using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.String;
using String = Pure.Primitives.String.String;

namespace KartChrono.Timing;

public sealed record Tracks : IAsyncEnumerable<ITrack>
{
    private const string IdAttribute = "track_key='";

    private const string SlugAttribute = "url_prefix='";

    private readonly IAsyncEnumerable<IString> _documents;

    public Tracks(IAsyncEnumerable<IString> documents)
    {
        _documents = documents;
    }

    public async IAsyncEnumerator<ITrack> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        await foreach (IString document in _documents.WithCancellation(cancellationToken))
        {
            string text = document.TextValue;
            int cursor = 0;

            while (cursor < text.Length)
            {
                int tagStart = text.IndexOf("<div", cursor, StringComparison.Ordinal);

                if (tagStart < 0)
                {
                    break;
                }

                int tagEnd = text.IndexOf('>', tagStart);

                if (tagEnd < 0)
                {
                    break;
                }

                cursor = tagEnd + 1;

                string tag = text[tagStart..tagEnd];
                int id = tag.IndexOf(IdAttribute, StringComparison.Ordinal);
                int slug = tag.IndexOf(SlugAttribute, StringComparison.Ordinal);

                if ((id < 0) || (slug < 0))
                {
                    continue;
                }

                id += IdAttribute.Length;
                slug += SlugAttribute.Length;

                int idEnd = tag.IndexOf('\'', id);
                int slugEnd = tag.IndexOf('\'', slug);
                int nameEnd = text.IndexOf("</div>", cursor, StringComparison.Ordinal);

                if ((idEnd < 0) || (slugEnd < 0) || (nameEnd < 0))
                {
                    continue;
                }

                yield return new Track(
                    new String(tag[id..idEnd]),
                    new String(tag[slug..slugEnd]),
                    new String(text[cursor..nameEnd].Trim())
                );
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
