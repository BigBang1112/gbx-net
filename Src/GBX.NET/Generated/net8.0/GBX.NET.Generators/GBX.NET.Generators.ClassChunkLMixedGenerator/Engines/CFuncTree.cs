namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0501C000</remarks>
[Class(0x0501C000)]
public abstract partial class CFuncTree : CFuncPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0501C000;




    /// <summary>
    /// Creates a new instance of <see cref="CFuncTree"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncTree() { }




}
