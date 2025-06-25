using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NewestCOServer.Features
{
    public class ClassPK
    {
        public static bool ClassPKOn = false;//false
        public static bool SignUp = false;
        public static bool EventByPM = false;
        public static ArrayList PlayerList = new ArrayList();
        public static string PreFix;
        public static string Hour;
        public static string EventTitle = "Class PK Tournament";

        public static void AddPlayer(Game.Character c)
        {
            if (SignUp || EventByPM)
            {
                PlayerList.Add(c);
                c.Teleport(1763, 35, 70);
            }
        }
        public static void RemovePlayer(Game.Character c)
        {
            c.CurHP = c.MaxHP;
            c.Teleport(1002, 430, 378);
            PlayerList.Remove(c);
        }

        public static void CanStart()
        {
            if (PlayerList.Count >= 0)
            {
                if (!ClassPKOn)
                    ClassPKOn = true;
                StartTurnny();
            }
            else
            {
                ClassPKOn = false;
                SendMsg("No one joined the " + EventTitle + " and so there are no winners!");
                PlayerList = new ArrayList();
            }
        }

        public static void StartTurnny()
        {
            new Thread(delegate ()
            {
                WaitForWinner();
                EndTurnny();
            }).Start();

        }
        public static void EndTurnny()
        {
            foreach (Game.Character c in PlayerList)
            {
                SendMsg(c.Name + " has won the " + PreFix + " " + EventTitle + "!");
                c.Silvers += 1500000;
                for (int i = 0; i < 5; i++)
                    c.AddItem(1088000);

                if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
                {
                    c.Top = 3;
                    c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopTrojan);
                }
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Tuesday)
                {
                    c.Top = 5;
                    c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopWarrior);
                }
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Wednesday)
                {
                    c.Top = 4;
                    c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopArcher);
                }
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Thursday)
                {
                    c.Top = 6;
                    c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopFireTaoist);
                }
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Friday)
                {
                    c.Top = 7;
                    c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopWaterTaoist);
                }
                    
            }
            TeleportOut();
            ClassPKOn = false;
            PlayerList = new ArrayList();

        }
        public static void TeleportOut()
        {
            foreach (Game.Character c in PlayerList)
            {
                    c.CurHP = c.MaxHP;
                    c.Teleport(1002, 430, 378);   
            }
        }
        public static void SendMsg(string msg)
        {
            Game.World.SendMsgToAll("[GM]", msg, 2011, 0);
        }
        public static void WaitForWinner()
        {
            ArrayList RemovePlayers = new ArrayList();
            while (PlayerList.Count > 1)
            {
                bool Sanity = false;
                foreach (Game.Character C in PlayerList)
                {
                    if (C.Alive && C.MyClient.Soc.Connected && C.Loc.Map == 1763 && !C.LogOff)
                        Sanity = true;

                    else
                        RemovePlayers.Add(C);
                }
                foreach (Game.Character C in RemovePlayers)
                {
                    RemovePlayer(C);
                    RemovePlayers.Remove(C);
                    Sanity = true;
                    break;
                }
                if (Sanity)
                    continue;
            }
        }
    }
}

