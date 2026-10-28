using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Managers;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class LegacyClassIdentityTests
{
    [Test]
    [Arguments(GameVersion.TMF)]
    [Arguments(GameVersion.Unspecified)]
    public async Task Parse_ReusedClassId_PreservesTheSelectedRootAndEmbeddedNode(GameVersion game)
    {
        var node = (CMwNod)ClassManager.New(0x030C0000, game)!;
        var expected = game == GameVersion.TMF ? typeof(CGameControlCardNetOnlineNews) : typeof(CGameCtnMediaBlockLightmap);
        await Assert.That(node.GetType()).IsEqualTo(expected);
        await Assert.That(ClassManager.Wrap(0x240B6000)).IsEqualTo(0x030C0000U); // TMU card ID
        using var file = new MemoryStream();
        node.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(file);
        file.Position = 0;
        var gbx = Gbx.Parse(file, new GbxReadSettings { GameVersion = game });
        await Assert.That(gbx.Node!.GetType()).IsEqualTo(expected);
        await Assert.That(gbx.GetType().GenericTypeArguments.Single()).IsEqualTo(expected);

        using var data = new MemoryStream();
        using (var writer = new GbxWriter(data))
        {
            writer.Write(1); // Embedded node index.
            writer.Write(0x030C0000U);
            writer.Write(0xFACADE01U);
        }
        data.Position = 0;
        using var reader = new GbxReader(data, new GbxReadSettings { GameVersion = game });
        await Assert.That(reader.ReadNodeRef<CMwNod>(null).Resolve()!.GetType()).IsEqualTo(expected);
    }

    [Test]
    public async Task ReadWrite_MwStack_PreservesTypesBeforeValuesAndNamedEntries()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(4);
            writer.Write(0);
            writer.Write(1);
            writer.Write(2);
            writer.Write(0x10000002);
            writer.Write(0x03043001);
            writer.Write(7);
            writer.Write(true);
            writer.Write("Player");
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var stack = new MwStack();
        using (var reader = new GbxReader(payload))
        {
            stack.Read(reader);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(stack.Values[0].Number).IsEqualTo(0x03043001);
        await Assert.That(stack.Values[1].Number).IsEqualTo(7);
        await Assert.That(stack.Values[2].Name).IsEqualTo("Player");
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) stack.Write(writer);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_GameSkinDirectory_PreservesItsPathBeforeTheMipMapFlag()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x03009000U);
            writer.Write("Menus");
            writer.Write(false); // Directory rather than a file reference.
            writer.Write("Menu\\Menu\\");
            writer.Write(true); // Mip maps.
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var fid = new CPlugGameSkin.Fid();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            fid.ReadWrite(rw, 4);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(fid.Directory).IsEqualTo("Menu\\Menu\\");
        await Assert.That(fid.MipMaps).IsTrue();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) fid.ReadWrite(rw, 4);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }
}
