using System.Collections.Generic;
using System.Linq;

using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNode">A type of node to return as a result.</typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class GameBoxIOCustom<TNode, TResult> where TNode : CMwNod
    {
        /// <summary>
        /// Input GBX.
        /// </summary>
        protected GameBox? InputGBX { get; private set; }

        /// <summary>
        /// Output GBX.
        /// </summary>
        protected GameBox<TNode> GBX { get; private set; }

        public abstract TResult Process();

        public GameBoxIOCustom(GameBox gbx)
        {
            InputGBX = gbx;
            GBX = new GameBox<TNode>();
        }

        public GameBoxIOCustom(GameBox<TNode> gbx)
        {
            GBX = gbx;
        }

        protected IEnumerable<CGameCtnGhost> ExtractGhosts(CGameCtnMediaClip? clip)
        {
            if (clip is null)
                yield break;

            foreach (var track in clip.Tracks)
                foreach (var block in track.Blocks)
                    if (block is CGameCtnMediaBlockGhost ghostBlock)
                        yield return ghostBlock.GhostModel;
        }

        protected IEnumerable<CGameCtnGhost> ExtractGhosts(CGameCtnReplayRecord replay)
        {
            if (replay.Ghosts.Length > 0)
                return replay.Ghosts;

            return ExtractGhosts(replay.Clip);
        }

        protected IEnumerable<CGameCtnGhost> ExtractAllGhosts(CGameCtnChallenge map)
        {
            var ghosts = Enumerable.Empty<CGameCtnGhost>();

            if (map.ClipIntro != null)
                ghosts = ghosts.Concat(ExtractGhosts(map.ClipIntro));
            if (map.ClipGroupInGame != null)
                ghosts = ghosts.Concat(ExtractAllGhosts(map.ClipGroupInGame));
            if (map.ClipGroupEndRace != null)
                ghosts = ghosts.Concat(ExtractAllGhosts(map.ClipGroupEndRace));
            if (map.ClipPodium != null)
                ghosts = ghosts.Concat(ExtractGhosts(map.ClipPodium));
            if (map.ClipAmbiance != null)
                ghosts = ghosts.Concat(ExtractGhosts(map.ClipAmbiance));

            return ghosts;
        }

        protected IEnumerable<CGameCtnGhost> ExtractAllGhosts(CGameCtnMediaClipGroup clipGroup)
        {
            var ghosts = Enumerable.Empty<CGameCtnGhost>();

            foreach (var clip in clipGroup.Clips)
                ghosts = ghosts.Concat(ExtractGhosts(clip.Item1));

            return ghosts;
        }

        protected Dictionary<CGameCtnMediaClip, IEnumerable<CGameCtnGhost>> ExtractAllGhostsDictionary(CGameCtnMediaClipGroup clipGroup)
        {
            return clipGroup.Clips.ToDictionary(x => x.Item1, x => ExtractGhosts(x.Item1));
        }
    }
}
