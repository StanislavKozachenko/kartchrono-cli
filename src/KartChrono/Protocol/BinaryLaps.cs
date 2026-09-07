using System.Buffers.Binary;
using System.Globalization;
using KartChrono.Abstractions.Timing;
using Pure.Primitives.Number;
using Date = Pure.Primitives.Date.Date;
using DateTime = Pure.Primitives.DateTime.DateTime;
using String = Pure.Primitives.String.String;
using Time = Pure.Primitives.Time.Time;

namespace KartChrono.Protocol;

public sealed record BinaryLaps : IEnumerable<ILap>
{
    private const int HeaderLength = 8;

    private const int RecordLength = 52;

    private readonly ReadOnlyMemory<byte> _frame;

    public BinaryLaps(ReadOnlyMemory<byte> frame)
    {
        _frame = frame;
    }

    public IEnumerator<ILap> GetEnumerator()
    {
        if (!_frame.Span.StartsWith("BINLAPS:"u8))
        {
            yield break;
        }

        for (
            int offset = HeaderLength;
            offset + RecordLength <= _frame.Length;
            offset += RecordLength
        )
        {
            ReadOnlySpan<byte> record = _frame.Span.Slice(offset, RecordLength);

            System.DateTime moment = System.DateTime.UnixEpoch.AddMilliseconds(
                BinaryPrimitives.ReadUInt32LittleEndian(record[36..])
                    + (
                        BinaryPrimitives.ReadUInt32LittleEndian(record[40..])
                        * 4294967296D
                    )
            );

            yield return new Lap(
                new String(
                    BinaryPrimitives
                        .ReadInt32LittleEndian(record)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new Int(BinaryPrimitives.ReadInt32LittleEndian(record[8..])),
                new Int(BinaryPrimitives.ReadInt32LittleEndian(record[16..])),
                new Int(BinaryPrimitives.ReadInt32LittleEndian(record[32..])),
                [
                    new Int(BinaryPrimitives.ReadInt32LittleEndian(record[20..])),
                    new Int(BinaryPrimitives.ReadInt32LittleEndian(record[24..])),
                    new Int(BinaryPrimitives.ReadInt32LittleEndian(record[28..])),
                    new Int(BinaryPrimitives.ReadInt32LittleEndian(record[48..])),
                ],
                new DateTime(
                    new Date(DateOnly.FromDateTime(moment)),
                    new Time(TimeOnly.FromDateTime(moment))
                ),
                new Int(BinaryPrimitives.ReadInt32LittleEndian(record[44..]))
            );
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
