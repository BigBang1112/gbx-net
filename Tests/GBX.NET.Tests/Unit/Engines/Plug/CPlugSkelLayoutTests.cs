using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSkelLayoutTests
{
    private static readonly Iso4 Transform = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

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
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    [Arguments(19)]
    [Arguments(20)]
    public async Task ReadWrite_Versions_PreserveJointsSetupSocketsAndIndependentArrays(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteArchive(writer, version, optionalData: true);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugSkel();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            node.ReadWrite(rw, version);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.Name).IsEqualTo("Skeleton");
        await Assert.That(node.Joints.Length).IsEqualTo(1);
        await Assert.That(node.Joints[0].Name).IsEqualTo("Root");
        await Assert.That(node.Joints[0].ParentIndex).IsEqualTo((short)-1);
        if (version <= 14)
        {
            await Assert.That(node.Joints[0].LocalRotation).IsEqualTo(Quat.Identity);
            await Assert.That(node.Joints[0].LocalTranslation).IsEqualTo(new Vec3(2, 3, 4));
        }
        if (version >= 1) await Assert.That(node.Joints[0].GlobalTransform).IsEqualTo(Transform);
        if (version >= 2)
        {
            await Assert.That(node.SkelSetup).IsNotNull();
            await Assert.That(node.SkelSetup!.JointInfos![0].Flags).IsEqualTo(0x80000001u);
            if (version < 12) await Assert.That(node.SkelSetup.InverseKinematics![0].U03).IsEqualTo(3);
            else await Assert.That(node.SkelSetup.InverseKinematics![0].U06).IsEqualTo((ushort)50002);
        }
        if (version >= 6)
        {
            await Assert.That(node.Sockets.Length).IsEqualTo(1);
            await Assert.That(node.Sockets[0].JointIndex).IsEqualTo((short)-1);
            await Assert.That(node.Sockets[0].LocalTransform).IsEqualTo(Transform);
        }
        if (version >= 9) await Assert.That(node.AdditionalJoints!.Names![0]).IsEqualTo("Extra");
        if (version is >= 10 and <= 15) await Assert.That(node.U03![0]).IsEqualTo(123);
        if (version >= 16) await Assert.That(node.AnimLodIndexs).IsEquivalentTo(new byte[] { 2 }, CollectionOrdering.Matching);
        if (version >= 13) await Assert.That(node.U04).IsEquivalentTo(new byte[] { 128 }, CollectionOrdering.Matching);
        if (version == 14)
        {
            await Assert.That(node.JointExprVersion).IsEqualTo(0);
            await Assert.That(node.JointExprs.Length).IsEqualTo(1);
            await Assert.That(node.JointExprs[0].U04).IsEqualTo("SourceB");
        }
        if (version >= 19) await Assert.That(node.U07).IsEquivalentTo(new byte[] { 32 }, CollectionOrdering.Matching);
        if (version >= 20) await Assert.That(node.U08![0]).IsEqualTo(Transform);
        if (version >= 17)
        {
            await Assert.That(node.AnimLodCount).IsEqualTo((byte)3);
            await Assert.That(node.AnimLodDistances).IsEquivalentTo(new float[] { 10, 20 }, CollectionOrdering.Matching);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) node.ReadWrite(rw, version);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(2)]
    [Arguments(9)]
    [Arguments(12)]
    [Arguments(14)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    [Arguments(19)]
    [Arguments(20)]
    public async Task ReadWrite_AbsentOptionalObjects_DoNotConsumeFollowingArrays(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload)) WriteArchive(writer, version, optionalData: false);
        payload.Position = 0;
        var node = new CPlugSkel();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) node.ReadWrite(rw, version);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.SkelSetup).IsNull();
        await Assert.That(node.AdditionalJoints).IsNull();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) node.ReadWrite(rw, version);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(12)]
    [Arguments(14)]
    [Arguments(16)]
    [Arguments(18)]
    [Arguments(19)]
    [Arguments(20)]
    public async Task ReadWrite_VersionedChunk_RoundTripsTheCompleteNodeBody(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugSkel.Id);
            writer.Write(version);
            WriteArchive(writer, version, optionalData: true);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugSkel>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.OfType<CPlugSkel.Chunk090BA000>().Single().Version).IsEqualTo(version);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_JointCount_UsesTheFullUnsignedSixteenBitRange()
    {
        var node = new CPlugSkel { Joints = Enumerable.Range(0, 32768).Select(_ => new CPlugSkel.Joint()).ToArray() };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) node.ReadWrite(rw);
        payload.Position = 0;
        var restored = new CPlugSkel();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) restored.ReadWrite(rw);
        await Assert.That(restored.Joints.Length).IsEqualTo(32768);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }

    [Test]
    public async Task Constructor_ConstructionAndChunkVersions_MatchNativeDefaults()
    {
        var node = new CPlugSkel();
        await Assert.That(node.Joints.Length).IsEqualTo(0);
        await Assert.That(node.Sockets.Length).IsEqualTo(0);
        await Assert.That(node.JointExprs.Length).IsEqualTo(0);
        await Assert.That(node.AnimLodCount).IsEqualTo((byte)1);
        await Assert.That(node.SkelSetup).IsNull();
        await Assert.That(node.AdditionalJoints).IsNull();
        await Assert.That(new CPlugSkel.Chunk090BA000(GameVersion.TMT).Version).IsEqualTo(12);
        await Assert.That(new CPlugSkel.Chunk090BA000(GameVersion.MP4).Version).IsEqualTo(14);
        await Assert.That(new CPlugSkel.Chunk090BA000(GameVersion.TM2020).Version).IsEqualTo(20);
        await Assert.That(new CPlugSkel.Setup().U03).IsEqualTo((ushort)0xFFFF);
        await Assert.That(new CPlugSkel.Setup().U06).IsEqualTo((ushort)0xFFFF);
    }

    private static void WriteArchive(GbxWriter writer, int version, bool optionalData)
    {
        writer.WriteIdAsString("Skeleton");
        writer.Write((ushort)1);
        writer.WriteIdAsString("Root");
        writer.Write((short)-1);
        if (version <= 14)
        {
            writer.Write(Quat.Identity);
            writer.Write(new Vec3(2, 3, 4));
        }
        if (version >= 1) writer.Write(Transform);
        if (version >= 2)
        {
            writer.Write(optionalData);
            if (optionalData) WriteSetup(writer, version);
        }
        if (version >= 6)
        {
            writer.Write(1);
            writer.WriteIdAsString("Socket");
            writer.Write((short)-1);
            writer.Write(Transform);
        }
        if (version >= 9)
        {
            writer.Write(optionalData);
            if (optionalData)
            {
                writer.Write(1);
                writer.WriteIdAsString("Extra");
                writer.Write(1);
                writer.Write(7);
                writer.Write(1);
                writer.Write(8);
                writer.Write(1);
                writer.Write(Quat.Identity);
            }
        }
        if (version >= 10)
        {
            writer.Write(1);
            if (version < 16) writer.Write(123);
            else writer.Write((byte)2);
        }
        if (version >= 13)
        {
            writer.Write(1);
            writer.Write((byte)128);
        }
        if (version == 14)
        {
            writer.Write(0);
            writer.Write(1);
            writer.Write(2);
            writer.WriteIdAsString("Target");
            writer.WriteIdAsString("SourceA");
            writer.WriteIdAsString("SourceB");
        }
        if (version >= 19)
        {
            writer.Write(1);
            writer.Write((byte)32);
        }
        if (version >= 20)
        {
            writer.Write(1);
            writer.Write(Transform);
        }
        if (version >= 17)
        {
            writer.Write((byte)3);
            writer.Write(2);
            writer.Write(10f);
            writer.Write(20f);
        }
    }

    private static void WriteSetup(GbxWriter writer, int version)
    {
        writer.Write(1); // Two-bone IK chains.
        if (version < 12)
        {
            writer.Write(1);
            writer.Write(2);
            writer.Write(3);
        }
        else
        {
            writer.Write((ushort)50000);
            writer.Write((ushort)50001);
            writer.Write((ushort)50002);
        }
        writer.Write(1); // Joint infos.
        writer.Write(4);
        writer.Write(5);
        writer.Write(6);
        writer.Write(0x80000001u);
        if (version < 11) writer.Write(123);
        if (version >= 3)
        {
            writer.Write(1);
            writer.Write(456);
        }
        if (version >= 5)
        {
            if (version < 12) writer.Write(-1);
            else writer.Write((ushort)0xFFFF);
        }
        if (version is >= 7 and <= 10) writer.Write(789);
        if (version >= 8)
        {
            if (version < 12) writer.Write(-1);
            else writer.Write((ushort)0xFFFF);
            if (version < 11) writer.Write(321);
        }
    }
}
