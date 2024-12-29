namespace GBX.NET.Engines.GameData;

public partial class CGameBlockItem
{
    private string archetypeBlockInfoId = string.Empty;
    [AppliedWithChunk<Chunk2E025000>]
    public string ArchetypeBlockInfoId { get => archetypeBlockInfoId; set => archetypeBlockInfoId = value; }

    private Id archetypeBlockInfoCollectionId = new();
    [AppliedWithChunk<Chunk2E025000>]
    public Id ArchetypeBlockInfoCollectionId { get => archetypeBlockInfoCollectionId; set => archetypeBlockInfoCollectionId = value; }

    private List<Mobil> customizedVariants = [];
    [AppliedWithChunk<Chunk2E025000>]
    public List<Mobil> CustomizedVariants { get => customizedVariants; set => customizedVariants = value; }

    public partial class Chunk2E025000 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameBlockItem n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.archetypeBlockInfoId);
            rw.Id(ref n.archetypeBlockInfoCollectionId);

            rw.ListReadableWritable<Mobil>(ref n.customizedVariants);

            if (Version >= 1
                && rw.Boolean(n.customizedVariants?.Any(x => x.Properties is not null) == true, asByte: true)
                && n.customizedVariants is not null)
            {
                for (var i = 0; i < n.customizedVariants.Count; i++)
                {
                    n.customizedVariants[i].Properties = rw.ReadableWritable<MobilProperties>(n.customizedVariants[i].Properties);
                }
            }
        }
    }

    public partial class Mobil
    {
        public MobilProperties? Properties { get; set; }
    }
}
