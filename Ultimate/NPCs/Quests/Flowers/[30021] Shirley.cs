using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;
using System.Collections;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [3002] Shirley
    /// </summary>
    public class NPC_30021 : NPCBase
    {
        public NPC_30021(Main.GameClient _client)
            : base(_client)
        {
            ID = 30021;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Random Rnd = new Random();
            //switch (Rnd.Next(0, 3))
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Hello there dreamer! We all know that love is a mysterious thing that's why you can show how much you care about love by sending flowers to all the girls in EternalConquer. You will receive points for every flower you get.");
                    AddText("You currently have : " + GC.MyChar.Flowers + " flower points!");
                    AddOption("II want to gain flower points.", 1);
                    AddOption("I want to spend the flower points.", 2);
                    AddOption("How can I get flowers?", 3);
                    AddOption("Just passing by", 255);
                    break;
                #region Gain ~ Flowers (1)
                case 1:
                    {
                        ArrayList Remove = new ArrayList();
                        uint AddFlowers = 0;
                        foreach (Item I in GC.MyChar.Inventory)
                        {
                            if (I.ID == 751001 || I.ID == 752001 || I.ID == 753001 || I.ID == 754001)
                            {
                                Remove.Add(I);
                                AddFlowers += 1;
                            }
                            else if (I.ID == 752003 || I.ID == 751003 || I.ID == 753003 || I.ID == 754003)
                            {
                                Remove.Add(I);
                                AddFlowers += 3;
                            }
                            else if (I.ID == 751009 || I.ID == 752009 || I.ID == 753009 || I.ID == 754009)
                            {
                                Remove.Add(I);
                                AddFlowers += 9;
                            }
                            else if (I.ID == 752099 || I.ID == 751099 || I.ID == 753099 || I.ID == 754099)
                            {
                                Remove.Add(I);
                                AddFlowers += 99;
                            }
                            else if (I.ID == 751999 || I.ID == 752999 || I.ID == 753999 || I.ID == 754999)
                            {
                                Remove.Add(I);
                                AddFlowers += 999;
                            }
                        }
                        foreach (Item I in Remove)
                        {
                            GC.MyChar.RemoveItem(I.UID);
                        }
                        GC.MyChar.Flowers += AddFlowers;
                        AddText("You received " + AddFlowers + " flower points!");
                        AddOption("Just passing by", 255);
                        break;
                    }

                #endregion
                #region How~To~Spend(3)
                case 3:
                    {
                        AddText("You can find flowers by killing monsters in TwinCity map! Taoist, Warriors and Trojans have more chances to find flowers than archers. Hurry!");
                        AddOption("Thanks", 255);
                        break;
                    }
                #endregion
                #region Prizes~Dialog (2)
                case 2:
                    {
                        AddText("Love is in the air?");
                        AddText("  You currently have : " + GC.MyChar.Flowers + " flower points!");
                        AddOption("Euxenite Ores (5 Points).", 4);
                        AddOption("Emerald (150 Points).", 5);
                        AddOption("Moonbox (5000 Points).", 6);
                        AddOption("CleanWater (2000 Points).", 7);
                        AddOption("MeteorScroll (1150 Points).", 8);
                        AddOption("Next page", 12);
                        break;
                    }
                case 12:
                    {
                        AddText("Love is in the air?");
                        AddText("  You currently have : " + GC.MyChar.Flowers + " flower points!");
                        AddOption("DBScroll (2150 Points)", 9);
                        AddOption("Random Refined GEM (1000 Points)", 10);
                        AddOption("QuestChanceA (10000 Points).", 11);
                        AddOption("I was just passing by...", 255);
                        break;
                    }
                #endregion
                default:

                    {
                        if (_linkback == 255) break;
                        int id = 0, points = 0;
                        switch (_linkback)
                        {

                            case 4: id = 1072031; points = 5; break;//Eux ore
                            case 5: id = 1080001; points = 150; break;//emerald
                            case 6: id = 721080; points = 5000; break;//Moonbox
                            case 7: id = 721258; points = 2000; break;//Cleanwater
                            case 8: id = 720027; points = 1150; break;//Metscroll
                            case 9: id = 720028; points = 2150; break;//dbscroll
                            case 10://Random Ref Gem
                                {
                                    List<uint> From = new List<uint>() { 700002, 700012, 700032, 700042, 700052, 700062, 700072 };
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
                                    id = (int)From[Tries];
                                    points = 1000;
                                    break;
                                }
                           case 11: id = 721774; points = 10000; break;// QuestChanceA
                        }
                        if (id != 0 && points != 0)
                        {
                            if (GC.MyChar.Flowers < points)
                            {
                                AddText("You need " + points + " flower point");
                                AddOption("Just passing by", 255);
                                return;
                            }
                            GC.MyChar.Flowers -= (uint)points;
                            GC.MyChar.AddItem((uint)id, 0);
                        }
                        break;
                    }
            }
            AddFinish();
            Send();
        }
    }
    
}
