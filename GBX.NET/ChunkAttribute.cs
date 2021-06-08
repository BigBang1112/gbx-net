using System;

namespace GBX.NET
{
    /// <summary>
    /// Attribute that assigns specific metadata to a chunk.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ChunkAttribute : Attribute
    {
        /// <summary>
        /// Full ID of the chunk.
        /// </summary>
        public uint ID { get; }
        /// <summary>
        /// If the chunk should be read immediately after finding. You should always set this to true if the chunk is skippable and contains a lookback string (or meta). This property is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).
        /// </summary>
        public bool ProcessSync { get; }
        /// <summary>
        /// Very short lowercase description of the chunk.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Assigns an ID to a chunk.
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        public ChunkAttribute(uint chunkID) : this(chunkID, false)
        {

        }

        /// <summary>
        /// Assigns an ID to a chunk.
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        /// <param name="processSync">If the chunk should be read immediately after finding. You should always set this to true if the chunk is skippable (or in header) and contains a lookback string (or meta). This setting is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).</param>
        public ChunkAttribute(uint chunkID, bool processSync) : this(chunkID, processSync, null)
        {
            
        }

        /// <summary>
        /// Assigns an ID to a chunk.
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        /// <param name="description">Very short description of the chunk.</param>
        public ChunkAttribute(uint chunkID, string description) : this(chunkID, false, description)
        {

        }

        /// <summary>
        /// Assigns an ID to a chunk.
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        /// <param name="processSync">If the chunk should be read immediately after finding. You should always set this to true if the chunk is skippable (or in header) and contains a lookback string (or meta). This setting is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).</param>
        /// <param name="description">Very short description of the chunk.</param>
        public ChunkAttribute(uint chunkID, bool processSync, string description)
        {
            ID = chunkID;
            ProcessSync = processSync;
            Description = description;
        }

        /// <summary>
        /// Gets the class part of <see cref="ID"/>.
        /// </summary>
        /// <returns>Class part of ID.</returns>
        public uint GetClassPart() => ID & 0xFFFFF000;

        /// <summary>
        /// Gets the chunk part of <see cref="ID"/>.
        /// </summary>
        /// <returns>Chunk part of ID.</returns>
        public uint GetChunkPart() => ID & 0xFFF;
    }
}