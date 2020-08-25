using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public static class Dir
    {
        public static Direction Clockwise(Direction d)
        {
            switch (d)
            {
                case Direction.North:
                    return Direction.East;
                case Direction.East:
                    return Direction.South;
                case Direction.South:
                    return Direction.West;
                case Direction.West:
                    return Direction.North;
                default:
                    throw new Exception();
            }
        }

        public static Direction CounterClockwise(Direction d)
        {
            switch (d)
            {
                case Direction.North:
                    return Direction.West;
                case Direction.West:
                    return Direction.South;
                case Direction.South:
                    return Direction.East;
                case Direction.East:
                    return Direction.North;
                default:
                    throw new Exception();
            }
        }
    }
}
