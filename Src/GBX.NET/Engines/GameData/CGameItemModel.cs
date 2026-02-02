using GBX.NET.Components;

namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
{
    private CMwNod? phyModelCustom;
    public CMwNod? PhyModelCustom
    {
        get => phyModelCustomFile?.GetNode(ref phyModelCustom) ?? phyModelCustom;
        set => phyModelCustom = value;
    }
    private GbxRefTableFile? phyModelCustomFile;
    public GbxRefTableFile? PhyModelCustomFile { get => phyModelCustomFile; set => phyModelCustomFile = value; }
    public CMwNod? GetPhyModelCustom(GbxReadSettings settings = default) => phyModelCustomFile?.GetNode(ref phyModelCustom, settings) ?? phyModelCustom;

    private CMwNod? visModelCustom;
    public CMwNod? VisModelCustom
    {
        get => visModelCustomFile?.GetNode(ref visModelCustom) ?? visModelCustom;
        set => visModelCustom = value;
    }
    private GbxRefTableFile? visModelCustomFile;
    public GbxRefTableFile? VisModelCustomFile { get => visModelCustomFile; set => visModelCustomFile = value; }
    public CMwNod? GetVisModelCustom(GbxReadSettings settings = default) => visModelCustomFile?.GetNode(ref visModelCustom, settings) ?? visModelCustom;


    private string? defaultWeaponName;
    public string? DefaultWeaponName { get => defaultWeaponName; set => defaultWeaponName = value; }

    private CGameActionModel[]? actions;
    public CGameActionModel[]? Actions { get => actions; set => actions = value; }

    private EDefaultCam defaultCam;
    public EDefaultCam DefaultCam { get => defaultCam; set => defaultCam = value; }

    private CMwNod? entityModelEdition;
    public CMwNod? EntityModelEdition
    {
        get => entityModelEditionFile?.GetNode(ref entityModelEdition) ?? entityModelEdition;
        set => entityModelEdition = value;
    }
    private GbxRefTableFile? entityModelEditionFile;
    public GbxRefTableFile? EntityModelEditionFile { get => entityModelEditionFile; set => entityModelEditionFile = value; }
    public CMwNod? GetEntityModelEdition(GbxReadSettings settings = default) => entityModelEditionFile?.GetNode(ref entityModelEdition, settings) ?? entityModelEdition;

    private CMwNod? entityModel;
    public CMwNod? EntityModel { get => entityModel; set => entityModel = value; }

    private CPlugVFXFile? vfx;
    public CPlugVFXFile? VFX { get => vfx; set => vfx = value; }

    private CPlugGameSkinAndFolder? materialModifier;
    public CPlugGameSkinAndFolder? MaterialModifier { get => materialModifier; set => materialModifier = value; }

    public partial class Chunk2E002019 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            var itemTypeVersion = GetItemTypeVersion((int)n.itemType);

            if (itemTypeVersion.HasValue && Version < itemTypeVersion)
            {
                rw.NodeRef<CMwNod>(ref n.phyModelCustom, ref n.phyModelCustomFile);
                rw.NodeRef<CMwNod>(ref n.visModelCustom, ref n.visModelCustomFile);
            }

            if (Version >= 3)
            {
                rw.Id(ref n.defaultWeaponName);

                if (Version >= 4)
                {
                    if (n.phyModelCustom is not null)
                    {
                        rw.Int32(-1);
                    }
                    else
                    {
                        rw.NodeRef<CMwNod>(ref n.phyModelCustom);
                    }

                    if (Version >= 5)
                    {
                        if (n.visModelCustom is not null)
                        {
                            rw.Int32(-1);
                        }
                        else
                        {
                            rw.NodeRef<CMwNod>(ref n.visModelCustom);
                        }

                        if (Version >= 6)
                        {
                            rw.ArrayNodeRef<CGameActionModel>(ref n.actions!);

                            if (Version >= 7)
                            {
                                rw.EnumInt32<EDefaultCam>(ref n.defaultCam);

                                if (Version >= 8)
                                {
                                    rw.NodeRef<CMwNod>(ref n.entityModelEdition, ref n.entityModelEditionFile); // CGameCommonItemEntityModelEdition, CGameBlockItem, CPlugVegetTreeModel

                                    if (n.entityModelEdition is null && n.entityModelEditionFile is null)
                                    {
                                        if (n.itemType == EItemType.Block)
                                        {
                                            throw new Exception("EntityModel cannot exist for Block");
                                        }

                                        rw.NodeRef(ref n.entityModel);
                                    }

                                    if (Version >= 13)
                                    {
                                        rw.NodeRef<CPlugVFXFile>(ref n.vfx);

                                        if (Version >= 15)
                                        {
                                            rw.NodeRef<CPlugGameSkinAndFolder>(ref n.materialModifier);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int? GetItemTypeVersion(int itemType) => itemType switch
        {
            1 or 2 => 9,
            4 => 10,
            5 => 9,
            11 => null,
            _ => 12
        };
    }

    public override IHeaderChunk? CreateHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            var chunk = new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
            Chunks.Add(chunk);
            return chunk;
        }
        else if (chunkId == 0x2E002000)
        {
            var chunk = new HeaderChunk2E002000();
            Chunks.Add(chunk);
            return chunk;
        }
        else if (chunkId == 0x2E002001)
        {
            var chunk = new HeaderChunk2E002001();
            Chunks.Add(chunk);
            return chunk;
        }

        return base.CreateHeaderChunk(chunkId);
    }
}
