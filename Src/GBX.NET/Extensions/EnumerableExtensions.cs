namespace GBX.NET.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source,
        Func<T, IEnumerable<T>> selector)
    {
        return source
            .SelectMany(x => selector(x)
            .Flatten(selector))
            .Concat(source);
    }

    /// <summary>
    /// Creates a <see cref="List{T}"/> from an <see cref="IEnumerable{T}"/> with predefiend capacity for little performance boost.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="List{T}"/> from.</param>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <returns>A <see cref="List{T}"/> that contains elements from the input sequence.</returns>
    public static List<T> ToList<T>(this IEnumerable<T> source, int capacity)
    {
        var list = new List<T>(capacity);

        try
        {
            foreach (var item in source)
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
