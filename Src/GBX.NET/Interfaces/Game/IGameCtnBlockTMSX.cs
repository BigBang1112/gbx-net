namespace GBX.NET.Interfaces.Game;

public partial interface IGameCtnBlockTMSX : IGameCtnBlock
{
    new byte Variant { get; set; }
    byte SubVariant { get; set; }
    bool IsClip { get; set; }
}
