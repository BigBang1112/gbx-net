using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

public partial class GbxReader
{
    public NodeRef<T> ReadNodeRef<T>(NodeRef<T>? reference) where T : CMwNod
    {
        reference ??= new();
        var index = default(int?);

        if (encapsulation is null)
        {
            index = ReadInt32();

            if (index == -1)
            {
                reference.SetEmbedded(null);
                return reference;
            }

            if (NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                logger?.LogDebug("NodeRef #{Index}: {ExistingNode} (existing)", index.Value, existingNode);

                if (existingNode is GbxRefTableNode descriptor)
                    reference.SetDescriptor(descriptor);
                else
                    reference.SetReadTarget((CMwNod)existingNode);
                return reference;
            }

            if (refTable?.TryGetValue(index.Value, out var externalNode) == true)
            {
                logger?.LogDebug("NodeRef #{Index}: {ExternalNode}", index.Value, externalNode);

                reference.SetDescriptor(externalNode);
                return reference;
            }
        }

        var rawClassId = ReadUInt32();

        if (encapsulation is not null && rawClassId == uint.MaxValue)
        {
            reference.SetEmbedded(null);
            return reference;
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

        var node = ClassManager.New(classId) ?? throw new Exception($"Unknown class ID: 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"})");

        if (index.HasValue)
        {
            if (logger is not null && NodeDict.TryGetValue(index.Value, out var existingNode))
            {
                logger.LogWarning("NodeRef #{Index}: {ExistingNode} (existing was overwriten!)", index.Value, existingNode);
            }

            NodeDict[index.Value] = node;
        }

        reference.SetReadTarget((CMwNod)ReadNode(node));
        return reference;
    }

    public NodeRefList<T> ReadListNodeRef<T>(int? length = null) where T : CMwNod
    {
        var count = length ?? ReadInt32();
        EnsureValidLength(count);
        var list = new NodeRefList<T>();
        for (var i = 0; i < count; i++) list.AddReference(ReadNodeRef<T>(null));
        list.IsFixedSize = length.HasValue;
        return list;
    }

    public NodeRefList<T> ReadListNodeRef_deprec<T>(int? length = null) where T : CMwNod
    {
        ReadDeprecVersion();
        return ReadListNodeRef<T>(length);
    }

    public NodeRefList<T>[] ReadJaggedArrayNodeRef<T>(bool fixedSize, int? innerLength = null, int? outerLength = null) where T : CMwNod
    {
        var count = outerLength ?? ReadInt32();
        EnsureValidLength(count);
        var result = new NodeRefList<T>[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = ReadListNodeRef<T>(innerLength);
            if (fixedSize) result[i].IsFixedSize = true;
        }
        return result;
    }
}
