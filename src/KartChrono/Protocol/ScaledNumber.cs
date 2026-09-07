using Pure.Primitives.Abstractions.Number;

namespace KartChrono.Protocol;

public sealed record ScaledNumber : INumber<long>
{
    private readonly INumber<int> _value;

    private readonly int _factor;

    public ScaledNumber(INumber<int> value, int factor)
    {
        _value = value;
        _factor = factor;
    }

    public long NumberValue => (long)_value.NumberValue * _factor;

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
