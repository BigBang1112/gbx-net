namespace GBX.NET;

public enum ClassIdRemapMode
{
    /// <summary>
    /// Latest class IDs.
    /// </summary>
    Latest,
    /// <summary>
    /// Same as <see cref="Latest"/> except for the <see cref="CGameCtnCollector"/> class ID.
    /// </summary>
    Id2008,
    /// <summary>
    /// Class IDs of Nadeo games from 2006 and older.
    /// </summary>
    Id2006
}
