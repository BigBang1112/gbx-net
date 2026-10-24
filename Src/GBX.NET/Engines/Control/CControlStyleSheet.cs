namespace GBX.NET.Engines.Control;

public partial class CControlStyleSheet
{
    public CControlStyleSheet()
    {
        colors = Enumerable.Repeat(new Vec3(1, 1, 1), 8).ToArray();
        colorsAlpha = Enumerable.Repeat(1f, 8).ToArray();
    }
}
