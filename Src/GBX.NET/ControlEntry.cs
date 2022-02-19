namespace GBX.NET;

/// <summary>
/// Input from an input device.
/// </summary>
public record ControlEntry(string Name, TimeInt32 Time, uint Data)
{
    public bool IsEnabled => Data != 0;

    public override string ToString()
    {
        return $"[{Time}] {Name}: {((Data == 128 || Data == 1 || Data == 0) ? IsEnabled.ToString() : Data.ToString())}";
    }
}
