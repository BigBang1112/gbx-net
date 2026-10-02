namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
{
    public CMwNod? GetPhyModelCustom(GbxReadSettings settings = default) => phyModelCustomFile?.GetNode(ref phyModelCustom, settings) ?? phyModelCustom;

    public CMwNod? GetVisModelCustom(GbxReadSettings settings = default) => visModelCustomFile?.GetNode(ref visModelCustom, settings) ?? visModelCustom;

    public CMwNod? GetEntityModelEdition(GbxReadSettings settings = default) => entityModelEditionFile?.GetNode(ref entityModelEdition, settings) ?? entityModelEdition;

    public CPlugGameSkinAndFolder? GetMaterialModifier(GbxReadSettings settings = default) => materialModifierFile?.GetNode(ref materialModifier, settings) ?? materialModifier;

    internal override IHeaderChunk? NewHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            return new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
        }

        return base.NewHeaderChunk(chunkId);
    }
}
