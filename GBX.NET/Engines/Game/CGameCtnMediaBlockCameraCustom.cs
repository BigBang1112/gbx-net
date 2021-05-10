using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block (0x030A2000)
    /// </summary>
    [Node(0x030A2000)]
    public class CGameCtnMediaBlockCameraCustom : CGameCtnMediaBlockCamera
    {
        #region Properties

        [NodeMember]
        public List<Key> Keys { get; set; } = new List<Key>();

        #endregion

        #region Chunks

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x001 chunk
        /// </summary>
        [Chunk(0x030A2001)]
        public class Chunk030A2001 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public override void Read(CGameCtnMediaBlockCameraCustom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var u01 = r1.ReadInt32(); // 1
                    var u02 = r1.ReadInt32(); // 0
                    var u03 = r1.ReadInt32(); // 0
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();
                    var u04 = r1.ReadInt32(); // 0
                    var u05 = r1.ReadInt32(); // -1
                    var u06 = r1.ReadInt32(); // 1
                    var u07 = r1.ReadInt32(); // -1
                    var u08 = r1.ReadSingle();
                    var u09 = r1.ReadSingle();
                    var u10 = r1.ReadSingle();
                    var u11 = r1.ReadSingle();
                    var u12 = r1.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,

                        Unknown = new object[]
                        {
                            u01, u02, u03, u04, u05, u06, u07, u08, u09, u10, u11, u12
                        }
                    };
                }).ToList();
            }

            public override void Write(CGameCtnMediaBlockCameraCustom n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys?.ToArray(), (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w1.Write(x.Position);
                    w1.Write(x.PitchYawRoll);
                    w1.Write(x.FOV);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(4));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(5));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(6));
                    w1.Write(x.TargetPosition.GetValueOrDefault());
                    w1.Write(x.LeftTangent);
                    w1.Write(x.RightTangent);
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
            public override void Read(CGameCtnMediaBlockCameraCustom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var a = r1.ReadInt32(); // 1
                    var b = r1.ReadInt32(); // 0
                    var c = r1.ReadInt32(); // 0
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();
                    var d = r1.ReadInt32(); // 0
                    var e = r1.ReadInt32(); // -1
                    var f = r1.ReadInt32(); // 1
                    var g = r1.ReadInt32(); // -1
                    var targetPosition = r1.ReadVec3();
                    var leftTangent = r1.ReadVec3();
                    var rightTangent = r1.ReadVec3();

                    return new Key()
                    {
                        Time = time,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,
                        TargetPosition = targetPosition,
                        LeftTangent = leftTangent,
                        RightTangent = rightTangent,

                        Unknown = new object[]
                        {
                            a, b, c, d, e, f, g
                        }
                    };
                }).ToList();
            }

            public override void Write(CGameCtnMediaBlockCameraCustom n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys?.ToArray(), (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w1.Write(x.Position);
                    w1.Write(x.PitchYawRoll);
                    w1.Write(x.FOV);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(4));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(5));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(6));
                    w1.Write(x.TargetPosition.GetValueOrDefault());
                    w1.Write(x.LeftTangent);
                    w1.Write(x.RightTangent);
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
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockCameraCustom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var a = r1.ReadInt32(); // 1
                    var b = r1.ReadInt32(); // 0
                    var c = r1.ReadInt32(); // 0
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();
                    var d = r1.ReadInt32(); // 0
                    var anchor = r1.ReadInt32(); // -1 if not set, 0 for local player
                    var f = r1.ReadInt32(); // 1
                    var target = r1.ReadInt32(); // -1 if not set, 0 for local player
                    var targetPosition = r1.ReadVec3();
                    var leftTangent = r1.ReadVec3();
                    var rightTangent = r1.ReadVec3();

                    return new Key()
                    {
                        Time = time,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,
                        Anchor = anchor,
                        Target = target,
                        TargetPosition = targetPosition,
                        LeftTangent = leftTangent,
                        RightTangent = rightTangent,

                        Unknown = new object[]
                        {
                            a, b, c, d, f
                        }
                    };
                }).ToList();
            }

            public override void Write(CGameCtnMediaBlockCameraCustom n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys.ToArray(), (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w1.Write(x.Position);
                    w1.Write(x.PitchYawRoll);
                    w1.Write(x.FOV);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w1.Write(x.Anchor);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(4));
                    w1.Write(x.Target);
                    w1.Write(x.TargetPosition.GetValueOrDefault());
                    w1.Write(x.LeftTangent);
                    w1.Write(x.RightTangent);
                });
            }
        }

        #endregion

        #region 0x006 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x006 chunk (ManiaPlanet)
        /// </summary>
        [Chunk(0x030A2006, "ManiaPlanet")]
        public class Chunk030A2006 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public int Version { get; set; } = 3;

            /// <summary>
            /// Constructs a new 0x030A2006 chunk with version 3.
            /// </summary>
            public Chunk030A2006() : this(3)
            {
                
            }

            public Chunk030A2006(int version)
            {
                Version = version;
            }

            public override void Read(CGameCtnMediaBlockCameraCustom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var a = r1.ReadInt32(); // 1
                    var anchorRot = r1.ReadBoolean(); // 0
                    var anchor = r1.ReadInt32(); // -1 if not set, 0 for local player
                    var anchorVis = r1.ReadBoolean(); // 1
                    var target = r1.ReadInt32(); // -1
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();
                    var f = r1.ReadInt32(); // 0
                    var g = r1.ReadInt32(); // 0
                    var h = r1.ReadInt32(); // 0
                    var zIndex = r1.ReadSingle();
                    var leftTangent = r1.ReadVec3();
                    var ii = r1.ReadArray<float>(8);
                    var rightTangent = r1.ReadVec3();
                    var j = r1.ReadArray<float>(8);

                    return new Key()
                    {
                        Time = time,
                        AnchorRot = anchorRot,
                        Anchor = anchor,
                        AnchorVis = anchorVis,
                        Target = target,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,
                        ZIndex = zIndex,
                        LeftTangent = leftTangent,
                        RightTangent = rightTangent,

                        Unknown = new object[]
                        {
                            a, f, g, h, ii, j
                        }
                    };
                }).ToList();
            }

            public override void Write(CGameCtnMediaBlockCameraCustom n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Keys?.ToArray(), (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w1.Write(x.AnchorRot);
                    w1.Write(x.Anchor);
                    w1.Write(x.AnchorVis);
                    w1.Write(x.Target);
                    w1.Write(x.Position);
                    w1.Write(x.PitchYawRoll);
                    w1.Write(x.FOV);
                    w1.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w1.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w1.Write(x.ZIndex.GetValueOrDefault());
                    w1.Write(x.LeftTangent);
                    w1.Write((float[])x.Unknown.ElementAtOrDefault(4));
                    w1.Write(x.RightTangent);
                    w1.Write((float[])x.Unknown.ElementAtOrDefault(5));
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
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

            public object[] Unknown { get; set; }
        }

        #endregion
    }
}
