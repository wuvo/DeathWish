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
    public class NPC_300001 : NPCBase
    {
        public NPC_300001(Main.GameClient _client)
            : base(_client)
        {
            ID = 300001;
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
                        AddText("I can help you to divorce your spouse. You just will have to agree to a divorce. I will also need a MeteorTear. If you have this item and you really want to get divorced, we will do it soon.");
                        AddOption("No, I am loved", 255);
                        AddOption("Yes, I want a divorce", 1);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Spouse != "None")
                        {
                            AddText("You really sure you want to divorce your spouse now? You will not regret later?");
                            AddOption("No I'll not. Let's do this.", 2);
                            AddOption("I prefer to remain married.", 255);
                        }
                        else
                        {
                            AddText("Sorry, I can not divorce you if you are not married.");
                            AddOption("Okay", 255);
                        }
                        
                        break;
                    }
                case 2:
                    AddText("Are you ready? Be sure you're with MeteorTear, I really need it.");
                    AddOption("Yeah. I am ready.", 3);
                    AddOption("Let me think it over.", 255);
                    break;
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(1088002, 1) && GC.MyChar.InventoryContains(1088001, 1))
                        {
                            Game.Character Love = Game.World.CharacterFromName2(GC.MyChar.Spouse);
                            if (Love != null)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088002));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));
                                if (Love.Inventory.Count <= 39)
                                {
                                    Love.AddItem(1088002);
                                    Love.MyClient.LocalMessage(2005, "You have gain a MeteorTear from your unloved.");
                                }
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Spouse + " and " + Love.Spouse + " are divorced now.", 2005, 0);
                                Love.Spouse = "None";
                                GC.MyChar.Spouse = "None";
                                Love.MyClient.AddSend(Packets.StringPacket(Love.EntityID, StringType.Spouse, Love.Spouse));
                                GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, StringType.Spouse, GC.MyChar.Spouse));
                                Database.SaveCharacter(Love, Love.MyClient.AuthInfo.Account);
                                Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                            }
                            else
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088002));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Spouse + " and " + GC.MyChar.Name + " are divorced now.", 2005, 0);
                                string SpouseName = GC.MyChar.Spouse;
                                if (GC.MyChar.Spouse.Contains("[PM]"))
                                    SpouseName = GC.MyChar.Spouse.Replace("[PM]", "");
                                if (GC.MyChar.Spouse.Contains("[GM]"))
                                    SpouseName = GC.MyChar.Spouse.Replace("[GM]", "");
                                string TempAccount = "";
                                Character TempChar = Database.LoadCharacter(SpouseName, ref TempAccount);
                                if (TempChar != null)
                                {
                                    TempChar.Spouse = "None";
                                }
                                GC.MyChar.Spouse = "None";
                                GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, StringType.Spouse, GC.MyChar.Spouse));

                                if (TempChar.Inventory.Count < 40)
                                    TempChar.AddItem(1088002);
                                Database.SaveCharacter(TempChar, TempAccount);
                                Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                            }
                        }
                        else
                        {
                            AddText("You do not have the required items. (Meteor & Meteor Tear)");
                            AddOption("Sorry.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}