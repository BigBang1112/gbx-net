namespace GBX.NET.Engines.Plug;

[Node(0x090BA000)]
public class CPlugSkel : CMwNod
{
    private string? name;
    private Joint[] joints;

    [NodeMember(ExactlyNamed = true)]
    public string? Name { get => name; set => name = value; }

    [NodeMember]
    public Joint[] Joints { get => joints; set => joints = value; }

    protected CPlugSkel()
    {
        joints = Array.Empty<Joint>();
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

        public int Version { get => version; set => version = value; }

        public override void Read(CPlugSkel n, GameBoxReader r)
        {
            version = r.ReadInt32();
            n.name = r.ReadId();
            n.joints = new Joint[r.ReadUInt16()];

            for (var i = 0; i < n.joints.Length; i++)
            {
                var name = r.ReadId();
                var parentIndex = r.ReadInt16();
                var globalJoint = default(Quat?);
                var u01 = default(Vec3?);
                var u02 = default(Iso4?);

                if (version < 15)
                {
                    globalJoint = r.ReadQuat();
                    u01 = r.ReadVec3();
                }

                if (version >= 1)
                {
                    u02 = r.ReadIso4();
                }

                n.joints[i] = new()
                {
                    Name = name,
                    ParentIndex = parentIndex,
                    GlobalJoint = globalJoint,
                    U01 = u01,
                    U02 = u02
                };
            }

            if (version >= 2)
            {
                var u03 = r.ReadBoolean();

                if (u03)
                {
                    throw new Exception("u03 == true");
                }

                if (version >= 6)
                {
                    var u04 = r.ReadInt32(); // SPlugSkelSocket array

                    if (version >= 9)
                    {
                        var u05 = r.ReadBoolean();

                        if (u05)
                        {
                            throw new Exception("u03 == true");
                        }

                        if (version >= 11)
                        {
                            var u06 = r.ReadArray<int>();

                            if (version >= 14)
                            {
                                // some array
                                // then NPlugSkel::ArchiveJointExprs
                                throw new ChunkVersionNotSupportedException(version);
                            }
                        }
                    }
                    
                }
            }
        }
    }

    #endregion

    #endregion

    public class Joint
    {
        public string? Name { get; set; }
        public short ParentIndex { get; set; }
        public Quat? GlobalJoint { get; set; }
        public Vec3? U01 { get; set; }
        public Iso4? U02 { get; set; }
    }
}
