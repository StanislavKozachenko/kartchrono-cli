using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.Number;

namespace KartChrono.Protocol;

public sealed record IsNonNegative : IBool
{
    private readonly INumber<int> _value;

    public IsNonNegative(INumber<int> value)
    {
        _value = value;
    }

    public bool BoolValue => _value.NumberValue >= 0;

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
