namespace GBX.NET.Engines.Plug;

public class CPlugVehicleWheelPhyModel : IReadableWritable
{
    private string id = "";
    private bool isDriving;
    private bool isSteering;

    public string Id { get => id; set => id = value; }
    public bool IsDriving { get => isDriving; set => isDriving = value; }
    public bool IsSteering { get => isSteering; set => isSteering = value; }

    public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
    {
        rw.Boolean(ref isDriving);
        rw.Boolean(ref isSteering);
        rw.Id(ref id!);
    }
}
