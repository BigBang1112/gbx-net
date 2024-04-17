namespace GBX.NET.Engines.GameData;

public partial class CGameBlockItem
{
    [AppliedWithChunk<Chunk2E025000>]
    public string ArchetypeBlockInfoId { get; set; } = "";

    [AppliedWithChunk<Chunk2E025000>]
    public string ArchetypeBlockInfoCollectionId { get; set; } = "";

    [AppliedWithChunk<Chunk2E025000>]
    public Dictionary<int, CPlugCrystal> CustomizedVariants { get; set; } = [];

    public partial class Chunk2E025000 : IVersionable
    {
        public int Version { get; set; }

        public short U01 = 1;

        public override void Read(CGameBlockItem n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.ArchetypeBlockInfoId = r.ReadId();
            n.ArchetypeBlockInfoCollectionId = r.ReadId();

            n.CustomizedVariants = [];
            for (var i = 0; i < r.ReadInt32(); i++)
            {
                n.CustomizedVariants.Add(r.ReadInt32(), r.ReadNodeRef<CPlugCrystal>()!);
            }

            if (Version >= 1)
            {
                U01 = r.ReadByte();
            }
        }

        public override void Write(CGameBlockItem n, GbxWriter w)
        {
            w.Write(Version);
            w.WriteIdAsString(n.ArchetypeBlockInfoId);
            w.WriteIdAsString(n.ArchetypeBlockInfoCollectionId);

            w.Write(n.CustomizedVariants?.Count ?? 0);
            if (n.CustomizedVariants is not null)
            {
                foreach (var pair in n.CustomizedVariants)
                {
                    w.Write(pair.Key);
                    w.WriteNodeRef(pair.Value);
                }
            }

            if (Version >= 1)
            {
                w.Write(U01);
            }
        }
    }
}
