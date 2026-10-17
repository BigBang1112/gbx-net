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
                var snapshotIndex = 0UL;
                foreach (var checkpoint in n.checkpoints!)
                {
                    checkpoint.SnapshotCount = rw.UInt32(checkpoint.SnapshotCount);
                    snapshotIndex += checkpoint.SnapshotCount;
                    if (rw.Reader is not null && snapshotIndex > (ulong)(n.snapshots?.Length ?? 0))
                    {
                        throw new InvalidDataException("Checkpoint snapshot counts exceed the snapshot array length.");
                    }
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

    public partial class CheckpointState
    {
        [Obsolete("Use CheckpointIndex instead.")]
        public int Time { get => unchecked((int)CheckpointIndex); set => CheckpointIndex = unchecked((uint)value); }

        [Obsolete("Use CaptureTime instead.")]
        public int U04 { get => unchecked((int)CaptureTime); set => CaptureTime = unchecked((uint)value); }
    }

    public partial class Snapshot
    {
        [Obsolete("Use PackedVehicleState instead.")]
        public int U16 { get => unchecked((int)PackedVehicleState); set => PackedVehicleState = unchecked((uint)value); }
    }
}
