namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    public partial class Data
    {
        public class Sample
        {
            private Data? owner;

            public TimeInt32? Timestamp { get; set; }
            public byte[] Data { get; }
            
            public byte? BufferType { get; set; }
            public Vec3 Position { get; set; }
            public Quat Rotation { get; set; }
            public float Speed { get; set; }
            public Vec3 Velocity { get; set; }

            public Sample(byte[] data)
            {
                Data = data;
            }

            internal void AssignTo(Data? ghostData)
            {
                owner = ghostData;

                if (owner == null || owner.samplePeriod.TotalMilliseconds <= 0)
                {
                    Timestamp = null;
                    return;
                }

                UpdateTimestamp();
            }

            internal void UpdateTimestamp()
            {
                if (owner is not null)
                    Timestamp = TimeInt32.FromMilliseconds(owner.samplePeriod.TotalMilliseconds * owner.Samples.IndexOf(this));
            }
        }
    }
}
