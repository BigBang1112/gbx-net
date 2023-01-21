namespace GBX.NET;

public class Badge : IReadableWritable
{
    private Vec3 color = (1, 1, 1);
    private int? u01;
    private string? u02;
    private (string, string)[] stickers = Array.Empty<(string, string)>();
    private string[] layers = Array.Empty<string>();

    public Vec3 Color { get => color; set => color = value; }
    public int? U01 { get => u01; set => u01 = value; }
    public string? U02 { get => u02; set => u02 = value; }
    public (string, string)[] Stickers { get => stickers; set => stickers = value; }
    public string[] Layers { get => layers; set => layers = value; }

    public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
    {
        rw.Vec3(ref color);

        if (version == 0)
        {
            rw.Int32(ref u01);
            rw.String(ref u02);
        }

        rw.Array(ref stickers!, (i, r) => (r.ReadString(), r.ReadString()), // SSticker array
        (x, w) =>
        {
            w.Write(x.Item1);
            w.Write(x.Item2);
        });

        rw.ArrayString(ref layers!); // SLayer array
    }
}
