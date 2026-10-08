using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

public class NullableIntegerSerializationTests
{
    [Test]
    [Arguments(8)]
    [Arguments(16)]
    [Arguments(32)]
    [Arguments(64)]
    [Arguments(128)]
    public async Task SignedIntegersTranslateOnlyMinusOneAndPreserveTheirWidth(int bits)
    {
        switch (bits)
        {
            case 8:
                await Verify<sbyte>(1, -1, sbyte.MinValue, -2, sbyte.MaxValue,
                    (w, v) => w.WriteSByteNullable(v), r => r.ReadSByteNullable(),
                    (GbxReaderWriter rw, ref sbyte? v) => rw.SByteNullable(ref v),
                    (rw, v) => rw.SByteNullable(v));
                break;
            case 16:
                await Verify<short>(2, -1, short.MinValue, -2, short.MaxValue,
                    (w, v) => w.WriteInt16Nullable(v), r => r.ReadInt16Nullable(),
                    (GbxReaderWriter rw, ref short? v) => rw.Int16Nullable(ref v),
                    (rw, v) => rw.Int16Nullable(v));
                break;
            case 32:
                await Verify<int>(4, -1, int.MinValue, -2, int.MaxValue,
                    (w, v) => w.WriteInt32Nullable(v), r => r.ReadInt32Nullable(),
                    (GbxReaderWriter rw, ref int? v) => rw.Int32Nullable(ref v),
                    (rw, v) => rw.Int32Nullable(v));
                break;
            case 64:
                await Verify<long>(8, -1, long.MinValue, -2, long.MaxValue,
                    (w, v) => w.WriteInt64Nullable(v), r => r.ReadInt64Nullable(),
                    (GbxReaderWriter rw, ref long? v) => rw.Int64Nullable(ref v),
                    (rw, v) => rw.Int64Nullable(v));
                break;
            case 128:
                await Verify<Int128>(16, -1, Int128.MinValue, -2, Int128.MaxValue,
                    (w, v) => w.WriteInt128Nullable(v), r => r.ReadInt128Nullable(),
                    (GbxReaderWriter rw, ref Int128? v) => rw.Int128Nullable(ref v),
                    (rw, v) => rw.Int128Nullable(v));
                break;
        }
    }

    private delegate void ReadWrite<T>(GbxReaderWriter rw, ref T? value) where T : struct;

    private static async Task Verify<T>(
        int byteWidth,
        T sentinel,
        T minimum,
        T negativeTwo,
        T maximum,
        Action<GbxWriter, T?> write,
        Func<GbxReader, T?> read,
        ReadWrite<T> readWrite,
        Func<GbxReaderWriter, T?, T?> readWriteValue) where T : struct
    {
        foreach (var value in new T?[] { null, sentinel, minimum, negativeTwo, default(T), maximum })
        {
            var expected = value.HasValue && value.Value.Equals(sentinel) ? null : value;
            using var input = new MemoryStream();
            using (var writer = new GbxWriter(input))
            {
                write(writer, value);
            }
            var bytes = input.ToArray();
            await Assert.That(bytes.Length).IsEqualTo(byteWidth);
            if (expected is null)
            {
                await Assert.That(bytes.All(x => x == 0xFF)).IsTrue();
            }

            input.Position = 0;
            using var reader = new GbxReader(input);
            await Assert.That(read(reader)).IsEqualTo(expected);
            await Assert.That(input.Position).IsEqualTo((long)byteWidth);

            input.Position = 0;
            using var output = new MemoryStream();
            using var outputWriter = new GbxWriter(output);
            using var rw = new GbxReaderWriter(reader, outputWriter);
            T? restored = maximum;
            readWrite(rw, ref restored);
            await Assert.That(restored).IsEqualTo(expected);
            await Assert.That(output.ToArray().SequenceEqual(bytes)).IsTrue();

            input.Position = 0;
            output.SetLength(0);
            await Assert.That(readWriteValue(rw, maximum)).IsEqualTo(expected);
            await Assert.That(output.ToArray().SequenceEqual(bytes)).IsTrue();

            input.Position = 0;
            using var readerOnly = new GbxReaderWriter(reader);
            restored = maximum;
            readWrite(readerOnly, ref restored);
            await Assert.That(restored).IsEqualTo(expected);

            output.SetLength(0);
            using var writerOnly = new GbxReaderWriter(outputWriter);
            restored = value;
            readWrite(writerOnly, ref restored);
            await Assert.That(restored).IsEqualTo(value);
            await Assert.That(output.ToArray().SequenceEqual(bytes)).IsTrue();
        }
    }
}
