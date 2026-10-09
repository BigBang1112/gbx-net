using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockInfoMobilLinkTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    public async Task ReadWrite_ReadsNativeArchiveAndPreservesReferences(int version, bool externalVis)
    {
        var phy = new CGameObjectPhyModel();
        var vis = new CGameObjectVisModel();
        var visFile = externalVis ? new GbxRefTableFile(new GbxRefTable(), 0, true, "Visual.Gbx") : null;
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            writer.WriteIdAsString("Socket");

            if (version == 0)
            {
                writer.WriteNodeRef(phy);
                writer.WriteNodeRef(visFile is null ? vis : null, visFile);
            }
            else
            {
                var model = new CGameObjectModel { Phy = phy, Vis = vis };
                model.CreateChunk<CGameObjectModel.Chunk2E01D000>();
                writer.WriteNodeRef(model);
            }
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        if (visFile is not null)
        {
            reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [2] = visFile });
        }
        using var readerWriter = new GbxReaderWriter(reader);
        var restored = new CGameCtnBlockInfoMobilLink();
        restored.ReadWrite(readerWriter);

        await Assert.That(restored.Version).IsEqualTo(version);
        await Assert.That(restored.SocketId).IsEqualTo("Socket");
        await Assert.That(restored.Model).IsNotNull();
        await Assert.That(restored.Model!.Phy).IsNotNull();
        await Assert.That(restored.Model.VisFile).IsEqualTo(visFile);
        if (externalVis)
        {
            await Assert.That(restored.Model.Vis).IsNull();
        }
        else
        {
            await Assert.That(restored.Model.Vis).IsNotNull();
        }
        await Assert.That(stream.Position).IsEqualTo(stream.Length);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            restored.ReadWrite(writerWriter);
        }

        await Assert.That(rewritten.ToArray().SequenceEqual(stream.ToArray())).IsTrue();
    }

    [Test]
    public async Task Version0_CreatesModelForNullReferences()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(0);
            writer.WriteIdAsString("Socket");
            writer.Write(-1);
            writer.Write(-1);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var readerWriter = new GbxReaderWriter(reader);
        var restored = new CGameCtnBlockInfoMobilLink();
        restored.ReadWrite(readerWriter);

        await Assert.That(restored.Model).IsNotNull();
        await Assert.That(restored.Model!.Phy).IsNull();
        await Assert.That(restored.Model.Vis).IsNull();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task NewLink_WritesVersion1WithOneNullModelReference()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            new CGameCtnBlockInfoMobilLink { SocketId = "Socket" }.ReadWrite(writerWriter);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadInt32()).IsEqualTo(1);
        await Assert.That(reader.ReadIdAsString()).IsEqualTo("Socket");
        await Assert.That(reader.ReadInt32()).IsEqualTo(-1);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }
}
