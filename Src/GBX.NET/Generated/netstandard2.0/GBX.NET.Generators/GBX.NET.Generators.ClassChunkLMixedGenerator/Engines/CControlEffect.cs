namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x07005000</remarks>
[Class(0x07005000)]
public abstract partial class CControlEffect : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x07005000;




    /// <summary>
    /// Creates a new instance of <see cref="CControlEffect"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlEffect() { }




}
