namespace IslandConverter
{
    public class BlockConversion
    {
        public string[] ItemModel { get; set; }
        public BlockConversion[] ItemModels { get; set; }
        public BlockConversion Air { get; set; }
        public BlockConversion Ground { get; set; }
        public BlockConversion[] Directions { get; set; }
        public float[] OffsetAbsolutePosition { get; set; }
        public float[] OffsetPitchYawRoll { get; set; }
        public float[] OffsetPivot { get; set; }
        public int? OffsetDirection { get; set; }
        public int? OffsetPivotDirection { get; set; }
        public bool? InverseDirection { get; set; }
        public int? OffsetYFromTerrain { get; set; }
        public int[][] Units { get; set; } = new int[][] { new int[] { 0, 0, 0 } };
        public float[] SkinPositionOffset { get; set; }
        public int SkinDirectionOffset { get; set; }
        public string SkinSignSet { get; set; }
        public float[] CenterFromCoord { get; set; }
        public string[][] Clip { get; set; }
        public bool KeepWater { get; set; }
    }
}
