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
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();
                    var anchorRot = r.ReadBoolean();
                    var anchor = r.ReadInt32();
                    var anchorVis = r.ReadBoolean();
                    var target = r.ReadInt32();
                    var targetPosition = r.ReadVec3();
                    var a = r.ReadSingle(); // 1
                    var b = r.ReadSingle(); // -0.48
                    var c = r.ReadSingle(); // 0
                    var d = r.ReadSingle(); // 0
                    var e = r.ReadSingle();

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
                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write(x.AnchorRot);
                    w.Write(x.Anchor);
                    w.Write(x.AnchorVis);
                    w.Write(x.Target);
                    w.Write(x.TargetPosition);
                    w.Write((float)x.Unknown[0]);
                    w.Write((float)x.Unknown[1]);
                    w.Write((float)x.Unknown[2]);
                    w.Write((float)x.Unknown[3]);
                    w.Write((float)x.Unknown[4]);
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
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();
                    var anchorRot = r.ReadBoolean();
                    var anchor = r.ReadInt32();
                    var anchorVis = r.ReadBoolean();
                    var target = r.ReadInt32();
                    var targetPosition = r.ReadVec3();
                    var a = r.ReadSingle(); // 1
                    var b = r.ReadSingle(); // -0.48
                    var c = r.ReadSingle(); // 0
                    var d = r.ReadSingle(); // 0
                    var e = r.ReadSingle();

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
                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    w.Write(x.AnchorRot);
                    w.Write(x.Anchor);
                    w.Write(x.AnchorVis);
                    w.Write(x.Target);
                    w.Write(x.TargetPosition);
                    w.Write((float)x.Unknown[0]);
                    w.Write((float)x.Unknown[1]);
                    w.Write((float)x.Unknown[2]);
                    w.Write((float)x.Unknown[3]);
                    w.Write((float)x.Unknown[4]);
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

                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();

                    float? nearZ = null;
                    if (Version >= 3)
                        nearZ = r.ReadSingle();
                    var anchorRot = r.ReadBoolean();
                    var anchor = r.ReadInt32();
                    var anchorVis = r.ReadBoolean();
                    var target = r.ReadInt32();
                    var targetPosition = r.ReadVec3();

                    var unknown = r.ReadArray<float>(5).Cast<object>().ToList();

                    if(Version >= 4)
                        unknown.AddRange(r.ReadArray<int>(2).Cast<object>());

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

                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Position);
                    w.Write(x.PitchYawRoll);
                    w.Write(x.FOV);
                    if (Version >= 3)
                        w.Write(x.NearZ.GetValueOrDefault());
                    w.Write(x.AnchorRot);
                    w.Write(x.Anchor);
                    w.Write(x.AnchorVis);
                    w.Write(x.Target);
                    w.Write(x.TargetPosition);

                    w.Write((float)x.Unknown[0]);
                    w.Write((float)x.Unknown[1]);
                    w.Write((float)x.Unknown[2]);
                    w.Write((float)x.Unknown[3]);
                    w.Write((float)x.Unknown[4]);

                    if (Version >= 4)
                    {
                        w.Write((int)x.Unknown[5]);
                        w.Write((int)x.Unknown[6]);
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
