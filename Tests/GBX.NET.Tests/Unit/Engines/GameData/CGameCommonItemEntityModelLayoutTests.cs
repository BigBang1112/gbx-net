using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.GameData;

[Category("Unit")]
public class CGameCommonItemEntityModelLayoutTests
{
    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(2, 0)]
    [Arguments(2, 1)]
    [Arguments(3, 0)]
    [Arguments(3, 1)]
    [Arguments(4, 0)]
    [Arguments(4, 1)]
    [Arguments(5, 0)]
    [Arguments(5, 1)]
    [Arguments(6, 0)]
    [Arguments(6, 1)]
    public async Task Chunk000PreservesNativeVersionBranchesAndExternalReferences(int version, int lightCount)
    {
        var refTable = new GbxRefTable();
        var files = new Dictionary<int, GbxRefTableNode>();
        GbxRefTableFile? phyFile = null, visFile = null, staticFile = null, triggerFile = null, particleFile = null, actionFile = null;
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            GbxRefTableFile Reference(string path)
            {
                var file = new GbxRefTableFile(refTable, 0, true, path);
                var index = files.Count + 1;
                files.Add(index, file);
                writer.Write(index);
                return file;
            }

            writer.Write(version);
            if (version == 0)
            {
                phyFile = Reference("Physics.Gbx");
                visFile = Reference("Visual.Gbx");
            }
            else if (version == 3)
            {
                writer.Write("LegacyMesh.Gbx");
                writer.Write("LegacyShape.Gbx");
            }
            else
            {
                staticFile = Reference("Static.Gbx");
            }

            if (version >= 2)
            {
                triggerFile = Reference("Trigger.Gbx");
                writer.Write(Iso4.Identity);
                particleFile = Reference("Particle.Gbx");
                writer.Write(1);
                actionFile = Reference("Action.Gbx");
                if (version < 6) Reference("Deprecated.Gbx");
                foreach (var sound in new[] { "Spawn", "Unspawn", "Grab", "Smashed", "Permanent" }) writer.Write(sound);
                writer.Write(Iso4.Identity);
                writer.Write(lightCount);
                if (lightCount != 0)
                {
                    writer.Write(0); // SPlugLightBallStateSimple archive version.
                    foreach (var value in new[] { 0.25f, -0.5f, 2f, 3f, 4f, 5f, 6f }) writer.Write(value);
                }
            }
            if (version >= 5) writer.Write((byte)1);
            writer.Write(0x11223344);
        }

        payload.Position = 0;
        var node = new CGameCommonItemEntityModel();
        var chunk = new CGameCommonItemEntityModel.Chunk2E027000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            reader.LoadRefTable(files);
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.PhyModelFile).IsSameReferenceAs(phyFile);
        await Assert.That(node.VisModelFile).IsSameReferenceAs(visFile);
        await Assert.That(node.StaticObjectFile).IsSameReferenceAs(staticFile);
        await Assert.That(node.TriggerShapeFile).IsSameReferenceAs(triggerFile);
        await Assert.That(node.ParticleEmitterModelFile).IsSameReferenceAs(particleFile);
        if (version >= 2)
        {
            await Assert.That(node.Actions![0].File).IsSameReferenceAs(actionFile);
            await Assert.That(node.SpawnLoc).IsEqualTo(Iso4.Identity);
            await Assert.That(node.SoundRefSpawn).IsEqualTo("Spawn");
            await Assert.That(node.SoundRefPermanent).IsEqualTo("Permanent");
            await Assert.That(node.SoundLocPermanent).IsEqualTo(Iso4.Identity);
            await Assert.That(node.LightBallStates!.Length).IsEqualTo(lightCount);
            if (lightCount != 0)
            {
                await Assert.That(node.LightBallStates[0].Version).IsEqualTo(0);
                await Assert.That(node.LightBallStates[0].U02).IsEqualTo(-0.5f);
                await Assert.That(node.LightBallStates[0].U07).IsEqualTo(6f);
            }
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(0x11223344);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
