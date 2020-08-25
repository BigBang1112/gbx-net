namespace GBX.NET
{
    public struct Int2
    {
        public int X { get; }
        public int Y { get; }

        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static implicit operator Int2((int, int) v)
        {
            return new Int2(v.Item1, v.Item2);
        }

        public static implicit operator (int, int)(Int2 v)
        {
            return (v.X, v.Y);
        }
    }
}