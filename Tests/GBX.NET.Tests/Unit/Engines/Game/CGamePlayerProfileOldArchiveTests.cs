using GBX.NET.Engines.Game;
using GBX.NET.Engines.Input;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGamePlayerProfileOldArchiveTests
{
    [Test]
    public async Task ReadWrite_OldProfileChunks_ReadsLocalArchives()
    {
        var payload = Payload(w =>
        {
            w.Write(3); // ID table version.
            w.Write(uint.MaxValue);
            String(w, "profile");
            w.Write(3);

            OldEntry(w, 0x03130000, 4); // VehiclesSettings.
            String(w, "");
            w.Write(new byte[32]);
            w.Write(1f);
            w.Write(0f);
            w.Write(0f);
            w.Write(1); // Vehicle profile count.
            NewId(w, "CarCommon");
            NewId(w, "Vehicles");
            NewId(w, "Nadeo");
            String(w, "Skins\\Vehicles\\CarCommon\\Default.zip");
            w.Write(new byte[32]);
            w.Write(2); // Game camera.
            w.Write(0.5f);
            w.Write(0.1f);
            w.Write(3);

            OldEntry(w, 0x03149000, 1); // ChallengesScores.
            w.Write(7);
            w.Write(1); // Record count.
            NewId(w, "MapUid");
            NewId(w, "Canyon");
            NewId(w, "author");
            for (var i = 0; i < 3; i++) w.Write(10 + i);
            w.Write(0UL);
            w.Write(0UL);
            for (var i = 0; i < 8; i++) w.Write(20 + i);
            w.Write(0); // No extra local data.
            w.Write(1); // Modified since the last synchronization.

            OldEntry(w, 0x0312F000, 0); // InputBindingsConfig.
            w.Write(-1); // Empty node reference.
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C07C();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        var vehicles = (CGamePlayerProfileChunk_VehiclesSettings)node.OldProfileChunks![0];
        await Assert.That(vehicles.ArchiveVersion).IsEqualTo(4);
        await Assert.That(vehicles.U03).IsEqualTo(1f);
        await Assert.That(vehicles.OldVehicleProfiles![0].SkinName).IsEqualTo("Skins\\Vehicles\\CarCommon\\Default.zip");
        await Assert.That(vehicles.OldVehicleProfiles[0].GameCamera).IsEqualTo(2);
        await Assert.That(vehicles.OldVehicleProfileVersion).IsEqualTo(3);

        var scores = (CGamePlayerProfileChunk_ChallengesScores)node.OldProfileChunks[1];
        var record = scores.OldChallengesScores![0];
        await Assert.That(scores.U01).IsEqualTo(7);
        await Assert.That(record.MapInfo!.Id).IsEqualTo("MapUid");
        await Assert.That(record.U01).IsEqualTo(10);
        await Assert.That(record.U13).IsEqualTo(27);
        await Assert.That(record.ModifiedSinceLastSynchro).IsTrue();

        var bindings = (CGamePlayerProfileChunk_InputBindingsConfig)node.OldProfileChunks[2];
        await Assert.That(bindings.Config).IsNull();
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    [Arguments(true, 0)]
    [Arguments(true, 1)]
    [Arguments(true, 2)]
    [Arguments(true, 3)]
    [Arguments(true, 4)]
    [Arguments(false, 0)]
    [Arguments(false, 1)]
    public async Task ReadWrite_PackageRecords_PreserveLegacyAndChunkLayouts(bool isLegacy, int version)
    {
        const string legacyValue = "legacy package string";
        const string key = "package key";
        const uint startTimestamp = 1_700_000_000;
        const uint endTimestamp = 1_700_000_001;
        var checksum = Enumerable.Range(0, 32).Select(value => (byte)value).ToArray();
        var hasLegacyString = isLegacy && version <= 1;
        var hasStartTimestamp = !isLegacy || version >= 1;
        var hasEndTimestamp = !isLegacy || version >= 3;
        var payload = Payload(w =>
        {
            if (!isLegacy) w.Write(version);

            w.Write(1);
            if (hasLegacyString) String(w, legacyValue);
            w.Write(checksum);
            String(w, key);
            if (hasStartTimestamp) w.Write(startTimestamp);
            if (hasEndTimestamp) w.Write(endTimestamp);
        });

        var node = new CGamePlayerProfileChunk_PackagesInfos();
        var chunk = new CGamePlayerProfileChunk_PackagesInfos.Chunk03140000();
        Action<GbxReaderWriter> serialize = isLegacy
            ? rw => node.ReadWrite(rw, version)
            : rw => chunk.ReadWrite(node, rw);
        Read(payload, serialize);

        var record = (isLegacy ? node.OldPackagesInfos : node.PackagesInfos)!.Single();
        await Assert.That(record.U01).IsEqualTo(hasLegacyString ? legacyValue : null);
        await Assert.That(record.Checksum!.SequenceEqual(checksum)).IsTrue();
        await Assert.That(record.Key).IsEqualTo(key);
        await Assert.That(record.StartTimestamp?.ToUnixTimeSeconds()).IsEqualTo(hasStartTimestamp ? (long?)startTimestamp : null);
        await Assert.That(record.EndTimestamp?.ToUnixTimeSeconds()).IsEqualTo(hasEndTimestamp ? (long?)endTimestamp : null);
        await Assert.That(Write(serialize).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    public async Task ReadWrite_ManiaPlanet1ProfileChunks_OmitLocalData()
    {
        var accountPayload = Payload(w =>
        {
            w.Write(6);
            w.Write(5); // Compatibility version.
            String(w, "description");
            String(w, ""); // DefaultGroupLogin.
            String(w, "avatar");
            w.Write(0); // Player-tags config version.
            w.Write(0);
            w.Write(0);
            w.Write((byte)1); // ReceiveNews.
            w.Write(1); // EulaVersion.
        });
        var scoresPayload = Payload(w =>
        {
            w.Write(2);
            w.Write(1); // Compatibility version.
            w.Write(1); // Record count, without the local header value.
            w.Write(3); // Each payload has its own ID table.
            NewId(w, "MapUid");
            NewId(w, "Canyon");
            NewId(w, "author");
            for (var i = 0; i < 3; i++) w.Write(10 + i);
            w.Write(0UL);
            w.Write(0UL);
            for (var i = 0; i < 8; i++) w.Write(20 + i);
        });
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(3); // ID table version.
            w.Write(uint.MaxValue);
            String(w, "profile");
            w.Write(2);
            Entry(w, 0x0312C000, accountPayload);
            Entry(w, 0x03149000, scoresPayload);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C07D();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        var account = (CGamePlayerProfileChunk_AccountSettings)node.ProfileChunks![0];
        await Assert.That(account.Description).IsEqualTo("description");
        await Assert.That(account.AvatarName).IsEqualTo("avatar");
        await Assert.That(account.EulaVersion).IsEqualTo(1);

        var scores = (CGamePlayerProfileChunk_ChallengesScores)node.ProfileChunks[1];
        await Assert.That(scores.ArchiveVersion).IsEqualTo(1);
        await Assert.That(scores.SkipArchiveVersion).IsEqualTo(2);
        await Assert.That(scores.OldChallengesScores![0].U13).IsEqualTo(27);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    public async Task ReadWrite_BufferMemoryArchive_EmbedsNodeAsGbx()
    {
        var bindings = new CGamePlayerProfileChunk_InputBindingsConfig
        {
            SkipArchiveVersion = 1,
            ArchiveVersion = 0,
            Config = new CInputBindingsConfig()
        };
        var node = new CGamePlayerProfile { ProfileChunks = [bindings] };
        var chunk = new CGamePlayerProfile.Chunk0308C07D();
        var payload = Write(rw => chunk.ReadWrite(node, rw));

        var restored = new CGamePlayerProfile();
        Read(payload, rw => chunk.ReadWrite(restored, rw));

        var restoredBindings = (CGamePlayerProfileChunk_InputBindingsConfig)restored.ProfileChunks![0];
        await Assert.That(restoredBindings.Config).IsNotNull();
        await Assert.That(restoredBindings.ArchiveVersion).IsEqualTo(0);
        await Assert.That(restoredBindings.SkipArchiveVersion).IsEqualTo(1);

        // Saved Gbx files are signed with the save time, so only the size is stable.
        await Assert.That(Write(rw => chunk.ReadWrite(restored, rw)).Length).IsEqualTo(payload.Length);
    }

    private static void OldEntry(BinaryWriter w, uint classId, int archiveVersion)
    {
        w.Write(classId);
        String(w, "");
        String(w, "");
        String(w, "checksum");
        w.Write(0u);
        w.Write(archiveVersion);
    }

    private static void Entry(BinaryWriter w, uint classId, byte[] data)
    {
        w.Write(classId);
        String(w, "");
        String(w, "");
        String(w, "checksum");
        w.Write(0u);
        w.Write(data.Length);
        w.Write(data);
    }

    private static void NewId(BinaryWriter w, string value)
    {
        w.Write(0x40000000);
        String(w, value);
    }

    private static void String(BinaryWriter w, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        w.Write(bytes.Length);
        w.Write(bytes);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        write(w);
        return ms.ToArray();
    }

    private static void Read(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var ms = new MemoryStream(payload);
        using var r = new GbxReader(ms);
        using var rw = new GbxReaderWriter(r);
        serialize(rw);
        if (ms.Position != ms.Length) throw new InvalidDataException($"{ms.Length - ms.Position} unread bytes.");
    }

    private static byte[] Write(Action<GbxReaderWriter> serialize)
    {
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        using var rw = new GbxReaderWriter(w);
        serialize(rw);
        return ms.ToArray();
    }
}
