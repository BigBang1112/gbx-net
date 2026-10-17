using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnMediaClipGroupLayoutTests
{
    [Test]
    public async Task ReadWrite_CombinedClipEntries_KeepExternalDescriptorsWithoutResolvingDuringWriteOrClone()
    {
        var calls = 0;
        var table = new GbxRefTable();
        table.ExternalNodes["Stored.Clip.Gbx"] = () =>
        {
            calls++;
            return new CGameCtnMediaClip { Name = "Resolved" }.ToGbx();
        };
        var file = new GbxRefTableFile(table, 0, true, "Stored.Clip.Gbx");
        var group = new CGameCtnMediaClipGroup
        {
            Clips =
            [
                CGameCtnMediaClipGroup.ClipTrigger.FromReference(new(), new() { RefCoord = (1, 2, 3) }),
                CGameCtnMediaClipGroup.ClipTrigger.FromReference(new(), new() { RefCoord = (4, 5, 6) })
            ]
        };
        foreach (var pair in group.Clips) pair.References.Clip!.Descriptor = file;

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        var chunk = new CGameCtnMediaClipGroup.Chunk0307A003();
        chunk.Write(group, writer);
#pragma warning disable GBXNET10001
        var clone = (CGameCtnMediaClipGroup)group.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(calls).IsEqualTo(0);
        await Assert.That(group.EnumerateReferences().Count()).IsEqualTo(2);
        await Assert.That(clone.Clips[0].References.Clip!.File!.FilePath).IsEqualTo(file.FilePath);
        await Assert.That(clone.Clips[0].References.Clip!.File).IsSameReferenceAs(clone.Clips[1].References.Clip!.File);
        await Assert.That(clone.Clips[0].Trigger).IsNotSameReferenceAs(group.Clips[0].Trigger);

        output.Position = 0;
        using var reader = new GbxReader(output);
        reader.LoadRefTable(writer.NodeDict.Where(x => x.Key is GbxRefTableNode)
            .ToDictionary(x => x.Value, x => (GbxRefTableNode)x.Key));
        var restored = new CGameCtnMediaClipGroup();
        chunk.Read(restored, reader);
        await Assert.That(calls).IsEqualTo(0);
        await Assert.That(restored.Clips[0].References.Clip!.File).IsSameReferenceAs(restored.Clips[1].References.Clip!.File);
        await Assert.That(restored.Clips[0].Trigger.RefCoord).IsEqualTo(new Int3(1, 2, 3));
        await Assert.That(restored.Clips[1].Clip.Name).IsEqualTo("Resolved");
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(restored.Clips[1].References.Clip!.State).IsEqualTo(NodeRefState.Resolved);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(0, true)]
    [Arguments(1, true)]
    [Arguments(2, true)]
    [Arguments(3, true)]
    public async Task ReadWrite_Groups_KeepClipReferencesAndTriggerOrder(int version, bool hasClips)
    {
        var payload = Payload(w =>
        {
            w.Write(10); // Deprecated clip-list version.
            w.Write(hasClips ? 3 : 0);
            if (hasClips)
            {
                w.Write(-1); // Null clip.
                w.Write(1); // First node reference.
                w.Write(0x03079000u);
                w.Write(0x03079005u);
                w.Write(10); // Deprecated track-list version.
                w.Write(0); // No tracks.
                w.Write(4); // Clip name length.
                w.Write(Encoding.UTF8.GetBytes("Clip"));
                w.Write(0xFACADE01u);
                w.Write(1); // Same clip again.
            }
            w.Write(hasClips ? 3 : 0); // Separate trigger count.
            if (hasClips)
            {
                for (var i = 0; i < 3; i++)
                {
                    Trigger(w, version, i);
                }
            }
        });
        var group = new CGameCtnMediaClipGroup();
        Chunk<CGameCtnMediaClipGroup> chunk = version switch
        {
            0 => new CGameCtnMediaClipGroup.Chunk0307A000(),
            1 => new CGameCtnMediaClipGroup.Chunk0307A001(),
            2 => new CGameCtnMediaClipGroup.Chunk0307A002(),
            3 => new CGameCtnMediaClipGroup.Chunk0307A003(),
            _ => throw new ArgumentOutOfRangeException(nameof(version))
        };

        await Assert.That(group.Clips).IsEmpty();
        await RoundTrip(payload, rw => chunk.ReadWrite(group, rw));
        await Assert.That(group.Clips.Count).IsEqualTo(hasClips ? 3 : 0);
        if (hasClips)
        {
            await Assert.That(group.Clips[0].References.Clip!.Node).IsNull();
            await Assert.That(group.Clips[1].Clip.Name).IsEqualTo("Clip");
            await Assert.That(group.Clips[2].Clip).IsSameReferenceAs(group.Clips[1].Clip);
            for (var i = 0; i < 3; i++)
            {
                await AssertTrigger(group.Clips[i].Trigger, version, i);
            }
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_TriggerVersions_UseTheNativeFieldOrder(int version)
    {
        var payload = Payload(w => Trigger(w, version, 2));
        var trigger = new CGameCtnMediaClipGroup.Trigger();

        await Assert.That(trigger.RefCoord).IsEqualTo(new Int3(-1, -1, -1));
        await Assert.That(trigger.RefDir).IsEqualTo(Direction.North);
        await Assert.That(trigger.Condition).IsEqualTo(CGameCtnMediaClipGroup.ECondition.None);
        await RoundTrip(payload, rw => trigger.ReadWrite(rw, version));
        await AssertTrigger(trigger, version, 2);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0x12345678)]
    public async Task ReadWrite_DiscardedLegacyWord_KeepsItsPayload(int value)
    {
        var payload = Payload(w => w.Write(value));
        var group = new CGameCtnMediaClipGroup();
        var chunk = new CGameCtnMediaClipGroup.Chunk0307A004();

        await Assert.That(chunk.U01).IsEqualTo(-1);
        await RoundTrip(payload, rw => chunk.ReadWrite(group, rw));
        await Assert.That(chunk.U01).IsEqualTo(value);
    }

    [Test]
    public async Task TriggerAliases_WhenChanged_ChangeOnlyTheirComponents()
    {
        var trigger = new CGameCtnMediaClipGroup.Trigger
        {
            RefCoord = (1, 2, 3),
            RefDir = Direction.East
        };

#pragma warning disable CS0618
        trigger.U01 = -4;
        await Assert.That(trigger.RefCoord).IsEqualTo(new Int3(-4, 2, 3));
        trigger.U02 = 5;
        await Assert.That(trigger.RefCoord).IsEqualTo(new Int3(-4, 5, 3));
        trigger.U03 = 6;
        await Assert.That(trigger.RefCoord).IsEqualTo(new Int3(-4, 5, 6));
        trigger.U04 = 43;
        await Assert.That(trigger.RefDir).IsEqualTo((Direction)43);

        trigger.RefCoord = (7, 8, 9);
        trigger.RefDir = Direction.West;
        await Assert.That(trigger.U01).IsEqualTo(7);
        await Assert.That(trigger.U02).IsEqualTo(8);
        await Assert.That(trigger.U03).IsEqualTo(9);
        await Assert.That(trigger.U04).IsEqualTo(3);
#pragma warning restore CS0618
    }

    [Test]
    public async Task DeepClone_ClipGroup_PreservesClipIdentityAndCopiesTriggerCoordinates()
    {
        var clip = new CGameCtnMediaClip { Name = "Shared" };
        var group = new CGameCtnMediaClipGroup
        {
            Clips =
            [
                new(clip, new() { RefCoord = (4, 5, 6), RefDir = Direction.West, Coords = [(1, 2, 3)] }),
                new(clip, new() { Coords = [(7, 8, 9)] })
            ]
        };
#pragma warning disable GBXNET10001
        var clone = (CGameCtnMediaClipGroup)group.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(clone.Clips[0].Clip).IsNotSameReferenceAs(clip);
        await Assert.That(clone.Clips[1].Clip).IsSameReferenceAs(clone.Clips[0].Clip);
        var (clonedClip, _) = clone.Clips[0];
        await Assert.That(clonedClip.Name).IsEqualTo("Shared");
        await Assert.That(clone.Clips[0].Trigger).IsNotSameReferenceAs(group.Clips[0].Trigger);
        await Assert.That(clone.Clips[0].Trigger.RefCoord).IsEqualTo(new Int3(4, 5, 6));
        await Assert.That(clone.Clips[0].Trigger.RefDir).IsEqualTo(Direction.West);
        clone.Clips[0].Trigger.Coords![0] = (10, 11, 12);
        await Assert.That(group.Clips[0].Trigger.Coords![0]).IsEqualTo(new Int3(1, 2, 3));
    }

    [Test]
    public async Task ReadWrite_TMUnlimiterMappings_KeepClipIndicesAndSharedResources()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)6); // TMUnlimiter version.
            w.Write((byte)0); // Map flags.
            w.Write(1); // Legacy script count.
            w.Write(6);
            w.Write(Encoding.UTF8.GetBytes("Script"));
            w.Write(3);
            w.Write(new byte[] { 1, 2, 3 });
            w.Write(1); // Parameter set count.
            w.Write(6);
            w.Write(Encoding.UTF8.GetBytes("Params"));
            w.Write(0); // Parameter count.
            w.Write(3); // Clip mapping count.
            w.Write(1); // Parameter set on the second clip.
            w.Write((byte)0);
            w.Write(0);
            foreach (var index in new[] { 3, 4 })
            {
                w.Write(index); // Shared script on the fourth and fifth clips.
                w.Write((byte)1);
                w.Write(0);
            }
            w.Write(0); // Block group count.
            w.Write(0); // Block count.
        });
        var group = new CGameCtnMediaClipGroup
        {
            Clips = Enumerable.Range(0, 5).Select(_ => new CGameCtnMediaClipGroup.ClipTrigger(new(), new())).ToList()
        };
        var map = new CGameCtnChallenge { ClipGroupInGame = group, Blocks = [] };
        var chunk = new CGameCtnChallenge.Chunk3F001001();

        await RoundTrip(payload, rw => chunk.ReadWrite(map, rw));
        await Assert.That(group.Clips[0].Clip.TMUnlimiterData).IsNull();
        await Assert.That(group.Clips[2].Clip.TMUnlimiterData).IsNull();
        var parameterSet = (CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet)group.Clips[1].Clip.TMUnlimiterData!.Resource!;
        var script = (CGameCtnMediaClip.TMUnlimiter.LegacyScript)group.Clips[3].Clip.TMUnlimiterData!.Resource!;
        await Assert.That(parameterSet.Name).IsEqualTo("Params");
        await Assert.That(script.Name).IsEqualTo("Script");
        await Assert.That(script.ByteCode).IsEquivalentTo(new byte[] { 1, 2, 3 }, CollectionOrdering.Matching);
        await Assert.That(group.Clips[4].Clip.TMUnlimiterData!.Resource).IsSameReferenceAs(script);
    }

    private static void Trigger(BinaryWriter writer, int version, int index)
    {
        if (version == 0)
        {
            Coord(writer, index, 0); // No coordinate count in chunk 000.
            return;
        }
        if (version <= 2)
        {
            Coords(writer, index);
        }
        if (version >= 2)
        {
            writer.Write(-1);
            writer.Write(index + 20);
            writer.Write(index + 30);
            writer.Write(index == 2 ? 43 : index); // RefDir, including an unknown enum value.
        }
        if (version >= 3)
        {
            writer.Write(index == 2 ? 43 : index + 3); // Condition.
            writer.Write(index + 0.375f);
            Coords(writer, index);
        }
    }

    private static void Coords(BinaryWriter writer, int index)
    {
        writer.Write(index);
        for (var i = 0; i < index; i++)
        {
            Coord(writer, index, i);
        }
    }

    private static void Coord(BinaryWriter writer, int index, int coordinateIndex)
    {
        writer.Write(index * 10 + coordinateIndex);
        writer.Write(coordinateIndex + 5);
        writer.Write(index + 9);
    }

    private static async Task AssertTrigger(CGameCtnMediaClipGroup.Trigger trigger, int version, int index)
    {
        await Assert.That(trigger.Coords!.Count).IsEqualTo(version == 0 ? 1 : index);
        if (trigger.Coords.Count > 0)
        {
            await Assert.That(trigger.Coords[0]).IsEqualTo(new Int3(index * 10, 5, index + 9));
        }
        await Assert.That(trigger.RefCoord).IsEqualTo(version >= 2 ? new Int3(-1, index + 20, index + 30) : new Int3(-1, -1, -1));
        await Assert.That(trigger.RefDir).IsEqualTo(version >= 2 ? (Direction)(index == 2 ? 43 : index) : Direction.North);
        await Assert.That(trigger.Condition).IsEqualTo(version >= 3
            ? (CGameCtnMediaClipGroup.ECondition)(index == 2 ? 43 : index + 3)
            : CGameCtnMediaClipGroup.ECondition.None);
        await Assert.That(trigger.ConditionValue).IsEqualTo(version >= 3 ? index + 0.375f : 0);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        const uint followingWord = 0xDEADBEEF;
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(followingWord);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(followingWord);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
