namespace GBX.NET.Interfaces.Game;

public partial interface IGameCtnBlockTM10 : IGameCtnBlock
{
    Ident BlockModel { get; set; }
    new byte Variant { get; set; }
}
