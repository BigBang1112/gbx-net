using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

public class JaggedArraySerializationTests
{
    [Test]
    public async Task JaggedArrayReadableWritable_RoundTripsAllLengthCombinations()
    {
        (int? InnerLength, int? OuterLength)[] lengths =
        [
            (null, null),
            (2, null),
            (null, 2),
            (2, 2)
        ];

        foreach (var (innerLength, outerLength) in lengths)
        {
            Value[][] original = innerLength.HasValue
                ? [[new(1), new(2)], [new(3), new(4)]]
                : [[new(1), new(2)], [new(3)]];

            using var stream = new MemoryStream();
            using (var writer = new GbxWriter(stream))
            using (var rw = new GbxReaderWriter(writer))
            {
                rw.JaggedArrayReadableWritable(ref original, innerLength, outerLength);
            }

            var valueCount = innerLength.HasValue ? 4 : 3;
            var prefixCount = (outerLength.HasValue ? 0 : 1) + (innerLength.HasValue ? 0 : 2);
            await Assert.That(stream.Length).IsEqualTo((valueCount + prefixCount) * sizeof(int));

            stream.Position = 0;
            using var reader = new GbxReader(stream);
            using var readWriter = new GbxReaderWriter(reader);
            Value[][]? restored = null;
            readWriter.JaggedArrayReadableWritable(ref restored, innerLength, outerLength);

            await Assert.That(restored).Count().IsEqualTo(2);
            await Assert.That(restored![0]).Count().IsEqualTo(2);
            await Assert.That(restored[1]).Count().IsEqualTo(innerLength.HasValue ? 2 : 1);
            await Assert.That(restored[0][0].Number).IsEqualTo(1);
            await Assert.That(restored[0][1].Number).IsEqualTo(2);
            await Assert.That(restored[1][0].Number).IsEqualTo(3);

            if (innerLength.HasValue)
            {
                await Assert.That(restored[1][1].Number).IsEqualTo(4);
            }
        }
    }

    [Test]
    public void JaggedArrayReadableWritable_RejectsMismatchedFixedLengths()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        Value[][] values = [[new(1)]];

        Assert.Throws<InvalidOperationException>(() => rw.JaggedArrayReadableWritable(ref values, outerLength: 2));
        Assert.Throws<InvalidOperationException>(() => rw.JaggedArrayReadableWritable(ref values, innerLength: 2));
    }

    private sealed class Value(int number) : IReadable, IWritable
    {
        public Value() : this(0) { }

        public int Number { get; private set; } = number;

        public void Read(GbxReader r, int v = 0) => Number = r.ReadInt32();

        public void Write(GbxWriter w, int v = 0) => w.Write(Number);
    }
}
