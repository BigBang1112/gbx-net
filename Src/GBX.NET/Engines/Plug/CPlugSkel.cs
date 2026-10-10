namespace GBX.NET.Engines.Plug;

public partial class CPlugSkel
{
    private string name = string.Empty;
    private Joint[] joints = [];
    private Socket[] sockets = [];
    private JointExpr[] jointExprs = [];
    private bool u01;
    private bool u02;
    private int[]? u03;
    private byte[]? u04;
    private byte? u05;
    private int? u06;
    private byte[]? u07;
    private Vec3[]? u08;
    private Setup? setup;
    private ExtraJoints? extraJoints;

    public string Name { get => name; set => name = value; }
    public bool U01 { get => u01; set => u01 = value; }
    public bool U02 { get => u02; set => u02 = value; }
    public int[]? U03 { get => u03; set => u03 = value; }
    public byte[]? U04 { get => u04; set => u04 = value; }
    public byte? U05 { get => u05; set => u05 = value; }
    public int? U06 { get => u06; set => u06 = value; }
    public byte[]? U07 { get => u07; set => u07 = value; }
    public Vec3[]? U08 { get => u08; set => u08 = value; }
    public Setup? SkelSetup { get => setup; set => setup = value; }
    public ExtraJoints? AdditionalJoints { get => extraJoints; set => extraJoints = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.Id(ref name);
        var jointsLength = rw.Int16(joints.Length);
        rw.ArrayReadableWritable<Joint>(ref joints!, jointsLength, v);

        if (v >= 2)
        {
            rw.Boolean(ref u01);

            if (u01)
            {
                rw.ReadableWritable(ref setup, v);
            }

            if (v >= 6)
            {
                rw.ArrayReadableWritable<Socket>(ref sockets!, version: v); // SPlugSkelSocket array

                if (v >= 9)
                {
                    rw.Boolean(ref u02);

                    if (u02)
                    {
                        rw.ReadableWritable(ref extraJoints);
                    }

                    if (v >= 10)
                    {
                        if (v >= 16)
                        {
                            rw.Data(ref u04);

                            if (v >= 18)
                            {
                                // Second u04 data read in version >= 18
                                rw.Data(ref u04);
                            }
                        }
                        else
                        {
                            rw.Array<int>(ref u03);
                        }

                        if (v >= 13)
                        {
                            if (v < 16)
                            {
                                rw.Data(ref u04);
                            }

                            if (v == 14)
                            {
                                // Empty array (count = 0)
                                rw.Int32(0);
                                // Then NPlugSkel::ArchiveJointExprs
                                rw.ArrayReadableWritable<JointExpr>(ref jointExprs!);
                            }

                            if (v >= 19)
                            {
                                rw.Data(ref u07);
                            }
                        }

                        if (v >= 17)
                        {
                            if (v >= 20)
                            {
                                rw.Array<Vec3>(ref u08);
                            }

                            rw.Byte(ref u05);
                            rw.Int32(ref u06);
                        }
                    }
                }
            }
        }
    }

    public partial class Chunk090BA000 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CPlugSkel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            n.ReadWrite(rw, Version);
        }
    }
}
