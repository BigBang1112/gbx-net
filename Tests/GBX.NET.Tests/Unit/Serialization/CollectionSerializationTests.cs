using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

public class CollectionSerializationTests
{
    [Test]
    [Arguments(-1, 0)]
    [Arguments(-1, 3)]
    [Arguments(0, 3)]
    [Arguments(1, 3)]
    [Arguments(3, 1)]
    [Arguments(3, 3)]
    [Arguments(3, 0)]
    [Arguments(1, 2050)]
    public async Task FixedPrimitiveCollections_TruncateAndPadWithoutWritingLengthPrefixes(int count, int length)
    {
        var values = count < 0 ? null : Enumerable.Range(1, count).Select(x => (short)x).ToArray();

        foreach (var asList in new[] { false, true })
        {
            using var stream = new MemoryStream();
            using (var writer = new GbxWriter(stream))
            {
                if (asList)
                {
                    writer.WriteList(values?.ToList(), length);
                }
                else
                {
                    writer.WriteArray(values, length);
                }
                writer.Write(0x11223344);
            }

            await Assert.That(stream.Length).IsEqualTo(length * sizeof(short) + sizeof(int));
            stream.Position = 0;
            using var reader = new GbxReader(stream);
            var restored = asList ? reader.ReadList<short>(length).ToArray() : reader.ReadArray<short>(length);
            for (var i = 0; i < length; i++)
            {
                await Assert.That(restored[i]).IsEqualTo((short)(i < count ? i + 1 : 0));
            }
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(stream.Position).IsEqualTo(stream.Length);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(3)]
    [Arguments(5)]
    public async Task FixedPrimitiveCollections_WithByteLengthsWriteExactlyTheRequestedBytes(int length)
    {
        short[] values = [0x1122, 0x3344];

        foreach (var asList in new[] { false, true })
        {
            using var stream = new MemoryStream();
            using var writer = new GbxWriter(stream);
            if (asList)
            {
                writer.WriteList(values.ToList(), length, lengthInBytes: true);
            }
            else
            {
                writer.WriteArray(values, length, lengthInBytes: true);
            }

            byte[] source = [0x22, 0x11, 0x44, 0x33, 0];
            await Assert.That(stream.ToArray().SequenceEqual(source.Take(length))).IsTrue();
        }
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task PrefixedPrimitiveCollections_UseElementOrByteLengthsAndRoundTrip(bool lengthInBytes, bool deprecated)
    {
        short[] values = [0x1122, 0x3344];

        foreach (var asList in new[] { false, true })
        {
            using var stream = new MemoryStream();
            using (var writer = new GbxWriter(stream))
            {
                if (asList)
                {
                    if (deprecated)
                    {
                        writer.WriteList_deprec(values.ToList(), lengthInBytes);
                    }
                    else
                    {
                        writer.WriteList(values.ToList(), lengthInBytes);
                    }
                }
                else if (deprecated)
                {
                    writer.WriteArray_deprec(values, lengthInBytes);
                }
                else
                {
                    writer.WriteArray(values, lengthInBytes);
                }
                writer.Write(0x55667788);
            }

            stream.Position = 0;
            using var reader = new GbxReader(stream);
            if (deprecated)
            {
                await Assert.That(reader.ReadInt32()).IsEqualTo(10);
            }
            await Assert.That(reader.ReadInt32()).IsEqualTo(lengthInBytes ? 4 : 2);
            await Assert.That(reader.ReadUInt16()).IsEqualTo((ushort)0x1122);
            await Assert.That(reader.ReadUInt16()).IsEqualTo((ushort)0x3344);

            stream.Position = 0;
            var restored = asList
                ? (deprecated ? reader.ReadList_deprec<short>(lengthInBytes) : reader.ReadList<short>(lengthInBytes)).ToArray()
                : deprecated ? reader.ReadArray_deprec<short>(lengthInBytes) : reader.ReadArray<short>(lengthInBytes);
            await Assert.That(restored.SequenceEqual(values)).IsTrue();
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x55667788);
        }
    }

    [Test]
    [Arguments(-1, 2)]
    [Arguments(0, 2)]
    [Arguments(1, 3)]
    [Arguments(3, 1)]
    [Arguments(3, 3)]
    [Arguments(3, 0)]
    public async Task FixedNodeCollections_PreserveReferencesAndPadWithNull(int count, int length)
    {
        var node = new CGameCtnBlockInfoClip();
        var values = count < 0 ? null : Enumerable.Range(0, count).Select(i => i == 1 ? null : node).ToArray();

        foreach (var asList in new[] { false, true })
        {
            using var stream = new MemoryStream();
            using (var writer = new GbxWriter(stream))
            {
                if (asList)
                {
                    writer.WriteListNodeRef(values?.ToList(), length);
                }
                else
                {
                    writer.WriteArrayNodeRef(values, length);
                }
                writer.Write(0x11223344);
            }

            stream.Position = 0;
            using var reader = new GbxReader(stream);
            var restored = asList
                ? reader.ReadListNodeRef<CGameCtnBlockInfoClip>(length).ToArray()
                : reader.ReadArrayNodeRef<CGameCtnBlockInfoClip>(length);
            for (var i = 0; i < length; i++)
            {
                if (i < count && i != 1)
                {
                    await Assert.That(restored[i]).IsNotNull();
                    await Assert.That(ReferenceEquals(restored[i], restored[0])).IsTrue();
                }
                else
                {
                    await Assert.That(restored[i]).IsNull();
                }
            }
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(stream.Position).IsEqualTo(stream.Length);
        }
    }

    [Test]
    [Arguments(-1, 2)]
    [Arguments(0, 2)]
    [Arguments(1, 3)]
    [Arguments(3, 1)]
    [Arguments(3, 3)]
    [Arguments(3, 0)]
    public async Task FixedExternalNodeLists_PreserveFilesAndPadWithNull(int count, int length)
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 0, true, "Clip.Gbx");
        var values = count < 0 ? null : Enumerable.Range(0, count)
            .Select(i => i == 1 ? null! : new External<CGameCtnBlockInfoClip>(null, file)).ToList();
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteListExternalNodeRef(values, length);
            writer.Write(0x11223344);
        }

        await Assert.That(stream.Length).IsEqualTo((length + 1) * sizeof(int));
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
        var restored = reader.ReadListExternalNodeRef<CGameCtnBlockInfoClip>(length);
        for (var i = 0; i < length; i++)
        {
            await Assert.That(ReferenceEquals(restored[i].File, file)).IsEqualTo(i < count && i != 1);
            await Assert.That(restored[i].Node).IsNull();
        }
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
    }

    [Test]
    public async Task FixedCollections_RejectNegativeLengthsBeforeWriting()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteArray<int>(null, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteList<int>(null, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteArrayNodeRef<CGameCtnBlockInfoClip>(null, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteListNodeRef<CGameCtnBlockInfoClip>(null, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteListExternalNodeRef<CGameCtnBlockInfoClip>(null, -1));
        await Assert.That(stream.Length).IsEqualTo(0);
    }

    [Test]
    public async Task Collections_RespectTheConfiguredByteLimitBeforeWriting()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream, new GbxWriteSettings { MaxDataSize = 7 });
        Assert.Throws<LengthLimitException>(() => writer.WriteArray<int>(null, 2));
        Assert.Throws<LengthLimitException>(() => writer.WriteList<int>(null, 2));
        Assert.Throws<LengthLimitException>(() => writer.WriteArray(new[] { 1, 2 }));
        Assert.Throws<LengthLimitException>(() => writer.WriteList(new List<int> { 1, 2 }));
        Assert.Throws<LengthLimitException>(() => writer.WriteArrayNodeRef<CGameCtnBlockInfoClip>(null, 8));
        Assert.Throws<LengthLimitException>(() => writer.WriteListNodeRef<CGameCtnBlockInfoClip>(null, 8));
        Assert.Throws<LengthLimitException>(() => writer.WriteListExternalNodeRef<CGameCtnBlockInfoClip>(null, 8));
        await Assert.That(stream.Length).IsEqualTo(0);
    }
}
