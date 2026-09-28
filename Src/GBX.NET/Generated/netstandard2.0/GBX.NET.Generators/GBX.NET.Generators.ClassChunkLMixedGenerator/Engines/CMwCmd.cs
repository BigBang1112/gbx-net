namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01005000</remarks>
[Class(0x01005000)]
public abstract partial class CMwCmd : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x01005000;




    /// <summary>
    /// Creates a new instance of <see cref="CMwCmd"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMwCmd() { }




}
