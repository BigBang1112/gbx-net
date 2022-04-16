namespace GBX.NET;

public partial class GameBoxReader
{
    /// <summary>
    /// Reads values in a dictionary kind (first key, then value). For node dictionaries, use the <see cref="ReadDictionaryNode{TKey, TValue}"/> method for better performance.
    /// </summary>
    /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>. Mustn't be null.</typeparam>
    /// <typeparam name="TValue">One of the supported types of <see cref="Read{T}"/>.</typeparam>
    /// <param name="overrideKey">If the pair in the dictionary should be overriden by the new value when a duplicate key is found. It is recommended to keep it false to easily spot errors.</param>
    /// <returns>A dictionary.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue> ReadDictionary<TKey, TValue>(bool overrideKey = false) where TKey : notnull
    {
        var length = ReadInt32();

        var dictionary = new Dictionary<TKey, TValue>(length);

        for (var i = 0; i < length; i++)
        {
            var key = Read<TKey>();
            var value = Read<TValue>();

            if (overrideKey)
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        return dictionary;
    }

    /// <summary>
    /// Reads nodes in a dictionary kind (first key, then value).
    /// </summary>
    /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>. Mustn't be null.</typeparam>
    /// <typeparam name="TValue">A node that is presented as node reference.</typeparam>
    /// <param name="overrideKey">If the pair in the dictionary should be overriden by the new value when a duplicate key is found. It is recommended to keep it false to easily spot errors.</param>
    /// <returns>A dictionary.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxReaderSettings.Gbx"/> is null.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue?> ReadDictionaryNode<TKey, TValue>(bool overrideKey = false) where TKey : notnull where TValue : Node
    {
        var length = ReadInt32();

        var dictionary = new Dictionary<TKey, TValue?>(length);

        for (var i = 0; i < length; i++)
        {
            var key = Read<TKey>();
            var value = ReadNodeRef<TValue>();

            AddOrOverrideKey(dictionary, key, value, overrideKey);
        }

        return dictionary;
    }

    public async Task<IDictionary<TKey, TValue?>?> ReadDictionaryNodeAsync<TKey, TValue>(
        bool overrideKey,
        CancellationToken cancellationToken = default)
        where TKey : notnull where TValue : Node
    {
        var length = ReadInt32();

        var dictionary = new Dictionary<TKey, TValue?>(length);

        for (var i = 0; i < length; i++)
        {
            var key = Read<TKey>();
            var value = await ReadNodeRefAsync<TValue>(cancellationToken);

            AddOrOverrideKey(dictionary, key, value, overrideKey);
        }

        return dictionary;
    }

    private static void AddOrOverrideKey<TKey, TValue>(Dictionary<TKey, TValue?> dictionary,
                                                       TKey key,
                                                       TValue? value,
                                                       bool overrideKey)
                                                       where TKey : notnull
                                                       where TValue : Node
    {
        if (overrideKey)
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
}
