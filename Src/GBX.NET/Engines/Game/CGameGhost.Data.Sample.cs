namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    public partial class Data
    {
        public class Sample : NET.Sample
        {
            private Data? owner;

            public Sample(byte[] data) : base(data)
            {
                
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
                    Timestamp = TimeSpan.FromMilliseconds(owner.samplePeriod.TotalMilliseconds * owner.Samples.IndexOf(this));
            }
        }
    }
}
