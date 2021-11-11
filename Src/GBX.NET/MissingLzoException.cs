using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET;

public class MissingLzoException : Exception
{
    public override string Message => "To parse a body of this GBX file, an LZO compression algorithm is required.\n\n" +
        "Please reference the GBX.NET.LZO library inside your project.\n\n" +
        "Note that the GBX.NET.LZO library is licensed under GNU General Public License 3, therefore you HAVE to use the same license for your project after referencing.\n\n" +
        "However, you DON'T HAVE TO reference the library if you're looking to extract the HEADER DATA only. See the GameBox.ParseHeader method.";
}
