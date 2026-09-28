namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05010000</remarks>
[Class(0x05010000)]
public abstract partial class CFunc : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x05010000;




    /// <summary>
    /// Creates a new instance of <see cref="CFunc"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFunc() { }




}
