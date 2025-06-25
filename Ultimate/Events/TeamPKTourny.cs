using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace Ultimate.Features
{
    public class TeamPKTourny
    {
        public static ArrayList Queue70To99 = new ArrayList();
        public static ArrayList Queue100To115 = new ArrayList();
        public static ArrayList Queue116To130 = new ArrayList();
        public static ArrayList TeamsInside70To99 = new ArrayList();
        public static ArrayList TeamsInside100To115 = new ArrayList();
        public static ArrayList TeamsInside116To130 = new ArrayList();
        public static ArrayList Characters70To99 = new ArrayList();
        public static ArrayList Characters100To115 = new ArrayList();
        public static ArrayList Characters116To130 = new ArrayList();
        // public static ArrayList IPs = new ArrayList();
        public static bool EventByPM = false;
        public static DateTime Start;
        public static bool Started;
        public static bool Started70To99;
        public static bool Started100To115;
        public static bool Started116To130;
        public static void StartTourny()
        {
            #region 70-99
            try
            {
                if (Queue70To99.Count >= 2)
                {
                    ArrayList RemoveTeams = new ArrayList();
                    foreach (Team T in Queue70To99)
                    {
                        if (T != null)
                            if (T.Members != null)
                                if (T.Members.Count == 3 || T.Leader.MyClient.PM)
                                {
                                    // ArrayList Classes = new ArrayList();
                                    foreach (Game.Character C in T.Members)
                                    {
                                        if (C.MyClient.Soc.Connected)
                                        {
                                            string IP = C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                            if (T.Leader != null && C.MyTeam.Leader.EntityID == T.Leader.EntityID && C.Level >= 70 && C.Level <= 99/* && !IPs.Contains(IP)*/)
                                            {
                                                // IPs.Add(IP);
                                                continue;
                                            }
                                            else
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                            /*if ((C.Level < 90 || C.Level > 100) && * Classes.Contains(C.Job / 10))
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                            Classes.Add(C.Job / 10); */
                                        }
                                        else
                                        {
                                            RemoveTeams.Add(T);
                                            break;
                                        }
                                    }
                                }
                                else RemoveTeams.Add(T);
                            else RemoveTeams.Add(T);
                        else RemoveTeams.Add(T);
                    }
                    foreach (Team T in RemoveTeams)
                    {
                        if (T != null)
                            if (T.Members != null)
                                T.Leader.MyClient.LocalMessage(2000, "Your team was removed from the Team PK Tourny queue because it didn't meet the requirements!");
                        Queue70To99.Remove(T);
                    }
                    if (Queue70To99.Count >= 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int TakeTeam = Program.Rnd.Next(0, Queue70To99.Count);
                            ushort X, Y;
                            if (i == 0)
                            { X = 123; Y = 89; }
                            else { X = 118; Y = 162; }
                            foreach (Game.Character C in ((Team)(Queue70To99[TakeTeam])).Members)
                            {
                                if (!C.Alive)
                                {
                                    C.CancelProtectTime = false;
                                    C.ProtectTime = DateTime.Now;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = C.MaxHP;
                                    C.Alive = true;
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                }
                                else C.CurHP = C.MaxHP;
                                if (i == 0)
                                    C.TeamWhiteGarment = false;
                                Characters70To99.Add(C);
                                C.Teleport(8001, X, Y);
                                X++;
                                Y++;
                            }
                            TeamsInside70To99.Add(Queue70To99[TakeTeam]);
                            Queue70To99.Remove(Queue70To99[TakeTeam]);
                        }
                        Game.World.SendMsgToAll("TeamPK", "Team PK Tournament started (70 - 99)! Good Luck!", 2005, 0);
                        Start = DateTime.Now;
                        Started = true;
                        Started70To99 = true;
                        CheckEndTourny();
                    }

                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
            #endregion
            #region 100-115
            try
            {
                if (Queue100To115.Count >= 2)
                {
                    ArrayList RemoveTeams = new ArrayList();
                    foreach (Team T in Queue100To115)
                    {
                        if (T != null)
                            if (T.Members != null)
                                if (T.Members.Count == 3)
                                {
                                    // ArrayList Classes = new ArrayList();
                                    foreach (Game.Character C in T.Members)
                                    {
                                        if (C.MyClient.Soc.Connected)
                                        {
                                            string IP = C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                            if (T.Leader != null && C.MyTeam.Leader.EntityID == T.Leader.EntityID && C.MyTeam.Members.Count == 3 && C.Level >= 100 && C.Level <= 115 /*&& !IPs.Contains(IP)*/)
                                            {
                                                //   IPs.Add(IP);
                                                continue;
                                            }
                                            else
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                            /*if ((C.Level < 90 || C.Level > 100) && * Classes.Contains(C.Job / 10))
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                            Classes.Add(C.Job / 10); */

                                        }
                                        else
                                        {
                                            RemoveTeams.Add(T);
                                            break;
                                        }
                                    }
                                }
                                else RemoveTeams.Add(T);
                            else RemoveTeams.Add(T);
                        else RemoveTeams.Add(T);
                    }
                    foreach (Team T in RemoveTeams)
                    {
                        if (T != null)
                            if (T.Members != null)
                                T.Leader.MyClient.LocalMessage(2000, "Your team was removed from the Team PK Tourny queue because it didn't meet the requirements!");
                        Queue100To115.Remove(T);
                    }
                    if (Queue100To115.Count >= 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int TakeTeam = Program.Rnd.Next(0, Queue100To115.Count);
                            ushort X, Y;
                            if (i == 0)
                            { X = 123; Y = 89; }
                            else { X = 118; Y = 162; }
                            foreach (Game.Character C in ((Team)(Queue100To115[TakeTeam])).Members)
                            {
                                if (!C.Alive)
                                {
                                    C.CancelProtectTime = false;
                                    C.ProtectTime = DateTime.Now;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = C.MaxHP;
                                    C.Alive = true;
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                }
                                else C.CurHP = C.MaxHP;
                                if (i == 0)
                                    C.TeamWhiteGarment = false;
                                Characters100To115.Add(C);
                                C.Teleport(8002, X, Y);
                                X++;
                                Y++;
                            }
                            TeamsInside100To115.Add(Queue100To115[TakeTeam]);
                            Queue100To115.Remove(Queue100To115[TakeTeam]);
                        }
                        Game.World.SendMsgToAll("TeamPK", "Team PK Tournament (100 - 115) started! Good Luck!", 2005, 0);
                        Start = DateTime.Now;
                        Started = true;
                        Started100To115 = true;
                    }

                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
            #endregion
            #region 116-130
            try
            {
                if (Queue116To130.Count >= 2)
                {
                    ArrayList RemoveTeams = new ArrayList();
                    foreach (Team T in Queue116To130)
                    {
                        if (T != null)
                            if (T.Members != null)
                                if (T.Members.Count == 3 || T.Leader.MyClient.PM)
                                {
                                    // ArrayList Classes = new ArrayList();
                                    foreach (Game.Character C in T.Members)
                                    {
                                        if (C.MyClient.Soc.Connected)
                                        {
                                            string IP = C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                            if (T.Leader != null && C.MyTeam.Leader.EntityID == T.Leader.EntityID && C.MyTeam.Members.Count == 3 && C.Level >= 116 && C.Level <= 130 /*&& !IPs.Contains(IP)*/)
                                            {
                                                // IPs.Add(IP);
                                                continue;
                                            }
                                            else
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                            /*if ((C.Level < 90 || C.Level > 100) && * Classes.Contains(C.Job / 10))
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                            Classes.Add(C.Job / 10); */

                                        }
                                        else
                                        {
                                            RemoveTeams.Add(T);
                                            break;
                                        }
                                    }
                                }
                                else RemoveTeams.Add(T);
                            else RemoveTeams.Add(T);
                        else RemoveTeams.Add(T);
                    }
                    foreach (Team T in RemoveTeams)
                    {
                        if (T != null)
                            if (T.Members != null)
                                T.Leader.MyClient.LocalMessage(2000, "Your team was removed from the Team PK Tourny queue because it didn't meet the requirements!");
                        Queue116To130.Remove(T);
                    }
                    if (Queue116To130.Count >= 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int TakeTeam = Program.Rnd.Next(0, Queue116To130.Count);
                            ushort X, Y;
                            if (i == 0)
                            { X = 123; Y = 89; }
                            else { X = 118; Y = 162; }
                            foreach (Game.Character C in ((Team)(Queue116To130[TakeTeam])).Members)
                            {
                                if (!C.Alive)
                                {
                                    C.CancelProtectTime = false;
                                    C.ProtectTime = DateTime.Now;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = C.MaxHP;
                                    C.Alive = true;
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                }
                                else C.CurHP = C.MaxHP;
                                if (i == 0)
                                    C.TeamWhiteGarment = false;
                                Characters116To130.Add(C);
                                C.Teleport(8003, X, Y);
                                X++;
                                Y++;
                            }
                            TeamsInside116To130.Add(Queue116To130[TakeTeam]);
                            Queue116To130.Remove(Queue116To130[TakeTeam]);
                        }
                        Game.World.SendMsgToAll("TeamPK", "Team PK Tournament (116 - 130) started! Good Luck!", 2005, 0);
                        Start = DateTime.Now;
                        Started = true;
                        Started116To130 = true;
                    }

                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
            #endregion
        }

        public static void OnFight()
        {
            new Thread(delegate ()
            {
                CheckEndTourny();
            }).Start();
        }

        public static void CheckEndTourny()
        {
            #region 70-99
            try
            {
                if (Started70To99)
                {
                    //Console.WriteLine("Time limit not passed!");
                    ArrayList RemoveTeams = new ArrayList();
                    bool FoundWinner = false;
                    foreach (Team T in TeamsInside70To99)
                    {
                        if (T != null)
                            if (T.Members != null)
                                continue;
                        RemoveTeams.Add(T);
                        // Console.WriteLine("Added to remove some teams!");
                    }
                    foreach (Team T in RemoveTeams)
                        TeamsInside70To99.Remove(T);
                    if (TeamsInside70To99.Count >= 1)
                    {
                        foreach (Team T in TeamsInside70To99)
                        {
                            bool Alive = false;
                            foreach (Game.Character C in T.Members)
                            {
                                if (C.MyClient.Soc.Connected && C.Alive && C.Loc.Map == 8001)
                                {
                                    Alive = true;
                                    T.PKTournyAlive = true;
                                    break;
                                }
                            }
                            if (!Alive)
                                T.PKTournyAlive = false;
                            if (!T.PKTournyAlive || TeamsInside70To99.Count <= 1)
                            {
                                FoundWinner = true;
                                if (TeamsInside70To99.Count == 2)
                                    TeamsInside70To99.Remove(T);
                                break;
                                //  Console.WriteLine("Found Winner!");
                            }

                        }
                        if (FoundWinner)
                        {
                            //  Console.WriteLine("Giving Reward finding team");
                            foreach (Team T in TeamsInside70To99)
                            {
                                //  Console.WriteLine("Searching awarded team");
                                /*  foreach (Game.Character C in T.Members)
                                  {
                                      //if leader....reward
                                      if (C.MyClient.Soc.Connected && C.Loc.Map == 8001)
                                          C.Teleport(1002, 427, 379);
                                  }*/
                                if (T.PKTournyAlive)
                                {
                                    // Console.WriteLine("Giving Reward to the alive team!");
                                    if (Game.World.LowRatedServer)
                                    {
                                        if (MyMath.ChanceSuccess(90))
                                        {
                                            // if (T.Leader.Inventory.Count < 39)
                                            // {
                                            T.Leader.DBScrolls += 5;
                                            T.Leader.MyClient.LocalMessage(2000, "Please check Prize NPC in market in order to pick up your 5 DBSCrolls!");
                                            //  }
                                            /*  else
                                              {
                                                  T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                  Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 2 DBs ");
                                              }*/
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 5 DBSCrolls! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 5 DBSCrolls! Congratulations!", 2011, 0);
                                        }
                                        else
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {


                                            top:
                                                Game.Item I2 = new Game.Item();
                                                I2.UID = (uint)Program.Rnd.Next(10000000);
                                                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;

                                                uint ItemID = 0;
                                                ArrayList From = new ArrayList();
                                                int Type = Program.Rnd.Next(0, 170);
                                                uint Part = 0;
                                                if (Type < 10) Part = 111;
                                                else if (Type < 20) Part = 113;
                                                else if (Type < 30) Part = 114;
                                                else if (Type < 40) Part = 117;
                                                else if (Type < 50) Part = 118;
                                                else if (Type < 60) Part = 120;
                                                else if (Type < 70) Part = 121;
                                                else if (Type < 80) Part = 130;
                                                else if (Type < 90) Part = 131;
                                                else if (Type < 100) Part = 133;
                                                else if (Type < 110) Part = 134;
                                                else if (Type < 120) Part = 141;
                                                else if (Type < 130) Part = 142;
                                                else if (Type < 140) Part = 150;
                                                else if (Type < 150) Part = 151;
                                                else if (Type < 160) Part = 152;
                                                else if (Type < 165) Part = 160;
                                                else if (Type < 170) Part = 900;


                                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                                {
                                                    if (D.LevReq >= 5 && D.LevReq <= 110)
                                                    {
                                                        if (D.LevReq != 0)
                                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                                From.Add(D.ID);
                                                    }
                                                }
                                                if (From != null)
                                                {
                                                    if (From.Count > 0)
                                                    {
                                                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                        ItemID = (uint)From[Tries];
                                                    }
                                                }
                                                if (ItemID != 0)
                                                {
                                                    I2.ID = ItemID;
                                                    if (I2.DBInfo.LevReq != 1)
                                                    {
                                                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                                                        E.QualityChange(Q);
                                                        I2.ID = E.ToID();
                                                    }

                                                    I2.Color = Game.Item.ArmorColor.Orange;


                                                    I2.Soc1 = Game.Item.Gem.EmptySocket;


                                                    I2.MaxDur = I2.DBInfo.Durability;
                                                    I2.CurDur = I2.MaxDur;

                                                    T.Leader.AddItem(I2);

                                                }
                                                else goto top;
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 socketed item! ");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 1 socketed item! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 1 socketed item! Congratulations!", 2011, 0);

                                        }
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(99))
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {
                                                uint Item = 0;
                                                int Type = Program.Rnd.Next(0, 5);
                                                if (Type == 0)
                                                    Item = 700003;
                                                else if (Type == 1)
                                                    Item = 700013;
                                                else if (Type == 2)
                                                    Item = 700033;
                                                else if (Type == 3)
                                                    Item = 700053;
                                                else if (Type == 4)
                                                    Item = 700063;
                                                T.Leader.AddItem(Item);
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 super gem");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 1 Super Gem! Congratulations!", 2005, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 1 Super Gem! Congratulations!", 2011, 0);
                                        }
                                        else
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {


                                            top:
                                                Game.Item I2 = new Game.Item();
                                                I2.UID = (uint)Program.Rnd.Next(10000000);
                                                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;

                                                uint ItemID = 0;
                                                ArrayList From = new ArrayList();
                                                int Type = Program.Rnd.Next(0, 170);
                                                uint Part = 0;
                                                if (Type < 10) Part = 111;
                                                else if (Type < 20) Part = 113;
                                                else if (Type < 30) Part = 114;
                                                else if (Type < 40) Part = 117;
                                                else if (Type < 50) Part = 118;
                                                else if (Type < 60) Part = 120;
                                                else if (Type < 70) Part = 121;
                                                else if (Type < 80) Part = 130;
                                                else if (Type < 90) Part = 131;
                                                else if (Type < 100) Part = 133;
                                                else if (Type < 110) Part = 134;
                                                else if (Type < 120) Part = 141;
                                                else if (Type < 130) Part = 142;
                                                else if (Type < 140) Part = 150;
                                                else if (Type < 150) Part = 151;
                                                else if (Type < 160) Part = 152;
                                                else if (Type < 165) Part = 160;
                                                else if (Type < 170) Part = 900;


                                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                                {
                                                    if (D.LevReq >= 5 && D.LevReq <= 110)
                                                    {
                                                        if (D.LevReq != 0)
                                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                                From.Add(D.ID);
                                                    }
                                                }
                                                if (From != null)
                                                {
                                                    if (From.Count > 0)
                                                    {
                                                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                        ItemID = (uint)From[Tries];
                                                    }
                                                }
                                                if (ItemID != 0)
                                                {
                                                    I2.ID = ItemID;
                                                    if (I2.DBInfo.LevReq != 1)
                                                    {
                                                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                                                        E.QualityChange(Q);
                                                        I2.ID = E.ToID();
                                                    }

                                                    I2.Color = Game.Item.ArmorColor.Orange;


                                                    I2.Soc1 = Game.Item.Gem.EmptySocket;



                                                    I2.MaxDur = I2.DBInfo.Durability;
                                                    I2.CurDur = I2.MaxDur;

                                                    T.Leader.AddItem(I2);

                                                }
                                                else goto top;
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 socketed item! ");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 1 socketed item! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (70-99) and 1 socketed item! Congratulations!", 2011, 0);

                                        }
                                    }
                                }
                                T.PKTournyAlive = false;

                            }
                            foreach (Game.Character C in Characters70To99)
                            {
                                if (C.MyClient.Soc.Connected && C.Loc.Map == 8001)
                                {
                                    C.Teleport(1002, 427, 379);
                                    if (!C.Alive)
                                    {
                                        C.CancelProtectTime = false;
                                        C.ProtectTime = DateTime.Now;
                                        C.Ghost = false;
                                        C.BlueName = false;
                                        C.CurHP = C.MaxHP;
                                        C.Alive = true;
                                        C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                        C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                        C.XPKO = 0;
                                        C.Body = C.Body;
                                        C.Hair = C.Hair;
                                        C.Equips.Send(C.MyClient, false);
                                    }
                                }
                            }
                            Started70To99 = false;
                            Started = false;
                            Characters70To99.Clear();
                            TeamsInside70To99.Clear();
                            Queue70To99.Clear();
                        }
                        else if (DateTime.Now >= Start.AddMinutes(10))
                        {

                            foreach (Game.Character C in Characters70To99)
                            {
                                if (C.MyClient.Soc.Connected && C.Loc.Map == 8001)
                                    C.Teleport(1002, 427, 379);
                            }
                            Started70To99 = false;
                            Started = false;
                            Game.World.SendMsgToAll("TeamPK", "Team PK Tourny (70-99) time limit exceeded (10 minutes). No winners this time!", 2005, 0);
                            Characters70To99.Clear();
                            Queue70To99.Clear();
                            TeamsInside70To99.Clear();
                        }
                    }
                    else
                    {

                        foreach (Game.Character C in Characters70To99)
                        {
                            if (C.MyClient.Soc.Connected && C.Loc.Map == 8001)
                            {
                                C.Teleport(1002, 427, 379);
                                if (!C.Alive)
                                {
                                    C.CancelProtectTime = false;
                                    C.ProtectTime = DateTime.Now;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = C.MaxHP;
                                    C.Alive = true;
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                }
                            }
                        }
                        Started70To99 = false;
                        Started = false;
                        Game.World.SendMsgToAll("TeamPK", "Team PK Tourny (70-99) ended with no winners because no team was found anymore!", 2005, 0);
                        Characters70To99.Clear();
                        Queue70To99.Clear();
                        TeamsInside70To99.Clear();

                    }

                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
            #endregion
            #region 100-115
            try
            {
                if (Started100To115)
                {
                    ArrayList RemoveTeams = new ArrayList();
                    bool FoundWinner = false;
                    foreach (Team T in TeamsInside100To115)
                    {
                        if (T != null)
                            if (T.Members != null)
                                continue;
                        RemoveTeams.Add(T);
                    }
                    foreach (Team T in RemoveTeams)
                        TeamsInside100To115.Remove(T);
                    if (TeamsInside100To115.Count >= 1)
                    {
                        foreach (Team T in TeamsInside100To115)
                        {

                            bool Alive = false;
                            foreach (Game.Character C in T.Members)
                            {
                                if (C.MyClient.Soc.Connected && C.Alive && C.Loc.Map == 8002)
                                {
                                    Alive = true;
                                    T.PKTournyAlive = true;
                                    break;
                                }
                            }
                            if (!Alive)
                                T.PKTournyAlive = false;
                            if (!T.PKTournyAlive || TeamsInside100To115.Count <= 1)
                            {
                                FoundWinner = true;
                                if (TeamsInside100To115.Count == 2)
                                    TeamsInside100To115.Remove(T);
                                break;
                            }

                        }
                        if (FoundWinner)
                        {
                            foreach (Team T in TeamsInside100To115)
                            {

                                /*  foreach (Game.Character C in T.Members)
                                  {
                                      //if leader....reward
                                      if (C.MyClient.Soc.Connected && C.Loc.Map == 8001)
                                          C.Teleport(1002, 427, 379);
                                  }*/
                                if (T.PKTournyAlive)
                                {

                                    if (Game.World.LowRatedServer)
                                    {
                                        if (MyMath.ChanceSuccess(90))
                                        {
                                            T.Leader.DBScrolls += 5;
                                            T.Leader.MyClient.LocalMessage(2000, "Please check Prize NPC in market in order to pick up your 5 DBs!");
                                            /*  if (T.Leader.Inventory.Count < 39)
                                              {
                                                  T.Leader.AddItem(1088000);
                                                  T.Leader.AddItem(1088000);
                                              }
                                              else
                                              {
                                                  T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                  Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 2 DBs ");
                                              }*/
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 5 DragonBallScrolls! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 5 DragonBallScrolls! Congratulations!", 2011, 0);
                                        }
                                        else
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {


                                            top:
                                                Game.Item I2 = new Game.Item();
                                                I2.UID = (uint)Program.Rnd.Next(10000000);
                                                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;

                                                uint ItemID = 0;
                                                ArrayList From = new ArrayList();
                                                int Type = Program.Rnd.Next(0, 170);
                                                uint Part = 0;
                                                if (Type < 10) Part = 111;
                                                else if (Type < 20) Part = 113;
                                                else if (Type < 30) Part = 114;
                                                else if (Type < 40) Part = 117;
                                                else if (Type < 50) Part = 118;
                                                else if (Type < 60) Part = 120;
                                                else if (Type < 70) Part = 121;
                                                else if (Type < 80) Part = 130;
                                                else if (Type < 90) Part = 131;
                                                else if (Type < 100) Part = 133;
                                                else if (Type < 110) Part = 134;
                                                else if (Type < 120) Part = 141;
                                                else if (Type < 130) Part = 142;
                                                else if (Type < 140) Part = 150;
                                                else if (Type < 150) Part = 151;
                                                else if (Type < 160) Part = 152;
                                                else if (Type < 165) Part = 160;
                                                else if (Type < 170) Part = 900;


                                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                                {
                                                    if (D.LevReq >= 5 && D.LevReq <= 110)
                                                    {
                                                        if (D.LevReq != 0)
                                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                                From.Add(D.ID);
                                                    }
                                                }
                                                if (From != null)
                                                {
                                                    if (From.Count > 0)
                                                    {
                                                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                        ItemID = (uint)From[Tries];
                                                    }
                                                }
                                                if (ItemID != 0)
                                                {
                                                    I2.ID = ItemID;
                                                    if (I2.DBInfo.LevReq != 1)
                                                    {
                                                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                                                        E.QualityChange(Q);
                                                        I2.ID = E.ToID();
                                                    }

                                                    I2.Color = Game.Item.ArmorColor.Orange;


                                                    I2.Soc1 = Game.Item.Gem.EmptySocket;


                                                    I2.MaxDur = I2.DBInfo.Durability;
                                                    I2.CurDur = I2.MaxDur;

                                                    T.Leader.AddItem(I2);

                                                }
                                                else goto top;
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 socketed item! ");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 1 socketed item! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 1 socketed item! Congratulations!", 2011, 0);

                                        }
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(99))
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {
                                                uint Item = 0;
                                                int Type = Program.Rnd.Next(0, 5);
                                                if (Type == 0)
                                                    Item = 700003;
                                                else if (Type == 1)
                                                    Item = 700013;
                                                else if (Type == 2)
                                                    Item = 700033;
                                                else if (Type == 3)
                                                    Item = 700053;
                                                else if (Type == 4)
                                                    Item = 700063;
                                                T.Leader.AddItem(Item);
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 super gem");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 1 Super Gem! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 1 Super Gem! Congratulations!", 2011, 0);
                                        }
                                        else
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {


                                            top:
                                                Game.Item I2 = new Game.Item();
                                                I2.UID = (uint)Program.Rnd.Next(10000000);
                                                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;

                                                uint ItemID = 0;
                                                ArrayList From = new ArrayList();
                                                int Type = Program.Rnd.Next(0, 170);
                                                uint Part = 0;
                                                if (Type < 10) Part = 111;
                                                else if (Type < 20) Part = 113;
                                                else if (Type < 30) Part = 114;
                                                else if (Type < 40) Part = 117;
                                                else if (Type < 50) Part = 118;
                                                else if (Type < 60) Part = 120;
                                                else if (Type < 70) Part = 121;
                                                else if (Type < 80) Part = 130;
                                                else if (Type < 90) Part = 131;
                                                else if (Type < 100) Part = 133;
                                                else if (Type < 110) Part = 134;
                                                else if (Type < 120) Part = 141;
                                                else if (Type < 130) Part = 142;
                                                else if (Type < 140) Part = 150;
                                                else if (Type < 150) Part = 151;
                                                else if (Type < 160) Part = 152;
                                                else if (Type < 165) Part = 160;
                                                else if (Type < 170) Part = 900;


                                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                                {
                                                    if (D.LevReq >= 5 && D.LevReq <= 110)
                                                    {
                                                        if (D.LevReq != 0)
                                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                                From.Add(D.ID);
                                                    }
                                                }
                                                if (From != null)
                                                {
                                                    if (From.Count > 0)
                                                    {
                                                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                        ItemID = (uint)From[Tries];
                                                    }
                                                }
                                                if (ItemID != 0)
                                                {
                                                    I2.ID = ItemID;
                                                    if (I2.DBInfo.LevReq != 1)
                                                    {
                                                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                                                        E.QualityChange(Q);
                                                        I2.ID = E.ToID();
                                                    }

                                                    I2.Color = Game.Item.ArmorColor.Orange;


                                                    I2.Soc1 = Game.Item.Gem.EmptySocket;
                                                    I2.Soc2 = Game.Item.Gem.EmptySocket;


                                                    I2.MaxDur = I2.DBInfo.Durability;
                                                    I2.CurDur = I2.MaxDur;

                                                    T.Leader.AddItem(I2);

                                                }
                                                else goto top;
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 2 socketed item! ");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 2 socketed item! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (100-115) and 2 socketed item! Congratulations!", 2011, 0);

                                        }
                                    }
                                }
                                T.PKTournyAlive = false;

                            }
                            foreach (Game.Character C in Characters100To115)
                            {
                                if (C.MyClient.Soc.Connected && C.Loc.Map == 8002)
                                {
                                    C.Teleport(1002, 427, 379);
                                    if (!C.Alive)
                                    {
                                        C.CancelProtectTime = false;
                                        C.ProtectTime = DateTime.Now;
                                        C.Ghost = false;
                                        C.BlueName = false;
                                        C.CurHP = C.MaxHP;
                                        C.Alive = true;
                                        C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                        C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                        C.XPKO = 0;
                                        C.Body = C.Body;
                                        C.Hair = C.Hair;
                                        C.Equips.Send(C.MyClient, false);
                                    }
                                }
                            }
                            Started100To115 = false;
                            Started = false;
                            Characters100To115.Clear();
                            TeamsInside100To115.Clear();
                            Queue100To115.Clear();
                        }
                        else if (DateTime.Now >= Start.AddMinutes(10))
                        {

                            foreach (Game.Character C in Characters100To115)
                            {
                                if (C.MyClient.Soc.Connected && C.Loc.Map == 8002)
                                    C.Teleport(1002, 427, 379);
                            }
                            Started100To115 = false;
                            Started = false;
                            Game.World.SendMsgToAll("TeamPK", "Team PK Tourny (100-115) time limit exceeded (10 minutes). No winners this time!", 2005, 0);
                            Characters100To115.Clear();
                            Queue100To115.Clear();
                            TeamsInside100To115.Clear();
                        }
                    }
                    else
                    {

                        foreach (Game.Character C in Characters100To115)
                        {
                            if (C.MyClient.Soc.Connected && C.Loc.Map == 8002)
                            {
                                C.Teleport(1002, 427, 379);
                                if (!C.Alive)
                                {
                                    C.CancelProtectTime = false;
                                    C.ProtectTime = DateTime.Now;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = C.MaxHP;
                                    C.Alive = true;
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                }
                            }
                        }
                        Started100To115 = false;
                        Started = false;
                        Game.World.SendMsgToAll("TeamPK", "Team PK Tourny (100-115) ended with no winners because no team was found anymore!", 2005, 0);

                        Characters100To115.Clear();
                        Queue100To115.Clear();
                        TeamsInside100To115.Clear();

                    }

                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
            #endregion
            #region 116-130
            try
            {
                if (Started116To130)
                {
                    ArrayList RemoveTeams = new ArrayList();
                    bool FoundWinner = false;
                    foreach (Team T in TeamsInside116To130)
                    {
                        if (T != null)
                            if (T.Members != null)
                                continue;
                        RemoveTeams.Add(T);
                    }
                    foreach (Team T in RemoveTeams)
                        TeamsInside116To130.Remove(T);
                    if (TeamsInside116To130.Count >= 1)
                    {
                        foreach (Team T in TeamsInside116To130)
                        {

                            bool Alive = false;
                            foreach (Game.Character C in T.Members)
                            {
                                if (C.MyClient.Soc.Connected && C.Alive && C.Loc.Map == 8003)
                                {
                                    Alive = true;
                                    T.PKTournyAlive = true;
                                    break;
                                }
                            }
                            if (!Alive)
                                T.PKTournyAlive = false;
                            if (!T.PKTournyAlive || TeamsInside116To130.Count <= 1)
                            {
                                FoundWinner = true;
                                if (TeamsInside116To130.Count == 2)
                                    TeamsInside116To130.Remove(T);
                                break;
                            }

                        }
                        if (FoundWinner)
                        {
                            foreach (Team T in TeamsInside116To130)
                            {

                                /*  foreach (Game.Character C in T.Members)
                                  {
                                      //if leader....reward
                                      if (C.MyClient.Soc.Connected && C.Loc.Map == 8001)
                                          C.Teleport(1002, 427, 379);
                                  }*/
                                {

                                    if (Game.World.LowRatedServer)
                                    {
                                        if (MyMath.ChanceSuccess(90))
                                        {
                                            T.Leader.DBScrolls += 5;
                                            T.Leader.MyClient.LocalMessage(2000, "Please check Prize NPC in market in order to pick up your 5 DBScrolls!");
                                            /*  if (T.Leader.Inventory.Count < 39)
                                              {
                                                  T.Leader.AddItem(1088000);
                                                  T.Leader.AddItem(1088000);
                                              }
                                              else
                                              {
                                                  T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                  Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 2 DBs ");
                                              }*/
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 5 DBScrolls! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 5 DBScrolls! Congratulations!", 2011, 0);
                                        }
                                        else
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {


                                            top:
                                                Game.Item I2 = new Game.Item();
                                                I2.UID = (uint)Program.Rnd.Next(10000000);
                                                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;

                                                uint ItemID = 0;
                                                ArrayList From = new ArrayList();
                                                int Type = Program.Rnd.Next(0, 170);
                                                uint Part = 0;
                                                if (Type < 10) Part = 111;
                                                else if (Type < 20) Part = 113;
                                                else if (Type < 30) Part = 114;
                                                else if (Type < 40) Part = 117;
                                                else if (Type < 50) Part = 118;
                                                else if (Type < 60) Part = 120;
                                                else if (Type < 70) Part = 121;
                                                else if (Type < 80) Part = 130;
                                                else if (Type < 90) Part = 131;
                                                else if (Type < 100) Part = 133;
                                                else if (Type < 110) Part = 134;
                                                else if (Type < 120) Part = 141;
                                                else if (Type < 130) Part = 142;
                                                else if (Type < 140) Part = 150;
                                                else if (Type < 150) Part = 151;
                                                else if (Type < 160) Part = 152;
                                                else if (Type < 165) Part = 160;
                                                else if (Type < 170) Part = 900;


                                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                                {
                                                    if (D.LevReq >= 5 && D.LevReq <= 110)
                                                    {
                                                        if (D.LevReq != 0)
                                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                                From.Add(D.ID);
                                                    }
                                                }
                                                if (From != null)
                                                {
                                                    if (From.Count > 0)
                                                    {
                                                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                        ItemID = (uint)From[Tries];
                                                    }
                                                }
                                                if (ItemID != 0)
                                                {
                                                    I2.ID = ItemID;
                                                    if (I2.DBInfo.LevReq != 1)
                                                    {
                                                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                                                        E.QualityChange(Q);
                                                        I2.ID = E.ToID();
                                                    }

                                                    I2.Color = Game.Item.ArmorColor.Orange;


                                                    I2.Soc1 = Game.Item.Gem.EmptySocket;


                                                    I2.MaxDur = I2.DBInfo.Durability;
                                                    I2.CurDur = I2.MaxDur;

                                                    T.Leader.AddItem(I2);

                                                }
                                                else goto top;
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 socketed item! ");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 1 socketed item! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 1 socketed item! Congratulations!", 2011, 0);

                                        }
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(99))
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {
                                                uint Item = 0;
                                                int Type = Program.Rnd.Next(0, 5);
                                                if (Type == 0)
                                                    Item = 700003;
                                                else if (Type == 1)
                                                    Item = 700013;
                                                else if (Type == 2)
                                                    Item = 700033;
                                                else if (Type == 3)
                                                    Item = 700053;
                                                else if (Type == 4)
                                                    Item = 700063;
                                                T.Leader.AddItem(Item);
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 super gem");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 1 Super Gem! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 1 Super Gem! Congratulations!", 2011, 0);
                                        }
                                        else
                                        {
                                            if (T.Leader.Inventory.Count < 40)
                                            {


                                            top:
                                                Game.Item I2 = new Game.Item();
                                                I2.UID = (uint)Program.Rnd.Next(10000000);
                                                Game.Item.ItemQuality Q = Game.Item.ItemQuality.Normal;

                                                uint ItemID = 0;
                                                ArrayList From = new ArrayList();
                                                int Type = Program.Rnd.Next(0, 170);
                                                uint Part = 0;
                                                if (Type < 10) Part = 111;
                                                else if (Type < 20) Part = 113;
                                                else if (Type < 30) Part = 114;
                                                else if (Type < 40) Part = 117;
                                                else if (Type < 50) Part = 118;
                                                else if (Type < 60) Part = 120;
                                                else if (Type < 70) Part = 121;
                                                else if (Type < 80) Part = 130;
                                                else if (Type < 90) Part = 131;
                                                else if (Type < 100) Part = 133;
                                                else if (Type < 110) Part = 134;
                                                else if (Type < 120) Part = 141;
                                                else if (Type < 130) Part = 142;
                                                else if (Type < 140) Part = 150;
                                                else if (Type < 150) Part = 151;
                                                else if (Type < 160) Part = 152;
                                                else if (Type < 165) Part = 160;
                                                else if (Type < 170) Part = 900;


                                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                                {
                                                    if (D.LevReq >= 5 && D.LevReq <= 110)
                                                    {
                                                        if (D.LevReq != 0)
                                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                                From.Add(D.ID);
                                                    }
                                                }
                                                if (From != null)
                                                {
                                                    if (From.Count > 0)
                                                    {
                                                        byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                        ItemID = (uint)From[Tries];
                                                    }
                                                }
                                                if (ItemID != 0)
                                                {
                                                    I2.ID = ItemID;
                                                    if (I2.DBInfo.LevReq != 1)
                                                    {
                                                        Game.ItemIDManipulation E = new Game.ItemIDManipulation(ItemID);
                                                        E.QualityChange(Q);
                                                        I2.ID = E.ToID();
                                                    }

                                                    I2.Color = Game.Item.ArmorColor.Orange;


                                                    I2.Soc1 = Game.Item.Gem.EmptySocket;

                                                    I2.MaxDur = I2.DBInfo.Durability;
                                                    I2.CurDur = I2.MaxDur;

                                                    T.Leader.AddItem(I2);

                                                }
                                                else goto top;
                                            }
                                            else
                                            {
                                                T.Leader.MyClient.LocalMessage(2000, "Your inventory was full! Talk to Ultimate[PM] or check the forum to receive your reward!");
                                                Program.WriteCmds(T.Leader.Name + " did not receive Team PK reward: 1 socketed item! ");
                                            }
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 1 socketed item! Congratulations!", 2000, 0);
                                            Game.World.SendMsgToAll("TeamPK", "The Team whose leader is " + T.Leader.Name + " has won the Team PK Tournament (116-130) and 1 socketed item! Congratulations!", 2011, 0);

                                        }
                                    }
                                }
                                T.PKTournyAlive = false;

                            }
                            foreach (Game.Character C in Characters116To130)
                            {
                                if (C.MyClient.Soc.Connected && C.Loc.Map == 8003)
                                {
                                    C.Teleport(1002, 427, 379);
                                    if (!C.Alive)
                                    {
                                        C.CancelProtectTime = false;
                                        C.ProtectTime = DateTime.Now;
                                        C.Ghost = false;
                                        C.BlueName = false;
                                        C.CurHP = C.MaxHP;
                                        C.Alive = true;
                                        C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                        C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                        C.XPKO = 0;
                                        C.Body = C.Body;
                                        C.Hair = C.Hair;
                                        C.Equips.Send(C.MyClient, false);
                                    }
                                }
                            }
                            Started116To130 = false;
                            Started = false;
                            Characters116To130.Clear();
                            TeamsInside116To130.Clear();
                            Queue116To130.Clear();
                        }
                        else if (DateTime.Now >= Start.AddMinutes(10))
                        {

                            foreach (Game.Character C in Characters116To130)
                            {
                                if (C.MyClient.Soc.Connected && C.Loc.Map == 8003)
                                    C.Teleport(1002, 427, 379);
                            }
                            Started116To130 = false;
                            Started = false;
                            Game.World.SendMsgToAll("TeamPK", "Team PK Tourny (116-130) time limit exceeded (10 minutes). No winners this time!", 2005, 0);
                            Characters116To130.Clear();
                            Queue116To130.Clear();
                            TeamsInside116To130.Clear();
                        }
                    }
                    else
                    {

                        foreach (Game.Character C in Characters116To130)
                        {
                            if (C.MyClient.Soc.Connected && C.Loc.Map == 8003)
                            {
                                C.Teleport(1002, 427, 379);
                                if (!C.Alive)
                                {
                                    C.CancelProtectTime = false;
                                    C.ProtectTime = DateTime.Now;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = C.MaxHP;
                                    C.Alive = true;
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.Dead);
                                    C.StatEff.Remove(Ultimate.Game.StatusEffectEn.BlueName);
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                }
                            }
                        }
                        Started116To130 = false;
                        Started = false;
                        Game.World.SendMsgToAll("TeamPK", "Team PK Tourny (116-130) ended with no winners because no team was found anymore!", 2005, 0);
                        Characters116To130.Clear();
                        Queue116To130.Clear();
                        TeamsInside116To130.Clear();
                    }

                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
            #endregion
            //IPs.Clear();
        }
    }
}
