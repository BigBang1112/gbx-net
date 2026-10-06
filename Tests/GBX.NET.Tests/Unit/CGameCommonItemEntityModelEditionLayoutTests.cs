using GBX.NET.Attributes;
using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;
using System.Reflection;

namespace GBX.NET.Tests.Unit;

public class CGameCommonItemEntityModelEditionLayoutTests
{
    public static IEnumerable<(int Version, CGameCommonItemEntityModelEdition.EItemType ItemType, bool Details)> Versions()
    {
        for (var version = 0; version <= 8; version++)
        {
            foreach (var itemType in new[]
            {
                CGameCommonItemEntityModelEdition.EItemType.Undefined,
                CGameCommonItemEntityModelEdition.EItemType.Ornament,
                CGameCommonItemEntityModelEdition.EItemType.PickUp,
                CGameCommonItemEntityModelEdition.EItemType.Spot
            })
            {
                yield return (version, itemType, false);
                yield return (version, itemType, true);
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(Versions))]
    public async Task Chunk000ReadsAndRoundTripsNativeBranches(int version, CGameCommonItemEntityModelEdition.EItemType itemType, bool details)
    {
        var files = new Dictionary<int, GbxRefTableNode>();
        var payload = Payload(version, itemType, details, 5, files);
        var node = new CGameCommonItemEntityModelEdition();
        var chunk = new CGameCommonItemEntityModelEdition.Chunk2E026000();
        Read(payload, files, node, chunk);

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.ItemType).IsEqualTo(itemType);
        await Assert.That(node.MeshCrystalFile!.FilePath).IsEqualTo("Mesh.Gbx");
        await Assert.That(node.Images!.Length).IsEqualTo(details ? 2 : 0);
        await Assert.That(node.SpriteParams!.Length).IsEqualTo(details ? 1 : 0);
        await Assert.That(node.LightBallStates!.Length).IsEqualTo(details ? 1 : 0);
        await Assert.That(node.DestroyParticleModelFile!.FilePath).IsEqualTo("Destroy.Gbx");
        await Assert.That(node.LocAnimFile!.FilePath).IsEqualTo("LocAnim.Gbx");
        await Assert.That(node.ParticleModelAliveRef).IsEqualTo(details ? "Alive.Particle.Gbx" : "");
        await Assert.That(node.SoundRefPermanent).IsEqualTo("Permanent");
        await Assert.That(node.SoundLocPermanent.Translation).IsEqualTo(new Vec3(4, 5, 6));
        await Assert.That(node.DynaPointModel is not null).IsEqualTo(details);
        await Assert.That(node.Program).IsEqualTo(CGameObjectPhyModel.EProgram.Turret);
        await Assert.That(node.UseMeshAsHitShape).IsEqualTo(!details);
        if (details)
        {
            await Assert.That(node.Images[0].ImageFile!.FilePath).IsEqualTo("Image.Gbx");
            await Assert.That(node.Images[1].Ref).IsEqualTo("Skin.dds");
            await Assert.That(node.SpriteParams[0].U01).IsEqualTo(new Vec3(1, 2, 3));
            await Assert.That(node.LightBallStates[0].Version).IsEqualTo(0);
            await Assert.That(node.LightBallStates[0].U08).IsEqualTo(7f);
            await Assert.That(node.ParticleModelAlivePos).IsEqualTo(new Vec3(7, 8, 9));
            await Assert.That(node.CustomHitShapeCrystalFile!.FilePath).IsEqualTo("Hit.Gbx");
        }
        if (itemType == CGameCommonItemEntityModelEdition.EItemType.PickUp)
        {
            await Assert.That(node.PickupActionModelFile!.FilePath).IsEqualTo("Pickup.Gbx");
            await Assert.That(node.Mass).IsEqualTo(version >= 3 ? 2.5f : 1f);
            if (version >= 7) await Assert.That(chunk.U04).IsEqualTo(unchecked((int)0x87654321));
        }
        if (itemType == CGameCommonItemEntityModelEdition.EItemType.Spot)
        {
            await Assert.That(node.TriggeredActions!.Length).IsEqualTo(details ? 1 : 0);
            await Assert.That(node.TriggerActionVersion).IsEqualTo(5);
            await Assert.That(node.Triggers!.Length).IsEqualTo(details ? 1 : 0);
            if (details) await Assert.That(node.TriggeredActions[0].File!.FilePath).IsEqualTo("Triggered.Gbx");
        }
        if (version < 4) await Assert.That(node.SpawnLoc.Translation).IsEqualTo(new Vec3(10, 11, 12));
        if (version >= 1)
        {
            await Assert.That(node.InventoryName).IsEqualTo("Inventory");
            await Assert.That(node.InventoryOccupation).IsEqualTo(1234);
        }
        await RoundTrip(payload, node, chunk);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task SpotUsesTheSerializedTriggerVersion(int triggerVersion)
    {
        var files = new Dictionary<int, GbxRefTableNode>();
        var payload = Payload(8, CGameCommonItemEntityModelEdition.EItemType.Spot, true, triggerVersion, files);
        var node = new CGameCommonItemEntityModelEdition();
        var chunk = new CGameCommonItemEntityModelEdition.Chunk2E026000();
        Read(payload, files, node, chunk);
        await Assert.That(node.TriggerActionVersion).IsEqualTo(triggerVersion);
        await Assert.That(node.Triggers!.Length).IsEqualTo(1);
        await RoundTrip(payload, node, chunk);
    }

    [Test]
    public async Task Chunk001PreservesItsSignedValueAndSkippableWriterMetadata()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0);
            writer.Write(unchecked((int)0xFEDCBA98));
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var chunk = new CGameCommonItemEntityModelEdition.Chunk2E026001();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(new(), rw);
            await Assert.That(chunk.U01).IsEqualTo(unchecked((int)0xFEDCBA98));
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }
        await Assert.That(chunk.Ignore).IsFalse();
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
        await Assert.That(chunk.GetType().GetCustomAttribute<ChunkGameVersionAttribute>()!.Version).IsEmpty();
    }

    [Test]
    public async Task DefaultsMatchNativeConstructors()
    {
        var node = new CGameCommonItemEntityModelEdition();
        var light = new CGameCommonItemEntityModelEdition.LightBallStateSimple();
        await Assert.That(node.Mass).IsEqualTo(1f);
        await Assert.That(node.UseMeshAsMoveShape).IsTrue();
        await Assert.That(node.UseMeshAsHitShape).IsTrue();
        await Assert.That(node.UseMeshAsTriggerShape).IsTrue();
        await Assert.That(node.InventoryOccupation).IsEqualTo(1000);
        await Assert.That(node.SoundLocPermanent).IsEqualTo(Iso4.Identity);
        await Assert.That(node.SpawnLoc).IsEqualTo(Iso4.Identity);
        await Assert.That(light.U05).IsEqualTo(1f);
        await Assert.That(light.U08).IsEqualTo(1f);
    }

    private static byte[] Payload(int version, CGameCommonItemEntityModelEdition.EItemType itemType,
        bool details, int triggerVersion, Dictionary<int, GbxRefTableNode> files)
    {
        var refTable = new GbxRefTable();
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        void Reference(string path)
        {
            var index = files.Count + 1;
            files.Add(index, new GbxRefTableFile(refTable, 0, true, path));
            writer.Write(index);
        }

        writer.Write(version);
        writer.Write((int)itemType);
        Reference("Mesh.Gbx");
        writer.Write(details ? "LegacySolid.Gbx" : "");
        if (!details) Reference("Solid.Gbx");
        writer.Write(details ? 2 : 0);
        if (details)
        {
            writer.Write("");
            Reference("Image.Gbx");
            writer.Write("Skin.dds");
        }
        writer.Write(details ? 1 : 0);
        if (details)
        {
            writer.Write(new Vec3(1, 2, 3));
            writer.Write(true);
            writer.Write(0.25f);
        }
        Reference("Destroy.Gbx");
        Reference("LocAnim.Gbx");
        writer.Write(details ? 1 : 0);
        if (details)
        {
            writer.Write(0); // SPlugLightBallStateSimple archive version.
            for (var i = 1; i <= 7; i++) writer.Write((float)i);
        }
        writer.Write(details ? "Alive.Particle.Gbx" : "");
        if (details) writer.Write(new Vec3(7, 8, 9));
        writer.Write(details ? "Smash.Particle.Gbx" : "");
        if (details) writer.WriteIdAsString(null);
        foreach (var sound in new[] { "Spawn", "Unspawn", "Grab", "Smashed", "Permanent" }) writer.Write(sound);
        writer.Write(Iso4.Identity with { TX = 4, TY = 5, TZ = 6 });
        if (version < 5 && (version >= 3 || itemType is CGameCommonItemEntityModelEdition.EItemType.Ornament or CGameCommonItemEntityModelEdition.EItemType.Spot))
        {
            writer.Write(!details);
            if (details) Reference("Move.Gbx");
        }
        if (version >= 3 && itemType == CGameCommonItemEntityModelEdition.EItemType.PickUp) writer.Write(2.5f);
        writer.Write(!details);
        if (details) Reference("Hit.Gbx");
        if (itemType == CGameCommonItemEntityModelEdition.EItemType.PickUp)
        {
            writer.Write(!details);
            if (details) Reference("TriggerShape.Gbx");
            Reference("Pickup.Gbx");
        }
        if (itemType == CGameCommonItemEntityModelEdition.EItemType.Spot)
        {
            writer.Write(details ? 1 : 0);
            if (details) Reference("Triggered.Gbx");
            writer.Write(triggerVersion);
            writer.Write(details ? 1 : 0);
            if (details)
            {
                if (triggerVersion >= 5) writer.Write(7);
                if (triggerVersion >= 4) for (var i = 0; i < 4; i++) writer.Write(0.5f + i);
                if (triggerVersion >= 3) writer.Write(3);
                if (triggerVersion >= 2) writer.Write(4);
                if (triggerVersion >= 1)
                {
                    writer.Write(5);
                    writer.Write(6);
                }
                for (var i = 0; i < 6; i++) writer.Write(1.5f + i);
                writer.WriteIdAsString("Trigger");
            }
        }
        writer.Write(details);
        if (details)
        {
            writer.Write(0); // CPlugDynaPointModel archive version.
            foreach (var value in new[] { 1f, 2f, 3f, 0.5f, 0.8f, 0.5f, 1f }) writer.Write(value);
        }
        writer.Write((int)CGameObjectPhyModel.EProgram.Turret);
        if (version < 4) writer.Write(Iso4.Identity with { TX = 10, TY = 11, TZ = 12 });
        if (version >= 1)
        {
            writer.Write("Inventory");
            writer.Write("Description");
            writer.Write(2);
            writer.Write(1234);
        }
        if (version < 2) Reference("DeprecatedMesh.Gbx");
        if (version is 6 or 7) Reference("Deprecated.Gbx");
        if (version >= 7 && itemType == CGameCommonItemEntityModelEdition.EItemType.PickUp) writer.Write(unchecked((int)0x87654321));
        writer.Write(0x11223344);
        return stream.ToArray();
    }

    private static void Read(byte[] payload, Dictionary<int, GbxRefTableNode> files,
        CGameCommonItemEntityModelEdition node, CGameCommonItemEntityModelEdition.Chunk2E026000 chunk)
    {
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        reader.LoadRefTable(files);
        chunk.ReadWrite(node, rw);
        if (reader.ReadInt32() != 0x11223344 || stream.Position != stream.Length)
        {
            throw new InvalidDataException("Chunk did not consume the native payload exactly.");
        }
    }

    private static async Task RoundTrip(byte[] payload, CGameCommonItemEntityModelEdition node,
        CGameCommonItemEntityModelEdition.Chunk2E026000 chunk)
    {
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(0x11223344);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
