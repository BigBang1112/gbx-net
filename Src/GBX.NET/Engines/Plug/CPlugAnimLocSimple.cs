using System.Drawing;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F8000</remarks>
[Node(0x090F8000)]
public class CPlugAnimLocSimple : CMwNod
{
    private int rotPeriod;
    private int transPeriod;
    private float transY;
    private int axis;
    private int rotPeriodMax;
    private int transPeriodMax;
    private byte rotFunc;
    private float rotAngle;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000))]
    public int RotPeriod { get => rotPeriod; set => rotPeriod = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000))]
    public int TransPeriod { get => transPeriod; set => transPeriod = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000))]
    public float TransY { get => transY; set => transY = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000), sinceVersion: 1)]
    public int Axis { get => axis; set => axis = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000), sinceVersion: 2)]
    public int RotPeriodMax { get => rotPeriodMax; set => rotPeriodMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000), sinceVersion: 2)]
    public int TransPeriodMax { get => transPeriodMax; set => transPeriodMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000), sinceVersion: 3)]
    public byte RotFunc { get => rotFunc; set => rotFunc = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090F8000), sinceVersion: 3)]
    public float RotAngle { get => rotAngle; set => rotAngle = value; }

    internal CPlugAnimLocSimple()
	{

	}

    #region 0x000 chunk

    /// <summary>
    /// CPlugAnimLocSimple 0x000 chunk
    /// </summary>
    [Chunk(0x090F8000)]
    public class Chunk090F8000 : Chunk<CPlugAnimLocSimple>, IVersionable
    {
        private int versionOld;
        private int version;

        public int VersionOld { get => versionOld; set => versionOld = value; }
        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugAnimLocSimple n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref versionOld);
            rw.Int32(ref version);

            rw.Int32(n.rotPeriod);
            rw.Int32(n.transPeriod);
            rw.Single(n.transY);

            if (version >= 1)
            {
                rw.Int32(n.axis);

                if (version >= 2)
                {
                    rw.Int32(n.rotPeriodMax);
                    rw.Int32(n.transPeriodMax);

                    if (version >= 3)
                    {
                        rw.Byte(n.rotFunc);
                        rw.Single(n.rotAngle);
                    }
                }
            }
        }
    }

    #endregion
}
