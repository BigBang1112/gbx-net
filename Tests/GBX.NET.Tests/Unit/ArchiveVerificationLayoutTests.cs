using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class ArchiveVerificationLayoutTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    public async Task SurfaceVersionZero_CompoundOmitsJointsButRetainsPrimitiveMaterialIndices()
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Chunk version.
            w.Write(13); // Compound.
            w.Write(1); // Child surfaces.
            w.Write(6); // Box.
            for (var i = 0; i < 6; i++) w.Write((float)i);
            w.Write((short)0); // Primitive material index.
            for (var i = 0; i < 12; i++) w.Write(i is 0 or 4 or 8 ? 1f : 0f);
            // No compound-joint array in surface version zero.
            w.Write(1); // Materials.
            w.Write(0); // Built-in material.
            w.Write((short)CPlugSurface.MaterialId.Grass);
            w.Write(0); // Legacy material data.
        });
        var node = new CPlugSurface();
        var chunk = new CPlugSurface.Chunk0900C003();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(((CPlugSurface.Compound)node.Surf!).Surfs.Length).IsEqualTo(1);
        await Assert.That(node.Materials.Single().SurfaceId).IsEqualTo((CPlugSurface.MaterialId?)CPlugSurface.MaterialId.Grass);
    }

    [Test]
    public async Task ScoresTable_VersionFourStoresColumnsWithoutLegacyAppearance()
    {
        var payload = Payload(w => { w.Write(4); w.Write(0); });
        var node = new CGameModulePlaygroundScoresTableModel();
        var chunk = new CGameModulePlaygroundScoresTableModel.Chunk2E017000();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Columns).IsNotNull();
        await Assert.That(node.Columns!.Length).IsEqualTo(0);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task MaterialSurface_VersionZeroStillContainsPackedSurfaceFlags(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write((byte)2);
            w.Write((byte)3);
            w.Write((short)0x4567);
            if (version >= 1)
            {
                w.Write(17);
                w.Write(23);
                w.Write(3);
                w.Write(Encoding.UTF8.GetBytes("abc"));
            }
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079017();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.U03).IsEqualTo((short)0x4567);
        await Assert.That((int)node.SurfaceId).IsEqualTo(2);
    }

    [Test]
    [Arguments(2)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    public async Task MaterialShader_OlderVersionsDoNotReadColorTargetTables(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1);
            w.Write(CPlugMaterial.Id);
            w.Write(0xFACADE01u);
            if (version >= 6) w.Write(0);
            if (version >= 7) w.Write(-1);
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079015();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Shader).IsTypeOf<CPlugMaterial>();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(4)]
    public async Task MaterialWithoutShader_PreservesLegacyConstantVersionBranches(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(-1);
            w.Write(0); // Device materials.
            w.Write(0); // Indices.
            if (version <= 1)
            {
                w.Write(1); // Constants.
                w.Write(3); // Id format.
                w.Write(-1); // Empty constant name.
                w.Write(2);
                w.Write(1.5f);
                w.Write(2.5f);
                if (version >= 1) w.Write(0); // Additional values.
            }
            if (version >= 3) w.Write(17);
            if (version == 4) w.Write(23);
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079015();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        if (version <= 1)
        {
            await Assert.That(node.LegacyConstants![0].Values![1]).IsEqualTo(2.5f);
        }
    }

    [Test]
    public async Task CampaignMedalValues_AreSixIntegersWithoutAnIdPrefix()
    {
        var payload = Payload(w => { for (var i = 1; i <= 6; i++) w.Write(i); });
        var node = new CGameCtnCampaign();
        var chunk = new CGameCtnCampaign.Chunk03090002();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.BronzeMedalValue).IsEqualTo(1);
        await Assert.That(node.GoldCompleteValue).IsEqualTo(6);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task TextClassIds_AreHexadecimalForNodeAndMetaReferences(bool meta)
    {
        using var input = new MemoryStream(Encoding.ASCII.GetBytes($"T{(meta ? "" : "1\r\n")}904f000\r\nFACADE01\r\n1234\r\n"));
        using var reader = new GbxReader(input);
        reader.ReadFormatByte();

        var node = meta ? reader.ReadMetaRef<CPlugTree>() : reader.ReadNodeRef<CPlugTree>();

        await Assert.That(node).IsNotNull();
        await Assert.That(reader.ReadInt32()).IsEqualTo(1234);
        await Assert.That(input.Position).IsEqualTo(input.Length);
    }

    [Test]
    public async Task TextShaderMaterial_ReadsRawBinaryPayloadAndRestoresTextFormat()
    {
        using var input = new MemoryStream();
        using (var writer = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write((byte)'T');
            for (var i = 1; i <= 21; i++) writer.Write(i + 0.5f);
            writer.Write(0x12345678u);
            writer.Write(Encoding.ASCII.GetBytes("1234\r\n"));
        }
        input.Position = 0;
        using var reader = new GbxReader(input);
        reader.ReadFormatByte();
        var material = new CPlugShaderGeneric.SMaterial();
        using (var rw = new GbxReaderWriter(reader)) material.ReadWrite(rw, 2);

        await Assert.That(material.DiffuseAlpha).IsEqualTo(7.5f);
        await Assert.That(material.Flags).IsEqualTo(0x12345678u);
        await Assert.That(reader.ReadInt32()).IsEqualTo(1234);
        await Assert.That(input.Position).IsEqualTo(input.Length);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ResourceData_SizeIncludesItsOwnFourByteHeader(bool absent)
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Empty resource name.
            w.Write(absent ? 0 : 7);
            if (!absent) w.Write(new byte[] { 4, 5, 6 });
        });
        var entry = new CGameResources.ResourceData();

        await Verify(payload, rw => entry.ReadWrite(rw));
        await Assert.That(entry.Data?.Length ?? 0).IsEqualTo(absent ? 0 : 3);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var data = new MemoryStream();
        using var writer = new BinaryWriter(data);
        write(writer);
        writer.Write(Sentinel);
        return data.ToArray();
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
