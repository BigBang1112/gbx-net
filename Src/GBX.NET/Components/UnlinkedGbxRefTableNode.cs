namespace GBX.NET.Components;

public abstract class UnlinkedGbxRefTableNode(int flags, bool useFile, int nodeIndex)
{
    public int Flags { get; set; } = flags;
    public bool UseFile { get; set; } = useFile;
    public int NodeIndex { get; } = nodeIndex;
}
