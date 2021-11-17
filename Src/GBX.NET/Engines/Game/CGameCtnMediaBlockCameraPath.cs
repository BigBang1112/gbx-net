namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera path
/// </summary>
[Node(0x030A1000)]
public sealed class CGameCtnMediaBlockCameraPath : CGameCtnMediaBlockCamera, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    private CGameCtnMediaBlockCameraPath()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x000 chunk
    /// </summary>
    [Chunk(0x030A1000)]
    public class Chunk030A1000 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        public override void Read(CGameCtnMediaBlockCameraPath n, GameBoxReader r)
        {
            n.keys = r.ReadList(r1 => new Key()
            {
                Time = r1.ReadSingle_s(),
                Position = r1.ReadVec3(),
                PitchYawRoll = r1.ReadVec3(), // in radians
                FOV = r1.ReadSingle(),
                AnchorRot = r1.ReadBoolean(),
                Anchor = r1.ReadInt32(),
                AnchorVis = r1.ReadBoolean(),
                Target = r1.ReadInt32(),
                TargetPosition = r1.ReadVec3(),
                U01 = r1.ReadSingle(), // 1
                U02 = r1.ReadSingle(), // -0.48
                U03 = r1.ReadSingle(), // 0
                U04 = r1.ReadSingle(), // 0
                U05 = r1.ReadSingle()
            });
        }

        public override void Write(CGameCtnMediaBlockCameraPath n, GameBoxWriter w)
        {
            w.Write(n.keys, (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.Position);
                w1.Write(x.PitchYawRoll);
                w1.Write(x.FOV);
                w1.Write(x.AnchorRot);
                w1.Write(x.Anchor);
                w1.Write(x.AnchorVis);
                w1.Write(x.Target);
                w1.Write(x.TargetPosition);
                w1.Write(x.U01);
                w1.Write(x.U02);
                w1.Write(x.U03);
                w1.Write(x.U04);
                w1.Write(x.U05);
            });
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x002 chunk
    /// </summary>
    [Chunk(0x030A1002)]
    public class Chunk030A1002 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        public override void Read(CGameCtnMediaBlockCameraPath n, GameBoxReader r)
        {
            n.keys = r.ReadList(r1 => new Key()
            {
                Time = r1.ReadSingle_s(),
                Position = r1.ReadVec3(),
                PitchYawRoll = r1.ReadVec3(), // in radians
                FOV = r1.ReadSingle(),
                AnchorRot = r1.ReadBoolean(),
                Anchor = r1.ReadInt32(),
                AnchorVis = r1.ReadBoolean(),
                Target = r1.ReadInt32(),
                TargetPosition = r1.ReadVec3(),
                U01 = r1.ReadSingle(), // 1
                U02 = r1.ReadSingle(), // -0.48
                U03 = r1.ReadSingle(), // 0
                U04 = r1.ReadSingle(), // 0
                U05 = r1.ReadSingle()
            });
        }

        public override void Write(CGameCtnMediaBlockCameraPath n, GameBoxWriter w)
        {
            w.Write(n.keys, (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.Position);
                w1.Write(x.PitchYawRoll);
                w1.Write(x.FOV);
                w1.Write(x.AnchorRot);
                w1.Write(x.Anchor);
                w1.Write(x.AnchorVis);
                w1.Write(x.Target);
                w1.Write(x.TargetPosition);
                w1.Write(x.U01);
                w1.Write(x.U02);
                w1.Write(x.U03);
                w1.Write(x.U04);
                w1.Write(x.U05);
            });
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x003 chunk
    /// </summary>
    [Chunk(0x030A1003)]
    public class Chunk030A1003 : Chunk<CGameCtnMediaBlockCameraPath>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnMediaBlockCameraPath n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.keys = r.ReadList(r1 =>
            {
                var time = r1.ReadSingle_s();
                var position = r1.ReadVec3();
                var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();

                var nearZ = default(float?);
                if (Version >= 3)
                    nearZ = r1.ReadSingle();

                var anchorRot = r1.ReadBoolean();
                var anchor = r1.ReadInt32();
                var anchorVis = r1.ReadBoolean();
                var target = r1.ReadInt32();
                var targetPosition = r1.ReadVec3();

                var u01 = r1.ReadSingle();
                var u02 = r1.ReadSingle();
                var u03 = r1.ReadSingle();
                var u04 = r1.ReadSingle();
                var u05 = r1.ReadSingle();

                var u06 = default(int?);
                var u07 = default(int?);

                if (Version >= 4)
                {
                    u06 = r1.ReadInt32();
                    u07 = r1.ReadInt32();
                }

                return new Key()
                {
                    Time = time,
                    Position = position,
                    PitchYawRoll = pitchYawRoll,
                    FOV = fov,
                    NearZ = nearZ,
                    AnchorRot = anchorRot,
                    Anchor = anchor,
                    AnchorVis = anchorVis,
                    Target = target,
                    TargetPosition = targetPosition,
                    U01 = u01,
                    U02 = u02,
                    U03 = u03,
                    U04 = u04,
                    U05 = u05,
                    U06 = u06,
                    U07 = u07,
                };
            });
        }

        public override void Write(CGameCtnMediaBlockCameraPath n, GameBoxWriter w)
        {
            w.Write(Version);

            w.Write(n.keys, (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.Position);
                w1.Write(x.PitchYawRoll);
                w1.Write(x.FOV);

                if (Version >= 3)
                    w1.Write(x.NearZ.GetValueOrDefault());

                w1.Write(x.AnchorRot);
                w1.Write(x.Anchor);
                w1.Write(x.AnchorVis);
                w1.Write(x.Target);
                w1.Write(x.TargetPosition);

                w1.Write(x.U01);
                w1.Write(x.U02);
                w1.Write(x.U03);
                w1.Write(x.U04);
                w1.Write(x.U05);

                if (Version >= 4)
                {
                    w1.Write(x.U06.GetValueOrDefault());
                    w1.Write(x.U07.GetValueOrDefault());
                }
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public int Anchor { get; set; }
        public bool AnchorVis { get; set; }
        public bool AnchorRot { get; set; }
        public int Target { get; set; }

        public Vec3 Position { get; set; }
        /// <summary>
        /// Pitch, yaw and roll in radians
        /// </summary>
        public Vec3 PitchYawRoll { get; set; }
        public float FOV { get; set; }
        public float? NearZ { get; set; }

        public Vec3 TargetPosition { get; set; }
        public Vec3 LeftTangent { get; set; }
        public Vec3 RightTangent { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public int? U06;
        public int? U07;
    }

    #endregion
}
