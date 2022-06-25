namespace GBX.NET.Engines.GameData;

/// <summary>
/// Item placement parameters (0x2E020000)
/// </summary>
[Node(0x2E020000)]
public class CGameItemPlacementParam : CMwNod
{
    #region Constants

    const int yawOnlyBit = 1;
    const int notOnObjectBit = 2;
    const int autoRotationBit = 3;
    const int switchPivotManuallyBit = 4;

    #endregion

    #region Fields

    private short flags;
    private Vec3 cubeCenter;
    private float cubeSize;
    private float gridSnapHStep;
    private float gridSnapVStep;
    private float gridSnapHOffset;
    private float gridSnapVOffset;
    private float flyVStep;
    private float flyVOffset;
    private float pivotSnapDistance;
    private Vec3[]? pivotPositions;
    private Quat[]? pivotRotations;

    #endregion

    #region Properties

    public short Flags
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return flags;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            flags = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public bool YawOnly
    {
        get => (Flags & (1 << yawOnlyBit)) != 0;
        set
        {
            if (value) Flags |= 1 << yawOnlyBit;
            else Flags &= ~(1 << yawOnlyBit);
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public bool NotOnObject
    {
        get => (Flags & (1 << notOnObjectBit)) != 0;
        set
        {
            if (value) Flags |= 1 << notOnObjectBit;
            else Flags &= ~(1 << notOnObjectBit);
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public bool AutoRotation
    {
        get => (Flags & (1 << autoRotationBit)) != 0;
        set
        {
            if (value) Flags |= 1 << autoRotationBit;
            else Flags &= ~(1 << autoRotationBit);
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public bool SwitchPivotManually
    {
        get => (Flags & (1 << switchPivotManuallyBit)) != 0;
        set
        {
            if (value) Flags |= 1 << switchPivotManuallyBit;
            else Flags &= ~(1 << switchPivotManuallyBit);
        }
    }

    [NodeMember(ExactName = "Cube_Center")]
    public Vec3 CubeCenter
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return cubeCenter;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            cubeCenter = value;
        }
    }

    [NodeMember(ExactName = "Cube_Size")]
    public float CubeSize
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return cubeSize;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            cubeSize = value;
        }
    }

    [NodeMember(ExactName = "GridSnap_HStep")]
    public float GridSnapHStep
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return gridSnapHStep;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            gridSnapHStep = value;
        }
    }

    [NodeMember(ExactName = "GridSnap_VStep")]
    public float GridSnapVStep
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return gridSnapVStep;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            gridSnapVStep = value;
        }
    }

    [NodeMember(ExactName = "GridSnap_HOffset")]
    public float GridSnapHOffset
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return gridSnapHOffset;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            gridSnapHOffset = value;
        }
    }

    [NodeMember(ExactName = "GridSnap_VOffset")]
    public float GridSnapVOffset
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return gridSnapVOffset;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            gridSnapVOffset = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public float FlyVStep
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return flyVStep;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            flyVStep = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public float FlyVOffset
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return flyVOffset;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            flyVOffset = value;
        }
    }

    [NodeMember(ExactName = "PivotSnap_Distance")]
    public float PivotSnapDistance
    {
        get
        {
            DiscoverChunk<Chunk2E020000>();
            return pivotSnapDistance;
        }
        set
        {
            DiscoverChunk<Chunk2E020000>();
            pivotSnapDistance = value;
        }
    }

    [NodeMember(ExactName = "Pivots_Positions")]
    public Vec3[]? PivotPositions
    {
        get
        {
            DiscoverChunk<Chunk2E020001>();
            return pivotPositions;
        }
        set
        {
            DiscoverChunk<Chunk2E020001>();
            pivotPositions = value;
        }
    }

    [NodeMember(ExactName = "PivotRotations")]
    public Quat[]? PivotRotations
    {
        get
        {
            DiscoverChunk<Chunk2E020001>();
            return pivotRotations;
        }
        set
        {
            DiscoverChunk<Chunk2E020001>();
            pivotRotations = value;
        }
    }

    #endregion

    #region Constructors

    protected CGameItemPlacementParam()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 skippable chunk

    /// <summary>
    /// CGameItemPlacementParam 0x000 skippable chunk
    /// </summary>
    [Chunk(0x2E020000)]
    public class Chunk2E020000 : SkippableChunk<CGameItemPlacementParam>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemPlacementParam n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int16(ref n.flags);
            rw.Vec3(ref n.cubeCenter);
            rw.Single(ref n.cubeSize);
            rw.Single(ref n.gridSnapHStep);
            rw.Single(ref n.gridSnapVStep);
            rw.Single(ref n.gridSnapHOffset);
            rw.Single(ref n.gridSnapVOffset);
            rw.Single(ref n.flyVStep);
            rw.Single(ref n.flyVOffset);
            rw.Single(ref n.pivotSnapDistance);
        }
    }

    #endregion

    #region 0x001 skippable chunk (pivot positions)

    /// <summary>
    /// CGameItemPlacementParam 0x001 skippable chunk (pivot positions)
    /// </summary>
    [Chunk(0x2E020001, "pivot positions")]
    public class Chunk2E020001 : SkippableChunk<CGameItemPlacementParam>
    {
        public int U01;

        public override void ReadWrite(CGameItemPlacementParam n, GameBoxReaderWriter rw)
        {
            rw.Array<Vec3>(ref n.pivotPositions);
            rw.Array<Quat>(ref n.pivotRotations);
        }
    }

    #endregion

    #endregion
}
