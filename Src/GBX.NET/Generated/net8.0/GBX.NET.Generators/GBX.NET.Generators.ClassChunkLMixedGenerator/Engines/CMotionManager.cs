namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x0804B000</remarks>
[Class(0x0804B000)]
public abstract partial class CMotionManager : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0804B000;




    /// <summary>
    /// Creates a new instance of <see cref="CMotionManager"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionManager() { }




}
