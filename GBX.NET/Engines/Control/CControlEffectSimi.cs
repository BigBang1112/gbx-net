using System.Collections.Generic;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Control
{
    [Node(0x07010000)]
    public class CControlEffectSimi : CMwNod
    {
        #region Properties

        [NodeMember]
        public List<Key> Keys { get; set; }

        [NodeMember]
        public bool Centered { get; set; }

        [NodeMember]
        public int ColorBlendMode { get; set; }

        [NodeMember]
        public bool IsContinousEffect { get; set; }

        [NodeMember]
        public bool IsInterpolated { get; set; }

        #endregion

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x07010002)]
        public class Chunk07010002 : Chunk<CControlEffectSimi>
        {
            public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.List(n.Keys, (i, r) =>
                {
                    var time = r.ReadSingle();
                    var x = r.ReadSingle();
                    var y = r.ReadSingle();
                    var rot = r.ReadSingle();
                    var scaleX = r.ReadSingle();
                    var scaleY = r.ReadSingle();
                    var opacity = r.ReadSingle();
                    var depth = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        X = x,
                        Y = y,
                        Rotation = rot,
                        ScaleX = scaleX,
                        ScaleY = scaleY,
                        Opacity = opacity,
                        Depth = depth
                    };
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.X);
                    w.Write(x.Y);
                    w.Write(x.Rotation);
                    w.Write(x.ScaleX);
                    w.Write(x.ScaleY);
                    w.Write(x.Opacity);
                    w.Write(x.Depth);
                });

                n.Centered = rw.Boolean(n.Centered);
            }
        }

        #endregion

        #region 0x004 chunk

        /// <summary>
        /// CControlEffectSimi 0x004 chunk
        /// </summary>
        [Chunk(0x07010004)]
        public class Chunk07010004 : Chunk<CControlEffectSimi>
        {
            public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.List(n.Keys, (i, r) =>
                {
                    var time = r.ReadSingle();
                    var x = r.ReadSingle();
                    var y = r.ReadSingle();
                    var rot = r.ReadSingle();
                    var scaleX = r.ReadSingle();
                    var scaleY = r.ReadSingle();
                    var opacity = r.ReadSingle();
                    var depth = r.ReadSingle();
                    var u01 = r.ReadSingle();
                    var isContinousEffect = r.ReadSingle();
                    var u02 = r.ReadSingle();
                    var u03 = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        X = x,
                        Y = y,
                        Rotation = rot,
                        ScaleX = scaleX,
                        ScaleY = scaleY,
                        Opacity = opacity,
                        Depth = depth,
                        IsContinuousEffect = isContinousEffect,
                        Unknown = new float[] { u01, u02, u03 }
                    };
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.X);
                    w.Write(x.Y);
                    w.Write(x.Rotation);
                    w.Write(x.ScaleX);
                    w.Write(x.ScaleY);
                    w.Write(x.Opacity);
                    w.Write(x.Depth);
                    w.Write(x.Unknown[0]);
                    w.Write(x.IsContinuousEffect);
                    w.Write(x.Unknown[1]);
                    w.Write(x.Unknown[2]);
                });

                n.Centered = rw.Boolean(n.Centered);
                n.ColorBlendMode = rw.Int32(n.ColorBlendMode);
                n.IsContinousEffect = rw.Boolean(n.IsContinousEffect);
            }
        }

        #endregion

        #region 0x005 chunk

        /// <summary>
        /// CControlEffectSimi 0x005 chunk
        /// </summary>
        [Chunk(0x07010005)]
        public class Chunk07010005 : Chunk<CControlEffectSimi>
        {
            public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.List(n.Keys, (i, r) =>
                {
                    var time = r.ReadSingle();
                    var x = r.ReadSingle();
                    var y = r.ReadSingle();
                    var rot = r.ReadSingle();
                    var scaleX = r.ReadSingle();
                    var scaleY = r.ReadSingle();
                    var opacity = r.ReadSingle();
                    var depth = r.ReadSingle();
                    var u01 = r.ReadSingle();
                    var isContinousEffect = r.ReadSingle();
                    var u02 = r.ReadSingle();
                    var u03 = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        X = x,
                        Y = y,
                        Rotation = rot,
                        ScaleX = scaleX,
                        ScaleY = scaleY,
                        Opacity = opacity,
                        Depth = depth,
                        IsContinuousEffect = isContinousEffect,
                        Unknown = new float[] { u01, u02, u03 }
                    };
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.X);
                    w.Write(x.Y);
                    w.Write(x.Rotation);
                    w.Write(x.ScaleX);
                    w.Write(x.ScaleY);
                    w.Write(x.Opacity);
                    w.Write(x.Depth);
                    w.Write(x.Unknown[0]);
                    w.Write(x.IsContinuousEffect);
                    w.Write(x.Unknown[1]);
                    w.Write(x.Unknown[2]);
                });

                n.Centered = rw.Boolean(n.Centered);
                n.ColorBlendMode = rw.Int32(n.ColorBlendMode);
                n.IsContinousEffect = rw.Boolean(n.IsContinousEffect);
                n.IsInterpolated = rw.Boolean(n.IsInterpolated);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key
        {
            public float Time { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            /// <summary>
            /// Rotation in radians
            /// </summary>
            public float Rotation { get; set; }
            public float ScaleX { get; set; } = 1;
            public float ScaleY { get; set; } = 1;
            public float Opacity { get; set; } = 1;
            public float Depth { get; set; } = 0.5f;
            public float IsContinuousEffect { get; set; }
            public float[] Unknown { get; set; } = new float[] { 0, 0, 0 };
        }

        #endregion
    }
}
