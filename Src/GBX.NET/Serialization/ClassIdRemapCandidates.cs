namespace GBX.NET.Serialization;

// The masks contain only candidate modes and can be copied when a bounded read is speculative.
internal struct ClassIdRemapCandidates()
{
    private ulong all = ulong.MaxValue;
    private ulong header = ulong.MaxValue;
    private ulong root = ulong.MaxValue;

    internal readonly bool HasMatch => all != 0;

    internal void Observe(ClassIdLocation location, uint classId, uint rawClassId)
    {
        var mask = ClassIdRemapping.MatchModes(location,
            classId & ClassIdRemapping.ClassMask, rawClassId & ClassIdRemapping.ClassMask);

        all &= mask;

        if (location is ClassIdLocation.Root or ClassIdLocation.HeaderChunk)
        {
            header &= mask;
        }

        if (location == ClassIdLocation.Root)
        {
            root &= mask;
        }
    }

    internal readonly ClassIdRemapMode SelectMode(GameVersion game)
        => ClassIdRemapping.SelectMode(all != 0 ? all : header != 0 ? header : root, game);
}
