using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public interface IGameBox
    {
        ClassIDRemap Game { get; set; }
        uint? ClassID { get; }
    }
}
