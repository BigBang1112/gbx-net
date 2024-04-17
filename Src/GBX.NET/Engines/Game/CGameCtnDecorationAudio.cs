namespace GBX.NET.Engines.Game;

public partial class CGameCtnDecorationAudio
{
    [AppliedWithChunk<Chunk03039000>]
    public Dictionary<string, CPlugSound?>? Sounds { get; set; }

    [AppliedWithChunk<Chunk03039000>]
    public Dictionary<string, CPlugSound?>? Musics { get; set; }

    public partial class Chunk03039000
    {
        public override void Read(CGameCtnDecorationAudio n, GbxReader r)
        {
            n.Sounds = [];
            for (var i = 0; i < r.ReadInt32(); i++)
            {
                n.Sounds.Add(r.ReadId(), r.ReadNodeRef<CPlugSound>());
            }

            n.Musics = [];
            for (var i = 0; i < r.ReadInt32(); i++)
            {
                n.Musics.Add(r.ReadId(), r.ReadNodeRef<CPlugSound>());
            }
        }

        public override void Write(CGameCtnDecorationAudio n, GbxWriter w)
        {
            w.Write(n.Sounds?.Count ?? 0);
            if (n.Sounds is not null)
            {
                foreach (var pair in n.Sounds)
                {
                    w.Write(pair.Key);
                    w.WriteNodeRef(pair.Value);
                }
            }

            w.Write(n.Musics?.Count ?? 0);
            if (n.Musics is not null)
            {
                foreach (var pair in n.Musics)
                {
                    w.Write(pair.Key);
                    w.WriteNodeRef(pair.Value);
                }
            }
        }
    }
}
