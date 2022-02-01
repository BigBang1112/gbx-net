namespace GBX.NET;

public interface INodeDependant<T> where T : Node
{
    T? DependingNode { get; set; }
}
