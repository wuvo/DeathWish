namespace NewestCOServer.Features
{
    public class Turnny : PVPEvents
    {
        public Turnny()
        {
            EventTitle = "Last Man Standing";
            Duration = 10;
            MapEvent = 700;
            FFADamage = false;
            NoDamage = false;
        }
        public override void TeleportPlayersToMap()
        {
            foreach (Game.Character c in PlayerList.Values)
            {
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(Game.StatusEffectEn.Fly);
                c.StatEff.Remove(Game.StatusEffectEn.Cyclone);
                c.StatEff.Remove(Game.StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
                PlayerScores.Add(c.EntityID, 0);
            }
        }
    }
}