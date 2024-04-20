namespace GBX.NET.Engines.Game;

public partial class CGameCtnDecorationAudio
{
    [AppliedWithChunk<Chunk03039000>]
    public Dictionary<string, External<CPlugSound>>? Sounds { get; set; }

    [AppliedWithChunk<Chunk03039000>]
    public Dictionary<string, External<CPlugSound>>? Musics { get; set; }

    public partial class Chunk03039000
    {
        public override void Read(CGameCtnDecorationAudio n, GbxReader r)
        {
            var numSounds = r.ReadInt32();
            n.Sounds = new(numSounds);
            for (var i = 0; i < numSounds; i++)
            {
                var name = r.ReadIdAsString();
                var node = r.ReadNodeRef<CPlugSound>(out var file);
                n.Sounds.Add(name, new(node, file));
            }

            var numMusics = r.ReadInt32();
            n.Musics = new(numMusics);
            for (var i = 0; i < numMusics; i++)
            {
                var name = r.ReadIdAsString();
                var node = r.ReadNodeRef<CPlugSound>(out var file);
                n.Musics.Add(name, new(node, file));
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
                    w.WriteNodeRef(pair.Value.Node, pair.Value.File);
                }
            }

            w.Write(n.Musics?.Count ?? 0);
            if (n.Musics is not null)
            {
                foreach (var pair in n.Musics)
                {
                    w.Write(pair.Key);
                    w.WriteNodeRef(pair.Value.Node, pair.Value.File);
                }
            }
        }
    }
}
