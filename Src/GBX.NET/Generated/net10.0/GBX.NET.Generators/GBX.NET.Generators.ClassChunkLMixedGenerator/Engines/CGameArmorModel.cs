namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E00D000</remarks>
[Class(0x2E00D000)]
public partial class CGameArmorModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E00D000;




    private int armorMax;
    [AppliedWithChunk<Chunk2E00D000>]
    public int ArmorMax { get => armorMax; set => armorMax = value; }

    private int replenishDelayAfterDamage;
    [AppliedWithChunk<Chunk2E00D000>]
    public int ReplenishDelayAfterDamage { get => replenishDelayAfterDamage; set => replenishDelayAfterDamage = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameArmorModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameArmorModel() { }


    /// <summary>
    /// CGameArmorModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E00D000)]
    public partial class Chunk2E00D000 : Chunk<CGameArmorModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00D000;

        public int Version { get; set; }


        public override void ReadWrite(CGameArmorModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.armorMax);
            rw.Int32(ref n.replenishDelayAfterDamage);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E00D000 => new Chunk2E00D000(),
        _ => base.NewChunk(chunkId),
    };
}
