using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockInfoVariantTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(4)]
    [Arguments(8)]
    [Arguments(0x10)]
    [Arguments(0x20)]
    [Arguments(0x3F)]
    [Arguments(0x7FFF)]
    [Arguments(0x8000)]
    [Arguments(0xFFFF)]
    public async Task ReadWrite_Flags_PreserveAllSixteenBits(int flags)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write((ushort)flags);
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B004();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.Flags).IsEqualTo((ushort)flags);
        await Assert.That(node.IsObsoleteVariant).IsEqualTo((flags & 1) != 0);
        await Assert.That(node.IsFakeReplacement).IsEqualTo((flags & 2) != 0);
        await Assert.That(node.AutoChangeVariantOff).IsEqualTo((flags & 4) != 0);
        await Assert.That(node.DontPlaySound1).IsEqualTo((flags & 8) != 0);
        await Assert.That(node.DontPlaySound2).IsEqualTo((flags & 0x10) != 0);
        await Assert.That(node.IsNoPillarBelowVariant).IsEqualTo((flags & 0x20) != 0);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task FlagProperties_Setters_PreserveOtherBits(int bit)
    {
        var node = new CGameCtnBlockInfoVariant { Flags = 0xFFC0 };
        Action<bool>[] setters =
        [
            value => node.IsObsoleteVariant = value,
            value => node.IsFakeReplacement = value,
            value => node.AutoChangeVariantOff = value,
            value => node.DontPlaySound1 = value,
            value => node.DontPlaySound2 = value,
            value => node.IsNoPillarBelowVariant = value
        ];

        setters[bit](true);
        await Assert.That(node.Flags).IsEqualTo((ushort)(0xFFC0 | (1 << bit)));

        node.Flags = ushort.MaxValue;
        setters[bit](false);
        await Assert.That(node.Flags).IsEqualTo((ushort)(ushort.MaxValue & ~(1 << bit)));
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    public async Task ReadWrite_MobilsAndHelpers_PreserveVersionedPayload(int version, bool hasHelper)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            var mobil = new CGameCtnBlockInfoMobil();
            writer.Write(version);
            writer.Write(3);
            writer.Write(2);
            writer.WriteNodeRef(mobil);
            writer.WriteNodeRef(mobil);
            writer.Write(0);
            writer.Write(1);
            writer.WriteNodeRef(mobil);
            if (version <= 1)
            {
                writer.WriteNodeRef(hasHelper ? new CSceneMobil() : null);
                writer.WriteNodeRef(hasHelper ? null : new CSceneMobil());
            }
            else
            {
                writer.WriteNodeRef(hasHelper ? new CPlugSolid() : null);
                writer.WriteNodeRef(hasHelper ? null : new CPlugSolid());
            }
            if (version >= 3)
            {
                writer.WriteNodeRef(hasHelper ? new CPlugPrefab { Url = "helper-prefab" } : null);
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B005();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.Mobils?.Length).IsEqualTo(3);
        await Assert.That(node.Mobils![0].Count).IsEqualTo(2);
        await Assert.That(node.Mobils[1].Count).IsEqualTo(0);
        await Assert.That(node.Mobils[2].Count).IsEqualTo(1);
        await Assert.That(node.Mobils[0][0]).IsNotNull();
        await Assert.That(node.Mobils[0][0]).IsSameReferenceAs(node.Mobils[0][1]);
        await Assert.That(node.Mobils[0][0]).IsSameReferenceAs(node.Mobils[2][0]);
        await Assert.That(node.HelperMobil is not null).IsEqualTo(version <= 1 && hasHelper);
        await Assert.That(node.FacultativeHelperMobil is not null).IsEqualTo(version <= 1 && !hasHelper);
        await Assert.That(node.HelperSolidFid is not null).IsEqualTo(version >= 2 && hasHelper);
        await Assert.That(node.FacultativeHelperSolidFid is not null).IsEqualTo(version >= 2 && !hasHelper);
        await Assert.That(node.HelperPrefabFid?.Url).IsEqualTo(version >= 3 && hasHelper ? "helper-prefab" : null);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_HelperFids_PreserveExternalDescriptors(bool resource)
    {
        var node = new CGameCtnBlockInfoVariant();
        node.CreateChunk<CGameCtnBlockInfoVariant.Chunk0315B005>().Version = 3;
        var table = new GbxRefTable();
        node.References.HelperSolidFid.Descriptor = new GbxRefTableFile(table, 0, true, "helper.Solid.Gbx");
        node.References.FacultativeHelperSolidFid.Descriptor = new GbxRefTableFile(table, 0, true, "facultative.Solid.Gbx");
        node.References.HelperPrefabFid.Descriptor = resource
            ? new GbxRefTableResource(table, 4, true, 42)
            : new GbxRefTableFile(table, 0, true, "helper.Prefab.Gbx");

        using var stream = new MemoryStream();
        node.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);

        for (var pass = 0; pass < 2; pass++)
        {
            stream.Position = 0;
            var parsed = Gbx.Parse(stream);
            var restored = (CGameCtnBlockInfoVariant)parsed.Node!;
            await Assert.That(restored.References.HelperSolidFid.File?.FilePath).IsEqualTo("helper.Solid.Gbx");
            await Assert.That(restored.References.FacultativeHelperSolidFid.File?.FilePath).IsEqualTo("facultative.Solid.Gbx");
            await Assert.That(restored.References.HelperPrefabFid.Kind).IsEqualTo(resource ? NodeRefKind.ExternalResource : NodeRefKind.ExternalFile);
            await Assert.That(restored.References.HelperPrefabFid.State).IsEqualTo(NodeRefState.Unresolved);
            await Assert.That(restored.References.HelperPrefabFid.CachedNode).IsNull();
            if (resource)
            {
                await Assert.That(restored.References.HelperPrefabFid.Resource?.ResourceIndex).IsEqualTo(42);
            }
            else
            {
                await Assert.That(restored.References.HelperPrefabFid.File?.FilePath).IsEqualTo("helper.Prefab.Gbx");
            }

            stream.SetLength(0);
            parsed.Save(stream);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    public async Task ReadWrite_InteractiveModels_PreserveVersionedPayload(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version <= 8) writer.WriteNodeRef(new CPlugSolid());
            writer.WriteNodeRef(new CPlugSolid());
            writer.Write(-1);
            if (version >= 11)
            {
                writer.WriteNodeRef(new CPlugSurface());
                writer.Write(-1);
            }
            if (version >= 1 && version <= 8) writer.Write(17);
            if (version >= 2)
            {
                if (version <= 3) writer.WriteNodeRef(new CPlugSurface());
                else writer.WriteNodeRef(new CGameGateModel());
            }
            if (version >= 3)
            {
                if (version == 3) writer.WriteNodeRef(new CPlugSurface());
                else writer.WriteNodeRef(new CGameTeleporterModel());
            }
            if (version >= 5) writer.WriteNodeRef(new CGameCaptureZoneModel());
            if (version >= 6) writer.WriteNodeRef(new CGameTurbineModel());
            if (version >= 7) writer.Write(-1);
            if (version >= 8) writer.WriteNodeRef(new CGameSpawnModel());
            if (version >= 10)
            {
                writer.Write(1);
                writer.WriteNodeRef(new CPlugEntitySpawner());
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B006();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.ScreenInteractionTriggerSolid).IsNotNull();
        await Assert.That(node.WaypointTriggerSolid).IsNull();
        await Assert.That(node.ScreenInteractionTriggerShape is not null).IsEqualTo(version >= 11);
        await Assert.That(node.WaypointTriggerShape).IsNull();
        await Assert.That(node.Gate is not null).IsEqualTo(version >= 4);
        await Assert.That(node.Teleporter is not null).IsEqualTo(version >= 4);
        await Assert.That(node.CaptureZone is not null).IsEqualTo(version >= 5);
        await Assert.That(node.Turbine is not null).IsEqualTo(version >= 6);
        await Assert.That(node.FlockEmitter).IsNull();
        await Assert.That(node.SpawnModel is not null).IsEqualTo(version >= 8);
        if (version >= 10) await Assert.That(node.EntitySpawners?.Count).IsEqualTo(1);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_FlockEmitterState_PreservesNativeFieldOrder(int version)
    {
        var matrix = new Mat3(1, 2, 3, 4, 5, 6, 7, 8, 9);
        var position = new Vec3(11, 12, 13);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(21f);
            writer.Write(22f);
            writer.Write(23);
            writer.Write(true);
            writer.Write(false);
            if (version >= 1) writer.Write(matrix);
            writer.Write(position);
            writer.Write(Sentinel);
        }

        var state = new CGameCtnBlockInfoVariant.FlockEmitterState();
        await Verify(payload.ToArray(), rw => state.ReadWrite(rw));
        await Assert.That(state.Radius).IsEqualTo(21f);
        await Assert.That(state.Power).IsEqualTo(22f);
        await Assert.That(state.SpawnCount).IsEqualTo(23);
        await Assert.That(state.IsRepulsor).IsTrue();
        await Assert.That(state.IsLandingArea).IsFalse();
        await Assert.That(state.Matrix).IsEqualTo(version >= 1 ? matrix : Mat3.Identity);
        await Assert.That(state.Position).IsEqualTo(position);
    }

    [Test]
    public async Task Constructor_FlockEmitterState_UsesNativeDefaults()
    {
        var state = new CGameCtnBlockInfoVariant.FlockEmitterState();
        await Assert.That(state.Version).IsEqualTo(1);
        await Assert.That(state.Radius).IsEqualTo(10f);
        await Assert.That(state.Power).IsEqualTo(2f);
        await Assert.That(state.SpawnCount).IsEqualTo(5);
        await Assert.That(state.IsRepulsor).IsFalse();
        await Assert.That(state.IsLandingArea).IsTrue();
        await Assert.That(state.Matrix).IsEqualTo(Mat3.Identity);
        await Assert.That(state.Position).IsEqualTo(default(Vec3));
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    public async Task ReadWrite_CompoundModel_PreservesVersionedPayload(int version, bool hasModel)
    {
        var location = new Iso4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version <= 1)
            {
                writer.WriteNodeRef(hasModel ? new CGameObjectPhyModel() : null);
                writer.WriteNodeRef(hasModel ? new CGameObjectVisModel() : null);
                if (version >= 1) writer.Write(location);
            }
            else
            {
                writer.WriteNodeRef(hasModel ? new CGameObjectPhyCompoundModel() : null);
                if (version <= 2 || hasModel) writer.Write(location);
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B00A();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(chunk.U01 is not null).IsEqualTo(version <= 1 && hasModel);
        await Assert.That(chunk.U02 is not null).IsEqualTo(version <= 1 && hasModel);
        await Assert.That(chunk.U03).IsEqualTo(version == 1 ? location : (Iso4?)null);
        await Assert.That(node.CompoundModel is not null).IsEqualTo(version >= 2 && hasModel);
        await Assert.That(node.CompoundLoc).IsEqualTo(version >= 2 && (version <= 2 || hasModel) ? location : Iso4.Identity);
    }

    [Test]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    public async Task ReadWrite_CompoundModel_PreservesExternalReferenceAndLocation(int version, bool resource)
    {
        var location = Iso4.Identity with { TX = 7, TY = 8, TZ = 9 };
        var node = new CGameCtnBlockInfoVariant { CompoundLoc = location };
        node.CreateChunk<CGameCtnBlockInfoVariant.Chunk0315B00A>().Version = version;
        var table = new GbxRefTable();
        node.References.CompoundModel.Descriptor = resource
            ? new GbxRefTableResource(table, 4, true, 42)
            : new GbxRefTableFile(table, 0, true, "compound.GameObjectPhyCompoundModel.Gbx");

        using var stream = new MemoryStream();
        node.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);

        for (var pass = 0; pass < 2; pass++)
        {
            stream.Position = 0;
            var parsed = Gbx.Parse(stream);
            var restored = (CGameCtnBlockInfoVariant)parsed.Node!;
            await Assert.That(restored.CompoundLoc).IsEqualTo(location);
            await Assert.That(restored.References.CompoundModel.Kind).IsEqualTo(resource ? NodeRefKind.ExternalResource : NodeRefKind.ExternalFile);
            await Assert.That(restored.References.CompoundModel.State).IsEqualTo(NodeRefState.Unresolved);
            await Assert.That(restored.References.CompoundModel.CachedNode).IsNull();
            if (resource)
            {
                await Assert.That(restored.References.CompoundModel.Resource?.ResourceIndex).IsEqualTo(42);
            }
            else
            {
                await Assert.That(restored.References.CompoundModel.File?.FilePath).IsEqualTo("compound.GameObjectPhyCompoundModel.Gbx");
            }

            stream.SetLength(0);
            parsed.Save(stream);
        }
    }

    [Test]
    public async Task Constructor_CompoundLoc_UsesNativeIdentityDefault()
    {
        var node = new CGameCtnBlockInfoVariant();
        await Assert.That(node.CompoundLoc).IsEqualTo(Iso4.Identity);
        await Assert.That(node.References.CompoundModel.Kind).IsEqualTo(NodeRefKind.None);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 1)]
    [Arguments(0, 3)]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments(1, 3)]
    public async Task ReadWrite_PillarParams_PreserveVersionedPayload(int version, int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            var blockInfo = new CGameCtnBlockInfoClassic();
            writer.Write(version);
            writer.Write(count);
            for (var index = 0; index < count; index++)
            {
                writer.WriteNodeRef(index == 2 ? null : blockInfo);
                writer.Write(new Int3(index + 11, index + 21, index + 31));
                writer.Write(index + 1);
            }
            if (version >= 1)
            {
                writer.Write(count);
                for (var index = 0; index < count; index++)
                {
                    writer.WriteNodeRef(index == 2 ? null : blockInfo);
                    writer.Write(new Int3(index + 41, index + 51, index + 61));
                    writer.Write(index + 5);
                    writer.Write(index == 1, asByte: true);
                }
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B009();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.PlacedPillarParams?.Length).IsEqualTo(count);
        await Assert.That(node.ReplacedPillarParams?.Length).IsEqualTo(version >= 1 ? count : (int?)null);
        for (var index = 0; index < count; index++)
        {
            var placed = node.PlacedPillarParams![index];
            await Assert.That(placed.BlockInfo is not null).IsEqualTo(index != 2);
            await Assert.That(placed.Offset).IsEqualTo(new Int3(index + 11, index + 21, index + 31));
            await Assert.That(placed.Dir).IsEqualTo((CGameCtnBlockInfoVariant.ECardinalDir)(index + 1));
            if (index == 1)
            {
                await Assert.That(placed.BlockInfo).IsSameReferenceAs(node.PlacedPillarParams[0].BlockInfo);
            }
            if (version >= 1)
            {
                var replaced = node.ReplacedPillarParams![index];
                await Assert.That(replaced.BlockInfo).IsSameReferenceAs(placed.BlockInfo);
                await Assert.That(replaced.Offset2D).IsEqualTo(new Int3(index + 41, index + 51, index + 61));
                await Assert.That(replaced.MultiDir).IsEqualTo((CGameCtnBlockInfoVariant.EMultiDir)(index + 5));
                await Assert.That(replaced.IsOnFlyingBase).IsEqualTo(index == 1);
            }
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_PillarParams_PreserveExternalBlockInfo(bool resource)
    {
        var placed = new CGameCtnBlockInfoVariant.PlacedPillarParam
        {
            Offset = new Int3(1, 2, 3),
            Dir = CGameCtnBlockInfoVariant.ECardinalDir.West
        };
        var replaced = new CGameCtnBlockInfoVariant.ReplacedPillarParam
        {
            Offset2D = new Int3(4, 5, 6),
            MultiDir = CGameCtnBlockInfoVariant.EMultiDir.PreviousDirOnly,
            IsOnFlyingBase = true
        };
        var table = new GbxRefTable();
        GbxRefTableNode descriptor = resource
            ? new GbxRefTableResource(table, 4, true, 42)
            : new GbxRefTableFile(table, 0, true, "pillar.BlockInfo.Gbx");
        placed.References.BlockInfo.Descriptor = descriptor;
        replaced.References.BlockInfo.Descriptor = descriptor;
        var node = new CGameCtnBlockInfoVariant
        {
            PlacedPillarParams = [placed],
            ReplacedPillarParams = [replaced]
        };
        node.CreateChunk<CGameCtnBlockInfoVariant.Chunk0315B009>().Version = 1;

        using var stream = new MemoryStream();
        node.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);

        for (var pass = 0; pass < 2; pass++)
        {
            stream.Position = 0;
            var parsed = Gbx.Parse(stream);
            var restored = (CGameCtnBlockInfoVariant)parsed.Node!;
            var restoredPlaced = restored.PlacedPillarParams!.Single();
            var restoredReplaced = restored.ReplacedPillarParams!.Single();
            await Assert.That(restoredPlaced.Offset).IsEqualTo(placed.Offset);
            await Assert.That(restoredPlaced.Dir).IsEqualTo(placed.Dir);
            await Assert.That(restoredReplaced.Offset2D).IsEqualTo(replaced.Offset2D);
            await Assert.That(restoredReplaced.MultiDir).IsEqualTo(replaced.MultiDir);
            await Assert.That(restoredReplaced.IsOnFlyingBase).IsTrue();
            foreach (var reference in new[] { restoredPlaced.References.BlockInfo, restoredReplaced.References.BlockInfo })
            {
                await Assert.That(reference.Kind).IsEqualTo(resource ? NodeRefKind.ExternalResource : NodeRefKind.ExternalFile);
                await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
                await Assert.That(reference.CachedNode).IsNull();
                if (resource)
                {
                    await Assert.That(reference.Resource?.ResourceIndex).IsEqualTo(42);
                }
                else
                {
                    await Assert.That(reference.File?.FilePath).IsEqualTo("pillar.BlockInfo.Gbx");
                }
            }

            stream.SetLength(0);
            parsed.Save(stream);
        }
    }

    [Test]
    [Arguments((byte)0)]
    [Arguments((byte)1)]
    [Arguments(byte.MaxValue)]
    public async Task ReadWrite_ReplacedPillarParam_UsesOneByteBoolean(byte value)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(-1);
            writer.Write(new Int3(1, 2, 3));
            writer.Write((int)CGameCtnBlockInfoVariant.EMultiDir.PreviousDirOnly);
            writer.Write(value);
            writer.Write(Sentinel);
        }

        var expected = payload.ToArray();
        expected[^5] = value == 0 ? (byte)0 : (byte)1;
        var replaced = new CGameCtnBlockInfoVariant.ReplacedPillarParam();
        await Verify(payload.ToArray(), rw => replaced.ReadWrite(rw), expected);
        await Assert.That(replaced.Offset2D).IsEqualTo(new Int3(1, 2, 3));
        await Assert.That(replaced.MultiDir).IsEqualTo(CGameCtnBlockInfoVariant.EMultiDir.PreviousDirOnly);
        await Assert.That(replaced.IsOnFlyingBase).IsEqualTo(value != 0);
    }

    [Test]
    public async Task Constructor_PillarParams_UseNativeDefaults()
    {
        var placed = new CGameCtnBlockInfoVariant.PlacedPillarParam();
        var replaced = new CGameCtnBlockInfoVariant.ReplacedPillarParam();
        await Assert.That(placed.BlockInfo).IsNull();
        await Assert.That(placed.Offset).IsEqualTo(Int3.Zero);
        await Assert.That(placed.Dir).IsEqualTo(CGameCtnBlockInfoVariant.ECardinalDir.North);
        await Assert.That(replaced.BlockInfo).IsNull();
        await Assert.That(replaced.Offset2D).IsEqualTo(Int3.Zero);
        await Assert.That(replaced.MultiDir).IsEqualTo(CGameCtnBlockInfoVariant.EMultiDir.SameDir);
        await Assert.That(replaced.IsOnFlyingBase).IsFalse();
    }

    [Test]
    public async Task PillarParams_LegacyAliases_ForwardToNamedMembers()
    {
        var blockInfo = new CGameCtnBlockInfoClassic();
#pragma warning disable CS0618
        var placed = new CGameCtnBlockInfoVariant.PlacedPillarParam
        {
            U01 = blockInfo,
            U02 = 1,
            U03 = 2,
            U04 = 3,
            U05 = 3
        };
        var replaced = new CGameCtnBlockInfoVariant.ReplacedPillarParam
        {
            U01 = blockInfo,
            U02 = 4,
            U03 = 5,
            U04 = 6,
            U05 = (int)CGameCtnBlockInfoVariant.EMultiDir.PreviousDirOnly,
            U06 = byte.MaxValue
        };
        await Assert.That(placed.BlockInfo).IsSameReferenceAs(blockInfo);
        await Assert.That(placed.Offset).IsEqualTo(new Int3(1, 2, 3));
        await Assert.That(placed.Dir).IsEqualTo(CGameCtnBlockInfoVariant.ECardinalDir.West);
        await Assert.That(replaced.BlockInfo).IsSameReferenceAs(blockInfo);
        await Assert.That(replaced.Offset2D).IsEqualTo(new Int3(4, 5, 6));
        await Assert.That(replaced.MultiDir).IsEqualTo(CGameCtnBlockInfoVariant.EMultiDir.PreviousDirOnly);
        await Assert.That(replaced.IsOnFlyingBase).IsTrue();

        placed.Offset = new Int3(11, 12, 13);
        placed.Dir = CGameCtnBlockInfoVariant.ECardinalDir.East;
        replaced.Offset2D = new Int3(14, 15, 16);
        replaced.MultiDir = CGameCtnBlockInfoVariant.EMultiDir.AllDir;
        replaced.IsOnFlyingBase = false;
        await Assert.That(placed.U01).IsSameReferenceAs(blockInfo);
        await Assert.That(placed.U02).IsEqualTo(11);
        await Assert.That(placed.U03).IsEqualTo(12);
        await Assert.That(placed.U04).IsEqualTo(13);
        await Assert.That(placed.U05).IsEqualTo(1);
        await Assert.That(replaced.U01).IsSameReferenceAs(blockInfo);
        await Assert.That(replaced.U02).IsEqualTo(14);
        await Assert.That(replaced.U03).IsEqualTo(15);
        await Assert.That(replaced.U04).IsEqualTo(16);
        await Assert.That(replaced.U05).IsEqualTo(2);
        await Assert.That(replaced.U06).IsEqualTo((byte)0);
#pragma warning restore CS0618
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize, byte[]? expectedPayload = null)
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
        await Assert.That(output.ToArray().SequenceEqual(expectedPayload ?? payload)).IsTrue();
    }
}
