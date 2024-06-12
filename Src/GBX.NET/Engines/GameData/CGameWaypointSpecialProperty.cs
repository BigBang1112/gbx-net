namespace GBX.NET.Engines.GameData;

public partial class CGameWaypointSpecialProperty
{
    [AppliedWithChunk<Chunk2E009001>]
    public CScriptTraitsMetadata? ScriptMetadata { get; set; }

    public partial class Chunk2E009001 : IVersionable
    {
        public int Version { get; set; }

        public int U01;

        public override void Read(CGameWaypointSpecialProperty n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (!r.ReadBoolean())
            {
                return;
            }

            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.ScriptMetadata = r.ReadNode<CScriptTraitsMetadata>();
        }

        public override void Write(CGameWaypointSpecialProperty n, GbxWriter w)
        {
            w.Write(Version);

            w.Write(n.ScriptMetadata is not null);

            if (n.ScriptMetadata is null)
            {
                return;
            }

            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            wBuffer.WriteNode(n.ScriptMetadata);

            w.Write((int)ms.Length);
            ms.WriteTo(w.BaseStream);
        }
    }
}
