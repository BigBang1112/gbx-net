using System.Linq.Expressions;

namespace GBX.NET;

// THANKS! <3 https://stackoverflow.com/a/23391746/3923447

/// <summary>
/// Class to cast to type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">Target type</typeparam>
internal static class CastTo<T>
{
    /// <summary>
    /// Casts <typeparamref name="S"/> to <typeparamref name="T"/>.
    /// This does not cause boxing for value types.
    /// Useful in generic methods.
    /// </summary>
    /// <typeparam name="S">Source type to cast from. Usually a generic type.</typeparam>
    public static T From<S>(S s)
    {
        return Cache<S>.caster(s);
    }

    private static class Cache<S>
    {
        public static readonly Func<S, T> caster = Get();

        private static Func<S, T> Get()
        {
            var p = Expression.Parameter(typeof(S));
            var c = Expression.ConvertChecked(p, typeof(T));
            return Expression.Lambda<Func<S, T>>(c, p).Compile();
        }
    }
}
