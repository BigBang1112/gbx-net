namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Effect.
/// </summary>
/// <remarks>ID: 0x0307E000</remarks>
[Class(0x0307E000)]
public abstract partial class CGameCtnMediaBlockFx : CGameCtnMediaBlock, IClass
{
    [Hexadecimal] public static new uint Id => 0x0307E000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFx"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFx() { }




}
