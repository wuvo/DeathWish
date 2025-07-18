using DeathWish.Client;
using DeathWish.Game.MsgServer;
using System;
using System.Linq;

namespace DeathWish.Game.MsgEvents
{
    class LuckyDraw : Events
    {
        private Random _rand = new Random();
        public LuckyDraw()
        {
            IDEvent = 4;
            EventTitle = "LuckyDraw";
            IDMessage = MsgStaticMessage.Messages.LuckyDraw;
            BaseMap = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = false;
            Duration = 60;
            PotionsAllowed = false;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (Duration <= 0)
                Finish();
            if (DateTime.Now >= DisplayScores.AddSeconds(3))
                DisplayScore();
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values.ToArray())
            {
                player.SendSysMesage($"---------{EventTitle}---------", MsgMessage.ChatMode.FirstRightCorner);
                TimeSpan t = TimeSpan.FromSeconds(Duration);
                player.SendSysMesage($"Time left {t.ToString(@"mm\:ss")}", MsgMessage.ChatMode.ContinueRightCorner);
            }
            if (Duration > 0)
                --Duration;
        }

        public override void End()
        {
            if (PlayerList.Count > 0)
            {
                var index = _rand.Next(PlayerList.Count);
                var winner = PlayerList.ElementAt(index).Value;
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    winner.SendSysMesage("You won the LuckyDraw event!", MsgMessage.ChatMode.System);
                    Reward(winner);
                }
            }
            foreach (var client in PlayerList.Values.ToArray())
                RemovePlayer(client);
            PlayerList.Clear();
            PlayerScores.Clear();
        }
    }
}
