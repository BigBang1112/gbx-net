namespace GBX.NET.Engines.GameData;

public partial class CGameActionModel
{
    public partial class SpriteBlock
    {
        [Obsolete("Use Image instead.")]
        public Plug.CPlugFileImg? U02 { get => Image; set => Image = value; }
    }
}
