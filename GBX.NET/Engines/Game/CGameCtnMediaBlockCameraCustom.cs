using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block (0x030A2000)
    /// </summary>
    [Node(0x030A2000)]
    public class CGameCtnMediaBlockCameraCustom : CGameCtnMediaBlockCamera, CGameCtnMediaBlock.IHasKeys
    {
        #region Fields

        private IList<Key> keys = new List<Key>();

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

        #region Chunks

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x001 chunk
        /// </summary>
        [Chunk(0x030A2001)]
        public class Chunk030A2001 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
            {
                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    U00X01 = r.ReadInt32(), // 1
                    U00X02 = r.ReadInt32(), // 0
                    U00X03 = r.ReadInt32(), // 0
                    Position = r.ReadVec3(),
                    PitchYawRoll = r.ReadVec3(), // in radians
                    FOV = r.ReadSingle(),
                    U00X04 = r.ReadInt32(), // 0
                    U00X05 = r.ReadInt32(), // -1
                    U00X06 = r.ReadInt32(), // 1
                    U00X07 = r.ReadInt32(), // -1
                    U00108 = r.ReadSingle(),
                    U00109 = r.ReadSingle(),
                    U00110 = r.ReadSingle(),
                    U00111 = r.ReadSingle(),
                    U00112 = r.ReadSingle(),
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.U00X01.GetValueOrDefault(1));
                    w.Write(x.U00X02.GetValueOrDefault());
                    w.Write(x.U00X03.GetValueOrDefault());
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write(x.U00X04.GetValueOrDefault());
                    w.Write(x.U00X05.GetValueOrDefault());
                    w.Write(x.U00X06.GetValueOrDefault());
                    w.Write(x.U00X07.GetValueOrDefault());
                    w.Write(x.U00108.GetValueOrDefault());
                    w.Write(x.U00109.GetValueOrDefault());
                    w.Write(x.U00110.GetValueOrDefault());
                    w.Write(x.U00111.GetValueOrDefault());
                    w.Write(x.U00112.GetValueOrDefault());
                });
            }
        }

        #endregion

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x002 chunk
        /// </summary>
        [Chunk(0x030A2002)]
        public class Chunk030A2002 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
            {
                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    U00X01 = r.ReadInt32(), // 1
                    U00X02 = r.ReadInt32(), // 0
                    U00X03 = r.ReadInt32(), // 0
                    Position = r.ReadVec3(),
                    PitchYawRoll = r.ReadVec3(), // in radians
                    FOV = r.ReadSingle(),
                    U00X04 = r.ReadInt32(), // 0
                    U00X05 = r.ReadInt32(), // -1
                    U00X06 = r.ReadInt32(), // 1
                    U00X07 = r.ReadInt32(), // -1
                    TargetPosition = r.ReadVec3(),
                    LeftTangent = r.ReadVec3(),
                    RightTangent = r.ReadVec3()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.U00X01.GetValueOrDefault(1));
                    w.Write(x.U00X02.GetValueOrDefault());
                    w.Write(x.U00X03.GetValueOrDefault());
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write(x.U00X04.GetValueOrDefault());
                    w.Write(x.U00X05.GetValueOrDefault());
                    w.Write(x.U00X06.GetValueOrDefault());
                    w.Write(x.U00X07.GetValueOrDefault());
                    w.Write(x.TargetPosition.GetValueOrDefault());
                    w.Write(x.LeftTangent);
                    w.Write(x.RightTangent);
                });
            }
        }

        #endregion

        #region 0x005 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x005 chunk (TMUF)
        /// </summary>
        [Chunk(0x030A2005, "TMUF")]
        public class Chunk030A2005 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
            {
                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    U00X01 = r.ReadInt32(), // 1
                    U00X02 = r.ReadInt32(), // 0
                    U00X03 = r.ReadInt32(), // 0
                    Position = r.ReadVec3(),
                    PitchYawRoll = r.ReadVec3(), // in radians
                    FOV = r.ReadSingle(),
                    U00X04 = r.ReadInt32(), // 0
                    Anchor = r.ReadInt32(), // -1 if not set, 0 for local player
                    U00X05 = r.ReadInt32(), // 1
                    Target = r.ReadInt32(), // -1 if not set, 0 for local player
                    TargetPosition = r.ReadVec3(),
                    LeftTangent = r.ReadVec3(),
                    RightTangent = r.ReadVec3()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.U00X01.GetValueOrDefault(1));
                    w.Write(x.U00X02.GetValueOrDefault());
                    w.Write(x.U00X03.GetValueOrDefault());
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write(x.U00X04.GetValueOrDefault());
                    w.Write(x.Anchor);
                    w.Write(x.U00X05.GetValueOrDefault());
                    w.Write(x.Target);
                    w.Write(x.TargetPosition.GetValueOrDefault());
                    w.Write(x.LeftTangent);
                    w.Write(x.RightTangent);
                });
            }
        }

        #endregion

        #region 0x006 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x006 chunk (ManiaPlanet)
        /// </summary>
        [Chunk(0x030A2006, "ManiaPlanet")]
        public class Chunk030A2006 : Chunk<CGameCtnMediaBlockCameraCustom>, IVersionable
        {
            private int version = 3;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    U00X01 = r.ReadInt32(), // 1
                    AnchorRot = r.ReadBoolean(),
                    Anchor = r.ReadInt32(), // -1 if not set, 0 for local player
                    AnchorVis = r.ReadBoolean(),
                    Target = r.ReadInt32(),
                    Position = r.ReadVec3(),
                    PitchYawRoll = r.ReadVec3(), // in radians
                    FOV = r.ReadSingle(),
                    U00X02 = r.ReadInt32(), // 0
                    U00X03 = r.ReadInt32(), // 0
                    U00X04 = r.ReadInt32(), // 0
                    ZIndex = r.ReadSingle(),
                    LeftTangent = r.ReadVec3(),
                    U00605 = r.ReadSingle(),
                    U00606 = r.ReadSingle(),
                    U00607 = r.ReadSingle(),
                    U00608 = r.ReadSingle(),
                    U00609 = r.ReadSingle(),
                    U00610 = r.ReadSingle(),
                    U00611 = r.ReadSingle(),
                    U00612 = r.ReadSingle(),
                    RightTangent = r.ReadVec3(),
                    U00613 = r.ReadSingle(),
                    U00614 = r.ReadSingle(),
                    U00615 = r.ReadSingle(),
                    U00616 = r.ReadSingle(),
                    U00617 = r.ReadSingle(),
                    U00618 = r.ReadSingle(),
                    U00619 = r.ReadSingle(),
                    U00620 = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.U00X01.GetValueOrDefault());
                    w.Write(x.AnchorRot);
                    w.Write(x.Anchor);
                    w.Write(x.AnchorVis);
                    w.Write(x.Target);
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write(x.U00X02.GetValueOrDefault());
                    w.Write(x.U00X03.GetValueOrDefault());
                    w.Write(x.U00X04.GetValueOrDefault());
                    w.Write(x.ZIndex.GetValueOrDefault());
                    w.Write(x.LeftTangent);
                    w.Write(x.U00605.GetValueOrDefault());
                    w.Write(x.U00606.GetValueOrDefault());
                    w.Write(x.U00607.GetValueOrDefault());
                    w.Write(x.U00608.GetValueOrDefault());
                    w.Write(x.U00609.GetValueOrDefault());
                    w.Write(x.U00610.GetValueOrDefault());
                    w.Write(x.U00611.GetValueOrDefault());
                    w.Write(x.U00612.GetValueOrDefault());
                    w.Write(x.RightTangent);
                    w.Write(x.U00613.GetValueOrDefault());
                    w.Write(x.U00614.GetValueOrDefault());
                    w.Write(x.U00615.GetValueOrDefault());
                    w.Write(x.U00616.GetValueOrDefault());
                    w.Write(x.U00617.GetValueOrDefault());
                    w.Write(x.U00618.GetValueOrDefault());
                    w.Write(x.U00619.GetValueOrDefault());
                    w.Write(x.U00620.GetValueOrDefault());
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public Vec3 Position { get; set; }
            /// <summary>
            /// Pitch, yaw and roll in radians.
            /// </summary>
            public Vec3 PitchYawRoll { get; set; }
            public float FOV { get; set; }
            public float? ZIndex { get; set; }
            public Vec3? TargetPosition { get; set; }
            public Vec3 LeftTangent { get; set; }
            public Vec3 RightTangent { get; set; }
            public int Anchor { get; set; }
            public int Target { get; set; }
            public bool AnchorVis { get; set; }
            public bool AnchorRot { get; set; }

            public int? U00X01;
            public int? U00X02;
            public int? U00X03;
            public int? U00X04;
            public int? U00X05;
            public int? U00X06;
            public int? U00X07;
            public float? U00108;
            public float? U00109;
            public float? U00110;
            public float? U00111;
            public float? U00112;

            public float? U00605;
            public float? U00606;
            public float? U00607;
            public float? U00608;
            public float? U00609;
            public float? U00610;
            public float? U00611;
            public float? U00612;
            public float? U00613;
            public float? U00614;
            public float? U00615;
            public float? U00616;
            public float? U00617;
            public float? U00618;
            public float? U00619;
            public float? U00620;
        }

        #endregion
    }
}
