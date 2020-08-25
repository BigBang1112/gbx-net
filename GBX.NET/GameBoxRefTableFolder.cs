using System.Collections.Generic;
using System.Linq;

namespace GBX.NET
{
    public class GameBoxRefTableFolder
    {
        public string Name { get; }
        public GameBoxRefTableFolder Parent { get; }
        public List<GameBoxRefTableFolder> Folders { get; }

        public GameBoxRefTableFolder(string name, GameBoxRefTableFolder parent, params GameBoxRefTableFolder[] folders)
        {
            Name = name;
            Parent = parent;
            Folders = folders.ToList();
        }

        public GameBoxRefTableFolder(string name, GameBoxRefTableFolder parent) : this(name, parent, new GameBoxRefTableFolder[0])
        {

        }

        public GameBoxRefTableFolder(string name) : this(name, null)
        {

        }
    }
}