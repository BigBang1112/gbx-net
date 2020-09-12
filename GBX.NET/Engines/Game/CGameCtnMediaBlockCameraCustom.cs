using System;
using System.Collections.Generic;
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
        public List<Key> Keys { get; set; } = new List<Key>();

        public CGameCtnMediaBlockCameraCustom(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        #region Chunks

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraCustom 0x002 chunk
        /// </summary>
        [Chunk(0x030A2002)]
        public class Chunk030A2002 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public override void Read(CGameCtnMediaBlockCameraCustom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var a = r.ReadInt32(); // 1
                    var b = r.ReadInt32(); // 0
                    var c = r.ReadInt32(); // 0
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();
                    var d = r.ReadInt32(); // 0
                    var e = r.ReadInt32(); // -1
                    var f = r.ReadInt32(); // 1
                    var g = r.ReadInt32(); // -1
                    var targetPosition = r.ReadVec3();
                    var leftTangent = r.ReadVec3();
                    var rightTangent = r.ReadVec3();

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
                w.Write(n.Keys?.ToArray(), x =>
                {
                    w.Write(x.Time);
                    w.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w.Write((int)x.Unknown.ElementAtOrDefault(4));
                    w.Write((int)x.Unknown.ElementAtOrDefault(5));
                    w.Write((int)x.Unknown.ElementAtOrDefault(6));
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
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockCameraCustom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var a = r.ReadInt32(); // 1
                    var b = r.ReadInt32(); // 0
                    var c = r.ReadInt32(); // 0
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();
                    var d = r.ReadInt32(); // 0
                    var anchor = r.ReadInt32(); // -1 if not set, 0 for local player
                    var f = r.ReadInt32(); // 1
                    var target = r.ReadInt32(); // -1 if not set, 0 for local player
                    var targetPosition = r.ReadVec3();
                    var leftTangent = r.ReadVec3();
                    var rightTangent = r.ReadVec3();

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
                w.Write(n.Keys.ToArray(), x =>
                {
                    w.Write(x.Time);
                    w.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w.Write(x.Anchor);
                    w.Write((int)x.Unknown.ElementAtOrDefault(4));
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
        public class Chunk030A2006 : Chunk<CGameCtnMediaBlockCameraCustom>
        {
            public int Version { get; set; } = 3;

            /// <summary>
            /// Constructs a new 0x030A2006 chunk with version 3.
            /// </summary>
            /// <param name="node"></param>
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
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var a = r.ReadInt32(); // 1
                    var anchorRot = r.ReadBoolean(); // 0
                    var anchor = r.ReadInt32(); // -1 if not set, 0 for local player
                    var anchorVis = r.ReadBoolean(); // 1
                    var target = r.ReadInt32(); // -1
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();
                    var f = r.ReadInt32(); // 0
                    var g = r.ReadInt32(); // 0
                    var h = r.ReadInt32(); // 0
                    var zIndex = r.ReadSingle();
                    var leftTangent = r.ReadVec3();
                    var ii = r.ReadArray<float>(8);
                    var rightTangent = r.ReadVec3();
                    var j = r.ReadArray<float>(8);

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

                w.Write(n.Keys?.ToArray(), x =>
                {
                    w.Write(x.Time);
                    w.Write((int)x.Unknown.ElementAtOrDefault(0));
                    w.Write(x.AnchorRot);
                    w.Write(x.Anchor);
                    w.Write(x.AnchorVis);
                    w.Write(x.Target);
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write((int)x.Unknown.ElementAtOrDefault(1));
                    w.Write((int)x.Unknown.ElementAtOrDefault(2));
                    w.Write((int)x.Unknown.ElementAtOrDefault(3));
                    w.Write(x.ZIndex.GetValueOrDefault());
                    w.Write(x.LeftTangent);
                    w.Write((float[])x.Unknown.ElementAtOrDefault(4));
                    w.Write(x.RightTangent);
                    w.Write((float[])x.Unknown.ElementAtOrDefault(5));
                });
            }
        }

        #endregion

        #endregion

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
    }
}
