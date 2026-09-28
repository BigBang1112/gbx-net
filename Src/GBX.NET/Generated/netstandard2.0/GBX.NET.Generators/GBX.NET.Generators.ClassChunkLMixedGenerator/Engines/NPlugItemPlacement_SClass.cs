namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09187000</remarks>
[Class(0x09187000)]
public partial class NPlugItemPlacement_SClass : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09187000;




    private string? sizeGroup;
    public string? SizeGroup { get => sizeGroup; set => sizeGroup = value; }

    private string[]? compatibleGroupsIds;
    public string[]? CompatibleGroupsIds { get => compatibleGroupsIds; set => compatibleGroupsIds = value; }

    private bool alwaysUp;
    public bool AlwaysUp { get => alwaysUp; set => alwaysUp = value; }

    private bool alignToInterior;
    public bool AlignToInterior { get => alignToInterior; set => alignToInterior = value; }

    private bool alignToWorldDir;
    public bool AlignToWorldDir { get => alignToWorldDir; set => alignToWorldDir = value; }

    private Vec3 worldDir;
    public Vec3 WorldDir { get => worldDir; set => worldDir = value; }

    private PatchLayout[]? patchLayouts;
    public PatchLayout[]? PatchLayouts { get => patchLayouts; set => patchLayouts = value; }

    private int[]? groupCurPatchLayouts;
    public int[]? GroupCurPatchLayouts { get => groupCurPatchLayouts; set => groupCurPatchLayouts = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.Id(ref sizeGroup);
        rw.ArrayId(ref compatibleGroupsIds!);
        rw.Boolean(ref alwaysUp);
        rw.Boolean(ref alignToInterior);
        rw.Boolean(ref alignToWorldDir);
        rw.Vec3(ref worldDir);
        rw.ArrayReadableWritable<PatchLayout>(ref patchLayouts!);
        rw.Array<int>(ref groupCurPatchLayouts!);
    }



    public sealed partial class PatchLayout : IReadableWritable
    {

        private int itemCount;
        public int ItemCount { get => itemCount; set => itemCount = value; }

        private float itemSpacing;
        public float ItemSpacing { get => itemSpacing; set => itemSpacing = value; }

        private int fillAlign;
        public int FillAlign { get => fillAlign; set => fillAlign = value; }

        private int fillDir;
        public int FillDir { get => fillDir; set => fillDir = value; }

        private float normedPos;
        public float NormedPos { get => normedPos; set => normedPos = value; }

        private float u01;
        /// <summary>
        /// DistFromNormedPos?
        /// </summary>
        public float U01 { get => u01; set => u01 = value; }

        private string[]? onlyOnGroups;
        public string[]? OnlyOnGroups { get => onlyOnGroups; set => onlyOnGroups = value; }

        private float altitude;
        public float Altitude { get => altitude; set => altitude = value; }

        private float u02;
        /// <summary>
        /// FillBorderOffset?
        /// </summary>
        public float U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref itemCount);
            rw.Single(ref itemSpacing);
            rw.Int32(ref fillAlign);
            rw.Int32(ref fillDir);
            rw.Single(ref normedPos);
            rw.Single(ref u01); // DistFromNormedPos?
            rw.ArrayId(ref onlyOnGroups!);
            rw.Single(ref altitude);
            rw.Single(ref u02); // FillBorderOffset?
        }
    }


}
