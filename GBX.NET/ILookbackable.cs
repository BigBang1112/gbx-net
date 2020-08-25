using System.Collections.Generic;

namespace GBX.NET
{
    /// <summary>
    /// Supports lookback string management on its own
    /// </summary>
    public interface ILookbackable
    {
        int? LookbackVersion { get; set; }
        List<string> LookbackStrings { get; set; }
        bool LookbackWritten { get; set; }
    }
}
