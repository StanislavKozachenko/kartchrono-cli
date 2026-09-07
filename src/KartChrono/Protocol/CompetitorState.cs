using KartChrono.Abstractions.Timing;
using Pure.Primitives.Abstractions.Number;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Protocol;

public sealed record CompetitorState : ICompetitor
{
    private readonly IReadOnlyDictionary<string, string> _fields;

    public CompetitorState(
        IReadOnlyDictionary<string, string> fields,
        IEnumerable<ILap> laps
    )
    {
        _fields = fields;
        Laps = laps;
    }

    public INumber<int> Position => new FieldNumber(_fields, Field.Position);

    public IString Number => new FieldText(_fields, Field.Number);

    public IString Name => new FieldText(_fields, Field.Name);

    public IString KartModel => new FieldText(_fields, Field.KartModel);

    public INumber<int> LapsCount => new FieldNumber(_fields, Field.LapsCount);

    public INumber<int> BestLapMilliseconds =>
        new FieldNumber(_fields, Field.BestLapTime);

    public INumber<int> LastLapMilliseconds =>
        new FieldNumber(_fields, Field.LastLapTime);

    public INumber<int> GapMilliseconds => new FieldNumber(_fields, Field.Gap);

    public INumber<int> DifferenceMilliseconds =>
        new FieldNumber(_fields, Field.Difference);

    public INumber<int> PitsCount => new FieldNumber(_fields, Field.PitsCount);

    public IEnumerable<INumber<int>> LastSectors =>
        [
            new FieldNumber(_fields, Field.LastSector1),
            new FieldNumber(_fields, Field.LastSector2),
            new FieldNumber(_fields, Field.LastSector3),
            new FieldNumber(_fields, Field.LastSector4),
            new FieldNumber(_fields, Field.LastSector5),
        ];

    public IEnumerable<ILap> Laps { get; }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
