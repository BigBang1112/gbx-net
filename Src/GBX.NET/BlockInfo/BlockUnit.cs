namespace GBX.NET.BlockInfo;

public class BlockUnit
{
    public Int3 Coord { get; set; }
    public int? NorthClip { get; set; }
    public int? EastClip { get; set; }
    public int? SouthClip { get; set; }
    public int? WestClip { get; set; }
    public int? TopClip { get; set; }
    public int? BottomClip { get; set; }
}