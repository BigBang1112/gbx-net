namespace GBX.NET;

public interface INodeFull<T> : INodeFull where T : Node
{
    /// <summary>
    /// Wraps this node into <see cref="GameBox{T}"/> object with explicit conversion.
    /// </summary>
    /// <typeparam name="T">Type of the node to use on <see cref="GameBox{T}"/>.</typeparam>
    /// <returns>A <see cref="GameBox{T}"/>.</returns>
    GameBox<T> ToGbx();
}
