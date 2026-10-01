using GBX.NET.Engines.Function;
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

    [Test]
    public async Task JaggedArray_RoundTripsAllLengthCombinations()
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
            int[][] values = innerLength.HasValue ? [[1, 2], [3, 4]] : [[1, 2], [3]];
            using var stream = new MemoryStream();
            using (var writer = new GbxWriter(stream))
            using (var rw = new GbxReaderWriter(writer))
            {
                rw.JaggedArray(ref values, innerLength, outerLength);
            }

            var valueCount = innerLength.HasValue ? 4 : 3;
            var prefixCount = (outerLength.HasValue ? 0 : 1) + (innerLength.HasValue ? 0 : 2);
            await Assert.That(stream.Length).IsEqualTo((valueCount + prefixCount) * sizeof(int));

            stream.Position = 0;
            using var reader = new GbxReader(stream);
            using var readWriter = new GbxReaderWriter(reader);
            int[][]? restored = null;
            readWriter.JaggedArray(ref restored, innerLength, outerLength);

            await Assert.That(restored![0]).IsEquivalentTo(new[] { 1, 2 });
            await Assert.That(restored[1]).IsEquivalentTo(innerLength.HasValue ? new[] { 3, 4 } : new[] { 3 });
        }
    }

    [Test]
    public async Task JaggedArrayIdsAndStrings_RoundTrip()
    {
        string[][] ids = [["First", "Second"], ["Third"]];
        string[][] strings = [["One"], ["Two", "Three"]];
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.JaggedArrayId(ref ids);
            rw.JaggedArrayString(ref strings, outerLength: 2);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var readWriter = new GbxReaderWriter(reader);
        string[][]? restoredIds = null;
        string[][]? restoredStrings = null;
        readWriter.JaggedArrayId(ref restoredIds);
        readWriter.JaggedArrayString(ref restoredStrings, outerLength: 2);

        await Assert.That(restoredIds![0]).IsEquivalentTo(ids[0]);
        await Assert.That(restoredIds[1]).IsEquivalentTo(ids[1]);
        await Assert.That(restoredStrings![0]).IsEquivalentTo(strings[0]);
        await Assert.That(restoredStrings[1]).IsEquivalentTo(strings[1]);
    }

    [Test]
    public async Task JaggedArrayNodeReferences_SupportFixedLengths()
    {
        CFuncSkel?[][] nodeRefs = [[]];
        External<CFuncSkel>[][] externalRefs = [[]];
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.JaggedArrayNodeRef(ref nodeRefs, innerLength: 0, outerLength: 1);
            rw.JaggedArrayExternalNodeRef(ref externalRefs, innerLength: 0, outerLength: 1);
        }

        await Assert.That(stream.Length).IsEqualTo(0);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var readWriter = new GbxReaderWriter(reader);
        CFuncSkel?[][]? restoredNodes = null;
        External<CFuncSkel>[][]? restoredExternals = null;
        readWriter.JaggedArrayNodeRef(ref restoredNodes, innerLength: 0, outerLength: 1);
        readWriter.JaggedArrayExternalNodeRef(ref restoredExternals, innerLength: 0, outerLength: 1);

        await Assert.That(restoredNodes).Count().IsEqualTo(1);
        await Assert.That(restoredNodes![0]).IsEmpty();
        await Assert.That(restoredExternals).Count().IsEqualTo(1);
        await Assert.That(restoredExternals![0]).IsEmpty();
    }

    [Test]
    public async Task JaggedArray_RejectsMismatchedFixedLengths()
    {
        int[][] values = [[1]];
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);

        Assert.Throws<InvalidOperationException>(() => rw.JaggedArray(ref values, innerLength: 2));
        Assert.Throws<InvalidOperationException>(() => rw.JaggedArray(ref values, outerLength: 2));
        await Assert.That(stream.Length).IsEqualTo(0);
    }

    private sealed class Value(int number) : IReadable, IWritable
    {
        public Value() : this(0) { }

        public int Number { get; private set; } = number;

        public void Read(GbxReader r, int v = 0) => Number = r.ReadInt32();

        public void Write(GbxWriter w, int v = 0) => w.Write(Number);
    }
}
