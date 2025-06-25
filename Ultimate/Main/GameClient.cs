using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using OpenSSL;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;

namespace Ultimate.Main
{
    public class GameClient
    {

        public int NextCaptcha = 5;
        public DateTime LastSuccessCaptcha = DateTime.Now;
        public int MobsKilled = 0;
        internal void SolveCaptcha()
        {
            MobsKilled = 0;
            WaitingKillCaptcha = false;
            KillCountCaptcha = "";
            LastSuccessCaptcha = DateTime.Now;
            NextCaptcha = Program.Rnd.Next(6, 15);
        }
        public DateTime LastAttack;
        public DateTime KillCountCaptchaStamp;
        public bool WaitingKillCaptcha;
        public string KillCountCaptcha;
        public Game.Character MyChar;
        public AuthWorker.AuthInfo AuthInfo;
        public GameCrypto Crypto;

        public DH KeyExchance;
        public byte[] NewServerIV;
        public byte[] NewClientIV;
        public bool SetBF = true;

        public Socket Soc;
        public uint MessageID = 0;
        public bool DoneLoading = false;
        public uint DialogNPC = 0;
        public NPCs.NPCBase CurrentNPC;
        public bool Paid = false;
        public bool Agreed = false;
        public bool ER = false;
        public bool DR = false;

        public bool SpawnOnHold = false;
        public ushort SpawnXStart = 0;
        public ushort SpawnYStart = 0;

        public bool LoginDataSent = false;
        internal DateTime LastCheatPacket;
        internal DateTime ReceivePing;
        internal int ReceiveTest;
        internal DateTime LastSuspect;
        internal string SignatureKey;
        internal string MacAddress;

