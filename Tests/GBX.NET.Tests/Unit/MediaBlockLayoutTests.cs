using GBX.NET.Engines.Game;
using GBX.NET.Engines.TrackMania;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class MediaBlockLayoutTests
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
    public async Task CustomCameraKeysPreserveEveryNativeVersion(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            w.Write(2);
            if (version <= 5)
            {
                w.Write(17);
                w.Write(19);
                Floats(w, 7);
            }
            w.Write(1);
            if (version == 4)
            {
                w.Write(3); // Lookback-string version.
                w.Write(uint.MaxValue);
            }
            else
            {
                w.Write(-1);
            }
            w.Write(0);
            w.Write(-1);
            if (version <= 5)
            {
                Floats(w, 3);
                if (version == 1)
                {
                    Floats(w, 2);
                }
                else if (version >= 2)
                {
                    Floats(w, 6);
                    if (version == 3)
                    {
                        Floats(w, 5);
                    }
                }
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    Floats(w, version >= 7 ? 11 : 10);
                    if (version >= 10)
                    {
                        w.Write(101f + i);
                    }
                }
                if (version == 8)
                {
                    w.Write(23);
                    w.Write(29);
                }
            }
        });
        var key = new CGameCtnMediaBlockCameraCustom.Key();

        await RoundTrip(payload, rw => key.ReadWrite(rw, version));
        await Assert.That(key.Time.TotalSeconds).IsEqualTo(1.25f);
        if (version == 8)
        {
            await Assert.That(key.LegacyTargetSceneUId).IsEqualTo(23);
            await Assert.That(key.LegacyAnchorSceneUId).IsEqualTo(29);
        }
        if (version == 10)
        {
            await Assert.That(key.PssmDistScale).IsEqualTo(101f);
            await Assert.That(key.LeftTangent!.PssmDistScale).IsEqualTo(102f);
            await Assert.That(key.RightTangent!.PssmDistScale).IsEqualTo(103f);
        }
    }

    [Test]
    public async Task ScriptCameraVersionZeroEndsBeforeAnyKeys()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(0); // Empty script string.
            w.Write(1.25f);
            w.Write(2.5f);
        });
        var block = new CGameCtnMediaBlockCameraEffectScript();
        var chunk = new CGameCtnMediaBlockCameraEffectScript.Chunk03161000();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Start!.Value.TotalSeconds).IsEqualTo(1.25f);
        await Assert.That(block.End!.Value.TotalSeconds).IsEqualTo(2.5f);
    }

    [Test]
    [Arguments(0, -1, 0)]
    [Arguments(1, -1, 0)]
    [Arguments(2, 0, 0)]
    [Arguments(2, 0, 1)]
    [Arguments(2, 1, 0)]
    [Arguments(2, 1, 1)]
    [Arguments(2, 1, 2)]
    [Arguments(2, 1, 3)]
    [Arguments(2, 2, 0)]
    [Arguments(2, 2, 1)]
    [Arguments(2, 2, 2)]
    [Arguments(2, 2, 3)]
    public async Task CapturableColoringPreservesOldFieldOrderAndModernWidths(int version, int keyVersion, int flags)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 2)
            {
                w.Write(keyVersion);
            }
            w.Write(1);
            w.Write(1.25f);
            w.Write(keyVersion < 0 ? 0.75f : 0.25f);
            w.Write(keyVersion < 0 ? 0.25f : 0.75f);
            if (keyVersion >= 0)
            {
                w.Write((ushort)60000);
                w.Write((byte)(flags & 1));
                if (keyVersion >= 1)
                {
                    w.Write((byte)((flags >> 1) & 1));
                }
            }
            if (version >= 1)
            {
                w.Write(37);
            }
        });
        var block = new CGameCtnMediaBlockColoringCapturable();
        var chunk = new CGameCtnMediaBlockColoringCapturable.Chunk0316C000();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys![0].Hue).IsEqualTo(0.25f);
        await Assert.That(block.Keys[0].Gauge).IsEqualTo(0.75f);
        await Assert.That(block.Keys[0].EmblemBlink).IsEqualTo((flags & 1) != 0);
        await Assert.That(block.Keys[0].FullIntensity).IsEqualTo((flags & 2) != 0);
        if (keyVersion >= 0)
        {
            await Assert.That(block.Keys[0].Emblem).IsEqualTo((ushort)60000);
        }
    }

    [Test]
    [Arguments(0, -1, 0)]
    [Arguments(1, -1, 0)]
    [Arguments(2, 0, 0)]
    [Arguments(2, 1, 0)]
    [Arguments(2, 2, 60000)]
    [Arguments(2, 2, 65535)]
    public async Task BaseColoringPreservesEmblemVersionsAndUnsignedRange(int version, int keyVersion, int emblem)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 2)
            {
                w.Write(keyVersion);
            }
            w.Write(1);
            w.Write(1.25f);
            w.Write(0.25f);
            if (keyVersion >= 0)
            {
                w.Write(0.75f);
            }
            if (keyVersion >= 2)
            {
                w.Write((ushort)emblem);
            }
            if (version >= 1)
            {
                w.Write(37);
            }
        });
        var block = new CGameCtnMediaBlockColoringBase();
        var chunk = new CGameCtnMediaBlockColoringBase.Chunk03172000();
        var defaultKey = new CGameCtnMediaBlockColoringBase.Key();

        await Assert.That(defaultKey.Emblem).IsEqualTo((ushort)0);
        await Assert.That(defaultKey.Intensity).IsEqualTo(1f);
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys![0].Hue).IsEqualTo(0.25f);
        await Assert.That(block.Keys[0].Intensity).IsEqualTo(keyVersion >= 0 ? 0.75f : 1f);
        await Assert.That(block.Keys[0].Emblem).IsEqualTo((ushort)emblem);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ToneMappingKeysPreserveLegacyParameters(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            if (version == 3)
            {
                w.Write(1);
            }
            Floats(w, version switch { 0 => 2, 1 => 3, 2 or 3 => 4, _ => 3 });
            if (version >= 3)
            {
                w.Write(41);
            }
        });
        var key = new CGameCtnMediaBlockToneMapping.Key();

        await Assert.That(key.LightTrailScale).IsEqualTo(8f);
        await RoundTrip(payload, rw => key.ReadWrite(rw, version));
        await Assert.That(key.Time.TotalSeconds).IsEqualTo(1.25f);
        await Assert.That(key.ExposureBias).IsEqualTo(0.25f);
        await Assert.That(key.U02).IsEqualTo(version == 3);
        await Assert.That(key.U03).IsEqualTo(version < 4 ? 1.25f : 0f);
        await Assert.That(key.MaxHDR).IsEqualTo(version switch { 0 => 0f, 4 => 1.25f, _ => 2.25f });
        await Assert.That(key.U04).IsEqualTo(version is 2 or 3 ? 3.25f : 0f);
        await Assert.That(key.LightTrailScale).IsEqualTo(version == 4 ? 2.25f : 8f);
        if (version >= 3)
        {
            await Assert.That((int)key.FilmCurve).IsEqualTo(41);
        }
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    [Arguments(4, false)]
    public async Task ToneMappingChunksSelectTheirFixedKeyArchive(int version, bool legacyFlag)
    {
        var payload = Payload(w =>
        {
            w.Write(2); // Key count, with no payload version word.
            for (var i = 0; i < 2; i++)
            {
                w.Write(i + 1.25f);
                if (version == 3)
                {
                    w.Write(legacyFlag ? 1 : 0); // 32-bit boolean.
                }
                w.Write(i - 1.5f); // Exposure bias can be negative.
                if (version < 4)
                {
                    w.Write(11.25f + i);
                }
                if (version >= 1)
                {
                    w.Write(21.25f + i);
                }
                if (version is 2 or 3)
                {
                    w.Write(31.25f + i);
                }
                if (version == 4)
                {
                    w.Write(41.25f + i);
                }
                if (version >= 3)
                {
                    w.Write(i + 1);
                }
            }
        });
        var block = new CGameCtnMediaBlockToneMapping();
        CGameCtnMediaBlockToneMapping.Chunk03127000 chunk = version switch
        {
            0 => new(),
            1 => new CGameCtnMediaBlockToneMapping.Chunk03127001(),
            2 => new CGameCtnMediaBlockToneMapping.Chunk03127002(),
            3 => new CGameCtnMediaBlockToneMapping.Chunk03127003(),
            _ => new CGameCtnMediaBlockToneMapping.Chunk03127004()
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys!.Count).IsEqualTo(2);
        for (var i = 0; i < 2; i++)
        {
            var key = block.Keys[i];
            await Assert.That(key.Time.TotalSeconds).IsEqualTo(i + 1.25f);
            await Assert.That(key.ExposureBias).IsEqualTo(i - 1.5f);
            await Assert.That(key.U02).IsEqualTo(version == 3 && legacyFlag);
            await Assert.That(key.U03).IsEqualTo(version < 4 ? 11.25f + i : 0f);
            await Assert.That(key.MaxHDR).IsEqualTo(version >= 1 ? 21.25f + i : 0f);
            await Assert.That(key.U04).IsEqualTo(version is 2 or 3 ? 31.25f + i : 0f);
            await Assert.That(key.LightTrailScale).IsEqualTo(version == 4 ? 41.25f + i : 8f);
            await Assert.That((int)key.FilmCurve).IsEqualTo(version >= 3 ? i + 1 : 0);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task TrianglesPreserveThePositionMatrixAndOptionalTrailer(bool hasTrailer)
    {
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(1.25f);
            w.Write(2.5f);
            w.Write(2);
            w.Write(3);
            Floats(w, 18);
            w.Write(3);
            Floats(w, 12);
            w.Write(1);
            w.Write(0);
            w.Write(1);
            w.Write(2);
            if (hasTrailer)
            {
                w.Write(43);
                Floats(w, 6);
            }
        });
        var block = new CGameCtnMediaBlockTriangles();
        var chunk = hasTrailer
            ? new CGameCtnMediaBlockTriangles.Chunk03029001()
            : new CGameCtnMediaBlockTriangles.Chunk03029000();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys.Count).IsEqualTo(2);
        await Assert.That(block.Keys[1].Positions.Length).IsEqualTo(3);
        await Assert.That(block.Vertices.Length).IsEqualTo(3);
        await Assert.That(block.Triangles[0]).IsEqualTo(new Int3(0, 1, 2));
        if (hasTrailer)
        {
            await Assert.That((int)block.PickMode3D).IsEqualTo(43);
            await Assert.That(block.PickPlaneNormal).IsEqualTo(new Vec3(0.25f, 1.25f, 2.25f));
            await Assert.That(block.PickPlanePosition).IsEqualTo(new Vec3(3.25f, 4.25f, 5.25f));
        }
        block.Keys[0].Positions = new Vec3[4];
        await Assert.That(block.Vertices.Length).IsEqualTo(4);
        await Assert.That(block.Keys[1].Positions.Length).IsEqualTo(4);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(43)]
    public async Task TrianglesPreserveEditorPickingSettingsWithoutGeometry(int pickMode)
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Key count.
            w.Write(0); // Position matrix rows.
            w.Write(0); // Position matrix columns.
            w.Write(0); // Vertex color count.
            w.Write(0); // Triangle count.
            w.Write(pickMode);
            w.Write(-0.25f);
            w.Write(0.5f);
            w.Write(0.75f);
            w.Write(2f);
            w.Write(-3f);
            w.Write(4f);
        });
        var block = new CGameCtnMediaBlockTriangles();
        var chunk = new CGameCtnMediaBlockTriangles.Chunk03029001();

        await Assert.That(block.PickMode3D).IsEqualTo(CGameCtnMediaBlockTriangles.E3DPickMode.Plane);
        await Assert.That(block.PickPlaneNormal).IsEqualTo(new Vec3(0, 0, 1));
        await Assert.That(block.PickPlanePosition).IsEqualTo(default(Vec3));
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys.Count).IsEqualTo(0);
        await Assert.That(block.Vertices.Length).IsEqualTo(0);
        await Assert.That(block.Triangles.Length).IsEqualTo(0);
        await Assert.That((int)block.PickMode3D).IsEqualTo(pickMode);
        await Assert.That(block.PickPlaneNormal).IsEqualTo(new Vec3(-0.25f, 0.5f, 0.75f));
        await Assert.That(block.PickPlanePosition).IsEqualTo(new Vec3(2, -3, 4));
    }

    [Test]
    [Arguments(0, 12345)]
    [Arguments(1, 12345)]
    [Arguments(2, 12345)]
    [Arguments(3, 12345)]
    [Arguments(3, 123456789)]
    public async Task RaceEventsUseTheirStoredVersionAndIntegerRaceTimes(int version, int milliseconds)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            w.Write((byte)version);
            if (version == 0)
            {
                w.Write(1); // Checkpoint event.
                w.Write((byte)7);
                w.Write(0);
                w.Write(0);
            }
            else
            {
                if (version < 3)
                {
                    w.Write(47);
                }
                else
                {
                    w.Write((byte)7);
                }
                w.Write(1);
            }
            w.Write(0); // Checkpoint payload version.
            w.Write(milliseconds); // Milliseconds, not float seconds.
            w.Write(2);
            w.Write(3);
            w.Write(4);
        });
        var key = new CCtnMediaBlockEventTrackMania.Event();

        await RoundTrip(payload, rw => key.ReadWrite(rw, 99));
        await Assert.That(key.Version).IsEqualTo((byte)version);
        await Assert.That(key.Checkpoint!.RaceTime.TotalMilliseconds).IsEqualTo(milliseconds);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task FxColorKeysUseTheParametersForTheirChunk(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            if (version >= 1)
            {
                w.Write(0.75f);
            }
            if (version == 3)
            {
                Floats(w, 3);
            }
            Floats(w, version switch { 0 or 1 => 8, 2 => 12, _ => 24 });
        });
        var key = new CGameCtnMediaBlockFxColors.Key();

        await RoundTrip(payload, rw => key.ReadWrite(rw, version));
        await Assert.That(key.Inverse).IsEqualTo(0.25f);
        await Assert.That(key.Intensity).IsEqualTo(version == 0 ? 1f : 0.75f);
        await Assert.That(key.ModulateRgb).IsEqualTo(new Vec3(5.25f, 6.25f, 7.25f));
        if (version >= 2)
        {
            await Assert.That(key.BlendRgb).IsEqualTo(new Vec3(8.25f, 9.25f, 10.25f));
            await Assert.That(key.BlendAlpha).IsEqualTo(11.25f);
        }
        if (version == 3)
        {
            await Assert.That(key.FarIntensity).IsEqualTo(0.25f);
            await Assert.That(key.FarBlendRgb).IsEqualTo(new Vec3(20.25f, 21.25f, 22.25f));
            await Assert.That(key.FarBlendAlpha).IsEqualTo(23.25f);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task HdrBloomKeysPreserveLegacyLengths(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            Floats(w, version == 0 ? 1 : 3);
        });
        var key = new CGameCtnMediaBlockBloomHdr.Key();

        await RoundTrip(payload, rw => key.ReadWrite(rw, version));
        await Assert.That(key.Intensity).IsEqualTo(0.25f);
    }

    [Test]
    [Arguments(0, 0f)]
    [Arguments(1, 0f)]
    [Arguments(2, 0f)]
    [Arguments(3, 0f)]
    [Arguments(4, 0f)]
    [Arguments(5, 0.75f)]
    [Arguments(5, 0f)]
    [Arguments(5, 1f)]
    [Arguments(5, 1.5f)]
    public async Task GhostVersionsPreserveFlagsLightsAndTrailIntensity(int version, float trailIntensity)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 3)
            {
                w.Write(1);
                w.Write(1.25f);
                if (version == 3)
                {
                    w.Write(0.75f);
                }
                else
                {
                    w.Write(2);
                }
            }
            else
            {
                w.Write(1.25f);
                w.Write(2.5f);
            }
            w.Write(-1); // Null ghost model.
            w.Write(0.5f);
            w.Write(1);
            if (version >= 1)
            {
                w.Write(0);
            }
            if (version >= 2)
            {
                w.Write(1);
            }
            if (version >= 5)
            {
                w.Write(trailIntensity);
            }
        });
        var block = new CGameCtnMediaBlockGhost();
        var chunk = new CGameCtnMediaBlockGhost.Chunk030E5002();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.NoDamage).IsTrue();
        if (version >= 4)
        {
            await Assert.That(block.Keys![0].Lights).IsEqualTo(2);
        }
        if (version >= 5)
        {
            await Assert.That(block.TrailIntensity).IsEqualTo(trailIntensity);
        }
    }

    [Test]
    public async Task CameraMapMarkerPrecedesTheTransform()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)7);
            Floats(w, 15);
            w.Write((byte)3); // PackDesc version.
            w.Write(new byte[32]); // Checksum.
            w.Write(0); // Empty path.
            w.Write(0); // Empty locator URL.
        });
        var block = new CGameCtnMediaBlockFxCameraMap();
        var chunk = new CGameCtnMediaBlockFxCameraMap.Chunk03139001();

        await Assert.That(block.Transform).IsEqualTo(Iso4.Identity);
        await Assert.That(block.Fov).IsEqualTo(90f);
        await Assert.That(block.NearZ).IsEqualTo(-1f);
        await Assert.That(block.FarZ).IsEqualTo(-1f);
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.ArchiveMarker).IsEqualTo((byte)7);
        await Assert.That(block.Transform.XX).IsEqualTo(0.25f);
        await Assert.That(block.Transform.TZ).IsEqualTo(11.25f);
        await Assert.That(block.Fov).IsEqualTo(12.25f);
        await Assert.That(block.NearZ).IsEqualTo(13.25f);
        await Assert.That(block.FarZ).IsEqualTo(14.25f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ObjectVisibilityPrecedesBothObjectFormats(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1.25f);
            w.Write(2.5f);
            w.Write(0); // ShowObjects.
            if (version == 0)
            {
                w.Write(1);
                w.Write(1);
                w.Write(57);
                Floats(w, 3);
                w.Write(17);
            }
            else
            {
                w.Write(-1); // Null replay-object data.
            }
        });
        var block = new CGameCtnMediaBlockObject();
        var chunk = new CGameCtnMediaBlockObject.Chunk03196000();

        await Assert.That(block.ShowObjects).IsTrue();
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.ShowObjects).IsFalse();
    }

    [Test]
    [Arguments(-1f)]
    [Arguments(0f)]
    [Arguments(0.75f)]
    [Arguments(64f)]
    public async Task SceneryKeysPreserveVortexRadiusAndWorldCenter(float radius)
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Chunk version.
            w.Write(1); // Key count.
            w.Write(1.25f);
            w.Write(radius);
            w.Write(12.5f); // World X.
            w.Write(-24.75f); // World Z.
        });
        var block = new CGameCtnMediaBlockScenery();
        var chunk = new CGameCtnMediaBlockScenery.Chunk03188000();

        await Assert.That(new CGameCtnMediaBlockScenery.Key().VortexRadius).IsEqualTo(-1f);
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys![0].Time.TotalSeconds).IsEqualTo(1.25f);
        await Assert.That(block.Keys[0].VortexRadius).IsEqualTo(radius);
        await Assert.That(block.Keys[0].VortexCenterXZ).IsEqualTo(new Vec2(12.5f, -24.75f));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(11)]
    public async Task SceneryCapturableCountPrecedesTheDataTapeReference(int count)
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Chunk version.
            w.Write(count);
            w.Write(-1); // Null data tape.
        });
        var block = new CGameCtnMediaBlockScenery();
        var chunk = new CGameCtnMediaBlockScenery.Chunk03188001();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.CapturableCount).IsEqualTo(count);
        await Assert.That(block.DataTape).IsNull();
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(1, 1)]
    [Arguments(2, 1)]
    [Arguments(3, 1)]
    [Arguments(4, 1)]
    [Arguments(5, 1)]
    [Arguments(5, 0)]
    [Arguments(5, 2)]
    public async Task SkelVersionsPreserveRootTransformsJointsAndLegacyFields(int version, int jointCount)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1); // Key count.
            w.Write(1.25f);
            if (version >= 2)
            {
                w.Write(10.25f);
                w.Write(11.25f);
                w.Write(12.25f);
                w.Write(13.25f);
            }
            w.Write(jointCount);
            for (var joint = 0; joint < jointCount; joint++)
            {
                for (var component = 0; component < 7; component++)
                {
                    w.Write(joint * 10 + component + 0.25f);
                }
            }
            if (version >= 1)
            {
                w.Write(53);
            }
            if (version == 4)
            {
                w.Write(17);
            }
            if (version == 3 || version == 4)
            {
                w.Write(1.5f);
                w.Write(2.5f);
                w.Write(3.5f);
                w.Write(4.5f);
                if (version == 4)
                {
                    w.Write(0.75f);
                }
            }
        });
        var block = new CGameCtnMediaBlockSkel();
        var chunk = new CGameCtnMediaBlockSkel.Chunk0314A000();

        await Assert.That(chunk.Version).IsEqualTo(5);
        await Assert.That(chunk.U01).IsEqualTo(8);
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        var key = block.Keys![0];
        await Assert.That(key.Time.TotalSeconds).IsEqualTo(1.25f);
        await Assert.That(key.RootTranslation).IsEqualTo(version >= 2 ? new Vec3(10.25f, 11.25f, 12.25f) : Vec3.Zero);
        await Assert.That(key.RootYaw).IsEqualTo(version >= 2 ? 13.25f : 0f);
        await Assert.That(key.JointTransforms!.Length).IsEqualTo(jointCount);
        for (var joint = 0; joint < jointCount; joint++)
        {
            var first = joint * 10 + 0.25f;
            await Assert.That(key.JointTransforms[joint]).IsEqualTo(new TransQuat(first, first + 1, first + 2, first + 3, first + 4, first + 5, first + 6));
        }
        await Assert.That(chunk.U01).IsEqualTo(version >= 1 ? 53 : 8);
        if (version == 3 || version == 4)
        {
            await Assert.That(block.LegacyRootTranslationX).IsEqualTo(1.5f);
            await Assert.That(block.LegacyRootTranslationY).IsEqualTo(2.5f);
            await Assert.That(block.LegacyRootTranslationZ).IsEqualTo(3.5f);
            await Assert.That(block.LegacyRootYaw).IsEqualTo(4.5f);
        }
        if (version == 4)
        {
            await Assert.That(chunk.U02).IsEqualTo(17);
            await Assert.That(chunk.U07).IsEqualTo(0.75f);
        }
    }

    [Test]
    public async Task FadeColorRetainsItsStoredAlphaSeparatelyFromKeyOpacity()
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(1.25f);
            w.Write(0.5f);
            Floats(w, 3);
            w.Write(0.75f);
        });
        var block = new CGameCtnMediaBlockTransitionFade();
        var chunk = new CGameCtnMediaBlockTransitionFade.Chunk030AB000();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Color).IsEqualTo(new Vec4(0.25f, 1.25f, 2.25f, 0.75f));
        await Assert.That(block.Keys![0].Opacity).IsEqualTo(0.5f);
    }

    [Test]
    [Arguments(2)]
    [Arguments(3)]
    public async Task SimpleCameraExposesItsInterpolationFlagOnTheBlock(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0); // Keys.
            w.Write(-1);
            w.Write(-1);
            w.Write(1); // Smooth interpolation.
        });
        var block = new CGameCtnMediaBlockCameraSimple();
        var chunk = new CGameCtnMediaBlockCameraSimple.Chunk0309F002();

        await Assert.That(block.UseSmoothInterpolation).IsTrue();
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.UseSmoothInterpolation).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task OrbitalCameraPreservesTheScalarAndTargetOffset(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1);
            w.Write(1.25f);
            w.Write((byte)0);
            Floats(w, 9);
            w.Write(-1); // Clip entity ID.
            if (version >= 1)
            {
                Floats(w, 4);
            }
        });
        var block = new CGameCtnMediaBlockCameraOrbital();
        var chunk = new CGameCtnMediaBlockCameraOrbital.Chunk030A0001();

        await Assert.That(new CGameCtnMediaBlockCameraOrbital.Key().TargetOffset).IsEqualTo(Vec3.Zero);
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Keys![0].ClipEntId).IsEqualTo(-1);
        await Assert.That(block.Keys[0].TargetOffset).IsEqualTo(version >= 1 ? new Vec3(1.25f, 2.25f, 3.25f) : Vec3.Zero);
        if (version >= 1)
        {
            await Assert.That(block.Keys[0].U03).IsEqualTo(0.25f);
        }
    }

    [Test]
    [Arguments(0, 5)]
    [Arguments(0, 2)]
    [Arguments(1, 5)]
    [Arguments(1, 2)]
    [Arguments(2, 5)]
    [Arguments(2, 2)]
    [Arguments(3, 5)]
    [Arguments(3, 2)]
    [Arguments(4, 5)]
    [Arguments(4, 2)]
    public async Task GameCameraPreservesHudPingAndCrosshairSettings(int version, int flags)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1.25f);
            w.Write(2.5f);
            if (version <= 1)
            {
                w.Write(3); // Lookback-string version.
                w.Write(uint.MaxValue); // Null camera identifier.
            }
            else
            {
                w.Write(0); // Default camera.
            }
            w.Write(-1); // Clip entity ID.
            Floats(w, 11);
            w.Write(flags & 1);
            w.Write((flags >> 1) & 1);
            w.Write((flags >> 2) & 1);
            if (version >= 1)
            {
                w.Write(1.5f);
            }
            if (version >= 3)
            {
                w.Write(0x0FF00000); // Discarded scene UID.
            }
        });
        var block = new CGameCtnMediaBlockCameraGame();
        var chunk = new CGameCtnMediaBlockCameraGame.Chunk03084007();

        await Assert.That(block.ShowHUD).IsTrue();
        await Assert.That(block.ShowPing).IsTrue();
        await Assert.That(block.ShowCrossHair).IsTrue();
        await Assert.That(block.CrossHairSizeScale).IsEqualTo(1f);
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.ShowHUD).IsEqualTo((flags & 1) != 0);
        await Assert.That(block.ShowPing).IsEqualTo((flags & 2) != 0);
        await Assert.That(block.ShowCrossHair).IsEqualTo((flags & 4) != 0);
        await Assert.That(block.CrossHairSizeScale).IsEqualTo(version >= 1 ? 1.5f : 1f);
    }

    [Test]
    [Arguments(false, 0.25f, 0)]
    [Arguments(true, 0.75f, 1)]
    public async Task DecalsPreserveOpacityFlipUAndImageIndex(bool flipU, float opacity, int imageIndex)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            w.Write(2.5f);
            w.Write(2); // Image count.
            for (var i = 0; i < 2; i++)
            {
                w.Write((byte)3); // PackDesc version.
                w.Write(new byte[32]); // Checksum.
                w.Write(0); // Empty path.
                w.Write(0); // Empty locator URL.
            }
            w.Write(1); // Decal count.
            Floats(w, 15); // Oriented-box transform and scale.
            w.Write(opacity);
            w.Write(flipU ? 1 : 0); // A 32-bit boolean, separate from the image index.
            w.Write(imageIndex);
        });
        var block = new CGameCtnMediaBlockDecal2d();
        var chunk = new CGameCtnMediaBlockDecal2d.Chunk031AA000();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.Decals![0].Transform.XX).IsEqualTo(0.25f);
        await Assert.That(block.Decals[0].Transform.TZ).IsEqualTo(11.25f);
        await Assert.That(block.Decals[0].Scale.X).IsEqualTo(12.25f);
        await Assert.That(block.Decals[0].Opacity).IsEqualTo(opacity);
        await Assert.That(block.Decals[0].FlipU).IsEqualTo(flipU);
        await Assert.That(block.Decals[0].ImageIndex).IsEqualTo(imageIndex);
    }

    [Test]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    public async Task EntityKeysPreserveTrailColorVersionsAndIntensityDefaults(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(1.25f);
            w.Write(1); // Lights on.
            if (version >= 6)
            {
                w.Write(0.25f);
                w.Write(0.5f);
                w.Write(0.75f);
                w.Write(0.9f);
            }
            if (version >= 9)
            {
                w.Write(1.5f);
            }
        });
        var key = new CGameCtnMediaBlockEntity.Key();

        await RoundTrip(payload, rw => key.ReadWrite(rw, version));
        await Assert.That(key.Lights).IsEqualTo(CGameCtnMediaBlockEntity.ELights.On);
        await Assert.That(key.TrailColor!.Value).IsEqualTo(version >= 6 ? new Vec3(0.25f, 0.5f, 0.75f) : new Vec3(1, 0, 0));
        await Assert.That(key.TrailIntensity).IsEqualTo(version >= 6 ? 0.9f : 1f);
        await Assert.That(key.SelfIllumIntensity).IsEqualTo(version >= 9 ? 1.5f : 1f);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task EntityLegacyTrailOverrideUsesASeparateBoolean(bool forceTrail)
    {
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(-1); // Null record data.
            w.Write(1.25f);
            w.Write(2.5f);
            w.Write(0.5f); // Start offset.
            w.Write(0); // Record index count.
            w.Write(0); // NoDamage.
            w.Write(forceTrail ? 1 : 0);
            w.Write(0); // ForceLight.
            w.Write(0); // ForceHue.
            w.Write(0.25f);
            w.Write(0.5f);
            w.Write(0.75f);
        });
        var block = new CGameCtnMediaBlockEntity();
        var chunk = new CGameCtnMediaBlockEntity.Chunk0329F000();

        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.ForceTrail).IsEqualTo(forceTrail);
        await Assert.That(block.LightTrailColor).IsEqualTo(new Vec3(0.25f, 0.5f, 0.75f));
    }

    [Test]
    [Arguments(8, -1)]
    [Arguments(8, 12345)]
    [Arguments(11, -1)]
    [Arguments(11, 12345)]
    public async Task EntityRaceTimePreservesMillisecondsAndUnavailableSentinel(int version, int raceTime)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(-1); // Null record data.
            w.Write(0.5f); // Start offset.
            w.Write(0); // Record index count.
            for (var i = 0; i < 4; i++)
            {
                w.Write(0); // NoDamage, ForceTrail, ForceLight and ForceHue.
            }
            if (version >= 11)
            {
                w.Write(1); // Appearance archive version.
            }
            w.Write(3); // Lookback-string version.
            w.Write(uint.MaxValue); // Empty player model ID.
            w.Write(uint.MaxValue); // Empty collection.
            w.Write(uint.MaxValue); // Empty author.
            Floats(w, 3); // Appearance color.
            w.Write(0); // Skin count.
            w.Write(0); // No badge.
            if (version >= 11)
            {
                w.Write(0); // Empty skin options.
            }
            w.Write(0); // Key count.
            w.Write(0); // Empty ghost name.
            w.Write(raceTime);
            if (version >= 10)
            {
                w.Write(0);
                w.Write(0); // Discarded legacy words.
            }
        });
        var block = new CGameCtnMediaBlockEntity();
        var chunk = new CGameCtnMediaBlockEntity.Chunk0329F000();

        await Assert.That(block.RaceTime).IsNull();
        await RoundTrip(payload, rw => chunk.ReadWrite(block, rw));
        await Assert.That(block.RaceTime?.TotalMilliseconds).IsEqualTo(raceTime == -1 ? (int?)null : raceTime);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task EntityBadgesPreserveStickerNamesAndSlots(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            Floats(w, 3);
            if (version == 0)
            {
                w.Write(35); // Discarded legacy value.
                w.Write(0); // Discarded empty string.
            }
            w.Write(1); // Sticker count.
            w.Write(5);
            w.Write(Encoding.UTF8.GetBytes("Stars"));
            w.Write(4);
            w.Write(Encoding.UTF8.GetBytes("Rear"));
            w.Write(1); // Layer count.
            w.Write(5);
            w.Write(Encoding.UTF8.GetBytes("Metal"));
        });
        var badge = new CGameCtnMediaBlockEntity.SBadge();

        await Assert.That(badge.Color).IsEqualTo(new Vec3(1, 1, 1));
        await RoundTrip(payload, rw => badge.ReadWrite(rw));
        await Assert.That(badge.Stickers![0].Name).IsEqualTo("Stars");
        await Assert.That(badge.Stickers[0].Slot).IsEqualTo("Rear");
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static void Floats(BinaryWriter writer, int count)
    {
        for (var i = 0; i < count; i++)
        {
            writer.Write(i + 0.25f);
        }
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
