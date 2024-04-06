namespace GBX.NET.Engines.Plug;

public partial class CPlugSkel
{
    private string? name;
    private Joint[] joints = [];
    private Socket[] sockets = [];
    private JointExpr[] jointExprs = [];

    public partial class Chunk090BA000 : IVersionable
    {
        public int Version { get; set; }

        public bool U01;
        public bool U02;
        public int[]? U03;
        public byte[]? U04;
        public byte? U05;
        public int? U06;
        public byte[]? U07;

        public override void ReadWrite(CPlugSkel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.name);
            var jointsLength = rw.Int16(n.joints.Length);
            rw.ArrayReadableWritable<Joint>(ref n.joints!, jointsLength, Version);

            if (Version >= 2)
            {
                rw.Boolean(ref U01);

                if (U01)
                {
                    throw new Exception("u03 == true");
                }

                if (Version >= 6)
                {
                    rw.ArrayReadableWritable<Socket>(ref n.sockets!, version: Version); // SPlugSkelSocket array

                    if (Version >= 9)
                    {
                        rw.Boolean(ref U02);

                        if (U02)
                        {
                            /*var u07 = r.ReadArray(r => r.ReadId());
                            var u08 = r.ReadArray<ulong>();
                            var u09 = r.ReadArray<ulong>();
                            var u10 = r.ReadArray<Quat>();*/

                            throw new Exception("U04 == true");
                        }

                        if (Version >= 10)
                        {
                            if (Version >= 16)
                            {
                                rw.Data(ref U04);

                                if (Version >= 18)
                                {
                                    rw.Data(ref U04);
                                }
                            }
                            else
                            {
                                rw.Array<int>(ref U03);
                            }

                            if (Version >= 13)
                            {
                                if (Version < 16)
                                {
                                    rw.Data(ref U04);
                                }

                                if (Version == 14)
                                {
                                    // some array
                                    rw.Int32(0);
                                    // then NPlugSkel::ArchiveJointExprs
                                    rw.ArrayReadableWritable<JointExpr>(ref n.jointExprs!);
                                }

                                if (Version >= 19)
                                {
                                    rw.Data(ref U07);
                                }
                            }

                            if (Version >= 17)
                            {
                                rw.Byte(ref U05);
                                rw.Int32(ref U06);
                            }
                        }
                    }
                }
            }
        }
    }
}
