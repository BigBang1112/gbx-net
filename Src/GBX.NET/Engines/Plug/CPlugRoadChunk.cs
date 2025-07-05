namespace GBX.NET.Engines.Plug;

public partial class CPlugRoadChunk
{
    public partial class Chunk09128000 : IVersionable
    {
        public int Version { get; set; }

        public int U01;
        public int U02;
        public Vec3[]? U03;
        public Vec3[]? U04;
        public Vec3[]? U05;
        public int U06;
        public Vec3[]? U07;
        public int U08;
        public byte U09;
        public byte U10;
        public float U11;
        public float U12;
        public byte U13;
        public string? U14;
        public Vec3[][]? U15;
        public byte U16;
        public string? U17;
        public Vec3 U18;
        public Quat U19;

        public override void ReadWrite(CPlugRoadChunk n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Array<Vec3>(ref U03!);
            rw.Array<Vec3>(ref U04!);
            rw.Array<Vec3>(ref U05!);
            if (Version >= 2)
            {
                rw.Int32(ref U06);
                rw.Array<Vec3>(ref U07!);
                if (Version >= 3)
                {
                    rw.Int32(ref U08);
                    if (Version >= 5)
                    {
                        rw.Byte(ref U09);
                        rw.Byte(ref U10);
                        if (Version >= 6)
                        {
                            rw.Single(ref U11);
                            rw.Single(ref U12);
                            if (Version >= 7)
                            {
                                rw.Byte(ref U13);
                                if (Version >= 8)
                                {
                                    rw.Id(ref U14);
                                    if (Version >= 9)
                                    {
                                        if (rw.Reader is not null)
                                        {
                                            var count = rw.Reader.ReadInt32();
                                            U15 = new Vec3[count][];

                                            for (var i = 0; i < count; i++)
                                            {
                                                U15[i] = rw.Reader.ReadArray<Vec3>();
                                            }
                                        }
                                        else if (rw.Writer is not null)
                                        {
                                            rw.Writer.Write(U15?.Length ?? 0);

                                            if (U15 is not null)
                                            {
                                                foreach (var array in U15)
                                                {
                                                    rw.Writer.WriteArray(array);
                                                }
                                            }
                                        }

                                        if (Version >= 10)
                                        {
                                            rw.Byte(ref U16);
                                            rw.Id(ref U17);
                                            if (Version == 11)
                                            {
                                                rw.Vec3(ref U18);
                                            }
                                            if (Version >= 12)
                                            {
                                                rw.Quat(ref U19);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
