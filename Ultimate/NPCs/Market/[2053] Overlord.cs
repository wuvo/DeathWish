using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_2053 : NPCBase
    {
        public NPC_2053(Main.GameClient _client)
            : base(_client)
        {
            ID = 2053;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Howdy! I was sent to this earth to promote companionship within the server! One can pay a tribute in order to achieve a common goal that will reward the whole server!");
                        AddOption("I'd like contribute", 1);
                        AddOption("Just passing by.", 255);
                        GC.ER = false;
                        GC.DR = false;
                        break;
                    }
                case 1:
                    {
                        AddText("Alright! I'm glad to know that you're willing to help! What would you like to contribute to?");
                        AddOption("2x EXP (" + World.ERPts + " Pts left)", 2);
                        AddOption("25% Drop Rates (" + World.DRPts + " Pts left)", 4);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 2:
                case 4:
                    {
                        if (_linkback == 2)
                        {
                            AddText("Awesome! There are " + World.ERPts + " points left to activate 1 hour of 2x EXP Rate! What would you like to contribute with?");
                            GC.ER = true;
                        }
                        else if (_linkback == 4)
                        {
                            AddText("Awesome! There are " + World.DRPts + " points left to activate 1 hour of 25% higher drop rates! What would you like to contribute with?");
                            GC.DR = true;
                        }
                        AddOption("MeteorScroll (10 Points)", 10);
                        AddOption("DragonBall (10 Points)", 20);
                        AddOption("MetscrollBag (100 Points)", 100);
                        AddOption("DBScroll (100 Points)", 200);
                        AddOption("Super Gem (100 Points)", 14);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 14:
                    {
                        AddText("Each Super Gem equals 100 Points! Which Super Gem would you like to contribute?");
                        AddOption("Dragon Gem", 13);
                        AddOption("Phoenix Gem", 3);
                        AddOption("Violet Gem", 53);
                        AddOption("Rainbow Gem", 33);
                        AddOption("Moon Gem", 63);
                        AddOption("Kylin Gem", 43);
                        AddOption("Fury Gem", 23);
                        AddOption("I changed my mind", 255);
                        break;
                    }
                case 10:
                case 20:
                case 100:
                case 200:
                case 13:
                case 3:
                case 53:
                case 33:
                case 63:
                case 43:
                case 23:
                    {
                        uint ID = 0;
                        if (_linkback == 10)
                            ID = 720027;
                        else if (_linkback == 20)
                            ID = 1088000;
                        else if (_linkback == 100)
                            ID = 729912;
                        else if (_linkback == 200)
                            ID = 720028;
                        else if (_linkback % 10 == 3)
                            ID = (700000 + Convert.ToUInt32(_linkback));
                        Item IID = new Item();
                        IID.ID = ID;
                        if (GC.MyChar.InventoryContains(ID, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(ID));
                            if (GC.ER)
                            {
                                if (_linkback % 10 == 3)
                                    _linkback = 100;
                                else if (_linkback == 20)
                                    _linkback = 10;
                                else if (_linkback == 200)
                                    _linkback = 100;
                                if (World.ERPts > _linkback)
                                    World.ERPts -= _linkback;
                                else
                                {
                                    World.ERPts = 1000;
                                    World.EREvent = DateTime.Now.AddMinutes(60);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has made the final donation and activated global 2x EXP Rate for 1 hour! Enjoy!", 2011, 0);
                                }
                            }
                            else if (GC.DR)
                            {
                                if (_linkback % 10 == 3)
                                    _linkback = 100;
                                else if (_linkback == 20)
                                    _linkback = 10;
                                else if (_linkback == 200)
                                    _linkback = 100;
                                if (World.DRPts > _linkback)
                                    World.DRPts -= _linkback;
                                else
                                {
                                    World.DRPts = 1000;
                                    World.DREvent = DateTime.Now.AddMinutes(60);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has made the final donation and activated global 25% higher drop rates for 1 hour! Enjoy!", 2011, 0);
                                }
                            }
                            AddText("Congratulations ! You've contributed to the global events with a " + IID.DBInfo.Name + " and donated " + _linkback + " Points!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("It seems that you don't have a " + IID.DBInfo.Name + "! Please come back when you have it!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}