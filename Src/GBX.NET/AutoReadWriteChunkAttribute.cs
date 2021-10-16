using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// Specifies an unskippable chunk that can't be managed properly to read with a method by reading and writing it based on peeking
    /// the 0xFACADE01 integer. This attribute mustn't be used on chunks containing node references and lookback strings. If more chunks
    /// are presented after a chunk marked with <see cref="AutoReadWriteChunkAttribute"/>, those will be merged into this chunk.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoReadWriteChunkAttribute : Attribute
    {

    }
}
