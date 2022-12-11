namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x0600D000</remarks>
[Node(0x0600D000)]
public class CHmsSoundSource : CMwNod
{
    private CMwNod? audioSound;
    private GameBoxRefTable.File? audioSoundFile;

    [NodeMember]
    [AppliedWithChunk<Chunk0600D000>]
    [AppliedWithChunk<Chunk0600D002>]
    [AppliedWithChunk<Chunk0600D005>]
    public CMwNod? AudioSound { get => audioSound; set => audioSound = value; }
    public GameBoxRefTable.File? AudioSoundFile { get => audioSoundFile; set => audioSoundFile = value; }

    internal CHmsSoundSource()
    {

    }

    #region 0x000 chunk (AudioSound)

    /// <summary>
    /// CHmsSoundSource 0x000 chunk (AudioSound)
    /// </summary>
    [Chunk(0x0600D000, "AudioSound")]
    public class Chunk0600D000 : Chunk<CHmsSoundSource>
    {
        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.audioSound, ref n.audioSoundFile); // sound
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CHmsSoundSource 0x002 chunk
    /// </summary>
    [Chunk(0x0600D002)]
    public class Chunk0600D002 : Chunk<CHmsSoundSource>
    {
        public int U02;
        public bool U03;

        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.audioSound, ref n.audioSoundFile); // sound
            rw.Int32(ref U02);
            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CHmsSoundSource 0x003 chunk
    /// </summary>
    [Chunk(0x0600D003)]
    public class Chunk0600D003 : Chunk<CHmsSoundSource>
    {
        public Vec3 U01;

        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref U01);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CHmsSoundSource 0x004 chunk
    /// </summary>
    [Chunk(0x0600D004)]
    public class Chunk0600D004 : Chunk<CHmsSoundSource>
    {
        public Vec3 U01;
        
        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref U01);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CHmsSoundSource 0x005 chunk
    /// </summary>
    [Chunk(0x0600D005)]
    public class Chunk0600D005 : Chunk<CHmsSoundSource>
    {
        public float U02;
        public bool U03;

        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.audioSound, ref n.audioSoundFile); // sound
            rw.Single(ref U02);
            rw.Boolean(ref U03);
        }
    }

    #endregion
}