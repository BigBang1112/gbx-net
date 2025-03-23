namespace GBX.NET.Engines.Game;

public partial class CGameCtnCollection
{
    private CPlugBitmap? iconFid;
    [AppliedWithChunk<Chunk0303300D>]
    public CPlugBitmap? IconFid { get => iconFidFile?.GetNode(ref iconFid) ?? iconFid; set => iconFid = value; }
    private Components.GbxRefTableFile? iconFidFile;
    public Components.GbxRefTableFile? IconFidFile { get => iconFidFile; set => iconFidFile = value; }
    public CPlugBitmap? GetIconFid(GbxReadSettings settings = default, bool exceptions = false) => iconFidFile?.GetNode(ref iconFid, settings, exceptions) ?? iconFid;

    private CPlugBitmap? iconSmallFid;
    [AppliedWithChunk<Chunk0303300D>]
    public CPlugBitmap? IconSmallFid { get => iconSmallFidFile?.GetNode(ref iconSmallFid) ?? iconSmallFid; set => iconSmallFid = value; }
    private Components.GbxRefTableFile? iconSmallFidFile;
    public Components.GbxRefTableFile? IconSmallFidFile { get => iconSmallFidFile; set => iconSmallFidFile = value; }
    public CPlugBitmap? GetIconSmallFid(GbxReadSettings settings = default, bool exceptions = false) => iconSmallFidFile?.GetNode(ref iconSmallFid, settings, exceptions) ?? iconSmallFid;

    public partial class Chunk0303300D
    {
        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            if (rw.Boolean(n.iconFid is not null))
            {
                rw.NodeRef<CPlugBitmap>(ref n.iconFid, ref n.iconFidFile);
            }

            if (rw.Boolean(n.iconSmallFid is not null))
            {
                rw.NodeRef<CPlugBitmap>(ref n.iconSmallFid, ref n.iconSmallFidFile);
            }
        }
    }
}
