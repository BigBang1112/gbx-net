using GBX.NET.Engines.Function;
using GBX.NET.Engines.Motion;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CMotionLayoutTests
{
    [Test]
    [Arguments(0u)]
    [Arguments(1u)]
    [Arguments(2u)]
    [Arguments(3u)]
    public async Task PlayerReadsConditionalStateBeforePhysicsAndTracks(uint savePlayState)
    {
        var payload = Payload(w =>
        {
            w.Write(0xFACADE01u); // Direct command body, without a class ID or reference index.
            w.Write(savePlayState);
            if (savePlayState == 3) w.Write(1u);
            w.Write(1); // IsPhysics, a 32-bit boolean.
            w.WriteIdAsString("Player");
            w.Write(2); // One track and a repeated reference.
            Node(w, 1, 0x08033000);
            w.Write(1);
        });
        var node = new CMotionPlayer();
        var chunk = new CMotionPlayer.Chunk08034004();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.PlayState).IsEqualTo(savePlayState == 3 ? 1u : savePlayState);
        await Assert.That(node.IsPhysics).IsTrue();
        await Assert.That(node.Name).IsEqualTo("Player");
        await Assert.That(node.Tracks).Count().IsEqualTo(2);
        await Assert.That(node.Tracks[1]).IsSameReferenceAs(node.Tracks[0]);
    }

    [Test]
    public async Task CommandReadsParameterNodeAfterFiveScalarFields()
    {
        var payload = Payload(w =>
        {
            w.Write(2500u);
            w.Write(0.25f);
            w.Write(4); // InverseSawTooth.
            w.Write(1); // IsOnce.
            w.Write(1); // IsAbsolutePhase.
            w.Write(1); // Parameter node reference index.
            w.Write(0x0802D000u);
            w.Write(0x0802D000u);
            w.Write(9000u);
            w.Write(0.75f);
            w.Write(0xFACADE01u);
        });
        var node = new CMotionCmdBase();
        var chunk = new CMotionCmdBase.Chunk08029002();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Period).IsEqualTo(2500u);
        await Assert.That(node.WaveType).IsEqualTo(CMotionCmdBase.EWaveType.InverseSawTooth);
        await Assert.That(node.CmdBaseParams!.Period).IsEqualTo(TimeInt32.FromMilliseconds(9000));
        await Assert.That(node.CmdBaseParams.Phase).IsEqualTo(0.75f);
    }

    [Test]
    public async Task LegacyCommandConvertsPeriodAndPhaseWithoutChangingArchivedValues()
    {
        var payload = Payload(w =>
        {
            w.Write(1001u);
            w.Write(2f);
            w.Write(3);
            w.Write(0);
            w.Write(-250);
        });
        var node = new CMotionCmdBase();
        var chunk = new CMotionCmdBase.Chunk0802A003();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Period).IsEqualTo(500u);
        await Assert.That(node.Phase).IsEqualTo(-250f / 1001);
    }

    [Test]
    public async Task SharedLeavesChunkIdUsesTheOwningNodesLayout()
    {
        var payload = Payload(w =>
        {
            w.Write(0x0804C000u); // Legacy emitter chunk.
            w.Write(1);
            w.Write(0x0804D000u); // Manager node.
            w.Write(0x0804C000u); // The manager uses the same chunk ID for a different layout.
            w.Write(-1); // MobilLeaves.
            w.Write(0xFACADE01u);
            w.Write(2f);
            w.Write(3f);
            w.Write(4f);
            w.Write(5f); // Uniform radius.
            w.Write(0xFACADE01u);
        });
        var node = new CMotionEmitterLeaves();
        await RoundTrip(payload, node.ReadWrite);
        await Assert.That(node.ManagerModel).IsNotNull();
        await Assert.That(node.Pos).IsEqualTo(new Vec3(2, 3, 4));
        await Assert.That(node.Radius).IsEqualTo(new Vec3(5, 5, 5));
    }

    [Test]
    public async Task ShaderReadsShaderAndFunctionNodeReferences()
    {
        var payload = Payload(w =>
        {
            w.Write(-1); // Material.
            Node(w, 1, 0x09002000); // Shader.
            Node(w, 2, 0x05015000); // FuncShaderLayerUV.
        });
        var node = new CMotionShader();
        var chunk = new CMotionShader.Chunk0802B000();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Shader).IsNotNull();
        await Assert.That(node.FuncShader).IsTypeOf<CFuncShaderLayerUV>();
    }

    [Test]
    public async Task ShaderWithMaterialWritesNullShaderWithoutClearingTheProperty()
    {
        var shader = new CPlugShader();
        var node = new CMotionShader
        {
            Material = new CPlugMaterial(),
            Shader = shader,
            FuncShader = new CFuncShaderLayerUV()
        };
        var payload = Payload(w =>
        {
            using var rw = new GbxReaderWriter(w);
            new CMotionShader.Chunk0802B000().ReadWrite(node, rw);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadNodeRef<CPlugMaterial>()).IsNotNull();
        await Assert.That(reader.ReadInt32()).IsEqualTo(-1);
        await Assert.That(reader.ReadNodeRef<CFuncShader>()).IsTypeOf<CFuncShaderLayerUV>();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
        await Assert.That(node.Shader).IsSameReferenceAs(shader);
    }

    [Test]
    public async Task DayTimeReadsMaterialReferencesAfterTheLegacyNode()
    {
        var payload = Payload(w =>
        {
            w.Write(-1);
            Node(w, 1, 0x09079000);
            Node(w, 2, 0x09079000);
        });
        var node = new CMotionDayTime();
        var chunk = new CMotionDayTime.Chunk08055000();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DayMaterial).IsNotNull();
        await Assert.That(node.NightMaterial).IsNotNull();
        await Assert.That(node.NightMaterial).IsNotSameReferenceAs(node.DayMaterial);
        await Assert.That(node).IsAssignableTo<CMotionManaged>();
    }

    [Test]
    public async Task SkeletonValuesUseBoneCountWithoutAnArrayPrefix()
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(0x05005000u);
            w.Write(0x05005002u);
            w.Write(2);
            w.WriteIdAsString("A");
            w.WriteIdAsString("B");
            w.Write(0xFACADE01u);
            for (var i = 1; i <= 14; i++) w.Write((float)i);
        });
        var node = new CFuncSkelValues();
        var chunk = new CFuncSkelValues.Chunk05007000();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Locations).Count().IsEqualTo(2);
        await Assert.That(node.Locations![0].Rotation).IsEqualTo(new Quat(1, 2, 3, 4));
        await Assert.That(node.Locations[1].Position).IsEqualTo(new Vec3(12, 13, 14));
    }

    private static void Node(GbxWriter writer, int index, uint classId)
    {
        writer.Write(index);
        writer.Write(classId);
        writer.Write(0xFACADE01u);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
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
