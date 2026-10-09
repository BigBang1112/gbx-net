using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameCtnPrecalcPartParamsChunkTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    public async Task NativeLayout_PreservesVersionsAndExternalReferences(int version, bool collision)
    {
        var collisionFile = collision ? new GbxRefTableFile(new(), 0, true, "Collision.Surface.Gbx") : null;
        var decalFile = new GbxRefTableFile(new(), 0, true, "Particle.Decal.Gbx");
        var files = new Dictionary<int, GbxRefTableNode> { [collision ? 2 : 1] = decalFile };
        if (collisionFile is not null) files.Add(1, collisionFile);
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(0x0311C000u);
            writer.Write(version);
            writer.Write(0.125f); // FluidFriction.
            writer.Write(2f); // Mass.
            writer.Write(0.25f); // Friction.
            writer.Write(0.75f); // Restitution.
            writer.Write(0x80000008u); // CollGroupFlags.
            writer.Write(false); // LeafMotion, stored as a 32-bit boolean.
            writer.Write(3f); // LeafMotionFreq precedes LeafMotionScale.
            writer.Write(4f);
            if (version >= 2) writer.Write(collision ? 1 : -1);
            writer.Write(42); // IterCount.
            writer.Write(0.02f); // TimeStep.
            writer.Write(123); // RandValSeed.
            writer.Write(new Vec3(1, -2, 3)); // PhyZoneGravity precedes PhyZoneWind.
            writer.Write(new Vec3(4, 5, 6));
            writer.Write(collision ? 2 : 1);
            writer.Write(0.4f); // DecalSizeBase.
            writer.Write(0.2f); // DecalSizeVar.
            writer.Write(0.1f); // DecalBoxDepth.
            writer.Write(0xFACADE01u);
            writer.Write(0xDEADBEEFu);
        }

        input.Position = 0;
        using var reader = new GbxReader(input);
        reader.LoadRefTable(files);
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnPrecalcPartParams();
        node.Read(rw);
        var payloadLength = input.Position;
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);
        await Assert.That(node.GetChunk<CGameCtnPrecalcPartParams.Chunk0311C000>()!.Version).IsEqualTo(version);
        await Assert.That(node.FluidFriction).IsEqualTo(0.125f);
        await Assert.That(node.Mass).IsEqualTo(2f);
        await Assert.That(node.Friction).IsEqualTo(0.25f);
        await Assert.That(node.Restitution).IsEqualTo(0.75f);
        await Assert.That(node.CollGroupFlags).IsEqualTo(0x80000008u);
        await Assert.That(node.LeafMotion).IsFalse();
        await Assert.That(node.LeafMotionFreq).IsEqualTo(3f);
        await Assert.That(node.LeafMotionScale).IsEqualTo(4f);
        await Assert.That(node.CollSurfFile).IsEqualTo(collisionFile);
        await Assert.That(node.IterCount).IsEqualTo(42);
        await Assert.That(node.TimeStep).IsEqualTo(0.02f);
        await Assert.That(node.RandValSeed).IsEqualTo(123);
        await Assert.That(node.PhyZoneGravity).IsEqualTo(new Vec3(1, -2, 3));
        await Assert.That(node.PhyZoneWind).IsEqualTo(new Vec3(4, 5, 6));
        await Assert.That(node.DecalModelFile).IsEqualTo(decalFile);
        await Assert.That(node.DecalSizeBase).IsEqualTo(0.4f);
        await Assert.That(node.DecalSizeVar).IsEqualTo(0.2f);
        await Assert.That(node.DecalBoxDepth).IsEqualTo(0.1f);

        using var output = new MemoryStream();
        using var outputWriter = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(outputWriter);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray().Take((int)payloadLength), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Defaults_MatchNativeConstructor()
    {
        var node = new CGameCtnPrecalcPartParams();
        await Assert.That(node.FluidFriction).IsEqualTo(0.5f);
        await Assert.That(node.Mass).IsEqualTo(1f);
        await Assert.That(node.Friction).IsEqualTo(0.8f);
        await Assert.That(node.Restitution).IsEqualTo(0.2f);
        await Assert.That(node.CollGroupFlags).IsEqualTo(8u);
        await Assert.That(node.LeafMotion).IsTrue();
        await Assert.That(node.LeafMotionFreq).IsEqualTo(10f);
        await Assert.That(node.LeafMotionScale).IsEqualTo(1f);
        await Assert.That(node.IterCount).IsEqualTo(500);
        await Assert.That(node.TimeStep).IsEqualTo(0.01f);
        await Assert.That(node.RandValSeed).IsEqualTo(15000);
        await Assert.That(node.PhyZoneGravity).IsEqualTo(new Vec3(0, -4, 0));
        await Assert.That(node.PhyZoneWind).IsEqualTo(new Vec3(0, 0, 2));
        await Assert.That(node.DecalSizeBase).IsEqualTo(0.3f);
        await Assert.That(node.DecalSizeVar).IsEqualTo(0.1f);
        await Assert.That(node.DecalBoxDepth).IsEqualTo(0.05f);
        await Assert.That(new CGameCtnPrecalcPartParams.Chunk0311C000(GameVersion.TMT).Version).IsEqualTo(2);
        await Assert.That(new CGameCtnPrecalcPartParams.Chunk0311C000().GameVersion).IsEqualTo(GameVersion.TMT);
    }

    [Test]
    public async Task BlockInfo_ReadsTypedInlineParticleParameters()
    {
        var parameters = new CGameCtnPrecalcPartParams { Mass = 2, IterCount = 42 };
        parameters.CreateChunk<CGameCtnPrecalcPartParams.Chunk0311C000>().Version = 2;
        var location = Iso4.Identity with { TX = 7, TY = 8, TZ = 9 };
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(0x0304E015u);
            writer.WriteNodeRef(parameters);
            writer.Write(location);
            writer.Write(0xFACADE01u);
        }

        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnBlockInfoClassic();
        node.Read(rw);
        await Assert.That(node.PrecalcPartParams).IsNotNull();
        await Assert.That(node.PrecalcPartParams!.Mass).IsEqualTo(2f);
        await Assert.That(node.PrecalcPartParams.IterCount).IsEqualTo(42);
        await Assert.That(node.PrecalcPartLoc).IsEqualTo(location);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var outputWriter = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(outputWriter);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }
}
