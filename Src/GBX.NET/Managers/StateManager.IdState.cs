namespace GBX.NET.Managers;

public partial class StateManager
{
    public class IdState
    {
        public int? Version { get; set; }
        public List<string> Strings { get; }
        public bool IsWritten { get; set; }

        public Dictionary<Guid, IdState> SubStates { get; set; }

        public IdState()
        {
            Strings = new List<string>();
            SubStates = new Dictionary<Guid, IdState>();
        }
    }
}
