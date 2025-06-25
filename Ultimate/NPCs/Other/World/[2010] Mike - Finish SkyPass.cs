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
    public class NPC_2010 : NPCBase
    {
        public NPC_2010(Main.GameClient _client)
            : base(_client)
        {
            ID = 2010;
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
                        AddText("Ah, you must be the hero who broke through the 5 tough floors in one time! You deserve to be rewarded ");
                        AddText("for your great perseverance. Hope to see you again.");
                        AddOption("Claim my reward", 1);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721109, 1))
                        {
                            AddText("There are two Treasure Boxes here, 5 DBs in the first box and the second box has the chance of giving 7 Meteors, 1 DragonBall or some money! Which would you like to receive?");
                            AddOption("Treasure Box 1", 2);
                            AddOption("Treasure Box 2", 3);
                        }
                        else
                        {
                            AddText("I'm sorry but I shall only reward those who have a SkyPrizeToken!");
                            AddOption("Take me to Twin City", 4);
                            AddOption("Let me think it over", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.Inventory.Count < 36)
                        {
                            for (int a = 0; a < 5; a++)
                                GC.MyChar.AddItem(1088000);
                            AddText("Here you go, enjoy!");
                            AddOption("Thanks", 255);
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721109));
                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received a 5 Dragonball in return!", 2011, 0);
                            GC.MyChar.Teleport(1002, 430, 378);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.Inventory.Count < 36)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721109));
                            if (MyMath.ChanceSuccess(30))
                            {
                                GC.MyChar.Silvers += 2500000;
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received a 2,500,00 in return!", 2011, 0);
                                GC.MyChar.Teleport(1002, 430, 378);
                            }
                            else if (MyMath.ChanceSuccess(15))
                            {
                                for (int a = 0; a < 5; a++)
                                    GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received a 5 Dragonball in return!", 2011, 0);
                                GC.MyChar.Teleport(1002, 430, 378);
                            }
                            else if (MyMath.ChanceSuccess(30))
                            {
                                GC.MyChar.AddItem(721080);
                                AddText("Congratulations! You have received a MoonBox!");
                                GC.MyChar.Teleport(1002, 430, 378);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received a MoonBox in return!", 2011, 0);
                            }
                            else if (MyMath.ChanceSuccess(30))
                            {
                                GC.MyChar.Silvers += 3000000;
                                AddText("Congratulations! You have received 3,000,000 silvers!");
                                GC.MyChar.Teleport(1002, 430, 378);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received 3,000,000 silvers in return!", 2011, 0);
                            }
                            else if (MyMath.ChanceSuccess(15))
                            {
                                GC.MyChar.AddItem(721258);
                                AddText("Congratulations! You have received a CleanWater!");
                                GC.MyChar.Teleport(1002, 430, 378);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received a CleanWater in return!", 2011, 0);
                            }
                            else
                            {
                                GC.MyChar.Silvers += 5000000;
                                AddText("Congratulations! You have received 5,000,000 silvers!");
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has completed the Sky Pass Quest and received a 5,000,000 in return!", 2011, 0);
                                GC.MyChar.Teleport(1002, 430, 378);
                            }
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    GC.MyChar.Teleport(1002, 430, 378);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}