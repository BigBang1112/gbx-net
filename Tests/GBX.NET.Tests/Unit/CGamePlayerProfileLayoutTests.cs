using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CGamePlayerProfileLayoutTests
{
    [Test]
    public async Task PlayerTagsPreserveConsultationFlagsAndDisplayOrder()
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Config version.
            w.Write(3);
            String(w, "tag-first");
            w.Write(1);
            String(w, "tag-private");
            w.Write(0);
            String(w, "tag-last");
            w.Write(1);
            w.Write(2);
            w.Write(2); // Display the last tag first.
            w.Write(0);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C064();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(new CGamePlayerProfile.PlayerTagConfig().IsVisibleByOtherPlayers).IsTrue();
        await Assert.That(node.PlayerTags![1].TagId).IsEqualTo("tag-private");
        await Assert.That(node.PlayerTags[1].IsVisibleByOtherPlayers).IsFalse();
        await Assert.That(node.TagDisplayList).IsEquivalentTo(new[] { 2, 0 });
        await Assert.That(node.TagDisplayList![0]).IsEqualTo(2);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);

        var accountConfig = new CGamePlayerProfileChunk_AccountSettings.SPlayerTagsConfig();
        Read(payload, rw => accountConfig.ReadWrite(rw));
        await Assert.That(new CGamePlayerProfileChunk_AccountSettings.PlayerTagConfig().IsVisibleByOtherPlayers).IsTrue();
        await Assert.That(accountConfig.PlayerTags![1].IsVisibleByOtherPlayers).IsFalse();
        await Assert.That(accountConfig.TagDisplayList).IsEquivalentTo(new[] { 2, 0 });
        await Assert.That(accountConfig.TagDisplayList![0]).IsEqualTo(2);
        await Assert.That(Write(rw => accountConfig.ReadWrite(rw))).IsEquivalentTo(payload);
    }

    [Test]
    public async Task UnsupportedPlayerTagsVersionHasNoArrayPayload()
    {
        var payload = BitConverter.GetBytes(1);
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C064();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(1);
        await Assert.That(node.PlayerTags).IsNull();
        await Assert.That(node.TagDisplayList).IsNull();
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);

        var accountConfig = new CGamePlayerProfileChunk_AccountSettings.SPlayerTagsConfig();
        Read(payload, rw => accountConfig.ReadWrite(rw));
        await Assert.That(accountConfig.Version).IsEqualTo(1);
        await Assert.That(accountConfig.PlayerTags).IsNull();
        await Assert.That(accountConfig.TagDisplayList).IsNull();
        await Assert.That(Write(rw => accountConfig.ReadWrite(rw))).IsEquivalentTo(payload);
    }

    [Test]
    public async Task ProfileIdentitySharesItsUuidAcrossLegacyAndModernChunks()
    {
        const string profileId = "01234567-89ab-cdef-0123-456789abcdef";
        var identity = Payload(w =>
        {
            String(w, "Local profile");
            String(w, "$f00Nickname");
            w.Write(3); // ID table version.
            w.Write(0x40000000); // New string ID.
            String(w, profileId);
        });
        var payload = Payload(w =>
        {
            w.Write(identity);
            w.Write(7); // Reserved words are preserved even though TMF writes zero.
            w.Write(9);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C05B();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.ProfileId).IsEqualTo(profileId);
        await Assert.That(node.ProfileName).IsEqualTo("Local profile");
        await Assert.That(node.NickName).IsEqualTo("$f00Nickname");
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
        await Assert.That(Write(rw => new CGamePlayerProfile.Chunk0308C00F().ReadWrite(node, rw))).IsEquivalentTo(identity);

        var modernPayload = Payload(w =>
        {
            w.Write(2);
            w.Write(3);
            w.Write(0x40000000);
            String(w, profileId);
            String(w, "Local profile");
            w.Write(0); // No profile chunks.
        });
        var modern = new CGamePlayerProfile.Chunk0308C07E();
        await Assert.That(Write(rw => modern.ReadWrite(node, rw))).IsEquivalentTo(modernPayload);
        var restored = new CGamePlayerProfile();
        Read(modernPayload, rw => modern.ReadWrite(restored, rw));
        await Assert.That(restored.ProfileId).IsEqualTo(profileId);
    }

    [Test]
    public async Task TmfInterfaceSettingsShareEditorHelpAndPainterColorsWithLegacyChunks()
    {
        var colors = Enumerable.Range(0, 5).Select(i => new Vec3(i / 5f, (i + 1) / 6f, (i + 2) / 7f)).ToArray();
        var colorPayload = Payload(w =>
        {
            foreach (var color in colors)
            {
                w.Write(color.X);
                w.Write(color.Y);
                w.Write(color.Z);
            }
        });
        var payload = Payload(w =>
        {
            String(w, "Solo playlist");
            String(w, "Campaigns/Custom/Solo.txt");
            w.Write(0); // Editor help has already been shown.
            w.Write(colorPayload); // Five RGB triples, without an array length.
        });
        var node = new CGamePlayerProfile();
        await Assert.That(node.ShowEditorHelp).IsTrue();

        var chunk = new CGamePlayerProfile.Chunk0308C038();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.CurrentSoloPlaylistName).IsEqualTo("Solo playlist");
        await Assert.That(node.CurrentSoloPlaylistPath).IsEqualTo("Campaigns/Custom/Solo.txt");
        await Assert.That(node.ShowEditorHelp).IsFalse();
        var restoredColors = new[] { node.PainterCustomColor1, node.PainterCustomColor2, node.PainterCustomColor3, node.PainterCustomColor4, node.PainterCustomColor5 };
        await Assert.That(restoredColors).IsEquivalentTo(colors);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
        await Assert.That(Write(rw => new CGamePlayerProfile.Chunk0308C014().ReadWrite(node, rw))).IsEquivalentTo(BitConverter.GetBytes(0));
        await Assert.That(Write(rw => new CGamePlayerProfile.Chunk0308C021().ReadWrite(node, rw))).IsEquivalentTo(colorPayload);
        var colorsAndTrail = Payload(w =>
        {
            w.Write(colorPayload);
            w.Write(1f);
            w.Write(0f);
            w.Write(0f);
        });
        await Assert.That(Write(rw => new CGamePlayerProfile.Chunk0308C025().ReadWrite(node, rw))).IsEquivalentTo(colorsAndTrail);
    }

    [Test]
    public async Task TmfDisplaySettingsExposeTrailColorAndStereoParameters()
    {
        var payload = Payload(w =>
        {
            w.Write(1); // Chat.
            w.Write(0); // Avatars.
            w.Write(1); // Car skin geometry.
            w.Write(0); // Unlimited horns.
            w.Write(0.2f);
            w.Write(0.4f);
            w.Write(0.6f);
            w.Write(1);
            w.Write(0); // Unlock-all cheat.
            w.Write(0);
            w.Write(0.75f);
            w.Write(0.03f);
            w.Write(6f);
        });
        var node = new CGamePlayerProfile();
        var display = new CGamePlayerProfile.Chunk0308C063();
        var stereo = new CGamePlayerProfile.Chunk0308C060();
        void Serialize(GbxReaderWriter rw)
        {
            display.ReadWrite(node, rw);
            stereo.ReadWrite(node, rw);
        }
        Read(payload, Serialize);

        await Assert.That(node.TrailColor).IsEqualTo(new Vec3(0.2f, 0.4f, 0.6f));
        await Assert.That(node.EnableAvatars).IsFalse();
        await Assert.That(node.EnableUnlimitedHorns).IsFalse();
        await Assert.That(node.StereoscopyStrength01).IsEqualTo(0.75f);
        await Assert.That(node.StereoscopyAdvancedSeparation).IsEqualTo(0.03f);
        await Assert.That(node.StereoscopyAdvancedScreenDist).IsEqualTo(6f);
        await Assert.That(Write(Serialize)).IsEquivalentTo(payload);
    }

    [Test]
    public async Task TmfServerSettingsExposeServerCreationPreferences()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)24);
            w.Write((byte)12);
            w.Write(1);
            w.Write(10000);
            w.Write(1); // Show advanced server settings.
            w.Write(3); // Network game mode.
            w.Write(1);
            w.Write(0);
            w.Write(0);
            w.Write(0);
            for (var i = 0; i < 6; i++) w.Write(0);
            w.Write(0);
            w.Write(1); // Allow download.
            String(w, "Hosted server");
            w.Write(0);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C03B();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.MaxPlayerCount).IsEqualTo((byte)24);
        await Assert.That(node.MaxSpectatorCount).IsEqualTo((byte)12);
        await Assert.That(node.ShowAdvancedServerSettings).IsTrue();
        await Assert.That(node.NetworkGameMode).IsEqualTo(3);
        await Assert.That(node.AllowDownload).IsTrue();
        await Assert.That(node.ServerName).IsEqualTo("Hosted server");
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    public async Task TmfAccountFlagsExposeNicknameChangeCountAndConversionPrompt()
    {
        var payload = Payload(w =>
        {
            w.Write(-1);
            w.Write(2);
            w.Write(1);
        });
        var node = new CGamePlayerProfile();
        var counters = new CGamePlayerProfile.Chunk0308C050();
        var conversion = new CGamePlayerProfile.Chunk0308C06A();
        void Serialize(GbxReaderWriter rw)
        {
            counters.ReadWrite(node, rw);
            conversion.ReadWrite(node, rw);
        }
        Read(payload, Serialize);

        await Assert.That(node.OnlineRemainingNickNamesChangesCount).IsEqualTo(2);
        await Assert.That(node.AskForAccountConversion).IsTrue();
        await Assert.That(Write(Serialize)).IsEquivalentTo(payload);
    }

    [Test]
    public async Task MultiLocalProfileHeadersAreInlineVersionedArchives()
    {
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(1);
            String(w, "First profile");
            String(w, "$f00First nickname");
            String(w, "First avatar");
            w.Write(-1); // Version 1 has a node reference.
            w.Write(2);
            String(w, "Second profile");
            String(w, "Second nickname");
            String(w, "Second avatar");
            w.Write(2);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C04A();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.MultiLocalProfileHeaders!.Length).IsEqualTo(2);
        await Assert.That(node.MultiLocalProfileHeaders[0].Version).IsEqualTo(1);
        await Assert.That(node.MultiLocalProfileHeaders[1].ProfileName).IsEqualTo("Second profile");
        await Assert.That(chunk.U02).IsEqualTo(2);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    public async Task BuddyVersionZeroUsesPackedSystemTime()
    {
        var date = new DateTime(2025, 7, 15, 12, 34, 56, 789);
        var packed = (ulong)date.Year | ((ulong)date.Month << 16) | ((ulong)date.DayOfWeek << 20)
            | ((ulong)date.Day << 23) | ((ulong)date.Hour << 32) | ((ulong)date.Minute << 37)
            | ((ulong)date.Second << 43) | ((ulong)date.Millisecond << 49);
        var payload = Payload(w =>
        {
            w.Write(0); // Container version.
            w.Write(1);
            w.Write(0); // Each buddy has its own version.
            String(w, "buddy");
            w.Write(0);
            w.Write(packed);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C056();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.Buddies![0].U04).IsEqualTo(date);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    public async Task UnsupportedBuddyContainerVersionHasNoArrayPayload()
    {
        var payload = BitConverter.GetBytes(6);
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C056();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(6);
        await Assert.That(node.Buddies).IsNull();
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    [Arguments(0)]
    [Arguments(4)]
    public async Task CryptedPasswordUsesItsExplicitByteCount(int passwordLength)
    {
        var cipher = new byte[] { 0xC0, 0xDE, 0xFE, 0xED };
        var payload = Payload(w =>
        {
            String(w, "server comment");
            String(w, "$fffDescription");
            w.Write(7);
            w.Write(1);
            String(w, "login");
            w.Write(passwordLength);
            if (passwordLength > 0)
            {
                w.Write(Enumerable.Range(1, 16).Select(x => (byte)x).ToArray());
                w.Write(cipher);
            }
            String(w, "validation key");
            String(w, "support key");
            w.Write(1);
            w.Write(1);
            String(w, "server");
            String(w, "path");
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C069();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.Description).IsEqualTo("$fffDescription");
        await Assert.That(node.ServerComment).IsEqualTo("server comment");
        await Assert.That(node.VehicleNetQuality).IsEqualTo(7);
        await Assert.That(node.LadderMode).IsEqualTo(1);
        await Assert.That(node.OnlineValidationKeyHexa).IsEqualTo("validation key");
        await Assert.That(node.OnlineSupportKeyHexa).IsEqualTo("support key");
        await Assert.That(node.LastUsedMSAddress).IsEqualTo("server");
        await Assert.That(node.LoginValidated).IsTrue();
        await Assert.That(node.EncryptedPasswordLength).IsEqualTo(passwordLength);
        if (passwordLength > 0)
        {
            await Assert.That(node.EncryptedPassword).IsEquivalentTo(cipher);
        }
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task VehicleChecksumWidthDependsOnContainerVersion(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1);
            NullIdent(w);
            String(w, "skin");
            w.Write(Enumerable.Range(1, version == 0 ? 4 : version == 1 ? 16 : 32).Select(x => (byte)x).ToArray());
            w.Write(7);
            w.Write(0.25f);
            w.Write(0.75f);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C079();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.U01![0].GameCamera).IsEqualTo(7);
        await Assert.That(chunk.U01[0].AnalogSensitivity).IsEqualTo(0.25f);
        await Assert.That(chunk.U01[0].AnalogDeadZone).IsEqualTo(0.75f);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    [Test]
    [Arguments(false, 1)]
    [Arguments(true, 0)]
    [Arguments(true, 1)]
    [Arguments(true, 2)]
    public async Task ProfileContainersPreserveEmptyUnknownAndModernEntries(bool currentContainer, int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(3); // ID table version.
            w.Write(uint.MaxValue);
            String(w, "profile");
            w.Write(4);
            ProfileEntry(w, 0x0312C000, [], currentContainer && version >= 2);
            ProfileEntry(w, 0x0BADF000, [7, 8, 9, 10, 11], currentContainer && version >= 2);
            ProfileEntry(w, 0x0312D000, Payload(p => { p.Write(10); p.Write(10); p.Write(0xFACADE01u); }), currentContainer && version >= 2);
            ProfileEntry(w, 0x0312D000, [5, 0, 0, 0, 0xAA, 0xBB], currentContainer && version >= 2); // Unsupported old archive.
        });
        var node = new CGamePlayerProfile();
        var old = new CGamePlayerProfile.Chunk0308C07D();
        var current = new CGamePlayerProfile.Chunk0308C07E();
        void Serialize(GbxReaderWriter rw)
        {
            if (currentContainer) current.ReadWrite(node, rw);
            else old.ReadWrite(node, rw);
        }
        Read(payload, Serialize);

        await Assert.That(node.ProfileChunks!.Length).IsEqualTo(4);
        await Assert.That(node.ProfileChunks[0]).IsTypeOf<CGamePlayerProfileChunk_Unknown>();
        await Assert.That(((CGamePlayerProfileChunk_Unknown)node.ProfileChunks[1]).ClassId).IsEqualTo(0x0BADF000u);
        await Assert.That(node.ProfileChunks[2]).IsTypeOf<CGamePlayerProfileChunk_GameSettings>();
        await Assert.That(node.ProfileChunks[2].ArchiveVersion).IsEqualTo(10);
        await Assert.That(node.ProfileChunks[3]).IsTypeOf<CGamePlayerProfileChunk_Unknown>();
        await Assert.That(node.ProfileChunks[0].CreatedAt.HasValue).IsEqualTo(currentContainer && version >= 2);
        await Assert.That(Write(Serialize)).IsEquivalentTo(payload);

#pragma warning disable GBXNET10001
        var clone = (CGamePlayerProfile)node.DeepClone();
#pragma warning restore GBXNET10001
        var originalData = ((CGamePlayerProfileChunk_Unknown)node.ProfileChunks[1]).Data;
        var clonedData = ((CGamePlayerProfileChunk_Unknown)clone.ProfileChunks![1]).Data;
        await Assert.That(clonedData).IsEquivalentTo(originalData);
        await Assert.That(ReferenceEquals(clonedData, originalData)).IsFalse();
    }

    [Test]
    [Arguments(5)]
    [Arguments(6)]
    public async Task AccountArchivesBeforeNodeVersionKeepTheirOwnVersion(int archiveVersion)
    {
        var accountPayload = Payload(w =>
        {
            w.Write(archiveVersion);
            if (archiveVersion > 5) w.Write(9); // Compatibility version appears after beta version 5.
            String(w, "description");
            String(w, "nickname");
            w.Write((byte)0); // Flags.
            String(w, "private key");
            w.Write(0UL);
            w.Write(0);
            w.Write(0); // Buddy count.
            w.Write(0UL);
            for (var i = 0; i < 3; i++) { w.Write(10); w.Write(0); } // Deprecated node-reference arrays.
            w.Write((byte)0); // Flags2.
            String(w, "unused");
            String(w, "avatar");
            w.Write(0); // Player-tags config version.
            w.Write(0);
            w.Write(0);
            w.Write((byte)0); // ReceiveNews.
            w.Write(1); // EulaVersion.
        });
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(3);
            w.Write(uint.MaxValue);
            String(w, "profile");
            w.Write(1);
            ProfileEntry(w, 0x0312C000, accountPayload, hasCreationTime: true);
        });
        var node = new CGamePlayerProfile();
        var chunk = new CGamePlayerProfile.Chunk0308C07E();
        Read(payload, rw => chunk.ReadWrite(node, rw));

        var account = (CGamePlayerProfileChunk_AccountSettings)node.ProfileChunks![0];
        await Assert.That(account.Description).IsEqualTo("description");
        await Assert.That(account.EulaVersion).IsEqualTo(1);
        await Assert.That(Write(rw => chunk.ReadWrite(node, rw))).IsEquivalentTo(payload);
    }

    private static void ProfileEntry(BinaryWriter w, uint classId, byte[] data, bool hasCreationTime)
    {
        w.Write(classId);
        String(w, "name");
        String(w, "game");
        String(w, "checksum");
        w.Write(123u);
        if (hasCreationTime) w.Write(456u);
        w.Write(data.Length);
        w.Write(data);
    }

    private static void NullIdent(BinaryWriter w)
    {
        w.Write(3);
        w.Write(uint.MaxValue);
        w.Write(uint.MaxValue);
        w.Write(uint.MaxValue);
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
