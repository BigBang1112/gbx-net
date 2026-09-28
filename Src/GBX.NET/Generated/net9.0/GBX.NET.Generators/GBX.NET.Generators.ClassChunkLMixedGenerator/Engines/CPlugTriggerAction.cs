namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09104000</remarks>
[Class(0x09104000)]
public partial class CPlugTriggerAction : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09104000;




    private int u01;
    public int U01 { get => u01; set => u01 = value; }

    private float u02;
    public float U02 { get => u02; set => u02 = value; }

    private float u03;
    public float U03 { get => u03; set => u03 = value; }

    private float u04;
    public float U04 { get => u04; set => u04 = value; }

    private float u05;
    public float U05 { get => u05; set => u05 = value; }

    private int u06;
    public int U06 { get => u06; set => u06 = value; }

    private int u07;
    public int U07 { get => u07; set => u07 = value; }

    private int u08;
    public int U08 { get => u08; set => u08 = value; }

    private int u09;
    public int U09 { get => u09; set => u09 = value; }

    private float u10;
    public float U10 { get => u10; set => u10 = value; }

    private float u11;
    public float U11 { get => u11; set => u11 = value; }

    private float u12;
    public float U12 { get => u12; set => u12 = value; }

    private float u13;
    public float U13 { get => u13; set => u13 = value; }

    private float u14;
    public float U14 { get => u14; set => u14 = value; }

    private float u15;
    public float U15 { get => u15; set => u15 = value; }

    private string? u16;
    public string? U16 { get => u16; set => u16 = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugTriggerAction"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugTriggerAction() { }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (v >= 5)
        {
            rw.Int32(ref u01);
        }
        if (v >= 4)
        {
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
        }
        if (v >= 3)
        {
            rw.Int32(ref u06);
        }
        if (v >= 2)
        {
            rw.Int32(ref u07);
        }
        if (v >= 1)
        {
            rw.Int32(ref u08);
            rw.Int32(ref u09);
        }
        rw.Single(ref u10);
        rw.Single(ref u11);
        rw.Single(ref u12);
        rw.Single(ref u13);
        rw.Single(ref u14);
        rw.Single(ref u15);
        rw.Id(ref u16);
    }




}
