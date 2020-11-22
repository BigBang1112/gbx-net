using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public class GameBoxReadProgress
    {
        /// <summary>
        /// Reading stage of GBX.
        /// </summary>
        public GameBoxReadProgressStage Stage { get; set; }
        /// <summary>
        /// Progress in percentage of each stage.
        /// </summary>
        public float Percentage { get; set; }
        /// <summary>
        /// Gradually updated GBX object.
        /// </summary>
        public GameBox GBX { get; set; }
        /// <summary>
        /// Chunk that has been currently read. This is null in <see cref="GameBoxReadProgressStage.Header"/> and <see cref="GameBoxReadProgressStage.RefTable"/> stages.
        /// </summary>
        public Chunk Chunk { get; set; }

        public GameBoxReadProgress()
        {

        }

        public GameBoxReadProgress(GameBoxReadProgressStage stage, float percentage, GameBox gbx)
            : this(stage, percentage, gbx, null) { }

        public GameBoxReadProgress(GameBoxReadProgressStage stage, float percentage, GameBox gbx, Chunk chunk)
        {
            Stage = stage;
            Percentage = percentage;
            GBX = gbx;
            Chunk = chunk;
        }
    }
}
