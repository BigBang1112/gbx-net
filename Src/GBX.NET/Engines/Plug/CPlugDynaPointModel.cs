namespace GBX.NET.Engines.Plug;

[Node(0x090D7000)]
public class CPlugDynaPointModel : CMwNod, IReadableWritable
{
    private float u01;
    private float u02;
    private float u03;
    private float u04;
    private float u05;
    private float u06;
    private float u07;

    public float U01 { get => u01; set => u01 = value; }
    public float U02 { get => u02; set => u02 = value; }
    public float U03 { get => u03; set => u03 = value; }
    public float U04 { get => u04; set => u04 = value; }
    public float U05 { get => u05; set => u05 = value; }
    public float U06 { get => u06; set => u06 = value; }
    public float U07 { get => u07; set => u07 = value; }

    public CPlugDynaPointModel()
	{

	}

    public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
    {
        rw.Int32(0);
        rw.Single(ref u01);
        rw.Single(ref u02);
        rw.Single(ref u03);
        rw.Single(ref u04);
        rw.Single(ref u05);
        rw.Single(ref u06);
        rw.Single(ref u07);
    }
}
