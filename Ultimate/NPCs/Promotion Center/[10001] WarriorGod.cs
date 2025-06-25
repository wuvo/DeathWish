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
    public class NPC_10001 : NPCBase
    {
        public NPC_10001(Main.GameClient _client)
            : base(_client)
        {
            ID = 10001;
            Face = 8;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            if (_linkback == 0)
            {
                if (GC.MyChar.Job >= 20 && GC.MyChar.Job <= 25)
                {
                    AddText("To be a good Warrior, you must know your weapon, your armor be a 2nd skin, and use your shield to ward off your enemy! Remember, fortune favors the brave, and you must keep your head, even in the darkest hours. So, what can I do for you?");
                    AddOption("Promote me.", 1);
                    AddOption("Learn Skills.", 2);
                    AddOption("Just passing by.", 255);
                }
                else
                {
                    AddText("Warriors do not share their secrets of battle with others. I shall not teach you.");
                    AddOption("I see", 255);
                }
            }
            #region Promote
            else if (GC.MyChar.Job >= 20 && GC.MyChar.Job <= 25)
            {
                if (GC.MyChar.Job <= 24 && (_linkback == 1 || _linkback == 10))
                {
                    if (_linkback == 1)
                    {
                        AddText("You need to be level " + GC.MyChar.LevReqForPromote + " to promote to " + ((Game.Character.JobName)(GC.MyChar.Job + 1)).ToString());
                        if (GC.MyChar.Job == 22)
                            AddText("Also you will need an Emerald.");
                        else if (GC.MyChar.Job == 23)
                            AddText("Also you will need a Meteor.");
                        else if (GC.MyChar.Job == 24)
                            AddText("Also you will need a MoonBox.");
                        AddOption("Promote me.", 10);
                        AddOption("Forget it.", 255);
                    }
                    else if (_linkback == 10)
                    {
                        if (GC.MyChar.Level >= GC.MyChar.LevReqForPromote)
                        {
                            if (GC.MyChar.Inventory.Count < 32 || GC.MyChar.Reborn)
                            {
                                byte Need = 1;
                                uint ID = GC.MyChar.PromoteItems;
                                if (ID == 1072031) Need = 1;
                                if (GC.MyChar.InventoryContains(ID, Need) || ID == 1)
                                {
                                    for (byte i = 0; i < Need; i++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(ID));
                                    GC.MyChar.Job++;
                                    if (GC.MyChar.Job == 21 && !GC.MyChar.Reborn)
                                    {
                                        if (!World.LowRatedServer) // changed
                                        {
                                            GC.MyChar.AddItem(111007);
                                            GC.MyChar.AddItem(120027);
                                            GC.MyChar.AddItem(150037);
                                            GC.MyChar.AddItem(131007);
                                            GC.MyChar.AddItem(160037);
                                            GC.MyChar.AddItem(480027);
                                            GC.MyChar.AddItem(420027);
                                            GC.MyChar.AddItem(410027);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(111008);
                                            GC.MyChar.AddItem(120028);
                                            GC.MyChar.AddItem(150038);
                                            GC.MyChar.AddItem(131008);
                                            GC.MyChar.AddItem(160038);
                                            GC.MyChar.AddItem(480028);
                                            GC.MyChar.AddItem(420028);
                                            GC.MyChar.AddItem(410028);
                                        }

                                    }
                                    else if (GC.MyChar.Job == 22 && !GC.MyChar.Reborn)
                                    {
                                        if (World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(111047);
                                            GC.MyChar.AddItem(900007);
                                            GC.MyChar.AddItem(120087);
                                            GC.MyChar.AddItem(150077);
                                            GC.MyChar.AddItem(131037);
                                            GC.MyChar.AddItem(160077);
                                            GC.MyChar.AddItem(410077);
                                            GC.MyChar.AddItem(420077);
                                            GC.MyChar.AddItem(480077);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(111048);
                                            GC.MyChar.AddItem(900008);
                                            GC.MyChar.AddItem(120088);
                                            GC.MyChar.AddItem(150078);
                                            GC.MyChar.AddItem(131038);
                                            GC.MyChar.AddItem(160078);
                                            GC.MyChar.AddItem(410078);
                                            GC.MyChar.AddItem(420078);
                                            GC.MyChar.AddItem(480078);
                                        }
                                    }
                                    else if (GC.MyChar.Job == 23 && !GC.MyChar.Reborn)
                                    {
                                        if (World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(111067);
                                            GC.MyChar.AddItem(120127);
                                            GC.MyChar.AddItem(150137);
                                            GC.MyChar.AddItem(131067);
                                            GC.MyChar.AddItem(160137);
                                            GC.MyChar.AddItem(900047);
                                            GC.MyChar.AddItem(410137);
                                            GC.MyChar.AddItem(420137);
                                            GC.MyChar.AddItem(480137);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(111068);
                                            GC.MyChar.AddItem(120128);
                                            GC.MyChar.AddItem(150138);
                                            GC.MyChar.AddItem(131068);
                                            GC.MyChar.AddItem(160138);
                                            GC.MyChar.AddItem(900048);
                                            GC.MyChar.AddItem(410138);
                                            GC.MyChar.AddItem(420138);
                                            GC.MyChar.AddItem(480138);
                                        }
                                    }
                                    else if (GC.MyChar.Job == 24 && (GC.MyChar.Reborns == 0 || GC.MyChar.Reborns == 2))
                                        GC.MyChar.AddItem(700031);
                                    else if (GC.MyChar.Job == 24 && GC.MyChar.Reborns == 1)
                                    {
                                        Game.Item I = new Ultimate.Game.Item();
                                        I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.ID = 131087;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        // try
                                        // {
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        //  }
                                        //catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                                        I.Soc1 = Game.Item.Gem.EmptySocket;
                                        I.Effect = Ultimate.Game.Item.RebornEffect.None;

                                        GC.MyChar.AddItem(I);
                                    }
                                    else if (GC.MyChar.Job == 25)
                                        GC.MyChar.AddItem(1088000);

                                    AddText("Congratulations! You are now " + ((Game.Character.JobName)GC.MyChar.Job).ToString() + " and received a gift.");
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
                                AddText("Make sure you have at least 9 free inventory slots!");
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
                    AddText("You are already WarriorMaster, i cannot promote you anymore.");
                    AddOption("I see.", 255);
                }
                #endregion
                else if (_linkback == 2)
                {
                    AddText("Choose from the skills listed below.");
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)2];
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
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)2];
                    for (byte i = 7; i < Skills.Count; i++)
                        AddOption(((Extra.SkillIDs)(((SkillLearn)Skills[i]).ID)).ToString() + "(Lv " + ((SkillLearn)Skills[i]).LevelReq.ToString() + " )", (byte)(20 + i));
                    AddOption("Previous", 2);
                }
                else if (_linkback >= 20 && _linkback <= 34)
                {
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)2];
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