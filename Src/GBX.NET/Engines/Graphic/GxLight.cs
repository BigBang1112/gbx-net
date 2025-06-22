namespace GBX.NET.Engines.Graphic;

public partial class GxLight
{
    private bool GetFlag(EFlags flag) => (Flags & flag) != 0;
    private void SetFlag(EFlags flag, bool value) => Flags = value ? Flags | flag : Flags & ~flag;

    public bool DoLighting
    {
        get => GetFlag(EFlags.DoLighting);
        set => SetFlag(EFlags.DoLighting, value);
    }

    public bool LightMapOnly
    {
        get => GetFlag(EFlags.LightMapOnly);
        set => SetFlag(EFlags.LightMapOnly, value);
    }

    public bool IsInverted => Intensity < 0;

    public bool IsShadowGen
    {
        get => GetFlag(EFlags.ShadowGen);
        set => SetFlag(EFlags.ShadowGen, value);
    }

    public bool DoSpecular
    {
        get => GetFlag(EFlags.Specular);
        set => SetFlag(EFlags.Specular, value);
    }

    public bool HasLensFlare
    {
        get => GetFlag(EFlags.LensFlare);
        set => SetFlag(EFlags.LensFlare, value);
    }

    public bool HasSprite
    {
        get => GetFlag(EFlags.Sprite);
        set => SetFlag(EFlags.Sprite, value);
    }

    public bool EnableGroup0
    {
        get => GetFlag(EFlags.EnableGroup0);
        set => SetFlag(EFlags.EnableGroup0, value);
    }

    public bool EnableGroup1
    {
        get => GetFlag(EFlags.EnableGroup1);
        set => SetFlag(EFlags.EnableGroup1, value);
    }

    public bool EnableGroup2
    {
        get => GetFlag(EFlags.EnableGroup2);
        set => SetFlag(EFlags.EnableGroup2, value);
    }

    public bool EnableGroup3
    {
        get => GetFlag(EFlags.EnableGroup3);
        set => SetFlag(EFlags.EnableGroup3, value);
    }

}
