using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NewestCOServer.Game;

namespace NewestCOServer.Features
{
    public class WeeklyPKTournament
    {
        public static bool War = false;//false
        public static bool SignUp = false;
        public static bool EventByPM = false;
        public uint MapEvent = 1508;
        public static Dictionary<uint, Character> PlayerList = new Dictionary<uint, Character>();
        public static string PreFix;
        public static string Hour;
        public static string EventTitle = "Weekly PK Tournament";
        public DateTime EndTime;
        private Thread PkThread;
        private int _countdown;

        public static void AddPlayer(Game.Character c)
        {
            if (SignUp || EventByPM)
            {
                ushort X, Y;

                X = (ushort)Program.Rnd.Next(108, 141);
                Y = (ushort)Program.Rnd.Next(126, 159);
                if (!PlayerList.ContainsKey(c.EntityID))
                {
                    PlayerList.Add(c.EntityID, c);
                    c.Teleport(1508, X, Y);
                }
                else
                    c.Teleport(1508, X, Y);
            }
        }

        public void RemovePlayer(Game.Character c)
        {
            if (!c.Alive)
            {
                #region Revive
                c.Protection = false;
                c.Action = (byte)100;
                c.Stamina = (byte)100;
                c.Ghost = false;
                c.BlueName = false;
                c.CurHP = c.MaxHP;
                if ((int)c.MaxMP > 1)
                    c.CurMP = c.MaxMP;
                c.Alive = true;
                c.StatEff.Remove(StatusEffectEn.Dead);
                c.StatEff.Remove(StatusEffectEn.BlueName);
                c.Body = c.Body;
                c.Hair = c.Hair;
                c.XPKO = (byte)0;
                #endregion
            }
            c.Teleport(1002, 430, 378);
            if (PlayerList.ContainsKey(c.EntityID))
                PlayerList.Remove(c.EntityID);
            Database.SaveCharacter(c, c.MyClient.AuthInfo.Account);
        }

        public void BeginTournament()
        {
            PlayerList.Clear();
            _countdown = 300;
            PkThread = new Thread((ThreadStart)(() =>
            {
                StartTournament();
                WaitForWinner();
                Finish();
            }));
            PkThread.IsBackground = true;
            PkThread.Start();
        }

        public void StartTournament()
        {
            SignUp = true;
            World.SendMsgToAll("SYSTEM", EventTitle + " starts at " + Hour + ":00! Prepare to fight!", 2011, 0);
            World.SendMsgToAll("SYSTEM", EventTitle + " starts at " + Hour + ":00! Prepare to fight!", 2005, 0);
            World.SendMsgToAll("SYSTEM", EventTitle + " starts at " + Hour + ":00! Prepare to fight!", 2000, 0);
            while (_countdown > 0)
            {
                if (_countdown % 60 == 0)
                    World.SendMsgToAll("SYSTEM", EventTitle + " starts at " + Hour + ":00! Prepare to fight!", 2011, 0);

                foreach (Character C in PlayerList.Values.ToList())
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        RemovePlayer(C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
                        RemovePlayer(C);
                }
                
                --_countdown;
                Thread.Sleep(1000);
            }
            if (!CanStart())
            {
                SignUp = false;
                EventByPM = false;
                World.SendMsgToAll("SYSTEM", "No one joined the " + EventTitle + "! There are no winners in this round!", 2000, 0);
                PlayerList.Clear();
                PkThread.Abort();
                return;
            }
        }

        public bool CanStart()
        {
            return PlayerList.Count >= 1;
        }

        public void SendMsg(string msg)
        {
            Game.World.SendMsgToAll("[GM]", msg, 2011, 0);
        }

        public void WaitForWinner()
        {
            War = true;
            EndTime = DateTime.UtcNow.AddMinutes(15);
            while (true)
            {
                foreach (Character C in PlayerList.Values.ToList())
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        RemovePlayer(C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
                        RemovePlayer(C);
                }
                
                if (DateTime.UtcNow >= EndTime)
                    break;
                else if (PlayerList.Count == 1)
                    break;
                else if (!War)
                    break;
            }
        }

        public void Finish()
        {
            if (PlayerList.Count > 1)
            {
                SendMsg("15 minutes have passed and no one managed to win the " + EventTitle + "! There are no winners in this round!");
                TeleportOut();
                War = false;
                SignUp = false;
                EventByPM = false;
                PlayerList.Clear();
                PkThread.Abort();
                return;
            }
            else if (PlayerList.Count == 1)
            {
                foreach (Game.Character c in PlayerList.Values)
                {
                    SendMsg(c.Name + " has won the " + PreFix + " " + EventTitle + "!");
                    c.Silvers += 5000000;
                    int j = Program.Rnd.Next(0, 8);
                    uint gem = (uint)(700003 + (j * 10));
                    c.AddItem(gem);
                    c.Top = 9;
                    c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.WeeklyPKChampion);
                }
                TeleportOut();
                War = false;
                SignUp = false;
                EventByPM = false;
                PlayerList.Clear();
                PkThread.Abort();
                return;
            }
        }

        public void TeleportOut()
        {
            foreach (Game.Character c in PlayerList.Values)
                c.Teleport(1002, 430, 378);
        }
    }
}