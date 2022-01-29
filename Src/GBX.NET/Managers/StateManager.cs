using System.Collections.Concurrent;

namespace GBX.NET.Managers;

public partial class StateManager
{
    /// <summary>
    /// Long-term state. Needs to be disposed with <see cref="IDisposable.Dispose"/> on the <see cref="GameBox"/>.
    /// </summary>
    public ConcurrentDictionary<Guid, SortedDictionary<int, Node>> AuxilaryNodeStates { get; }
    /// <summary>
    /// Long-term state. Needs to be disposed with <see cref="IDisposable.Dispose"/> on the <see cref="GameBox"/>.
    /// </summary>
    public ConcurrentDictionary<Guid, IdState> IdStates { get; }
    /// <summary>
    /// Long-term state. Needs to be disposed with <see cref="IDisposable.Dispose"/> on the <see cref="GameBox"/>.
    /// </summary>
    public ConcurrentDictionary<Guid, GameBoxRefTable?> ReferenceTableStates { get; }

    public StateManager()
    {
        AuxilaryNodeStates = new ConcurrentDictionary<Guid, SortedDictionary<int, Node>>();
        IdStates = new ConcurrentDictionary<Guid, IdState>();
        ReferenceTableStates = new ConcurrentDictionary<Guid, GameBoxRefTable?>();
    }

    public Guid CreateState()
    {
        var guid = Guid.NewGuid();

        AuxilaryNodeStates[guid] = new SortedDictionary<int, Node>();
        IdStates[guid] = new IdState();
        ReferenceTableStates[guid] = null;

        return guid;
    }

    public void RemoveState(Guid stateGuid)
    {
        AuxilaryNodeStates.TryRemove(stateGuid, out _);
        IdStates.TryRemove(stateGuid, out _);
        ReferenceTableStates.TryRemove(stateGuid, out _);
    }

    public void ResetIdState(Guid stateGuid)
    {
        var idStates = IdStates[stateGuid];
        idStates.Version = null;
        idStates.Strings.Clear();
        idStates.IsWritten = false;
    }

    public IdState GetIdState(Guid stateGuid)
    {
        return IdStates[stateGuid];
    }

    public Guid CreateIdSubState(Guid stateGuid)
    {
        var guid = Guid.NewGuid();
        IdStates[stateGuid].SubStates[guid] = new IdState();
        return guid;
    }

    public IdState GetIdSubState(Guid stateGuid, Guid subStateGuid)
    {
        return IdStates[stateGuid].SubStates[subStateGuid];
    }

    public bool TryGetIdSubState(Guid stateGuid, Guid subStateGuid, out IdState? value)
    {
        return IdStates[stateGuid].SubStates.TryGetValue(subStateGuid, out value);
    }

    public bool RemoveIdSubState(Guid stateGuid, Guid subStateGuid)
    {
        return IdStates[stateGuid].SubStates.TryRemove(subStateGuid, out _);
    }

    public Node GetLastNode(Guid stateGuid)
    {
        return AuxilaryNodeStates[stateGuid].Last().Value;
    }

    public bool ContainsNodeIndex(Guid stateGuid, int index)
    {
        return AuxilaryNodeStates[stateGuid].ContainsKey(index);
    }

    public bool ContainsNode(Guid stateGuid, Node node)
    {
        return AuxilaryNodeStates[stateGuid].ContainsValue(node);
    }

    public bool TryGetNode(Guid stateGuid, int index, out Node? node)
    {
        return AuxilaryNodeStates[stateGuid].TryGetValue(index, out node);
    }

    public Node? GetNode(Guid stateGuid, int index)
    {
        TryGetNode(stateGuid, index, out Node? node);
        return node;
    }

    public void SetNode(Guid stateGuid, int index, Node node)
    {
        AuxilaryNodeStates[stateGuid][index] = node;
    }

    public int GetNodeIndexByNode(Guid stateGuid, Node node)
    {
        return AuxilaryNodeStates[stateGuid].FirstOrDefault(x => x.Equals(node)).Key;
    }

    public void AddNode(Guid stateGuid, Node node)
    {
        var dictionary = AuxilaryNodeStates[stateGuid];
        dictionary[dictionary.Count] = node;
    }

    public int GetNodeCount(Guid stateGuid)
    {
        return AuxilaryNodeStates[stateGuid].Count;
    }

    public int IndexOfId(Guid stateGuid, string str)
    {
        return IdStates[stateGuid].Strings.IndexOf(str);
    }

    public GameBoxRefTable GetReferenceTable(Guid stateGuid)
    {
        return ReferenceTableStates[stateGuid]!;
    }

    public static readonly StateManager Shared = new();
}
