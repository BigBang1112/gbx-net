namespace GBX.NET.Engines.Plug;

[Node(0x09104000)] // Uninstantiable?
public class CPlugTriggerAction : CMwNod, IReadableWritable
{
    private int u01;
    private float? u02;
    private float? u03;
    private float? u04;
    private float? u05;
    private int? u06;
    private int? u07;
    private int? u08;
    private int? u09;
    private float u10;
    private float u11;
    private float u12;
    private float u13;
    private float u14 = 1;
    private float u15 = 2;
    private string? u16;

    public int U01 { get => u01; set => u01 = value; }
    public float? U02 { get => u02; set => u02 = value; }
    public float? U03 { get => u03; set => u03 = value; }
    public float? U04 { get => u04; set => u04 = value; }
    public float? U05 { get => u05; set => u05 = value; }
    public int? U06 { get => u06; set => u06 = value; }
    public int? U07 { get => u07; set => u07 = value; }
    public int? U08 { get => u08; set => u08 = value; }
    public int? U09 { get => u09; set => u09 = value; }
    public float U10 { get => u10; set => u10 = value; }
    public float U11 { get => u11; set => u11 = value; }
    public float U12 { get => u12; set => u12 = value; }
    public float U13 { get => u13; set => u13 = value; }
    public float U14 { get => u14; set => u14 = value; }
    public float U15 { get => u15; set => u15 = value; }
    public string? U16 { get => u16; set => u16 = value; }

    public CPlugTriggerAction()
	{

	}

    public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
    {
        if (version >= 5)
        {
            rw.Int32(ref u01);
        }
        else
        {
            u01 = 1;
        }

        if (version >= 4)
        {
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
        }
        
        if (version >= 3)
        {
            rw.Int32(ref u06);
        }
        
        if (version >= 2)
        {
            rw.Int32(ref u07);
        }
        
        if (version >= 1)
        {
            rw.Int32(ref u08);
            rw.Int32(ref u09);
        }
        
        if (version >= 0)
        {
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Single(ref u12);
            rw.Single(ref u13);
            rw.Single(ref u14);
            rw.Single(ref u15);
            rw.Id(ref u16);
        }
    }
}
