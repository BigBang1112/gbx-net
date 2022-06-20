namespace GBX.NET.Engines.Plug;

/// <summary>
/// Custom material (0x0903A000)
/// </summary>
[Node(0x0903A000), WritingNotSupported]
public class CPlugMaterialCustom : CPlug
{
    public SBitmap[]? Textures { get; set; }

    protected CPlugMaterialCustom()
    {

    }

    [Chunk(0x0903A004)]
    public class Chunk0903A004 : Chunk<CPlugMaterialCustom>
    {
        public int[]? U01;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.Array<int>(ref U01);
        }
    }

    [Chunk(0x0903A006)]
    public class Chunk0903A006 : Chunk<CPlugMaterialCustom>
    {
        public override void Read(CPlugMaterialCustom n, GameBoxReader r)
        {
            n.Textures = r.ReadArray(r =>
            {
                var name = r.ReadId();
                var u01 = r.ReadInt32();

                _ = r.ReadNodeRef<CPlugBitmap>(out int bitmapIndex);

                return new SBitmap(n, name, u01, bitmapIndex);
            });
        }
    }

    [Chunk(0x0903A00A)]
    public class Chunk0903A00A : Chunk<CPlugMaterialCustom>
    {
        public object? U01;

        public override void Read(CPlugMaterialCustom n, GameBoxReader r)
        {
            U01 = r.ReadArray(2, (i, r) =>
            {
                return r.ReadArray(r =>
                {
                    var u01 = r.ReadId();
                    var count1 = r.ReadInt32();
                    var count2 = r.ReadInt32();
                    var u02 = r.ReadBoolean();

                    var u03 = r.ReadArray(count2, r => r.ReadArray<float>(count1));

                    return new
                    {
                        u01,
                        u02,
                        u03
                    };
                });
            });
        }
    }

    [Chunk(0x0903A00B)]
    public class Chunk0903A00B : Chunk<CPlugMaterialCustom>
    {
        public override void Read(CPlugMaterialCustom n, GameBoxReader r)
        {
            var flags = r.ReadInt32();
            var u01 = r.ReadUInt64();

            if ((flags & 1) != 0) // SPlugVisibleFilter
            {
                var u02 = r.ReadInt16();
                var u03 = r.ReadInt16();
            }
        }
    }

    [Chunk(0x0903A00C)]
    public class Chunk0903A00C : Chunk<CPlugMaterialCustom>
    {
        public (string, bool)[]? GpuParamSkipSamplers;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            // array of SPlugGpuParamSkipSampler
            rw.Array(ref GpuParamSkipSamplers, 
                r => (r.ReadId(), r.ReadBoolean()),
                (x, w) =>
                {
                    w.Write(x.Item1);
                    w.Write(x.Item2);
                });
        }
    }

    [Chunk(0x0903A00D)]
    public class Chunk0903A00D : Chunk<CPlugMaterialCustom>
    {
        public ulong U01;
        public ulong U02;
        public short? U03;
        public short? U04;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.UInt64(ref U02);

            if ((U01 & 1) != 0)
            {
                // SPlugVisibleFilter
                rw.Int16(ref U03);
                rw.Int16(ref U04);
                //
            }
        }
    }

    public class SBitmap
    {
        private Node node;
        private CPlugBitmap? bitmap;
        private int bitmapIndex;

        public string Name { get; set; }
        public int U01 { get; set; }

        public CPlugBitmap? Bitmap
        {
            get => bitmap = node.GetNodeFromRefTable(bitmap, bitmapIndex) as CPlugBitmap;
            set => bitmap = value;
        }

        public SBitmap(Node node, string name, int u01, int bitmapIndex)
        {
            Name = name;
            U01 = u01;

            this.node = node;
            this.bitmapIndex = bitmapIndex;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
