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
    public class NPC_780000 : NPCBase
    {
        public NPC_780000(Main.GameClient _client)
            : base(_client)
        {
            ID = 780000;
            Face = 1;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do you want to receive VIP Level: " + GC.MyChar.VIPUsage.Bless + " for: " + GC.MyChar.VIPUsage.Plus + " days?");
                        AddOption("Yes! Add the VIP to my character!", 1);
                        AddOption("I'll keep it", 255);
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.VipLevel = 0;
                        GC.MyChar.VIPDays = 0;
                        AddText("Your vip days have been successfully reset. You can use now your normal Vip.");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.VipLevel != 3)
                        {
                            if (GC.MyChar.RemoveItem(GC.MyChar.VIPUsage.UID))
                            {
                                if (DateTime.Now > GC.MyChar.VIPStarted.AddHours(24) || GC.MyChar.VIPDays == 0)
                                    GC.MyChar.VIPStarted = DateTime.Now;
                                GC.MyChar.VipLevel = 3;
                                GC.MyChar.VIPDays += 30;
                                if (!Game.World.LowRatedServer)
                                {
                                    if (GC.MyChar.VIPDays > 0)
                                    {
                                        if (GC.MyChar.VipLevel <= 4)
                                            GC.MyChar.ExperienceRate = 5;
                                        else if (GC.MyChar.VipLevel >= 5) GC.MyChar.ExperienceRate = 6;
                                    }
                                }
                                else
                                {
                                    if (GC.MyChar.VIPDays > 0)
                                    {
                                        if (GC.MyChar.VipLevel <= 4)
                                            GC.MyChar.ExperienceRate = 3;
                                        else if (GC.MyChar.VipLevel >= 5) GC.MyChar.ExperienceRate = 4;
                                    }
                                }
                                GC.MyChar.VIPAura = true;
                                GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopNinja);
                                AddText("Your VIP was added successfuly! Type '/VIP' to check your VIP status!");
                                AddOption("Thanks.", 255);
                            }
                            else
                            {
                                AddText("You need the VipCard to receive the VIP!");
                                AddOption("Oh...", 255);
                            }

                        }
                        else
                        {
                            AddText("You have already VIP for Mining, if you want to use Normal Vip we need to reset your vip Days and Level. Are you sure ?");
                            AddOption("Yes! Reset my Vip!", 2);
                            AddOption("I'll use later", 255);
                        }
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}