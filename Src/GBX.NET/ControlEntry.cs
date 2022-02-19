namespace GBX.NET;

/// <summary>
/// Input from an input device.
/// </summary>
public class ControlEntry
{
    public string Name { get; set; }
    public TimeInt32 Time { get; set; }
    public uint Data { get; set; }
    public bool IsEnabled => Data != 0;

    public ControlEntry(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"[{Time}] {Name}: {((Data == 128 || Data == 1 || Data == 0) ? IsEnabled.ToString() : Data.ToString())}";
    }
}
