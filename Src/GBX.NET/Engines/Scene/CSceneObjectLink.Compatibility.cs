namespace GBX.NET.Engines.Scene;

public partial class CSceneObjectLink
{
    [Obsolete("Use Mobil.Name instead.")]
    public string? Name
    {
        get => Mobil?.Name;
        set
        {
            if (Mobil is not null)
            {
                Mobil.Name = value;
            }
        }
    }
}
