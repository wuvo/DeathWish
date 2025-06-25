using NewestCOServer.Game;
using System;

namespace NewestCOServer.Features
{
    public class SkillPK : PVPEvents
    {
        public static bool FBSS = false;
        public SkillPK()
        {
            EventTitle = "Five'n'Out";
            Duration = 10;
            MapEvent = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FFADamage = false;
            AllowedSkills = new System.Collections.Generic.List<ushort>{ (ushort)1045, (ushort)1046, (ushort)1047 };
        }
        
        public override void TeleportPlayersToMap()
        {
            foreach (Game.Character c in PlayerList.Values)
            {
                if (c.Loc.Map == 1616)
                {
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
                    PlayersToRemove.Add(c.EntityID, c);
                    //PlayerList.Remove(c.EntityID);
                    //break;
                }
            }
        }

        public override void WaitForWinner()
        {
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        PlayersToRemove.Add(C.EntityID, C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2) || C.PKTHits == 0)
                        PlayersToRemove.Add(C.EntityID, C);
                }
                foreach (Character C in PlayersToRemove.Values)
                {
                    C.EventBase?.RemovePlayer(C);
                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    RemovedPlayers.Add(C.EntityID, C);
                }
                foreach (Character C in RemovedPlayers.Values)
                {
                    if (PlayersToRemove.ContainsKey(C.EntityID))
                        PlayersToRemove.Remove(C.EntityID);
                }
                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count == 1)
                    break;

                if (!War)
                    break;
            }
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Victim.PKTHits > 1)
            {
                Victim.PKTHits--;
                Victim.MyClient.LocalMessage(2011, "You can only be hitted " + Victim.PKTHits + " more times!");
            }
            else if (Victim.PKTHits == 1)
            {
                Victim.PKTHits--;
                Victim.MyClient.LocalMessage(2011, "You'll be kicked if anyone hits you again! Watch out!");
            }
            else
                Victim.PKTHits = 0;
        }

        public override void End()
        {
            base.End();
        }
    }
}