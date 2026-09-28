namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09040000</remarks>
[Class(0x09040000)]
public abstract partial class CPlugFileGPU : CPlugFileText, IClass
{
    [Hexadecimal] public static new uint Id => 0x09040000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugFileGPU"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFileGPU() { }




}
