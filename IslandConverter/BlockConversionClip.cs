namespace IslandConverter
{
    public class BlockConversionClip
    {
        public string ItemModel { get; set; }
        public int[] OffsetCoord { get; set; }
        public float[] OffsetPosition { get; set; }
        public int OffsetDirection { get; set; }
        public float[] CenterFromCoord { get; set; }
    }
}
