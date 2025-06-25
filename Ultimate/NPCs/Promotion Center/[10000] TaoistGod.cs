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
    public class NPC_10000 : NPCBase
    {
        public NPC_10000(Main.GameClient _client)
            : base(_client)
        {
            ID = 10000;
            Face = 6;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            if (_linkback == 0)
            {
                if (GC.MyChar.Job >= 100 && GC.MyChar.Job <= 145)
                {
                    AddText("Most of the Taoists are little concerned with anything outside the pursuit of advanced spiritual powers. You are gifted in harnessing your inner power, but remember, the roots of success lie in thoroughness and attention to detail.");
                    AddOption("Promote me.", 1);
                    AddOption("Learn Skills.", 2);
                    AddOption("Just passing by.", 255);
                }
                else
                {
                    AddText("Taoists do not share their secrets of battle with others. I shall not teach you.");
                    AddOption("I see", 255);
                }
            }
            #region Promote
            else if (GC.MyChar.Job >= 100 && GC.MyChar.Job <= 145)
            {
                if (_linkback == 1 && (GC.MyChar.Job == 145 || GC.MyChar.Job == 135))
                {
                    AddText("You are already a saint, i cannot promote you anymore.");
                    AddOption("I see.", 255);
                }
                else if (_linkback == 1 || _linkback == 10 || _linkback == 11 && (GC.MyChar.Job <= 144 || (GC.MyChar.Job < 140 && GC.MyChar.Job <= 134)))
                {
                    if (_linkback == 1)
                    {
                        if (GC.MyChar.Job != 101)
                            AddText("You need to be level " + GC.MyChar.LevReqForPromote + " to promote to " + ((Game.Character.JobName)(GC.MyChar.Job + 1)).ToString());
                        else
                        {
                            AddText("You need to be level " + GC.MyChar.LevReqForPromote + " to promote.");
                            AddText("You have to choose if you want to become a FireTaoist or a WaterTaoist.");
                        }
                        if (GC.MyChar.Job == 132 || GC.MyChar.Job == 142)
                            AddText("Also you will need an Emerald.");
                        else if (GC.MyChar.Job == 133 || GC.MyChar.Job == 143)
                            AddText("Also you will need a Meteor.");
                        else if (GC.MyChar.Job == 134 || GC.MyChar.Job == 144)
                            AddText("Also you will need a MoonBox.");
                        if (GC.MyChar.Job == 101)
                        {
                            AddOption("Promote me to FireTaoist", 10);
                            AddOption("Promote me to WaterTaoist", 11);
                        }
                        else
                            AddOption("Promote me.", 10);
                        AddOption("Forget it.", 255);
                    }
                    else if (_linkback == 10 || _linkback == 11)
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
                                    if (GC.MyChar.Job == 101)
                                    {
                                        if (_linkback == 10)
                                            GC.MyChar.Job = 142;
                                        else
                                            GC.MyChar.Job = 132;
                                    }
                                    else if (GC.MyChar.Job <= 134)
                                        GC.MyChar.Job++;
                                    else if (GC.MyChar.Job >= 142 && GC.MyChar.Job <= 144)
                                        GC.MyChar.Job++;
                                    if (GC.MyChar.Job == 101 && !GC.MyChar.Reborn)
                                    {
                                        if (!World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(114007);
                                            GC.MyChar.AddItem(121027);
                                            GC.MyChar.AddItem(152017);
                                            GC.MyChar.AddItem(421027);
                                            GC.MyChar.AddItem(134007);
                                            GC.MyChar.AddItem(160037);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(114008);
                                            GC.MyChar.AddItem(121028);
                                            GC.MyChar.AddItem(152018);
                                            GC.MyChar.AddItem(421028);
                                            GC.MyChar.AddItem(134008);
                                            GC.MyChar.AddItem(160038);
                                        }
                                    }
                                    else if ((GC.MyChar.Job == 132 || GC.MyChar.Job == 142) && !GC.MyChar.Reborn)
                                    {
                                        if (World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(114037);
                                            GC.MyChar.AddItem(121087);
                                            GC.MyChar.AddItem(152087);
                                            GC.MyChar.AddItem(421077);
                                            GC.MyChar.AddItem(134037);
                                            GC.MyChar.AddItem(160077);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(114038);
                                            GC.MyChar.AddItem(121088);
                                            GC.MyChar.AddItem(152088);
                                            GC.MyChar.AddItem(421078);
                                            GC.MyChar.AddItem(134038);
                                            GC.MyChar.AddItem(160078);
                                        }
                                    }
                                    else if ((GC.MyChar.Job == 133 || GC.MyChar.Job == 143) && !GC.MyChar.Reborn)
                                    {
                                        if (World.LowRatedServer)
                                        {
                                            GC.MyChar.AddItem(114067);
                                            GC.MyChar.AddItem(121127);
                                            GC.MyChar.AddItem(152147);
                                            GC.MyChar.AddItem(421137);
                                            GC.MyChar.AddItem(134067);
                                            GC.MyChar.AddItem(160137);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(114068);
                                            GC.MyChar.AddItem(121128);
                                            GC.MyChar.AddItem(152148);
                                            GC.MyChar.AddItem(421138);
                                            GC.MyChar.AddItem(134068);
                                            GC.MyChar.AddItem(160138);
                                        }
                                    }
                                    else if ((GC.MyChar.Job == 134 || GC.MyChar.Job == 144) && (GC.MyChar.Reborns == 0 || GC.MyChar.Reborns == 2))
                                        GC.MyChar.AddItem(700031);
                                    else if ((GC.MyChar.Job == 134 || GC.MyChar.Job == 144) && GC.MyChar.Reborns == 1)
                                    {
                                        Game.Item I = new Ultimate.Game.Item();
                                        I.ID = 134087;
                                        I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        I.UID = (uint)Program.Rnd.Next(10000000);
                                        I.Soc1 = Game.Item.Gem.EmptySocket;
                                        GC.MyChar.AddItem(I);
                                    }
                                    else if (GC.MyChar.Job == 135 || GC.MyChar.Job == 145)
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
                    AddText("You are already a saint, i cannot promote you anymore.");
                    AddOption("I see.", 255);
                }
                #endregion
                else if (_linkback == 2)
                {
                    AddText("Choose from the skills listed below.");
                    byte e = 10;
                    if (GC.MyChar.Job > 130) e = 13;
                    if (GC.MyChar.Job > 140) e = 14;
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)e];
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
                    byte e = 10;
                    if (GC.MyChar.Job > 130) e = 13;
                    if (GC.MyChar.Job > 140) e = 14;
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)e];
                    for (byte i = 7; i < Skills.Count; i++)
                        AddOption(((Extra.SkillIDs)(((SkillLearn)Skills[i]).ID)).ToString() + "(Lv " + ((SkillLearn)Skills[i]).LevelReq.ToString() + " )", (byte)(20 + i));
                    AddOption("Previous", 2);
                }
                else if (_linkback >= 20 && _linkback <= 34)
                {
                    byte e = 10;
                    if (GC.MyChar.Job > 130) e = 13;
                    if (GC.MyChar.Job > 140) e = 14;
                    List<SkillLearn> Skills = (List<SkillLearn>)Database.SkillForLearning[(byte)e];
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