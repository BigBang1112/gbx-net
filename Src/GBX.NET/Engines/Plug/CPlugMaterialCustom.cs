namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterialCustom
{
    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Chunk0903A00A;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class GpuFx
    {
        public string U01 { get; set; } = "";
        public bool U02 { get; set; }
        public float[][] U03 { get; set; } = [];

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadId();
            var count1 = r.ReadInt32();
            var count2 = r.ReadInt32();
            U02 = r.ReadBoolean();

            U03 = new float[count2][];

            for (var i = 0; i < count2; i++)
            {
                U03[i] = r.ReadArray<float>(count1);
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            var count2 = U03.Length;
            var count1 = U03.Length > 0 ? U03[0].Length : 0;

            w.WriteIdAsString(U01);
            w.Write(count1);
            w.Write(count2);
            w.Write(U02);

            for (var i = 0; i < count2; i++)
            {
                for (var j = 0; j < count1; j++)
                {
                    w.Write(U03[i][j]);
                }
            }
        }
    }
}
