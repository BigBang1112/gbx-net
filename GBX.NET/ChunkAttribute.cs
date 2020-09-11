using System;

namespace GBX.NET
{
    public class ChunkAttribute : Attribute
    {
        public uint ID { get; }
        public uint ClassID => ID & 0xFFFFF000;
        public uint ChunkID => ID & 0xFFF;
        /// <summary>
        /// If the chunk should be read immediately after finding. You should always set this to <see cref="true"/> if the chunk is skippable and contains a lookback string (or meta). This property is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).
        /// </summary>
        public bool ProcessSync { get; }
        public string Description { get; }

        [Obsolete]
        public ChunkAttribute(uint classID, uint chunkID, bool processAsync = true)
        {
            ID = classID + chunkID;
            ProcessSync = !processAsync;
        }

        [Obsolete]
        public ChunkAttribute(uint classID, uint chunkID) : this(classID, chunkID, true)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        public ChunkAttribute(uint chunkID) : this(chunkID, false)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        /// <param name="processSync">If the chunk should be read immediately after finding. You should always set this to <see cref="true"/> if the chunk is skippable (or in header) and contains a lookback string (or meta). This setting is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).</param>
        public ChunkAttribute(uint chunkID, bool processSync) : this(chunkID, processSync, null)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        /// <param name="description">Very short description of the chunk.</param>
        public ChunkAttribute(uint chunkID, string description) : this(chunkID, false, description)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunkID">Full ID of the chunk.</param>
        /// <param name="processSync">If the chunk should be read immediately after finding. You should always set this to <see cref="true"/> if the chunk is skippable (or in header) and contains a lookback string (or meta). This setting is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).</param>
        /// <param name="description">Very short description of the chunk.</param>
        public ChunkAttribute(uint chunkID, bool processSync, string description)
        {
            ID = chunkID;
            ProcessSync = processSync;
            Description = description;
        }
    }
}