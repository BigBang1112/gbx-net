namespace GBX.NET;

/// <summary>
/// A control entry with an additional <see cref="float"/> value.
/// </summary>
[Obsolete("Use GBX.NET.Inputs.IInputReal instead. Class will be removed in 1.3.0")]
public record ControlEntryAnalog : ControlEntry
{
    public float Value
    {
        get
        {
            if (IsOldTM) return BitConverter.ToSingle(BitConverter.GetBytes(Data), 0);

            if (((Data >> 16) & 0xFF) == 0xFF) // Left steer
                return (Data & 0xFFFF) / (float)ushort.MaxValue - 1;
            if ((Data >> 16) == 1) // Full right steer
                return 1;
            return (Data & 0xFFFF) / (float)ushort.MaxValue;
        }
    }

    /// <summary>
    /// If the input value comes from TM1.0.
    /// </summary>
    public bool IsOldTM { get; init; }

    public ControlEntryAnalog(string name, TimeInt32 time, uint data, bool isOldTm = false) : base(name, time, data)
    {
        IsOldTM = isOldTm;
    }

    public override string ToString()
    {
        return $"[{Time}] {Name}: {Value}";
    }
}
