namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09025000</remarks>
[Class(0x09025000)]
public abstract partial class CPlugFileImg : CPlugFile, IClass
{
    [Hexadecimal] public static new uint Id => 0x09025000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugFileImg"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFileImg() { }




}
