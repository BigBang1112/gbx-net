namespace GBX.NET.Engines.GameData;

[Node(0x2E00E000)]
public class CGameSpawnModel : CMwNod
{
    private Iso4 loc;
    private bool underground;
    private float torqueX;
    private int torqueDuration;
    private Vec3 defaultGravitySpawn;

    internal CGameSpawnModel()
    {

    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public Iso4 Loc { get => loc; set => loc = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public bool Underground { get => underground; set => underground = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public float TorqueX { get => torqueX; set => torqueX = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public int TorqueDuration { get => torqueDuration; set => torqueDuration = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public Vec3 DefaultGravitySpawn { get => defaultGravitySpawn; set => defaultGravitySpawn = value; }

    #region 0x000 chunk

    /// <summary>
    /// CGameSpawnModel 0x000 chunk
    /// </summary>
    [Chunk(0x090BB000)]
    public class Chunk090BB000 : Chunk<CGameSpawnModel>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameSpawnModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Iso4(ref n.loc);
            rw.Boolean(ref n.underground);

            if (version >= 1)
            {
                rw.Single(ref n.torqueX);

                if (version >= 2)
                {
                    rw.Int32(ref n.torqueDuration);

                    if (version >= 3)
                    {
                        rw.Vec3(ref n.defaultGravitySpawn);
                    }
                }
            }
        }
    }

    #endregion
}
