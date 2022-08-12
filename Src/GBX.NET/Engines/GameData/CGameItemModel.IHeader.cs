namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
{
    public new interface IHeader : INodeHeader<CGameItemModel>, CGameCtnCollector.IHeader
    {
        public EItemType ItemType { get; set; }
    }
}
