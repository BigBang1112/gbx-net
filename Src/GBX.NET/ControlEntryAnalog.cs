namespace GBX.NET;

/// <summary>
/// A control entry with an additional <see cref="float"/> value.
/// </summary>
public class ControlEntryAnalog : ControlEntry
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
    public bool IsOldTM { get; }

    public ControlEntryAnalog(string name) : base(name)
    {

    }

    public ControlEntryAnalog(string name, bool isOldTm) : base(name)
    {
        IsOldTM = isOldTm;
    }

    public override string ToString()
    {
        return $"[{Time.ToTmString()}] {Name}: {Value}";
    }
}
