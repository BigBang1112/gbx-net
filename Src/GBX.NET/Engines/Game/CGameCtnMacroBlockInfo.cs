namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    [AppliedWithChunk<Chunk0310D00B>]
    public CScriptTraitsMetadata? ScriptMetadata { get; set; }

    [AppliedWithChunk<Chunk0310D011>]
    public CGameCtnMediaClipGroup? ClipGroupInGame { get; set; }

    [AppliedWithChunk<Chunk0310D011>]
    public CGameCtnMediaClipGroup? ClipGroupEndRace { get; set; }

    public partial class Chunk0310D00B
    {
        public int U01;

        public override void Read(CGameCtnMacroBlockInfo n, GbxReader r)
        {
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.ScriptMetadata = r.ReadNode<CScriptTraitsMetadata>();
        }

        public override void Write(CGameCtnMacroBlockInfo n, GbxWriter w)
        {
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            wBuffer.WriteNode(n.ScriptMetadata);

            w.Write((int)ms.Length);
            ms.WriteTo(w.BaseStream);
        }
    }

    public partial class Chunk0310D011 : IVersionable
    {
        public int Version { get; set; }

        public int U01;

        public Int3 U02;
        public Int3 U03;

        public override void Read(CGameCtnMacroBlockInfo n, GbxReader r)
        {
            Version = r.ReadInt32();

            U01 = r.ReadInt32();
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            // SMediaTrackSpawns
            U02 = r.ReadInt3();
            U03 = r.ReadInt3();

            n.ClipGroupInGame = r.ReadNodeRef<CGameCtnMediaClipGroup>();
            n.ClipGroupEndRace = r.ReadNodeRef<CGameCtnMediaClipGroup>();
            //
        }

        public override void Write(CGameCtnMacroBlockInfo n, GbxWriter w)
        {
            w.Write(Version);

            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            wBuffer.Write(U02);
            wBuffer.Write(U03);

            wBuffer.WriteNodeRef(n.ClipGroupInGame);
            wBuffer.WriteNodeRef(n.ClipGroupEndRace);

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }
}
