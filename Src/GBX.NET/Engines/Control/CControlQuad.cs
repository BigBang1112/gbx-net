namespace GBX.NET.Engines.Control;

public partial class CControlQuad
{
    // An icon id can be a name or a number, and both must be written back as stored.
    private GBX.NET.Id? iconId;
    public string? IconId { get => GBX.NET.Id.ToStoredString(iconId); set => iconId = GBX.NET.Id.FromStoredString(value, iconId); }
}
