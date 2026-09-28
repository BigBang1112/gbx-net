namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A018000</remarks>
[Class(0x0A018000)]
public partial class CSceneVehicleVis : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A018000;







    public enum ReactorBoostType
    {
        None,
        Up,
        Down,
        UpAndDown,
    }

    public enum ReactorBoostLvl
    {
        None,
        Lvl1,
        Lvl2,
    }

}
