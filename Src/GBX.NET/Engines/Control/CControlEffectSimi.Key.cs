namespace GBX.NET.Engines.Control;

public partial class CControlEffectSimi
{
    /// <summary>
    /// Keyframe of <see cref="CControlEffectSimi"/>.
    /// </summary>
    /// <remarks>Internally called <c>SKeyVal</c>.</remarks>
    public class Key : CGameCtnMediaBlock.Key
    {
        private Vec2 position;
        private float rotation;
        private Vec2 scale;
        private float opacity;
        private float depth;
        private float isContinuousEffect;

        public float U01;
        public float U02;
        public float U03;

        public Vec2 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Rotation in radians
        /// </summary>
        public float Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        public Vec2 Scale
        {
            get => scale;
            set => scale = value;
        }

        public float Opacity
        {
            get => opacity;
            set => opacity = value;
        }

        public float Depth
        {
            get => depth;
            set => depth = value;
        }

        public float IsContinuousEffect
        {
            get => isContinuousEffect;
            set => isContinuousEffect = value;
        }

        public Key()
        {
            scale = new(1, 1);
            opacity = 1;
            depth = 0.5f;
        }

        protected internal override void ReadWrite<TChunk>(GameBoxReaderWriter rw)
        {
            base.ReadWrite<TChunk>(rw);

            rw.Vec2(ref position);
            rw.Single(ref rotation);
            rw.Vec2(ref scale);
            rw.Single(ref opacity);
            rw.Single(ref depth);

            if (typeof(TChunk) != typeof(Chunk07010002))
            {
                rw.Single(ref U01);
                rw.Single(ref isContinuousEffect);
                rw.Single(ref U02);
                rw.Single(ref U03);
            }
        }
    }
}
