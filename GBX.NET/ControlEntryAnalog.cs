using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// A control entry with an additional <see cref="float"/> value.
    /// </summary>
    public class ControlEntryAnalog : ControlEntry
    {
        public float Value
        {
            get
            {
                if (((Data >> 16) & 0xFF) == 0xFF) // Left steer
                    return (Data & 0xFFFF) / (float)ushort.MaxValue - 1;
                if ((Data >> 16) == 1) // Full right steer
                    return 1;
                return (Data & 0xFFFF) / (float)ushort.MaxValue;
            }
        }

        public override string ToString()
        {
            return $"[{Time.ToStringTM()}] {Name}: {Value}";
        }
    }
}
