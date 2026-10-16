namespace GBX.NET.Engines.Game;

public partial class CGameCtnDecorationAudio
{
    /// <remarks>Applied with: 0x000 body chunk</remarks>
    [AppliedWithChunk<Chunk03039000>]
    public Dictionary<string, NodeRef<CPlugSound>>? Sounds { get; set; }

    /// <remarks>Applied with: 0x000 body chunk</remarks>
    [AppliedWithChunk<Chunk03039000>]
    public Dictionary<string, NodeRef<CPlugSound>>? Musics { get; set; }

    public partial class Chunk03039000
    {
        public override void Read(CGameCtnDecorationAudio n, GbxReader r)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(n, rw);
        }

        public override void Write(CGameCtnDecorationAudio n, GbxWriter w)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(n, rw);
        }

        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            n.Sounds = ReadWriteReferences(n.Sounds, rw);
            n.Musics = ReadWriteReferences(n.Musics, rw);
        }

        private static Dictionary<string, NodeRef<CPlugSound>>? ReadWriteReferences(Dictionary<string, NodeRef<CPlugSound>>? references, GbxReaderWriter rw)
        {
            var count = rw.Int32(references?.Count ?? 0);
            if (rw.Reader is { } reader)
            {
                references = new(count);
                for (var i = 0; i < count; i++)
                {
                    var name = reader.ReadIdAsString();
                    references.Add(name, reader.ReadNodeRef<CPlugSound>(null));
                }
            }
            if (rw.Writer is { } writer && references is not null)
            {
                foreach (var pair in references)
                {
                    writer.WriteIdAsString(pair.Key);
                    writer.WriteNodeRef(pair.Value);
                }
            }
            return references;
        }
    }
}
