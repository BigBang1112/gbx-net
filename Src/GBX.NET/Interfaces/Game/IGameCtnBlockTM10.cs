namespace GBX.NET.Interfaces.Game;

public interface IGameCtnBlockTM10 : IGameCtnBlock
{
    Ident BlockModel { get; set; }
    byte Variant { get; set; }
}
