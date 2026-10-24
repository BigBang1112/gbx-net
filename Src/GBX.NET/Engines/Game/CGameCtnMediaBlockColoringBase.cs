namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockColoringBase
{
    public partial class Key
    {
        [Obsolete("Use Emblem instead.")]
        public short U01 { get => unchecked((short)Emblem); set => Emblem = unchecked((ushort)value); }
    }
}
