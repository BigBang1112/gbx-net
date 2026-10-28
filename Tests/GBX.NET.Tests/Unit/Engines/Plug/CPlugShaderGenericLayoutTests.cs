using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugShaderGenericLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_MaterialChunks_PreserveFieldOrderAndFlags(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugShaderGeneric.Id + (uint)offset);
            for (var i = 1; i <= 21; i++) writer.Write(i + 0.25f);
            writer.Write(0xABCD0A55u);
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        CPlugShaderGeneric node;
        using (var reader = new GbxReader(payload))
        {
            node = reader.ReadNode<CPlugShaderGeneric>()!;
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }

        var material = node.Material!;
        await Assert.That(material.AmbientRGB).IsEqualTo(new Vec3(1.25f, 2.25f, 3.25f));
        await Assert.That(material.DiffuseRGB).IsEqualTo(new Vec3(4.25f, 5.25f, 6.25f));
        await Assert.That(material.DiffuseAlpha).IsEqualTo(7.25f);
        await Assert.That(material.SpecularRGB).IsEqualTo(new Vec3(8.25f, 9.25f, 10.25f));
        await Assert.That(material.SpecularExp).IsEqualTo(11.25f);
        await Assert.That(material.EmissiveRGB).IsEqualTo(new Vec3(12.25f, 13.25f, 14.25f));
        await Assert.That(material.EmissiveModifier).IsEqualTo(new Vec3(15.25f, 16.25f, 17.25f));
        await Assert.That(material.WireRGB).IsEqualTo(new Vec3(18.25f, 19.25f, 20.25f));
        await Assert.That(material.WireAlpha).IsEqualTo(21.25f);
        await Assert.That(material.Flags).IsEqualTo(0xABCD0A55u);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);

#pragma warning disable GBXNET10001
        var clone = (CPlugShaderGeneric)node.DeepClone();
#pragma warning restore GBXNET10001
        clone.Material!.DiffuseAlpha = 0;
        await Assert.That(material.DiffuseAlpha).IsEqualTo(7.25f);
    }

    [Test]
    [Arguments(GameVersion.Unspecified, false)]
    [Arguments(GameVersion.TM10, true)]
    [Arguments(GameVersion.TMPU, true)]
    [Arguments(GameVersion.TMSX, true)]
    [Arguments(GameVersion.TMNESWC, true)]
    [Arguments(GameVersion.VSK5, true)]
    [Arguments(GameVersion.TMF, true)]
    public async Task Constructor_MaterialDefaults_MatchNativeConstruction(GameVersion gameVersion, bool legacy)
    {
        var material = new CPlugShaderGeneric(gameVersion).Material!;
        var color = legacy ? Vec3.One : Vec3.Zero;
        await Assert.That(material.AmbientRGB).IsEqualTo(color);
        await Assert.That(material.DiffuseRGB).IsEqualTo(color);
        await Assert.That(material.SpecularRGB).IsEqualTo(color);
        await Assert.That(material.EmissiveRGB).IsEqualTo(color);
        await Assert.That(material.EmissiveModifier).IsEqualTo(Vec3.Zero);
        await Assert.That(material.WireRGB).IsEqualTo(color);
        await Assert.That(material.DiffuseAlpha).IsEqualTo(legacy ? 1f : 0f);
        await Assert.That(material.WireAlpha).IsEqualTo(legacy ? 1f : 0f);
        await Assert.That(material.SpecularExp).IsEqualTo(legacy ? 20f : 0f);
        await Assert.That(material.Flags).IsEqualTo(legacy ? 5u : 0x800u);
    }

    [Test]
    public async Task LightingProperties_WhenChanged_PreserveOtherFlagBits()
    {
        var material = new CPlugShaderGeneric.SMaterial { Flags = 0xFFFFFC00 };
        material.UseAmbient = true;
        material.UseAmbientColorVertex = true;
        material.UseDiffuse = true;
        material.UseDiffuseColorVertex = true;
        material.UseSpecular = true;
        material.UseEmissive = true;
        material.UseEmissiveColorVertex = true;
        material.NormalizeNs = true;
        material.OnlyInfiniteLights = true;
        material.DblSidedLighting = true;
        await Assert.That(material.Flags).IsEqualTo(uint.MaxValue);
        material.UseDiffuseColorVertex = false;
        material.OnlyInfiniteLights = false;
        await Assert.That(material.Flags).IsEqualTo(uint.MaxValue & ~0x108u);
    }
}
