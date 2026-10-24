using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Serialization;

public partial class GbxReader
{
    public NodeRef<T> ReadNodeRef<T>(NodeRef<T>? reference) where T : CMwNod?
    {
        reference ??= new();
        ReadNodeRef(ref reference);
        return reference;
    }

    /// <summary>
    /// Reads stored reference data, leaving an absent handle null when the file stores a null reference.
    /// </summary>
    public void ReadNodeRef<T>([NotNullIfNotNull(nameof(reference))] ref NodeRef<T>? reference) where T : CMwNod?
    {
        var index = default(int?);

        if (encapsulation is null)
        {
            index = ReadInt32();

            if (index == -1)
            {
                reference?.Clear();
                return;
            }

            if (NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                logger?.LogDebug("NodeRef #{Index}: {ExistingNode} (existing)", index.Value, existingNode);

                if (existingNode is GbxRefTableNode descriptor)
                {
                    NodeRef.GetOrCreate(ref reference).Descriptor = descriptor;
                }
                else
                {
                    NodeRef.GetOrCreate(ref reference).SetNodeCore((CMwNod)existingNode);
                }

                return;
            }

            if (refTable?.TryGetValue(index.Value, out var externalNode) == true)
            {
                logger?.LogDebug("NodeRef #{Index}: {ExternalNode}", index.Value, externalNode);

                NodeRef.GetOrCreate(ref reference).Descriptor = externalNode;
                return;
            }
        }

        var rawClassId = ReadHexUInt32();

        if (encapsulation is not null && rawClassId == uint.MaxValue)
        {
            reference?.Clear();
            return;
        }

        var classId = ClassManager.Wrap(rawClassId);

        if (logger is not null)
        {
            if (classId == rawClassId)
            {
                logger.LogDebug("NodeRef #{Index}: 0x{ClassId:X8} ({ClassName})", index, classId, ClassManager.GetName(classId));
            }
            else
            {
                logger.LogDebug("NodeRef #{Index}: 0x{ClassId:X8} ({ClassName}, raw: 0x{RawClassId:X8})", index, classId, ClassManager.GetName(classId), rawClassId);
            }
        }

        var node = ClassManager.New(classId, Settings.GameVersion) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");

        if (index.HasValue)
        {
            if (logger is not null && NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                logger.LogWarning("NodeRef #{Index}: {ExistingNode} (existing was overwriten!)", index.Value, existingNode);
            }

            NodeDict[index.Value] = node;
        }

        NodeRef.GetOrCreate(ref reference).SetNodeCore((CMwNod)ReadNode(node));
    }

    public NodeRefList<T> ReadListNodeRef<T>(int? length = null) where T : CMwNod?
    {
        var count = length ?? ReadInt32();
        EnsureValidLength(count);

        var list = new NodeRefList<T>(count);

        for (var i = 0; i < count; i++)
        {
            list.AddOwned(ReadNodeRef<T>(null));
        }

        return list;
    }

    public NodeRefList<T> ReadListNodeRef_deprec<T>(int? length = null) where T : CMwNod?
    {
        ReadDeprecVersion();
        return ReadListNodeRef<T>(length);
    }

    public NodeRefList<T>[] ReadJaggedListNodeRef<T>(
        int? innerLength = null,
        int? outerLength = null) where T : CMwNod?
    {
        var count = outerLength ?? ReadInt32();
        EnsureValidLength(count);

        var result = new NodeRefList<T>[count];

        for (var i = 0; i < count; i++)
        {
            result[i] = ReadListNodeRef<T>(innerLength);
        }

        return result;
    }
}
