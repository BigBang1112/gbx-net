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
    /// MediaTracker block - Camera path
    /// </summary>
    [Node(0x030A1000)]
    public class CGameCtnMediaBlockCameraPath : CGameCtnMediaBlockCamera
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraPath 0x000 chunk
        /// </summary>
        [Chunk(0x030A1000)]
        public class Chunk030A1000 : Chunk<CGameCtnMediaBlockCameraPath>
        {
            public override void Read(CGameCtnMediaBlockCameraPath n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();
                    var anchorRot = r1.ReadBoolean();
                    var anchor = r1.ReadInt32();
                    var anchorVis = r1.ReadBoolean();
                    var target = r1.ReadInt32();
                    var targetPosition = r1.ReadVec3();
                    var a = r1.ReadSingle(); // 1
                    var b = r1.ReadSingle(); // -0.48
                    var c = r1.ReadSingle(); // 0
                    var d = r1.ReadSingle(); // 0
                    var e = r1.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,
                        AnchorRot = anchorRot,
                        Anchor = anchor,
                        AnchorVis = anchorVis,
                        Target = target,
                        TargetPosition = targetPosition,
                        Unknown = new object[] { a, b, c, d, e }
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockCameraPath n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.Position);
                    w1.Write(x.PitchYawRoll);
                    w1.Write(x.FOV);
                    w1.Write(x.AnchorRot);
                    w1.Write(x.Anchor);
                    w1.Write(x.AnchorVis);
                    w1.Write(x.Target);
                    w1.Write(x.TargetPosition);
                    w1.Write((float)x.Unknown[0]);
                    w1.Write((float)x.Unknown[1]);
                    w1.Write((float)x.Unknown[2]);
                    w1.Write((float)x.Unknown[3]);
                    w1.Write((float)x.Unknown[4]);
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
            public override void Read(CGameCtnMediaBlockCameraPath n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();
                    var anchorRot = r1.ReadBoolean();
                    var anchor = r1.ReadInt32();
                    var anchorVis = r1.ReadBoolean();
                    var target = r1.ReadInt32();
                    var targetPosition = r1.ReadVec3();
                    var a = r1.ReadSingle(); // 1
                    var b = r1.ReadSingle(); // -0.48
                    var c = r1.ReadSingle(); // 0
                    var d = r1.ReadSingle(); // 0
                    var e = r1.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,
                        AnchorRot = anchorRot,
                        Anchor = anchor,
                        AnchorVis = anchorVis,
                        Target = target,
                        TargetPosition = targetPosition,
                        Unknown = new object[] { a, b, c, d, e }
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockCameraPath n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.Position);
                    w1.Write(x.PitchYawRoll);
                    w1.Write(x.FOV);
                    w1.Write(x.AnchorRot);
                    w1.Write(x.Anchor);
                    w1.Write(x.AnchorVis);
                    w1.Write(x.Target);
                    w1.Write(x.TargetPosition);
                    w1.Write((float)x.Unknown[0]);
                    w1.Write((float)x.Unknown[1]);
                    w1.Write((float)x.Unknown[2]);
                    w1.Write((float)x.Unknown[3]);
                    w1.Write((float)x.Unknown[4]);
                });
            }
        }

        #endregion

        #region 0x003 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraPath 0x003 chunk
        /// </summary>
        [Chunk(0x030A1003)]
        public class Chunk030A1003 : Chunk<CGameCtnMediaBlockCameraPath>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockCameraPath n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var position = r1.ReadVec3();
                    var pitchYawRoll = r1.ReadVec3(); // in radians
                    var fov = r1.ReadSingle();

                    float? nearZ = null;
                    if (Version >= 3)
                        nearZ = r1.ReadSingle();
                    var anchorRot = r1.ReadBoolean();
                    var anchor = r1.ReadInt32();
                    var anchorVis = r1.ReadBoolean();
                    var target = r1.ReadInt32();
                    var targetPosition = r1.ReadVec3();

                    var unknown = r1.ReadArray<float>(5).Cast<object>().ToList();

                    if (Version >= 4)
                        unknown.AddRange(r1.ReadArray<int>(2).Cast<object>());

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
                        Unknown = unknown.Cast<object>().ToArray()
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockCameraPath n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
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

                    w1.Write((float)x.Unknown[0]);
                    w1.Write((float)x.Unknown[1]);
                    w1.Write((float)x.Unknown[2]);
                    w1.Write((float)x.Unknown[3]);
                    w1.Write((float)x.Unknown[4]);

                    if (Version >= 4)
                    {
                        w1.Write((int)x.Unknown[5]);
                        w1.Write((int)x.Unknown[6]);
                    }
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
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

            public object[] Unknown { get; set; }
        }

        #endregion
    }
}
