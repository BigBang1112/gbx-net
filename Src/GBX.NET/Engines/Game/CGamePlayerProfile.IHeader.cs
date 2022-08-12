namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfile
{
    public interface IHeader : INodeHeader<CGamePlayerProfile>
    {
        public string? OnlineLogin { get; set; }
        public string? OnlineSupportKey { get; set; }
    }
}
