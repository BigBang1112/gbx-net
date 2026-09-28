namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E020000</remarks>
[Class(0x2E020000)]
public partial class CGameItemPlacementParam : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E020000;




    private short flags;
    [AppliedWithChunk<Chunk2E020000>]
    public short Flags { get => flags; set => flags = value; }

    private Vec3 cubeCenter;
    [AppliedWithChunk<Chunk2E020000>]
    public Vec3 CubeCenter { get => cubeCenter; set => cubeCenter = value; }

    private float cubeSize;
    [AppliedWithChunk<Chunk2E020000>]
    public float CubeSize { get => cubeSize; set => cubeSize = value; }

    private float gridSnapHStep;
    [AppliedWithChunk<Chunk2E020000>]
    public float GridSnapHStep { get => gridSnapHStep; set => gridSnapHStep = value; }

    private float gridSnapVStep;
    [AppliedWithChunk<Chunk2E020000>]
    public float GridSnapVStep { get => gridSnapVStep; set => gridSnapVStep = value; }

    private float gridSnapHOffset;
    [AppliedWithChunk<Chunk2E020000>]
    public float GridSnapHOffset { get => gridSnapHOffset; set => gridSnapHOffset = value; }

    private float gridSnapVOffset;
    [AppliedWithChunk<Chunk2E020000>]
    public float GridSnapVOffset { get => gridSnapVOffset; set => gridSnapVOffset = value; }

    private float flyVStep;
    [AppliedWithChunk<Chunk2E020000>]
    public float FlyVStep { get => flyVStep; set => flyVStep = value; }

    private float flyVOffset;
    [AppliedWithChunk<Chunk2E020000>]
    public float FlyVOffset { get => flyVOffset; set => flyVOffset = value; }

    private float pivotSnapDistance;
    [AppliedWithChunk<Chunk2E020000>]
    public float PivotSnapDistance { get => pivotSnapDistance; set => pivotSnapDistance = value; }

    private Vec3[]? pivotPositions;
    [AppliedWithChunk<Chunk2E020001>]
    public Vec3[]? PivotPositions { get => pivotPositions; set => pivotPositions = value; }

    private Quat[]? pivotRotations;
    [AppliedWithChunk<Chunk2E020001>]
    public Quat[]? PivotRotations { get => pivotRotations; set => pivotRotations = value; }

    private NPlugItemPlacement_SClass? placementClass;
    [AppliedWithChunk<Chunk2E020005>]
    public NPlugItemPlacement_SClass? PlacementClass { get => placementClassFile?.GetNode(ref placementClass) ?? placementClass; set => placementClass = value; }
    private Components.GbxRefTableFile? placementClassFile;
    public Components.GbxRefTableFile? PlacementClassFile { get => placementClassFile; set => placementClassFile = value; }
    public NPlugItemPlacement_SClass? GetPlacementClass(GbxReadSettings settings = default, bool exceptions = false) => placementClassFile?.GetNode(ref placementClass, settings, exceptions) ?? placementClass;


    /// <summary>
    /// CGameItemPlacementParam 0x000 skippable chunk
    /// </summary>
    [Chunk(0x2E020000)]
    public partial class Chunk2E020000 : SkippableChunk<CGameItemPlacementParam>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E020000;

        public int Version { get; set; }


        public override void ReadWrite(CGameItemPlacementParam n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
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

    /// <summary>
    /// CGameItemPlacementParam 0x001 skippable chunk (pivot positions)
    /// </summary>
    [Chunk(0x2E020001, "pivot positions")]
    public partial class Chunk2E020001 : SkippableChunk<CGameItemPlacementParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E020001;


        public override void ReadWrite(CGameItemPlacementParam n, GbxReaderWriter rw)
        {
            rw.Array<Vec3>(ref n.pivotPositions!);
            rw.Array<Quat>(ref n.pivotRotations!);
        }
    }

    /// <summary>
    /// CGameItemPlacementParam 0x003 skippable chunk (PlacementClass)
    /// </summary>
    [Chunk(0x2E020003, "PlacementClass")]
    public partial class Chunk2E020003 : SkippableChunk<CGameItemPlacementParam>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E020003;

        /// <inheritdoc />
        public override bool Ignore => true;

        public int Version { get; set; }


        public override void ReadWrite(CGameItemPlacementParam n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
        }
    }

    /// <summary>
    /// CGameItemPlacementParam 0x004 skippable chunk
    /// </summary>
    [Chunk(0x2E020004)]
    public partial class Chunk2E020004 : SkippableChunk<CGameItemPlacementParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E020004;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGameItemPlacementParam 0x005 skippable chunk
    /// </summary>
    [Chunk(0x2E020005)]
    public partial class Chunk2E020005 : SkippableChunk<CGameItemPlacementParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E020005;


        public override void ReadWrite(CGameItemPlacementParam n, GbxReaderWriter rw)
        {
            rw.NodeRef<NPlugItemPlacement_SClass>(ref n.placementClass, ref n.placementClassFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E020000 => new Chunk2E020000(),
        0x2E020001 => new Chunk2E020001(),
        0x2E020003 => new Chunk2E020003(),
        0x2E020004 => new Chunk2E020004(),
        0x2E020005 => new Chunk2E020005(),
        _ => base.NewChunk(chunkId),
    };
}
