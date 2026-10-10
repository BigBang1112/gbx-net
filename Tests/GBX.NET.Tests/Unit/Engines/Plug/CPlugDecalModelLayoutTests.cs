using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDecalModelLayoutTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(4, false)]
    [Arguments(5, false)]
    [Arguments(5, true)]
    [Arguments(6, false)]
    [Arguments(6, true)]
    public async Task TexturesPreserveVersionBranchesAndExternalReferences(int version, bool paths)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        if (version < 5)
        {
            payload.Reference("Diffuse.Gbx");
            payload.Reference("Normal.Gbx");
        }
        if (version >= 1) writer.Write(128f);
        if (version >= 2) writer.Write(false);
        if (version >= 3) writer.Write(0.25f);
        if (version == 4) payload.Reference("Specular.Gbx");
        if (version >= 5)
        {
            foreach (var name in new[] { "Diffuse", "Normal", "Specular" })
            {
                writer.Write(paths ? name + ".dds" : "");
                if (!paths) payload.Reference(name + ".Gbx");
            }
        }
        if (version >= 6)
        {
            writer.Write(paths ? "Roughness.dds" : "");
            if (!paths) payload.Reference("Roughness.Gbx");
        }
        payload.Finish();

        var node = new CPlugDecalModel();
        var chunk = new CPlugDecalModel.Chunk090A7002();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.TexelByMeter).IsEqualTo(version >= 1 ? 128f : 256f);
        await Assert.That(node.FadeNormalAndZ).IsEqualTo(version < 2);
        await Assert.That(node.MaxAngleNCos).IsEqualTo(version >= 3 ? 0.25f : MathF.Cos(1.4835298f));
        if (!paths)
        {
            await Assert.That(node.DiffuseAFile!.FilePath).IsEqualTo("Diffuse.Gbx");
            await Assert.That(node.NormalFile!.FilePath).IsEqualTo("Normal.Gbx");
            if (version >= 4) await Assert.That(node.SpecularFile!.FilePath).IsEqualTo("Specular.Gbx");
            if (version >= 6) await Assert.That(node.RoughnessFile!.FilePath).IsEqualTo("Roughness.Gbx");
        }
        else
        {
            await Assert.That(node.DiffuseARef).IsEqualTo("Diffuse.dds");
            await Assert.That(node.NormalRef).IsEqualTo("Normal.dds");
            await Assert.That(node.SpecularRef).IsEqualTo("Specular.dds");
            if (version >= 6) await Assert.That(node.RoughnessRef).IsEqualTo("Roughness.dds");
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    [Arguments(5, false)]
    [Arguments(6, false)]
    [Arguments(7, false)]
    [Arguments(8, false)]
    [Arguments(8, true)]
    public async Task SpriteAndMacroDecalsPreserveTheirNativePayload(int version, bool svgPath)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        payload.Reference("Solid.Gbx");
        writer.Write(false); // Random instances.
        if (version >= 2) writer.Write(0.75f);
        if (version >= 3)
        {
            payload.Reference("Sprite.Gbx");
            writer.WriteIdAsString("SpriteGroup");
        }
        if (version >= 4)
        {
            writer.Write(svgPath ? "Decal.svg" : "");
            if (!svgPath) payload.Reference("Svg.Gbx");
        }
        if (version >= 5) writer.Write(2f);
        if (version >= 6) writer.Write(0.5f);
        if (version >= 7) writer.Write(30);
        if (version >= 8)
        {
            writer.WriteDeprecVersion();
            writer.Write(1); // Decal models.
            payload.Reference("Decal.Gbx");
            writer.Write(1); // Macro sets.
            writer.WriteIdAsString("Set");
            writer.Write(new BoxAligned(1, 2, 3, 4, 5, 6));
            writer.Write(1); // Decals in the set.
            writer.Write(0); // Model-table index.
            writer.WriteIdAsString("SpriteGroup");
            writer.Write(2.5f); // Uniform scale.
            writer.Write(7.5f); // Editing-plane X.
            writer.Write(-8.5f); // Editing-plane Z.
        }
        payload.Finish();

        var node = new CPlugDecalModel();
        var chunk = new CPlugDecalModel.Chunk090A7004();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SolidFile!.FilePath).IsEqualTo("Solid.Gbx");
        await Assert.That(node.RandomInstances).IsFalse();
        await Assert.That(node.MaxAngleN3dCos).IsEqualTo(version >= 2 ? 0.75f : MathF.Cos(1.0471976f));
        if (version >= 3)
        {
            await Assert.That(node.Sprite3dBitmapFile!.FilePath).IsEqualTo("Sprite.Gbx");
            await Assert.That(node.Sprite3dGroupId).IsEqualTo("SpriteGroup");
        }
        if (version >= 4)
        {
            if (svgPath) await Assert.That(node.SvgRef).IsEqualTo("Decal.svg");
            else await Assert.That(node.SvgFile!.FilePath).IsEqualTo("Svg.Gbx");
        }
        await Assert.That(node.SvgSize).IsEqualTo(version >= 5 ? 2f : 1f);
        await Assert.That(node.SvgAlpha).IsEqualTo(version >= 6 ? 0.5f : 1f);
        if (version >= 8)
        {
            await Assert.That(node.DecalModels.Single().File!.FilePath).IsEqualTo("Decal.Gbx");
            var set = node.MacroDecalSets.Single();
            await Assert.That(set.Id).IsEqualTo("Set");
            await Assert.That(set.Bounds).IsEqualTo(new BoxAligned(1, 2, 3, 4, 5, 6));
            var decal = set.Decals.Single();
            await Assert.That(decal.DecalModelIndex).IsEqualTo(0);
            await Assert.That(decal.SpriteGroupId).IsEqualTo("SpriteGroup");
            await Assert.That(decal.Scale).IsEqualTo(2.5f);
            await Assert.That(decal.Position).IsEqualTo(new Vec2(7.5f, -8.5f));
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task LegacyChunksReadThroughTheNodeDispatcherAndPreserveFlagBits()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x090A7000u);
            writer.Write(-1); // Diffuse.
            writer.Write(-1); // Normal.
            writer.Write(0x090A7001u);
            writer.Write(-1); // Solid.
            writer.Write(0x090A7005u);
            writer.Write(0); // Version.
            writer.Write(0.35f);
            writer.Write(0x090A7006u);
            writer.Write(0); // Version.
            writer.Write(0x80000001u);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugDecalModel>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.GetChunk<CPlugDecalModel.Chunk090A7000>()).IsNotNull();
        await Assert.That(node.GetChunk<CPlugDecalModel.Chunk090A7001>()).IsNotNull();
        await Assert.That(node.ImpactSize).IsEqualTo(0.35f);
        await Assert.That(node.IsObsolete).IsTrue();
        node.IsObsolete = false;
        await Assert.That(node.Flags).IsEqualTo(0x80000000u);
        node.IsObsolete = true;
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
