namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual
{
    public sealed partial class SSkinData : IReadableWritable, IReadable, IWritable
    {
        public bool U01 { get; set; }
        public int Flags { get; set; }
        public bool HasVertexWeights { get; set; }
        public bool HasBoneNames { get; set; }
        public int VertexCount { get; set; }
        public int SkinIndexCount { get; set; }
        public byte[] VertexWeights { get; set; } = [];
        public int BoneCount { get; set; }
        public Iso4[]? LegacyBoneTransforms { get; set; }
        public string[]? LegacyBones { get; set; }
        public BoxAligned[]? LegacyBoneBoundingBoxes { get; set; }
        public string[]? Bones { get; set; }
        public int[]? BoneIndices { get; set; }

        internal void ReadWriteVertexWeights(GbxReaderWriter rw, int vertexCount, int skinIndexCount)
        {
            if ((uint)(skinIndexCount - 1) > 3) throw new InvalidDataException("Skin index count must be between 1 and 4.");
            VertexCount = vertexCount;
            SkinIndexCount = skinIndexCount;
            var stride = skinIndexCount == 1 ? 1 : skinIndexCount * 4;
            var length = checked(vertexCount * stride);
            if (rw.Writer is not null && VertexWeights.Length != length)
            {
                throw new InvalidDataException("Skin vertex weight data length does not match the vertex count.");
            }
            VertexWeights = rw.Data(VertexWeights, length)!;
        }

        internal void ReadWrite(GbxReaderWriter rw, uint chunkId, int version, int vertexCount, int skinIndexCount)
        {
            VertexCount = vertexCount;
            SkinIndexCount = skinIndexCount;
            U01 = rw.Boolean(U01);
            Flags = version < 4 ? (rw.Boolean(Flags != 0) ? 1 : 0) : rw.Int32(Flags);
            if (version >= 3)
            {
                HasVertexWeights = rw.Boolean(HasVertexWeights);
                HasBoneNames = rw.Boolean(HasBoneNames);
            }
            else
            {
                HasVertexWeights = true;
                HasBoneNames = true;
            }
            if (HasVertexWeights) ReadWriteVertexWeights(rw, vertexCount, skinIndexCount);

            BoneCount = rw.Int32(BoneCount == 0 ? Bones?.Length ?? BoneIndices?.Length ?? 0 : BoneCount);
            var legacy = chunkId < 0x0900600F || version == 0;
            if (legacy)
            {
                // Preserve the transforms and per-bone bounds emitted by the C/D/E writers.
                LegacyBoneTransforms = rw.Array(LegacyBoneTransforms, BoneCount);
                if (chunkId == 0x0900600C) LegacyBones = rw.ArrayId(LegacyBones, BoneCount);
            }

            var nameCount = HasBoneNames ? BoneCount : 0;
            if (rw.Reader is not null)
            {
                Bones = new string[nameCount];
                if (legacy) LegacyBoneBoundingBoxes = new BoxAligned[nameCount];
            }
            for (var i = 0; i < nameCount; i++)
            {
                Bones![i] = rw.Id(Bones[i])!;
                if (legacy) LegacyBoneBoundingBoxes![i] = rw.BoxAligned(LegacyBoneBoundingBoxes[i]);
            }
            if (chunkId >= 0x0900600F && version != 1) BoneIndices = rw.Array(BoneIndices);
        }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            var format = v & 15;
            var chunkId = format == 0 ? 0x0900600Cu : format == 1 ? 0x0900600Du : 0x0900600Fu;
            ReadWrite(rw, chunkId, Math.Max(0, format - 2), VertexCount, v >> 4);
        }

        public void Read(GbxReader r, int v = 0)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(rw, v);
        }

        public void Write(GbxWriter w, int v = 0)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(rw, v);
        }
    }
}
