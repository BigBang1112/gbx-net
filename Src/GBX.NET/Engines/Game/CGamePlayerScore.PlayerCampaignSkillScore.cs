namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public class PlayerCampaignSkillScore
    {
        public int Version { get; }

        public static PlayerCampaignSkillScore Parse(GameBoxReader r)
        {
            var version = r.ReadInt32();
            var u01 = r.ReadInt32(); // + 0
            var u02 = r.ReadString(); // + 4
            var u03 = default(int?);
            var u04 = default(int?);
            var u05 = default(int?);
            var u06 = default(int?);
            var u07 = default(int?);
            var u08 = default(int?);

            if (version == 1)
            {
                u03 = r.ReadInt32();
            }
            else if (version == 2)
            {
                u04 = r.ReadInt32();
                var ara = r.ReadInt32(); // PlayerCampaignFilteredSkillScore array
            }
            else
            {
                u05 = r.ReadInt32();
                u06 = r.ReadInt32();
                var ara1 = r.ReadInt32(); // PlayerCampaignFilteredSkillScore array
            }
            
            if (version >= 4)
            {
                u07 = r.ReadInt32();
                u08 = r.ReadInt32();
                var ara2 = r.ReadInt32(); // PlayerCampaignFilteredSkillScore array
            }

            return new();
        }
    }
}
