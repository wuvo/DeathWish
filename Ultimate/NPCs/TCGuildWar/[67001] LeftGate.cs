using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;
using Ultimate.Structures;

namespace Ultimate.NPCs
{
    public class NPC_67001 : NPCBase
    {
        public NPC_67001(Main.GameClient _client)
            : base(_client)
        {
            ID = 67001;
            Face = 30;
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
                        AddText("What do you want to do?");
                        AddOption("Open/close the gate.", 1);
                        AddOption("Get inside.", 2);
                       // AddOption("Repair the gate.", 3);
                        if (GC.MyChar.InventoryContains(721261, 1))
                            AddOption("Use bomb!", 4);
                        AddOption("Nothing", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (Features.TCGuildWars.LastWinner != null)
                            {
                                if (Features.TCGuildWars.LastWinner.GuildID == GC.MyChar.MyGuild.GuildID && (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader || GC.MyChar.GuildRank == Features.GuildRank.DeputyManager))
                                {
                                    if (Features.TCGuildWars.TheLeftGate.CurHP > 0)
                                    {
                                        World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].Opened = !World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].Opened;
                                        World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].ReSpawn();
                                    }
                                    else
                                    {
                                        AddText("The gate is broken it will take 2,000,000 silvers of guild funds to repair it.");
                                        AddOption("Repair the gate.", 3);
                                        AddOption("Nevermind", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You are not authorized to do that.");
                                    AddOption("I see", 255);
                                }
                            }
                        }
                        else
                        {
                            AddText("You are not authorized to do that.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (Features.TCGuildWars.LastWinner != null)
                            {
                                if (Features.TCGuildWars.LastWinner.GuildID == GC.MyChar.MyGuild.GuildID)
                                {
                                    if (GC.MyChar.Alive)
                                        GC.MyChar.Teleport(10200, 438, 395);
                                }
                                else
                                {
                                    AddText("Your guild doesn't have the pole.");
                                    AddOption("I see", 255);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            AddText("You don't have a guild.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.MyGuild.Fund >= 5000000)
                        {
                            GC.MyChar.MyGuild.Fund -= 5000000;
                            World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].CurHP = World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].MaxHP;
                            World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].Opened = false;
                            World.H_SOBs[Features.TCGuildWars.TheLeftGate.EntityID].ReSpawn();
                            AddText("Congratulations ! The Left Gate have been repaired. The 5,000,000 silvers were deducted from your guild fund.");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Your guild doesn't have 5,000,000 silvers of guild funds.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.InventoryContains(721261, 1))
                        {
                            if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 222, 177) <= 8)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721261));
                                if (World.H_SOBs[Features.TCGuildWars.TheRightGate.EntityID].CurHP > 2500000)
                                {
                                    World.H_SOBs[Features.TCGuildWars.TheRightGate.EntityID].CurHP -= 5000000;
                                    World.Action(GC.MyChar, Packets.AttackPacket(GC.MyChar.EntityID, 6702, 222, 177, 2500000, (byte)AttackType.Melee).Get);
                                }
                                else
                                {
                                    World.H_SOBs[Features.TCGuildWars.TheRightGate.EntityID].CurHP = 0;
                                    World.H_SOBs[Features.TCGuildWars.TheRightGate.EntityID].Opened = true;
                                    World.H_SOBs[Features.TCGuildWars.TheRightGate.EntityID].ReSpawn();
                                }
                                World.Action(GC.MyChar, Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "change").Get);
                                World.Action(World.H_SOBs[Features.TCGuildWars.TheRightGate.EntityID], Packets.ShakeScreen(Features.TCGuildWars.TheRightGate.EntityID).Get);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has blown a bomb and the RightGate was severely damaged!", 2011, 0);
                                #region Kill Bomb User
                                GC.MyChar.AtkMem.Attacking = false;
                                GC.MyChar.AtkMem.Target = 0;
                                GC.MyChar.Alive = false;
                                GC.MyChar.CurHP = 0;
                                GC.MyChar.DeathHit = DateTime.Now;
                                World.Action(GC.MyChar, Packets.AttackPacket(GC.MyChar.EntityID, GC.MyChar.EntityID, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, GC.MyChar.CurHP, (byte)AttackType.Kill).Get);
                                foreach (Buff B in GC.MyChar.Buffs.Keys)
                                    GC.MyChar.BDelete.TryAdd(B, B.Lasts);
                                GC.MyChar.BlueName = false;
                                GC.MyChar.PoisonedInfo.Times = 0;
                                GC.MyChar.StatEff.Add(StatusEffectEn.Dead);
                                if (GC.MyChar.MyCompanion != null)
                                    GC.MyChar.MyCompanion.Dissappear();
                                #endregion
                            }
                            else
                            {
                                AddText("You have to be closer to the gate!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have the bomb!");
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