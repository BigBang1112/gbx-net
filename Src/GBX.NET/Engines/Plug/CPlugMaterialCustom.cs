namespace GBX.NET.Engines.Plug;

[Node(0x0903A000), WritingNotSupported]
public sealed class CPlugMaterialCustom : CPlug
{
    private CPlugMaterialCustom()
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
        public object[]? U01;
        public int U02;

        public override void Read(CPlugMaterialCustom n, GameBoxReader r)
        {
            U01 = r.ReadArray(r =>
            {
                return new
                {
                    u01 = r.ReadId(),
                    u02 = r.ReadInt32(),
                    u03 = r.ReadInt32()
                };
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
        public int U01;
        public string? U02;
        public int U03;
        public string? U04;

        public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Id(ref U02);
            rw.Int32(ref U03);
            rw.Id(ref U04);

            // ...
        }
    }
}
