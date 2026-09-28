namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090D7000</remarks>
[Class(0x090D7000)]
public partial class CPlugDynaPointModel : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x090D7000;




    private int u01;
    public int U01 { get => u01; set => u01 = value; }

    private int u02;
    public int U02 { get => u02; set => u02 = value; }

    private int u03;
    public int U03 { get => u03; set => u03 = value; }

    private int u04;
    public int U04 { get => u04; set => u04 = value; }

    private int u05;
    public int U05 { get => u05; set => u05 = value; }

    private int u06;
    public int U06 { get => u06; set => u06 = value; }

    private int u07;
    public int U07 { get => u07; set => u07 = value; }

    private int u08;
    public int U08 { get => u08; set => u08 = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugDynaPointModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugDynaPointModel() { }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.Int32(ref u01);
        rw.Int32(ref u02);
        rw.Int32(ref u03);
        rw.Int32(ref u04);
        rw.Int32(ref u05);
        rw.Int32(ref u06);
        rw.Int32(ref u07);
        rw.Int32(ref u08);
    }




}
