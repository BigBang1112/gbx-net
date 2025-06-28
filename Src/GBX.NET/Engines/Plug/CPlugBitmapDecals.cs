namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmapDecals
{
    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Chunk09078000;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Decal;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class DecalSet
    {
        public Decal[][] Decals { get; set; } = [];

        public void Read(GbxReader r, int v = 0)
        {
            var count = r.ReadInt32();
            Decals = new Decal[count][];

            if (v < 3)
            {
                for (var i = 0; i < count; i++)
                {
                    Decals[i] = [r.ReadReadable<Decal>(v)];
                }
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    var count2 = r.ReadInt32();
                    Decals[i] = new Decal[count2];
                    for (var j = 0; j < count2; j++)
                    {
                        var decal = new Decal();
                        decal.Read(r, v);
                        Decals[i][j] = decal;
                    }
                }
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(Decals.Length);
            if (v < 3)
            {
                foreach (var decal in Decals)
                {
                    w.WriteWritable(decal[0], v);
                }
            }
            else
            {
                foreach (var decals in Decals)
                {
                    w.Write(decals.Length);
                    foreach (var decal in decals)
                    {
                        decal.Write(w, v);
                    }
                }
            }
        }
    }
}
