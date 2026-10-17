namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual2D
{
    public CPlugVisual2D()
    {
        HasVertexNormals = false;
    }

    public partial class Vertex2D
    {
        [Obsolete("Use Position.X instead.")]
        public float U01
        {
            get => Position.X;
            set => Position = Position with { X = value };
        }

        [Obsolete("Use Position.Y instead.")]
        public float U02
        {
            get => Position.Y;
            set => Position = Position with { Y = value };
        }

        [Obsolete("Use Normal.X instead.")]
        public float U03
        {
            get => Normal.X;
            set => Normal = Normal with { X = value };
        }

        [Obsolete("Use Normal.Y instead.")]
        public float U04
        {
            get => Normal.Y;
            set => Normal = Normal with { Y = value };
        }

        [Obsolete("Use Color.R instead.")]
        public float U05
        {
            get => Color.R;
            set => Color = Color with { R = value };
        }

        [Obsolete("Use Color.G instead.")]
        public float U06
        {
            get => Color.G;
            set => Color = Color with { G = value };
        }

        [Obsolete("Use Color.B instead.")]
        public float U07
        {
            get => Color.B;
            set => Color = Color with { B = value };
        }

        [Obsolete("Use Color.A instead.")]
        public float U08
        {
            get => Color.A;
            set => Color = Color with { A = value };
        }
    }
}
