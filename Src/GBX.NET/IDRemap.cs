using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// Tells the library to save a GBX file with correct IDs related to the game version.
    /// </summary>
    public enum IDRemap
    {
        /// <summary>
        /// ID system used since TMUF until today.
        /// </summary>
        Latest,
        /// <summary>
        /// ID system used in TMU and older TM games.
        /// </summary>
        TrackMania2006,
        /// <summary>
        /// ID system used in Virtual Skipper.
        /// </summary>
        VirtualSkipper
    }
}
