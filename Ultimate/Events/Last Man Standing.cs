using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class LastManStanding : Events
    {
        public LastManStanding()
        {
            EventTitle = "Last Man Standing";
            Duration = 10;
            BaseMap = 1509;
            NoDamage = false;
            DialogID = 10;
        }
        public override void TeleportPlayersToMap()
        {
            foreach (Game.Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.PK);
                X = (ushort)(103 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(109 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(Game.StatusEffectEn.Fly);
                c.StatEff.Remove(Game.StatusEffectEn.Cyclone);
                c.StatEff.Remove(Game.StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
        }
        public override void DisplayScore()
        {
            DisplayScores = System.DateTime.Now;
            foreach (var player in PlayerList.Values)
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));

            Broadcast($"Players left: {PlayerList.Count}", BroadCastLoc.Score, 2);
        }
        public override void Kill(Character Attacker, Character Victim)
        {
            base.Kill(Attacker, Victim);
            RemovePlayer(Victim);
        }
    }
}