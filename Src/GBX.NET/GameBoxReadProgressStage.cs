using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET;

/// <summary>
/// Stage of the GBX parse.
/// </summary>
public enum GameBoxReadProgressStage
{
    /// <summary>
    /// Parse of the most basic header information.
    /// </summary>
    Header,
    /// <summary>
    /// Parse of the user data chunks.
    /// </summary>
    HeaderUserData,
    /// <summary>
    /// Parse of the reference table.
    /// </summary>
    RefTable,
    /// <summary>
    /// Parse of the body.
    /// </summary>
    Body
}
