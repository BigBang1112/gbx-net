using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// A direction-related feature set.
    /// </summary>
    public static class Dir
    {
        /// <summary>
        /// Gets the next clockwise direction.
        /// </summary>
        /// <param name="d">Direction to reference.</param>
        /// <returns>Returns the next clockwise direction.</returns>
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
    
        /// <summary>
        /// Gets the next counter clockwise direction.
        /// </summary>
        /// <param name="d">Direction to reference.</param>
        /// <returns>Returns the next counter clockwise direction.</returns>
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
