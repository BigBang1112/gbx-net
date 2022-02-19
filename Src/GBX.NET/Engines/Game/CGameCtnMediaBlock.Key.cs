namespace GBX.NET.Engines.Game;

public abstract partial class CGameCtnMediaBlock
{
    public abstract class Key : IReadableWritable, ICloneable
    {
        private TimeSingle time;

        public TimeSingle Time { get => time; set => time = value; }

        protected Key()
        {
            
        }

        protected Key(GameBoxReader r)
        {
            time = r.ReadTimeSingle();
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
        /// Reads or writes the keyframe structure. 
        /// </summary>
        /// <remarks>Base includes time of the keyframe (in seconds).</remarks>
        /// <param name="rw">Reader/Writer.</param>
        /// <param name="version">Version to determine how to read the key.</param>
        public virtual void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.TimeSingle(ref time);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {{ Time: {time} }}";
        }
    }
}
