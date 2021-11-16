using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public static List<T> ToList<T>(this IEnumerable<T> source, int capacity)
    {
        var list = new List<T>(capacity);

        foreach (var item in source)
            list.Add(item);

        return list;
    }
}
