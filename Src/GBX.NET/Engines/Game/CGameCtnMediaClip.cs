namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClip
{
    public override string ToString()
    {
        return $"{nameof(CGameCtnMediaClip)}: {(string.IsNullOrEmpty(Name) ? "(unnamed)" : Name)}";
    }
}
