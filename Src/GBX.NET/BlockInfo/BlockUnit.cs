namespace GBX.NET.BlockInfo;

public class BlockUnit
{
    public Int3 Coord { get; set; }
    public string[]? NorthClips { get; set; }
    public string[]? EastClips { get; set; }
    public string[]? SouthClips { get; set; }
    public string[]? WestClips { get; set; }
    public string[]? TopClips { get; set; }
    public string[]? BottomClips { get; set; }
}