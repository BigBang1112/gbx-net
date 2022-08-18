namespace GBX.NET.Engines.Game;

public partial class CGameCtnCollection
{
    public interface IHeader : INodeHeader<CGameCtnCollection>
    {
        public string? Collection { get; set; }
        public bool NeedUnlock { get; set; }
        public string? IconEnv { get; set; }
        public string? IconCollection { get; set; }
        public int SortIndex { get; set; }
        public string? DefaultZone { get; set; }
        public Ident? Vehicle { get; set; }
        public Vec2? MapCoordElem { get; set; }
        public Vec2? MapCoordIcon { get; set; }
        public string? LoadScreen { get; set; }
        public Vec2? MapCoordDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsEditable { get; set; }
    }
}
