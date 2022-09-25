namespace GBX.NET.Engines.Input;

/// <remarks>ID: 0x1300D000</remarks>
[Node(0x1300D000)]
[NodeExtension("InputsReplay")]
[WritingNotSupported]
public class CInputReplay : CMwNod
{
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk1300D000))]
    public Event[] Events { get; set; }

    internal CInputReplay()
	{
        Events = Array.Empty<Event>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CInputReplay 0x000 chunk
    /// </summary>
    [Chunk(0x1300D000)]
    public class Chunk1300D000 : Chunk<CInputReplay>, IVersionable
    {        
        public int U01;
        public int U02;

        public int Version { get; set; } = 1;

        public override void Read(CInputReplay n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            // CInputEventsStore::Archive
            U01 = r.ReadInt32();
            var inputTypes = r.ReadArray<string>(r => r.ReadId(cannotBeCollection: true));
            var count = r.ReadInt32();
            U02 = r.ReadInt32();
            n.Events = r.ReadArray(count, r => new Event(
                Index: r.ReadInt32(),
                Action: inputTypes[r.ReadByte()],
                Value: r.ReadInt32())
            );
        }
    }

    #endregion

    public record Event(int Index, string Action, int Value);
}
