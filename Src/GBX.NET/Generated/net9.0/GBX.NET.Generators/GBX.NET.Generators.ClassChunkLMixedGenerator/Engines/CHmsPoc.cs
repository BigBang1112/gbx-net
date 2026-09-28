namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06007000</remarks>
[Class(0x06007000)]
public partial class CHmsPoc : CHmsZoneElem, IClass
{
    [Hexadecimal] public static new uint Id => 0x06007000;




    /// <summary>
    /// Creates a new instance of <see cref="CHmsPoc"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsPoc() { }




}
