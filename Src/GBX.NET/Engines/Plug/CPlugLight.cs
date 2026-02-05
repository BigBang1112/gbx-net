namespace GBX.NET.Engines.Plug;

public partial class CPlugLight
{
    private bool GetFlag(EFlags flag) => (Flags & flag) != 0;
    private void SetFlag(EFlags flag, bool value) => Flags = value ? Flags | flag : Flags & ~flag;

    public bool NightOnly
    {
        get => GetFlag(EFlags.NightOnly);
        set => SetFlag(EFlags.NightOnly, value);
    }

    public bool ReflectByGround
    {
        get => GetFlag(EFlags.ReflectByGround);
        set => SetFlag(EFlags.ReflectByGround, value);
    }

    public bool DuplicateGxLight
    {
        get => GetFlag(EFlags.DuplicateGxLight);
        set => SetFlag(EFlags.DuplicateGxLight, value);
    }

    public bool SceneLightOnlyWhenTreeVisible
    {
        get => GetFlag(EFlags.SceneLightOnlyWhenTreeVisible);
        set => SetFlag(EFlags.SceneLightOnlyWhenTreeVisible, value);
    }
}
