using System.Diagnostics;

namespace GBX.NET.Engines.Control
{
    [Node(0x07010000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CControlEffectSimi : Node
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x004 chunk

        /// <summary>
        /// CControlEffectSimi 0x004 chunk
        /// </summary>
        [Chunk(0x07010004)]
        public class Chunk004 : Chunk<CControlEffectSimi>
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }

            public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var x = rw.Reader.ReadSingle();
                    var y = rw.Reader.ReadSingle();
                    var rot = rw.Reader.ReadSingle();
                    var scaleX = rw.Reader.ReadSingle();
                    var scaleY = rw.Reader.ReadSingle();
                    var opacity = rw.Reader.ReadSingle();
                    var depth = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(4);

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
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.X);
                    rw.Writer.Write(x.Y);
                    rw.Writer.Write(x.Rotation);
                    rw.Writer.Write(x.ScaleX);
                    rw.Writer.Write(x.ScaleY);
                    rw.Writer.Write(x.Opacity);
                    rw.Writer.Write(x.Depth);
                    rw.Writer.Write(x.Unknown);
                });

                rw.Array<int>(Unknown, 3); // 3
            }
        }

        #endregion

        #region 0x005 chunk

        /// <summary>
        /// CControlEffectSimi 0x005 chunk
        /// </summary>
        [Chunk(0x07010005)]
        public class Chunk005 : Chunk<CControlEffectSimi>
        {
            public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var x = rw.Reader.ReadSingle();
                    var y = rw.Reader.ReadSingle();
                    var rot = rw.Reader.ReadSingle();
                    var scaleX = rw.Reader.ReadSingle();
                    var scaleY = rw.Reader.ReadSingle();
                    var opacity = rw.Reader.ReadSingle();
                    var depth = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(4);

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
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.X);
                    rw.Writer.Write(x.Y);
                    rw.Writer.Write(x.Rotation);
                    rw.Writer.Write(x.ScaleX);
                    rw.Writer.Write(x.ScaleY);
                    rw.Writer.Write(x.Opacity);
                    rw.Writer.Write(x.Depth);
                    rw.Writer.Write(x.Unknown);
                });

                rw.Array<int>(Unknown, 4); // 4
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
            public float ScaleX { get; set; }
            public float ScaleY { get; set; }
            public float Opacity { get; set; }
            public float Depth { get; set; }
            public float[] Unknown { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CControlEffectSimi node;

            public Key[] Keys => node.Keys;

            public DebugView(CControlEffectSimi node) => this.node = node;
        }

        #endregion
    }
}
