using GBX.NET.Managers;

namespace GBX.NET.Serialization;

internal static partial class ClassIdRemapping
{
    internal const uint ClassMask = 0xFFFFF000;

    internal static uint ToWireId(ClassIdRemapMode mode, ClassIdLocation location, uint id)
    {
        var classId = id & ClassMask;
        var chunkPart = id & ~ClassMask;

        return mode switch
        {
            ClassIdRemapMode.Latest => id,
            ClassIdRemapMode.Id2006 or ClassIdRemapMode.Id2008 when location == ClassIdLocation.MetaRef => id,
            ClassIdRemapMode.Id2008 when classId == 0x2E001000 => 0x0301A000 | chunkPart,
            ClassIdRemapMode.Id2006 or ClassIdRemapMode.Id2008 => ClassManager.Unwrap(id),
            _ when location == ClassIdLocation.BodyChunk && !ClassManager.IsChunkIdRemapped(id) => id,
            _ => GetWireClassId(mode, location, classId) | chunkPart
        };
    }

    internal static ClassIdRemapMode SelectMode(ulong candidates, GameVersion game)
    {
        // Identity wins when the file supplies no evidence for a different mapping.
        if ((candidates & GetModeMask(ClassIdRemapMode.Latest)) != 0)
        {
            return ClassIdRemapMode.Latest;
        }

        var preferred = game switch
        {
            GameVersion.TM10 or GameVersion.TMPU => ClassIdRemapMode.EarlyTrackMania,
            GameVersion.TMSX or GameVersion.TMNESWC => ClassIdRemapMode.TrackManiaSunriseExtreme,
            GameVersion.TMU or GameVersion.VSK5 => ClassIdRemapMode.TrackManiaUnited,
            GameVersion.TMF => ClassIdRemapMode.TrackManiaForever,
            GameVersion.MP1 or GameVersion.MP2 => ClassIdRemapMode.EarlyManiaPlanet,
            _ => ClassIdRemapMode.Latest
        };

        return (candidates & GetModeMask(preferred)) != 0 ? preferred : GetFirstMode(candidates);
    }

    internal static partial ulong MatchModes(ClassIdLocation location, uint classId, uint rawClassId);
    private static partial ulong GetModeMask(ClassIdRemapMode mode);
    private static partial ClassIdRemapMode GetFirstMode(ulong candidates);
    private static partial uint GetWireClassId(ClassIdRemapMode mode, ClassIdLocation location, uint classId);
}
