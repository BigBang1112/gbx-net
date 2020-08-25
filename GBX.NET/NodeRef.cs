using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    /// <summary>
    /// A reference to an auxiliary node instance.
    /// </summary>
    /// <typeparam name="T">The class of the node.</typeparam>
    [Obsolete]
    public class NodeRef<T> where T : Node
    {
        /// <summary>
        /// Body which this reference is part of.
        /// </summary>
        public IGameBoxBody Body { get; }
        /// <summary>
        /// Index of the auxiliary node.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// An object reference of the auxiliary node. Will be <see cref="null"/> if <see cref="Index"/> is -1.
        /// </summary>
        public T Node
        {
            get
            {
                if (Body.AuxilaryNodes.TryGetValue(Index, out Node node))
                    return (T)node;
                return null;
            }
        }

        /// <summary>
        /// Reference to an auxiliary node.
        /// </summary>
        /// <param name="body">Body which this reference if part of.</param>
        /// <param name="index">An object reference of the auxiliary node. Can be -1.</param>
        public NodeRef(IGameBoxBody body, int index)
        {
            Body = body;
            Index = index;
        }

        public override string ToString()
        {
            return $"Node reference: [{Node}]";
        }
    }

    /// <summary>
    /// A reference to an auxiliary node instance.
    /// </summary>
    /// <typeparam name="T">The class of the node.</typeparam>
    [Obsolete]
    public class NodeRef
    {
        /// <summary>
        /// Body which this reference is part of.
        /// </summary>
        public IGameBoxBody Body { get; }
        /// <summary>
        /// Index of the auxiliary node.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// An object reference of the auxiliary node. Will be <see cref="null"/> if <see cref="Index"/> is -1.
        /// </summary>
        public Node Node
        {
            get
            {
                if (Body.AuxilaryNodes.TryGetValue(Index, out Node node))
                    return node;
                return null;
            }
        }

        /// <summary>
        /// Reference to an auxiliary node.
        /// </summary>
        /// <param name="body">Body which this reference if part of.</param>
        /// <param name="index">An object reference of the auxiliary node. Can be -1.</param>
        public NodeRef(IGameBoxBody body, int index)
        {
            Body = body;
            Index = index;
        }

        public NodeRef<T> Cast<T>() where T : Node
        {
            return new NodeRef<T>(Body, Index);
        }

        public override string ToString()
        {
            return $"Node reference: [{Node}]";
        }
    }
}
