namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterialCustom
{
    public partial class GpuFx
    {
        public string? Name { get; set; }
        public bool U02 { get; set; }
        public float[][] Values { get; set; } = [[0]];

        /// <summary>Number of components per register, retained when Values is empty.</summary>
        public int ComponentCount { get; set; } = 1;

        public int U04 { get; set; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            Name = rw.Id(Name);
            var componentCount = v is 1 or 2 ? 4 : Values.Length > 0 ? Values[0].Length : ComponentCount;
            var registerCount = v == 0 ? Values.Length : 1;

            if (v == 1) U04 = rw.Int32(U04);
            if (v is 0 or 3) rw.Int32(ref componentCount);
            if (v == 0) rw.Int32(ref registerCount);
            if (v is 0 or 3) U02 = rw.Boolean(U02);

            if (rw.Reader is not null)
            {
                Values = new float[registerCount][];
            }
            ComponentCount = componentCount;

            for (var i = 0; i < registerCount; i++)
            {
                Values[i] = rw.Array(Values[i], componentCount)!;
            }
        }
    }
}
