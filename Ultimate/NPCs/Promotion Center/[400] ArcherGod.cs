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
    public class NPC_400 : NPCBase
    {
        public NPC_400(Main.GameClient _client)
            : base(_client)
        {
            ID = 400;
            Face = 10;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            if (_linkback == 0)
            {
                if (GC.MyChar.Job >= 40 && GC.MyChar.Job <= 45)
                {
                    AddText("Mercilessly fight your way to the top, my friend, because Archers are destined for greatness! Your swift mind will give you the ability to attack from afar, at a pace unmatched by any of your peers. So, what can I do for you?");
                    AddOption("Promote me.", 1);
                    AddOption("Learn Skills.", 2);
                    AddOption("Just passing by.", 255);
                }
                else
                {
                    AddText("Archers do not share their secrets of battle with others. I shall not teach you.");
                    AddOption("I see", 255);
                }
            }
            #region Promote
            else if (GC.MyChar.Job >= 40 && GC.MyChar.Job <= 45)
            {
                if (GC.MyChar.Job <= 44 && (_linkback == 1 || _linkback == 10))
                {
                    if (_linkback == 1)
                    {
                        AddText("You need to be level " + GC.MyChar.LevReqForPromote + " to promote to " + ((Game.Character.JobName)(GC.MyChar.Job + 1)).ToString());
                        if (GC.MyChar.Job == 41)
                            AddText("Also you will need 5 Euxenite Ores.");
                        if (GC.MyChar.Job == 42)
                            AddText("Also you will need an Emerald.");
                        else if (GC.MyChar.Job == 43)
                            AddText("Also you will need a Meteor.");
                        else if (GC.MyChar.Job == 44)
                            AddText("Also you will need a MoonBox.");
                        AddOption("Promote me.", 10);
                        AddOption("Forget it.", 255);
                    }
                    else if (_linkback == 10)
                    {
                        if (GC.MyChar.Level >= GC.MyChar.LevReqForPromote)
                        {
                            if (GC.MyChar.Inventory.Count < 35 || GC.MyChar.Reborn)
                            {
                                byte Need = 1;
                                uint ID = GC.MyChar.PromoteItems;
                                if (ID == 1072031) Need = 5;
                                if (GC.MyChar.InventoryContains(ID, Need) || ID == 1)
                                {
                                    for (byte i = 0; i < Need; i++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(ID));
                                    GC.MyChar.Job++;
                                    if (GC.MyChar.Job == 41 && !GC.MyChar.Reborn)
                                    {
                                        if (!World.LowRatedServer) //changed
                                        {
                                            GC.MyChar.AddItem(113007);
                                            GC.MyChar.AddItem(120027);
                                            GC.MyChar.AddItem(150037);
                                            GC.MyChar.AddItem(133007);
                                            GC.MyChar.AddItem(160037);
                                            GC.MyChar.AddItem(500017);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(113008);
                                            GC.MyChar.AddItem(120028);
                                            GC.MyChar.AddItem(150038);
                                            GC.MyChar.AddItem(133008);
                                            GC.MyChar.AddItem(160038);
                                            GC.MyChar.AddItem(500018);
                                        }
                                    }
                                    else if (GC.MyChar.Job == 42 && !GC.MyChar.Reborn)
                                    {
                                        if (World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(113037);
                                            GC.MyChar.AddItem(120087);
                                            GC.MyChar.AddItem(150077);
                                            GC.MyChar.AddItem(160077);
                                            GC.MyChar.AddItem(133027);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(113038);
                                            GC.MyChar.AddItem(120088);
                                            GC.MyChar.AddItem(150078);
                                            GC.MyChar.AddItem(160078);
                                            GC.MyChar.AddItem(133028);
                                        }
                                        Game.Item I = new Ultimate.Game.Item();
                                        I.ID = 500077;
                                        I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        I.Soc1 = Ultimate.Game.Item.Gem.EmptySocket;
                                        GC.MyChar.AddItem(I);
                                    }
                                    else if (GC.MyChar.Job == 43 && !GC.MyChar.Reborn)
                                    {
                                        if (World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(113047);
                                            GC.MyChar.AddItem(120127);
                                            GC.MyChar.AddItem(150137);
                                            GC.MyChar.AddItem(133047);
                                            GC.MyChar.AddItem(160137);
                                            GC.MyChar.AddItem(500127);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(113048);
                                            GC.MyChar.AddItem(120128);
                                            GC.MyChar.AddItem(150138);
                                            GC.MyChar.AddItem(133048);
                                            GC.MyChar.AddItem(160138);
                                            GC.MyChar.AddItem(500128);
                                        }
                                    }
                                    else if (GC.MyChar.Job == 44 && (GC.MyChar.Reborns == 0 || GC.MyChar.Reborns == 2))
                                        GC.MyChar.AddItem(700031);
                                    else if (GC.MyChar.Job == 44 && GC.MyChar.Reborns == 1)
                                    {
                                        Game.Item I = new Ultimate.Game.Item();
                                        I.ID = 133077;
                                        I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        I.Soc1 = Game.Item.Gem.EmptySocket;
                                        GC.MyChar.AddItem(I);
                                    }
                                    else if (GC.MyChar.Job == 45)
                                        GC.MyChar.AddItem(1088000);

                                    AddText("Congratulations! You are now " + ((Game.Character.JobName)GC.MyChar.Job).ToString() + "! Check your inventory for the reward!");
                                    AddOption("Thanks.", 255);
                                }
                                else
                                {
                                    AddText("You don't have the required materials.");
                                    AddOption("I'll go get em.", 255);
                                }
                            }
                            else
                            {
                                AddText("Make sure you have at least 6 free inventory slots!");
                                AddOption("I'll get some space", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not qualified yet. Your level is too low.");
                            AddOption("I see.", 255);
                        }
                    }
                }
                else if (_linkback == 1 || _linkback == 10)
                {
                    AddText("You are already an ArcherMaster, I cannot promote you anymore.");
                    AddOption("I see.", 255);
                }
                #endregion
                else if (_linkback == 2)
                {
                    AddText("Choose from the skills listed below.");
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)4];
                    if (Skills.Count < 8)
                    {
                        for (byte i = 0; i < Skills.Count; i++)
                            AddOption(((Extra.SkillIDs)(((SkillLearn)Skills[i]).ID)).ToString() + "(Lv " + ((SkillLearn)Skills[i]).LevelReq.ToString() + " )", (byte)(20 + i));
                    }
                    else
                    {
                        for (byte i = 0; i < 7; i++)
                            AddOption(((Extra.SkillIDs)(((SkillLearn)Skills[i]).ID)).ToString() + "(Lv " + ((SkillLearn)Skills[i]).LevelReq.ToString() + " )", (byte)(20 + i));
                        AddOption("Next", 100);
                    }
                }
                else if (_linkback == 100)
                {
                    AddText("Choose from the skills listed below.");
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)4];
                    for (byte i = 7; i < Skills.Count; i++)
                        AddOption(((Extra.SkillIDs)(((SkillLearn)Skills[i]).ID)).ToString() + "(Lv " + ((SkillLearn)Skills[i]).LevelReq.ToString() + " )", (byte)(20 + i));
                    AddOption("Previous", 2);
                }
                else if (_linkback >= 20 && _linkback <= 34)
                {
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)4];
                    SkillLearn S = (SkillLearn)Skills[(byte)(_linkback - 20)];
                    if (GC.MyChar.Level >= S.LevelReq)
                        GC.MyChar.NewSkill(S.ToSkill());
                    else
                    {
                        AddText("You are not high level enough.");
                        AddOption("I see.", 255);
                    }
                }
            }

            AddFinish();
            Send();
        }
    }
}