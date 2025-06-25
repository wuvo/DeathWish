namespace NewestCOServer.Features
{
    public class SkillPK : PVPEvents
    {
        public SkillPK()
        {
            EventTitle = "FB/SS/VP";
            Duration = 20;
            MapEvent = 701;
            AllowedSkills = new System.Collections.ArrayList{ (ushort)1045, (ushort)1046, (ushort)1047 };
        }

        public override void TeleportPlayersToMap()
        {
            foreach (Game.Character c in PlayerList.Values)
            {
                if (c.Loc.Map == 1616)
                {
                    c.EventBase = this;
                    c.Lottery = false;
                    c.PKTHits = 5;
                    X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                    Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                    c.StatEff.Remove(Game.StatusEffectEn.Fly);
                    c.StatEff.Remove(Game.StatusEffectEn.Cyclone);
                    c.StatEff.Remove(Game.StatusEffectEn.SuperMan);
                    c.Teleport(MapEvent, X, Y);
                    c.CurHP = c.MaxHP;
                    c.Protection = true;
                }
                else
                {
                    c.MyClient.LocalMessage(2000, "You've been removed from the " + EventTitle + " Event!");
                    PlayerList.Remove(c.EntityID);
                    break;
                }
            }
        }
    }
}