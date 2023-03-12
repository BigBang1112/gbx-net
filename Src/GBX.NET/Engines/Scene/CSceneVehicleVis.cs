namespace GBX.NET.Engines.Scene;

[Node(0x0A018000)]
public abstract partial class CSceneVehicleVis
{
    public enum ReactorBoostLvl
    {
        None,
        Lvl1,
        Lvl2
    }
    
    public enum ReactorBoostType
    {
        None,
        Up,
        Down,
        UpAndDown
    }
}
