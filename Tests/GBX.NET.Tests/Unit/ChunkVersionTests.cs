using GBX.NET.Attributes;
using GBX.NET.Exceptions;
using GBX.NET.Managers;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Reflection;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class ChunkVersionTests
{
    [Test]
    public async Task ChunkVersions_OnlyIncludeZeroAlongsideANonzeroGameVersion()
    {
        var missing = new List<string>();
        foreach (var type in typeof(Gbx).Assembly.GetTypes().Where(type =>
            !type.IsAbstract && typeof(IChunk).IsAssignableFrom(type) && typeof(IVersionable).IsAssignableFrom(type)))
        {
            var metadata = type.GetCustomAttribute<ChunkGameVersionAttribute>();
            if (metadata is null) continue;
            if (metadata.Version.Contains(0) && !metadata.Version.Any(version => version > 0)) missing.Add(type.FullName!);
        }
        await Assert.That(missing).IsEmpty();
    }

    [Test]
    [Arguments(0, GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    [Arguments(2, GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    [Arguments(3, GameVersion.TM2020)]
    [Arguments(9, GameVersion.TM2020)]
    public async Task GameVersion_OlderChunkVersions_StayValidUpToEachGamesMaximum(int version, GameVersion expected)
    {
        var chunk = new GBX.NET.Engines.Game.CGameCtnGhost.Chunk03092000 { Version = version };

        await Assert.That(chunk.GameVersion).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0x03122003u, 24)]
    [Arguments(0x0303600Cu, 3)]
    [Arguments(0x03078005u, 2)]
    [Arguments(0x03121001u, 3)]
    [Arguments(0x0303F006u, 2)]
    [Arguments(0x090ED0AEu, 3)]
    [Arguments(0x0602102Au, 6)]
    [Arguments(0x031CC004u, 2)]
    [Arguments(0x03043047u, 1)]
    public async Task ReadWrite_VersionAboveNativeMaximum_ThrowsBeforePayload(uint chunkId, int version)
    {
        const int sentinel = 0x12345678;
        var chunk = (Chunk)ClassManager.NewChunk(chunkId)!;
        var node = ClassManager.New(chunkId & 0xFFFFF000)!;
        using var input = new MemoryStream();
        using (var writer = new BinaryWriter(input, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(version);
            writer.Write(sentinel);
        }
        input.Position = 0;
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader))
            Assert.Throws<ChunkVersionNotSupportedException>(() => chunk.ReadWrite(node, rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
            Assert.Throws<ChunkVersionNotSupportedException>(() => chunk.ReadWrite(node, rw));
        await Assert.That(output.Length).IsEqualTo(sizeof(int));
    }
}
