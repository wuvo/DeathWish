using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using System.IO;
using System.Drawing.Imaging;
using Ultimate.Game;
using Ultimate.Structures;
using Ultimate.Events;
using System.Linq;
using Ultimate.MysqlDB;

namespace Ultimate.PacketHandling
{

    public class Chat
    {
        public void Broadcast(string msg, BroadCastLoc loc, uint index = 0, ushort _chatType = 2011)
        {
            if (loc == BroadCastLoc.World)
                World.SendMsgToAll("[System]", msg, 2005, 0);

            else if (loc == BroadCastLoc.Map)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, _chatType, 0U));
            }
            else if (loc == BroadCastLoc.Score)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83d, 0));
            }
            else if (loc == BroadCastLoc.Title)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83c, 0));
            }
        }
        public Dictionary<uint, Character> PlayerList = new Dictionary<uint, Character>();
        public static void Handle(Main.GameClient GC, byte[] Data)
        {

            MemoryStream MS = new MemoryStream(Data);
            BinaryReader BR = new BinaryReader(MS);
            BR.ReadBytes(8);
            ushort ChatType = (ushort)BR.ReadUInt32();
            BR.ReadBytes(13);
            int Position = 26;
            int Len = 0;
            string From = "";
            string To = "";
            string Message = "";

            for (int C = 0; C < Data[25]; C++)
            {
                From += Convert.ToChar(Data[Position]);
                Position++;
            }
            Len = Data[Position];
            Position++;
            for (int C = 0; C < Len; C++)
            {
                To += Convert.ToChar(Data[Position]);
                Position++;
            }
            Position++;
            Len = Data[Position];
            Position++;
            for (int C = 0; C < Len; C++)
            {
                Message += Convert.ToChar(Data[Position]);
                Position++;
            }



            /*   string From = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
               string To = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
               BR.ReadByte();
               string Message = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));*/
            BR.Close();
            MS.Close();
            if (ChatType >= 2201 && ChatType <= 2205)
            {
                MessageBoard.Write(GC, Message, ChatType);
                return;
            }
            if (ChatType == 2104 && GC.MyChar.MyShop != null)
                GC.MyChar.MyShop.Hawk = Message;
            try
            {
                if (Message.Length > 0)
                    if (Message[0] == '/' || Message[0] == '@')
                    {
                        string[] Cmd = Message.Split(' ');
                        #region Normal Commands
                        switch (Cmd[0].ToLower())
                        {
                            #region /roll
                            case "@roll":
                                if (Game.World.Drawing == true)
                                    if (!Game.World.H_CharsDrawing.ContainsKey(GC.MyChar.EntityID))
                                    {
                                        Game.World.H_CharsDrawing.Add(GC.MyChar.EntityID, GC.MyChar);
                                        GC.MyChar.Roll = (byte)Game.World.Rnd.Next(1, 100);
                                        GC.LocalMessage(2000, "You rolled: " + GC.MyChar.Roll);
                                    }
                                    else
                                        GC.LocalMessage(2000, "You already rolled!");
                                else
                                    GC.LocalMessage(2000, "The event is over for now you can roll when the event will start!");
                                break;
                            #endregion
                            #region /guildwar
                            case "@guildwar":
                                if (Features.GuildWars.War)
                                {
                                    var timeLeft = DateTime.Now;
                                    if (timeLeft.DayOfWeek != DayOfWeek.Sunday)
                                        timeLeft = timeLeft.AddDays(7 - (byte)timeLeft.DayOfWeek);
                                    timeLeft = timeLeft.AddHours(19 - timeLeft.Hour).AddMinutes(-timeLeft.Minute);
                                    var toDisplay = timeLeft.Subtract(DateTime.Now);

                                    GC.LocalMessage(2000, $"The Guild War will end in {toDisplay.Days} Days, {toDisplay.Hours} Hours and {toDisplay.Minutes} Minutes. Make sure you won't miss it!");
                                }
                                else
                                {
                                    GC.LocalMessage(2000, $"This command only works on Saturday and Sunday!");
                                }
                                break;
                            #endregion
                            #region /help
                            case "@help":
                                //Discord DCord1 = new Discord();
                                //DCord1.MesajVer5 = "Player Name : __**" + GC.MyChar.Name + "**__ : Hello ! I need your help, can you contact me ?";
                                GC.LocalMessage(2000, $"Hi! Please join out Discord server by using the @discord command and submit a ticket via the 'Submit-A-Ticket' page and one of our staff will get back to you as soon as possible!");

                                break;
                            #endregion

                            #region /
                            case "@expmob":
                                Game.World.ExpMob = true;
                                break;
                            #endregion


                            #region /cleargems
                            case "@cleargems":
                                {
                                    byte ia = 0;
                                    byte ib = 0;
                                    byte ic = 0;
                                    byte id = 0;
                                    byte ie = 0;
                                    byte ig = 0;
                                    byte ij = 0;
                                    if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                    {
                                        foreach (Game.Item I in GC.MyChar.Inventory)
                                        {
                                            if (I.ID == 700001)
                                                ia++;
                                            else if (I.ID == 700011)
                                                ib++;
                                            else if (I.ID == 700021)
                                                ic++;
                                            else if (I.ID == 700031)
                                                id++;
                                            else if (I.ID == 700041)
                                                ie++;
                                            else if (I.ID == 700051)
                                                ig++;
                                            else if (I.ID == 700061)
                                                ij++;
                                        }
                                        if (ia > 0)
                                        {
                                            for (int aa = 0; aa < ia; aa++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700001));
                                            GC.LocalMessage(2000, "You successfully removed " + ia + " Phoenix!");
                                        }
                                        if (ib > 0)
                                        {
                                            for (int bb = 0; bb < ib; bb++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700011));
                                            GC.LocalMessage(2000, "You successfully removed " + ib + " DragonGems!");
                                        }
                                        if (ic > 0)
                                        {
                                            for (int cc = 0; cc < ic; cc++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700021));
                                            GC.LocalMessage(2000, "You successfully removed " + ic + " FuryGems!");
                                        }
                                        if (id > 0)
                                        {
                                            for (int dd = 0; dd < id; dd++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700031));
                                            GC.LocalMessage(2000, "You successfully removed " + id + " RainbowGems!");
                                        }
                                        if (ie > 0)
                                        {
                                            for (int ee = 0; ee < ie; ee++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700041));
                                            GC.LocalMessage(2000, "You successfully removed " + ie + " KylinGems!");
                                        }
                                        if (ig > 0)
                                        {
                                            for (int gg = 0; gg < ig; gg++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700051));
                                            GC.LocalMessage(2000, "You successfully removed " + ig + " VioletGem!");
                                        }
                                        if (ij > 0)
                                        {
                                            for (int jj = 0; jj < ij; jj++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700061));
                                            GC.LocalMessage(2000, "You successfully removed " + ij + " MoonGem!");
                                        }
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "Sorry you need to be vip level 3 / 6!");
                                    }
                                    break;
                                }
                            #endregion
                            #region /skip
                            case "@skip":
                                if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                {
                                    if (Cmd[1] == "elite")
                                    {
                                        if (GC.MyChar.skipelite)
                                        {
                                            GC.MyChar.skipelite = false;
                                            GC.LocalMessage(2000, "Skip Elite Items : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipelite = true;
                                            GC.LocalMessage(2000, "Skip Elite Items : true");
                                        }
                                    }
                                    if (Cmd[1] == "super")
                                    {
                                        if (GC.MyChar.skipsuper)
                                        {
                                            GC.MyChar.skipsuper = false;
                                            GC.LocalMessage(2000, "Skip Super Items : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipsuper = true;
                                            GC.LocalMessage(2000, "Skip Super Items : true");
                                        }
                                    }
                                    if (Cmd[1] == "greenegg")
                                    {
                                        if (GC.MyChar.skipgreenegg)
                                        {
                                            GC.MyChar.skipgreenegg = false;
                                            GC.LocalMessage(2000, "Skip Green Egg : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipgreenegg = true;
                                            GC.LocalMessage(2000, "Skip Green Egg : true");
                                        }
                                    }
                                    if (Cmd[1] == "redegg")
                                    {
                                        if (GC.MyChar.skipredegg)
                                        {
                                            GC.MyChar.skipredegg = false;
                                            GC.LocalMessage(2000, "Skip Red Egg : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipredegg = true;
                                            GC.LocalMessage(2000, "Skip Red Egg : true");
                                        }
                                    }
                                    if (Cmd[1] == "meteor")
                                    {
                                        if (GC.MyChar.skipmeteor)
                                        {
                                            GC.MyChar.skipmeteor = false;
                                            GC.LocalMessage(2000, "Skip Meteors : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipmeteor = true;
                                            GC.LocalMessage(2000, "Skip Meteors : true");
                                        }
                                    }
                                    if (Cmd[1] == "pg")
                                    {
                                        if (GC.MyChar.skiphoenixgem)
                                        {
                                            GC.MyChar.skiphoenixgem = false;
                                            GC.LocalMessage(2000, "Skip Phoenix Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skiphoenixgem = true;
                                            GC.LocalMessage(2000, "Skip Phoenix Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "dg")
                                    {
                                        if (GC.MyChar.skipdragongem)
                                        {
                                            GC.MyChar.skipdragongem = false;
                                            GC.LocalMessage(2000, "Skip Dragon Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipdragongem = true;
                                            GC.LocalMessage(2000, "Skip Dragon Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "fg")
                                    {
                                        if (GC.MyChar.skipfurygem)
                                        {
                                            GC.MyChar.skipfurygem = false;
                                            GC.LocalMessage(2000, "Skip Fury Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipfurygem = true;
                                            GC.LocalMessage(2000, "Skip Fury Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "rg")
                                    {
                                        if (GC.MyChar.skiprainbowgem)
                                        {
                                            GC.MyChar.skiprainbowgem = false;
                                            GC.LocalMessage(2000, "Skip Rainbow Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skiprainbowgem = true;
                                            GC.LocalMessage(2000, "Skip Rainbow Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "kg")
                                    {
                                        if (GC.MyChar.skipkylingem)
                                        {
                                            GC.MyChar.skipkylingem = false;
                                            GC.LocalMessage(2000, "Skip Kylin Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipkylingem = true;
                                            GC.LocalMessage(2000, "Skip Kylin Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "vg")
                                    {
                                        if (GC.MyChar.skipvioletgem)
                                        {
                                            GC.MyChar.skipvioletgem = false;
                                            GC.LocalMessage(2000, "Skip Violet Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipvioletgem = true;
                                            GC.LocalMessage(2000, "Skip Violet Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "mg")
                                    {
                                        if (GC.MyChar.skipmoongem)
                                        {
                                            GC.MyChar.skipmoongem = false;
                                            GC.LocalMessage(2000, "Skip Moon Gem : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipmoongem = true;
                                            GC.LocalMessage(2000, "Skip Moon Gem : true");
                                        }
                                    }
                                    else if (Cmd[1] == "allgems")
                                    {
                                        if (GC.MyChar.skipallgems)
                                        {
                                            GC.MyChar.skipallgems = false;
                                            GC.MyChar.skipdragongem = false;
                                            GC.MyChar.skiphoenixgem = false;
                                            GC.MyChar.skiprainbowgem = false;
                                            GC.MyChar.skipkylingem = false;
                                            GC.MyChar.skipfurygem = false;
                                            GC.MyChar.skipvioletgem = false;
                                            GC.MyChar.skipmoongem = false;
                                            GC.LocalMessage(2000, "Skip All Gems (without Tortoise) : false");
                                        }
                                        else
                                        {
                                            GC.MyChar.skipallgems = true;
                                            GC.MyChar.skipdragongem = true;
                                            GC.MyChar.skiphoenixgem = true;
                                            GC.MyChar.skiprainbowgem = true;
                                            GC.MyChar.skipkylingem = true;
                                            GC.MyChar.skipfurygem = true;
                                            GC.MyChar.skipvioletgem = true;
                                            GC.MyChar.skipmoongem = true;
                                            GC.LocalMessage(2000, "Skip All Gems (without Tortoise) : true");
                                        }
                                    }




                                }
                                else
                                {
                                    GC.LocalMessage(2005, "Sorry you need to be vip level 3 / 6!");
                                }

                                break;



                            #endregion
                            #region /compose
                            case "@compose":
                                if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                {
                                    if (Cmd[1] == "pg")
                                    {
                                        if (GC.MyChar.InventoryContains(700001, 15))
                                        {
                                            for (int gem1 = 0; gem1 < 15; gem1++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700001));
                                            GC.MyChar.AddItem(700002);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }
                                    else if (Cmd[1] == "dg")
                                    {
                                        if (GC.MyChar.InventoryContains(700011, 15))
                                        {
                                            for (int gem11 = 0; gem11 < 15; gem11++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700011));
                                            GC.MyChar.AddItem(700012);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }
                                    else if (Cmd[1] == "fg")
                                    {
                                        if (GC.MyChar.InventoryContains(700021, 15))
                                        {
                                            for (int gem21 = 0; gem21 < 15; gem21++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700021));
                                            GC.MyChar.AddItem(700022);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }
                                    else if (Cmd[1] == "rg")
                                    {
                                        if (GC.MyChar.InventoryContains(700031, 15))
                                        {
                                            for (int gem31 = 0; gem31 < 15; gem31++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700031));
                                            GC.MyChar.AddItem(700032);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }
                                    else if (Cmd[1] == "kg")
                                    {
                                        if (GC.MyChar.InventoryContains(700041, 15))
                                        {
                                            for (int gem41 = 0; gem41 < 15; gem41++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700041));
                                            GC.MyChar.AddItem(700042);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }
                                    else if (Cmd[1] == "vg")
                                    {
                                        if (GC.MyChar.InventoryContains(700051, 15))
                                        {
                                            for (int gem51 = 0; gem51 < 15; gem51++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700051));
                                            GC.MyChar.AddItem(700052);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }
                                    else if (Cmd[1] == "mg")
                                    {
                                        if (GC.MyChar.InventoryContains(700061, 15))
                                        {
                                            for (int gem61 = 0; gem61 < 15; gem61++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700061));
                                            GC.MyChar.AddItem(700062);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "Sorry you dont have 15 Normal gems in your inventory. !");
                                        }
                                    }




                                }
                                else
                                {
                                    GC.LocalMessage(2005, "Sorry you need to be vip level 3 / 6!");
                                }

                                break;



                            #endregion

                            #region /watch
                            case "@watch":
                                if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                                {
                                    GC.LocalMessage(2000, "You cant use this command in this map");
                                }
                                else
                                {
                                    if (!GC.MyChar.BOTJailed)
                                    {
                                        if (GC.MyChar.VipLevel >= 5)
                                        {
                                            if (Cmd[1] == "10000")
                                                if (DMaps.EventMaps.ContainsKey(10000))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10000, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);

                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10001")
                                                if (DMaps.EventMaps.ContainsKey(10001))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10001, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);

                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10002")
                                                if (DMaps.EventMaps.ContainsKey(10002))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10002, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);

                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10003")
                                                if (DMaps.EventMaps.ContainsKey(10003))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10003, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10004")
                                                if (DMaps.EventMaps.ContainsKey(10004))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10004, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10005")
                                                if (DMaps.EventMaps.ContainsKey(10005))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10005, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10006")
                                                if (DMaps.EventMaps.ContainsKey(10006))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10006, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10007")
                                                if (DMaps.EventMaps.ContainsKey(10007))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10007, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10008")
                                                if (DMaps.EventMaps.ContainsKey(10008))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10008, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10009")
                                                if (DMaps.EventMaps.ContainsKey(10009))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10009, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10010")
                                                if (DMaps.EventMaps.ContainsKey(10010))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10010, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10011")
                                                if (DMaps.EventMaps.ContainsKey(10011))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10011, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10012")
                                                if (DMaps.EventMaps.ContainsKey(10012))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10012, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10013")
                                                if (DMaps.EventMaps.ContainsKey(10013))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10013, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10014")
                                                if (DMaps.EventMaps.ContainsKey(10014))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10014, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }
                                            else if (Cmd[1] == "10015")
                                                if (DMaps.EventMaps.ContainsKey(10015))
                                                {
                                                    GC.MyChar.Loc.OldMap = GC.MyChar.Loc.Map;
                                                    GC.MyChar.Loc.OldX = GC.MyChar.Loc.X;
                                                    GC.MyChar.Loc.OldY = GC.MyChar.Loc.Y;
                                                    GC.MyChar.Teleport(10015, 50, 50);
                                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                    GC.MyChar.Invisible = true;
                                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                                    GC.LocalMessage(2000, "Please type /quitwatch if you dont watch fight!");
                                                    World.SendMsgToAll("SYSTEM", "" + GC.MyChar.Name + " entered the arena to watch you. Enjoy the show..", 2005, 0, GC.MyChar.Loc.Map);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "No such room number. Please write right door number..");

                                                }

                                        }
                                        else
                                        {
                                            GC.LocalMessage(2000, "You require VIP 5 or more to use this function.");

                                        }
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2000, "Sorry you cant use this command if you are BotJailed!");
                                    }
                                }
                                break;

                            #endregion


                            #region /passive
                            case "@passive":
                                if (GC.MyChar.VipLevel >= 5)
                                {
                                    GC.MyChar.PassiveSkills = !GC.MyChar.PassiveSkills;
                                    if (GC.MyChar.PassiveSkills)
                                        GC.LocalMessage(2000, "Passive skills activated!");
                                    else
                                        GC.LocalMessage(2000, "Passive skills deactivated!");
                                }
                                else
                                {
                                    if (!GC.MyChar.PassiveSkills)
                                    {
                                        GC.MyChar.PassiveSkills = !GC.MyChar.PassiveSkills;
                                        GC.LocalMessage(2000, "Passive skills activated!");
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2000, "You require VIP 5 or more to use this function.");
                                    }
                                }
                                break;
                            #endregion



                            #region /scroll
                            case "@scroll":
                                if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                                {
                                    GC.LocalMessage(2000, "You cant use this command in this map");
                                }
                                else
                                {
                                    if (!GC.MyChar.StatEff.Contains(StatusEffectEn.BlueName) || !GC.MyChar.StatEff.Contains(StatusEffectEn.RedName) || !GC.MyChar.StatEff.Contains(StatusEffectEn.BlackName))
                                    {
                                        if (GC.MyChar.MyShop == null)
                                        {
                                            if (!GC.MyChar.Trading)
                                            {
                                                if (GC.MyChar.Alive)
                                                {
                                                    Game.World.ChatAdd += "Command Scroll ! : " + GC.MyChar.Name + " Time : " + DateTime.Now + "\r\n";
                                                    if (GC.MyChar.Loc.Map != 6001 && GC.MyChar.Loc.Map != 1049 && GC.MyChar.Loc.Map != 1212 && GC.MyChar.Loc.Map != 1039 && GC.MyChar.Loc.Map != 6000 && GC.MyChar.Loc.Map != 6002 && GC.MyChar.Loc.Map != 6003)
                                                    {
                                                        if (Cmd[1] == "minetc")
                                                        {
                                                            if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                                            {
                                                                GC.MyChar.Teleport(1028, 155, 95);
                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry you are not vip level 3 / 6.");
                                                            }
                                                        }
                                                        else if (Cmd[1] == "minejail")
                                                        {
                                                            if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                                            {
                                                                GC.MyChar.Teleport(6000, 32, 72);
                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry you are not vip level 3 / 6.");
                                                            }
                                                        }
                                                        else if (Cmd[1] == "minepc")
                                                        {
                                                            if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                                            {
                                                                GC.MyChar.Teleport(1025, 28, 70);
                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry you are not vip level 3 / 6.");
                                                            }
                                                        }
                                                        else if (Cmd[1] == "mineac")
                                                        {
                                                            if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                                            {
                                                                GC.MyChar.Teleport(1026, 140, 104);
                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry you are not vip level 3 / 6.");
                                                            }
                                                        }
                                                        else if (Cmd[1] == "minedc")
                                                        {
                                                            if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                                            {
                                                                GC.MyChar.Teleport(1027, 140, 104);
                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry you are not vip level 3 / 6.");
                                                            }
                                                        }
                                                        else if (Cmd[1] == "minemeteor")
                                                        {
                                                            if (GC.MyChar.VipLevel == 3 || GC.MyChar.VipLevel >= 5)
                                                            {
                                                                GC.MyChar.Teleport(1029, 30, 70);
                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry you are not vip level 3 / 6.");
                                                            }
                                                        }
                                                        if (GC.MyChar.VipLevel >= 3)
                                                        {
                                                            if (!GC.MyChar.BlueName)
                                                            {
                                                                if (Cmd[1] == "tc")
                                                                    GC.MyChar.Teleport(1002, 427, 379);
                                                                else if (Cmd[1] == "tg")
                                                                    GC.MyChar.Teleport(1039, 228, 226);
                                                                else if (Cmd[1] == "pc")
                                                                    GC.MyChar.Teleport(1011, 190, 271);
                                                                else if (Cmd[1] == "dc")
                                                                    GC.MyChar.Teleport(1000, 500, 650);
                                                                else if (Cmd[1] == "bi")
                                                                    GC.MyChar.Teleport(1015, 723, 573);
                                                                else if (Cmd[1] == "ac")
                                                                    GC.MyChar.Teleport(1020, 566, 564);
                                                                else if (Cmd[1] == "ma" || Cmd[1] == "mk")
                                                                    GC.MyChar.Teleport(1036, 207, 196);
                                                                else if (Cmd[1] == "mc")
                                                                    GC.MyChar.Teleport(1001, 312, 646);
                                                                else if (Cmd[1] == "hk")
                                                                    GC.MyChar.Teleport(1013, 80, 60);
                                                                else if (Cmd[1] == "pka")
                                                                    GC.MyChar.Teleport(1002, 459, 294);
                                                                else if (Cmd[1] == "pkj")
                                                                    GC.MyChar.Teleport(1002, 512, 353);
                                                                else if (Cmd[1] == "gp")
                                                                    GC.MyChar.Teleport(1004, 50, 50);
                                                                else if (Cmd[1] == "lab")
                                                                {
                                                                    if (GC.MyChar.VP >= 2000)
                                                                    {
                                                                        GC.MyChar.VP -= 2000;
                                                                        GC.MyChar.Teleport(1351, 17, 127);
                                                                    }
                                                                    else
                                                                    {
                                                                        GC.LocalMessage(2000, "Sorry, you dont have 2000 Virtue Point.");
                                                                    }
                                                                }

                                                                else if (Cmd[1] == "lab2")
                                                                    if (GC.MyChar.Loc.Map == 1351 || GC.MyChar.Loc.Map == 1352 || GC.MyChar.Loc.Map == 1353 || GC.MyChar.Loc.Map == 1354)
                                                                    {
                                                                        if (GC.MyChar.InventoryContains(721537, 1))
                                                                        {

                                                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721537));
                                                                            GC.MyChar.Teleport(1352, 28, 221);
                                                                        }
                                                                        else
                                                                        {
                                                                            GC.LocalMessage(2000, "Sorry, you dont have SkyToken.");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        GC.LocalMessage(2000, "Sorry, you use this command only in Labyrenth");
                                                                    }

                                                                else if (Cmd[1] == "lab3")
                                                                    if (GC.MyChar.Loc.Map == 1351 || GC.MyChar.Loc.Map == 1352 || GC.MyChar.Loc.Map == 1353 || GC.MyChar.Loc.Map == 1354)
                                                                    {
                                                                        if (GC.MyChar.InventoryContains(721538, 1))
                                                                        {

                                                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721538));
                                                                            GC.MyChar.Teleport(1353, 26, 261);
                                                                        }
                                                                        else
                                                                        {
                                                                            GC.LocalMessage(2000, "Sorry, you dont have EarthToken.");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        GC.LocalMessage(2000, "Sorry, you use this command only in Labyrenth");
                                                                    }
                                                                else if (Cmd[1] == "lab4")
                                                                    if (GC.MyChar.Loc.Map == 1351 || GC.MyChar.Loc.Map == 1352 || GC.MyChar.Loc.Map == 1353 || GC.MyChar.Loc.Map == 1354)
                                                                    {
                                                                        if (GC.MyChar.InventoryContains(721539, 1))
                                                                        {

                                                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721539));
                                                                            GC.MyChar.Teleport(1354, 7, 288);
                                                                        }
                                                                        else
                                                                        {
                                                                            GC.LocalMessage(2000, "Sorry, you dont have SoulToken.");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        GC.LocalMessage(2000, "Sorry, you use this command only in Labyrenth");
                                                                    }
                                                                else if (Cmd[1] == "gano")
                                                                    GC.MyChar.Teleport(1011, 453, 819);
                                                                else if (Cmd[1] == "titan")
                                                                    GC.MyChar.Teleport(1020, 232, 461);
                                                                else if (Cmd[1] == "dragon")
                                                                    GC.MyChar.Teleport(1002, 569, 815);
                                                                else if (Cmd[1] == "tash")
                                                                    GC.MyChar.Teleport(1000, 496, 299);
                                                                else if (Cmd[1] == "spook")
                                                                    GC.MyChar.Teleport(1015, 722, 918);
                                                                else if (Cmd[1] == "raikou")
                                                                    GC.MyChar.Teleport(1105, 115, 120);
                                                                else if (Cmd[1] == "capricorn")
                                                                    GC.MyChar.Teleport(1011, 777, 486);


                                                            }
                                                            else
                                                            {
                                                                GC.LocalMessage(2000, "Sorry, blue name cant use scroll commands.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            GC.LocalMessage(2000, "You require VIP 5 or more to use this function.");
                                                        }

                                                    }

                                                    else
                                                    {
                                                        GC.LocalMessage(2000, "Sorry you cant use command in this map..!");
                                                    }

                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2000, "Sorry you cant use this command if you are Alive!");
                                                }
                                            }
                                            else
                                            {
                                                GC.LocalMessage(2000, "Sorry you cant use this command if you are Trading!");
                                            }
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2000, "Sorry you cant use this command if you are Shopping!");
                                        }
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2000, "Sorry you cant use this command if you are Blue / Red / Black Name!");
                                    }
                                }
                                break;

                            #endregion



                            #region /clearinv
                            case "@clearinv":
                                List<Item> Items = new List<Item>();
                                foreach (Game.Item I in GC.MyChar.Inventory)
                                    Items.Add(I);
                                if (GC.AuthInfo.Status == "[PM]")
                                    foreach (Game.Item I in Items)
                                        GC.MyChar.RemoveItem(I);
                                else
                                {
                                    if (GC.MyChar.VipLevel >= 0)
                                    {
                                        foreach (Game.Item I in Items)
                                            if (!I.IsWorth())
                                                GC.MyChar.RemoveItem(I);
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2000, "You require VIP 5 or more to use this function.");
                                    }
                                }
                                GC.LocalMessage(2005, "Inventory Cleared!");
                                Game.World.ChatAdd += "Command Inventory Cleared! : " + GC.MyChar.Name + " Time : " + DateTime.Now + "\r\n";

                                break;
                            #endregion
                            #region Mute IP
                            case "@muteip":
                                if (GC.MyChar.VipLevel >= 5)
                                {
                                    Character C = World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        if (!GC.MyChar.IPMuted.Contains(C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()))
                                        {
                                            GC.LocalMessage(2000, "You have muted the messages from " + C.Name + "'s IP Address!");
                                            GC.MyChar.IPMuted.Add(C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString());
                                        }
                                }
                                else
                                {
                                    GC.LocalMessage(2000, "You require VIP 5 or more to use this function.");
                                }
                                break;
                            #endregion

                            #region /giftbag

                            case "@giftbag":
                                if (GC.MyChar.InventoryContains(720374, 1))
                                {
                                    GC.LocalMessage(2000, "Please don't spam the command. You already have a giftbag in your inventory.");
                                }
                                else
                                {
                                    GC.MyChar.AddItem(720374);
                                    GC.LocalMessage(2000, "A giftbag has been added to your inventory.");
                                }
                                break;

                            #endregion
                            #region /dc
                            case "@dc":
                                if (!GC.MyChar.BlueName || GC.GM)
                                {
                                    if (!GC.MyChar.Trading)
                                    {
                                        if (GC.MyChar.Alive)
                                        {
                                            if (GC.MyChar.MyShop == null)
                                            {
                                                if (GC.MyChar.PKPoints <= 29)
                                                {
                                                    if ((GC.MyChar.Loc.Map != 1038 || !Features.GuildWars.War) || GC.GM)
                                                    {
                                                        if (GC.MyChar.MyShop != null)
                                                            foreach (uint UID in GC.MyChar.MyShop.Items.Keys)
                                                                GC.AddSend(Packets.ItemPacket(UID, 0, 3));

                                                        GC.Disconnect();
                                                        Game.World.ChatAdd += "Command Relogin ! : " + GC.MyChar.Name + " Time : " + DateTime.Now + "\r\n";
                                                        if (GC.Soc.Connected)
                                                            GC.Soc.Disconnect(false);
                                                    }
                                                    else
                                                        GC.LocalMessage(2005, "You can't log out in GW area while GW is on.");
                                                }
                                                else
                                                    GC.LocalMessage(2005, "You can't log out while +30 PK Points");
                                            }
                                            else
                                                GC.LocalMessage(2005, "You can't log out while shoping");
                                        }
                                        else
                                            GC.LocalMessage(2005, "You can't log out while trading");
                                    }
                                    else
                                        GC.LocalMessage(2005, "You can't log out while blue name");
                                }
                                else
                                    GC.LocalMessage(2005, "You can't log out while blue name");
                                break;
                            #endregion
                            #region /joinpvp
                            case "@joinpvp":
                                if (GC.MyChar.EventBase == null)
                                {
                                    if (World.Events.Count > 0)
                                    {
                                        if (World.Events.Count == 1)
                                        {
                                            if (World.Events[0].AddPlayer(GC.MyChar))
                                                GC.MyChar.EventBase = World.Events[0];
                                        }
                                        else if (World.Events.Count > 1)
                                        {
                                            if (Cmd.Length == 1)
                                            {
                                                string titles = "";
                                                for (int a = 0; a < World.Events.Count; a++)
                                                    titles += $"{World.Events[a].EventTitle}: {a},";
                                                GC.LocalMessage(2000, titles);
                                                GC.LocalMessage(2000, "More than one event is running. Please type @joinpvp X where 'X' corresponds to the event ID.");
                                            }
                                            else
                                            {
                                                int a = 0;
                                                bool b = int.TryParse(Cmd[1], out a);
                                                if (b)
                                                    if (World.Events.Count >= a)
                                                        if (World.Events[a].AddPlayer(GC.MyChar))
                                                            GC.MyChar.EventBase = World.Events[a];
                                            }
                                        }
                                    }
                                    else
                                        GC.LocalMessage(2000, "There are no PVP Events running!");
                                }
                                break;
                            #endregion
                            #region giveup
                            case "@giveup":
                                {
                                    GC.MyChar.EventBase?.RemovePlayer(GC.MyChar, false);
                                    break;
                                }
                            #endregion
                            #region /vip
                            case "@vip":
                                GC.LocalMessage(2000, "VIP days left: " + GC.MyChar.VIPDays + " VIP level: " + GC.MyChar.VipLevel);
                                break;
                            #endregion

                            #region /vipaura
                            case "@vipaura":
                                if (GC.MyChar.VIPAura == false)
                                {
                                    GC.MyChar.VIPAura = true;
                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopNinja);
                                    GC.LocalMessage(2000, "VIP Aura: ON");
                                }
                                else
                                {
                                    GC.MyChar.VIPAura = false;
                                    GC.MyChar.StatEff.Remove(Game.StatusEffectEn.TopNinja);
                                    GC.LocalMessage(2000, "VIP Aura: OFF");
                                }
                                break;
                            #endregion

                            #region /count
                            case "@count":
                                if (GC.MyChar.CountEffect == false)
                                {
                                    GC.MyChar.CountEffect = true;
                                    GC.LocalMessage(2000, "Count Effects: ON");
                                }
                                else
                                {
                                    GC.MyChar.CountEffect = false;
                                    GC.LocalMessage(2000, "Count Effects: OFF");
                                }
                                break;
                            #endregion

                            #region /gem
                            case "@gem":
                                if (GC.MyChar.GemEffectsRemove == false)
                                {
                                    GC.MyChar.GemEffectsRemove = true;
                                    GC.LocalMessage(2000, "Gem Effects: ON");
                                }
                                else
                                {
                                    GC.MyChar.GemEffectsRemove = false;
                                    GC.LocalMessage(2000, "Gem Effects: OFF");
                                }
                                break;
                            #endregion

                            #region /players
                            case "@players":

                                if (GC.AuthInfo.Status == "[PM]")
                                {
                                    GC.LocalMessage(2000, "Time : " + DateTime.Now);
                                    GC.LocalMessage(2000, "Players Online: " + Game.World.H_Chars.Count);

                                    string eMsg = "";
                                    bool ToDisplay = true;
                                    foreach (Game.Character B in Game.World.H_Chars.Values)
                                    {
                                        if (eMsg.Length <= 240)
                                        {
                                            eMsg += B.Name + B.MyClient.AuthInfo.Status + ", ";
                                            ToDisplay = true;
                                        }
                                        else
                                        {
                                            eMsg = eMsg.Remove(eMsg.Length - 2, 2);
                                            GC.LocalMessage(2000, eMsg);
                                            ToDisplay = false;
                                            eMsg = "";
                                        }
                                    }
                                    if (ToDisplay)
                                        if (eMsg.Length > 1)
                                        {
                                            eMsg = eMsg.Remove(eMsg.Length - 2, 2);
                                            GC.LocalMessage(2000, eMsg);
                                        }
                                }
                                break;
                            #endregion
                            #region /items
                            case "@items":
                                GC.MyChar.MyClient.LocalMessage(2000, "Items: " + GC.MyChar.Inventory.Count);
                                break;
                            #endregion
                            #region /revive
                            case "@forcerevive":
                            case "@revive":
                            case "@rev":
                                PacketHandling.Revive.Handle(GC);
                                break;
                            #endregion
                            #region /list
                            case "@list":
                                byte _curList = Convert.ToByte(Convert.ToByte(Cmd[1]) - 1);
                                GC.MyChar.List = _curList;
                                GC.LocalMessage(2000, "Member List : " + (GC.MyChar.List + 1).ToString());
                                break;
                            #endregion
                            #region /cd
                            case "@cd":
                                GC.LocalMessage(2000, "Attack cooldown: " + GC.MyChar.AtkFrequence + " miliseconds! (lower is faster attack)");
                                break;
                            #endregion
                            #region /vipmineores
                            case "@vipmineores":
                                if (GC.MyChar.VipLevel >= 3)
                                {
                                    GC.MyChar.VIPMiningSkipOres = !GC.MyChar.VIPMiningSkipOres;
                                    GC.LocalMessage(2000, "VIP mining: Skip all Ores:" + GC.MyChar.VIPMiningSkipOres);
                                }
                                else
                                {
                                    GC.LocalMessage(2000, "You require VIP5 or more to use this function (disabled)");
                                    GC.MyChar.VIPMiningSkipOres = false;
                                }
                                break;
                            #endregion

                            #region /discord
                            case "@discord":
                                GC.LocalMessage(2105, "https://discord.gg/YmkmTsA");
                                break;
                            #endregion
                            #region /youtube
                            case "@youtube":
                                //GC.LocalMessage(2105, "https://www.youtube.com/channel/UCCoQzcX67yaZCtrx833HUFA?view_as=subscriber");
                                break;
                            #endregion
                            #region /report
                            case "@report":
                                //GC.LocalMessage(2105, "https://bitbucket.org/UltimateConquer/Ultimateconquer/issues?status=new&status=open");
                                break;
                            #endregion

                            #region /facebook
                            case "@facebook":
                                GC.LocalMessage(2105, "https://www.facebook.com/Ultimateconquerfb");
                                break;
                            #endregion

                            #region /store
                            case "@store":
                                GC.LocalMessage(2105, "http://www.Ultimateconquer.com/Store");
                                break;
                            #endregion


                            #region /quests
                            case "@quests":
                                if (GC.MyChar.BI_Quest != 0)
                                {
                                    GC.LocalMessage(2000, "Quest: Animal Purification (Bird Island 747,517)");
                                    GC.LocalMessage(2000, "Objective: Kill 15,000 Monsters around Bird Island, must be Birdmen or Hawkings or BanditL97s.");
                                    GC.LocalMessage(2000, "Bird Island Monsters Killed: " + GC.MyChar.BI_Quest_Kills + "/15000");
                                }
                                if (GC.MyChar.AC_Quest_Hops)
                                {
                                    GC.LocalMessage(2000, "Quest: Stress Relief (Ape City 550,598");
                                    GC.LocalMessage(2000, "Objective: Collect 5 hops from any of the following: Macaque, MacaqueL48, GiantApe, GiantApeL53, ThunderApe or ThunderApeL58.");
                                    GC.LocalMessage(2000, "Return to Breeder in Ape City (550,598) when your done.");
                                }
                                if (GC.MyChar.BI_Quest == 0 && !GC.MyChar.AC_Quest_Hops)
                                    GC.LocalMessage(2000, "You don't have any active quests.");
                                if (GC.MyChar.BI_Quest == 0)
                                    GC.LocalMessage(2000, "[QUEST] Animal Purification (Bird Island 747,517) is available.");
                                if (!GC.MyChar.AC_Quest_Hops)
                                    GC.LocalMessage(2000, "[QUEST] Stress Relief (Ape City 550,598) is available.");
                                break;
                            #endregion
                            #region /command
                            case "@command":
                                if (GC.MyChar.VipLevel > 0)
                                {
                                    GC.LocalMessage(2000, "@vip @vipaura @passive @clearinv @vipmineores @vit @str @spi @agi");
                                    GC.LocalMessage(2000, "@dc @joinpvp @giveup @vip @vipaura @items @roll @quests");
                                    GC.LocalMessage(2000, "@invitations @duel @acceptbet @quitduel @forcerevive @scroll tc");
                                }
                                break;
                            #endregion
                            #region /invitations
                            case "@invitations":
                                GC.MyChar.Invitations = !GC.MyChar.Invitations;
                                GC.LocalMessage(2000, $"Event invitations: {GC.MyChar.Invitations}!");
                                break;
                            #endregion

                            #region /plvlon
                            case "@plvl":
                                if (!World.Archers.ContainsKey(GC.MyChar.EntityID))
                                    World.Archers.TryAdd(GC.MyChar.EntityID, GC.MyChar);
                                break;
                            #endregion

                            #region /plvloff
                            case "@plvloff":
                                if (World.Archers.ContainsKey(GC.MyChar.EntityID))
                                    World.Archers.Remove(GC.MyChar.EntityID);
                                break;
                            #endregion


                            #region Duel
                            case "@duel":
                                if (GC.MyChar.Dueler != 0 && World.H_Chars.ContainsKey(GC.MyChar.Dueler) && World.H_Chars[GC.MyChar.Dueler].Loc.Map == GC.MyChar.Loc.Map)
                                {
                                    if (World.H_Chars[GC.MyChar.Dueler].Arena != null)
                                    {
                                        if (!World.H_Chars[GC.MyChar.Dueler].Arena.Wager)
                                        {
                                            GC.MyChar.Arena = World.H_Chars[GC.MyChar.Dueler].Arena;
                                            if (GC.MyChar.Arena.MapID != GC.MyChar.Loc.Map)
                                                GC.MyChar.Arena.AcceptDuel(GC.MyChar, World.H_Chars[GC.MyChar.Dueler]);
                                        }
                                        else
                                            GC.LocalMessage(2000, "To accept duels with bets please type /acceptbet");
                                    }
                                    else
                                    {
                                        GC.MyChar.Dueler = 0;
                                        GC.LocalMessage(2000, "Duel invitation expired! Send a new invitation!");
                                    }
                                }
                                else
                                {
                                    GC.MyChar.Dueler = 0;
                                    GC.LocalMessage(2000, "Your opponent is either offline or in a different map!");
                                }
                                break;
                            #endregion
                            #region /quitduel
                            case "@quitduel":
                                {
                                    if (GC.MyChar.Arena != null && GC.MyChar.Arena.MapID == GC.MyChar.Loc.Map)
                                    {
                                        GC.MyChar.Arena.RemovePlayer(GC.MyChar);
                                        foreach (Game.Character c in Game.World.H_Chars.Values)
                                        {
                                            if (GC.MyChar.Arena.MapID == GC.MyChar.Loc.Map)
                                            {
                                                Game.World.Action(c, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                                GC.MyChar.Invisible = false;
                                                GC.MyChar.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        GC.LocalMessage(2000, "You can't quit a duel if you're not in one.");
                                    }
                                    break;
                                }




                            #endregion
                            #region /quitwatch
                            case "@quitwatch":
                                {
                                    if (GC.MyChar.Arena != null && GC.MyChar.Arena.MapID == GC.MyChar.Loc.Map)
                                        GC.MyChar.Arena.RemovePlayer(GC.MyChar);

                                    else
                                    {
                                        if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                                        {
                                            GC.MyChar.Teleport(1002, 430, 380);
                                            Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                            GC.MyChar.Invisible = false;
                                            GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);

                                        }
                                        else
                                        {
                                            GC.LocalMessage(2000, "You can't quit a duel if you're not in one.");
                                        }
                                    }
                                }
                                break;
                            #endregion
                            #region AcceptWager
                            case "@acceptbet":
                                if (GC.MyChar.Dueler != 0 && World.H_Chars.ContainsKey(GC.MyChar.Dueler) && World.H_Chars[GC.MyChar.Dueler].Loc.Map == GC.MyChar.Loc.Map)
                                {
                                    if (World.H_Chars[GC.MyChar.Dueler].Arena != null)
                                    {
                                        if (World.H_Chars[GC.MyChar.Dueler].Arena.Wager)
                                        {
                                            GC.MyChar.Arena = World.H_Chars[GC.MyChar.Dueler].Arena;
                                            if (GC.MyChar.Arena.MapID != GC.MyChar.Loc.Map)
                                                GC.MyChar.Arena.AcceptDuel(GC.MyChar, World.H_Chars[GC.MyChar.Dueler]);
                                        }
                                        else
                                            GC.LocalMessage(2000, "To accept normal duels please type @duel");
                                    }
                                    else
                                    {
                                        GC.MyChar.Dueler = 0;
                                        GC.LocalMessage(2000, "Duel invitation expired! Send a new invitation!");
                                    }
                                }
                                else
                                {
                                    GC.MyChar.Dueler = 0;
                                    GC.LocalMessage(2000, "Your opponent is either offline or in a different map!");
                                }
                                break;
                            #endregion
                            #region Attribute Points
                            case "@vit":
                                ushort toAdd = 0;
                                if (ushort.TryParse(Cmd[1], out toAdd))
                                {
                                    if (GC.MyChar.VipLevel >= 5)
                                    {
                                        if (GC.MyChar.StatPoints >= toAdd)
                                        {
                                            GC.MyChar.StatPoints -= toAdd;
                                            GC.MyChar.Vit += toAdd;
                                            //GC.AddSend(Packets.CharacterInfo(GC.MyChar));
                                            //GC.MyChar.Equips.Send(GC, false);
                                        }
                                        else
                                            GC.LocalMessage(2005, "You don't have enough attribute points!");
                                    }
                                    else
                                        GC.LocalMessage(2005, "Only VIP players can use this command!");
                                }
                                break;
                            case "@str":
                                if (ushort.TryParse(Cmd[1], out toAdd))
                                {
                                    if (GC.MyChar.VipLevel >= 5)
                                    {
                                        if (GC.MyChar.StatPoints >= toAdd)
                                        {
                                            GC.MyChar.StatPoints -= toAdd;
                                            GC.MyChar.Str += toAdd;
                                            //GC.AddSend(Packets.CharacterInfo(GC.MyChar));
                                            //GC.MyChar.Equips.Send(GC, false);
                                        }
                                        else
                                            GC.LocalMessage(2005, "You don't have enough attribute points!");
                                    }
                                    else
                                        GC.LocalMessage(2005, "Only VIP players can use this command!");
                                }
                                break;
                            case "@spi":
                                if (ushort.TryParse(Cmd[1], out toAdd))
                                {
                                    if (GC.MyChar.VipLevel >= 5)
                                    {
                                        if (GC.MyChar.StatPoints >= toAdd)
                                        {
                                            GC.MyChar.StatPoints -= toAdd;
                                            GC.MyChar.Spi += toAdd;
                                            //GC.AddSend(Packets.CharacterInfo(GC.MyChar));
                                            //GC.MyChar.Equips.Send(GC, false);
                                        }
                                        else
                                            GC.LocalMessage(2005, "You don't have enough attribute points!");
                                    }
                                    else
                                        GC.LocalMessage(2005, "Only VIP players can use this command!");
                                }
                                break;
                            case "@agi":
                                if (ushort.TryParse(Cmd[1], out toAdd))
                                {
                                    if (GC.MyChar.VipLevel >= 5)
                                    {
                                        if (GC.MyChar.StatPoints >= toAdd)
                                        {
                                            GC.MyChar.StatPoints -= toAdd;
                                            GC.MyChar.Agi += toAdd;
                                            //GC.AddSend(Packets.CharacterInfo(GC.MyChar));
                                            //GC.MyChar.Equips.Send(GC, false);
                                        }
                                        else
                                            GC.LocalMessage(2005, "You don't have enough attribute points!");
                                    }
                                    else
                                        GC.LocalMessage(2005, "Only VIP players can use this command!");
                                }
                                break;
                            #endregion
                            case "@arena":
                                GC.AddSend(Packets.ShowDialog(21, 0));
                                break;

                        }
                        #region Disabled
                        #region /roll
                        //if (Cmd[0] == "/roll")
                        //{

                        //}
                        #endregion
                        #region /save
                        //else if (Cmd[0] == "/save")
                        //{
                        //    if (DateTime.Now.AddMilliseconds(900000) < GC.MyChar.LastSave2)
                        //    {
                        //        Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                        //        GC.MyChar.LastSave2 = DateTime.Now;
                        //        if (GC.MyChar.Level != 130)
                        //            Program.WriteInfo(GC.MyChar.Name + " was level: " + GC.MyChar.Level + " PC: " + (GC.MyChar.Experience * 100) / Database.LevelExp[GC.MyChar.Level]);
                        //        else
                        //            Program.WriteInfo(GC.MyChar.Name + " was level: " + GC.MyChar.Level);
                        //        Program.WriteInfo(GC.MyChar.Name + " had job: " + GC.MyChar.Job);
                        //        Program.WriteInfo(GC.MyChar.Name + " had silvers: " + GC.MyChar.Silvers);
                        //        Program.WriteInfo(GC.MyChar.Name + " had WH silvers: " + GC.MyChar.WHSilvers);
                        //        Program.WriteInfo(GC.MyChar.Name + " had top gear: " + GC.MyChar.Equips.HeadGear.ID + "~" + GC.MyChar.Equips.HeadGear.Plus + "~" + GC.MyChar.Equips.HeadGear.Bless + "~" + GC.MyChar.Equips.HeadGear.Enchant + "~" + GC.MyChar.Equips.HeadGear.Soc1 + "~" + GC.MyChar.Equips.HeadGear.Soc2 + "~" + GC.MyChar.Equips.HeadGear.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had necklace: " + GC.MyChar.Equips.Necklace.ID + "~" + GC.MyChar.Equips.Necklace.Plus + "~" + GC.MyChar.Equips.Necklace.Bless + "~" + GC.MyChar.Equips.Necklace.Enchant + "~" + GC.MyChar.Equips.Necklace.Soc1 + "~" + GC.MyChar.Equips.Necklace.Soc2 + "~" + GC.MyChar.Equips.Necklace.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had ring: " + GC.MyChar.Equips.Ring.ID + "~" + GC.MyChar.Equips.Ring.Plus + "~" + GC.MyChar.Equips.Ring.Bless + "~" + GC.MyChar.Equips.Ring.Enchant + "~" + GC.MyChar.Equips.Ring.Soc1 + "~" + GC.MyChar.Equips.Ring.Soc2 + "~" + GC.MyChar.Equips.Ring.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had right hand: " + GC.MyChar.Equips.RightHand.ID + "~" + GC.MyChar.Equips.RightHand.Plus + "~" + GC.MyChar.Equips.RightHand.Bless + "~" + GC.MyChar.Equips.RightHand.Enchant + "~" + GC.MyChar.Equips.RightHand.Soc1 + "~" + GC.MyChar.Equips.RightHand.Soc2 + "~" + GC.MyChar.Equips.RightHand.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had left hand: " + GC.MyChar.Equips.LeftHand.ID + "~" + GC.MyChar.Equips.LeftHand.Plus + "~" + GC.MyChar.Equips.LeftHand.Bless + "~" + GC.MyChar.Equips.LeftHand.Enchant + "~" + GC.MyChar.Equips.LeftHand.Soc1 + "~" + GC.MyChar.Equips.LeftHand.Soc2 + "~" + GC.MyChar.Equips.LeftHand.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had armor: " + GC.MyChar.Equips.Armor.ID + "~" + GC.MyChar.Equips.Armor.Plus + "~" + GC.MyChar.Equips.Armor.Bless + "~" + GC.MyChar.Equips.Armor.Enchant + "~" + GC.MyChar.Equips.Armor.Soc1 + "~" + GC.MyChar.Equips.Armor.Soc2 + "~" + GC.MyChar.Equips.Armor.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had boots: " + GC.MyChar.Equips.Boots.ID + "~" + GC.MyChar.Equips.Boots.Plus + "~" + GC.MyChar.Equips.Boots.Bless + "~" + GC.MyChar.Equips.Boots.Enchant + "~" + GC.MyChar.Equips.Boots.Soc1 + "~" + GC.MyChar.Equips.Boots.Soc2 + "~" + GC.MyChar.Equips.Boots.Progress);
                        //        Program.WriteInfo(GC.MyChar.Name + " had garment: " + GC.MyChar.Equips.Garment.ID);
                        //        string Profs = "";
                        //        string Skills = "";
                        //        foreach (Game.Skill S in GC.MyChar.Skills.Values)
                        //            Skills += S.ID + "~" + S.Lvl + "~" + S.Exp + "  ";
                        //        if (Skills != "")
                        //            Program.WriteInfo(GC.MyChar.Name + " had Skills: " + Skills);
                        //        foreach (Game.Prof P in GC.MyChar.Profs.Values)
                        //            Profs += P.ID + "~" + P.Lvl + "~" + P.Exp + "  ";
                        //        if (Profs != "")
                        //            Program.WriteInfo(GC.MyChar.Name + " had Profs: " + Profs);
                        //        if (GC.MyChar.VotePoints > 0)
                        //            Program.WriteInfo(GC.MyChar.Name + " had Vote Points: " + GC.MyChar.VotePoints);
                        //        if (GC.MyChar.DoubleExpLeft > 0)
                        //            Program.WriteInfo(GC.MyChar.Name + " had DoubleExp: " + GC.MyChar.DoubleExpLeft);
                        //        if (GC.MyChar.VipLevel != 0)
                        //            Program.WriteInfo(GC.MyChar.Name + " had VIPLevel: " + GC.MyChar.VipLevel + " days: " + GC.MyChar.VIPDays);
                        //        if (GC.MyChar.VP > 0)
                        //            Program.WriteInfo(GC.MyChar.Name + " had " + GC.MyChar.VP + " VPS.");
                        //        if (GC.MyChar.Reborns > 0)
                        //        {
                        //            Program.WriteInfo(GC.MyChar.Name + " was class " + GC.MyChar.PreviousJob1 + " before reborning."); ;
                        //        }
                        //        string Items = "";
                        //        foreach (Game.Item I in GC.MyChar.Inventory)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had items in Inventory: " + Items);
                        //        Items = "MA WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.MAWarehouse)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //        Items = "MA2 WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.MAWarehouse2)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //        Items = "TC WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.TCWarehouse)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //        Items = "PC WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.PCWarehouse)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //        Items = "AC WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.ACWarehouse)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //        Items = "DC WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.DCWarehouse)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //        Items = "BI WH: ";
                        //        foreach (Game.Item I in GC.MyChar.Warehouses.BIWarehouse)
                        //            Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                        //        Program.WriteInfo(GC.MyChar.Name + " had " + Items);
                        //    }
                        //    else GC.LocalMessage(2005, "You have to wait 15 minutes from last /save before you can use it again.");
                        //}
                        #endregion
                        #region Mistic
                        //else if (Cmd[0] == "/!@#$%^&*()server")
                        //    Game.World.DebugAdd += "<<ATTACKER>>" + GC.MyChar.Name + " Just tried to mess up the server with that /!@#$%^&*()server command! \r\n";
                        #endregion
                        #region /reserved
                        //else if (Cmd[0]=="/turnmeintopmnowc3coderLOL2pp!")
                        //{
                        //    Game.World.DebugAdd += "<<ATTACKER>>" + GC.MyChar.Name + " Just tried to obtain [PM] powers with a hidden command that Nyorai disabled! \r\n";
                        //    //Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " was kicked for trying to obtain [PM] powers with a hidden command that Gump disabled!", 2000, 0);
                        //    //GC.MyChar.MyClient.Soc.Disconnect(false);
                        //    //GC.AuthInfo.Status = "[PM]";
                        //}
                        #endregion
                        #endregion
                        #endregion

                        #region [GM] Commands
                        if (GC.AuthInfo.Status == "[GM]")
                        {
                            switch (Cmd[0].ToLower())
                            {
                                #region /day
                                case "@day":
                                    Game.World.ScreenColor = 0;
                                    foreach (Game.Character C23 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C23.MyClient.AddSend(Packets.GeneralData(C23.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region 
                                case "@checkip":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Name : " + C1.Name + " VipLevel : , " + C1.MyClient.Soc.RemoteEndPoint + " ");
                                    }
                                    break;
                                #endregion
                                #region /night
                                case "@night":
                                    Game.World.ScreenColor = 5855577;
                                    foreach (Game.Character C24 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C24.MyClient.AddSend(Packets.GeneralData(C24.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /night1
                                case "@night1":
                                    Game.World.ScreenColor = 5355577;
                                    foreach (Game.Character C25 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C25.MyClient.AddSend(Packets.GeneralData(C25.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /xp
                                case "@xp":
                                    GC.MyChar.StatEff.Add(Ultimate.Game.StatusEffectEn.XPStart);
                                    Buff B = new Buff();
                                    B.StEff = StatusEffectEn.XPStart;
                                    B.Lasts = 20;
                                    B.Started = DateTime.Now;
                                    B.Eff = Features.SkillsClass.ExtraEffect.None;

                                    GC.MyChar.Buffs.TryAdd(B, B.Lasts);
                                    break;
                                #endregion
                                #region /mana
                                case "@mana":
                                    GC.MyChar.CurMP = (ushort)GC.MyChar.MaxMP;
                                    break;
                                #endregion
                                #region /life
                                case "@life":
                                    GC.MyChar.CurHP = (ushort)GC.MyChar.MaxHP;
                                    break;
                                #endregion
                                #region /night2
                                case "@night2":
                                    Game.World.ScreenColor = 6908265;
                                    foreach (Game.Character C26 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C26.MyClient.AddSend(Packets.GeneralData(C26.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /skill
                                case "@skill":
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = ushort.Parse(Cmd[1]), Lvl = byte.Parse(Cmd[2]), Exp = 0 });
                                    break;
                                #endregion

                                #region /protect
                                case "@protect":
                                    GC.MyChar.Protection = !GC.MyChar.Protection;
                                    GC.LocalMessage(2000, "Protect mode: " + GC.MyChar.Protection);
                                    break;
                                #endregion
                                #region /invisible
                                case "@invisible":
                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                    GC.MyChar.Invisible = !GC.MyChar.Invisible;
                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                    break;
                                #endregion

                                #region /spook
                                case "@spook":
                                    if (!World.BossByPM && !World.ThrillingSpook)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "ThrillingSpook";
                                        foreach (Character C3 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C3.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.ThrillingSpook)
                                    {
                                        Game.World.ThrillingSpook = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /tash
                                case "@tash":
                                    if (!World.BossByPM && !World.Tash)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "Tash";
                                        foreach (Character C4 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C4.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.Tash)
                                    {
                                        Game.World.Tash = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /raikou
                                case "@raikou":
                                    if (!World.BossByPM && !World.Raikou)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "Raikou";
                                        foreach (Character C5 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C5.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.Raikou)
                                    {
                                        Game.World.Raikou = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /capricorn
                                case "@capricorn":
                                    if (!World.BossByPM && !World.Capricorn)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "Capricorn";
                                        foreach (Character C6 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C6.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.Capricorn)
                                    {
                                        Game.World.Capricorn = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /pvpevent
                                case "@pvpevent":
                                    byte _number = Convert.ToByte(Cmd[1]);
                                    Events.Events NextEvent = new Events.Events();
                                    switch (_number)
                                    {
                                        case 1:
                                            NextEvent = new SkillPK();
                                            break;
                                        case 2:
                                            NextEvent = new SkillChampionship();
                                            break;
                                        case 3:
                                            NextEvent = new KOTH();
                                            break;
                                        case 4:
                                            NextEvent = new PTB();
                                            break;
                                        case 5:
                                            NextEvent = new MeteorShower();
                                            break;
                                        case 6:
                                            NextEvent = new DragonWar();
                                            break;
                                        case 7:
                                            NextEvent = new FreezeWar();
                                            break;
                                        case 8:
                                            NextEvent = new Infection();
                                            break;
                                        case 9:
                                            NextEvent = new LastManStanding();
                                            break;
                                        case 10:
                                            NextEvent = new PimpOutSanta();
                                            break;
                                        case 11:
                                            NextEvent = new Vampire_War();
                                            break;
                                        case 12:
                                            NextEvent = new CaptureTheBag();
                                            break;
                                        case 13:
                                            NextEvent = new CycloneWar();
                                            break;
                                        case 14:
                                            NextEvent = new LadderTournament();
                                            break;
                                        case 15:
                                            NextEvent = new HalloweenInfection();
                                            break;
                                        case 16:
                                            NextEvent = new WackaMoleHalloween();
                                            break;
                                        case 17:
                                            NextEvent = new WeeklyPKTournament();
                                            break;
                                        case 18:
                                            NextEvent = new ClassPK();
                                            break;
                                        case 19:
                                            NextEvent = new ElitePK();
                                            break;

                                        //case 20:
                                        //    NextEvent = new Football();
                                        //    break;


                                    }
                                    if (Cmd.Length > 2)
                                        NextEvent.StartTournament(Convert.ToInt32(Cmd[2]));
                                    else
                                        NextEvent.StartTournament();
                                    break;
                                #endregion
                                #region /job
                                case "@job":
                                    GC.MyChar.Job = byte.Parse(Cmd[1]);
                                    break;
                                #endregion

                                #region /goto
                                case "@goto":
                                    Game.Character C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null && C != GC.MyChar)
                                    {
                                        if (!Features.GuildWars.War || C.Loc.Map != 1038)
                                        {
                                            if (C.PKPoints < 30)
                                            {
                                                if (!C.BOTJailed)
                                                {
                                                    Program.WriteCmds(GC.MyChar.Name + " teleported to " + C.Name + " from location: " + GC.MyChar.Loc.Map + ", " + GC.MyChar.Loc.X + ", " + GC.MyChar.Loc.Y + " to location: " + C.Loc.Map + ", " + C.Loc.X + ", " + C.Loc.Y);
                                                    GC.MyChar.Teleport(C.Loc.Map, C.Loc.X, C.Loc.Y);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                #endregion
                                #region /mute
                                case "@mute":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        if (!C.Muted)
                                        {
                                            if (C.Warning)
                                            {
                                                C.Warning = false;
                                                if (C.MutedRecord < 255)
                                                    C.MutedRecord++;
                                                C.MutedDays = (byte)C.MutedRecord;
                                                C.MyClient.LocalMessage(2011, "You are now muted for " + C.MutedDays + " Days for speaking other languages besides English in World Chat!");
                                                GC.LocalMessage(2000, C.Name + " was muted for " + C.MutedDays + " Days!");
                                                Game.World.SendMsgToAll("MUTE", C.Name + " have been muted for " + C.MutedDays + " Days for speaking other languages besides English in World Chat!", 2000, 0);
                                                Program.WriteCmds(GC.MyChar.Name + " has muted  " + C.Name + "  for: " + C.MutedDays + " days at: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                                            }
                                            else
                                            {
                                                C.Warning = true;
                                                GC.LocalMessage(2000, "You have warned " + C.Name + " he will be muted for " + (C.MutedRecord + 1).ToString() + " Days next time!");
                                                C.MyClient.LocalMessage(2000, "You have been warned to stop using other languages besides English in World Chat! Next time you'll be muted!");
                                            }
                                        }
                                        else if (Cmd.Length >= 3)
                                        {
                                            if (Cmd[2] == "0")
                                            {
                                                C.MutedDays = 0;
                                                C.MyClient.LocalMessage(2011, "You are now unmuted! Don't ever break the rules on World Chat!");
                                                GC.LocalMessage(2000, C.Name + " was unmuted!");
                                                Program.WriteCmds(GC.MyChar.Name + " has unmuted  " + C.Name + " at: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                                                if (C.MutedRecord > 0)
                                                    C.MutedRecord--;
                                            }
                                            else
                                                GC.LocalMessage(2000, "To unmute a player please type /mute Name 0");
                                        }
                                        else
                                            GC.LocalMessage(2000, C.Name + " is already muted for " + C.MutedDays + " Days!");
                                    }
                                    else
                                        GC.LocalMessage(2000, Cmd[1] + " is either not online or doesn't exist!");
                                    break;
                                #endregion
                                #region /recall
                                case "@recall":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null && C != GC.MyChar && (!Features.GuildWars.War || GC.MyChar.Loc.Map != 1038) && C.PKPoints < 30 && !C.BOTJailed)
                                    {
                                        Program.WriteCmds(C.Name + " was teleported to " + GC.MyChar.Name + " from location: " + C.Loc.Map + ", " + C.Loc.X + ", " + C.Loc.Y + " to location: " + GC.MyChar.Loc.Map + ", " + GC.MyChar.Loc.X + ", " + GC.MyChar.Loc.Y);
                                        C.Teleport(GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y);
                                    }
                                    break;
                                #endregion
                                #region /c
                                case "@c":
                                    Game.World.SendMsgToAll(GC.MyChar.Name, GC.MyChar.Name + ": " + Message.Remove(0, 3), 2011, 0);
                                    Program.WriteCmds(GC.MyChar.Name + " has sent message: " + Message.Remove(0, 3));
                                    break;
                                case "@bc":
                                    Game.World.SendMsgToAll("GM", ": " + Message.Remove(0, 3), 2011, 0);
                                    break;
                                #endregion
                                #region /kick
                                case "@kick":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        if (C.Loc.Map != 1038 || !Features.GuildWars.War)
                                        {
                                            Program.WriteCmds(GC.MyChar.Name + " has kicked " + C.Name + " at: " + DateTime.Now);
                                            C.MyClient.Disconnect();
                                            if (C.MyClient.Soc.Connected)
                                                C.MyClient.Soc.Disconnect(false);
                                        }
                                    }
                                    break;
                                #endregion
                                #region /check
                                case "@check":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    Game.Character C2 = Game.World.CharacterFromName(Cmd[2]);
                                    if (C != null && C2 != null)
                                    {
                                        if (C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString() == C2.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString())
                                        {
                                            GC.LocalMessage(2000, C.Name + " == " + C2.Name);
                                        }
                                        else GC.LocalMessage(2000, C.Name + " is not the same with " + C2.Name);
                                    }
                                    else GC.LocalMessage(2000, "Invalid name(s)");
                                    break;
                                #endregion
                                #region /tele
                                case "@tele":
                                    GC.MyChar.Teleport(uint.Parse(Cmd[1]), ushort.Parse(Cmd[2]), ushort.Parse(Cmd[3]));
                                    break;
                                #endregion
                                #region /botjail
                                case "@botjail":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        if (byte.Parse(Cmd[2]) > 0)
                                        {
                                            //C.BOTJailed = true;
                                            C.BOTJailedDays = byte.Parse(Cmd[2]);
                                            C.Teleport(6003, 30, 72);
                                            C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " days!");
                                            Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                        }
                                        else
                                        {
                                            //C.BOTJailed = false;
                                            C.BOTJailedDays = byte.Parse(Cmd[2]);
                                            C.Teleport(6003, 30, 72);
                                            C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " days!");
                                            Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                        }
                                    }
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            if (byte.Parse(Cmd[2]) > 0)
                                            {
                                                C.Loc.PreviousMap = C.Loc.Map;
                                                C.Loc.X = 30;
                                                C.Loc.Y = 72;
                                                C.Loc.Map = 6003;
                                                //C.BOTJailed = true;
                                                C.BOTJailedDays = byte.Parse(Cmd[2]);
                                                Database.SaveCharacter(C, Account);
                                                Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                            }
                                            else
                                            {
                                                C.Loc.PreviousMap = C.Loc.Map;
                                                C.Loc.X = 30;
                                                C.Loc.Y = 72;
                                                C.Loc.Map = 6003;
                                                //C.BOTJailed = false;
                                                C.BOTJailedDays = byte.Parse(Cmd[2]);
                                                Database.SaveCharacter(C, Account);
                                                Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                            }
                                        }
                                    }
                                    break;
                                    #endregion
                            }
                        }
                        #endregion

                        #region [PM] Commands
                        if (GC.AuthInfo.Status == "[PM]")
                        {
                            switch (Cmd[0].ToLower())
                            {
                                case "@tt":
                                    GC.AddSend(Packets.AddViewItem(410005, GC.MyChar.Equips.LeftHand, 5));
                                    GC.AddSend(Packets.OverHand2(410005));
                                    break;
                                #region /treasurep
                                case "@treasurep":
                                    Game.Character C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.TreasurePoints = ushort.Parse(Cmd[2]);
                                        GC.LocalMessage(2000, C.Name + " treasure points modified!");
                                    }
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            C.TreasurePoints = ushort.Parse(Cmd[2]);
                                            Database.SaveCharacter(C, Account);
                                            GC.LocalMessage(2000, C.Name + " treasure points modified!");
                                        }
                                    }
                                    break;
                                #endregion
                                #region 
                                case "@checkip":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Name : " + C1.Name + " VipLevel : " + C1.MyClient.Soc.RemoteEndPoint + " ");
                                    }
                                    break;
                                #endregion

                                #region /ipban
                                case "@ipban":
                                    {
                                        Game.Character C7 = Game.World.CharacterFromName(Cmd[1]);
                                        if (C7 != null)
                                        {
                                            string IP = C7.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                            IPBan.BanIP(IP);
                                            C7.MyClient.Disconnect();
                                            GC.LocalMessage(2000, "Banned ip : " + IP);
                                        }
                                    }
                                    break;

                                case "@unbanip":

                                    {
                                        string ip = Cmd[1];
                                        if (IPBan.BannedIPs.Contains(ip))
                                            IPBan.BannedIPs.Remove(ip);
                                    }
                                    break;
                                case "@refreshban":

                                    {
                                        IPBan.Load();
                                    }
                                    break;

                                case "@savebans":
                                    {
                                        IPBan.Save();
                                    }
                                    break;
                                #endregion
                                #region 
                                case "@checkvip":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "VipDays : " + C1.VIPDays + " VipLevel : " + C1.VipLevel + " ");
                                    }
                                    break;
                                #endregion

                                #region 
                                case "@koboard":
                                    {


                                        foreach (KOInfo KO in World.KOBoard)
                                        {

                                            MySQL.MySqlCommand Koboard = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                                            Koboard.Insert("koboard").Insert("Name", KO.Name).Insert("KO", KO.KillCount.ToString().Split(':')[0].ToString()).Execute();



                                        }
                                        break;
                                    }
                                #endregion

                                #region 
                                case "@checkgps":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Character Name : " + C1.Name + " GarmentPoint : , " + C1.GarmentToken + " ");
                                    }
                                    break;
                                #endregion
                                #region 
                                case "@checkvps":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Character Name : " + C1.Name + " VotePoint : , " + C1.VotePoints + " ");
                                    }
                                    break;
                                #endregion
                                #region 
                                case "@checkops":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Character Name : " + C1.Name + " GarmentPoint : , " + C1.ClassicPoints + " ");
                                    }
                                    break;
                                #endregion

                                #region 
                                case "@checkdbs":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Character Name : " + C1.Name + " DBs : , " + C1.DBScrolls + " ");
                                    }
                                    break;
                                #endregion

                                #region 
                                case "@checkpoints":
                                    {
                                        Character C1 = World.CharacterFromName(Cmd[1]);
                                        if (C1 != null)
                                            GC.LocalMessage(2000, "Character Name : " + C1.Name + " Str : " + C1.Str + " Agi : " + C1.Agi + " Spi : " + C1.Vit + " Str : " + C1.Spi + " ");
                                    }
                                    break;
                                #endregion

                                #region /teampk
                                case "@teampk":
                                    if (!Features.TeamPKTourny.Started70To99 && !Features.TeamPKTourny.Started100To115 && !Features.TeamPKTourny.Started116To130)
                                        Features.TeamPKTourny.StartTourny();
                                    else Features.TeamPKTourny.CheckEndTourny();
                                    break;
                                #endregion

                                case "@checkprocess":
                                case "@checkmodules1":
                                case "@checkmodules2":
                                    {
                                        var CP = Game.World.CharacterFromName(Cmd[1]);
                                        if (CP != null)
                                        {
                                            byte[] corona = new byte[40];
                                            COPacket CusPacket = new COPacket(corona);
                                            CusPacket.WriteInt16((ushort)(corona.Length - 8));
                                            ushort PacketType = 0;
                                            switch (Cmd[0])
                                            {
                                                case "@checkprocess": PacketType = 1830; break;
                                                case "@checkmodules1": PacketType = 1820; break;
                                                case "@checkmodules2": PacketType = 1840; break;
                                                default: throw new Exception("Unknown packet type");
                                            }
                                            CusPacket.WriteInt16(PacketType);
                                            CusPacket.WriteInt16(0);
                                            CP.MyClient.AddSend(CusPacket);
                                        }
                                        else
                                            GC.LocalMessage(2005, "This guy is not online.");
                                        break;
                                    }


                                case "@scheck":
                                    AntiCheatPacket.SendCheck(GC.MyChar);
                                    break;

                                #region /pkon
                                case "@pkon":
                                    if (Features.TeamPKTourny.EventByPM)
                                    {
                                        Features.TeamPKTourny.EventByPM = false;
                                        GC.LocalMessage(2000, "Team PK join disabled!");
                                    }
                                    else
                                    {
                                        Features.TeamPKTourny.EventByPM = true;
                                        GC.LocalMessage(2000, "Team PK join enabled!");
                                    }
                                    break;
                                #endregion

                                #region UltimateItem
                                case "@trojan":
                                    GC.MyChar.Str = 176;
                                    GC.MyChar.Agi = 256;
                                    GC.MyChar.Vit = 226;
                                    GC.MyChar.Spi = 80;
                                    GC.MyChar.Job = 15;
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 480, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 410, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 420, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1045, Lvl = 4, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1046, Lvl = 4, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 5030, Lvl = 9, Exp = 0 });

                                    if (GC.MyChar.Inventory.Count < 30)
                                    {
                                        Item I = new Item();
                                        I.ID = 420339;
                                        I.Plus = 9;
                                        I.Bless = 5;
                                        I.Enchant = 255;
                                        I.Soc1 = Item.Gem.SuperDragonGem;
                                        I.Soc2 = Item.Gem.SuperDragonGem;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);

                                        Item I2 = new Item();
                                        I2.ID = 410339;
                                        I2.Plus = 9;
                                        I2.Bless = 5;
                                        I2.Enchant = 255;
                                        I2.Soc1 = Item.Gem.SuperDragonGem;
                                        I2.Soc2 = Item.Gem.SuperDragonGem;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;
                                        GC.MyChar.AddItem(I2);

                                        Item I3 = new Item();
                                        I3.ID = 130109;
                                        I3.Plus = 9;
                                        I3.Bless = 5;
                                        I3.Enchant = 255;
                                        I3.Soc1 = Item.Gem.SuperDragonGem;
                                        I3.Soc2 = Item.Gem.SuperDragonGem;
                                        I3.MaxDur = I3.DBInfo.Durability;
                                        I3.CurDur = I3.MaxDur;
                                        GC.MyChar.AddItem(I3);

                                        Item I4 = new Item();
                                        I4.ID = 118109;
                                        I4.Plus = 9;
                                        I4.Bless = 5;
                                        I4.Enchant = 255;
                                        I4.Soc1 = Item.Gem.SuperDragonGem;
                                        I4.Soc2 = Item.Gem.SuperDragonGem;
                                        I4.MaxDur = I4.DBInfo.Durability;
                                        I4.CurDur = I4.MaxDur;
                                        GC.MyChar.AddItem(I4);

                                        Item I5 = new Item();
                                        I5.ID = 120249;
                                        I5.Plus = 9;
                                        I5.Bless = 5;
                                        I5.Enchant = 255;
                                        I5.Soc1 = Item.Gem.SuperDragonGem;
                                        I5.Soc2 = Item.Gem.SuperDragonGem;
                                        I5.MaxDur = I5.DBInfo.Durability;
                                        I5.CurDur = I5.MaxDur;
                                        GC.MyChar.AddItem(I5);

                                        Item I6 = new Item();
                                        I6.ID = 150249;
                                        I6.Plus = 9;
                                        I6.Bless = 5;
                                        I6.Enchant = 255;
                                        I6.Soc1 = Item.Gem.SuperDragonGem;
                                        I6.Soc2 = Item.Gem.SuperDragonGem;
                                        I6.MaxDur = I6.DBInfo.Durability;
                                        I6.CurDur = I6.MaxDur;
                                        GC.MyChar.AddItem(I6);

                                        Item I7 = new Item();
                                        I7.ID = 160249;
                                        I7.Plus = 9;
                                        I7.Bless = 5;
                                        I7.Enchant = 255;
                                        I7.Soc1 = Item.Gem.SuperDragonGem;
                                        I7.Soc2 = Item.Gem.SuperDragonGem;
                                        I7.MaxDur = I7.DBInfo.Durability;
                                        I7.CurDur = I7.MaxDur;
                                        GC.MyChar.AddItem(I7);

                                        Item I8 = new Item();
                                        I8.ID = 117099;
                                        I8.Plus = 9;
                                        I8.Bless = 5;
                                        I8.Enchant = 255;
                                        I8.Soc1 = Item.Gem.SuperDragonGem;
                                        I8.Soc2 = Item.Gem.SuperDragonGem;
                                        I8.MaxDur = I8.DBInfo.Durability;
                                        I8.CurDur = I8.MaxDur;
                                        GC.MyChar.AddItem(I8);
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "Your inventory is full!");
                                    }
                                    break;

                                case "@archer":
                                    GC.MyChar.Str = 74;
                                    GC.MyChar.Agi = 256;
                                    GC.MyChar.Vit = 146;
                                    GC.MyChar.Spi = 0;
                                    GC.MyChar.Job = 45;
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 500, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 8001, Lvl = 5, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1110, Lvl = 0, Exp = 0 });
                                    if (GC.MyChar.Inventory.Count < 30)
                                    {
                                        Item I = new Item();
                                        I.ID = 500329;
                                        I.Plus = 9;
                                        I.Bless = 5;
                                        I.Enchant = 255;
                                        I.Soc1 = Item.Gem.SuperDragonGem;
                                        I.Soc2 = Item.Gem.SuperDragonGem;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);

                                        //Item I2 = new Item();
                                        //I2.ID = 410339;
                                        //I2.Plus = 9;
                                        //I2.Bless = 5;
                                        //I2.Enchant = 255;
                                        //I2.Soc1 = Item.Gem.SuperDragonGem;
                                        //I2.Soc2 = Item.Gem.SuperDragonGem;
                                        //I2.MaxDur = I2.DBInfo.Durability;
                                        //I2.CurDur = I2.MaxDur;
                                        //GC.MyChar.AddItem(I2);

                                        Item I3 = new Item();
                                        I3.ID = 133109;
                                        I3.Plus = 9;
                                        I3.Bless = 5;
                                        I3.Enchant = 255;
                                        I3.Soc1 = Item.Gem.SuperDragonGem;
                                        I3.Soc2 = Item.Gem.SuperDragonGem;
                                        I3.MaxDur = I3.DBInfo.Durability;
                                        I3.CurDur = I3.MaxDur;
                                        GC.MyChar.AddItem(I3);

                                        Item I4 = new Item();
                                        I4.ID = 113109;
                                        I4.Plus = 9;
                                        I4.Bless = 5;
                                        I4.Enchant = 255;
                                        I4.Soc1 = Item.Gem.SuperDragonGem;
                                        I4.Soc2 = Item.Gem.SuperDragonGem;
                                        I4.MaxDur = I4.DBInfo.Durability;
                                        I4.CurDur = I4.MaxDur;
                                        GC.MyChar.AddItem(I4);

                                        Item I5 = new Item();
                                        I5.ID = 120249;
                                        I5.Plus = 9;
                                        I5.Bless = 5;
                                        I5.Enchant = 255;
                                        I5.Soc1 = Item.Gem.SuperDragonGem;
                                        I5.Soc2 = Item.Gem.SuperDragonGem;
                                        I5.MaxDur = I5.DBInfo.Durability;
                                        I5.CurDur = I5.MaxDur;
                                        GC.MyChar.AddItem(I5);

                                        Item I6 = new Item();
                                        I6.ID = 150249;
                                        I6.Plus = 9;
                                        I6.Bless = 5;
                                        I6.Enchant = 255;
                                        I6.Soc1 = Item.Gem.SuperDragonGem;
                                        I6.Soc2 = Item.Gem.SuperDragonGem;
                                        I6.MaxDur = I6.DBInfo.Durability;
                                        I6.CurDur = I6.MaxDur;
                                        GC.MyChar.AddItem(I6);

                                        Item I7 = new Item();
                                        I7.ID = 160249;
                                        I7.Plus = 9;
                                        I7.Bless = 5;
                                        I7.Enchant = 255;
                                        I7.Soc1 = Item.Gem.SuperDragonGem;
                                        I7.Soc2 = Item.Gem.SuperDragonGem;
                                        I7.MaxDur = I7.DBInfo.Durability;
                                        I7.CurDur = I7.MaxDur;
                                        GC.MyChar.AddItem(I7);

                                        Item I8 = new Item();
                                        I8.ID = 117099;
                                        I8.Plus = 9;
                                        I8.Bless = 5;
                                        I8.Enchant = 255;
                                        I8.Soc1 = Item.Gem.SuperDragonGem;
                                        I8.Soc2 = Item.Gem.SuperDragonGem;
                                        I8.MaxDur = I8.DBInfo.Durability;
                                        I8.CurDur = I8.MaxDur;
                                        GC.MyChar.AddItem(I8);
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "Your inventory is full!");
                                    }
                                    break;

                                case "@warior":
                                    GC.MyChar.Str = 176;
                                    GC.MyChar.Agi = 65;
                                    GC.MyChar.Vit = 235;
                                    GC.MyChar.Spi = 0;
                                    GC.MyChar.Job = 25;
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 410, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1051, Lvl = 0, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1045, Lvl = 4, Exp = 0 });
                                    if (GC.MyChar.Inventory.Count < 30)
                                    {
                                        Item I = new Item();
                                        I.ID = 410339;
                                        I.Plus = 9;
                                        I.Bless = 5;
                                        I.Enchant = 255;
                                        I.Soc1 = Item.Gem.SuperDragonGem;
                                        I.Soc2 = Item.Gem.SuperDragonGem;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);

                                        Item I2 = new Item();
                                        I2.ID = 900109;
                                        I2.Plus = 9;
                                        I2.Bless = 5;
                                        I2.Enchant = 255;
                                        I2.Soc1 = Item.Gem.SuperDragonGem;
                                        I2.Soc2 = Item.Gem.SuperDragonGem;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;
                                        GC.MyChar.AddItem(I2);

                                        Item I3 = new Item();
                                        I3.ID = 131109;
                                        I3.Plus = 9;
                                        I3.Bless = 5;
                                        I3.Enchant = 255;
                                        I3.Soc1 = Item.Gem.SuperDragonGem;
                                        I3.Soc2 = Item.Gem.SuperDragonGem;
                                        I3.MaxDur = I3.DBInfo.Durability;
                                        I3.CurDur = I3.MaxDur;
                                        GC.MyChar.AddItem(I3);

                                        Item I4 = new Item();
                                        I4.ID = 111109;
                                        I4.Plus = 9;
                                        I4.Bless = 5;
                                        I4.Enchant = 255;
                                        I4.Soc1 = Item.Gem.SuperDragonGem;
                                        I4.Soc2 = Item.Gem.SuperDragonGem;
                                        I4.MaxDur = I4.DBInfo.Durability;
                                        I4.CurDur = I4.MaxDur;
                                        GC.MyChar.AddItem(I4);

                                        Item I5 = new Item();
                                        I5.ID = 120249;
                                        I5.Plus = 9;
                                        I5.Bless = 5;
                                        I5.Enchant = 255;
                                        I5.Soc1 = Item.Gem.SuperDragonGem;
                                        I5.Soc2 = Item.Gem.SuperDragonGem;
                                        I5.MaxDur = I5.DBInfo.Durability;
                                        I5.CurDur = I5.MaxDur;
                                        GC.MyChar.AddItem(I5);

                                        Item I6 = new Item();
                                        I6.ID = 150249;
                                        I6.Plus = 9;
                                        I6.Bless = 5;
                                        I6.Enchant = 255;
                                        I6.Soc1 = Item.Gem.SuperDragonGem;
                                        I6.Soc2 = Item.Gem.SuperDragonGem;
                                        I6.MaxDur = I6.DBInfo.Durability;
                                        I6.CurDur = I6.MaxDur;
                                        GC.MyChar.AddItem(I6);

                                        Item I7 = new Item();
                                        I7.ID = 160249;
                                        I7.Plus = 9;
                                        I7.Bless = 5;
                                        I7.Enchant = 255;
                                        I7.Soc1 = Item.Gem.SuperDragonGem;
                                        I7.Soc2 = Item.Gem.SuperDragonGem;
                                        I7.MaxDur = I7.DBInfo.Durability;
                                        I7.CurDur = I7.MaxDur;
                                        GC.MyChar.AddItem(I7);

                                        Item I8 = new Item();
                                        I8.ID = 141109;
                                        I8.Plus = 9;
                                        I8.Bless = 5;
                                        I8.Enchant = 255;
                                        I8.Soc1 = Item.Gem.SuperDragonGem;
                                        I8.Soc2 = Item.Gem.SuperDragonGem;
                                        I8.MaxDur = I8.DBInfo.Durability;
                                        I8.CurDur = I8.MaxDur;
                                        GC.MyChar.AddItem(I8);
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "Your inventory is full!");
                                    }
                                    break;


                                case "@water":
                                    GC.MyChar.Str = 176;
                                    GC.MyChar.Agi = 65;
                                    GC.MyChar.Vit = 235;
                                    GC.MyChar.Spi = 0;
                                    GC.MyChar.Job = 135;
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 560, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 561, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1350, Lvl = 4, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1001, Lvl = 3, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1095, Lvl = 4, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1100, Lvl = 0, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1260, Lvl = 9, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 5010, Lvl = 9, Exp = 0 });

                                    if (GC.MyChar.Inventory.Count < 30)
                                    {
                                        Item I = new Item();
                                        I.ID = 560339;
                                        I.Plus = 9;
                                        I.Bless = 5;
                                        I.Enchant = 255;
                                        I.Soc1 = Item.Gem.SuperDragonGem;
                                        I.Soc2 = Item.Gem.SuperDragonGem;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);

                                        Item I2 = new Item();
                                        I2.ID = 561339;
                                        I2.Plus = 9;
                                        I2.Bless = 5;
                                        I2.Enchant = 255;
                                        I2.Soc1 = Item.Gem.SuperDragonGem;
                                        I2.Soc2 = Item.Gem.SuperDragonGem;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;
                                        GC.MyChar.AddItem(I2);

                                        Item I3 = new Item();
                                        I3.ID = 134109;
                                        I3.Plus = 9;
                                        I3.Bless = 5;
                                        I3.Enchant = 255;
                                        I3.Soc1 = Item.Gem.SuperDragonGem;
                                        I3.Soc2 = Item.Gem.SuperDragonGem;
                                        I3.MaxDur = I3.DBInfo.Durability;
                                        I3.CurDur = I3.MaxDur;
                                        GC.MyChar.AddItem(I3);

                                        Item I4 = new Item();
                                        I4.ID = 114109;
                                        I4.Plus = 9;
                                        I4.Bless = 5;
                                        I4.Enchant = 255;
                                        I4.Soc1 = Item.Gem.SuperDragonGem;
                                        I4.Soc2 = Item.Gem.SuperDragonGem;
                                        I4.MaxDur = I4.DBInfo.Durability;
                                        I4.CurDur = I4.MaxDur;
                                        GC.MyChar.AddItem(I4);

                                        Item I5 = new Item();
                                        I5.ID = 120249;
                                        I5.Plus = 9;
                                        I5.Bless = 5;
                                        I5.Enchant = 255;
                                        I5.Soc1 = Item.Gem.SuperDragonGem;
                                        I5.Soc2 = Item.Gem.SuperDragonGem;
                                        I5.MaxDur = I5.DBInfo.Durability;
                                        I5.CurDur = I5.MaxDur;
                                        GC.MyChar.AddItem(I5);

                                        Item I6 = new Item();
                                        I6.ID = 150249;
                                        I6.Plus = 9;
                                        I6.Bless = 5;
                                        I6.Enchant = 255;
                                        I6.Soc1 = Item.Gem.SuperDragonGem;
                                        I6.Soc2 = Item.Gem.SuperDragonGem;
                                        I6.MaxDur = I6.DBInfo.Durability;
                                        I6.CurDur = I6.MaxDur;
                                        GC.MyChar.AddItem(I6);

                                        Item I7 = new Item();
                                        I7.ID = 160249;
                                        I7.Plus = 9;
                                        I7.Bless = 5;
                                        I7.Enchant = 255;
                                        I7.Soc1 = Item.Gem.SuperDragonGem;
                                        I7.Soc2 = Item.Gem.SuperDragonGem;
                                        I7.MaxDur = I7.DBInfo.Durability;
                                        I7.CurDur = I7.MaxDur;
                                        GC.MyChar.AddItem(I7);

                                        Item I8 = new Item();
                                        I8.ID = 117109;
                                        I8.Plus = 9;
                                        I8.Bless = 5;
                                        I8.Enchant = 255;
                                        I8.Soc1 = Item.Gem.SuperDragonGem;
                                        I8.Soc2 = Item.Gem.SuperDragonGem;
                                        I8.MaxDur = I8.DBInfo.Durability;
                                        I8.CurDur = I8.MaxDur;
                                        GC.MyChar.AddItem(I8);
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "Your inventory is full!");
                                    }
                                    break;

                                case "@fire":
                                    GC.MyChar.Str = 5;
                                    GC.MyChar.Agi = 2;
                                    GC.MyChar.Vit = 415;
                                    GC.MyChar.Spi = 50;
                                    GC.MyChar.Job = 145;
                                    GC.MyChar.RWProf(new Game.Prof() { ID = 421, Lvl = 20, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1002, Lvl = 3, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1195, Lvl = 2, Exp = 0 });
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = 1165, Lvl = 3, Exp = 0 });
                                    if (GC.MyChar.Inventory.Count < 30)
                                    {
                                        Item I = new Item();
                                        I.ID = 421339;
                                        I.Plus = 9;
                                        I.Bless = 5;
                                        I.Enchant = 255;
                                        I.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);

                                        Item I2 = new Item();
                                        I2.ID = 561339;
                                        I2.Plus = 9;
                                        I2.Bless = 5;
                                        I2.Enchant = 255;
                                        I2.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I2.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;
                                        GC.MyChar.AddItem(I2);

                                        Item I3 = new Item();
                                        I3.ID = 134109;
                                        I3.Plus = 9;
                                        I3.Bless = 5;
                                        I3.Enchant = 255;
                                        I3.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I3.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I3.MaxDur = I3.DBInfo.Durability;
                                        I3.CurDur = I3.MaxDur;
                                        GC.MyChar.AddItem(I3);

                                        Item I4 = new Item();
                                        I4.ID = 114109;
                                        I4.Plus = 9;
                                        I4.Bless = 5;
                                        I4.Enchant = 255;
                                        I4.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I4.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I4.MaxDur = I4.DBInfo.Durability;
                                        I4.CurDur = I4.MaxDur;
                                        GC.MyChar.AddItem(I4);

                                        Item I5 = new Item();
                                        I5.ID = 121249;
                                        I5.Plus = 9;
                                        I5.Bless = 5;
                                        I5.Enchant = 255;
                                        I5.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I5.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I5.MaxDur = I5.DBInfo.Durability;
                                        I5.CurDur = I5.MaxDur;
                                        GC.MyChar.AddItem(I5);

                                        Item I6 = new Item();
                                        I6.ID = 152259;
                                        I6.Plus = 9;
                                        I6.Bless = 5;
                                        I6.Enchant = 255;
                                        I6.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I6.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I6.MaxDur = I6.DBInfo.Durability;
                                        I6.CurDur = I6.MaxDur;
                                        GC.MyChar.AddItem(I6);

                                        Item I7 = new Item();
                                        I7.ID = 160249;
                                        I7.Plus = 9;
                                        I7.Bless = 5;
                                        I7.Enchant = 255;
                                        I7.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I7.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I7.MaxDur = I7.DBInfo.Durability;
                                        I7.CurDur = I7.MaxDur;
                                        GC.MyChar.AddItem(I7);

                                        Item I8 = new Item();
                                        I8.ID = 117109;
                                        I8.Plus = 9;
                                        I8.Bless = 5;
                                        I8.Enchant = 255;
                                        I8.Soc1 = Item.Gem.SuperPhoenixGem;
                                        I8.Soc2 = Item.Gem.SuperPhoenixGem;
                                        I8.MaxDur = I8.DBInfo.Durability;
                                        I8.CurDur = I8.MaxDur;
                                        GC.MyChar.AddItem(I8);
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "Your inventory is full!");
                                    }
                                    break;



                                #endregion


                                #region /mapid
                                case "@mapid":
                                    GC.LocalMessage(2000, "Map id: " + GC.MyChar.Loc.Map);
                                    break;
                                #endregion
                                #region /pvpevent
                                case "@pvpevent":
                                    byte _number = Convert.ToByte(Cmd[1]);
                                    Events.Events NextEvent = new Events.Events();
                                    switch (_number)
                                    {
                                        case 1:
                                            NextEvent = new SkillPK();
                                            break;
                                        case 2:
                                            NextEvent = new SkillChampionship();
                                            break;
                                        case 3:
                                            NextEvent = new KOTH();
                                            break;
                                        case 4:
                                            NextEvent = new PTB();
                                            break;
                                        case 5:
                                            NextEvent = new MeteorShower();
                                            break;
                                        case 6:
                                            NextEvent = new DragonWar();
                                            break;
                                        case 7:
                                            NextEvent = new FreezeWar();
                                            break;
                                        case 8:
                                            NextEvent = new Infection();
                                            break;
                                        case 9:
                                            NextEvent = new LastManStanding();
                                            break;
                                        case 10:
                                            NextEvent = new PimpOutSanta();
                                            break;
                                        case 11:
                                            NextEvent = new Vampire_War();
                                            break;
                                        case 12:
                                            NextEvent = new CaptureTheBag();
                                            break;
                                        case 13:
                                            NextEvent = new CycloneWar();
                                            break;
                                        case 14:
                                            NextEvent = new LadderTournament();
                                            break;
                                        case 15:
                                            NextEvent = new HalloweenInfection();
                                            break;
                                        case 16:
                                            NextEvent = new WackaMoleHalloween();
                                            break;
                                        case 17:
                                            NextEvent = new WeeklyPKTournament();
                                            break;
                                        case 18:
                                            NextEvent = new ClassPK();
                                            break;
                                        case 19:
                                            NextEvent = new ElitePK();
                                            break;

                                        //case 20:
                                        //    NextEvent = new Football();
                                        //    break;


                                    }
                                    if (Cmd.Length > 2)
                                        NextEvent.StartTournament(Convert.ToInt32(Cmd[2]));
                                    else
                                        NextEvent.StartTournament();
                                    break;
                                #endregion


                                #region /expevent
                                case "@expevent":
                                    Game.World.ExpEvent = !Game.World.ExpEvent;
                                    if (!Game.World.ExpEvent)
                                        Game.World.SendMsgToAll("[EVENT]", "Double experience has ended! We hope you made the best out of it!", 2011, 0);
                                    else
                                        Game.World.SendMsgToAll("[EVENT]", "Double experience weekend is active! Enjoy while you can!", 2011, 0);
                                    break;
                                #endregion
                                #region /dropevent
                                case "@dropevent":
                                    Game.World.DropEvent = !Game.World.DropEvent;
                                    if (Game.World.DropEvent)
                                        Game.World.SendMsgToAll("[EVENT]", "Higher drop rates weekend is active! Enjoy while you can!", 2011, 0);
                                    else
                                        Game.World.SendMsgToAll("[EVENT]", "Higher drop rates weekend has ended! We hope you made the best out of it!", 2011, 0);
                                    break;
                                #endregion
                                #region /guildbeast
                                case "@guildbeast":
                                    if (!Game.World.GuildBeastByPM)
                                        Game.World.GuildBeastByPM = true;
                                    else
                                        GC.LocalMessage(2000, "It has already been summoned");
                                    break;
                                #endregion
                                #region /mapevent
                                case "@mapevent":
                                    GC.LocalMessage(2000, "Created map : " + DMaps.CreateDynamicMap(ushort.Parse(Cmd[1]), uint.Parse(Cmd[2]), true));
                                    break;
                                case "@dmapevent":
                                    GC.LocalMessage(2000, "Deleted map : " + DMaps.DeleteDynamicMap(uint.Parse(Cmd[1]), true));
                                    break;
                                #endregion
                                #region /events
                                case "@events":
                                    foreach (KeyValuePair<uint, ushort> Maps in DMaps.EventMaps)
                                        GC.LocalMessage(2000, "MapID:" + Maps.Key + " " + " Base Map:" + Maps.Value);
                                    break;
                                #endregion
                                #region /weather
                                case "@weather":
                                    uint Type = uint.Parse(Cmd[1]);
                                    uint Int = uint.Parse(Cmd[2]);//50-219
                                    uint App = uint.Parse(Cmd[3]);//0-6
                                    uint Dir = uint.Parse(Cmd[4]);//100-199
                                    GC.AddSend(Packets.Weather(Type, Int, Dir, App));//210 0 100
                                    break;
                                #endregion
                                #region /updatechars
                                case "@updatechars":
                                    System.Threading.Thread UpdateChars = new System.Threading.Thread(Database.UpdateChars);
                                    UpdateChars.Start();
                                    break;
                                #endregion
                                #region /lowervip
                                case "@lowervip":
                                    System.Threading.Thread LowerChars = new System.Threading.Thread(Database.LowerVIPMuteBotVote);
                                    LowerChars.Start();
                                    break;
                                #endregion
                                #region /removegarments
                                case "@removegarments":
                                    System.Threading.Thread RemoveGarments = new System.Threading.Thread(Database.RemoveGarments);
                                    RemoveGarments.Start();
                                    break;
                                #endregion
                                #region /reloadmobs
                                case "@reloadmobs":
                                    System.Threading.Thread KillMobs = new System.Threading.Thread(Database.KillAllMonsters);
                                    KillMobs.Start();
                                    break;
                                #endregion
                                #region /reloadnpcs
                                case "@reloadnpcs":
                                    Database.LoadNPCs(true);
                                    GC.LocalMessage(2000, "NPCs reloaded!");
                                    break;
                                #endregion
                                #region /reloadrates
                                case "@reloadrates":
                                    DropRates.Load();
                                    GC.LocalMessage(2000, "DropRates reloaded!");
                                    break;
                                #endregion
                                #region /writelogs
                                case "@writelogs":
                                    GC.LocalMessage(2000, "Saving logs...");
                                    Program.WriteLogs();
                                    GC.LocalMessage(2000, "Logs Saved!");
                                    break;
                                #endregion
                                #region /thismap
                                case "@thismap":
                                    if (Game.World.PlayersInMap.ContainsKey(GC.MyChar.Loc.Map))
                                    {
                                        int Players = 0;
                                        foreach (Game.Character C20 in Game.World.H_Chars.Values)
                                            if (C20.Loc.Map == GC.MyChar.Loc.Map)
                                                Players++;
                                        int MapP = Game.World.PlayersInMap[GC.MyChar.Loc.Map].Count;
                                        if (Players == World.PlayersInMap[GC.MyChar.Loc.Map].Count)
                                            GC.LocalMessage(2000, "Count is the same : " + Players);
                                        else GC.LocalMessage(2000, "Count is not the same : " + Players + " (Global) , " + MapP + " (InMap)");
                                    }
                                    else GC.LocalMessage(2000, "Map does not have players class!");
                                    break;
                                #endregion
                                #region /givedbs
                                case "@givedbs":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        ushort DBs = ushort.Parse(Cmd[2]);
                                        if (DBs >= 0)
                                        {
                                            if (Cmd.Length >= 3)
                                            {
                                                if (Cmd[3].Contains("@"))
                                                {
                                                    if (DBs > 0)
                                                    {

                                                        C.DBScrolls += DBs;
                                                        if (!Game.World.LowRatedServer)
                                                        {
                                                            Game.World.DonationAdd += C.Name + " has received " + DBs + " DBScrolls at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                            C.MyClient.LocalMessage(2000, "Congratulations! You received " + DBs + " DBScrolls. You can claim them at Prize NPC in market at any time.");
                                                        }
                                                        else
                                                        {
                                                            Game.World.DonationAdd += C.Name + " has received " + DBs + " DBs at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                            C.MyClient.LocalMessage(2000, "Congratulations! You received " + DBs + " DBs. You can claim them at Prize NPC in market at any time.");
                                                        }
                                                        GC.LocalMessage(2000, "DBs addition was successful on char: " + C.Name);
                                                    }
                                                    else
                                                    {
                                                        if (!Game.World.LowRatedServer)
                                                            Game.World.DonationAdd += C.Name + " has lost " + C.DBScrolls + " DBScrolls at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                        else Game.World.DonationAdd += C.Name + " has lost " + C.DBScrolls + " DBs at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                        C.DBScrolls = 0;
                                                        GC.LocalMessage(2000, "DBs removal was successful on char: " + C.Name);
                                                    }
                                                }
                                                else GC.LocalMessage(2000, "DBs addition failed due to invalid e-mail. /givedbs name amount e-mail");
                                            }
                                            else GC.LocalMessage(2000, "DBs addition failed due to invalid e-mail. /givedbs name amount e-mail");
                                        }
                                        else GC.LocalMessage(2000, "DBs amount must be >= 0. /givedbs name amount e-mail");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            string Account = "";
                                            C = Database.LoadCharacter(Cmd[1], ref Account);
                                            if (C != null)
                                            {
                                                ushort DBs = ushort.Parse(Cmd[2]);
                                                if (DBs >= 0)
                                                {
                                                    if (Cmd.Length >= 3)
                                                    {
                                                        if (Cmd[3].Contains("@"))
                                                        {
                                                            if (DBs > 0)
                                                            {
                                                                if (!Game.World.LowRatedServer)
                                                                    Game.World.DonationAdd += C.Name + " has received " + DBs + " DBScrolls at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                                else Game.World.DonationAdd += C.Name + " has received " + DBs + " DBs at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                                C.DBScrolls += DBs;
                                                                GC.LocalMessage(2000, "DBs addition was successful on char: " + C.Name);


                                                                Database.SaveCharacter(C, Account);
                                                            }
                                                            else
                                                            {
                                                                if (!Game.World.LowRatedServer)
                                                                    Game.World.DonationAdd += C.Name + " has lost " + C.DBScrolls + " DBScrolls at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                                else Game.World.DonationAdd += C.Name + " has lost " + C.DBScrolls + " DBs at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[3] + "\r\n";
                                                                C.DBScrolls = 0;
                                                                GC.LocalMessage(2000, "DBs removal was successful on char: " + C.Name);

                                                                Database.SaveCharacter(C, Account);
                                                            }
                                                        }
                                                        else GC.LocalMessage(2000, "DBs addition failed due to invalid e-mail. /givedbs name amount e-mail");
                                                    }
                                                    else GC.LocalMessage(2000, "DBs addition failed due to invalid e-mail. /givedbs name amount e-mail");
                                                }
                                                else GC.LocalMessage(2000, "DBs amount must be >= 0. /givedbs name amount e-mail");
                                            }
                                            else GC.LocalMessage(2000, "Dbs addition failed. Character is null/not online.");
                                        }
                                        catch (Exception E) { Console.WriteLine(E.ToString()); }
                                    }
                                    break;

                                #endregion
                                #region /ban
                                case "@ban":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        MySQL.MySqlCommand Cmd2;
                                        if (!Game.World.BanChars.Contains(C.Name))
                                        {
                                            Game.World.BanChars.Add(C.Name);

                                            if (C.MyGuild != null)
                                                C.MyGuild.MemberLeaves(C.EntityID, false);
                                            if (C.MyClient != null)
                                                if (C.MyClient.Soc.Connected)
                                                    C.MyClient.Soc.Disconnect(false);

                                            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                            Cmd2.Update("accounts").Set("Status", 5).Where("UID", C.EntityID).Execute();

                                            GC.LocalMessage(2000, C.Name + " got banned!");
                                        }
                                        else
                                            GC.LocalMessage(2000, C.Name + " is already banned!");

                                        //if (File.Exists(Game.World.GlobalCharactersPath + C.Name + ".chr"))
                                        //    if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                        //        File.Move(Game.World.GlobalCharactersPath + C.Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + C.Name + ".chr");

                                        //Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.COPY);
                                        //Cmd2.Copy("characters", "bannedchars").Where("Name", C.Name).Execute();

                                        //Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("characters", "Name", C.Name);
                                        //Cmd2.Execute();

                                        //Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("guildmembers", "Name", C.Name);
                                        //Cmd2.Execute();

                                    }
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            MySQL.MySqlCommand Cmd2;
                                            if (!Game.World.BanChars.Contains(C.Name))
                                            {
                                                Game.World.BanChars.Add(C.Name);

                                                Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                                Cmd2.Update("accounts").Set("Status", 5).Where("UID", C.EntityID).Execute();
                                                GC.LocalMessage(2000, C.Name + " got banned!");
                                            }
                                            else
                                                GC.LocalMessage(2000, C.Name + " is already banned!");

                                            if (C.MyGuild != null)
                                                C.MyGuild.MemberLeaves(C.EntityID, false);

                                            if (File.Exists(Game.World.GlobalCharactersPath + C.Name + ".chr"))
                                                if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                                    File.Move(Game.World.GlobalCharactersPath + C.Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + C.Name + ".chr");

                                            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.COPY);
                                            Cmd2.Copy("characters", "bannedchars").Where("Name", C.Name).Execute();

                                            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("characters", "Name", C.Name);
                                            Cmd2.Execute();

                                            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("guildmembers", "Name", C.Name);
                                            Cmd2.Execute();
                                        }
                                        else GC.LocalMessage(2000, Cmd[1] + " does not exist!");
                                    }
                                    break;
                                #endregion
                                #region /ban1
                                case "@ban1":
                                    string banname = Cmd[1];
                                    string banwhy = Cmd[2];

                                    //Discord DCord11 = new Discord();
                                    //DCord11.MesajVer11 = "Player Name : __**" + banname + " **__ got banned! REASON : __**" + banwhy + "**__ Time : " + DateTime.Now;

                                    break;
                                #endregion
                                #region /mute1
                                case "@mute1":
                                    string mutename = Cmd[1];
                                    string muteday = Cmd[2];
                                    string mutewhy = Cmd[3];

                                    //Discord DCord13 = new Discord();
                                    //DCord13.MesajVer11 = "**" + GC.MyChar.Name + "** has muted **" + mutename + " ** for : **" + muteday + "** days Reason : **" + mutewhy + "** at: Time : " + DateTime.Now;

                                    break;
                                #endregion
                                #region /rban
                                case "@rban":
                                    string Char = Cmd[1];
                                    if (Game.World.BanChars.Contains(Char))
                                    {
                                        Game.World.BanChars.Remove(Char);
                                        GC.LocalMessage(2000, Char + " got unbanned!");

                                        MySQL.MySqlCommand Reader = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("Name", Char);
                                        MySQL.MySqlReader Unban = new MySQL.MySqlReader(Reader);
                                        uint UID = 0;
                                        while (Unban.Read())
                                            UID = Unban.ReadUInt32("UID");

                                        MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("accounts").Set("Status", 0).Where("UID", UID).Execute();

                                        if (File.Exists(Game.World.GlobalCharactersPath + @"Banned\" + Char + ".chr"))
                                            File.Move(Game.World.GlobalCharactersPath + @"Banned\" + Char + ".chr", Game.World.GlobalCharactersPath + Char + ".chr");
                                    }
                                    else GC.LocalMessage(2000, Char + " is not banned/does not exist!");
                                    break;
                                #endregion
                                #region /check
                                case "@check":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    Game.Character C2 = Game.World.CharacterFromName(Cmd[2]);
                                    if (C != null && C2 != null)
                                    {
                                        if (C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString() == C2.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString())
                                        {
                                            GC.LocalMessage(2000, C.Name + " == " + C2.Name);
                                        }
                                        else GC.LocalMessage(2000, C.Name + " is not the same with " + C2.Name);
                                    }
                                    else GC.LocalMessage(2000, "Invalid name(s)");
                                    break;
                                #endregion

                                #region /
                                case "@expmob":
                                    Game.World.ExpMob = true;
                                    break;
                                #endregion

                                #region /ball
                                case "@ball":
                                    Game.World.Ball = true;
                                    break;
                                #endregion
                                #region /dragon
                                case "@dragon":
                                    Game.World.Dragon = true;
                                    break;
                                #endregion
                                #region /spook
                                case "@spook":
                                    if (!World.BossByPM && !World.ThrillingSpook)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "ThrillingSpook";
                                        foreach (Character C3 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C3.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.ThrillingSpook)
                                    {
                                        Game.World.ThrillingSpook = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /tash
                                case "@tash":
                                    if (!World.BossByPM && !World.Tash)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "Tash";
                                        foreach (Character C4 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C4.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.Tash)
                                    {
                                        Game.World.Tash = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /raikou
                                case "@raikou":
                                    if (!World.BossByPM && !World.Raikou)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "Raikou";
                                        foreach (Character C5 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C5.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.Raikou)
                                    {
                                        Game.World.Raikou = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /capricorn
                                case "@capricorn":
                                    if (!World.BossByPM && !World.Capricorn)
                                    {
                                        World.BossByPM = true;
                                        World.CurrentBoss = "Capricorn";
                                        foreach (Character C6 in World.H_Chars.Values)
                                            Ultimate.NPCs.NPCHandler.Handle(C6.MyClient, null, 2094, 0);
                                    }
                                    else if (World.BossByPM && !World.Capricorn)
                                    {
                                        Game.World.Capricorn = true;
                                        World.BossByPM = false;
                                    }
                                    break;
                                #endregion
                                #region /playersm
                                case "@playersm":
                                    string eMsg = "";
                                    foreach (Game.Character C6 in Game.World.H_Chars.Values)
                                        if (C6.Loc.Map == GC.MyChar.Loc.Map)
                                            eMsg += C6.Name + C6.MyClient.AuthInfo.Status + ", ";
                                    if (eMsg.Length > 1)
                                        eMsg = eMsg.Remove(eMsg.Length - 2, 2);
                                    GC.LocalMessage(2000, eMsg);
                                    break;
                                #endregion
                                #region /day
                                case "@day":
                                    Game.World.ScreenColor = 0;
                                    foreach (Game.Character C23 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C23.MyClient.AddSend(Packets.GeneralData(C23.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /night
                                case "@night":
                                    Game.World.ScreenColor = 5855577;
                                    foreach (Game.Character C24 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C24.MyClient.AddSend(Packets.GeneralData(C24.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /night1
                                case "@night1":
                                    Game.World.ScreenColor = 5355577;
                                    foreach (Game.Character C25 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C25.MyClient.AddSend(Packets.GeneralData(C25.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /night2
                                case "@night2":
                                    Game.World.ScreenColor = 6908265;
                                    foreach (Game.Character C26 in Game.World.H_Chars.Values)
                                        try
                                        {
                                            C26.MyClient.AddSend(Packets.GeneralData(C26.EntityID, Game.World.ScreenColor, 0, 0, 104));
                                        }
                                        catch { }
                                    break;
                                #endregion
                                #region /ratt
                                case "@ratt":
                                    System.Threading.Thread ResetAttributes = new System.Threading.Thread(Database.AttributesReset);
                                    ResetAttributes.Start();
                                    break;
                                #endregion
                                #region /whpw
                                case "@whpw":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        GC.LocalMessage(2000, "Password: " + '"' + C.WHPassword + '"');
                                    break;
                                #endregion
                                #region /rwhpw
                                case "@rwhpw":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.WHPassword = "0";
                                        GC.LocalMessage(2000, "WH Password removed from " + C.Name);
                                    }
                                    break;
                                #endregion
                                #region /revive
                                case "@revive":
                                    PacketHandling.Revive.Handle(GC);
                                    break;
                                #endregion
                                #region /invisible
                                case "@invisible":
                                    Game.World.Action(GC.MyChar, Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                    GC.MyChar.Invisible = !GC.MyChar.Invisible;
                                    GC.LocalMessage(2000, "Invisible: " + GC.MyChar.Invisible);
                                    break;
                                #endregion
                                #region /body
                                case "@body":
                                    if (Cmd[1] == "smale")
                                        GC.MyChar.Body = 1003;
                                    else if (Cmd[1] == "lmale")
                                        GC.MyChar.Body = 1004;
                                    else if (Cmd[1] == "sfemale")
                                        GC.MyChar.Body = 2001;
                                    else if (Cmd[1] == "lfemale")
                                        GC.MyChar.Body = 2002;
                                    break;
                                #endregion
                                #region /bodyp
                                case "@bodyp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        if (Cmd[2] == "smale")
                                            C.Body = 1003;
                                        else if (Cmd[2] == "lmale")
                                            C.Body = 1004;
                                        else if (Cmd[2] == "sfemale")
                                            C.Body = 2001;
                                        else if (Cmd[2] == "lfemale")
                                            C.Body = 2002;
                                    }
                                    break;
                                #endregion
                                #region /kill
                                case "@kill":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.AtkMem.Attacking = false;
                                        C.AtkMem.Target = 0;
                                        C.DeathHit = DateTime.Now;
                                        C.Alive = false;
                                        C.CurHP = 0;

                                        // World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                                        Game.World.Action(C, Packets.AttackPacket(0, C.EntityID, C.Loc.X, C.Loc.Y, 0, (byte)Game.AttackType.Kill).Get);
                                        // C.Equips.Send(C.MyClient, false);

                                        foreach (Buff B3 in C.Buffs.Keys)
                                            C.BDelete.TryAdd(B3, B3.Lasts);
                                        C.BlueName = false;
                                        C.StatEff.Add(Ultimate.Game.StatusEffectEn.Dead);
                                        C.PoisonedInfo.Times = 0;

                                    }
                                    break;
                                #endregion
                                #region save Tops
                                case "@topcharacters":

                                    TopRankings.LoadTops();

                                    break;
                                #endregion

                                #region save Tops
                                case "@tops1":

                                    string Nobility;
                                    if (GC.MyChar.Nobility.Rank == Ranks.Duke)
                                        if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                            Nobility = "Duke";
                                        else
                                            Nobility = "Duchess";
                                    else if (GC.MyChar.Nobility.Rank == Ranks.Prince)
                                        if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                            Nobility = "Prince";
                                        else
                                            Nobility = "Princess";
                                    else if (GC.MyChar.Nobility.Rank == Ranks.King)
                                        if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                            Nobility = "King";
                                        else
                                            Nobility = "Queen";
                                    else if (GC.MyChar.Nobility.Rank == Ranks.Knight)
                                        Nobility = "Knight";
                                    else if (GC.MyChar.Nobility.Rank == Ranks.Baron)
                                        if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                            Nobility = "Baron";
                                        else
                                            Nobility = "Baroness";
                                    else if (GC.MyChar.Nobility.Rank == Ranks.Earl)
                                        if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                            Nobility = "Earl";
                                        else
                                            Nobility = "Countess";
                                    else
                                        Nobility = "Serf";
                                    try
                                    {
                                        MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Top.Update("CharStats").Set("Level", GC.MyChar.Level).Set("Potency", GC.MyChar.Potency).Set("Nobility", Nobility).Set("PKPoints", GC.MyChar.PKPoints).Set("VirtuePoints", GC.MyChar.VP).Set("Gold", ((GC.MyChar.Silvers + GC.MyChar.WHSilvers))).Set("OnlineTime", GC.MyChar.OnlineTime).Set("Job", GC.MyChar.Job).Set("VipDays", GC.MyChar.VIPDays).Set("VipLevel", GC.MyChar.VipLevel).Set("Spouse", GC.MyChar.Spouse).Set("Face", GC.MyChar.Avatar).Set("GuildName", GC.MyChar.MyGuild.GuildName).Where("Name", GC.MyChar.Name).Execute();
                                        using (var session = NHibernateHelper.OpenSession())
                                        {
                                            var t = session.CreateSQLQuery("UPDATE CharStats SET Level= " + GC.MyChar.Level + ", Potency=" + GC.MyChar.Potency + ", Nobility=" + Nobility + ", PKPoints=" + GC.MyChar.PKPoints + ", VirtuePoints=" + GC.MyChar.VP + ", Gold=" + ((GC.MyChar.Silvers + GC.MyChar.WHSilvers)) + ", OnlineTime=" + GC.MyChar.OnlineTime + ", Job=" + GC.MyChar.Job + ", VipDays= " + GC.MyChar.VIPDays + ", VipLevel=" + GC.MyChar.VipLevel + ", Spouse=" + GC.MyChar.Spouse + ", Face=" + GC.MyChar.Avatar + ", GuildName=" + GC.MyChar.MyGuild.GuildName);
                                            t.ExecuteUpdate();
                                        }
                                    }
                                    catch { Console.WriteLine("Tops has been un saved!"); }

                                    break;
                                #endregion

                                #region /drawing
                                case "@drawing":
                                    if (!Game.World.Drawing)
                                    {
                                        Game.World.Drawing = true;
                                        Game.World.SendMsgToAll("SYSTEM", "The DB draw has begun! Type /roll to pick a random number 1 to 100. The biggest wins a DB!", 2011, 0);
                                        Game.World.SendMsgToAll("SYSTEM", "The DB draw has begun! Type /roll to pick a random number 1 to 100. The biggest wins a DB!", 2005, 0);
                                        Game.World.SendMsgToAll("SYSTEM", "The DB draw has begun! Type /roll to pick a random number 1 to 100. The biggest wins a DB!", 2000, 0);
                                    }
                                    break;
                                #endregion
                                #region /stopdrawing
                                case "@stopdrawing":
                                    if (Game.World.Drawing)
                                    {
                                        byte BiggestRoll = 0;
                                        eMsg = "";
                                        Game.World.Drawing = false;
                                        Dictionary<uint, Character> Winners = new Dictionary<uint, Character>();
                                        foreach (Game.Character C27 in Game.World.H_CharsDrawing.Values)
                                        {
                                            if (C27.MyClient.Soc.Connected)
                                            {
                                                if (C27.Roll > BiggestRoll)
                                                {
                                                    BiggestRoll = C27.Roll;
                                                    if (Winners != null)
                                                        Winners.Clear();
                                                    Winners.Add(C27.EntityID, C27);
                                                }
                                                else if (C27.Roll == BiggestRoll)
                                                {
                                                    Winners.Add(C27.EntityID, C27);
                                                }
                                            }
                                        }
                                        foreach (Game.Character C7 in Winners.Values)
                                        {
                                            if (C7.MyClient.Soc.Connected)
                                            {
                                                if (C7.Roll == BiggestRoll)
                                                {
                                                    if (C7.Inventory.Count < 40)
                                                        C7.AddItem(1088000);
                                                    else
                                                        Game.World.DebugAdd += C7.Name + " didn't have enough space in inventory and didn't get the DBS. \r\n";

                                                    eMsg += C7.Name + ", ";
                                                }
                                            }
                                        }
                                        if (eMsg.Length > 1)
                                            eMsg = eMsg.Remove(eMsg.Length - 2, 2);
                                        if (Winners != null)
                                        {
                                            Game.World.SendMsgToAll("[GM]", eMsg + " won a DB at the DB draws with the score: " + BiggestRoll + "! Congratulations!", 2011, 0);
                                            Game.World.SendMsgToAll("[GM]", eMsg + " won a DB at the DB draws with the score: " + BiggestRoll + "! Congratulations!", 2005, 0);
                                            Game.World.SendMsgToAll("[GM]", eMsg + " won a DB at the DB draws with the score: " + BiggestRoll + "! Congratulations!", 2000, 0);
                                        }
                                        Game.World.H_CharsDrawing.Clear();
                                    }
                                    break;
                                #endregion
                                #region /mapeffects
                                case "@mapeffects":
                                    uint DropID = Convert.ToUInt16(Cmd[1]);

                                    Random Rnd = new Random();

                                    Game.MapEffect DI;
                                    DI = new Game.MapEffect();
                                    DI.DropTime = DateTime.Now;
                                    DI.Loc = new Game.Location();
                                    DI.Loc.Map = GC.MyChar.Loc.Map;
                                    DI.Info = new Game.MEffect();
                                    DI.Info.ID = DropID;


                                    DI.UID = (uint)Rnd.Next(900000, 999999);
                                    DI.Info.UID = DI.UID;
                                    DI.Loc.X = (ushort)(GC.MyChar.Loc.X + Rnd.Next(15) - Rnd.Next(15));
                                    DI.Loc.Y = (ushort)(GC.MyChar.Loc.Y + Rnd.Next(15) - Rnd.Next(15));
                                    if (!DI.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.MapEffect>)Game.World.H_Effects[GC.MyChar.Loc.Map])) return;
                                    DI.Drop();

                                    break;
                                #endregion
                                #region /ips
                                case "@ips":
                                    try
                                    {
                                        Dictionary<uint, Main.GameClient> Clients = Game.World.H_Clients;
                                        List<string> IPs = new List<string>();
                                        foreach (Main.GameClient C8 in Clients.Values)
                                        {
                                            if (C8.Soc.Connected)
                                            {
                                                string IP = C8.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                                if (!IPs.Contains(IP))
                                                    IPs.Add(IP);
                                            }
                                        }
                                        GC.LocalMessage(2000, "IPs online: " + IPs.Count + ". Chars per IP: " + ((float)Clients.Count / IPs.Count));
                                    }
                                    catch (Exception E) { Console.WriteLine(E.ToString()); }
                                    break;
                                #endregion



                                #region /agip
                                case "@agip":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.Agi = ushort.Parse(Cmd[2]);
                                    break;
                                #endregion
                                #region /strp
                                case "@strp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.Str = ushort.Parse(Cmd[2]);
                                    break;
                                #endregion
                                #region /vitp
                                case "@vitp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.Vit = ushort.Parse(Cmd[2]);
                                    break;
                                #endregion
                                #region /spip
                                case "@spip":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.Spi = ushort.Parse(Cmd[2]);
                                    break;
                                #endregion
                                #region /reborn
                                case "@reborn":
                                    GC.MyChar.Reborns = byte.Parse(Cmd[1]);
                                    break;
                                #endregion
                                #region /gw
                                case "@gw":
                                    if (!Features.GuildWars.War)
                                        Features.GuildWars.StartWar();
                                    break;
                                #endregion
                                #region /ccgw
                                case "@ccgw":
                                    if (!Features.CounterClock.War)
                                        Features.CounterClock.StartWar();
                                    else
                                        Features.CounterClock.EndWarForGood();
                                    break;
                                #endregion
                                #region /citytc
                                case "@citytc":
                                    if (!Features.CityWarTc.War)
                                        Features.CityWarTc.StartWar();
                                    else
                                        Features.CityWarTc.EndWarForGood();
                                    break;
                                #endregion
                                #region /citypc
                                case "@citypc":
                                    if (!Features.CityWarPc.War)
                                        Features.CityWarPc.StartWar();
                                    else
                                        Features.CityWarPc.EndWarForGood();
                                    break;
                                #endregion
                                #region /cityac
                                case "@cityac":
                                    if (!Features.CityWarAc.War)
                                        Features.CityWarAc.StartWar();
                                    else
                                        Features.CityWarAc.EndWarForGood();
                                    break;
                                #endregion
                                #region /citydc
                                case "@citydc":
                                    if (!Features.CityWarDc.War)
                                        Features.CityWarDc.StartWar();
                                    else
                                        Features.CityWarDc.EndWarForGood();
                                    break;
                                #endregion
                                #region /citybi
                                case "@citybi":
                                    if (!Features.CityWarBi.War)
                                        Features.CityWarBi.StartWar();
                                    else
                                        Features.CityWarBi.EndWarForGood();
                                    break;
                                #endregion
                                #region /tcgw
                                case "@tcgw":
                                    if (!Features.TCGuildWars.War)
                                        Features.TCGuildWars.StartWar();
                                    else
                                        Features.TCGuildWars.EndWarForGood();
                                    break;
                                #endregion
                                #region /stopgw
                                case "@stopgw":
                                    if (Features.GuildWars.War)
                                        Features.GuildWars.EndWarForGood();
                                    break;
                                #endregion

                                #region /socket1
                                case "@socket1":
                                    string Char1 = Cmd[1];
                                    string Char2 = Cmd[2];
                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.SendMsgToAll("SYSTEM", Char1 + " has got first socket into his/her " + Char2, 2011, 0);
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + Char1 + "**__ has got first socket into his/her __**" + Char2 + "**__ with meteor  " + DateTime.Now;

                                    break;
                                #endregion

                                #region /socket2
                                case "@socket2":
                                    string Char3 = Cmd[1];
                                    string Char4 = Cmd[2];
                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.SendMsgToAll("SYSTEM", Char3 + " has got second socket into his/her " + Char4, 2011, 0);
                                    //Discord DCord1 = new Discord();
                                    //DCord1.MesajVer3 = "  __**" + Char3 + "**__ has got second socket into his/her __**" + Char4 + "**__ with meteor " + DateTime.Now;

                                    break;
                                #endregion

                                #region /eqlow
                                case "@eqlow":
                                    for (byte i3 = 1; i3 < 9; i3++)
                                        if (i3 != 7)
                                        {
                                            Game.Item I = GC.MyChar.Equips.Get(i3);
                                            if (I.ID != 0)
                                            {
                                                Game.ItemIDManipulation IDM = new Ultimate.Game.ItemIDManipulation(I.ID);
                                                IDM.LowestLevel(i3);
                                                I.ID = IDM.ToID();
                                                GC.AddSend(Packets.AddItem(I, i3));
                                            }
                                        }
                                    break;
                                #endregion
                                #region /protect
                                case "@protect":
                                    GC.MyChar.Protection = !GC.MyChar.Protection;
                                    GC.LocalMessage(2000, "Protect mode: " + GC.MyChar.Protection);
                                    break;
                                #endregion
                                #region /effect
                                case "@effect":
                                    World.Action(Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, Cmd[1]).Get);
                                    //GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, Cmd[1]));
                                    break;
                                #endregion
                                #region /goto player
                                case "@goto":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null && C != GC.MyChar)
                                        GC.MyChar.Teleport(C.Loc.Map, C.Loc.X, C.Loc.Y);
                                    break;
                                #endregion
                                #region /bring player
                                case "@bring":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null && C != GC.MyChar)
                                    {
                                        if (C.BOTJailed)
                                            C.BOTJailedDays = 0;
                                        C.Teleport(GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y);
                                    }
                                    break;
                                #endregion
                                #region /classicpts
                                case "@giveops":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.ClassicPoints = ushort.Parse(Cmd[2]);
                                    }
                                    break;
                                #endregion
                                #region /givegps
                                case "@givegps":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.GarmentToken = ushort.Parse(Cmd[2]);
                                    }
                                    break;
                                #endregion
                                #region /givectb
                                case "@givectb":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.CTBPoints = ushort.Parse(Cmd[2]);
                                    }
                                    break;
                                #endregion
                                #region /resetctb
                                case "@resetctb":
                                    foreach (Character C6 in World.H_Chars.Values)
                                        C6.CTBPoints = 0;
                                    break;
                                #endregion


                                #region /givevps
                                case "@givevps":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.VotePoints = byte.Parse(Cmd[2]);
                                    }
                                    break;
                                #endregion
                                #region /c and /bc
                                case "@c":
                                    Game.World.SendMsgToAll(GC.MyChar.Name, GC.MyChar.Name + ": " + Message.Remove(0, 3), 2011, 0);
                                    break;
                                case "@bc":
                                    Game.World.SendMsgToAll("GM", ": " + Message.Remove(0, 3), 2011, 0);
                                    break;
                                #endregion
                                #region /treasurehunt
                                case "@treasurehunt":
                                    if (!Game.World.TreasureHunt)
                                    {
                                        Game.World.TreasureHunt = true;
                                        Game.World.TreasureMap = (ushort)Program.Rnd.Next(8004, 8007);
                                        Game.World.SendMsgToAll("SYSTEM", "Treasure Hunt Event Started!", 2005, 0);
                                    }
                                    else
                                    {
                                        Game.World.TreasureHunt = false;
                                        Game.World.SendMsgToAll("SYSTEM", "Treasure Hunt Event Ended!", 2005, 0);
                                    }
                                    break;
                                #endregion
                                #region /seff
                                case "@seff":
                                    GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Effect, ulong.Parse(Cmd[1])));
                                    break;
                                #endregion
                                #region /vipp
                                case "@vipp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C == null)
                                    {
                                        try
                                        {
                                            string Account = "";
                                            C = Database.LoadCharacter(Cmd[1], ref Account);
                                            if (C != null)
                                            {
                                                if (Cmd.Length >= 4)
                                                {
                                                    if (Cmd[4].Contains("@"))
                                                    {
                                                        if (byte.Parse(Cmd[3]) > 0)
                                                        {
                                                            C.VIPLevelToReceive = byte.Parse(Cmd[2]);
                                                            C.VIPDaysToReceive += byte.Parse(Cmd[3]);
                                                            /*if (DateTime.Now > C.VIPStarted.AddHours(24))
                                                                C.VIPStarted = DateTime.Now;*/
                                                            Game.World.DonationAdd += Cmd[1] + " has been added to PRIZE NPC, VIP " + C.VIPLevelToReceive + " at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[4] + "\r\n";
                                                            GC.LocalMessage(2000, "VIP added succesfully on: " + Cmd[1]);
                                                            Database.SaveCharacter(C, Account);
                                                        }
                                                        else
                                                        {
                                                            C.VipLevel = 0;
                                                            C.VIPDays = 0;
                                                            C.VIPLevelToReceive = 0;
                                                            C.VIPDaysToReceive = 0;
                                                            Game.World.DonationAdd += Cmd[1] + " has lost VIP at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[4] + "\r\n";
                                                            GC.LocalMessage(2000, "VIP removed succesfully on: " + Cmd[1]);
                                                            Database.SaveCharacter(C, Account);
                                                        }
                                                    }
                                                    else GC.LocalMessage(2000, "VIP add/removal failed. Use /vipp name level days email. Email must be of type name@domain.com");
                                                }
                                                else GC.LocalMessage(2000, "VIP add/removal failed. Use /vipp name level days email. Email must be of type name@domain.com");
                                            }
                                            else GC.LocalMessage(2000, "VIP add/removal failed because the character: " + Cmd[1] + " doesn't exist.");
                                        }
                                        catch (Exception E) { Console.WriteLine(E.ToString()); }
                                    }
                                    else
                                    {
                                        if (Cmd.Length >= 4)
                                        {
                                            if (Cmd[4].Contains("@"))
                                            {
                                                if (byte.Parse(Cmd[3]) > 0)
                                                {
                                                    C.VIPLevelToReceive = byte.Parse(Cmd[2]);
                                                    C.VIPDaysToReceive += byte.Parse(Cmd[3]);
                                                    /* if (DateTime.Now > C.VIPStarted.AddHours(24))
                                                         C.VIPStarted = DateTime.Now;*/
                                                    Game.World.DonationAdd += C.Name + " has been added to PRIZE NPC, VIP " + C.VIPLevelToReceive + " at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[4] + "\r\n";
                                                    GC.LocalMessage(2000, "VIP added succesfully on: " + C.Name);
                                                    C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", C.Name, "Congratulations! Check PRIZE NPC in market to receive your VIP " + C.VIPLevelToReceive + " . Thank you for donating.", 2001, 0));
                                                }
                                                else
                                                {
                                                    C.VipLevel = 0;
                                                    C.VIPDays = 0;
                                                    C.VIPLevelToReceive = 0;
                                                    C.VIPDaysToReceive = 0;
                                                    Game.World.DonationAdd += C.Name + " has lost VIP at the time: " + DateTime.Now + " GMT -7 due to a donation made on the e-mail: " + Cmd[4] + "\r\n";
                                                    GC.LocalMessage(2000, "VIP removed succesfully on: " + C.Name);
                                                    C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", C.Name, "Your VIP was removed by a PM! (Contact UltimateConquerGM if you didn't request this)", 2001, 0));
                                                }
                                            }
                                            else GC.LocalMessage(2000, "VIP add/removal failed. Use /vipp name level days email. Email must be of type name@domain.com");
                                        }
                                        else GC.LocalMessage(2000, "VIP add/removal failed. Use /vipp name level days email. Email must be of type name@domain.com");
                                    }
                                    break;
                                #endregion
                                #region /spawn
                                case "@spawn":
                                    if (Cmd.Length == 3)
                                    {
                                        GC.SpawnOnHold = !GC.SpawnOnHold;
                                        if (GC.SpawnOnHold)
                                        {
                                            GC.SpawnXStart = GC.MyChar.Loc.X;
                                            GC.SpawnYStart = GC.MyChar.Loc.Y;
                                            GC.LocalMessage(2000, "Starting to create spawn; Spawn starts: " + GC.SpawnXStart + ", " + GC.SpawnYStart);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2000, "Ending the spawn.");
                                            StreamWriter SW = new StreamWriter(@"C:\OldCODB\MobSpawns.txt", true);
                                            SW.WriteLine(Cmd[1] + " " + Cmd[2] + " " + GC.MyChar.Loc.Map + " " + GC.SpawnXStart + " " + GC.SpawnYStart + " " + GC.MyChar.Loc.X + " " + GC.MyChar.Loc.Y);
                                            SW.Flush();
                                            SW.Close();
                                        }
                                    }
                                    break;
                                #endregion
                                #region /addnpc
                                case "@addnpc":
                                    Game.NPC N2 = new Ultimate.Game.NPC(Cmd[1] + ' ' + Cmd[2] + ' ' + Cmd[3] + ' ' + Cmd[4] + ' ' + GC.MyChar.Loc.Map + ' ' + ((ushort)(GC.MyChar.Loc.X + 2)) + ' ' + GC.MyChar.Loc.Y);
                                    Game.World.Spawn(N2);
                                    break;
                                #endregion
                                #region /xp
                                case "@xp":
                                    GC.MyChar.StatEff.Add(Ultimate.Game.StatusEffectEn.XPStart);
                                    Buff B = new Buff();
                                    B.StEff = StatusEffectEn.XPStart;
                                    B.Lasts = 20;
                                    B.Started = DateTime.Now;
                                    B.Eff = Features.SkillsClass.ExtraEffect.None;

                                    GC.MyChar.Buffs.TryAdd(B, B.Lasts);
                                    break;
                                #endregion
                                #region /mana
                                case "@mana":
                                    GC.MyChar.CurMP = (ushort)GC.MyChar.MaxMP;
                                    break;
                                #endregion
                                #region /life
                                case "@life":
                                    GC.MyChar.CurHP = (ushort)GC.MyChar.MaxHP;
                                    break;
                                #endregion
                                #region /skill
                                case "@skill":
                                    GC.MyChar.RWSkill(new Game.Skill() { ID = ushort.Parse(Cmd[1]), Lvl = byte.Parse(Cmd[2]), Exp = 0 });
                                    break;
                                #endregion
                                #region /rskill
                                case "@rskill":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        if (C.Skills.ContainsKey(ushort.Parse(Cmd[2])))
                                        {
                                            C.Skills.Remove(ushort.Parse(Cmd[2]));
                                            C.MyClient.AddSend(Packets.GeneralData(C.EntityID, ushort.Parse(Cmd[2]), 0, 0, 109));
                                        }
                                    break;
                                #endregion
                                #region /skillp
                                case "@skillp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.RWSkill(new Game.Skill() { ID = ushort.Parse(Cmd[2]), Lvl = byte.Parse(Cmd[3]), Exp = uint.Parse(Cmd[4]) });
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            C.RWSkill(new Game.Skill() { ID = ushort.Parse(Cmd[2]), Lvl = byte.Parse(Cmd[3]), Exp = uint.Parse(Cmd[4]) });
                                            Database.SaveCharacter(C, Account);
                                        }
                                    }
                                    break;
                                #endregion
                                #region /prof
                                case "@prof":
                                    GC.MyChar.RWProf(new Game.Prof() { ID = ushort.Parse(Cmd[1]), Lvl = byte.Parse(Cmd[2]), Exp = 0 });
                                    break;
                                #endregion
                                #region /profp
                                case "@profp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.RWProf(new Game.Prof() { ID = ushort.Parse(Cmd[2]), Lvl = byte.Parse(Cmd[3]), Exp = uint.Parse(Cmd[4]) });
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            C.RWProf(new Game.Prof() { ID = ushort.Parse(Cmd[2]), Lvl = byte.Parse(Cmd[3]), Exp = uint.Parse(Cmd[4]) });
                                            Database.SaveCharacter(C, Account);
                                        }
                                    }
                                    break;
                                #endregion
                                #region /garm
                                case "@garm":
                                    int i = 0;
                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (Game.ItemIDManipulation.Part(D.ID, 0, 3) >= 181 && Game.ItemIDManipulation.Part(D.ID, 0, 3) <= 199)
                                        {
                                            if (i >= GC.MyChar.garment)
                                                if (GC.MyChar.Inventory.Count < 40)
                                                {
                                                    GC.MyChar.AddItem(D.ID);
                                                    GC.MyChar.garment++;
                                                }
                                            i++;
                                        }
                                    }
                                    break;
                                #endregion
                                #region /level
                                case "@level":
                                    GC.MyChar.Experience = 0;
                                    GC.MyChar.Level = byte.Parse(Cmd[1]);
                                    break;
                                #endregion
                                #region /levelp
                                case "@levelp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.Experience = 0;
                                        C.Level = byte.Parse(Cmd[2]);
                                    }
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            C.Experience = 0;
                                            C.Level = byte.Parse(Cmd[2]);
                                            Database.SaveCharacter(C, Account);
                                        }
                                    }
                                    break;
                                #endregion



                                #region /kick
                                case "@kick":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.MyClient.Disconnect();
                                        C.MyClient.LogOff();
                                        if (C.MyClient.Soc.Connected)
                                            C.MyClient.Soc.Disconnect(false);

                                    }
                                    break;
                                #endregion
                                #region /vpp
                                case "@vpp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.VP = ulong.Parse(Cmd[2]);
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            C.VP = ulong.Parse(Cmd[2]);
                                            Database.SaveCharacter(C, Account);
                                        }
                                    }
                                    break;
                                #endregion
                                #region /map
                                case "@map":
                                    GC.LocalMessage(2000, "The ID of the map you are on is " + GC.MyChar.Loc.Map);
                                    break;
                                #endregion
                                #region /job
                                case "@job":
                                    GC.MyChar.Job = byte.Parse(Cmd[1]);
                                    break;
                                #endregion
                                #region /jobp
                                case "@jobp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.Job = byte.Parse(Cmd[2]);
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            C.Job = byte.Parse(Cmd[2]);
                                            Database.SaveCharacter(C, Account);
                                        }
                                    }
                                    break;
                                #endregion
                                #region /silvers
                                case "@silvers":
                                    GC.MyChar.Silvers = uint.Parse(Cmd[1]);
                                    break;
                                #endregion
                                #region /tele
                                case "@tele":
                                    GC.MyChar.Teleport(uint.Parse(Cmd[1]), ushort.Parse(Cmd[2]), ushort.Parse(Cmd[3]));
                                    break;
                                #endregion
                                #region /statp
                                case "@statp":
                                    GC.MyChar.StatPoints = ushort.Parse(Cmd[1]);
                                    break;
                                #endregion
                                #region /statpp
                                case "@statpp":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                        C.StatPoints = ushort.Parse(Cmd[2]);
                                    break;
                                #endregion
                                #region /dison
                                case "@dison":
                                    if (!Game.World.DisCityON)
                                    {
                                        Game.World.DisCityON = true;
                                        World.SendMsgToAll("SYSTEM", "The Dis City quest has been started! Run to ApeMountain and find SolarSaint(530,482)!", 2011, 0);
                                        Game.World.LeftKills = 0;
                                        Game.World.RightKills = 0;
                                        Game.World.PlutoKilled = false;
                                        Game.World.Syrens = 8;
                                        Game.World.Dis2 = 0;
                                        Game.World.Dis3 = 0;
                                        Game.World.LeftFlank = 0;
                                        Game.World.RightFlank = 0;
                                        Game.World.Pluto = false;
                                    }
                                    break;
                                #endregion
                                #region /disoff
                                case "@disoff":
                                    if (Game.World.DisCityON)
                                    {
                                        Game.World.DisCityON = false;
                                        Game.World.SendMsgToAll("SYSTEM", "The entrance of Dis City quest now is blocked! Who don't get in try your luck next time!", 2011, 0);
                                    }
                                    break;
                                #endregion
                                #region /trade
                                case "@trade":
                                    GC.MyChar.Trading = false;
                                    GC.AddSend(Packets.TradePacket(GC.MyChar.TradingWith, 5));
                                    break;
                                #endregion
                                #region /itemid
                                case "@itemid":
                                    uint ID = 0;
                                    foreach (DatabaseItem DI2 in Database.DatabaseItems.Values)
                                        if (DI2.ID == uint.Parse(Cmd[1]))
                                        {
                                            ID = DI2.ID;
                                        }
                                    if (ID != 0 && GC.MyChar.Inventory.Count < 40)
                                    {
                                        Game.ItemIDManipulation e = new Game.ItemIDManipulation(ID);
                                        if (!Database.DatabaseItems.ContainsKey(ID))
                                        {
                                            return;
                                        }
                                        Game.Item I = new Ultimate.Game.Item();
                                        if (e.Part(0, 2) == 11 || e.Part(0, 2) == 13 || e.Part(0, 3) == 123 || e.Part(0, 3) == 141 || e.Part(0, 3) == 142)
                                            I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.ID = ID;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        try
                                        {
                                            I.MaxDur = I.DBInfo.Durability;
                                            I.CurDur = I.MaxDur;
                                        }
                                        catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                                        try
                                        {
                                            I.Plus = byte.Parse(Cmd[2]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Bless = byte.Parse(Cmd[3]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Enchant = byte.Parse(Cmd[4]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Soc1 = (Game.Item.Gem)byte.Parse(Cmd[5]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Soc2 = (Game.Item.Gem)byte.Parse(Cmd[6]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Progress = ushort.Parse(Cmd[7]);
                                        }
                                        catch { }
                                        I.Effect = Ultimate.Game.Item.RebornEffect.None;

                                        GC.MyChar.AddItem(I);
                                    }
                                    break;
                                #endregion
                                #region /rnobility
                                case "@rnobility":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        for (int i2 = 0; i2 <= 49; i2++)
                                        {
                                            if (Game.World.EmpireBoard[i2].ID == C.EntityID)
                                                for (int j = i2; j <= 48; j++)
                                                {
                                                    Game.World.EmpireBoard[j] = Game.World.EmpireBoard[j + 1];
                                                }
                                        }
                                        C.Nobility.Donation = 0;
                                        C.Nobility.ListPlace = -1;
                                    }
                                    else
                                    {
                                        for (int i2 = 0; i2 <= 49; i2++)
                                        {
                                            if (Game.World.EmpireBoard[i2].Name.ToLower() == Cmd[1].ToLower())
                                                for (int j = i2; j <= 48; j++)
                                                {
                                                    Game.World.EmpireBoard[j] = Game.World.EmpireBoard[j + 1];
                                                }
                                        }
                                    }
                                    break;
                                #endregion
                                #region /mute
                                case "@mute":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        if (!C.Muted)
                                        {
                                            if (C.Warning)
                                            {
                                                C.Warning = false;
                                                if (C.MutedRecord < 255)
                                                    C.MutedRecord++;
                                                C.MutedDays = (byte)C.MutedRecord;
                                                C.MyClient.LocalMessage(2011, "You are now muted for " + C.MutedDays + " Days for speaking other languages besides English in World Chat!");
                                                GC.LocalMessage(2000, C.Name + " was muted for " + C.MutedDays + " Days!");
                                                Game.World.SendMsgToAll("MUTE", C.Name + " have been muted for " + C.MutedDays + " Days for speaking other languages besides English in World Chat!", 2000, 0);
                                                Program.WriteCmds(GC.MyChar.Name + " has muted  " + C.Name + "  for: " + C.MutedDays + " days at: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                                            }
                                            else
                                            {
                                                C.Warning = true;
                                                GC.LocalMessage(2000, "You have warned " + C.Name + " he will be muted for " + (C.MutedRecord + 1).ToString() + " Days next time!");
                                                C.MyClient.LocalMessage(2000, "You have been warned to stop using other languages besides English in World Chat! Next time you'll be muted!");
                                            }
                                        }
                                        else if (Cmd.Length >= 3)
                                        {
                                            if (Cmd[2] == "0")
                                            {
                                                C.MutedDays = 0;
                                                C.MyClient.LocalMessage(2011, "You are now unmuted! Don't ever break the rules on World Chat!");
                                                GC.LocalMessage(2000, C.Name + " was unmuted!");
                                                Program.WriteCmds(GC.MyChar.Name + " has unmuted  " + C.Name + " at: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                                                if (C.MutedRecord > 0)
                                                    C.MutedRecord--;
                                            }
                                            else
                                                GC.LocalMessage(2000, "To unmute a player please type /mute Name 0");
                                        }
                                        else
                                            GC.LocalMessage(2000, C.Name + " is already muted for " + C.MutedDays + " Days!");
                                    }
                                    else
                                        GC.LocalMessage(2000, Cmd[1] + " is either not online or doesn't exist!");
                                    break;
                                #endregion
                                #region /cycloneall
                                case "@cycloneall":
                                    DateTime TimeNow = DateTime.Now;
                                    foreach (Game.Character C22 in Game.World.H_Chars.Values)
                                    {
                                        Buff S = C22.BuffOf(Features.SkillsClass.ExtraEffect.Superman);
                                        Buff CC = C22.BuffOf(Features.SkillsClass.ExtraEffect.Cyclone);

                                        if (S.Eff != Features.SkillsClass.ExtraEffect.Superman && CC.Eff != Features.SkillsClass.ExtraEffect.Cyclone)
                                        {
                                            Buff B2 = new Buff();
                                            B2.Eff = Features.SkillsClass.ExtraEffect.Cyclone;
                                            B2.Lasts = 90;
                                            B2.Value = 90;
                                            B2.Started = TimeNow;
                                            B2.StEff = Game.StatusEffectEn.Cyclone;
                                            C22.TimeBuff = B2.Lasts;
                                            C22.AddBuff(B2);

                                            C22.MyClient.LocalMessage(2011, "Cyclone Event! You received cyclone for 90 seconds! Use it wisely!");
                                        }

                                    }
                                    Game.World.CycloneEvent = TimeNow;
                                    break;
                                #endregion
                                #region /botjail
                                case "@botjail":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        if (byte.Parse(Cmd[2]) > 0)
                                        {
                                            //C.BOTJailed = true;
                                            C.BOTJailedDays = byte.Parse(Cmd[2]);
                                            C.Teleport(6003, 30, 72);
                                            C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " days!");
                                            Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                        }
                                        else
                                        {
                                            //C.BOTJailed = false;
                                            C.BOTJailedDays = byte.Parse(Cmd[2]);
                                            C.Teleport(6003, 30, 72);
                                            C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " days!");
                                            Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                        }
                                    }
                                    else
                                    {
                                        string Account = "";
                                        C = Database.LoadCharacter(Cmd[1], ref Account);
                                        if (C != null)
                                        {
                                            if (byte.Parse(Cmd[2]) > 0)
                                            {
                                                C.Loc.PreviousMap = C.Loc.Map;
                                                C.Loc.X = 30;
                                                C.Loc.Y = 72;
                                                C.Loc.Map = 6003;
                                                //C.BOTJailed = true;
                                                C.BOTJailedDays = byte.Parse(Cmd[2]);
                                                Database.SaveCharacter(C, Account);
                                                Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                            }
                                            else
                                            {
                                                C.Loc.PreviousMap = C.Loc.Map;
                                                C.Loc.X = 30;
                                                C.Loc.Y = 72;
                                                C.Loc.Map = 6003;
                                                //C.BOTJailed = false;
                                                C.BOTJailedDays = byte.Parse(Cmd[2]);
                                                Database.SaveCharacter(C, Account);
                                                Program.WriteCmds(GC.MyChar.Name + " botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now);
                                            }
                                        }
                                    }
                                    break;
                                #endregion
                                #region /item
                                case "@item":
                                    ID = 0;
                                    foreach (DatabaseItem DI2 in Database.DatabaseItems.Values)
                                        if (DI2.Name == Cmd[1])
                                        {
                                            ID = DI2.ID;
                                            Game.ItemIDManipulation e = new Game.ItemIDManipulation(ID);
                                            Game.Item.ItemQuality Quality = e.Quality;

                                            if (Cmd[2].ToLower() == "refined") Quality = Game.Item.ItemQuality.Refined;
                                            else if (Cmd[2].ToLower() == "unique") Quality = Game.Item.ItemQuality.Unique;
                                            else if (Cmd[2].ToLower() == "elite") Quality = Game.Item.ItemQuality.Elite;
                                            else if (Cmd[2].ToLower() == "super") Quality = Game.Item.ItemQuality.Super;
                                            else if (Cmd[2].ToLower() == "simple") Quality = Game.Item.ItemQuality.Simple;
                                            else if (Cmd[2].ToLower() == "fixed") Quality = Game.Item.ItemQuality.Fixed;
                                            else if (Cmd[2].ToLower() == "poor") Quality = Game.Item.ItemQuality.Poor;
                                            else if (Cmd[2].ToLower() == "normal") Quality = Game.Item.ItemQuality.Normal;
                                            if (e.Quality == Quality)
                                                break;
                                        }
                                    if (ID != 0 && GC.MyChar.Inventory.Count < 40)
                                    {
                                        Game.ItemIDManipulation e = new Game.ItemIDManipulation(ID);
                                        Game.Item.ItemQuality Quality = e.Quality;
                                        bool change = true;
                                        if (Cmd[2].ToLower() == "refined") Quality = Game.Item.ItemQuality.Refined;
                                        else if (Cmd[2].ToLower() == "unique") Quality = Game.Item.ItemQuality.Unique;
                                        else if (Cmd[2].ToLower() == "elite") Quality = Game.Item.ItemQuality.Elite;
                                        else if (Cmd[2].ToLower() == "super") Quality = Game.Item.ItemQuality.Super;
                                        else if (Cmd[2].ToLower() == "simple") Quality = Game.Item.ItemQuality.Simple;
                                        else if (Cmd[2].ToLower() == "fixed") Quality = Game.Item.ItemQuality.Fixed;
                                        else if (Cmd[2].ToLower() == "poor") Quality = Game.Item.ItemQuality.Poor;
                                        else if (Cmd[2].ToLower() == "normal") Quality = Game.Item.ItemQuality.Normal;
                                        else change = false;
                                        if (change)
                                            e.QualityChange(Quality);
                                        ID = e.ToID();
                                        if (!Database.DatabaseItems.ContainsKey(ID))
                                        {
                                            return;
                                        }
                                        Game.Item I = new Ultimate.Game.Item();
                                        if (e.Part(0, 2) == 11 || e.Part(0, 2) == 13 || e.Part(0, 3) == 123 || e.Part(0, 3) == 141 || e.Part(0, 3) == 142)
                                            I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.FreeItem = false;
                                        I.ID = ID;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        try
                                        {
                                            I.MaxDur = I.DBInfo.Durability;
                                            I.CurDur = I.MaxDur;
                                        }
                                        catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                                        try
                                        {
                                            I.Plus = byte.Parse(Cmd[3]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Bless = byte.Parse(Cmd[4]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Enchant = byte.Parse(Cmd[5]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Soc1 = (Game.Item.Gem)byte.Parse(Cmd[6]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Soc2 = (Game.Item.Gem)byte.Parse(Cmd[7]);
                                        }
                                        catch { }
                                        try
                                        {
                                            if (Cmd[8] != null)
                                            {
                                                if (Cmd[8] == "none")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.None;
                                                else if (Cmd[8] == "poison")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.Poison;
                                                else if (Cmd[8] == "hp")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.HP;
                                                else if (Cmd[8] == "mp")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.MP;
                                                else if (Cmd[8] == "shield")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.Shield;
                                                else if (Cmd[8] == "horsie")
                                                    I.Effect = Item.RebornEffect.Horsie;
                                            }
                                            else
                                                I.Effect = Ultimate.Game.Item.RebornEffect.None;
                                        }
                                        catch { }
                                        try
                                        {
                                            if (Cmd[9] != null)
                                                I.RestrainType = ushort.Parse(Cmd[9]);
                                        }
                                        catch { }
                                        GC.MyChar.AddItem(I);
                                    }
                                    break;
                                #endregion

                                #region /freeitem
                                case "@freeitem":
                                    ID = 0;
                                    foreach (DatabaseItem DI2 in Database.DatabaseItems.Values)
                                        if (DI2.Name == Cmd[1])
                                        {
                                            ID = DI2.ID;
                                            Game.ItemIDManipulation e = new Game.ItemIDManipulation(ID);
                                            Game.Item.ItemQuality Quality = e.Quality;

                                            if (Cmd[2].ToLower() == "refined") Quality = Game.Item.ItemQuality.Refined;
                                            else if (Cmd[2].ToLower() == "unique") Quality = Game.Item.ItemQuality.Unique;
                                            else if (Cmd[2].ToLower() == "elite") Quality = Game.Item.ItemQuality.Elite;
                                            else if (Cmd[2].ToLower() == "super") Quality = Game.Item.ItemQuality.Super;
                                            else if (Cmd[2].ToLower() == "simple") Quality = Game.Item.ItemQuality.Simple;
                                            else if (Cmd[2].ToLower() == "fixed") Quality = Game.Item.ItemQuality.Fixed;
                                            else if (Cmd[2].ToLower() == "poor") Quality = Game.Item.ItemQuality.Poor;
                                            else if (Cmd[2].ToLower() == "normal") Quality = Game.Item.ItemQuality.Normal;
                                            if (e.Quality == Quality)
                                                break;
                                        }
                                    if (ID != 0 && GC.MyChar.Inventory.Count < 40)
                                    {
                                        Game.ItemIDManipulation e = new Game.ItemIDManipulation(ID);
                                        Game.Item.ItemQuality Quality = e.Quality;
                                        bool change = true;
                                        if (Cmd[2].ToLower() == "refined") Quality = Game.Item.ItemQuality.Refined;
                                        else if (Cmd[2].ToLower() == "unique") Quality = Game.Item.ItemQuality.Unique;
                                        else if (Cmd[2].ToLower() == "elite") Quality = Game.Item.ItemQuality.Elite;
                                        else if (Cmd[2].ToLower() == "super") Quality = Game.Item.ItemQuality.Super;
                                        else if (Cmd[2].ToLower() == "simple") Quality = Game.Item.ItemQuality.Simple;
                                        else if (Cmd[2].ToLower() == "fixed") Quality = Game.Item.ItemQuality.Fixed;
                                        else if (Cmd[2].ToLower() == "poor") Quality = Game.Item.ItemQuality.Poor;
                                        else if (Cmd[2].ToLower() == "normal") Quality = Game.Item.ItemQuality.Normal;
                                        else change = false;
                                        if (change)
                                            e.QualityChange(Quality);
                                        ID = e.ToID();
                                        if (!Database.DatabaseItems.ContainsKey(ID))
                                        {
                                            return;
                                        }
                                        Game.Item I = new Ultimate.Game.Item();
                                        if (e.Part(0, 2) == 11 || e.Part(0, 2) == 13 || e.Part(0, 3) == 123 || e.Part(0, 3) == 141 || e.Part(0, 3) == 142)
                                            I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.FreeItem = true;
                                        I.ID = ID;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        try
                                        {
                                            I.MaxDur = I.DBInfo.Durability;
                                            I.CurDur = I.MaxDur;
                                        }
                                        catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                                        try
                                        {
                                            I.Plus = byte.Parse(Cmd[3]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Bless = byte.Parse(Cmd[4]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Enchant = byte.Parse(Cmd[5]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Soc1 = (Game.Item.Gem)byte.Parse(Cmd[6]);
                                        }
                                        catch { }
                                        try
                                        {
                                            I.Soc2 = (Game.Item.Gem)byte.Parse(Cmd[7]);
                                        }
                                        catch { }
                                        try
                                        {
                                            if (Cmd[8] != null)
                                            {
                                                if (Cmd[8] == "none")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.None;
                                                else if (Cmd[8] == "poison")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.Poison;
                                                else if (Cmd[8] == "hp")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.HP;
                                                else if (Cmd[8] == "mp")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.MP;
                                                else if (Cmd[8] == "shield")
                                                    I.Effect = Ultimate.Game.Item.RebornEffect.Shield;
                                                else if (Cmd[8] == "horsie")
                                                    I.Effect = Item.RebornEffect.Horsie;
                                            }
                                            else
                                                I.Effect = Ultimate.Game.Item.RebornEffect.None;
                                        }
                                        catch { }
                                        try
                                        {
                                            if (Cmd[9] != null)
                                                I.RestrainType = ushort.Parse(Cmd[9]);
                                        }
                                        catch { }
                                        GC.MyChar.AddItem(I);
                                    }
                                    break;
                                #endregion


                                #region /votep
                                case "@votep":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.VotePoints = byte.Parse(Cmd[2]);
                                    }
                                    break;
                                #endregion
                                #region /resetwhpass
                                case "@resetwhpass":
                                    C = Game.World.CharacterFromName(Cmd[1]);
                                    if (C != null)
                                    {
                                        C.WHPassword = "0";
                                        GC.LocalMessage(2000, "Warehouse Password reset for Character: " + C.Name);
                                    }
                                    else
                                        GC.LocalMessage(2000, "Error: Character is null (offline?) Character: " + C.Name);
                                    break;
                                #endregion
                                #region /eff
                                case "@eff":
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, Cmd[1])).Get);
                                    GC.LocalMessage(2000, "Effect: " + Cmd[1]);
                                    break;
                                #endregion
                                #region /serverclose
                                case "@serverclose":
                                    Program.ServerClose();
                                    System.Threading.Thread.Sleep(5000);
                                    Program.ExitProgram();
                                    break;
                                case "@servercloserr":
                                    Program.ServerClose();
                                    System.Threading.Thread.Sleep(20000);
                                    Program.RestartPC();
                                    Program.ExitProgram();
                                    break;
                                #endregion
                                #region LoadSquamas
                                case "@Squamas":
                                    Database.LoadSquamas(new Game.MapEffect(), true);
                                    break;
                                #endregion
                                #region /spawnmob
                                case "@spawnmob":
                                    Mob Mob = new Mob();
                                    Mob.Loc = new Location();
                                    Mob.Loc.X = (ushort)(GC.MyChar.Loc.X + Program.Rnd.Next(3));
                                    Mob.Loc.Y = (ushort)(GC.MyChar.Loc.Y + Program.Rnd.Next(3));
                                    Mob.Loc.Map = GC.MyChar.Loc.Map;
                                    Mob.StartLoc.XFrom = (ushort)(GC.MyChar.Loc.X + Program.Rnd.Next(10));
                                    Mob.StartLoc.XTo = (ushort)(GC.MyChar.Loc.X + Program.Rnd.Next(10));
                                    Mob.StartLoc.YFrom = (ushort)(GC.MyChar.Loc.Y + Program.Rnd.Next(10));
                                    Mob.StartLoc.Yto = (ushort)(GC.MyChar.Loc.Y + Program.Rnd.Next(10));
                                    Mob.StartLoc.Map = GC.MyChar.Loc.Map;
                                    Mob.MobID = (int)Program.Rnd.Next(1000, 3000);
                                    Mob.Name = "Test";
                                    Mob.Type = MobBehaveour.HuntPlayers;
                                    Mob.Mesh = ushort.Parse(Cmd[1]);
                                    Mob.Level = 1;
                                    Mob.MaxHP = (ushort)(Mob.Level * 10000);
                                    Mob.Defense = (ushort)(Mob.Level * 10);
                                    Mob.MDef = (ushort)(Mob.Level);
                                    Mob.MAttack = (ushort)(Mob.Level * 10);
                                    Mob.MinAttack = (ushort)(Mob.Level * 60);
                                    Mob.MaxAttack = (ushort)(Mob.Level * 80);
                                    Mob.DmgReduceTimes = 1;
                                    Mob.Dodge = 1;
                                    Mob.AtkType = AttackType.Melee;
                                    Mob.Gives = true;
                                    Mob.AttackDist = 2;
                                    Mob.MinSilvers = Mob.Level * 10;
                                    Mob.MaxSilvers = Mob.Level * 100;
                                    Mob.SpawnSpeed = 0;

                                    Mob.CurrentHP = Mob.MaxHP;
                                    if (!World.H_Mobs.ContainsKey(Mob.Loc.Map))
                                    {
                                        World.H_Mobs.TryAdd(Mob.Loc.Map, new ConcurrentDictionary<uint, Mob>());
                                        World.PlayersInMap.Add(Mob.Loc.Map, new ConcurrentDictionary<uint, Character>());
                                    }

                                    Mob.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                                    while (World.H_Mobs[Mob.Loc.Map].ContainsKey(Mob.EntityID))
                                        Mob.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                                    World.H_Mobs[Mob.Loc.Map].TryAdd(Mob.EntityID, Mob);
                                    Mob.Alive = true;
                                    Mob.Respawn();
                                    break;
                                #endregion
                                #region /checksocs
                                case "@checksocs":
                                    System.Threading.Thread Bugg = new System.Threading.Thread(Database.Get2Sockets);
                                    Bugg.Start();
                                    break;
                                #endregion
                                #region DemonBoxes
                                case "@checkdboxes":
                                    GC.LocalMessage(2000, "Current Gold: " + Game.World.demonBoxesCur);
                                    break;
                                case "@curdboxes":
                                    Game.World.demonBoxesCur = Convert.ToUInt64(Cmd[1]);
                                    GC.LocalMessage(2000, "Current Gold: " + Game.World.demonBoxesCur);
                                    break;
                                case "@startdemonbox":
                                    NPC N = new NPC()
                                    {
                                        EntityID = 2084,
                                        Type = 1850,
                                        Flags = 1,
                                        Avatar = 0,
                                        Loc = new Location() { Map = 1002, X = 436, Y = 382 }
                                    };
                                    if (!World.H_NPCs.ContainsKey(N.Loc.Map))
                                        World.H_NPCs.Add(N.Loc.Map, new Dictionary<uint, NPC>());

                                    Dictionary<uint, NPC> NPCMap = World.H_NPCs[N.Loc.Map];
                                    if (!NPCMap.ContainsKey(N.EntityID))
                                    {
                                        NPCMap.Add(N.EntityID, N);
                                        World.Spawn(N);
                                        World.SendMsgToAll("EVENT", "DemonBoxes have arrived to Ultimate Conquer! Find DemonBoxDealer at Twin City center!", 2011, 0);
                                        World.SendMsgToAll("EVENT", "DemonBoxes have arrived to Ultimate Conquer! Find DemonBoxDealer at Twin City center!", 2005, 0);
                                        World.SendMsgToAll("EVENT", "DemonBoxes have arrived to Ultimate Conquer! Find DemonBoxDealer at Twin City center!", 2000, 0);
                                    }
                                    break;
                                case "@enddemonbox":
                                    if (World.H_NPCs.ContainsKey(1002))
                                    {
                                        if (World.H_NPCs[1002].ContainsKey(2084))
                                        {
                                            Game.World.Action(World.H_NPCs[1002][2084], Packets.GeneralData(2084, 0, 0, 0, 135).Get);
                                            World.H_NPCs[1002].Remove(2084);
                                        }
                                    }
                                    break;
                                #endregion
                                #region /chp
                                case "@chp":
                                    GC.MyChar.TotalDemonBoxes = int.Parse(Cmd[1]);
                                    break;
                                #endregion
                                case "@phash":
                                    int a = 0;
                                    foreach (KeyValuePair<string, uint> PHash in World.Anticheat.ToList())
                                    {
                                        World.AntiCheatAdd += a + ". PatchesHash was diff " + "(" + PHash.Key + ") Count: " + PHash.Value + "\r\n";
                                        a = a++;
                                    }
                                    break;
                                case "@safebool":
                                    World.SafeBool = !World.SafeBool;
                                    if (World.SafeBool)
                                        GC.LocalMessage(2000, $"Safe bool = {World.SafeBool}: higher rate");
                                    else
                                        GC.LocalMessage(2000, $"Safe bool = {World.SafeBool}: lower rate");
                                    break;
                                case "@snowballs":
                                    World.Snowballs = int.Parse(Cmd[1]);
                                    break;
                                case "@ignorenull":
                                    World.IgnoreNull = !World.IgnoreNull;
                                    GC.LocalMessage(2000, $"Ignore Null: {World.IgnoreNull}");
                                    break;
                                case "@dialog":
                                    GC.AddSend(Packets.ShowDialog(int.Parse(Cmd[1]), 1));
                                    break;
                                case "@reloaddialogs":
                                    World.Dialogs.Clear();
                                    PacketHandling.CustomDialog.LoadDialogs();
                                    PacketHandling.CustomDialog.GetDialogs(GC);
                                    break;
                                //case "@disablearena":
                                //    World.Arena = !World.Arena;
                                //    GC.LocalMessage(2000, $"Arena Qualifier: {World.Arena}");
                                //    break;
                                case "@info":
                                    GC.LocalMessage(2000, $"SpamIps: {World.SpammIps.Count}");
                                    GC.LocalMessage(2000, $"Keyed clients: {Main.AuthWorker.KeyedClients.Count}");
                                    GC.LocalMessage(2000, $"Game clients: {World.H_Clients.Count}");
                                    break;
                                case "@removekeys":
                                    GC.LocalMessage(2000, $"Keyed clients: {Main.AuthWorker.KeyedClients.Count}");
                                    GC.LocalMessage(2000, $"Game clients: {World.H_Clients.Count}");
                                    foreach (Main.AuthWorker.AuthInfo Info in Main.AuthWorker.KeyedClients.Values.ToList())
                                        if (DateTime.Now > Info.Used.AddSeconds(60))
                                            Main.AuthWorker.KeyedClients.Remove(Info.CryptoKey);
                                    GC.LocalMessage(2000, $"Keyed clients: {Main.AuthWorker.KeyedClients.Count}");
                                    break;
                                case "@getmails":
                                    List<string> Emails = new List<string>();
                                    foreach (string Path in System.IO.Directory.GetFiles(Game.World.GlobalAccountsPath))
                                    {
                                        if (Path.Remove(0, Path.Length - 4) == ".txt")
                                        {
                                            string Line = File.ReadAllText(Path);
                                            if (!Emails.Contains(Line))
                                                Emails.Add(Line);
                                            if (Line.Contains("emredogn@hotmail.com"))
                                            {

                                            }
                                        }
                                    }
                                    File.WriteAllLines("C:\\Debug\\Emails.txt", Emails);
                                    break;
                                case "@got":
                                    GameOfThones.Start();
                                    break;
                                case "@addvote":
                                    MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                                    Vote.Insert("votes").Insert("EntityID", Cmd[1]).Insert("LastVote", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Insert("IPAddress", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()).Execute();
                                    break;
                                case "@addpayment":
                                    break;
                                case "@goldsource":
                                    var myList = World.GoldSource.ToList();
                                    myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
                                    for (int d = 0; d < myList.Count; d++)
                                        World.GMChatAdd += myList[d].Key + ": " + myList[d].Value + "\r\n";
                                    break;
                                case "@resetarena":
                                    Features.ArenaQualifier.ResetRankings();
                                    break;
                                case "@diceturnover":
                                    GC.LocalMessage(2000, $"DiceKing: {World.DiceKingTurnOver}");
                                    break;
                                case "@stringpacket":
                                    string newPacket = "";
                                    for (int e = 3; e < Cmd.Length; e++)
                                        newPacket += Cmd[e];
                                    GC.AddSend(Packets.StringPacket(Convert.ToUInt32(Cmd[1]), (StringType)(Convert.ToByte(Cmd[2])), newPacket, true).Get);
                                    break;
                                case "@rolldice":
                                    var small = 0;
                                    var big = 0;
                                    var same = 0;
                                    Rnd = new Random();
                                    Dictionary<byte, int> Results = new Dictionary<byte, int>();
                                    for (int e = 0; e < 1000000; e++)
                                    {
                                        int Dice1 = Rnd.Next(1, 7);
                                        int Dice2 = Rnd.Next(1, 7);
                                        int Dice3 = Rnd.Next(1, 7);
                                        if (Dice3 == Dice2 && Dice3 == Dice1)
                                            Dice3 = Rnd.Next(1, 7);
                                        int sum = Dice1 + Dice2 + Dice3;
                                        if (sum <= 10)
                                            small++;
                                        else
                                            big++;
                                        if (Dice1 == Dice2 && Dice2 == Dice3)
                                            same++;
                                        if (!Results.ContainsKey((byte)sum))
                                            Results.Add((byte)sum, 1);
                                        else
                                            Results[(byte)sum]++;
                                    }
                                    GC.LocalMessage(2000, $"Small: {small} Big: {big} Same: {same}");
                                    var myList2 = Results.ToList();
                                    myList2.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
                                    foreach (KeyValuePair<byte, int> Score in myList2)
                                        GC.LocalMessage(2000, $"Number {Score.Key} Count: {Score.Value}");
                                    break;
                                case "@banwave":
                                    if (File.Exists("ToBeBanned.txt"))
                                    {
                                        string Complete = "";
                                        string[] BanList = File.ReadAllLines("ToBeBanned.txt");
                                        foreach (string Line in BanList)
                                        {
                                            C = Game.World.CharacterFromName(Line);
                                            if (C != null)
                                            {
                                                if (!Game.World.BanChars.Contains(C.Name))
                                                {
                                                    Game.World.BanChars.Add(C.Name);
                                                    if (C.MyClient != null)
                                                        if (C.MyClient.Soc.Connected)
                                                            C.MyClient.Soc.Disconnect(false);

                                                    MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("UID", C.EntityID);
                                                    //MySQL.MySqlReader CharsOnEmail = new MySQL.MySqlReader(Cmd2);
                                                    //while (CharsOnEmail.Read())
                                                    //{

                                                    //}
                                                    Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                                    Cmd2.Update("accounts").Set("Status", 5).Where("UID", C.EntityID).Execute();

                                                    if (C.MyGuild != null)
                                                        C.MyGuild.MemberLeaves(C.EntityID, false);

                                                    for (int i2 = 0; i2 <= 49; i2++)
                                                    {
                                                        if (Game.World.EmpireBoard[i2].ID == C.EntityID)
                                                            for (int j = i2; j <= 48; j++)
                                                            {
                                                                Game.World.EmpireBoard[j] = Game.World.EmpireBoard[j + 1];
                                                            }
                                                    }

                                                    C.Nobility.ListPlace = -1;
                                                    C.LastLogin = new DateTime(2016, 01, 01);
                                                    string Account = "";
                                                    Database.SaveCharacter(C, Account);


                                                    Complete += C.Name + " got banned!\r\n";
                                                    GC.LocalMessage(2000, C.Name + " got banned!");
                                                }
                                                else
                                                    GC.LocalMessage(2000, C.Name + " is already banned!");

                                                if (File.Exists(Game.World.GlobalCharactersPath + C.Name + ".chr"))
                                                    if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                                        File.Move(Game.World.GlobalCharactersPath + C.Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + C.Name + ".chr");
                                            }
                                            else
                                            {
                                                string Account = "";
                                                C = Database.LoadCharacter(Line, ref Account);
                                                if (C != null)
                                                {
                                                    if (!Game.World.BanChars.Contains(C.Name))
                                                    {
                                                        Game.World.BanChars.Add(C.Name);

                                                        MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                                        Cmd2.Update("accounts").Set("Status", 5).Where("UID", C.EntityID).Execute();

                                                        for (int i2 = 0; i2 <= 49; i2++)
                                                        {
                                                            if (Game.World.EmpireBoard[i2].ID == C.EntityID)
                                                                for (int j = i2; j <= 48; j++)
                                                                {
                                                                    Game.World.EmpireBoard[j] = Game.World.EmpireBoard[j + 1];
                                                                }
                                                        }

                                                        C.Nobility.ListPlace = -1;
                                                        C.LastLogin = new DateTime(2016, 01, 01);
                                                        Database.SaveCharacter(C, Account);

                                                        GC.LocalMessage(2000, C.Name + " got banned!");

                                                        Complete += C.Name + " got banned!\r\n";
                                                    }
                                                    else
                                                    {
                                                        Complete += Line + " was already banned!\r\n";
                                                        GC.LocalMessage(2000, C.Name + " is already banned!");
                                                    }

                                                }
                                                else
                                                    GC.LocalMessage(2000, Line + " does not exist!");

                                                if (File.Exists(Game.World.GlobalCharactersPath + C.Name + ".chr"))
                                                    if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                                        File.Move(Game.World.GlobalCharactersPath + C.Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + C.Name + ".chr");
                                            }
                                        }
                                        World.GMChatAdd += Complete;
                                    }
                                    break;
                                case "@fixbans":
                                    foreach (string Name in World.BanChars)
                                        if (File.Exists(Game.World.GlobalCharactersPath + Name + ".chr"))
                                            if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                                File.Move(Game.World.GlobalCharactersPath + Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + Name + ".chr");

                                    //MySQL.MySqlCommand Cmd4 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("Status", 5);
                                    //MySQL.MySqlReader Bans = new MySQL.MySqlReader(Cmd4);
                                    //while (Bans.Read())
                                    //{
                                    //    MySQL.MySqlCommand Cmd5 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("UID", Bans.ReadUInt32("UID"));
                                    //    MySQL.MySqlReader BanChar = new MySQL.MySqlReader(Cmd5);
                                    //    if (BanChar.Read())

                                    //}

                                    break;
                                case "@sound":
                                    World.Action(Packets.StringPacket(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, StringType.Sound, Cmd[1]).Get);
                                    //GC.AddSend(Packets.StringPacket(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, StringType.Sound, Cmd[1]));
                                    break;
                                case "@screenshot":
                                    GC.AddSend(Packets.ShowDialog(33, 1));
                                    break;
                                case "@screengarment":
                                    {
                                        foreach (Item I in GC.MyChar.Inventory)
                                        {
                                            GC.AddSend(Packets.OverwriteGarment(I.ID));
                                            System.Threading.Thread.Sleep(500);
                                            var image = ScreenCapture.CaptureActiveWindow();
                                            image.Save(@"C:\OldCODB\log\Garments\" + I.ID + ".png", ImageFormat.Png);
                                        }
                                    }
                                    break;
                                case "@scaddnpc":
                                    {//uid type flags avatar
                                     //for (int a = 5; a < 20000; a++)
                                     //{
                                     //    try
                                     //    {
                                     //        Game.NPC N = new Game.NPC("2053" + ' ' + a + /*GC.MyChar.Direction +*/ ' ' + "2" + ' ' + "7" + ' ' + GC.MyChar.Loc.Map + ' ' + ((ushort)(GC.MyChar.Loc.X + 2)) + ' ' + GC.MyChar.Loc.Y);
                                     //        //Game.World.H_NPCs.Add(N.EntityID, N);
                                     //        Game.World.Spawn(N);

                                        //        System.Threading.Thread.Sleep(500);

                                        foreach (Item I in GC.MyChar.Inventory)
                                        {
                                            GC.AddSend(Packets.OverwriteGarment(I.ID));
                                            System.Threading.Thread.Sleep(500);
                                            var image = ScreenCapture.CaptureActiveWindow();
                                            image.Save(@"C:\OldCODB\log\Garments\" + I.ID + ".png", ImageFormat.Png);
                                        }
                                        //foreach (uint UID in NPCs.NPC_2.RegularGarments)
                                        //{
                                        //    GC.AddSend(Packets.OverwriteGarment(UID));
                                        //    System.Threading.Thread.Sleep(500);
                                        //    var image = ScreenCapture.CaptureActiveWindow();
                                        //    image.Save(@"C:\Users\Nyorai\Desktop\Garments\" + UID + ".png", ImageFormat.Png);
                                        //}
                                        //foreach (uint UID in NPCs.NPC_2.RareGarments)
                                        //{
                                        //    GC.AddSend(Packets.OverwriteGarment(UID));
                                        //    System.Threading.Thread.Sleep(500);
                                        //    var image = ScreenCapture.CaptureActiveWindow();
                                        //    image.Save(@"C:\Users\Nyorai\Desktop\Garments\" + UID + ".png", ImageFormat.Png);
                                        //}
                                        //foreach (uint UID in NPCs.NPC_2.SpecialGarments)
                                        //{
                                        //    GC.AddSend(Packets.OverwriteGarment(UID));
                                        //    System.Threading.Thread.Sleep(500);
                                        //    var image = ScreenCapture.CaptureActiveWindow();
                                        //    image.Save(@"C:\Users\Nyorai\Desktop\Garments\" + UID + ".png", ImageFormat.Png);
                                        //}

                                        GC.MyChar.Body = 2001;

                                        foreach (Item I in GC.MyChar.Inventory)
                                        {
                                            GC.AddSend(Packets.OverwriteGarment(I.ID));
                                            System.Threading.Thread.Sleep(500);
                                            var image = ScreenCapture.CaptureActiveWindow();
                                            image.Save(@"C:\OldCODB\log\Garments\" + (I.ID + 1).ToString() + ".png", ImageFormat.Png);
                                        }
                                        //foreach (uint UID in NPCs.NPC_2.RegularGarments)
                                        //{
                                        //    GC.AddSend(Packets.OverwriteGarment(UID));
                                        //    System.Threading.Thread.Sleep(500);
                                        //    var image = ScreenCapture.CaptureActiveWindow();
                                        //    image.Save(@"C:\Users\Nyorai\Desktop\Garments\" + (UID + 1).ToString() + ".png", ImageFormat.Png);
                                        //}
                                        //foreach (uint UID in NPCs.NPC_2.RareGarments)
                                        //{
                                        //    GC.AddSend(Packets.OverwriteGarment(UID));
                                        //    System.Threading.Thread.Sleep(500);
                                        //    var image = ScreenCapture.CaptureActiveWindow();
                                        //    image.Save(@"C:\Users\Nyorai\Desktop\Garments\" + (UID + 1).ToString() + ".png", ImageFormat.Png);
                                        //}
                                        //foreach (uint UID in NPCs.NPC_2.SpecialGarments)
                                        //{
                                        //    GC.AddSend(Packets.OverwriteGarment(UID));
                                        //    System.Threading.Thread.Sleep(500);
                                        //    var image = ScreenCapture.CaptureActiveWindow();
                                        //    image.Save(@"C:\Users\Nyorai\Desktop\Garments\" + (UID + 1).ToString() + ".png", ImageFormat.Png);
                                        //}
                                        //    System.Threading.Thread.Sleep(1000);
                                        //    a += 5;
                                        //}
                                        //catch (Exception e)
                                        //{
                                        //    Console.WriteLine("NPC " + a + " doesn't exist '{0}'", e);
                                        //    a += 5;
                                        //}
                                        //}
                                        break;
                                    }
                                case "@myctrls":
                                    string toWrite = "";
                                    foreach (string Path in Directory.GetFiles("C:\\OldCODB\\log\\Garments\\"))
                                    {
                                        if (Path.Remove(0, Path.Length - 4) == ".png")
                                        {
                                            string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                                            toWrite += $"[Image{Name}]\nFrameAmount=1\nFrame0=data/main/MyCtrls/Costumer/Garments/{Name}.dds\n\n";
                                        }
                                    }
                                    foreach (Item I in GC.MyChar.Inventory)
                                        toWrite += I.ID + ", ";
                                    System.IO.File.WriteAllText("C:\\OldCODB\\log\\Garments\\MyControls.txt", toWrite);
                                    break;
                                case "@loadeventnpc":
                                    Database.LoadNPCs(Convert.ToByte(Cmd[1]));
                                    break;
                                case "@testdialog":
                                    GC.AddSend(Packets.ShowDialog(34, 1));

                                    //CustomDialog.DlgBtnData Button = new CustomDialog.DlgBtnData() { AniHeight = 36, AniWidth = 110, xpos = Convert.ToUInt16(Cmd[1]), ypos = Convert.ToUInt16(Cmd[2]), Height = 36, Width = 110, TipColor = 0, TipStr = "" };
                                    //Button.AniId = 10142;
                                    //Button.ButtonUID = Button.AniId;

                                    //GC.AddSend(Packets.DynamicButton((int)34, Button));

                                    //Button = new CustomDialog.DlgBtnData() { AniHeight = 36, AniWidth = 110, xpos = Convert.ToUInt16(Cmd[3]), ypos = Convert.ToUInt16(Cmd[2]), Height = 36, Width = 110, TipColor = 0, TipStr = "" };
                                    //Button.AniId = 10143;
                                    //Button.ButtonUID = Button.AniId;

                                    //GC.AddSend(Packets.DynamicButton((int)34, Button));

                                    Bosses.BossHandler.WindowInformation(GC.MyChar);
                                    break;

                                case "@testgarment":
                                    GC.AddSend(Packets.OverwriteGarment(uint.Parse(Cmd[1])));
                                    break;
                                case "@testaccessory":
                                    GC.AddSend(Packets.OverwriteWeapon(uint.Parse(Cmd[1])));
                                    break;

                                #region /movenpc
                                case "@movenpc":
                                    {
                                        uint npcId = uint.Parse(Cmd[1]);
                                        if (World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                                            if (World.H_NPCs[GC.MyChar.Loc.Map].ContainsKey(npcId))
                                            {
                                                var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                                {
                                                    cmd.Update("npcs").Set("X", ushort.Parse(Cmd[2]))
                                                        .Set("Y", ushort.Parse(Cmd[3]))
                                                        .Where("UID", npcId);
                                                    cmd.Execute();
                                                }
                                                var npc = World.H_NPCs[GC.MyChar.Loc.Map][npcId];
                                                npc.Loc.X = ushort.Parse(Cmd[2]);
                                                npc.Loc.Y = ushort.Parse(Cmd[3]);
                                                World.Spawn(GC.MyChar, true);
                                            }
                                        break;
                                    }

                                #endregion
                                #region /removenpc
                                case "@removenpc":
                                    {
                                        uint npcId = uint.Parse(Cmd[1]);
                                        if (World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                                            if (World.H_NPCs[GC.MyChar.Loc.Map].ContainsKey(npcId))
                                            {
                                                var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                                                {
                                                    cmd.Delete("npcs", "UID", npcId).Execute();
                                                }
                                                World.H_NPCs[GC.MyChar.Loc.Map].Remove(npcId);
                                                World.Spawn(GC.MyChar, true);
                                            }
                                        break;
                                    }

                                #endregion


                                #region /lookface
                                case "@lookface":
                                    {
                                        uint npcId = uint.Parse(Cmd[1]);
                                        if (World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                                            if (World.H_NPCs[GC.MyChar.Loc.Map].ContainsKey(npcId))
                                            {
                                                var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                                {
                                                    cmd.Update("npcs")
                                                        .Set("Type", ushort.Parse(Cmd[2]))
                                                        .Where("UID", npcId);
                                                    cmd.Execute();
                                                }
                                                var npc = World.H_NPCs[GC.MyChar.Loc.Map][npcId];
                                                npc.Type = ushort.Parse(Cmd[2]);
                                                World.Spawn(GC.MyChar, true);
                                            }
                                        break;
                                    }

                                #endregion


                                #region /lookface1
                                case "@lookface1":
                                    {
                                        ushort X;
                                        X = (ushort)Program.Rnd.Next(10, 399);
                                        foreach (Character CC in World.H_Chars.Values)
                                            if (World.H_NPCs.ContainsKey(1002))

                                                if (World.H_NPCs[1002].ContainsKey(10019))
                                                {
                                                    var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                                    {
                                                        cmd.Update("npcs")
                                                            .Set("Type", X)
                                                            .Where("UID", 10019);
                                                        cmd.Execute();
                                                    }
                                                    var npc = World.H_NPCs[1002][10019];
                                                    npc.Type = X;
                                                    World.Spawn(CC, true);

                                                }
                                        break;
                                    }

                                #endregion

                                #region /lastwinner
                                case "@lastwinner":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (G.GuildName == Name)
                                            {
                                                Features.GuildWars.LastWinner = G;
                                                Features.GuildWars.ThePole.ReSpawn();
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                #endregion

                                #region /getinfo
                                case "@getinfo":
                                    {
                                        Game.Character c = Game.World.CharacterFromName(Cmd[1]);
                                        if (c != null)
                                        {
                                            Game.World.DebugAdd += "Name: " + c.Name + "\r\n";
                                            Game.World.DebugAdd += "Location Map: " + c.Loc.Map + " X: " + c.Loc.X + " Y: " + c.Loc.Y + "\r\n";
                                            if (c.Level < 130)
                                                Game.World.DebugAdd += "Level: " + c.Level + " PC: " + (c.Experience * 100) / Database.LevelExp[c.Level] + "\r\n";
                                            else Game.World.DebugAdd += "Level: " + c.Level + "\r\n";
                                            Game.World.DebugAdd += "WH Silvers: " + c.WHSilvers + "\r\n";
                                            Game.World.DebugAdd += "Silvers: " + c.Silvers + "\r\n";
                                            if (c.VIPDays > 0)
                                                Game.World.DebugAdd += "VIP Days Left: " + c.VIPDays + " VIP: " + c.VipLevel + "\r\n";
                                            string Items = "MA WH: ";
                                            foreach (Game.Item I in c.Warehouses.MAWarehouse)
                                                Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                                            Game.World.DebugAdd += Items + "\r\n";
                                            Items = "TC WH: ";
                                            foreach (Game.Item I in c.Warehouses.TCWarehouse)
                                                Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                                            Game.World.DebugAdd += Items + "\r\n";
                                        }
                                        else
                                        {
                                            string Account = "";
                                            C = Database.LoadCharacter(Cmd[1], ref Account);
                                            if (C != null)
                                            {
                                                Game.World.DebugAdd += "Name: " + c.Name + "\r\n";
                                                Game.World.DebugAdd += "Location Map: " + c.Loc.Map + " X: " + c.Loc.X + " Y: " + c.Loc.Y + "\r\n";
                                                Game.World.DebugAdd += "Level: " + c.Level + "\r\n";
                                                if (c.Level < 130)
                                                    Game.World.DebugAdd += "Exp left: " + (Database.LevelExp[c.Level] - c.Experience) + "\r\n";
                                                Game.World.DebugAdd += "WH Silvers: " + c.WHSilvers + "\r\n";
                                                Game.World.DebugAdd += "Silvers: " + c.Silvers + "\r\n";
                                                if (c.VIPDays > 0)
                                                    Game.World.DebugAdd += "VIP Days Left: " + c.VIPDays + " VIP: " + c.VipLevel + "\r\n";
                                                string Items = "MA WH: ";
                                                foreach (Game.Item I in c.Warehouses.MAWarehouse)
                                                    Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                                                Game.World.DebugAdd += Items + "\r\n";
                                                Items = "TC WH: ";
                                                foreach (Game.Item I in c.Warehouses.TCWarehouse)
                                                    Items += I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + "  ";
                                                Game.World.DebugAdd += Items + "\r\n";
                                                Database.SaveCharacter(C, Account);
                                            }
                                        }
                                        break;
                                    }
                                #endregion

                                #region /body
                                case "@bodymob":
                                    {
                                        if (Cmd[1] == "whitetiger")
                                        {
                                            GC.MyChar.Body = 384;
                                        }
                                        if (Cmd[1] == "yellowtiger")
                                        {
                                            GC.MyChar.Body = 385;
                                        }
                                        if (Cmd[1] == "redtiger")
                                        {
                                            GC.MyChar.Body = 386;
                                        }
                                        if (Cmd[1] == "smale")
                                        {
                                            GC.MyChar.Body = 1003;
                                        }
                                        if (Cmd[1] == "lmale")
                                        {
                                            GC.MyChar.Body = 1004;
                                        }
                                        if (Cmd[1] == "sfemale")
                                        {
                                            GC.MyChar.Body = 2001;
                                        }
                                        if (Cmd[1] == "lfemale")
                                        {
                                            GC.MyChar.Body = 2002;
                                        }
                                        if (Cmd[1] == "guard")
                                        {
                                            GC.MyChar.Body = 900;
                                        }
                                        if (Cmd[1] == "guard2")
                                        {
                                            GC.MyChar.Body = 910;
                                        }
                                        if (Cmd[1] == "guard3")
                                        {
                                            GC.MyChar.Body = 920;
                                        }
                                        if (Cmd[1] == "nd")
                                        {
                                            GC.MyChar.Body = 377;
                                        }
                                        if (Cmd[1] == "satan")
                                        {
                                            GC.MyChar.Body = 166;
                                        }
                                        if (Cmd[1] == "vampire")
                                        {
                                            GC.MyChar.Body = 111;
                                        }
                                        if (Cmd[1] == "bunny")
                                        {
                                            GC.MyChar.Body = 222;
                                        }
                                        if (Cmd[1] == "bunny2")
                                        {
                                            GC.MyChar.Body = 224;
                                        }
                                        if (Cmd[1] == "bunny3")
                                        {
                                            GC.MyChar.Body = 225;
                                        }
                                        if (Cmd[1] == "fairy")
                                        {
                                            GC.MyChar.Body = 130;
                                        }
                                        if (Cmd[1] == "pig")
                                        {
                                            GC.MyChar.Body = 215;
                                        }
                                        if (Cmd[1] == "titan")
                                        {
                                            GC.MyChar.Body = 153;
                                        }
                                        if (Cmd[1] == "pluto")
                                        {
                                            GC.MyChar.Body = 168;
                                        }
                                        if (Cmd[1] == "revenant")
                                        {
                                            GC.MyChar.Body = 265;
                                        }
                                        if (Cmd[1] == "eidolon")
                                        {
                                            GC.MyChar.Body = 266;
                                        }
                                        if (Cmd[1] == "troll")
                                        {
                                            GC.MyChar.Body = 353;
                                        }
                                        if (Cmd[1] == "soldier")
                                        {
                                            GC.MyChar.Body = 263;
                                        }
                                        if (Cmd[1] == "phantom")
                                        {
                                            GC.MyChar.Body = 363;
                                        }
                                        if (Cmd[1] == "spearman")
                                        {
                                            GC.MyChar.Body = 165;
                                        }
                                        if (Cmd[1] == "titan")
                                        {
                                            GC.MyChar.Body = 153;
                                        }
                                        if (Cmd[1] == "gano")
                                        {
                                            GC.MyChar.Body = 133;
                                        }
                                        if (Cmd[1] == "phe")
                                        {
                                            GC.MyChar.Body = 104;
                                        }
                                        if (Cmd[1] == "ghost")
                                        {
                                            GC.MyChar.Body = 1098;
                                        }
                                        break;
                                    }
                                #endregion

                                #region /bodymob2
                                case "@body2":
                                    {
                                        GC.MyChar.Body = ushort.Parse(Cmd[1]);
                                    }
                                    break;
                                #endregion

                                #region /drop
                                case "@drop":
                                    {
                                        uint DropID1 = 0;

                                        Random Rnd1 = new Random();
                                        string DropWhat = Cmd[1].ToLower();
                                        byte HowMany = (byte)Math.Min(ushort.Parse(Cmd[2]), (ushort)255);
                                        switch (DropWhat)
                                        {
                                            case "dragonball": DropID1 = 1088000; break;
                                            case "meteor": DropID1 = 720027; break;
                                            case "moonbox": DropID1 = 721080; break;
                                            case "celestial": DropID1 = 721259; break;
                                            case "cleanwater": DropID1 = 721258; break;
                                        }
                                        Game.DroppedItem DI1;
                                        for (int x = 0; x < HowMany; x++)
                                        {
                                            DI1 = new Game.DroppedItem();
                                            DI1.DropTime = DateTime.Now;
                                            DI1.Loc = new Game.Location();
                                            DI1.Loc.Map = GC.MyChar.Loc.Map;
                                            DI1.Info = new Game.Item();
                                            DI1.Info.ID = DropID1;


                                            DI1.UID = (uint)Rnd1.Next(10000000);
                                            DI1.Info.UID = DI1.UID;
                                            DI1.Loc.X = (ushort)(GC.MyChar.Loc.X + Rnd1.Next(15) - Rnd1.Next(15));
                                            DI1.Loc.Y = (ushort)(GC.MyChar.Loc.Y + Rnd1.Next(15) - Rnd1.Next(15));
                                            if (!DI1.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[GC.MyChar.Loc.Map])) return;
                                            DI1.Drop();
                                        }
                                        break;
                                    }

                                #endregion

                                #region /atk
                                case "@atk":
                                    GC.AddSend(Packets.AttackPacket(GC.MyChar.EntityID, GC.MyChar.EntityID, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 9999, byte.Parse(Cmd[1])));
                                    break;
                                #endregion

                                #region /recmsg
                                case "@recmsg":
                                    {
                                        GC.LocalMessage(ushort.Parse(Cmd[1]), "Message received!");
                                    }
                                    break;
                                #endregion

                                #region /shopflag
                                case "@shopflag":
                                    {
                                        Game.NPC N1 = new Ultimate.Game.NPC(1234.ToString() + ' ' + 1080 + ' ' + 16 + ' ' + 6 + ' ' + 0 + ' ' + GC.MyChar.Loc.Map + ' ' + GC.MyChar.Loc.X + ' ' + GC.MyChar.Loc.Y);
                                        Game.World.Spawn(N1);
                                        break;
                                    }
                                #endregion

                                #region /tryatk
                                case "@tryatk":
                                    GC.AddSend(Packets.AttackPacket(GC.MyChar.EntityID, GC.MyChar.EntityID, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 300, byte.Parse(Cmd[1])));
                                    break;
                                #endregion


                                #region /remguild
                                case "@remguild":
                                    {
                                        Game.Character c = Game.World.CharacterFromName(Cmd[1]);
                                        if (c != null && c.MyGuild != null)
                                        {
                                            if (c.GuildRank != Features.GuildRank.GuildLeader && c.MyGuild.GuildName != "")
                                            {
                                                c.MyClient.AddSend(Packets.SendGuild(c.MyGuild.GuildID, 19));
                                                c.MyGuild.MemberLeaves(c.EntityID, false);
                                                c.GuildRank = 0;
                                                c.GuildDonation = 0;
                                                c.MyGuild = null;
                                                Game.World.Spawn(c, false);
                                            }
                                        }
                                        break;
                                    }
                                #endregion

                                #region /makegl
                                case "@makegl":
                                    {
                                        Game.Character c = Game.World.CharacterFromName(Cmd[1]);
                                        if (c != null)
                                        {
                                            Features.MemberInfo M = c.MyGuild.MembOfName(c.Name);
                                            if (M != null)
                                            {
                                                if (M.Rank == Features.GuildRank.Member)
                                                {
                                                    M.Rank = Ultimate.Features.GuildRank.GuildLeader;
                                                    (c.MyGuild.Members[(byte)50]).Remove(M.MembID);
                                                    (c.MyGuild.Members[(byte)100]).Add(M.MembID, M);
                                                }
                                                else if (M.Rank == Features.GuildRank.DeputyManager)
                                                {
                                                    M.Rank = Ultimate.Features.GuildRank.GuildLeader;
                                                    (c.MyGuild.Members[(byte)90]).Remove(M.MembID);
                                                    (c.MyGuild.Members[(byte)100]).Add(M.MembID, M);
                                                }
                                                Game.Character Ch = M.Info;
                                                if (Ch != null)
                                                {
                                                    Ch.GuildRank = Ultimate.Features.GuildRank.GuildLeader;
                                                    Game.World.Spawn(c, false);
                                                    Ch.MyClient.AddSend(Packets.GuildInfo(c.MyGuild, c));
                                                }
                                            }
                                        }
                                        break;
                                    }
                                #endregion

                                #region /makedl
                                case "@makedl":
                                    {
                                        Game.Character c = Game.World.CharacterFromName(Cmd[1]);
                                        if (c != null)
                                        {
                                            Features.MemberInfo M = c.MyGuild.MembOfName(c.Name);
                                            if (M != null)
                                            {
                                                if (M.Rank == Features.GuildRank.Member)
                                                {
                                                    M.Rank = Ultimate.Features.GuildRank.GuildLeader;
                                                    (c.MyGuild.Members[(byte)50]).Remove(M.MembID);
                                                    (c.MyGuild.Members[(byte)90]).Add(M.MembID, M);
                                                }
                                                else if (M.Rank == Features.GuildRank.DeputyManager)
                                                {
                                                    M.Rank = Ultimate.Features.GuildRank.GuildLeader;
                                                    (c.MyGuild.Members[(byte)90]).Remove(M.MembID);
                                                    (c.MyGuild.Members[(byte)90]).Add(M.MembID, M);
                                                }
                                                Game.Character Ch = M.Info;
                                                if (Ch != null)
                                                {
                                                    Ch.GuildRank = Ultimate.Features.GuildRank.DeputyManager;
                                                    Game.World.Spawn(c, false);
                                                    Ch.MyClient.AddSend(Packets.GuildInfo(c.MyGuild, c));
                                                }
                                            }
                                        }
                                        break;
                                    }
                                #endregion

                                #region /kick2
                                case "@kick2":
                                    {
                                        Game.Character c = Game.World.CharacterFromName(Cmd[1]);
                                        if (c != null)
                                            c.MyClient.LogOff();
                                        break;
                                    }
                                #endregion

                                #region /mob
                                case "@mob":
                                    GC.AddSend(Packets.SpawnEntity(ushort.Parse(Cmd[1]), "FuckFace" + Cmd[1], GC.MyChar.Loc));
                                    break;
                                #endregion

                                #region /checkmob
                                case "@checkmob":
                                    {
                                        foreach (ConcurrentDictionary<uint, Mob> H in World.H_Mobs.Values)
                                        {
                                            foreach (Game.Mob M in H.Values)
                                            {
                                                if (M.MobID == int.Parse(Cmd[1]))
                                                    if (M.Alive)
                                                    {
                                                        GC.LocalMessage(2000, M.Name + " (" + M.Loc.Map + "," + M.Loc.X + "," + M.Loc.Y + ") HP: " + M.CurrentHP);
                                                    }
                                            }
                                        }
                                        break;
                                    }
                                #endregion

                                #region /top
                                case "@top":
                                    {
                                        GC.MyChar.Top = int.Parse(Cmd[1]);
                                    }
                                    break;
                                #endregion

                                #region /topp
                                case "@topp":
                                    {

                                        Game.Character c = Game.World.CharacterFromName(Cmd[1]);
                                        if (c != null)
                                        {
                                            c.Top = int.Parse(Cmd[2]);
                                        }
                                        break;
                                    }
                                #endregion


                                #region /supermanall
                                case "@supermanall":
                                    DateTime TimeNow1 = DateTime.Now;
                                    foreach (Game.Character C22 in Game.World.H_Chars.Values)
                                    {
                                        Buff S = C22.BuffOf(Features.SkillsClass.ExtraEffect.Superman);
                                        Buff CC = C22.BuffOf(Features.SkillsClass.ExtraEffect.Cyclone);

                                        if (S.Eff != Features.SkillsClass.ExtraEffect.Superman && CC.Eff != Features.SkillsClass.ExtraEffect.Cyclone)
                                        {
                                            Buff B2 = new Buff();
                                            B2.Eff = Features.SkillsClass.ExtraEffect.Cyclone;
                                            B2.Lasts = 90;
                                            B2.Value = 90;
                                            B2.Started = TimeNow1;
                                            B.StEff = Game.StatusEffectEn.SuperMan;
                                            C22.TimeBuff = B2.Lasts;
                                            C22.AddBuff(B2);

                                            C22.MyClient.LocalMessage(2011, "Superman Event! You received cyclone for 90 seconds! Use it wisely!");
                                        }

                                    }
                                    Game.World.CycloneEvent = TimeNow1;
                                    break;
                                #endregion

                                #region /freezeall
                                case "@freezeall":
                                    DateTime TimeNow2 = DateTime.Now;
                                    foreach (Game.Character C22 in Game.World.H_Chars.Values)
                                    {
                                        Buff S = C22.BuffOf(Features.SkillsClass.ExtraEffect.Superman);
                                        Buff CC = C22.BuffOf(Features.SkillsClass.ExtraEffect.Cyclone);

                                        if (S.Eff != Features.SkillsClass.ExtraEffect.Superman && CC.Eff != Features.SkillsClass.ExtraEffect.Cyclone)
                                        {
                                            Buff B2 = new Buff();
                                            B2.Eff = Features.SkillsClass.ExtraEffect.IceBlock;
                                            B2.Lasts = 90;
                                            B2.Value = 90;
                                            B2.Started = TimeNow2;
                                            B.StEff = Game.StatusEffectEn.IceBlock;
                                            C22.TimeBuff = B2.Lasts;
                                            C22.AddBuff(B2);

                                            C22.MyClient.LocalMessage(2011, "Superman Event! You received cyclone for 90 seconds! Use it wisely!");
                                        }

                                    }
                                    Game.World.CycloneEvent = TimeNow2;
                                    break;
                                #endregion


                                #region /winnergw
                                case "@winnergw":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {

                                            if (G.GuildName == Name)
                                            {
                                                Features.GuildWars.LastWinner = G; Features.GuildWars.ThePole.ReSpawn();
                                                World.H_SOBs[Features.GuildWars.ThePole.EntityID].LastWinner = G;
                                                //World.SendMsgToAll("SYSTEM", Features.GuildWars.LastWinner.GuildName + " GuildWar War have won!", 2011, 0);
                                                //World.SendMsgToAll("SYSTEM", Features.GuildWars.LastWinner.GuildName + " GuildWar War have won!", 2000, 0);
                                            }
                                        }
                                        World.H_SOBs[Features.GuildWars.ThePole.EntityID].CurHP = World.H_SOBs[Features.GuildWars.ThePole.EntityID].MaxHP;
                                        World.H_SOBs[Features.GuildWars.ThePole.EntityID].ReSpawn();


                                        MySQL.MySqlCommand Cmd2;
                                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("guildwars").Set("winner", Features.GuildWars.LastWinner.GuildName).Where("id", 1).Execute();

                                    }
                                    break;
                                #endregion

                                #region /eggevent
                                case "@eggevent":
                                    {
                                        World.EventDB = false;
                                        World.EventElite = true;
                                        World.EventGem = false;
                                        World.EventMet = false;
                                        World.EventProfExp = false;
                                        World.EventSkillExp = false;
                                        World.EventSuper = false;
                                        World.EventPlus = false;
                                        World.SendMsgToAll("SYSTEM", "Egg items drop rate has been increased for the next hour!", 2500, 0, GC.MyChar.Loc.Map);
                                    }
                                    break;
                                #endregion

                                #region /winnertc
                                case "@winnertc":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (G.GuildName == Name)
                                            {
                                                Features.CityWarTc.LastWinner = G; Features.CityWarTc.ThePole.ReSpawn();
                                                World.H_SOBs[Features.CityWarTc.ThePole.EntityID].LastWinner = G;
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarTc.LastWinner.GuildName + " TwinCity War have won!", 2011, 0);
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarTc.LastWinner.GuildName + " TwinCity War have won!", 2000, 0);
                                            }
                                        }
                                        World.H_SOBs[Features.CityWarTc.ThePole.EntityID].CurHP = World.H_SOBs[Features.CityWarTc.ThePole.EntityID].MaxHP;
                                        World.H_SOBs[Features.CityWarTc.ThePole.EntityID].ReSpawn();


                                        MySQL.MySqlCommand Cmd2;
                                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("guildwars").Set("winner", Features.CityWarTc.LastWinner.GuildName).Where("id", 2).Execute();
                                    }
                                    break;

                                #endregion

                                #region /winnerpc
                                case "@winnerpc":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (G.GuildName == Name)
                                            {
                                                Features.CityWarPc.LastWinner = G; Features.CityWarPc.ThePole.ReSpawn();
                                                World.H_SOBs[Features.CityWarPc.ThePole.EntityID].LastWinner = G;
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarPc.LastWinner.GuildName + " PhoenixCity War have won!", 2011, 0);
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarPc.LastWinner.GuildName + " PhoenixCity War have won!", 2000, 0);
                                            }
                                        }
                                        World.H_SOBs[Features.CityWarPc.ThePole.EntityID].CurHP = World.H_SOBs[Features.CityWarPc.ThePole.EntityID].MaxHP;
                                        World.H_SOBs[Features.CityWarPc.ThePole.EntityID].ReSpawn();


                                        MySQL.MySqlCommand Cmd2;
                                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("guildwars").Set("winner", Features.CityWarPc.LastWinner.GuildName).Where("id", 3).Execute();
                                    }
                                    break;
                                #endregion

                                #region /winnerac
                                case "@winnerac":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (G.GuildName == Name)
                                            {
                                                Features.CityWarAc.LastWinner = G; Features.CityWarAc.ThePole.ReSpawn();
                                                World.H_SOBs[Features.CityWarAc.ThePole.EntityID].LastWinner = G;
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarAc.LastWinner.GuildName + " ApeCity War have won!", 2011, 0);
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarAc.LastWinner.GuildName + " ApeCity War have won!", 2000, 0);
                                            }
                                        }
                                        World.H_SOBs[Features.CityWarAc.ThePole.EntityID].CurHP = World.H_SOBs[Features.CityWarAc.ThePole.EntityID].MaxHP;
                                        World.H_SOBs[Features.CityWarAc.ThePole.EntityID].ReSpawn();


                                        MySQL.MySqlCommand Cmd2;
                                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("guildwars").Set("winner", Features.CityWarAc.LastWinner.GuildName).Where("id", 4).Execute();
                                    }
                                    break;
                                #endregion

                                #region /winnerdc
                                case "@winnerdc":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (G.GuildName == Name)
                                            {
                                                Features.CityWarDc.LastWinner = G; Features.CityWarDc.ThePole.ReSpawn();
                                                World.H_SOBs[Features.CityWarDc.ThePole.EntityID].LastWinner = G;
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarDc.LastWinner.GuildName + " DesertCity War have won!", 2011, 0);
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarDc.LastWinner.GuildName + " DesertCity War have won!", 2000, 0);
                                            }
                                        }
                                        World.H_SOBs[Features.CityWarDc.ThePole.EntityID].CurHP = World.H_SOBs[Features.CityWarDc.ThePole.EntityID].MaxHP;
                                        World.H_SOBs[Features.CityWarDc.ThePole.EntityID].ReSpawn();


                                        MySQL.MySqlCommand Cmd2;
                                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("guildwars").Set("winner", Features.CityWarDc.LastWinner.GuildName).Where("id", 5).Execute();
                                    }
                                    break;
                                #endregion

                                #region /winnerbi
                                case "@winnerbi":
                                    {
                                        string Name = Cmd[1];
                                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (G.GuildName == Name)
                                            {
                                                Features.CityWarBi.LastWinner = G; Features.CityWarBi.ThePole.ReSpawn();
                                                World.H_SOBs[Features.CityWarBi.ThePole.EntityID].LastWinner = G;
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarBi.LastWinner.GuildName + " BirdIsland War have won!", 2011, 0);
                                                //World.SendMsgToAll("SYSTEM", Features.CityWarBi.LastWinner.GuildName + " BirdIsland War have won!", 2000, 0);
                                            }
                                        }
                                        World.H_SOBs[Features.CityWarBi.ThePole.EntityID].CurHP = World.H_SOBs[Features.CityWarBi.ThePole.EntityID].MaxHP;
                                        World.H_SOBs[Features.CityWarBi.ThePole.EntityID].ReSpawn();


                                        MySQL.MySqlCommand Cmd2;
                                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        Cmd2.Update("guildwars").Set("winner", Features.CityWarBi.LastWinner.GuildName).Where("id", 6).Execute();
                                    }
                                    break;
                                #endregion


                                #region /winnertc1
                                case "@winnertc1":
                                    {
                                        Features.CityWarTc.LastWinner.GuildName = GC.MyChar.MyGuild.GuildName;
                                        GC.LocalMessage(2000, "Last Guild War winner set to YOUR Guild!");
                                        break;
                                    }
                                #endregion

                                #region /winnerpc1
                                case "@winnerpc1":
                                    {
                                        Features.CityWarPc.LastWinner.GuildName = GC.MyChar.MyGuild.GuildName;
                                        GC.LocalMessage(2000, "Last Guild War winner set to YOUR Guild!");
                                        break;
                                    }
                                #endregion

                                #region /winnerac1
                                case "@winnerac1":
                                    {
                                        Features.CityWarAc.LastWinner.GuildName = GC.MyChar.MyGuild.GuildName;
                                        GC.LocalMessage(2000, "Last Guild War winner set to YOUR Guild!");
                                        break;
                                    }
                                #endregion

                                #region /winnderdc1
                                case "@winnderdc1":
                                    {
                                        Features.CityWarDc.LastWinner.GuildName = GC.MyChar.MyGuild.GuildName;
                                        GC.LocalMessage(2000, "Last Guild War winner set to YOUR Guild!");
                                        break;
                                    }
                                #endregion

                                #region /winnerbi1
                                case "@winnerbi1":
                                    {
                                        Features.CityWarBi.LastWinner.GuildName = GC.MyChar.MyGuild.GuildName;
                                        GC.LocalMessage(2000, "Last Guild War winner set to YOUR Guild!");
                                        break;
                                    }
                                #endregion

                                #region /poletc
                                case "@poletc":
                                    if (!Features.PoleWarTC.War)
                                        Features.PoleWarTC.StartWar();
                                    else
                                        Features.PoleWarTC.EndWarForGood();
                                    break;
                                #endregion
                                #region /polePc
                                case "@polepc":
                                    if (!Features.PoleWarPC.War)
                                        Features.PoleWarPC.StartWar();
                                    else
                                        Features.PoleWarPC.EndWarForGood();
                                    break;
                                #endregion
                                #region /poleac
                                case "@poleac":
                                    if (!Features.PoleWarAC.War)
                                        Features.PoleWarAC.StartWar();
                                    else
                                        Features.PoleWarAC.EndWarForGood();
                                    break;
                                #endregion
                                #region /poledc
                                case "@poledc":
                                    if (!Features.PoleWarDC.War)
                                        Features.PoleWarDC.StartWar();
                                    else
                                        Features.PoleWarDC.EndWarForGood();
                                    break;
                                #endregion
                                #region /polebi
                                case "@polebi":
                                    if (!Features.PoleWarBI.War)
                                        Features.PoleWarBI.StartWar();
                                    else
                                        Features.PoleWarBI.EndWarForGood();
                                    break;
                                    #endregion
                            }


                            #region Packets//Disabled
                            //case "@packet":
                            //    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 116, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 3276));
                            //    break;
                            #endregion
                            #region Packets//Disabled
                            //case "@packets":
                            //    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, Convert.ToUInt16(Cmd[1]), GC.MyChar.Loc.X, GC.MyChar.Loc.Y, Convert.ToUInt16(Cmd[2])));
                            //    break;
                            #endregion
                            #region Change Interserver//Disabled
                            //case "@changeserver")
                            //{
                            //    if (World.Interserver == 0)
                            //    {
                            //        World.Interserver = 1;
                            //        GC.LocalMessage(2000, "Changed to US Server!");
                            //    }
                            //    else
                            //    {
                            //        World.Interserver = 0;
                            //        GC.LocalMessage(2000, "Changed to EU Server!");
                            //    }
                            //}
                            #endregion
                            #region /record//Disabled
                            //case "@record":
                            //    C = Game.World.CharacterFromName(Cmd[1]);
                            //    if (C != null)
                            //    {
                            //        if (C.RecordAction)
                            //        {
                            //            C.RecordAction = false;
                            //            Program.WriteActions(Game.World.Actions);
                            //            Game.World.Actions = "";
                            //        }
                            //        else C.RecordAction = true;
                            //        GC.LocalMessage(2000, C.Name + " actions recording : " + C.RecordAction);
                            //    }
                            //    else
                            //    {
                            //        Program.WriteActions(Game.World.Actions);
                            //        Game.World.Actions = "";
                            //    }
                            //    break;
                            #endregion
                            #region /screenshotnpcs//Disabled
                            //case "@scaddnpc":
                            //{//uid type flags avatar
                            //    for (int a = 5; a < 20000; a++)
                            //    {
                            //        try
                            //        {
                            //            Game.NPC N = new Game.NPC("2053" + ' ' + a + /*GC.MyChar.Direction +*/ ' ' + "2" + ' ' + "7" + ' ' + GC.MyChar.Loc.Map + ' ' + ((ushort)(GC.MyChar.Loc.X + 2)) + ' ' + GC.MyChar.Loc.Y);
                            //            //Game.World.H_NPCs.Add(N.EntityID, N);
                            //            Game.World.Spawn(N);

                            //            System.Threading.Thread.Sleep(500);
                            //            var image = ScreenCapture.CaptureActiveWindow();
                            //            image.Save(@"C:\Users\Proprietário\Desktop\NPC\" + a + ".jpg", ImageFormat.Jpeg);
                            //            System.Threading.Thread.Sleep(1000);
                            //            a += 5;
                            //        }
                            //        catch (Exception e)
                            //        {
                            //            Console.WriteLine("NPC " + a + " doesn't exist '{0}'", e);
                            //            a += 5;
                            //        }
                            //    }
                            //    break;
                            //}
                            #endregion
                            #region /chesttime//Disabled
                            //case "@chesttime":
                            //    if (Features.GuildWars.GuildChests > 0)
                            //    {
                            //        for (int i = 0; i < Features.GuildWars.GuildChests; i++)
                            //            GC.LocalMessage(2000, Features.GuildWars.ChestTime[i].ToString());
                            //    }
                            //    else GC.LocalMessage(2000, "No GuildChests!");
                            //    break;
                            #endregion
                            #region /spawnchest//Disabled
                            //case "@spawnchest":
                            //    Features.GuildWars.GuildChests = 1;
                            //    Features.GuildWars.ChestTime[0] = DateTime.Now;
                            //    break;
                            #endregion
                            #region MapEffect//Disabled
                            //case "@meffect":
                            //Game.World.Action(GC.MyChar, (Packets.MapEffect(GC.MyChar, Convert.ToUInt32(Cmd[1]))).Get);
                            //break;
                            #endregion
                        }
                        #endregion
                    }
                    else
                        Game.World.Chat(GC.MyChar, ChatType, From, To, Message);
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); }
        }
    }
}
