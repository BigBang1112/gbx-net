using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugMaterialUserInstLayoutTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(4, false)]
    [Arguments(5, false)]
    [Arguments(6, false)]
    [Arguments(7, false)]
    [Arguments(8, false)]
    [Arguments(9, false)]
    [Arguments(10, false)]
    [Arguments(11, false)]
    [Arguments(11, true)]
    public async Task ReadWrite_Material_PreservesVersionedLinksAndShaderParameters(int version, bool fullLink)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 11) w.Write((byte)(fullLink ? 1 : 0));
            w.WriteIdAsString("Material");
            w.WriteIdAsString("TDSN");
            w.Write("Textures/Base");
            w.Write((byte)1);
            if (version >= 10) w.Write((byte)2);
            if (version >= 1)
            {
                if (version == 9 || version == 10 || fullLink) w.Write("Stadium/Platform");
                else w.WriteIdAsString("Platform");
            }
            if (version >= 2)
            {
                w.Write(1);
                w.WriteIdAsString("Color");
                w.WriteIdAsString("Real");
                w.Write(2);
                w.Write(2);
                w.Write(0x3F800000);
                w.Write(unchecked((int)0xBF000000));
            }
            if (version >= 3)
            {
                w.Write(1);
                w.WriteIdAsString("Diffuse");
                w.WriteIdAsString("Trans");
                w.Write(2.5f);
                w.Write(0x3F80000040000000UL);
                if (version >= 5) w.WriteIdAsString("Time");
            }
            if (version >= 4)
            {
                w.Write(1);
                w.WriteIdAsString("Visible");
            }
            if (version >= 6)
            {
                w.Write(1);
                w.Write(2); // Legacy layer numbering before version 8 is retained on the wire.
                w.Write("Textures/Normal");
            }
            if (version >= 7) w.WriteIdAsString("Group");
        });
        var node = new CPlugMaterialUserInst();
        var chunk = new CPlugMaterialUserInst.Chunk090FD000();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Name).IsEqualTo("Material");
        await Assert.That(node.Model).IsEqualTo("TDSN");
        if (version >= 1)
        {
            await Assert.That(node.Link).IsEqualTo(version == 9 || version == 10 || fullLink ? "Stadium/Platform" : "Platform");
        }
        if (version >= 2)
        {
            await Assert.That(node.Csts![0].Name).IsEqualTo("Color");
            await Assert.That(node.Csts[0].Type).IsEqualTo("Real");
            await Assert.That(node.Csts[0].ValueCount).IsEqualTo(2);
            await Assert.That(node.CstValues!).IsEquivalentTo(new[] { 0x3F800000, unchecked((int)0xBF000000) }, CollectionOrdering.Matching);
        }
        if (version >= 3)
        {
            await Assert.That(node.UvAnims![0].Layer).IsEqualTo("Diffuse");
            await Assert.That(node.UvAnims[0].Type).IsEqualTo("Trans");
            await Assert.That(node.UvAnims[0].Period).IsEqualTo(2.5f);
            await Assert.That(node.UvAnims[0].Data).IsEqualTo(0x3F80000040000000UL);
        }
        if (version >= 5) await Assert.That(node.UvAnims![0].InputVal).IsEqualTo("Time");
        if (version >= 4) await Assert.That(node.VisibleIds![0]).IsEqualTo("Visible");
        if (version >= 6)
        {
            await Assert.That(node.UserTextures![0].Layer).IsEqualTo(2);
            await Assert.That(node.UserTextures[0].Texture).IsEqualTo("Textures/Normal");
        }
        if (version >= 7) await Assert.That(node.HidingGroup).IsEqualTo("Group");
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task ReadWrite_TextureSettings_PreservesLegacyWidthsAndVersionBoundaries(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(-1); // Null texture atlas.
            if (version == 1)
            {
                w.Write(true);
                w.Write(2.5f);
            }
            if (version >= 2)
            {
                w.Write(1);
                w.Write(2);
                w.Write(2.5f);
                if (version == 2) w.Write(7.5f);
            }
            if (version >= 4) w.Write(4);
            if (version >= 5) w.Write(true);
        });
        var node = new CPlugMaterialUserInst();
        await Assert.That(node.TextureSizeInMeters).IsEqualTo(1f);
        var chunk = new CPlugMaterialUserInst.Chunk090FD001();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.TextureAtlas).IsNull();
        await Assert.That(node.TextureSizeInMeters).IsEqualTo(version >= 1 ? 2.5f : 1f);
        if (version >= 2)
        {
            await Assert.That(node.TilingU).IsEqualTo(CPlugMaterialUserInst.ETexAddress.Mirror);
            await Assert.That(node.TilingV).IsEqualTo(CPlugMaterialUserInst.ETexAddress.Clamp);
        }
        await Assert.That(node.IsNatural).IsEqualTo(version >= 5);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var writer = new GbxWriter(input)) writer.Write(0x11223344);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            serialize(rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
