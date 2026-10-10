namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual
{
    [Obsolete("Use Morphs instead.")]
    public int MorphCount
    {
        get => Morphs.Length;
        set
        {
            var morphs = Morphs;
            Array.Resize(ref morphs, value);
            for (var i = 0; i < morphs.Length; i++) morphs[i] ??= new Morph();
            Morphs = morphs;
        }
    }

    public partial class Chunk09006001
    {
        private string? legacyVisualId;
        private bool legacyVisualIdSet;

        [Obsolete("Use CPlugVisual.VisualId instead.")]
        public string? U01
        {
            get => legacyVisualId;
            set { legacyVisualId = value; legacyVisualIdSet = true; }
        }

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            if (rw.Reader is null && legacyVisualIdSet) n.VisualId = legacyVisualId;
            n.VisualId = rw.Id(n.VisualId);
            legacyVisualId = n.VisualId;
            legacyVisualIdSet = false;
        }
    }

    public partial class Chunk09006004
    {
        private CMwNod? legacyFuncVisual;
        private bool legacyFuncVisualSet;

        [Obsolete("Use CPlugVisual.FuncVisual instead.")]
        public CMwNod? U01
        {
            get => legacyFuncVisual;
            set { legacyFuncVisual = value; legacyFuncVisualSet = true; }
        }

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            if (rw.Reader is null && legacyFuncVisualSet) n.FuncVisual = legacyFuncVisual;
            n.FuncVisual = rw.NodeRef(n.FuncVisual);
            legacyFuncVisual = n.FuncVisual;
            legacyFuncVisualSet = false;
        }
    }

    public partial class Chunk09006007
    {
        private bool? legacyInverse;
        private bool legacyInverseSet;

        [Obsolete("Use CPlugVisual.IsInverse instead.")]
        public bool U01
        {
            get => legacyInverse.GetValueOrDefault();
            set { legacyInverse = value; legacyInverseSet = true; }
        }

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            if (rw.Reader is null && legacyInverseSet) n.IsInverse = legacyInverse.GetValueOrDefault();
            n.IsInverse = rw.Boolean(n.IsInverse);
            legacyInverse = n.IsInverse;
            legacyInverseSet = false;
        }
    }

    public partial class Chunk09006008
    {
        [Obsolete("Use LegacyBoneTransforms instead.")]
        public Iso4[]? U01
        {
            get => LegacyBoneTransforms;
            set => LegacyBoneTransforms = value;
        }
    }

    public partial class Chunk09006009
    {
        private float? legacyNPatchTessLevel;
        private bool legacyNPatchTessLevelSet;

        [Obsolete("Use CPlugVisual.NPatchTessLevel instead.")]
        public float U01
        {
            get => legacyNPatchTessLevel.GetValueOrDefault();
            set { legacyNPatchTessLevel = value; legacyNPatchTessLevelSet = true; }
        }

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            if (rw.Reader is null && legacyNPatchTessLevelSet) n.NPatchTessLevel = legacyNPatchTessLevel.GetValueOrDefault();
            n.NPatchTessLevel = rw.Single(n.NPatchTessLevel);
            legacyNPatchTessLevel = n.NPatchTessLevel;
            legacyNPatchTessLevelSet = false;
        }
    }

    public partial class Chunk0900600F
    {
        [Obsolete("Use PackedDataVersion instead.")]
        public int U02
        {
            get => PackedDataVersion;
            set => PackedDataVersion = value;
        }

        [Obsolete("Use PackedData instead.")]
        public int U03
        {
            get => PackedData is null ? 0 : checked(PackedData.Length + 4);
            set => PackedData = value == 0 ? null : new byte[checked(value - 4)];
        }

        [Obsolete("Use PackedData instead.")]
        public byte[]? U04
        {
            get => PackedData;
            set => PackedData = value;
        }
    }

    public partial class Split
    {
        [Obsolete("Use IndexOffset instead.")]
        public int U01
        {
            get => IndexOffset;
            set => IndexOffset = value;
        }

        [Obsolete("Use VertexOffset instead.")]
        public int U02
        {
            get => VertexOffset;
            set => VertexOffset = value;
        }
    }

    public partial class BitmapElemToPack
    {
        [Obsolete("Use MapperIndex instead.")]
        public int U01
        {
            get => MapperIndex;
            set => MapperIndex = value;
        }

        [Obsolete("Use Scale.X instead.")]
        public int U02
        {
            get => BitConverter.ToInt32(BitConverter.GetBytes(Scale.X), 0);
            set => Scale = Scale with { X = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
        }

        [Obsolete("Use Scale.Y instead.")]
        public int U03
        {
            get => BitConverter.ToInt32(BitConverter.GetBytes(Scale.Y), 0);
            set => Scale = Scale with { Y = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
        }

        [Obsolete("Use Translation.X instead.")]
        public int U04
        {
            get => BitConverter.ToInt32(BitConverter.GetBytes(Translation.X), 0);
            set => Translation = Translation with { X = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
        }

        [Obsolete("Use Translation.Y instead.")]
        public int U05
        {
            get => BitConverter.ToInt32(BitConverter.GetBytes(Translation.Y), 0);
            set => Translation = Translation with { Y = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
        }
    }

    public sealed partial class SSkinData
    {
        [Obsolete("Use Flags instead.")]
        public int U02 { get => Flags; set => Flags = value; }

        [Obsolete("Use HasVertexWeights instead.")]
        public bool U03 { get => HasVertexWeights; set => HasVertexWeights = value; }

        [Obsolete("Use HasBoneNames instead.")]
        public bool U04 { get => HasBoneNames; set => HasBoneNames = value; }

        [Obsolete("Use LegacyBoneTransforms instead.")]
        public Iso4[]? U05 { get => LegacyBoneTransforms; set => LegacyBoneTransforms = value; }

        [Obsolete("Use BoneIndices instead.")]
        public int[]? U07 { get => BoneIndices; set => BoneIndices = value; }
    }
}
