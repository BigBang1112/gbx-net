using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.GameData;

[Category("Unit")]
public class CGameObjectVisModelLayoutTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    [Arguments(5, false)]
    [Arguments(5, true)]
    [Arguments(6, false)]
    [Arguments(6, true)]
    [Arguments(7, false)]
    [Arguments(7, true)]
    [Arguments(8, false)]
    [Arguments(8, true)]
    [Arguments(9, false)]
    [Arguments(9, true)]
    [Arguments(10, false)]
    [Arguments(10, true)]
    [Arguments(11, false)]
    [Arguments(11, true)]
    [Arguments(12, false)]
    [Arguments(12, true)]
    [Arguments(13, false)]
    [Arguments(13, true)]
    [Arguments(14, false)]
    [Arguments(14, true)]
    [Arguments(15, false)]
    [Arguments(15, true)]
    [Arguments(16, false)]
    [Arguments(16, true)]
    [Arguments(17, false)]
    [Arguments(17, true)]
    [Arguments(18, false)]
    [Arguments(18, true)]
    [Arguments(19, false)]
    [Arguments(19, true)]
    [Arguments(20, false)]
    [Arguments(20, true)]
    [Arguments(21, false)]
    [Arguments(21, true)]
    [Arguments(22, false)]
    [Arguments(22, true)]
    public async Task MainChunkPreservesNativeBranchesAndNonemptyArrays(int version, bool paths)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        if (version == 1)
        {
            writer.Write(123);
            writer.Write(456);
            writer.Write(7.5f);
        }
        if (version < 8) payload.Reference("LegacyMesh.Gbx");
        if (version >= 9)
        {
            writer.Write(paths ? "Mesh.Gbx" : "");
            if (!paths) payload.Reference("Mesh.Gbx");
        }
        if (version < 18) payload.Reference("Deprecated.Anim.Gbx");
        if (version >= 2)
        {
            if (version < 9) writer.Write(paths ? "Mesh.Gbx" : "");
            if (version < 5)
            {
                writer.Write(2);
                WriteInlineAnimation(writer, 0);
                WriteInlineAnimation(writer, 4);
            }
            else if (version == 5)
            {
                writer.Write(2);
                payload.Reference("AnimationA.Gbx");
                payload.Reference("AnimationB.Gbx");
            }
            else payload.Reference("Animation.Gbx");

            writer.Write(1);
            writer.Write(0); // SPlugLightBallStateSimple archive version.
            for (var i = 0; i < 7; i++) writer.Write(i + 0.5f);
        }
        if (version >= 3 && version <= 16)
        {
            writer.Write("Spawn");
            writer.Write("Unspawn");
            writer.Write("Grab");
        }
        if (version >= 10)
        {
            writer.Write(paths ? "Alive.Particle.Gbx" : "");
            if (version >= 11 && paths)
            {
                writer.Write(1.5f);
                writer.Write(2.5f);
                writer.Write(3.5f);
            }
        }
        if (version >= 12)
        {
            writer.Write(paths ? "Smash.Particle.Gbx" : "");
            if (paths) writer.WriteIdAsString("Smash");
        }
        if (version == 8 && !paths) payload.Reference("Mesh.Gbx");
        if (version >= 13)
        {
            writer.Write(2); // Image references use a path with a node reference fallback.
            writer.Write("SpriteA.dds");
            writer.Write("");
            payload.Reference("SpriteB.Gbx");
            writer.Write(2); // Inline sprite parameters.
            writer.Write(new Vec3(1, 2, 3));
            writer.Write(true);
            writer.Write(4.5f);
            writer.Write(new Vec3(5, 6, 7));
            writer.Write(false);
            writer.Write(8.5f);
        }
        if (version >= 14)
        {
            writer.Write(paths ? "Solid.Gbx" : "");
            if (!paths) payload.Reference("Solid.Gbx");
        }
        if (version == 15) writer.Write("Version15Only");
        if (version >= 16) payload.Reference("Destroy.Particle.Gbx");
        if (version >= 19) writer.Write(12.5f);
        if (version >= 20)
        {
            writer.Write(paths ? "Effects.Gbx" : "");
            if (!paths) payload.Reference("Effects.Gbx");
        }
        if (version >= 21) payload.Reference("Mesh.Fid.Gbx");
        if (version >= 22) writer.Write(new Vec3(0.25f, 0.5f, 0.75f));
        payload.Finish();

        var node = new CGameObjectVisModel();
        var chunk = new CGameObjectVisModel.Chunk2E007001();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        if (version == 1)
        {
            await Assert.That(chunk.U23).IsEqualTo(123);
            await Assert.That(chunk.U24).IsEqualTo(456);
            await Assert.That(chunk.U25).IsEqualTo(7.5f);
        }
        if (version >= 2)
        {
            await Assert.That(chunk.U03).IsEqualTo(1);
            await Assert.That(node.LightBallStates![0].U07).IsEqualTo(6.5f);
            if (version < 5)
            {
                await Assert.That(node.LegacyLocAnims!.Length).IsEqualTo(2);
                await Assert.That(node.LegacyLocAnims[1].U01).IsEqualTo(123);
                await Assert.That(node.LegacyLocAnims[1].RotFunc).IsEqualTo((byte)2);
            }
            else if (version == 5)
            {
                await Assert.That(node.LegacyLocAnimRefs!.Length).IsEqualTo(2);
                await Assert.That(node.LegacyLocAnimRefs[1].File!.FilePath).IsEqualTo("AnimationB.Gbx");
            }
            else await Assert.That(node.LocAnimFile!.FilePath).IsEqualTo("Animation.Gbx");
        }
        if (version >= 13)
        {
            await Assert.That(chunk.U08).IsEqualTo(2);
            await Assert.That(chunk.U09).IsEqualTo(2);
            await Assert.That(node.Images![1].ImageFile!.FilePath).IsEqualTo("SpriteB.Gbx");
            await Assert.That(node.SpriteParams![1].U01).IsEqualTo(new Vec3(5, 6, 7));
            await Assert.That(node.SpriteParams[1].U03).IsEqualTo(8.5f);
        }
        if (version == 15) await Assert.That(chunk.U26).IsEqualTo("Version15Only");
        if (version >= 21) await Assert.That(node.MeshShadedFidFile!.FilePath).IsEqualTo("Mesh.Fid.Gbx");
        if (version >= 22) await Assert.That(node.DomeShaderColor).IsEqualTo(new Vec3(0.25f, 0.5f, 0.75f));
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task AnimationNodeIncludesVersion4Tail()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(0); // Outer CPlugAnimLocSimple chunk version.
        WriteInlineAnimation(payload.Writer, 4);
        payload.Finish();
        var node = new CPlugAnimLocSimple();
        var chunk = new CPlugAnimLocSimple.Chunk090F8000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(4);
        await Assert.That(chunk.U02).IsEqualTo(123);
        await Assert.That(node.RotAngle).IsEqualTo(1.5f);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task DeprecatedAndSoundChunksPreserveTheirPayloads()
    {
        var node = new CGameObjectVisModel();
        using (var payload = new ObjectModelPayload())
        {
            payload.Reference("LegacyMesh.Gbx");
            payload.Reference("Deprecated.Anim.Gbx");
            payload.Finish();
            var chunk = new CGameObjectVisModel.Chunk2E007000();
            payload.Read(rw => chunk.ReadWrite(node, rw));
            await Assert.That(node.MeshShadedFile!.FilePath).IsEqualTo("LegacyMesh.Gbx");
            await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        }
        using (var payload = new ObjectModelPayload())
        {
            payload.Writer.Write(0);
            foreach (var sound in new[] { "Spawn", "Unspawn", "Grab", "Smashed", "Permanent" }) payload.Writer.Write(sound);
            payload.Writer.Write(Iso4.Identity);
            payload.Finish();
            var chunk = new CGameObjectVisModel.Chunk2E007002();
            payload.Read(rw => chunk.ReadWrite(node, rw));
            await Assert.That(node.SoundRefPermanent).IsEqualTo("Permanent");
            await Assert.That(node.SoundLocPermanent).IsEqualTo(Iso4.Identity);
            await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        }
        using (var payload = new ObjectModelPayload())
        {
            payload.Reference("Deprecated.Gbx");
            payload.Finish();
            var chunk = new CGameObjectVisModel.Chunk2E007003();
            payload.Read(rw => chunk.ReadWrite(node, rw));
            await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        }
    }

    private static void WriteInlineAnimation(GbxWriter writer, int version)
    {
        writer.Write(version);
        writer.Write(1000);
        writer.Write(2000);
        writer.Write(3.5f);
        if (version >= 1) writer.Write(1);
        if (version >= 2)
        {
            writer.Write(4000);
            writer.Write(5000);
        }
        if (version >= 3)
        {
            writer.Write((byte)2);
            writer.Write(1.5f);
        }
        if (version >= 4) writer.Write(123);
    }
}

