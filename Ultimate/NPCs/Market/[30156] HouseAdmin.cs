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
    public class NPC_30156 : NPCBase
    {
        public NPC_30156(Main.GameClient _client)
            : base(_client)
        {
            ID = 30156;
            Face = 92;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        uint HouseId = DMaps.GetHouseID(GC.MyChar.EntityID);
                        AddText("What can I do for you?");
                        if (HouseId == 0)
                            AddOption("Buy a house.", 1);
                        else
                            AddOption("Enter my house", 2);

                        if (GC.MyChar.Spouse != "None" && GC.MyChar.Spouse != "")
                        {
                            if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                AddOption("Visit my wife`s house", 3);
                            else
                                AddOption("Visit my husband`s house", 3);
                        }
                        if (DMaps.GetHouseID(GC.MyChar.EntityID) > 0)
                            AddOption("Upgrade my house", 4);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721170, 1))
                        {
                            if (GC.MyChar.Silvers < 1000000)
                            {
                                AddText("To buy a house you need 1,000,000 silvers in addition to the HousePermit.");
                                AddOption("I will return with the money.", 255);
                            }
                            else
                            {
                                AddText("Are you sure you want to buy a house. You will be charged 1,000,000 silvers.");
                                AddOption("Yes, I do.", 5);
                                AddOption("Let me think it over.", 255);
                            }
                        }
                        else
                        {
                            AddText("Once you have a House Permit, you may come to apply for a house.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        uint HouseId = DMaps.GetHouseID(GC.MyChar.EntityID);
                        if (HouseId > 0)
                        {
                            ushort House_x = 33;
                            ushort House_y = 40;
                            switch (DMaps.HouseLevel(HouseId))
                            {
                                case 2:
                                    House_x = 51;
                                    House_y = 80;
                                    break;
                                case 3:
                                case 4:
                                    House_x = 80;
                                    House_y = 58;
                                    break;
                                case 5:
                                    House_x = 107;
                                    House_y = 140;
                                    break;
                            }
                            if (GC.MyChar.MyTeam != null)
                            {
                                if (GC.MyChar.MyTeam.Leader.EntityID == GC.MyChar.EntityID)
                                    foreach (Game.Character Member in GC.MyChar.MyTeam.Members)
                                    {
                                        //if (Member != null && Member.MyClient != null)
                                        if ((Member.EntityID != GC.MyChar.EntityID) && Member.Loc.Map == GC.MyChar.Loc.Map)
                                            if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, Member.Loc.X, Member.Loc.Y, 14))
                                                Member.Teleport(HouseId, House_x, House_y);
                                    }
                            }
                            GC.MyChar.Teleport(HouseId, House_x, House_y);
                        }
                        break;
                    }
                case 3:
                    {
                        //GC.LocalMessage(2011, "Houses were disabled until we fix them sorry for the inconvenience");
                        string SpouseAccount = "";
                        string SpouseName = GC.MyChar.Spouse;
                        if (SpouseName.Contains("[PM]"))
                            SpouseName = SpouseName.Replace("[PM]", "");
                        if (SpouseName.Contains("[GM]"))
                            SpouseName = SpouseName.Replace("[GM]", "");
                        Game.Character Spouse = Database.LoadCharacter(SpouseName, ref SpouseAccount);
                        uint HouseId = DMaps.GetHouseID(Spouse.EntityID);
                        if (HouseId > 0)
                        {
                            ushort House_x = 33;
                            ushort House_y = 40;
                            switch (DMaps.HouseLevel(HouseId))
                            {
                                case 2:
                                    House_x = 51;
                                    House_y = 80;
                                    break;
                                case 3:
                                case 4:
                                    House_x = 80;
                                    House_y = 58;
                                    break;
                                case 5:
                                    House_x = 107;
                                    House_y = 140;
                                    break;
                            }
                            GC.MyChar.Teleport(HouseId, House_x, House_y);
                        }
                        else
                        {
                            AddText("Your spouse does not own a house.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        uint HouseId = DMaps.GetHouseID(GC.MyChar.EntityID);
                        if (DMaps.HouseLevel(HouseId) == 2)
                        {
                            AddText("It seems like you already have a second-level house! There's no need to upgrade it any further!");
                            AddOption("I see.", 255);
                        }
                        else
                        {
                            AddText("To upgrade your house to second-class, you should have an Upgrade Certificate and 5,000,000 silver.");
                            AddOption("All are ready.", 6);
                            AddOption("I shall come later.", 255);
                        }
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.Silvers < 1000000)
                        {
                            AddText("To buy a house you need 1,000,000 silvers in addition to the HousePermit.");
                            AddOption("I will return with the money.", 255);
                        }
                        else
                        {
                            if (GC.MyChar.InventoryContains(721170, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721170));
                                GC.MyChar.Silvers -= 1000000;
                                bool Success = DMaps.CreateDynamicMap(/*10000,*/ 1098, GC.MyChar.EntityID, false);
                                Features.HouseTable.AddFurniture(GC.MyChar.EntityID, "2100", "8206", "2", "20", "40", true);
                                AddText("Congratulations! You have a home. Your spouse can visit at anytime. Others can visit your house if they are in your team.");
                                AddOption("Thank you.", 255);
                            }
                            else
                            {
                                AddText("Once you have a House Permit, you may come to apply for a house.");
                                AddOption("I see.", 255);
                            }
                        }
                        break;
                    }
                case 6:
                    {
                        if (DMaps.HouseLevel(DMaps.GetHouseID(GC.MyChar.EntityID)) == 1)
                        {
                            if (GC.MyChar.Silvers < 5000000)
                            {
                                AddText("Sorry, you do not have enough money. You need 5,000,000 silvers to upgrade your house.");
                                AddOption("I see", 255);
                            }
                            else
                            {
                                if (GC.MyChar.InventoryContains(721174, 1))
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(721174));
                                    GC.MyChar.Silvers -= 5000000;
                                    // uint HId = DMaps.GetHouseID(GC.MyChar.EntityID);
                                    bool Success = DMaps.DeleteDynamicMap(/*HId,*/ GC.MyChar.EntityID, false);
                                    Success = DMaps.CreateDynamicMap(1099, GC.MyChar.EntityID, false);
                                    Features.HouseTable.AddFurniture(GC.MyChar.EntityID, "2101", "8206", "2", "32", "38", true);
                                    Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                                    AddText("Congratulations! Your house is now upgraded to level 2. Your house is ready.");
                                    AddOption("Thank you.", 255);
                                }
                                else
                                {
                                    AddText("Sorry, you do not have an Upgrade Certificate. The House Agent is isuing the upgrade Certificates in Twin City..");
                                    AddOption("I see.", 255);
                                }
                            }
                        }
                        //else if (DMaps.HouseLevel(DMaps.GetHouseID(GC.MyChar.EntityID)) == 2)
                        //{
                        //    if (GC.MyChar.Silvers < 10000000)
                        //    {
                        //        AddText("Sorry, you do not have enough money. You need 10,000,000 silvers to upgrade your house.");
                        //        AddOption("I see", 255);
                        //    }
                        //    else
                        //    {
                        //        if (GC.MyChar.InventoryContains(721174, 1))
                        //        {
                        //            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721174));
                        //            GC.MyChar.Silvers -= 10000000;
                        //            bool Success = DMaps.DeleteDynamicMap(GC.MyChar.EntityID, false);
                        //            Success = DMaps.CreateDynamicMap(2080, GC.MyChar.EntityID, false);
                        //            Features.HouseTable.AddFurniture(GC.MyChar.EntityID, "2101", "8206", "2", "32", "38", true);
                        //            Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                        //            AddText("Congratulations! Your house is now upgraded to level 2. Your house is ready.");
                        //            AddOption("Thank you.", 255);
                        //        }
                        //        else
                        //        {
                        //            AddText("Sorry, you do not have an Upgrade Certificate. The House Agent is isuing the upgrade Certificates in Twin City..");
                        //            AddOption("I see.", 255);
                        //        }
                        //    }
                        //}
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}