using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.GameData;

[Category("Unit")]
public class CGameObjectPhyModelLayoutTests
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
    [Arguments(23, false)]
    [Arguments(23, true)]
    [Arguments(24, false)]
    [Arguments(24, true)]
    [Arguments(25, false)]
    [Arguments(25, true)]
    [Arguments(26, false)]
    [Arguments(26, true)]
    public async Task MainChunkPreservesNativeBranches(int version, bool paths)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        if (version < 11)
        {
            payload.Reference("Move.Shape.Gbx");
            payload.Reference("Hit.Shape.Gbx");
            payload.Reference("Trigger.Shape.Gbx");
        }

        var triggerVersion = version < 8 ? 0 : version % 6;
        if (version >= 8) writer.Write(triggerVersion);
        writer.Write(2);
        WriteTrigger(writer, triggerVersion, "TriggerA");
        WriteTrigger(writer, triggerVersion, "TriggerB");
        if (version < 14) payload.Reference("Deprecated.Anim.Gbx");
        if (version >= 2) writer.Write(paths ? "Move.Shape.Gbx" : "");
        if (version >= 4) writer.Write(paths ? "Hit.Shape.Gbx" : "");
        if (version >= 3)
        {
            writer.Write(!paths);
            if (!paths)
            {
                writer.Write(0); // Inline CPlugDynaPointModel archive version.
                for (var i = 1; i < 8; i++) writer.Write(i * 10);
            }
        }
        if (version >= 5) writer.Write(2); // EProgram.Turret.
        if (version >= 6)
        {
            if (version >= 22) writer.Write((byte)(paths ? 0 : 1));
            if (version < 22 || !paths) writer.Write(Iso4.Identity);
        }
        if (version >= 7) writer.Write(paths ? "Trigger.Shape.Gbx" : "");
        if (version >= 9)
        {
            if (version >= 13) writer.Write(paths ? "Action.Gbx" : "");
            if (version < 13 || !paths)
            {
                writer.Write(2);
                writer.Write(0xFACADE01u); // Two direct CGameActionModel bodies, without node reference framing.
                writer.Write(0xFACADE01u);
            }
        }
        if (version >= 21)
        {
            if (paths) payload.Reference("Move.Fid.Gbx");
            else writer.Write(-1);
        }
        if (version >= 11)
        {
            if (version >= 21 && paths) payload.Reference("Deprecated.Shape.Gbx");
            else if (!paths) payload.Reference("Move.Shape.Gbx");
            if (!paths)
            {
                payload.Reference("Hit.Shape.Gbx");
                payload.Reference("Trigger.Shape.Gbx");
            }
        }
        if (version >= 12)
        {
            writer.Write(paths ? "Solid.Gbx" : "");
            if (!paths) payload.Reference("Solid.Gbx");
        }
        if (version >= 15) writer.Write(123);
        if (version >= 16) payload.Reference("SpecialProperties.Gbx");
        if (version >= 17) writer.Write(2); // EPersistence.NeverRemove.
        if (version >= 18)
        {
            writer.Write(true);
            writer.Write(false);
        }
        if (version >= 19)
        {
            writer.Write(31.5f);
            writer.Write(11.5f);
            writer.Write(150);
            writer.Write(true);
            writer.Write(8000);
            writer.Write(2.5f);
            writer.Write(3.5f);
            writer.Write(600);
            foreach (var flag in new[] { true, false, true, false, true }) writer.Write(flag);
            writer.Write(true);
            writer.Write(20);
            writer.Write(false);
            writer.Write(300);
        }
        if (version >= 20) writer.Write(false);
        if (version >= 23) writer.Write(true);
        if (version >= 24) writer.Write(false);
        if (version == 25) payload.Reference("Version25Only.Gbx");
        payload.Finish();

        var node = new CGameObjectPhyModel();
        var chunk = new CGameObjectPhyModel.Chunk2E006001();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.Triggers!.Length).IsEqualTo(2);
        if (version >= 8) await Assert.That(node.TriggerActionVersion).IsEqualTo(triggerVersion);
        if (version >= 3) await Assert.That(node.DynaPointModel is not null).IsEqualTo(!paths);
        if (version >= 9 && (version < 13 || !paths))
        {
            await Assert.That(node.Actions!.Length).IsEqualTo(2);
            await Assert.That(node.Actions[0]).IsNotNull();
            await Assert.That(node.Actions[1]).IsNotNull();
        }
        if (version >= 19)
        {
            await Assert.That(node.Armor).IsEqualTo(150);
            await Assert.That(node.ThrowSpeed).IsEqualTo(31.5f);
            await Assert.That(chunk.U22).IsEqualTo(true);
        }
        if (version >= 21 && paths)
        {
            await Assert.That(chunk.U01File!.FilePath).IsEqualTo("Move.Fid.Gbx");
            await Assert.That(chunk.U27File!.FilePath).IsEqualTo("Deprecated.Shape.Gbx");
        }
        if (version >= 22) await Assert.That(chunk.U17).IsEqualTo(!paths);
        if (version == 25) await Assert.That(chunk.U26File!.FilePath).IsEqualTo("Version25Only.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task InlineDynaModelPreservesArchiveVersions(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(true);
        writer.Write(version);
        for (var i = 1; i <= 15; i++) writer.Write(i + 0.5f);
        if (version >= 1) writer.Write(false);
        if (version >= 2) writer.Write((byte)7);
        if (version >= 3) writer.Write(true);
        if (version >= 4) payload.Reference("Water.Gbx");
        payload.Finish();

        var node = new CGameObjectPhyModel();
        var chunk = new CGameObjectPhyModel.Chunk2E006003();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DynaModel!.Version).IsEqualTo(version);
        await Assert.That(node.DynaModel.LinearMass).IsEqualTo(1.5f);
        await Assert.That(node.DynaModel.MaxDistPerStep).IsEqualTo(2.5f);
        await Assert.That(node.DynaModel.CenterOfMass).IsEqualTo(new Vec3(3.5f, 4.5f, 5.5f));
        await Assert.That(node.DynaModel.InverseInertiaMatrix).IsEqualTo(new Mat3(6.5f, 7.5f, 8.5f, 9.5f, 10.5f, 11.5f, 12.5f, 13.5f, 14.5f));
        await Assert.That(node.DynaModel.AngularSpeedClamp).IsEqualTo(15.5f);
        await Assert.That(node.DynaModel.UseTMSimulation).IsEqualTo(version == 0);
        await Assert.That((byte)node.DynaModel.SleepingMethod).IsEqualTo((byte)(version >= 2 ? 7 : 1));
        await Assert.That(node.DynaModel.EnableSubStepping).IsEqualTo(version >= 3);
        if (version >= 4) await Assert.That(node.DynaModel.WaterModelFile!.FilePath).IsEqualTo("Water.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task MissingDynaModelConsumesOnlyPresenceFlag()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(false);
        payload.Finish();
        var node = new CGameObjectPhyModel();
        var chunk = new CGameObjectPhyModel.Chunk2E006003();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DynaModel).IsNull();
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ThrowableSettingsPreserveVersionBranches(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        writer.Write(true);
        writer.Write(2.5f);
        if (version >= 1) writer.Write(1500);
        if (version >= 2) writer.Write(500);
        if (version >= 3) writer.Write(850.5f);
        payload.Finish();
        var node = new CGameObjectPhyModel();
        var chunk = new CGameObjectPhyModel.Chunk2E006005();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ThrowableEnabled).IsTrue();
        await Assert.That(node.ThrowableDamageCoef).IsEqualTo(2.5f);
        await Assert.That(node.ThrowableStunnedDuration).IsEqualTo(version >= 1 ? 1500 : 1000);
        await Assert.That(node.StaminaThrowCost).IsEqualTo(version >= 3 ? 850.5f : 750f);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task DeprecatedChunksPreserveTheirPayloads()
    {
        var node = new CGameObjectPhyModel();
        using (var payload = new ObjectModelPayload())
        {
            payload.Reference("LegacyTrigger.Gbx");
            payload.Writer.Write(1);
            WriteTrigger(payload.Writer, 5, "Legacy");
            payload.Reference("LegacyAnimation.Gbx");
            payload.Finish();
            var chunk = new CGameObjectPhyModel.Chunk2E006000();
            payload.Read(rw => chunk.ReadWrite(node, rw));
            await Assert.That(node.TriggerShapeFidFile!.FilePath).IsEqualTo("LegacyTrigger.Gbx");
            await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        }
        using (var payload = new ObjectModelPayload())
        {
            payload.Reference("Deprecated.Gbx");
            payload.Finish();
            var chunk = new CGameObjectPhyModel.Chunk2E006002();
            payload.Read(rw => chunk.ReadWrite(node, rw));
            await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        }
        using (var payload = new ObjectModelPayload())
        {
            payload.Writer.Write(0);
            payload.Reference("Deprecated.Gbx");
            payload.Writer.Write(new Vec3(1, 2, 3));
            payload.Finish();
            var chunk = new CGameObjectPhyModel.Chunk2E006004();
            payload.Read(rw => chunk.ReadWrite(node, rw));
            await Assert.That(chunk.U02).IsEqualTo(new Vec3(1, 2, 3));
            await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        }
    }

    [Test]
    public async Task DirectActionArrayRejectsNullEntriesAndInvalidCounts()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            await Assert.That(() => writer.WriteArrayNode<CGameActionModel>([null])).Throws<ArgumentException>();
            await Assert.That(stream.Length).IsEqualTo(0L);
            writer.Write(-1);
        }
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(() => reader.ReadArrayNode<CGameActionModel>()).Throws<ArgumentOutOfRangeException>();
    }

    private static void WriteTrigger(GbxWriter writer, int version, string id)
    {
        if (version >= 5) writer.Write(101);
        if (version >= 4)
        {
            for (var i = 0; i < 4; i++) writer.Write(i + 1.5f);
        }
        if (version >= 3) writer.Write(102);
        if (version >= 2) writer.Write(103);
        if (version >= 1)
        {
            writer.Write(104);
            writer.Write(105);
        }
        for (var i = 0; i < 6; i++) writer.Write(i + 10.5f);
        writer.WriteIdAsString(id);
    }
}

internal sealed class ObjectModelPayload : IDisposable
{
    private const int sentinel = 0x11223344;
    private readonly MemoryStream stream = new();
    private readonly GbxRefTable refTable = new();
    private readonly Dictionary<int, GbxRefTableNode> files = [];
    public GbxWriter Writer { get; }

    public ObjectModelPayload()
    {
        Writer = new GbxWriter(stream);
    }

    public GbxRefTableFile Reference(string path)
    {
        var file = new GbxRefTableFile(refTable, 0, true, path);
        var index = files.Count + 1;
        files.Add(index, file);
        Writer.Write(index);
        return file;
    }

    public void Finish() => Writer.Write(sentinel);

    public void Read(Action<GbxReaderWriter> readWrite)
    {
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(files);
        using var rw = new GbxReaderWriter(reader);
        readWrite(rw);
        if (reader.ReadInt32() != sentinel || stream.Position != stream.Length)
        {
            throw new InvalidDataException("Chunk did not consume its native payload exactly.");
        }
    }

    public async Task AssertRoundTrip(Action<GbxReaderWriter> readWrite)
    {
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            readWrite(rw);
            writer.Write(sentinel);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    public void Dispose()
    {
        Writer.Dispose();
        stream.Dispose();
    }
}
