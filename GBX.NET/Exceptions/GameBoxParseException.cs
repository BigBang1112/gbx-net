using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class GameBoxParseException : Exception
    {
        public GameBoxParseException() : base("GBX couldn't be parsed.")
        {
        }
    }
}
