namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BA000</remarks>
[Node(0x090BA000)]
public class CPlugSkel : CMwNod
{
    private string? name;
    private Joint[] joints = Array.Empty<Joint>();
    private Socket[] sockets = Array.Empty<Socket>();
    private JointExpr[] jointExprs = Array.Empty<JointExpr>();

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BA000))]
    public string? Name { get => name; set => name = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BA000))]
    public Joint[] Joints { get => joints; set => joints = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BA000), sinceVersion: 6)]
    public Socket[] Sockets { get => sockets; set => sockets = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BA000), sinceVersion: 14)]
    public JointExpr[] JointExprs { get => jointExprs; set => jointExprs = value; }

    internal CPlugSkel()
    {
        
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CPlugSkel 0x000 chunk
    /// </summary>
    [Chunk(0x090BA000)]
    public class Chunk090BA000 : Chunk<CPlugSkel>, IVersionable
    {
        private int version;

        public bool U03;
        public bool U04;
        public int[]? U05;
        public byte[]? U06;
        public byte? U07;
        public int? U08;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSkel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Id(ref n.name);
            rw.ArrayArchive<Joint>(ref n.joints!, version, shortLength: true);

            if (version >= 2)
            {
                rw.Boolean(ref U03);

                if (U03)
                {
                    throw new Exception("u03 == true");
                }

                if (version >= 6)
                {
                    rw.ArrayArchive<Socket>(ref n.sockets!, version); // SPlugSkelSocket array

                    if (version >= 9)
                    {
                        rw.Boolean(ref U04);

                        if (U04)
                        {
                            /*var u07 = r.ReadArray(r => r.ReadId());
                            var u08 = r.ReadArray<ulong>();
                            var u09 = r.ReadArray<ulong>();
                            var u10 = r.ReadArray<Quat>();*/

                            throw new Exception("U04 == true");
                        }

                        if (version >= 10)
                        {
                            if (version >= 16)
                            {
                                rw.Bytes(ref U06);

                                if (version >= 18)
                                {
                                    rw.Bytes(ref U06);
                                }
                            }
                            else
                            {
                                rw.Array<int>(ref U05);
                            }

                            if (version >= 13)
                            {
                                if (version < 16)
                                {
                                    rw.Bytes(ref U06);
                                }

                                if (version == 14)
                                {
                                    // some array
                                    rw.Int32(0);
                                    // then NPlugSkel::ArchiveJointExprs
                                    rw.ArrayArchive<JointExpr>(ref n.jointExprs!);
                                }
                            }

                            if (version >= 17)
                            {
                                rw.Byte(ref U07);
                                rw.Int32(ref U08);
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #endregion

    public class Joint : IReadableWritable
    {
        private string? name;
        private short parentIndex;
        private Quat? globalJoint;
        private Vec3? u01;
        private Iso4? u02;

        public string? Name { get => name; set => name = value; }
        public short ParentIndex { get => parentIndex; set => parentIndex = value; }
        public Quat? GlobalJoint { get => globalJoint; set => globalJoint = value; }
        public Vec3? U01 { get => u01; set => u01 = value; }
        public Iso4? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref name);
            rw.Int16(ref parentIndex);

            if (version < 15)
            {
                rw.Quat(ref globalJoint);
                rw.Vec3(ref u01);
            }

            if (version >= 1)
            {
                rw.Iso4(ref u02);
            }
        }
    }

    public class Socket : IReadableWritable
    {
        private string? name;
        private short u01;
        private Iso4 u02;

        public string? Name { get => name; set => name = value; }
        public short U01 { get => u01; set => u01 = value; }
        public Iso4 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref name);
            rw.Int16(ref u01);
            rw.Iso4(ref u02);
        }
    }

    public class JointExpr : IReadableWritable
    {
        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            throw new NotImplementedException();
        }
    }
}
