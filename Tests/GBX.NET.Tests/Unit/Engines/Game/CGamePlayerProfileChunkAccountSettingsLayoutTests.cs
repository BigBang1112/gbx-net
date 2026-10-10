using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGamePlayerProfileChunkAccountSettingsLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task CredentialsPreserveEncryptedKeyAndVersionedConsent(int version)
    {
        const ulong packedTime = 0x0012345601234567;
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(9); // EULA.
            String(w, "$f00Player");
            w.Write((byte)3); // Validated login and remembered password.
            foreach (var value in new[] { "login", "password", "validation", "support", "master", "path", "session", "World|Europe" })
                String(w, value);
            w.Write(-1);
            w.Write(42);
            String(w, "00ABCDEF00112233"); // Hexadecimal ciphertext, preserved without decryption.
            String(w, "private");
            w.Write(packedTime); // SSystemTime::Archive writes exactly eight bytes.
            if (version >= 2) w.Write(1);
            if (version >= 3) { w.Write(7); w.Write(21); }
        });
        var node = new CGamePlayerProfileChunk_AccountSettings();
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C000();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.OnlineValidationCode).IsEqualTo("validation");
        await Assert.That(node.EncryptedKeyHexa).IsEqualTo("00ABCDEF00112233");
        await Assert.That(chunk.U01).IsEqualTo(packedTime);
        await Assert.That(node.PrivacyPolicyVersion).IsEqualTo(version >= 3 ? 7 : 0);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw)).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    public async Task WritingUnrememberedPasswordKeepsItInMemoryAndEmitsEmptyString()
    {
        var node = new CGamePlayerProfileChunk_AccountSettings
        {
            Flags = 1,
            OnlineLogin = "login",
            OnlinePassword = "memory-only"
        };
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C000 { Version = 1 };
        var payload = Write(rw => chunk.ReadWrite(node, rw));
        var restored = new CGamePlayerProfileChunk_AccountSettings();
        Read(payload, rw => chunk.ReadWrite(restored, rw));

        await Assert.That(node.OnlinePassword).IsEqualTo("memory-only");
        await Assert.That(restored.OnlinePassword).IsEqualTo(string.Empty);
        await Assert.That(restored.OnlineLogin).IsEqualTo("login");
    }

    [Test]
    [Arguments(0)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task OnlineCredentialsOmitLocalFieldsButKeepConsent(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(11);
            if (version >= 3) { w.Write(17); w.Write(30); }
        });
        var node = new CGamePlayerProfileChunk_AccountSettings { IsOnlineSaveArchivation = true };
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C000();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.EulaVersion).IsEqualTo(11);
        await Assert.That(node.PrivacyPolicyVersion).IsEqualTo(version >= 3 ? 17 : 0);
        await Assert.That(node.NickName).IsNull();
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw)).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(3, false)]
    [Arguments(4, false)]
    [Arguments(6, false)]
    [Arguments(7, false)]
    [Arguments(7, true)]
    public async Task BuddiesKeepTheirOwnHeadersAndVersionedLocalBuffers(int version, bool online)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(5); // Container buddy version.
            w.Write(1);
            w.Write(0); // Each buddy also has its own archive version.
            String(w, "buddy-login");
            w.Write(0);
            w.Write(0UL);
            String(w, "extra");
            if (version == 4) { w.Write(2); String(w, "first"); String(w, "second"); }
            if (!online && version >= 6) { w.Write(3); w.Write(new byte[] { 8, 9, 10 }); }
            if (!online && version >= 7) { w.Write(2); w.Write(new byte[] { 11, 12 }); }
        });
        var node = new CGamePlayerProfileChunk_AccountSettings { IsOnlineSaveArchivation = online };
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C003();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.Buddies![0].Login).IsEqualTo("buddy-login");
        await Assert.That(node.Buddies[0].Version).IsEqualTo(0);
        await Assert.That(node.BuddyArchiveVersion).IsEqualTo(5);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw)).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task MessagesUsePackedTimeAndEncapsulationStartingAtVersionTwo(int version)
    {
        var messages = Payload(w =>
        {
            w.Write(0UL);
            for (var i = 0; i < 3; i++) { w.Write(10); w.Write(0); }
        });
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 2) { w.Write(0); w.Write(messages.Length); }
            w.Write(messages);
        });
        var node = new CGamePlayerProfileChunk_AccountSettings();
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C005();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.InboxMessages).IsEmpty();
        await Assert.That(node.ReceivedMessagesAt).IsNull();
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw)).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    public async Task OnlineMessagesAndCheatsHaveOnlyTheirVersionHeader()
    {
        var node = new CGamePlayerProfileChunk_AccountSettings { IsOnlineSaveArchivation = true, Flags2 = 3 };
        var messages = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C005 { Version = 2 };
        var cheats = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C006 { Version = 1 };
        var payload = Payload(w => { w.Write(2); w.Write(1); });
        void Serialize(GbxReaderWriter rw)
        {
            messages.ReadWrite(node, rw);
            cheats.ReadWrite(node, rw);
        }
        Read(payload, Serialize);
        await Assert.That(node.Flags2).IsEqualTo((byte)3);
        await Assert.That(Write(Serialize).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task InputTuningsExposeNamedFieldsInNativeOrder(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0.2f); w.Write(0.3f);
            if (version >= 2) w.Write(1);
            w.Write(0.4f); w.Write(1); w.Write(0);
            w.Write(0.5f); w.Write(0.006f); w.Write(1);
            if (version >= 3) w.Write(0.7f);
            w.Write(0.8f); w.Write(0.9f);
        });
        var node = new CGamePlayerProfileChunk_AccountSettings();
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C00A();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.AnalogDeadZone).IsEqualTo(0.3f);
        await Assert.That(node.RumbleIntensity).IsEqualTo(0.4f);
        await Assert.That(node.MouseLookInvertY).IsTrue();
        await Assert.That(node.MouseScaleFreeLook).IsEqualTo(version >= 3 ? 0.7f : 1f);
        await Assert.That(node.MouseSensitivity_Laser).IsEqualTo(0.9f);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw)).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    public async Task Tm2020ChunkTenContainsTwoWordsAndPackedSystemTime()
    {
        var payload = Payload(w => { w.Write(0); w.Write(123); w.Write(456); w.Write(0x0012345601234567UL); });
        var node = new CGamePlayerProfileChunk_AccountSettings();
        var chunk = new CGamePlayerProfileChunk_AccountSettings.Chunk0312C010();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.U01).IsEqualTo(123);
        await Assert.That(chunk.U02).IsEqualTo(456);
        await Assert.That(chunk.U03).IsEqualTo(0x0012345601234567UL);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw)).SequenceEqual(payload)).IsTrue();
    }

    [Test]
    public async Task VersionEightReplacesLegacyBuddiesWithCurrentList()
    {
        var payload = Payload(w =>
        {
            String(w, "description"); String(w, "nickname");
            w.Write((byte)0);
            String(w, "private");
            w.Write(0UL);
            w.Write(5); w.Write(1);
            w.Write(0); String(w, "legacy-buddy"); w.Write(0); w.Write(0UL);
            w.Write(0UL);
            for (var i = 0; i < 3; i++) { w.Write(10); w.Write(0); }
            w.Write((byte)0);
            String(w, "extra"); String(w, "avatar");
            w.Write(0); w.Write(0); w.Write(0);
            w.Write((byte)0);
            w.Write(1);
            w.Write(5); w.Write(1);
            w.Write(0); String(w, "current-buddy"); w.Write(0); w.Write(0UL);
        });
        var node = new CGamePlayerProfileChunk_AccountSettings();
        Read(payload, rw => node.ReadWrite(rw, 8));

        await Assert.That(node.Buddies![0].Login).IsEqualTo("current-buddy");
        var restored = new CGamePlayerProfileChunk_AccountSettings();
        Read(Write(rw => node.ReadWrite(rw, 8)), rw => restored.ReadWrite(rw, 8));
        await Assert.That(restored.Buddies![0].Login).IsEqualTo("current-buddy");
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(3)]
    [Arguments(5)]
    [Arguments(8)]
    public async Task LegacyOnlineArchiveKeepsOnlySharedProfileData(int version)
    {
        var payload = Payload(w =>
        {
            String(w, "description");
            if (version >= 3) String(w, "extra");
            String(w, "avatar");
            w.Write(0); w.Write(0); w.Write(0); // Empty player-tags config.
            w.Write((byte)1);
            if (version <= 1) w.Write(0);
            if (version >= 5) w.Write(23);
            if (version >= 8) { w.Write(5); w.Write(0); }
        });
        var node = new CGamePlayerProfileChunk_AccountSettings { IsOnlineSaveArchivation = true };
        Read(payload, rw => node.ReadWrite(rw, version));

        await Assert.That(node.Description).IsEqualTo("description");
        await Assert.That(node.ReceiveNews).IsTrue();
        await Assert.That(Write(rw => node.ReadWrite(rw, version)).SequenceEqual(payload)).IsTrue();
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        write(w);
        return ms.ToArray();
    }

    private static void String(BinaryWriter w, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        w.Write(bytes.Length);
        w.Write(bytes);
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
