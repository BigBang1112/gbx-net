namespace GBX.NET.Interfaces.Game;

public interface IGameCtnBlockTMSX : IGameCtnBlock
{
    byte Variant { get; set; }
    byte SubVariant { get; set; }
    bool IsClip { get; set; }
}
