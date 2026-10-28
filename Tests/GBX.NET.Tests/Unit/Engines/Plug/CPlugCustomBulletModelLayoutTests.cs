using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCustomBulletModelLayoutTests
{
    [Test]
    public async Task ReadWrite_Version28_KeepsNativeOrder()
    {
        var bullet = new CPlugCustomBulletModel();

        await RoundTrip(BulletPayload(28), rw => bullet.ReadWrite(rw, 28));

        await Assert.That(bullet.BulletName).IsEqualTo("Rocket");
        await Assert.That(bullet.BulletExplosionDetectionRadius).IsEqualTo(1.1f);
        await Assert.That(bullet.BulletExplosionBlowRadius).IsEqualTo(1.2f);
        await Assert.That(bullet.BulletExplosionBlowValue).IsEqualTo(1.3f);
        await Assert.That(bullet.BulletExplosionDamageRadius).IsEqualTo(1.4f);
        await Assert.That(bullet.BulletExplosionBlowRadiusAttenuation).IsEqualTo(2.1f);
        await Assert.That(bullet.BulletExplosionDamageValue).IsEqualTo(200);
        await Assert.That(bullet.BulletExplosionDamageRadiusAttenuation).IsEqualTo(2.3f);
        await Assert.That(bullet.BulletExplosionBlowVerticalScale).IsEqualTo(0.5f);
        await Assert.That(bullet.BulletExplosionOnAllPlayers).IsTrue();
        await Assert.That(bullet.BulletType).IsEqualTo(4);
        await Assert.That(bullet.BulletDirectHitDamageValue).IsEqualTo(300);
        await Assert.That(bullet.BulletExplosionBlowOnAllPlayers).IsFalse();
        await Assert.That(bullet.U18).IsEqualTo(0);
        await Assert.That(bullet.U19).IsEqualTo((byte)7);
    }

    [Test]
    public async Task ReadWrite_Version18_ReadsLegacyTypeFlags()
    {
        var bullet = new CPlugCustomBulletModel();

        await RoundTrip(BulletPayload(18), rw => bullet.ReadWrite(rw, 18));

        await Assert.That(bullet.U11).IsFalse();
        await Assert.That(bullet.U14).IsFalse();
        await Assert.That(bullet.U15).IsTrue();
        await Assert.That(bullet.BulletGuidedMinLifeTime).IsEqualTo(250);
        await Assert.That(bullet.BulletGunSpeedCoef).IsEqualTo(0f);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var bullet = new CPlugCustomBulletModel();

        await Assert.That(bullet.BulletSpeed).IsEqualTo(10f);
        await Assert.That(bullet.BulletHitboxRadius).IsEqualTo(0.1f);
        await Assert.That(bullet.BulletExplodeOnEndLife).IsTrue();
        await Assert.That(bullet.BulletBounceOnTechWall).IsTrue();
        await Assert.That(bullet.PatternBulletCount).IsEqualTo(3);
        await Assert.That(bullet.PatternBulletBlendDuration).IsEqualTo(0.1f);
        await Assert.That(bullet.NoPatternBulletCount).IsEqualTo(1);
        await Assert.That(bullet.NoPatternBulletSpeedCoef).IsEqualTo(1f);
        await Assert.That(bullet.BulletHomingAngularSpeed).IsEqualTo(90f);
        await Assert.That(bullet.BulletHomingPeriod).IsEqualTo(0.1f);
        await Assert.That(bullet.BulletShowPlayerExplosion).IsTrue();
        await Assert.That(bullet.BulletExplosionBlowVerticalScale).IsEqualTo(1f);
        await Assert.That(bullet.BulletAliveSoundVolume).IsEqualTo(-1f);
        await Assert.That(bullet.BulletHomingSoundVolume).IsEqualTo(-1f);
        await Assert.That(bullet.BulletShowDebris).IsTrue();
        await Assert.That(bullet.BulletModifyFOV).IsTrue();
        await Assert.That(bullet.BulletFlareAttractionRadius).IsEqualTo(50f);
        await Assert.That(bullet.BulletFlareExplosionRadius).IsEqualTo(2f);
        await Assert.That(bullet.BulletGuidedAngularSpeed).IsEqualTo(90f);
        await Assert.That(bullet.BulletHomingLockDuration).IsEqualTo(500);
        await Assert.That(bullet.BulletWardRadius).IsEqualTo(50f);
        await Assert.That(bullet.BulletGuidedMinLifeTime).IsEqualTo(500);
        await Assert.That(bullet.BulletHomingDamageMinAngle_Deg).IsEqualTo(360);
        await Assert.That(bullet.BulletHomingDamageMaxAngle_Deg).IsEqualTo(360);
        await Assert.That(bullet.BulletGunSpeedCoefRatioMin).IsEqualTo(0.2f);
        await Assert.That(bullet.BulletGunSpeedCoefRatioMax).IsEqualTo(1f);
        await Assert.That(bullet.BulletDirectHitDamageValue).IsEqualTo(-1);
        await Assert.That(bullet.BulletExplosionBlowOnAllPlayers).IsTrue();
    }

    private static byte[] BulletPayload(int version) => Payload(w =>
    {
        String(w, "Rocket");
        foreach (var value in new[] { 50f, 2f, 0.2f, 0.01f, 4f }) w.Write(value); // Speed, mass, hitbox, friction, life time.
        w.Write(1); // BulletExplodeOnEndLife
        w.Write(2); // BulletRebounds
        w.Write(0); // BulletBounceOnTechWall
        w.Write(1); // BulletPattern
        foreach (var value in new[] { 1.1f, 1.2f, 1.3f, 1.4f }) w.Write(value);
        w.Write(5); // PatternBulletCount
        foreach (var value in new[] { 0.5f, 0.6f, 0.7f }) w.Write(value);
        w.Write(1); // PatternBulletApexRegroup
        w.Write(0.8f);
        w.Write(0); // PatternBulletRandomRotations
        w.Write(2); // NoPatternBulletCount
        w.Write(3f);
        w.Write(1.5f);
        w.Write(3); // Id table version.
        NewId(w, "Alive");
        NewId(w, "Explosion");
        NewId(w, "Shoot");
        w.Write(10); // Deprecated alive emitter list.
        w.Write(0);
        w.Write(10); // Deprecated explosion emitter list.
        w.Write(0);
        if (version < 1) return;
        w.Write(2.1f);
        w.Write(200);
        w.Write(2.3f);
        if (version < 3)
        {
            w.Write(0);
            w.Write(0);
            w.Write(0);
        }
        w.Write(1f); // BulletLifeTimeAfterFirstImpact
        if (version < 2) return;
        w.Write(0.4f);
        w.Write(0.6f);
        if (version < 4) return;
        if (version < 19) w.Write(0); // Legacy homing flag.
        foreach (var value in new[] { 20f, 45f, 0.2f }) w.Write(value);
        if (version < 5) return;
        w.Write(0); // BulletShowPlayerExplosion
        if (version < 6) return;
        w.Write(0.5f);
        if (version < 7) return;
        NewId(w, "Rebound");
        if (version < 8) return;
        foreach (var value in new[] { 0.1f, 0.2f, 0.3f, 0.4f }) w.Write(value);
        if (version < 9) return;
        w.Write(0); // BulletShowDebris
        if (version < 10) return;
        w.Write(0); // BulletModifyFOV
        if (version < 11) return;
        w.Write(1); // BulletExplosionOnAllPlayers
        if (version < 12) return;
        w.Write(1); // BulletIsFlare
        if (version < 13) return;
        w.Write(30f);
        w.Write(3f);
        if (version < 14) return;
        NewId(w, "Homing");
        w.Write(0.9f);
        if (version < 15) return;
        if (version < 19) w.Write(0); // Legacy guided flag.
        w.Write(60f);
        if (version < 16) return;
        w.Write(400);
        if (version < 17) return;
        w.Write(1); // BulletIsWard
        w.Write(40f);
        if (version < 18) return;
        if (version == 18) w.Write(1); // Legacy type flag.
        w.Write(250);
        if (version < 20) return;
        w.Write(1.2f);
        if (version < 21) return;
        w.Write(4);
        if (version < 22) return;
        w.Write(90);
        w.Write(180);
        if (version < 23) return;
        w.Write(0.3f);
        if (version < 24) return;
        w.Write(0.5f);
        w.Write(0.9f);
        if (version < 25) return;
        w.Write(300);
        if (version < 26) return;
        w.Write(0); // BulletExplosionBlowOnAllPlayers
        if (version < 27) return;
        w.Write(0);
        if (version < 28) return;
        w.Write((byte)7);
    });
}
