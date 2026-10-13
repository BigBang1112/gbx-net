using GBX.NET.Engines.Function;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleVisModel
{
    private CPlugVehicleVisGeomModel geomModel = new();

    public partial class SoundSet
    {

        [Obsolete("The body surface sound is unused by the game.")]
        public CPlugLocatedSound? BodySurface_Deprecated
        {
            get => bodySurface_DeprecatedRef?.Resolve();
            set => NodeRef.SetEmbedded(ref bodySurface_DeprecatedRef, value);
        }
    }

    public partial class Chunk090E7000
    {
        public override void ReadWrite(CPlugVehicleVisModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef(ref n.sharedRef);
            rw.NodeRef(ref U01Ref);
            rw.NodeRef(ref U02Ref);
            rw.NodeRef(ref U03Ref);
            if (Version >= 5)
            {
                rw.Int32(ref n.surfVersion);
            }
            rw.ReadableWritable<SoundSet>(ref n.sounds, Version);
            if (Version >= 1)
            {
                rw.Single(ref n.defaultDamperMinLen);
                rw.Single(ref n.defaultWheelOffsetY);
            }
            if (Version >= 9)
            {
                rw.Single(ref n.defaultSparkleRelSpeedThreshold);
                rw.Single(ref n.defaultSparkleScale);
            }
            if (Version >= 3)
            {
                n.geomModel ??= new();
                n.geomModel.SurfVersion = Version < 5 ? 0 : n.surfVersion;
                n.geomModel.ReadWrite(rw, Version);
            }
            if (Version is >= 12 and <= 17)
            {
                rw.NodeRef(ref U04Ref);
            }
            if (Version >= 17)
            {
                rw.NodeRef(ref n.fxSystemRef);
            }
        }
    }

    public partial class Chunk090E7002
    {
        private float? legacyWheelHardThreshold;
        private float? legacyWheelSoftThreshold;
        private float? legacyBodyHardThreshold;
        private float? legacyBodySoftThreshold;

        public override void ReadWrite(CPlugVehicleVisModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            Threshold(rw, ref n.soundWheelImpact_Hard_ThresholdKmh, ref legacyWheelHardThreshold);
            Threshold(rw, ref n.soundWheelImpact_Soft_ThresholdKmh, ref legacyWheelSoftThreshold);
            Threshold(rw, ref n.soundBodyImpact_Hard_ThresholdKmh, ref legacyBodyHardThreshold);
            Threshold(rw, ref n.soundBodyImpact_Soft_ThresholdKmh, ref legacyBodySoftThreshold);

            if (Version is >= 1 and <= 3)
            {
                rw.NodeRef(ref U01Ref);
            }
            if (Version is >= 2 and <= 3)
            {
                rw.NodeRef(ref U02Ref);
            }
            if (Version >= 3)
            {
                rw.NodeRef(ref n.audioEnvironmentForFirstPersonRef);
            }
            if (Version >= 4)
            {
                rw.Vec3(ref n.soundImpactWheelFrontPos);
                rw.Vec3(ref n.soundImpactWheelBackPos);
            }
        }

        private void Threshold(GbxReaderWriter rw, ref float value, ref float? legacyValue)
        {
            if (Version >= 5)
            {
                rw.Single(ref value);
                return;
            }

            if (rw.Reader is not null)
            {
                legacyValue = rw.Reader.ReadSingle();
                value = legacyValue.Value * 3.6f;
            }
            if (rw.Writer is not null)
            {
                // Retain the original float when conversion to km/h lost precision.
                rw.Writer.Write(legacyValue.HasValue && value == legacyValue.Value * 3.6f
                    ? legacyValue.Value : value / 3.6f);
            }
        }
    }
}
