using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CGameUserProfileLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ContextTimesKeepTheVersionTwoCounter(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(2);
            foreach (var context in new[] { "create", "clubs" })
            {
                String(w, context);
                w.Write(0xFEDCBA98u);
                if (version >= 2)
                {
                    w.Write(0x87654321u);
                }
                w.Write(0x80000001u);
            }
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC004();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ContextTimes!.Length).IsEqualTo(2);
        await Assert.That(node.ContextTimes[0].Context).IsEqualTo("create");
        await Assert.That(node.ContextTimes[1].Context).IsEqualTo("clubs");
        await Assert.That(node.ContextTimes[1].GameModeTimeSeconds).IsEqualTo(0xFEDCBA98u);
        await Assert.That(node.ContextTimes[1].PlayTimeSeconds).IsEqualTo(0x80000001u);
        await Assert.That(node.ContextTimes[1].U04).IsEqualTo(version >= 2 ? 0x87654321u : 0u);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.Unspecified);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task InputBindingsKeepFullLegacyVehicleIdentifiers(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(version == 1 ? -1 : 0); // Null node, or an empty node array.
            w.Write(0.8f);
            w.Write(0.2f);
            w.Write(1);
            w.Write(0xFEDCBA98u);
            w.Write(0);
            w.Write(1.5f);
            w.Write(2.5f);
            w.Write(0.002f);
            w.Write(1);
            w.Write(0.6f);
            w.Write(0.7f);
            if (version < 3)
            {
                w.Write(1);
                w.Write(3); // Id table version.
                w.Write(0x40000000u);
                String(w, "CarSnow");
                w.Write(26); // Numeric collection ID.
                w.Write(0x40000000u);
                String(w, "Nadeo");
                w.Write(1.25f);
                w.Write(0.15f);
                w.Write(1);
            }
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC007();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.RumbleIntensity).IsEqualTo(0.8f);
        await Assert.That(node.CenterSpringIntensity).IsEqualTo(0.2f);
        await Assert.That(node.MouseLookInvertY).IsTrue();
        await Assert.That(node.MouseReleaseKey).IsEqualTo(unchecked((CGameUserProfile.EMouseReleaseKey)0xFEDCBA98u));
        await Assert.That(node.MouseAccel).IsFalse();
        await Assert.That(node.MouseScaleY).IsEqualTo(1.5f);
        await Assert.That(node.MouseScaleFreeLook).IsEqualTo(2.5f);
        await Assert.That(node.MouseAccelQuantity).IsEqualTo(0.002f);
        await Assert.That(node.MouseSensitivities_EnableSpecific).IsTrue();
        await Assert.That(node.MouseSensitivity_Default).IsEqualTo(0.6f);
        await Assert.That(node.MouseSensitivity_Laser).IsEqualTo(0.7f);
        if (version < 3)
        {
            await Assert.That(node.LegacyVehicleSettings![0].Vehicle).IsEqualTo(new Ident("CarSnow", 26, "Nadeo"));
            await Assert.That(node.LegacyVehicleSettings[0].AnalogSensitivity).IsEqualTo(1.25f);
            await Assert.That(node.LegacyVehicleSettings[0].AnalogDeadZone).IsEqualTo(0.15f);
            await Assert.That(node.LegacyVehicleSettings[0].AnalogSteerV2).IsTrue();
        }
    }

    [Test]
    public async Task PersistentTraitsAreAnEncapsulatedDirectNode()
    {
        var payload = Payload(w =>
        {
            w.Write(1); // Profile chunk version.
            w.Write(0); // Encapsulation header.
            w.Write(16); // Encapsulated body size.
            w.Write(0x11001000u);
            w.Write(2); // Persistent traits chunk version.
            w.Write(0); // No namespaces.
            w.Write(0xFACADE01u);
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC001();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ScriptPersistentTraits).IsNotNull();
        await Assert.That(node.ScriptPersistentTraits!.PersistentTraits).IsEmpty();
    }

    [Test]
    public async Task ShootParamsAreTwoDirectNodesWithoutReferenceIndices()
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            foreach (var fps in new[] { 30, 60 })
            {
                w.Write(0x03060001u); // Legacy shoot parameters, with enum motion blur/stereo.
                w.Write(fps);
                w.Write(1920);
                w.Write(1080);
                w.Write(1); // HQ.
                w.Write(3); // Samples per axis.
                w.Write(0); // Motion blur.
                w.Write(1); // Soft shadows.
                w.Write(0); // Ambient occlusion.
                w.Write(1); // Audio stream.
                w.Write(0); // Stereo.
                w.Write(0xFACADE01u);
            }
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC00E();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Ignore).IsFalse();
        await Assert.That(node.ShootParamsVideo!.VideoFps).IsEqualTo(30);
        await Assert.That(node.ShootParamsScreenshot!.VideoFps).IsEqualTo(60);
        await Assert.That(node.ShootParamsVideo).IsNotSameReferenceAs(node.ShootParamsScreenshot);
    }

    [Test]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task BadgeVersionThreeStillContainsTheDiscardedWordAndString(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0.25f);
            w.Write(0.5f);
            w.Write(0.75f);
            if (version < 4)
            {
                w.Write(17);
                String(w, "legacy");
            }
            if (version == 2)
            {
                String(w, "first");
                String(w, "second");
            }
            if (version >= 3)
            {
                w.Write(1); // Stickers.
                String(w, "Nadeo");
                String(w, "Front");
                w.Write(0); // Layers.
            }
            if (version >= 4)
            {
                w.Write(1);
                String(w, "Skin.zip");
            }
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC000();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.BadgeColor).IsEqualTo(new Vec3(0.25f, 0.5f, 0.75f));
        if (version >= 3)
        {
            await Assert.That(node.Stickers![0].StickerName).IsEqualTo("Nadeo");
            await Assert.That(node.Stickers[0].SlotName).IsEqualTo("Front");
        }
        if (version >= 4)
        {
            await Assert.That(node.UseBadge).IsTrue();
            await Assert.That(node.SkinName).IsEqualTo("Skin.zip");
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task CampaignRecordsKeepTheMapUidAndMedalOrder(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1); // Campaign count.
            w.Write(7);
            String(w, "first");
            String(w, "second");
            String(w, "third");
            w.Write(1); // Map record count.
            w.Write(3); // Id table version.
            w.Write(0x40000000u);
            String(w, "MapUid");
            w.Write(100);
            w.Write(200);
            w.Write(300);
            w.Write(400);
            w.Write(4); // Author medal.
            if (version >= 2)
            {
                w.Write(4u);
                for (var i = 0; i < 4; i++)
                {
                    w.Write(1u);
                }
            }
            if (version >= 3)
            {
                w.Write(11);
            }
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC00A();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        var record = chunk.U01![0].InnerData![0];
        await Assert.That(record.MapUid).IsEqualTo("MapUid");
        await Assert.That(record.U02).IsEqualTo(100);
        await Assert.That(record.U03).IsEqualTo(200);
        await Assert.That(record.U04).IsEqualTo(300);
        await Assert.That(record.U05).IsEqualTo(400);
        await Assert.That(record.Medal).IsEqualTo(4);
        if (version >= 2)
        {
            await Assert.That(node.TotalMedals).IsEqualTo(4u);
            await Assert.That(node.AuthorMedalCount).IsEqualTo(1u);
        }
    }

    [Test]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    public async Task EditorSettingsKeepTheBooleanWidthChangeAndEntryVersion(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1); // Show help.
            for (var i = 0; i < 6; i++)
            {
                String(w, $"preset{i}");
            }
            for (var i = 0; i < 18; i++)
            {
                w.Write(i);
            }
            w.Write(1); // Entry count.
            w.Write(0); // Entry's independent version.
            String(w, "editor");
            w.Write(11);
            w.Write(12);
            w.Write(13);
            w.Write(14);
            w.Write(9);
            w.Write(17);
            w.Write(18);
            if (version == 6)
            {
                w.Write(1); // Bool32 in version 6.
            }
            else
            {
                w.Write((byte)0xFE); // Uninterpreted byte in versions 7+.
            }
            w.Write(0x87654321u);
            if (version >= 8)
            {
                w.Write(0xFEDCBA98u);
            }
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC00B();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Editor_ShowHelp).IsTrue();
        await Assert.That(chunk.U18![0].Version).IsEqualTo(0);
        await Assert.That(chunk.U18[0].U02).IsEqualTo("editor");
        await Assert.That(chunk.U24).IsEqualTo(unchecked((int)0x87654321u));
        if (version == 6)
        {
            await Assert.That(chunk.U22).IsTrue();
        }
        else
        {
            await Assert.That(chunk.U23).IsEqualTo((byte)0xFE);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task QuaternionSettingsUseTheirOwnArchiveVersion(int archiveVersion)
    {
        var payload = Payload(w =>
        {
            w.Write(4); // Profile chunk version.
            w.Write(archiveVersion);
            if (archiveVersion != 0)
            {
                w.Write(7);
                w.Write(1); // Quaternion count.
                w.Write(0.25f);
                w.Write(0.5f);
                w.Write(0.75f);
                w.Write(1f);
            }
            w.Write((byte)0xFE);
            w.Write(10);
            w.Write(11);
            w.Write(12);
        });
        var node = new CGameUserProfile();
        var chunk = new CGameUserProfile.Chunk031CC00F();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.U01!.Version).IsEqualTo(archiveVersion);
        await Assert.That(chunk.U02).IsEqualTo((byte)0xFE);
        await Assert.That(new CGameUserProfile.Unknown6().Version).IsEqualTo(1);
    }

    [Test]
    public async Task WriterMetadataExcludesReaderOnlyChunksAndUsesNativeVersions()
    {
        await Assert.That(new CGameUserProfile.Chunk031CC000().Version).IsEqualTo(16);
        await Assert.That(new CGameUserProfile.Chunk031CC007().Version).IsEqualTo(3);
        await Assert.That(new CGameUserProfile.Chunk031CC00B().Version).IsEqualTo(8);
        await Assert.That(new CGameUserProfile.Chunk031CC012().Version).IsEqualTo(0);
        await Assert.That(new CGameUserProfile.Chunk031CC014().Version).IsEqualTo(1);
        await Assert.That(new CGameUserProfile.Chunk031CC020().Version).IsEqualTo(8);
        await Assert.That(new CGameUserProfile.Chunk031CC006().GameVersion).IsEqualTo(GameVersion.Unspecified);
        await Assert.That(new CGameUserProfile.Chunk031CC008().GameVersion).IsEqualTo(GameVersion.Unspecified);
        await Assert.That(new CGameUserProfile.Chunk031CC015().GameVersion).IsEqualTo(GameVersion.Unspecified);
        await Assert.That(new CGameUserProfile.Chunk031CC024().GameVersion).IsEqualTo(GameVersion.TM2020);
    }

    private static void String(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
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
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
