namespace GBX.NET.Engines.Game;

public partial class CGameSaveLaunchedCheckpoints
{
    public partial class Chunk03262000
    {
        public override void ReadWrite(CGameSaveLaunchedCheckpoints n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 8)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            if (Version >= 3)
            {
                rw.Int32(ref n.recordVersion);
            }
            else
            {
                n.recordVersion = 30;
            }

            rw.ArrayReadableWritable(ref n.checkpoints, version: Version);
            if (Version >= 1)
            {
                rw.ArrayReadableWritable(ref n.snapshots, version: n.recordVersion);
                foreach (var checkpoint in n.checkpoints!)
                {
                    checkpoint.SnapshotCount = rw.Int32(checkpoint.SnapshotCount);
                }
            }

            if (Version >= 6)
            {
                foreach (var checkpoint in n.checkpoints!)
                {
                    if (Version == 6)
                    {
                        checkpoint.LegacyModelIndex = rw.Int32(checkpoint.LegacyModelIndex);
                        checkpoint.LegacyModelFlags = rw.Int32(checkpoint.LegacyModelFlags);
                    }

                    checkpoint.ModelIndex = rw.Int32(checkpoint.ModelIndex);
                    checkpoint.Model = rw.Ident(checkpoint.Model);
                }
            }
        }
    }

    public partial class Checkpoint
    {
        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            // The containing chunk writes the other fields after the snapshot array.
            rw.ReadableWritable(ref state, v);
        }
    }
}
