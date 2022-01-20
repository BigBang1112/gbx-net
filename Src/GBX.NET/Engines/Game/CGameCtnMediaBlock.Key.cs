namespace GBX.NET.Engines.Game;

public abstract partial class CGameCtnMediaBlock
{
    public abstract class Key : ICloneable
    {
        private TimeSpan time;

        public TimeSpan Time { get => time; set => time = value; }

        protected Key()
        {

        }

        protected Key(GameBoxReader r)
        {
            Time = r.ReadSingle_s();
        }

        public Key Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Reads or writes the time (in seconds) of the keyframe.
        /// </summary>
        /// <typeparam name="TChunk">Type of the chunk.</typeparam>
        /// <param name="rw">Reader/Writer.</param>
        protected internal virtual void ReadWrite<TChunk>(GameBoxReaderWriter rw) where TChunk : Chunk
        {
            rw.Single_s(ref time);
        }
    }
}
