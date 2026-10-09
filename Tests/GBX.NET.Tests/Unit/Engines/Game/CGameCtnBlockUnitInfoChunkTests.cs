using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockUnitInfoChunkTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task Chunk0303600C_ReadsNativeLayoutAndRoundTripsExternalClips(int version)
    {
        // Counts are north=3, east=2, south=1, west=0, top=3, bottom=1.
        int[] counts = [3, 2, 1, 0, 3, 1];
        var refTable = new GbxRefTable();
        var files = Enumerable.Range(1, 10).ToDictionary(index => index,
            index => (GbxRefTableNode)new GbxRefTableFile(refTable, 0, true, $"Clip{index}.Gbx"));
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            if (version == 0)
            {
                writer.Write((ushort)0x071B);
            }
            else
            {
                writer.Write(0xB053);
            }
            for (var index = 1; index <= 10; index++)
            {
                writer.Write(index);
            }
            if (version >= 2)
            {
                writer.Write((short)-123);
                writer.Write((short)456);
            }
            else
            {
                writer.Write(-123456789);
                writer.Write(987654321);
            }
            writer.Write(0x11223344);
        }

        var payloadLength = stream.Length - sizeof(int);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(files);
        var restored = new CGameCtnBlockUnitInfo();
        var chunk = new CGameCtnBlockUnitInfo.Chunk0303600C();
        using var readWrite = new GbxReaderWriter(reader);
        chunk.ReadWrite(restored, readWrite);

        await Assert.That(chunk.Version).IsEqualTo(version);
        var directions = new[] { restored.ClipsNorth!, restored.ClipsEast!, restored.ClipsSouth!,
            restored.ClipsWest!, restored.ClipsTop!, restored.ClipsBottom! };
        var nextIndex = 1;
        for (var direction = 0; direction < counts.Length; direction++)
        {
            await Assert.That(directions[direction].Length).IsEqualTo(counts[direction]);
            foreach (var clip in directions[direction])
            {
                await Assert.That(clip.File).IsEqualTo((GbxRefTableFile)files[nextIndex++]);
            }
        }
        if (version >= 2)
        {
            await Assert.That(chunk.U01).IsEqualTo((short)-123);
            await Assert.That(chunk.U02).IsEqualTo((short)456);
        }
        else
        {
            await Assert.That(chunk.U03).IsEqualTo(-123456789);
            await Assert.That(chunk.U04).IsEqualTo(987654321);
        }
        await Assert.That(stream.Position).IsEqualTo(payloadLength);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        {
            using var rw = new GbxReaderWriter(writer);
            chunk.ReadWrite(restored, rw);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(stream.ToArray().Take((int)payloadLength))).IsTrue();
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Chunk0303600C_LaterVersionsWriteSevenClipsPerDirection(int version)
    {
        var original = new CGameCtnBlockUnitInfo
        {
            ClipsNorth = Enumerable.Range(0, 7).Select(_ => new External<CGameCtnBlockInfoClip>(null, null)).ToArray(),
            ClipsBottom = Enumerable.Range(0, 7).Select(_ => new External<CGameCtnBlockInfoClip>(null, null)).ToArray()
        };
        var chunk = new CGameCtnBlockUnitInfo.Chunk0303600C { Version = version };
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            using var rw = new GbxReaderWriter(writer);
            chunk.ReadWrite(original, rw);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadInt32()).IsEqualTo(version);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x38007);
        for (var index = 0; index < 14; index++)
        {
            await Assert.That(reader.ReadInt32()).IsEqualTo(-1);
        }
        if (version >= 2)
        {
            await Assert.That(reader.ReadInt16()).IsEqualTo((short)0);
            await Assert.That(reader.ReadInt16()).IsEqualTo((short)0);
        }
        else
        {
            await Assert.That(reader.ReadInt32()).IsEqualTo(0);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0);
        }
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task Chunk0303600C_Version0RejectsMoreThanThreeClips(int direction)
    {
        var clips = Enumerable.Range(0, 4).Select(_ => new External<CGameCtnBlockInfoClip>(null, null)).ToArray();
        var original = new CGameCtnBlockUnitInfo
        {
            ClipsNorth = direction == 0 ? clips : null,
            ClipsEast = direction == 1 ? clips : null,
            ClipsSouth = direction == 2 ? clips : null,
            ClipsWest = direction == 3 ? clips : null,
            ClipsTop = direction == 4 ? clips : null,
            ClipsBottom = direction == 5 ? clips : null
        };
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        var chunk = new CGameCtnBlockUnitInfo.Chunk0303600C { Version = 0 };
        Assert.Throws<InvalidOperationException>(() => chunk.ReadWrite(original, rw));
        await Assert.That(stream.Length).IsEqualTo(0);
    }

    [Test]
    public async Task Chunk0303600C_Version0WritesEmptyClipCounts()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            using var rw = new GbxReaderWriter(writer);
            new CGameCtnBlockUnitInfo.Chunk0303600C { Version = 0 }.ReadWrite(new CGameCtnBlockUnitInfo(), rw);
        }
        await Assert.That(stream.ToArray().SequenceEqual(new byte[14])).IsTrue();
    }
}
