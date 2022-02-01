namespace GBX.NET.Extensions;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER

public static class AsyncEnumerableExtensions
{
    public static async Task<IList<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, int capacity)
    {
        var list = new List<T>(capacity);

        try
        {
            await foreach (var item in source)
            {
                list.Add(item);
            }
        }
        catch (EndOfStreamException)
        {

        }

        return list;
    }
}

#endif