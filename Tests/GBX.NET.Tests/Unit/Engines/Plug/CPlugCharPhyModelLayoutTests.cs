using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCharPhyModelLayoutTests
{
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
    [Arguments(21)]
    public async Task ReadWrite_MovementChunk_UsesNativeVersionBoundaries(int version)
    {
        var fields = new List<object> { (ushort)0x1234 };
        if (version < 2) fields.Add(1.25f);
        if (version < 13) fields.Add(2.5f);
        fields.AddRange([3.75f, -1]);
        if (version is >= 1 and <= 11) fields.Add(4.5f);
        if (version is >= 2 and <= 13) fields.Add(-1);
        if (version is >= 3 and <= 11) fields.Add(-1);
        if (version is >= 4 and <= 13) fields.Add(5.5f);
        if (version >= 5) fields.Add(6.5f);
        if (version is >= 6 and <= 12) fields.Add(-1);
        if (version is >= 7 and <= 12) fields.AddRange([-1, -1]);
        if (version >= 8) fields.AddRange([123, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f]);
        if (version is >= 9 and <= 10) fields.AddRange([15f, 16f, 17f, 18f]);
        if (version >= 10) fields.AddRange([234, 345, 19f, 20f, 21f, 22f]);
        if (version >= 13) fields.Add(23f);
        if (version >= 15) fields.Add(24f);
        if (version >= 16) fields.Add(-1);
        if (version == 17) fields.Add(25f);
        if (version >= 18) fields.Add(-1);
        if (version >= 19) fields.Add(456);
        if (version >= 20) fields.Add(true);
        if (version >= 21) fields.AddRange([26f, 27f, 28f, 29f, 30f, 31f]);
        await AssertPayloadRoundtrip(0x01C, version, fields);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(9)]
    [Arguments(10)]
    public async Task ReadWrite_JumpChunk_ConsumesTrailingNaturalOnlyFromVersionTen(int version)
    {
        var fields = new List<object> { true, false, 1f, 2f, 3f, 4f };
        if (version >= 1) fields.Add(5f);
        if (version is >= 2 and <= 5) fields.Add(123);
        if (version >= 3) fields.AddRange([6f, 234, -1]);
        if (version >= 4) fields.AddRange([345, -1]);
        if (version == 5) fields.Add(456);
        if (version >= 6) fields.Add(-1);
        if (version >= 7) fields.Add(7f);
        if (version >= 8) fields.Add(true);
        if (version >= 9) fields.Add(8f);
        if (version >= 10) fields.AddRange([false, 567]);
        await AssertPayloadRoundtrip(0x019, version, fields);
    }

    [Test]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(19)]
    public async Task ReadWrite_AccelerationChunk_OnlyVersionsFourteenAndFifteenHaveDeprecatedReference(int version)
    {
        var fields = new List<object> { -1, -1, 1f, 123, 234, 2f, 3f, -1, 4f, -1, 5f, true, -1, -1 };
        if (version >= 14) fields.Add(false);
        if (version is >= 14 and <= 15) fields.Add(-1);
        if (version >= 15) fields.Add(-1);
        if (version >= 16) fields.AddRange([345, 456, 6f]);
        if (version >= 17) fields.Add(567);
        if (version >= 18) fields.AddRange([678, 789]);
        if (version >= 19) fields.AddRange([890, 901]);
        await AssertPayloadRoundtrip(0x01B, version, fields);
    }

    [Test]
    [Arguments(0x01F, 5)]
    [Arguments(0x01F, 6)]
    [Arguments(0x020, 2)]
    [Arguments(0x022, 6)]
    [Arguments(0x023, 2)]
    [Arguments(0x023, 6)]
    [Arguments(0x028, 2)]
    [Arguments(0x028, 3)]
    [Arguments(0x028, 5)]
    [Arguments(0x02D, 7)]
    [Arguments(0x02F, 1)]
    public async Task ReadWrite_CorrectedChunks_PreserveNativeScalarsAndInlineSmoothing(int offset, int version)
    {
        var fields = offset switch
        {
            0x01F => new List<object> { true, -1, -1, -1, -1, 123, 234 },
            0x020 => [1.25f, true],
            0x022 => [1f, 123, 2f, 3f, -1, 234, 4f, 5f, 6f, 7f, 8f, true],
            0x023 => [1f, -1, -1, -1, 0, 0, 2f, 3f, 4f, 5f, 6f, 7f],
            0x028 => [1f, 2f, 3f, 4f, 5f, 6f, true, 7f, 8f, false, 9f, -1, true, false, -1, 10f],
            0x02D => [true, 123, 234, -1, false, true, 1f, false, 2f, true, false, true],
            0x02F => [true, 123, 234, (ushort)0x1234],
            _ => throw new ArgumentOutOfRangeException(nameof(offset))
        };
        if (offset == 0x023 && version >= 3) fields.AddRange([true, 123, 8f]);
        if (offset == 0x023 && version >= 4) fields.AddRange([9f, 10f, 11f]);
        if (offset == 0x023 && version >= 5) fields.AddRange([12f, 13f, 14f, 15f]);
        if (offset == 0x023 && version >= 6) fields.Add(16f);
        if (offset == 0x028 && version == 2) fields.AddRange([-1, 123]);
        if (offset == 0x028 && version >= 3) fields.AddRange([123, 234, -1]);
        if (offset == 0x028 && version >= 4) fields.Add(-1);
        if (offset == 0x028 && version >= 5) fields.Add(true);
        await AssertPayloadRoundtrip(offset, version, fields);
    }

    [Test]
    [Arguments(0x02C, 13)]
    [Arguments(0x02E, 4)]
    [Arguments(0x031, 4)]
    [Arguments(0x031, 5)]
    public async Task ReadWrite_ManiaPlanetTypedChunks_UseNativeBooleanAndNaturalFields(int offset, int version)
    {
        object[] curve = version >= 5 && offset == 0x031 ? [3, 0, (ushort)0] : [-1];
        var fields = offset switch
        {
            0x02C => new List<object>
            {
                true, 1f, false, true, true, false, true, 123, true, 234, 345, 456, false,
                1, true, 2, 3, false, 2f, 3f, 4, 4f, 5f,
                5, true, 6, 7, false, 6f, 7f, 8, 8f, 9, 9f, 10f
            },
            0x02E => [true, 1f, 2f, 3f, 4f, -1, 123, true, false, 5f, 6f, 7f, 8f, 9f],
            0x031 => [true, false, true, false, 123, 1f, true, 234, 345, 2f, .. curve, .. curve, .. curve, .. curve, .. curve, true, 456, 3f, .. curve, .. curve],
            _ => throw new ArgumentOutOfRangeException(nameof(offset))
        };
        await AssertPayloadRoundtrip(offset, version, fields);
    }

    [Test]
    public async Task ReadWrite_GrapnelChunk_MapsManiaPlanetMetaMembers()
    {
        var node = await AssertPayloadRoundtrip(0x02B, 6,
        [
            true, 0.5f, 40f, 30f, 2f, 0.25f,
            true, false, true, false, true, false,
            true, -1, -1, -1,
            false, 3f, 0.1f, 0.2f, 0.3f,
            true, false, 1500, -1, 0.4f,
            false, true, 0.6f
        ]);

        await Assert.That(node.GrapnelEnabled).IsTrue();
        await Assert.That(node.GrapnelCharPhyFrictionCoef).IsEqualTo(0.5f);
        await Assert.That(node.GrapnelInitialSpeed).IsEqualTo(40f);
        await Assert.That(node.GrapnelMaxDist).IsEqualTo(30f);
        await Assert.That(node.GrapnelMinDist).IsEqualTo(2f);
        await Assert.That(node.GrapnelRadius).IsEqualTo(0.25f);
        await Assert.That(node.GrapnelElastic).IsTrue();
        await Assert.That(node.GrapnelMouseStretchSpeed).IsEqualTo(3f);
        await Assert.That(node.GrapnelAfterGrapnelFrictionDuration).IsEqualTo(1500);
        await Assert.That(node.GrapnelOffsetDirYForce).IsEqualTo(0.4f);
        await Assert.That(node.GrapnelOnlyOnChar).IsTrue();
        await Assert.That(node.GrapnelDynaTractionForceCoef).IsEqualTo(0.6f);
    }

    [Test]
    public async Task ReadWrite_EquipmentChunks_MapManiaPlanetMetaMembers()
    {
        var powerBoots = await AssertPayloadRoundtrip(0x02D, 7, [true, 2000, 500, -1, true, false, 3f, true, 1.5f, false, true, true]);
        await Assert.That(powerBoots.PowerBootsAccelEnabled).IsTrue();
        await Assert.That(powerBoots.PowerBootsCooldown).IsEqualTo(2000);
        await Assert.That(powerBoots.PowerBootsDuration).IsEqualTo(500);
        await Assert.That(powerBoots.PowerBootsWalkOnWaterEnabled).IsTrue();
        await Assert.That(powerBoots.PowerBootsMinSpeedToStayOnGround).IsEqualTo(3f);
        await Assert.That(powerBoots.PowerBootsSuperJumpCoef).IsEqualTo(1.5f);
        await Assert.That(powerBoots.PowerBootsStrafeDisabled).IsTrue();
        await Assert.That(powerBoots.PowerBootsMaintainButtonDown).IsTrue();

        var jetPack = await AssertPayloadRoundtrip(0x02E, 4, [true, 1f, 2f, 3f, 4f, -1, 1, true, false, 10f, 20f, 0.5f, 90f, 45f]);
        await Assert.That(jetPack.JetPackEnabled).IsTrue();
        await Assert.That(jetPack.JetPackAirForceCoef).IsEqualTo(3f);
        await Assert.That(jetPack.JetPackTakeOffImpulse).IsEqualTo(4f);
        await Assert.That(jetPack.JetPackVersion2).IsEqualTo(1);
        await Assert.That(jetPack.JetPackClampRollValueDeg).IsEqualTo(20f);
        await Assert.That(jetPack.JetPackFrictionRotCoef).IsEqualTo(0.5f);
        await Assert.That(jetPack.JetPackRollRotationSpeedDegS).IsEqualTo(45f);

        var sniperTurret = await AssertPayloadRoundtrip(0x02F, 1, [true, 3000, 10000, (ushort)5]);
        await Assert.That(sniperTurret.SniperTurretEnabled).IsTrue();
        await Assert.That(sniperTurret.SniperTurretDuration).IsEqualTo(3000);
        await Assert.That(sniperTurret.SniperTurretCooldown).IsEqualTo(10000);
        await Assert.That(sniperTurret.SniperTurretAmmoGain).IsEqualTo((ushort)5);
    }

    [Test]
    [Arguments(0x000, GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    [Arguments(0x02C, GameVersion.MP4 | GameVersion.TM2020)]
    [Arguments(0x031, GameVersion.MP4 | GameVersion.TM2020)]
    public async Task GameVersion_ManiaPlanetWritableChunks_IncludeMP4(int offset, GameVersion expected)
    {
        var chunk = (IChunk)Activator.CreateInstance(
            typeof(CPlugCharPhyModel).GetNestedType($"Chunk{0x090C9000 + offset:X8}")!)!;

        await Assert.That(chunk.GameVersion).IsEqualTo(expected);
    }

    [Test]
    [Arguments("")]
    [Arguments("abc")]
    [Arguments("abcd")]
    [Arguments("éé")]
    public async Task ReadWrite_Name_InitializesCipherWithFirstFourUtf8Bytes(string name)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString(name);
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var initializer = new RecordingInitializer();
        var node = new CPlugCharPhyModel();
        var chunk = new CPlugCharPhyModel.Chunk090C9000();
        using (var reader = new GbxReader(payload, new GbxReadSettings { EncryptionInitializer = initializer }))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.Name).IsEqualTo(name);
        var nameBytes = Encoding.UTF8.GetBytes(name);
        var expected = nameBytes.Length >= 4 ? nameBytes[..4] : [];
        await Assert.That(initializer.Bytes).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    private static async Task<CPlugCharPhyModel> AssertPayloadRoundtrip(int offset, int version, List<object> fields)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            foreach (var field in fields)
            {
                switch (field)
                {
                    case bool boolean: writer.Write(boolean); break;
                    case int integer: writer.Write(integer); break;
                    case float real: writer.Write(real); break;
                    case ushort natural: writer.Write(natural); break;
                    default: throw new InvalidOperationException();
                }
            }
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var node = new CPlugCharPhyModel();
        var chunk = (Chunk<CPlugCharPhyModel>)Activator.CreateInstance(
            typeof(CPlugCharPhyModel).GetNestedType($"Chunk{0x090C9000 + offset:X8}")!)!;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
        return node;
    }

    private sealed class RecordingInitializer : IEncryptionInitializer
    {
        public List<byte> Bytes { get; } = [];

        public void Initialize(byte[] data, uint offset, uint count)
        {
            Bytes.AddRange(data.AsSpan((int)offset, (int)count).ToArray());
        }
    }
}
