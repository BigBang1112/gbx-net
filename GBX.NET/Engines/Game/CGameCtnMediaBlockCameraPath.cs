using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A1000)]
    public class CGameCtnMediaBlockCameraPath : CGameCtnMediaBlockCamera
    {
        public Key[] Keys
        {
            get => GetValue<Chunk002>(x => x.Keys) as Key[];
            set => SetValue<Chunk002>(x => x.Keys = value);
        }

        public CGameCtnMediaBlockCameraPath(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030A1000)]
        public class Chunk000_0A1 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk000_0A1(CGameCtnMediaBlockCameraPath node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Keys = r.ReadArray(i =>
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

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Keys, x =>
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

        [Chunk(0x030A1002)]
        public class Chunk002 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk002(CGameCtnMediaBlockCameraPath node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Keys = r.ReadArray(i =>
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

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Keys, x =>
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

        [Chunk(0x030A1000, 0x003)]
        public class Chunk003 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk003(CGameCtnMediaBlockCameraPath node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32(); // 5

                Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var position = r.ReadVec3();
                    var pitchYawRoll = r.ReadVec3(); // in radians
                    var fov = r.ReadSingle();
                    var zIndex = r.ReadSingle();

                    var a = r.ReadArray<int>(7);
                    var b = r.ReadArray<float>(5);
                    var c = r.ReadArray<int>(2);

                    return new Key()
                    {
                        Time = time,
                        Position = position,
                        PitchYawRoll = pitchYawRoll,
                        FOV = fov,
                        ZIndex = zIndex
                    };
                });
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public int Anchor { get; set; }
            public bool AnchorVis { get; set; }
            public bool AnchorRot { get; set; }
            public int Target { get; set; }

            public Vector3 Position { get; set; }
            /// <summary>
            /// Pitch, yaw and roll in radians
            /// </summary>
            public Vector3 PitchYawRoll { get; set; }
            public float FOV { get; set; }
            public float? ZIndex { get; set; }

            public Vector3 TargetPosition { get; set; }
            public Vector3 LeftTangent { get; set; }
            public Vector3 RightTangent { get; set; }

            public object[] Unknown { get; set; }
        }
    }
}
