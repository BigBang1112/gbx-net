namespace GBX.NET.Engines.GameData;

/// <summary>
/// Custom block.
/// </summary>
/// <remarks>ID: 0x2E025000</remarks>
[Node(0x2E025000)]
[NodeExtension("BlockItem")]
public class CGameBlockItem : CMwNod
{
    private string archetypeBlockInfoId;
    private string archetypeBlockInfoCollectionId;
    private IDictionary<int, CPlugCrystal> customizedVariants;

    [NodeMember]
    [AppliedWithChunk<Chunk2E025000>]
    public string ArchetypeBlockInfoId
    {
        get => archetypeBlockInfoId;
        set => archetypeBlockInfoId = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk2E025000>]
    public string ArchetypeBlockInfoCollectionId
    {
        get => archetypeBlockInfoCollectionId;
        set => archetypeBlockInfoCollectionId = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk2E025000>]
    public IDictionary<int, CPlugCrystal> CustomizedVariants
    {
        get => customizedVariants;
        set => customizedVariants = value;
    }

    internal CGameBlockItem()
    {
        archetypeBlockInfoId = "";
        archetypeBlockInfoCollectionId = "";
#if NET6_0_OR_GREATER
        customizedVariants = global::System.Collections.Immutable.ImmutableDictionary.Create<int, CPlugCrystal>();
#else
        customizedVariants = null!;
#endif
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameBlockItem 0x000 chunk
    /// </summary>
    [Chunk(0x2E025000)]
    public class Chunk2E025000 : Chunk<CGameBlockItem>, IVersionable
    {
        private int version;

        public byte U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameBlockItem n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Id(ref n.archetypeBlockInfoId!);
            rw.Id(ref n.archetypeBlockInfoCollectionId!);
            rw.DictionaryNode(ref n.customizedVariants!);

            if (version >= 1)
            {
                rw.Byte(ref U01);

                if (version >= 2)
                {
                    throw new ChunkVersionNotSupportedException(version);
                }
            }
        }
    }

    #endregion
}