        //public List<Packet> PacketGroup;
        //private MemoryStream MainStream;
        //private BinaryWriter StreamWriter;
        public bool PM
        {
            get { return (AuthInfo.Status.Contains("PM")); }
        }
        public bool GM
        {
            get { return (AuthInfo.Status.Contains("PM") || AuthInfo.Status.Contains("GM")); }
        }
        public GameClient()
        {
            try
            {
                //MainStream = new MemoryStream();
                // StreamWriter = new BinaryWriter(MainStream);
                // PacketGroup = new List<Packet>();
                //string Blowfish = new IniFile(@"C:\OldCODB\Config.ini").ReadString("Blowfish", "Key");
                Crypto = new GameCrypto(ASCIIEncoding.ASCII.GetBytes(Game.World.Blowfish));//kjdsahdasidjasdk  kdjuhboalskdirks    afzgjmusijqkleab    bjofwelrjbkbqads
                //ljdvhpidsasalcjs 
                //jkasdfchixjfkjas

                //jhdyijbjayqbhsjy   4/26/2013

               // qkndiklssdgewrna  ultima
                  
                KeyExchance = new DH();
                KeyExchance.G = 5;
                KeyExchance.P = OpenSSL.BigNumber.FromHexString("A320A85EDD79171C341459E94807D71D39BB3B3F3B5161CA84894F3AC3FC7FEC317A2DDEC83B66D30C29261C6492643061AECFCF4A051816D7C359A6A7B7D8FB\0");
                KeyExchance.GenerateKeys();

                NewServerIV = GenerateIV();
                NewClientIV = GenerateIV();
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        //~GameClient()
        //{
        //    KeyExchance.Dispose();
        //}
        byte[] GenerateIV()
        {
            System.Random Rnd = new System.Random();
            byte[] iv = new byte[8];
            for (byte i = 0; i < 8; i++)
                iv[i] = (byte)Rnd.Next(byte.MaxValue);
            return iv;
        }
        public void LocalMessage(ushort Type, string Message)
        {
            AddSend(Packets.ChatMessage(MessageID, "SYSTEM", MyChar.Name, Message, Type, MyChar.Mesh));
        }
        public void Message(ushort Type, string Message)
        {
            foreach (Game.Character G in Game.World.H_Chars.Values)
            {
                G.MyClient.AddSend(Packets.ChatMessage(MessageID, "SYSTEM", MyChar.Name, Message, Type, MyChar.Mesh));
            }
        }
        /* public unsafe void EndSend()
        {
            if (this != null)
            {
                if (PacketGroup.Count > 0)
                {
                    Monitor.Enter(this);
                    try
                    {
                        List<Packet> Packets = new List<Packet>();
                        for (int i = 0; i < PacketGroup.Count; i++)
                            Packets.Add(PacketGroup[i]);
                        int TotalLength = 0;
                        foreach (byte[] P in Packets)
                            if (P != null)
                                if (P.Length > 0)
                                    TotalLength += P.Length;
                        byte[] Data = new byte[TotalLength];
                        int Pos = 0;
                        lock (Packets)
                        {
                            foreach (byte[] P in Packets)
                            {
                                if (P != null)
                                {
                                    if (P.Length > 0)
                                    {
                                        Buffer.BlockCopy(P, 0, Data, Pos, P.Length);
                                        Pos += P.Length;
                                    }
                                }
                            }
                            try
                            {
                                Crypto.Encrypt(Data);
                                Soc.Send(Data);
                            }
                            catch { }
                        }
                        PacketGroup = new List<Packet>();
                    }
                    catch (Exception c) { PacketGroup = new List<Packet>(); Game.World.DebugAdd +=c.ToString()); }
                    Monitor.Exit(this);
                }
            }
        }
        */
        /* public unsafe void AddSend(COPacket Packet)
         {
             try
             {
                 byte[] P = Packet.Get;
                 fixed (byte* p = P)
                 {
                     string TQServer = "TQServer";
                     for (int i = 0; i < 8; i++)
                         *(p + i + P.Length - 8) = Convert.ToByte(TQServer[i]);
                 }
                 StreamWriter.Write(P);
                 //Crypto.Encrypt(P);
                 //Soc.Send(P);
             }
             catch (Exception Exc) { Game.World.DebugAdd +=Exc.ToString()); }
         } */
        /*  public unsafe void AddSend(byte[] Packet)
          {
              byte[] P = new byte[Packet.Length];
              Packet.CopyTo(P,0);
              fixed (byte* p = P)
              {
                  string TQServer = "TQServer";
                  for (int i = 0; i < 8; i++)
                      *(p + i + P.Length - 8) = Convert.ToByte(TQServer[i]);
              }
              if (!Robot)
              {
                  PacketGroup.Add(P);
                  //Crypto.Encrypt(P);
                  //Soc.Send(P);
              }
          }*/
#warning public unsafe void - Derek changes
        public void AddSend(COPacket Packet)
        {
            try
            {
                AddSend(Packet.Get);
            }
            catch (Exception e) { Game.World.ExcAdd += e.ToString() + "\r\n"; } 
        }
        /*  public unsafe void AddSend(byte[] Packet)
          {
              try
              {
                  // if (Soc.Connected)
                  // {

                  /*  if (Packet.Length > 1024)
                    {
                        Soc.Disconnect(false);
                        return;
                    }*/
        /* if (Soc.Connected && !Game.World.Exit)
         {

             byte[] P;
             if (Packet.Length > 1024)
             {
                 P = new byte[1024];
             }
             else
             {
                 P = new byte[Packet.Length];
             }
             int j = 0;
             for (int i = 0; i < Packet.Length; i++)
             {

                 P[j] = Packet[i];
                 j++;

                 if ((i % 1024 == 0 && i > 0) || i == (Packet.Length - 1))
                 {
                     Console.WriteLine("P.length: " + P.Length);
                     fixed (byte* p = P)
                     {
                         string TQServer = "TQServer";
                         for (int m = 0; m < 8; m++)
                             *(p + m + P.Length - 8) = Convert.ToByte(TQServer[m]);
                     }
                     // PacketGroup.Add(P); 


                     if (Monitor.TryEnter(Crypto, 10))
                     {
                         Crypto.Encrypt(P);
                         Soc.Send(P);
                         Monitor.Exit(Crypto);
                         Console.WriteLine("Sent");
                     }
                     Console.WriteLine("Continued");
                     j = 0;
                     if (Packet.Length - i > 1024)
                         P = new byte[1024];
                     else
                         P = new byte[Packet.Length - i];
                 }
             }


         }
         //}
     }
     catch { }//(Exception Exc) { Console.WriteLine(Exc.ToString()); }
 }  */
#warning public unsafe void - Derek changes
        public void AddSend(byte[] Packet)
       {
           try
           {
               // if (Soc.Connected)
               // {
               
               if (Packet.Length > 1024)
               {
                   if (Packet.Length >= 2048)
                        if (Soc.Connected)
                            Soc.Disconnect(false);
                   Game.World.ExcAdd += AuthInfo.Character + " tried to receive packet length : " + Packet.Length + "\r\n";
                   return;
               }
               if (Soc != null)
               if (Soc.Connected && !Game.World.Exit)
               {

                   byte[] P = new byte[Packet.Length];
                   Packet.CopyTo(P, 0);
                    string TQServer = "TQServer";
                    for (int i = 0; i < 8; i++)
                        P[i + P.Length - 8] = Convert.ToByte(TQServer[i]);
                    /* unsafe - Joao 27/07/2016
                   fixed (byte* p = P)
                   {
                       string TQServer = "TQServer";
                       for (int i = 0; i < 8; i++)
                           *(p + i + P.Length - 8) = Convert.ToByte(TQServer[i]);
                   } */
                    // PacketGroup.Add(P); 


                    if (Monitor.TryEnter(Crypto, 10))
                   {
                       Crypto.Encrypt(P);
                       Soc.Send(P);
                       Monitor.Exit(Crypto);
                   }
               }
               //}
           }
           catch { }// (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
       } 
        public void LogOff()
        {
            if (MyChar != null)
            {
                if (Game.World.H_Chars.ContainsKey(MyChar.EntityID) || Game.World.H_Clients.ContainsKey(MyChar.EntityID))
                {
                    if (MyChar.LogOff)
                        return;
                    try
                    {
                        //try
                        //{
                        // Game.World.Action(MyChar, Packets.GeneralData(MyChar.EntityID, 0, 0, 0, 135).Get);


                        if (MyChar != null)
                        {
                            MyChar.LogOff = true;
                            if (MyChar.Level < 130)
                                Game.World.InfoAdd += MyChar.Name + " was level: " + MyChar.Level + " PC: " + (MyChar.Experience * 100) / Database.LevelExp[MyChar.Level] + "\r\n";
                            else
                                Game.World.InfoAdd += MyChar.Name + " was level: " + MyChar.Level + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had job: " + MyChar.Job + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had silvers: " + MyChar.Silvers + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had WH silvers: " + MyChar.WHSilvers + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had top gear: " + MyChar.Equips.HeadGear.UID + "~" + MyChar.Equips.HeadGear.ID + "~" + MyChar.Equips.HeadGear.Plus + "~" + MyChar.Equips.HeadGear.Bless + "~" + MyChar.Equips.HeadGear.Enchant + "~" + (byte)MyChar.Equips.HeadGear.Soc1 + "~" + (byte)MyChar.Equips.HeadGear.Soc2 + "~" + MyChar.Equips.HeadGear.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had necklace: " + MyChar.Equips.Necklace.UID + "~" + MyChar.Equips.Necklace.ID + "~" + MyChar.Equips.Necklace.Plus + "~" + MyChar.Equips.Necklace.Bless + "~" + MyChar.Equips.Necklace.Enchant + "~" + (byte)MyChar.Equips.Necklace.Soc1 + "~" + (byte)MyChar.Equips.Necklace.Soc2 + "~" + MyChar.Equips.Necklace.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had ring: " + MyChar.Equips.Ring.UID + "~" + MyChar.Equips.Ring.ID + "~" + MyChar.Equips.Ring.Plus + "~" + MyChar.Equips.Ring.Bless + "~" + MyChar.Equips.Ring.Enchant + "~" + (byte)MyChar.Equips.Ring.Soc1 + "~" + (byte)MyChar.Equips.Ring.Soc2 + "~" + MyChar.Equips.Ring.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had right hand: " + MyChar.Equips.RightHand.UID + "~" + MyChar.Equips.RightHand.ID + "~" + MyChar.Equips.RightHand.Plus + "~" + MyChar.Equips.RightHand.Bless + "~" + MyChar.Equips.RightHand.Enchant + "~" + (byte)MyChar.Equips.RightHand.Soc1 + "~" + (byte)MyChar.Equips.RightHand.Soc2 + "~" + MyChar.Equips.RightHand.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had left hand: " + MyChar.Equips.LeftHand.UID + "~" + MyChar.Equips.LeftHand.ID + "~" + MyChar.Equips.LeftHand.Plus + "~" + MyChar.Equips.LeftHand.Bless + "~" + MyChar.Equips.LeftHand.Enchant + "~" + (byte)MyChar.Equips.LeftHand.Soc1 + "~" + (byte)MyChar.Equips.LeftHand.Soc2 + "~" + MyChar.Equips.LeftHand.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had armor: " + MyChar.Equips.Armor.UID + "~" + MyChar.Equips.Armor.ID + "~" + MyChar.Equips.Armor.Plus + "~" + MyChar.Equips.Armor.Bless + "~" + MyChar.Equips.Armor.Enchant + "~" + (byte)MyChar.Equips.Armor.Soc1 + "~" + (byte)MyChar.Equips.Armor.Soc2 + "~" + MyChar.Equips.Armor.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had boots: " + MyChar.Equips.Boots.UID + "~" + MyChar.Equips.Boots.ID + "~" + MyChar.Equips.Boots.Plus + "~" + MyChar.Equips.Boots.Bless + "~" + MyChar.Equips.Boots.Enchant + "~" + (byte)MyChar.Equips.Boots.Soc1 + "~" + (byte)MyChar.Equips.Boots.Soc2 + "~" + MyChar.Equips.Boots.Progress + "\r\n";
                            Game.World.InfoAdd += MyChar.Name + " had garment: " + MyChar.Equips.Garment.UID + "~" + MyChar.Equips.Garment.ID + "\r\n";
                            string Profs = "";
                            string Skills = "";
                            foreach (Game.Skill S in MyChar.Skills.Values)
                                Skills += S.ID + "~" + S.Lvl + "~" + S.Exp + "  ";
                            if (Skills != "")
                                Game.World.InfoAdd += MyChar.Name + " had Skills: " + Skills + "\r\n";
                            foreach (Game.Prof P in MyChar.Profs.Values)
                                Profs += P.ID + "~" + P.Lvl + "~" + P.Exp + "  ";
                            if (Profs != "")
                                Game.World.InfoAdd += MyChar.Name + " had Profs: " + Profs + "\r\n";
                            if (MyChar.VotePoints > 0)
                                Game.World.InfoAdd += MyChar.Name + " had Vote Points: " + MyChar.VotePoints + "\r\n";
                            if (MyChar.DoubleExpLeft > 0)
                                Game.World.InfoAdd += MyChar.Name + " had DoubleExp: " + MyChar.DoubleExpLeft + "\r\n";
                            if (MyChar.VipLevel != 0)
                                Game.World.InfoAdd += MyChar.Name + " had VIPLevel: " + MyChar.VipLevel + " days: " + MyChar.VIPDays + "\r\n";
                            if (MyChar.VP > 0)
                                Game.World.InfoAdd += MyChar.Name + " had " + MyChar.VP + " VPS. \r\n";
                            if (MyChar.Reborns > 0)
                            {
                                Game.World.InfoAdd += MyChar.Name + " was class " + MyChar.PreviousJob1 + " before reborning. \r\n";
                            }
                            string Items = "";
                            foreach (Game.Item I in MyChar.Inventory)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had items in Inventory: " + Items + "\r\n";
                            Items = "MA WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.MAWarehouse)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                            Items = "MA2 WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.MAWarehouse2)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                            Items = "TC WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.TCWarehouse)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                            Items = "PC WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.PCWarehouse)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                            Items = "AC WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.ACWarehouse)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                            Items = "DC WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.DCWarehouse)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                            Items = "BI WH: ";
                            foreach (Game.Item I in MyChar.Warehouses.BIWarehouse)
                                Items += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                            Game.World.InfoAdd += MyChar.Name + " had " + Items + "\r\n";
                        }
                        // }
                        //catch { }
                    }
                    catch (Exception E) { Game.World.ExcAdd += E.ToString() + "\r\n"; }

                    try
                    {
                        /* Game.Buff B = MyChar.BuffOf(Ultimate.Features.SkillsClass.ExtraEffect.Transform);
                         if (!MyChar.BDelete.Contains(B))
                             MyChar.BDelete.Add(B);*/
                        // MyChar.BDelete = MyChar.Buffs;
                        if (MyChar != null)
                            if (MyChar.EventBase != null)
                                MyChar.EventBase.RemovePlayer(MyChar);
                        if (MyChar != null)
                            if (MyChar.Arena != null)
                                if (MyChar.Loc.Map == MyChar.Arena.MapID)
                                    MyChar.Arena.RemovePlayer(MyChar);
                        if (MyChar != null)
                        {
                            if (MyChar.ArenaQualifier != null)
                            {
                                if (Features.ArenaQualifier.PlayersInWaiting.ContainsKey(MyChar.EntityID))
                                    Features.ArenaQualifier.PlayersInWaiting.Remove(MyChar.EntityID);
                                MyChar.ArenaQualifier.RemovePlayer(MyChar);
                            }
                            else if (Features.ArenaQualifier.PlayersInWaiting.ContainsKey(MyChar.EntityID))
                                    Features.ArenaQualifier.PlayersInWaiting.Remove(MyChar.EntityID);
                        }
                        if (MyChar != null)
                            if (MyChar.MyCompanion != null)
                                MyChar.MyCompanion.Dissappear();
                        if (MyChar.MyTeam != null)
                        {
                            if (MyChar.TeamLeader)
                                MyChar.MyTeam.Dismiss(MyChar);
                            else
                                MyChar.MyTeam.Leaves(MyChar);
                        }


                        if (MyChar != null)
                        {
                            if (MyChar.Loc.Map >= 8000 || MyChar.Loc.Map == 1090)
                            {
                                MyChar.Loc.Map = 1002;
                                MyChar.Loc.X = 430;
                                MyChar.Loc.Y = 378;
                            }
                            else if (MyChar.Loc.Map == 1039 || MyChar.Loc.Map == 1036 || MyChar.Loc.Map == 1051)
                            {
                                if (Database.DefaultCoords.ContainsKey(MyChar.Loc.PreviousMap))
                                {
                                    Game.Vector2 V = (Game.Vector2)Database.DefaultCoords[MyChar.Loc.PreviousMap];
                                    MyChar.Loc.Map = MyChar.Loc.PreviousMap;
                                    MyChar.Loc.X = V.X;
                                    MyChar.Loc.Y = V.Y;
                                }
                            }
                            else if (MyChar.Loc.Map == 2021 || MyChar.Loc.Map == 2022 || MyChar.Loc.Map == 2023 || MyChar.Loc.Map == 2024)
                            {
                                /* foreach (DictionaryEntry DE in Database.DefaultCoords)
                                 {
                                     Console.WriteLine(DE.Key);
                                     Game.Vector2 V2 = (Game.Vector2)DE.Value;
                                     Console.WriteLine(V2.X);
                                     Console.WriteLine(V2.Y);
                                 }*/
                                Game.Vector2 V = (Game.Vector2)Database.DefaultCoords[(uint)1020];//324
                                MyChar.Loc.Map = 1020;
                                MyChar.Loc.X = V.X;
                                MyChar.Loc.Y = V.Y;
                            }
                        }

                        if (MyChar.MyShop != null)
                        {
                            //foreach (uint UID in MyChar.MyShop.Items.Keys)
                            //{
                            //    // AddSend(Packets.ItemPacket(UID, MyChar.MyShop.UID, 23));
                            //    AddSend(Packets.ItemPacket(UID, 0, 3));
                            //}
                            //if (MyChar != null)
                            //    if (MyChar.MyShop != null)
                            //        MyChar.MyShop.Close();
                            MyChar.MyShop.Close();
                        }
                        if (MyChar != null)
                        {
                            foreach (Game.Friend F in MyChar.Friends.Values)
                            {
                                AddSend(Packets.FriendEnemyPacket(F.UID, F.Name, 15, Convert.ToByte(F.Online)));
                                if (F.Online)
                                {
                                    F.Info.MyClient.AddSend(Packets.FriendEnemyPacket(MyChar.EntityID, MyChar.Name, 14, 1));
                                    F.Info.MyClient.AddSend(Packets.FriendEnemyPacket(MyChar.EntityID, MyChar.Name, 15, 0));
                                    F.Info.MyClient.LocalMessage(2005, "Your friend " + MyChar.Name + " has logged off.");
                                }
                            }
                        }
                        if (MyChar != null)
                        {
                            if (MyChar.Loc.Map == 1038 && Features.GuildWars.War && MyChar.Alive)
                            {
                                MyChar.Loc.PreviousMap = MyChar.Loc.Map;
                                MyChar.Loc.Map = 1002;
                                MyChar.Loc.X = 430;
                                MyChar.Loc.Y = 380;
                            }
                            if (!MyChar.Alive)
                            {
                                if (MyChar.Loc.Map == 1038 && Features.GuildWars.War)
                                {
                                    MyChar.Loc.PreviousMap = MyChar.Loc.Map;
                                    MyChar.Loc.Map = 6001;
                                    MyChar.Loc.X = 32;
                                    MyChar.Loc.Y = 72;
                                }
                                else if (MyChar.Loc.Map == 1844)
                                {
                                    MyChar.Loc.PreviousMap = MyChar.Loc.Map;
                                    MyChar.Loc.Map = 1002;
                                    MyChar.Loc.X = 430;
                                    MyChar.Loc.Y = 378;
                                    Database.SaveCharacter(MyChar, AuthInfo.Account);
                                }
                                else
                                {
                                    foreach (uint[] Point in Database.RevPoints)
                                    {
                                        if (Point[0] == MyChar.Loc.Map)
                                        {
                                            MyChar.Loc.PreviousMap = MyChar.Loc.Map;
                                            MyChar.Loc.Map = Point[1];
                                            MyChar.Loc.X = (ushort) Point[2];
                                            MyChar.Loc.Y = (ushort) Point[3];
                                            break;
                                        }
                                    }

                                }
                                MyChar.CurHP = 1;
                            }
                            //else
                            //{
                            //    if (MyChar.Loc.Map == 1038 && Features.GuildWars.War)
                            //    {
                            //        MyChar.Loc.PreviousMap = MyChar.Loc.Map;
                            //        MyChar.Loc.Map = 6001;
                            //        MyChar.Loc.X = 32;
                            //        MyChar.Loc.Y = 72;
                            //    }
                            //}
                        }

                    }
                    catch (Exception E) { Game.World.ExcAdd += E.ToString() + "\r\n"; }
                    try
                    {
                        if (MyChar != null)
                        {
                            if (MyChar.ScreenChars != null)
                            {
                                if (MyChar.ScreenChars.Values != null)
                                {
                                    foreach (Game.Character C in MyChar.ScreenChars.Values)
                                        if (C != null)
                                            if (C.ScreenChars.Values != null)
                                            {
                                                if (C.ScreenChars.ContainsKey(MyChar.EntityID))
                                                {
                                                    C.ScreenChars.Remove(MyChar.EntityID);
                                                    C.MyClient.AddSend(Packets.GeneralData(MyChar.EntityID, MyChar.Loc.Map, MyChar.Loc.X, MyChar.Loc.Y, 135).Get);
                                                }
                                            }
                                }
                            }
                        }
                    }
                    catch (Exception E) { Game.World.ExcAdd += E.ToString() + "\r\n"; }
                    try
                    {
                        if (MyChar != null)
                        {
                            Game.World.DebugAdd += "User " + MyChar.Name + " disconnected. \r\n";
                            if (MyChar.MyGuild != null)
                                MyChar.MyGuild.GuildMsg("SYSTEM", MyChar.MyGuild.Members.Values.ToString(), MyChar.Name + " has logged off.", 0);
                        }
                    }
                    catch { }
                    Database.SaveCharacter(MyChar, AuthInfo.Account);
                    try
                    {
                        if (Game.World.H_Chars.ContainsKey(MyChar.EntityID))
                            Game.World.H_Chars.Remove(MyChar.EntityID);
                        //ConcurrentDictionary<uint, Game.Character> Map = (ConcurrentDictionary<uint, Game.Character>)Game.World.PlayersInMap[MyChar.Loc.Map];
                        if (Game.World.PlayersInMap.ContainsKey(MyChar.Loc.Map))
                        {
                            if (Game.World.PlayersInMap[MyChar.Loc.Map].ContainsKey(MyChar.EntityID))
                                Game.World.PlayersInMap[MyChar.Loc.Map].Remove(MyChar.EntityID);
                        }
                        if (Game.World.PlayersInMap.ContainsKey(MyChar.Loc.PreviousMap))
                        {
                            if (Game.World.PlayersInMap[MyChar.Loc.PreviousMap].ContainsKey(MyChar.EntityID))
                                Game.World.PlayersInMap[MyChar.Loc.PreviousMap].Remove(MyChar.EntityID);
                        }
                        //  for (int i = 0; i < 7; i++)
                        //Program.ThreadInfo.Modified = true;
                    }

                    catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                    try
                    {
                        if (Game.World.H_Clients.ContainsKey(MyChar.EntityID))
                            Game.World.H_Clients.Remove(MyChar.EntityID);
                    }
                    catch (Exception Exc)
                    {
                        Game.World.ExcAdd += Exc.ToString() + "\r\n";
                    }
                    //if (Features.Turnny.PlayerList.Contains(MyChar.EntityID))
                    //    Features.Turnny.RemovePlayers.Add(MyChar);

                    //if (Features.SkillPK.PlayerList.Contains(MyChar.EntityID))
                    //    Features.SkillPK.RemovePlayer(MyChar);


                }
            }
        }
        public void Disconnect()
        {
            try
            {
                if (Soc.Connected)
                {
                    Soc.Shutdown(SocketShutdown.Both);
                    LogOff();
                    Soc.Close();
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        } 
        public bool ValidName(string name)
        {
            foreach (char ch in name)
            {
                byte c = Convert.ToByte(ch);
                if (!
                    ((c >= 48 && c <= 57) ||
                    (c >= 65 && c <= 90) ||
                    (c >= 97 && c <= 122))
                    )
                    return false;
            }
            return true;
        }

        public bool ValidWHPass(string Name)
        {
            
            if (Name.IndexOfAny(new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
