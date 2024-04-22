namespace GBX.NET.Engines.Plug;

public partial class CPlugTree
{
    public IEnumerable<CPlugTree> GetAllChildren()
    {
        return GetAllChildren(this);
        
        static IEnumerable<CPlugTree> GetAllChildren(CPlugTree tree)
        {
            if (tree.Children is null)
            {
                yield break;
            }

            foreach (var child in tree.Children)
            {
                foreach (var descendant in GetAllChildren(child))
                {
                    yield return descendant;
                }
            }
        }
    }
}
