namespace GBX.NET.Engines.Control
{
    [Node(0x07010000)]
    public class CControlEffectSimi : Node
    {
        public Key[] Keys
        {
            get => GetValue<Chunk005>(x => x.Keys) as Key[];
            set => SetValue<Chunk005>(x => x.Keys = value);
        }

        public CControlEffectSimi(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x07010004)]
        public class Chunk004 : Chunk
        {
            public Key[] Keys { get; set; }

            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }

            public Chunk004(CControlEffectSimi node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
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

        [Chunk(0x07010005)]
        public class Chunk005 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk005(CControlEffectSimi node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
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
    }
}
