using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET;

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
    public static Direction Clockwise(Direction d) => d switch
    {
        Direction.North => Direction.East,
        Direction.East => Direction.South,
        Direction.South => Direction.West,
        Direction.West => Direction.North,
        _ => throw new Exception(),
    };

    /// <summary>
    /// Gets the next counter clockwise direction.
    /// </summary>
    /// <param name="d">Direction to reference.</param>
    /// <returns>Returns the next counter clockwise direction.</returns>
    public static Direction CounterClockwise(Direction d) => d switch
    {
        Direction.North => Direction.West,
        Direction.West => Direction.South,
        Direction.South => Direction.East,
        Direction.East => Direction.North,
        _ => throw new Exception(),
    };

    public static Direction Add(Direction a, Direction b)
    {
        return (Direction)(((int)a + (int)b) % 4);
    }
}
