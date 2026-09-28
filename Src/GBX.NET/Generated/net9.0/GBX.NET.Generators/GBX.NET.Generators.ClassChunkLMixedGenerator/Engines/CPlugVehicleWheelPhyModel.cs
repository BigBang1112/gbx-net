namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0912E000</remarks>
[Class(0x0912E000)]
public partial class CPlugVehicleWheelPhyModel : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x0912E000;




    private bool isDriving;
    public bool IsDriving { get => isDriving; set => isDriving = value; }

    private bool isSteering;
    public bool IsSteering { get => isSteering; set => isSteering = value; }

    private string? wheelId;
    public string? WheelId { get => wheelId; set => wheelId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehicleWheelPhyModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehicleWheelPhyModel() { }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.Boolean(ref isDriving);
        rw.Boolean(ref isSteering);
        rw.Id(ref wheelId);
    }




}
