using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET;

public enum DifferenceSolution
{
    FirstChunk,
    Average,
    ExceptionIfDifferent,
    Default = ExceptionIfDifferent
}
