namespace GBX.NET.Engines.Script;

[Node(0x11002000)]
public class CScriptTraitsMetadata : CMwNod
{
	protected CScriptTraitsMetadata()
	{
        
	}

    #region 0x000 chunk

    /// <summary>
    /// CScriptTraitsMetadata 0x000 chunk
    /// </summary>
    [Chunk(0x11002000)]
    public class Chunk11002000 : Chunk<CScriptTraitsMetadata>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CScriptTraitsMetadata n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version < 2)
            {
                return; // temporary
            }

            // CScriptTraitsGenericContainer::Archive
        }
    }

    #endregion
}
