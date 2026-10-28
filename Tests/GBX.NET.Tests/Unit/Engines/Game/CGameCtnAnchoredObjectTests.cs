using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Script;
using GBX.NET.Serialization;
using static GBX.NET.Engines.Script.CScriptTraitsMetadata;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnAnchoredObjectTests
{
    private const int Sentinel = 0x12345678;

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
    public async Task ReadWrite_Placement_PreservesVersionedPayload(int version)
    {
        var position = new Vec3(11, 12, 13);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(new Ident("Item", "Stadium", "Author"));
            if (version <= 1)
            {
                writer.Write((byte)Direction.West);
                if (version == 1) writer.Write((byte)2);
            }
            else writer.Write(new Vec3(0.1f, 0.2f, 0.3f));
            writer.Write(new Byte3(1, 2, 3));
            writer.WriteIdAsString("Anchor");
            writer.Write(position);
            writer.WriteNodeRef<CGameWaypointSpecialProperty>(null);
            if (version is 3 or 4) writer.Write(17);
            if (version >= 4) writer.Write(unchecked((short)0x8003));
            if (version >= 5) writer.Write(new Vec3(4, 5, 6));
            if (version >= 6) writer.Write(2.5f);
            if (version >= 8)
            {
                for (var i = 1; i <= 6; i++) writer.Write(i * 1f);
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnAnchoredObject();
        var chunk = new CGameCtnAnchoredObject.Chunk03101002();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AbsolutePositionInMap).IsEqualTo(position);
        await Assert.That(node.PivotIndex).IsEqualTo(version is 3 or 4 ? 17 : -1);
        await Assert.That(node.Scale).IsEqualTo(version >= 6 ? 2.5f : 1f);
        await Assert.That(node.IsFlying).IsEqualTo(version >= 4);
        await Assert.That(node.IsAutoPiloted).IsEqualTo(version >= 4);
        if (version <= 1)
        {
            var expectedYaw = version == 0 ? MathF.PI / 2 : 2 * MathF.PI / 3;
            await Assert.That(MathF.Abs(node.YawPitchRoll.X - expectedYaw) < 0.000001f).IsTrue();
        }
        await Assert.That(node.BoundingBox).IsEqualTo(version >= 8
            ? new BoxAligned(1, 2, 3, 4, 5, 6)
            : new BoxAligned(0, 0, 0, -1, -1, -1));
    }

    [Test]
    [Arguments(7)]
    [Arguments(8)]
    public async Task Write_PlacementWithSkin_SetsPackDescFlag(int version)
    {
        var node = new CGameCtnAnchoredObject { Flags = unchecked((short)0x8001), PackDesc = new PackDesc("Skin.zip") };
        await Assert.That(node.Flags).IsEqualTo(unchecked((short)0x8005));
        var chunk = new CGameCtnAnchoredObject.Chunk03101002 { Version = version };
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(node.Flags).IsEqualTo(unchecked((short)0x8005));
        stream.Position = 0;
        var restored = new CGameCtnAnchoredObject();
        using var reader = new GbxReader(stream);
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(restored, rw);
        await Assert.That(restored.Flags).IsEqualTo(unchecked((short)0x8005));
        await Assert.That(restored.PackDesc!.FilePath).IsEqualTo("Skin.zip");
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task PackDesc_Set_UpdatesFlagAndPreservesOtherBits()
    {
        var node = new CGameCtnAnchoredObject { Flags = unchecked((short)0x8003) };
        node.PackDesc = new PackDesc("Skin.zip");
        await Assert.That(node.Flags).IsEqualTo(unchecked((short)0x8007));

        node.PackDesc = null;
        await Assert.That(node.Flags).IsEqualTo(unchecked((short)0x8003));
    }

    [Test]
    [Arguments(-1)]
    [Arguments(3)]
    public async Task ReadWrite_BotPathIndex_PreservesPathReference(int botPathIndex)
    {
        var node = new CGameCtnAnchoredObject();
        await Assert.That(node.BotPathIndex).IsEqualTo(-1);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0);
            writer.Write(botPathIndex);
            writer.Write(Sentinel);
        }

        var chunk = new CGameCtnAnchoredObject.Chunk03101004();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.BotPathIndex).IsEqualTo(botPathIndex);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_ScriptParameters_PreservesPrimitiveTypes(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version >= 1) writer.Write((byte)4);
            writer.Write(4);
            for (var i = 0; i < 4; i++)
            {
                writer.Write($"Param{i}");
                if (version == 0) writer.Write(i);
                else writer.Write((byte)(i == 3 ? 5 : i + 1));
                switch (i)
                {
                    case 0: writer.Write(true, asByte: version >= 1); break;
                    case 1: writer.Write(42); break;
                    case 2: writer.Write(2.5f); break;
                    case 3:
                        if (version == 0) writer.Write("Text");
                        else writer.WriteSmallString("Text");
                        break;
                }
            }
            writer.Write(Sentinel);
        }
        var node = new CGameCtnAnchoredObject();
        var chunk = new CGameCtnAnchoredObject.Chunk03101005();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ScriptParameters!.Length).IsEqualTo(4);
        await Assert.That(((ScriptTrait<bool>)node.ScriptParameters[0].Value!).Value).IsTrue();
        await Assert.That(((ScriptTrait<int>)node.ScriptParameters[1].Value!).Value).IsEqualTo(42);
        await Assert.That(((ScriptTrait<float>)node.ScriptParameters[2].Value!).Value).IsEqualTo(2.5f);
        await Assert.That(((ScriptTrait<string>)node.ScriptParameters[3].Value!).Value).IsEqualTo("Text");
    }

    [Test]
    [Arguments(Direction.North, 0f)]
    [Arguments(Direction.East, -1.5707964f)]
    [Arguments(Direction.South, -3.1415927f)]
    [Arguments(Direction.West, 1.5707964f)]
    public async Task ReadWrite_LegacyChunks_PreservesPlacementAndAnchor(Direction dir, float yaw)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(new Ident("LegacyItem", "Stadium", "Author"));
            writer.Write((byte)dir);
            writer.Write(new Byte3(4, 5, 6));
            writer.Write((byte)0x7F);
            writer.WriteIdAsString("Anchor");
            writer.Write(Sentinel);
        }
        var node = new CGameCtnAnchoredObject();
        var placement = new CGameCtnAnchoredObject.Chunk03101000();
        var anchor = new CGameCtnAnchoredObject.Chunk03101001();
        await Verify(payload.ToArray(), rw =>
        {
            placement.ReadWrite(node, rw);
            anchor.ReadWrite(node, rw);
        });
        await Assert.That(MathF.Abs(node.YawPitchRoll.X - yaw) < 0.000001f).IsTrue();
        await Assert.That(node.BlockUnitCoord).IsEqualTo(new Byte3(4, 5, 6));
        await Assert.That(node.AnchorTreeId).IsEqualTo("Anchor");
    }

    [Test]
    public async Task ReadWrite_ScriptParameters_PreservesArrayAndStruct()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(1);
            writer.Write((byte)4);
            writer.Write(2);
            writer.Write("Numbers");
            writer.Write((byte)EScriptType.Array);
            writer.Write((byte)EScriptType.Void);
            writer.Write((byte)EScriptType.Integer);
            writer.WriteSmallLen(2);
            writer.Write(11);
            writer.Write(22);
            writer.Write("Settings");
            writer.Write((byte)EScriptType.Struct);
            writer.Write((byte)1);
            writer.Write("SettingsType");
            writer.Write("Enabled");
            writer.Write((byte)EScriptType.Boolean);
            writer.Write(true, asByte: true);
            writer.Write(Sentinel);
        }
        var node = new CGameCtnAnchoredObject();
        var chunk = new CGameCtnAnchoredObject.Chunk03101005();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        var array = (ScriptArrayTrait)node.ScriptParameters![0].Value!;
        await Assert.That(((ScriptTrait<int>)array.Value[1]).Value).IsEqualTo(22);
        var structure = (ScriptStructTrait)node.ScriptParameters[1].Value!;
        await Assert.That(((ScriptTrait<bool>)structure.Value["Enabled"]).Value).IsTrue();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task LegacyScriptMetadata_UsesExistingWaypoint(bool hasWaypoint, bool hasMetadata)
    {
        var metadata = new CScriptTraitsMetadata();
        metadata.CreateChunk<CScriptTraitsMetadata.Chunk11002000>().Version = 6;
        metadata.Declare("Score", 42);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteEncapsulated(w => w.WriteNode(metadata));
            writer.Write(Sentinel);
        }

        var waypoint = hasWaypoint ? new CGameWaypointSpecialProperty { Tag = "Checkpoint" } : null;
        if (hasMetadata)
        {
            waypoint!.ScriptMetadata = new CScriptTraitsMetadata();
            waypoint.ScriptMetadata.Declare("Score", 7);
        }
        var node = new CGameCtnAnchoredObject { WaypointSpecialProperty = waypoint };
        var chunk = new CGameCtnAnchoredObject.Chunk03101003();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(node.WaypointSpecialProperty!.ScriptMetadata!.GetInteger("Score")).IsEqualTo(hasMetadata ? 7 : 42);
        if (hasWaypoint)
        {
            await Assert.That(node.WaypointSpecialProperty).IsSameReferenceAs(waypoint);
            await Assert.That(node.WaypointSpecialProperty.Tag).IsEqualTo("Checkpoint");
        }
        if (!hasMetadata)
        {
            using var output = new MemoryStream();
            using var writer = new GbxWriter(output);
            using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
            writer.Write(Sentinel);
            await Assert.That(output.ToArray().SequenceEqual(payload.ToArray())).IsTrue();
        }
    }

    [Test]
    public async Task LegacyScriptMetadata_EmptyDoesNotCreateWaypoint()
    {
        var node = new CGameCtnAnchoredObject();
        var chunk = new CGameCtnAnchoredObject.Chunk03101003();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            var metadata = new CScriptTraitsMetadata();
            metadata.CreateChunk<CScriptTraitsMetadata.Chunk11002000>().Version = 6;
            writer.WriteEncapsulated(w => w.WriteNode(metadata));
        }
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(node.WaypointSpecialProperty).IsNull();

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        output.Position = 0;
        using (var reader = new GbxReader(output))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(node.WaypointSpecialProperty).IsNull();
        await Assert.That(output.Position).IsEqualTo(output.Length);
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
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }
}
