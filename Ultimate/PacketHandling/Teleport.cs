using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class Teleport
    {

        public static void Handle(Main.GameClient GC, byte[] Data)
        {

            if (!GC.LoginDataSent)
            {
                GC.MyChar.ScreenChars = new System.Collections.Concurrent.ConcurrentDictionary<uint, Game.Character>();
                GC.AddSend(Packets.Packet1012Time(GC.MyChar.EntityID));
                if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                    GC.AddSend(Packets.GeneralData((ushort)DMaps.EventMaps[GC.MyChar.Loc.Map], (ushort)DMaps.EventMaps[GC.MyChar.Loc.Map], GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x4a));
                else
                    GC.AddSend(Packets.GeneralData(GC.MyChar.Loc.Map, GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x4a));
                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0xffffffff, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x68));

                if (GC.MyChar.Loc.Map == 1036)
                    GC.AddSend(Packets.MapStatus(GC.MyChar.Loc.Map, 30));
                else if (GC.MyChar.Loc.Map == 2068)
                    GC.AddSend(Packets.MapStatus(GC.MyChar.Loc.Map, 30));
                else if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                    GC.AddSend(Packets.MapStatus((ushort)DMaps.EventMaps[GC.MyChar.Loc.Map], 32));
                else GC.AddSend(Packets.MapStatus(GC.MyChar.Loc.Map, 32));
                foreach (Game.Item I in GC.MyChar.Inventory)
                    GC.AddSend(Packets.AddItem(I, 0));
                GC.MyChar.Equips.Send(GC, true);

                var donations = new Donations(); //WUVO
                donations.check(GC.MyChar);  //WUVO

                foreach (Game.Prof P in GC.MyChar.Profs.Values)
                    GC.AddSend(Packets.Prof(P));
                foreach (Game.Skill S in GC.MyChar.Skills.Values)
                    GC.AddSend(Packets.Skill(S));
                if (GC.MyChar.MyGuild != null)
                {
                    GC.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                    GC.AddSend(Packets.StringPacket(GC.MyChar.MyGuild.GuildID, Game.StringType.GuildName, GC.MyChar.MyGuild.GuildName));
                }
                foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                    GC.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildName, G.GuildName));
                GC.MyChar.PKPoints = GC.MyChar.PKPoints;
                GC.MyChar.Nobility.Rank = GC.MyChar.Nobility.Rank;
                GC.MyChar.BI_Quest = 0;
                CustomDialog.GetDialogs(GC);
                GC.LocalMessage(2000, "Welcome to Ultimate-Conquer! http://ultimate-conquer.com/ ");
                //GC.LocalMessage(2000, "Currently we only have an EU server! We might get one hosted in the US if it reveals to be worth it.");
                GC.LocalMessage(2000, "If you tried reviving and it didn't work please type /forcerevive !");
                GC.LocalMessage(2000, "Enjoy the server at its best, our team is fully commited to provide you the best experience possible.");
                GC.LocalMessage(2000, "Do not forget to vote every 12 hours, it help us expand the server and increase our community.");

                MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Vote.Insert("votes").Insert("EntityID", GC.MyChar.Name).Insert("LastVote", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Insert("IPAddress", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()).Execute();

                if (GC.MyChar.Version != Game.World._serverVersion)
                {
                    GC.AddSend(Packets.ShowDialog(26, 1));
                    Features.Changelog.WindowsInformation(GC.MyChar, (uint)26, GC.MyChar.Version, true);
                    GC.MyChar.Version = (ushort)Game.World._serverVersion;
                }
                else if (GC.MyChar.Level >= 15)
                {
                    GC.AddSend(Packets.ShowDialog(1, 1));
                    GC.AddSend(Packets.ShowDialog(21, 1));
                }
                if (DateTime.Now.Month == 2 && DateTime.Now.Day >= 17 && DateTime.Now.Day <= 20)
                    GC.AddSend(Packets.ShowDialog(27, 1));
                //if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && DateTime.Now.Hour < 19)
                //    GC.LocalMessage(2000, "The Cross-Server Guild War will end at 19:00 Server Time! Make sure you join in and fight for our server!");


                //foreach (Game.Friend F in GC.MyChar.Friends.Values)
                //    GC.AddSend(Packets.FriendEnemyPacket(F.UID, F.Name, 15, Convert.ToByte(F.Online)));

                if (GC.MyChar.CTBPoints > 999)
                    GC.MyChar.CTBPoints = 0;
                if (GC.MyChar.VipLevel == 6)
                    if (GC.MyChar.PassiveSkills)
                        GC.LocalMessage(2000, "Passive Skills: Activated . Type '/passive' if you wish to deactivate them.");
                    else GC.LocalMessage(2000, "Passive Skills: Deactivated: Type '/passive' if you wish to activate them.");
                else if (!GC.MyChar.PassiveSkills)
                    GC.MyChar.PassiveSkills = true;
                if (GC.MyChar.Warning)
                    GC.LocalMessage(2000, "You have a mute warning! Next time you will be muted from world chat!");
                if (GC.MyChar.Muted)
                    GC.LocalMessage(2000, $"You are muted for {GC.MyChar.MutedDays} Days !");
                if (GC.MyChar.Level == 130 && GC.MyChar.Experience > 0)
                    GC.MyChar.Experience = 0;
                if (GC.MyChar.VIPDays == 0 && GC.MyChar.VipLevel != 0)
                    GC.MyChar.VipLevel = 0;
                if (!Game.World.LowRatedServer)
                    GC.MyChar.ExperienceRate = 3;
                else
                    GC.MyChar.ExperienceRate = 2;

                if (GC.MyChar.VipLevel > 0)
                {
                    try
                    {
                        while (DateTime.Now > GC.MyChar.VIPStarted.AddHours(24))
                        {
                            if (GC.MyChar.VIPDays > 0)
                            {
                                GC.MyChar.VIPDays--;
                                GC.MyChar.VIPStarted = GC.MyChar.VIPStarted.AddHours(24);
                            }
                            else
                            {
                                GC.MyChar.VIPStarted = DateTime.Now;
                                GC.MyChar.VipLevel = 0;
                            }
                        }
                    }
                    catch (Exception E) { Console.WriteLine(E.ToString()); }
                    if (GC.MyChar.VIPDays > 0)
                    {
                        if (GC.MyChar.VipLevel <= 4)
                            GC.MyChar.ExperienceRate = 4;
                        else if (GC.MyChar.VipLevel == 6)
                        {
                            if (GC.MyChar.Level >= 125)
                                GC.MyChar.ExperienceRate = 3;
                            else
                                GC.MyChar.ExperienceRate = 5;
                        }
                    }

                    GC.MyChar.VIPAura = true;
                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopNinja);

                }
                if (GC.MyChar.DBScrolls > 0)
                    GC.LocalMessage(2000, "You have " + GC.MyChar.DBScrolls + " DBScrolls waiting to be claimed at Prize NPC in market!");
                if (GC.MyChar.VIPDaysToReceive > 0)
                    GC.LocalMessage(2000, "You have VIP Card " + GC.MyChar.VIPLevelToReceive + " , " + GC.MyChar.VIPDaysToReceive + " days waiting to be claimed at Prize NPC in market!");

                // Program.Voting.AvailabilityToVote(GC.MyChar);
                MySQL.MySqlCommand KayitDosyasi = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("OylamaKaydi").Where("IpAdres", GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString());
                MySQL.MySqlReader KayitSatiri = new MySQL.MySqlReader(KayitDosyasi);

                bool IP_Varligi = false, Isim_Varligi = false;
                string Tarih = "";

                while (KayitSatiri.Read())
                {
                    IP_Varligi = true;
                    Tarih = KayitSatiri.ReadString("Tarih");
                }

                if (!IP_Varligi)
                {
                    KayitDosyasi = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("OylamaKaydi").Where("OyuncuAdi", GC.MyChar.Name);
                    KayitSatiri = new MySQL.MySqlReader(KayitDosyasi);
                    while (KayitSatiri.Read())
                    {
                        Isim_Varligi = true;
                        Tarih = KayitSatiri.ReadString("Tarih");
                    }

                }

                if (Isim_Varligi || IP_Varligi)
                {

                    if (DateTime.Now >= Convert.ToDateTime(Tarih).AddHours(12))
                    {


                        GC.MyChar.MyClient.DialogNPC = 13653;
                        Ultimate.NPCs.NPCHandler.Handle(GC.MyChar.MyClient, null, 13653, 0);

                    }
                }
                else
                {

                    GC.MyChar.MyClient.DialogNPC = 13653;
                    Ultimate.NPCs.NPCHandler.Handle(GC.MyChar.MyClient, null, 13653, 0);
                }


                if (!GC.MyChar.Reborn && GC.MyChar.Level < 70)
                    GC.LocalMessage(2000, "Hello there! Because you are a newbie you can receive free double exp at any time from Assistant in middle Twin City!");

                if (Features.GuildWars.War)
                {
                    var timeLeft = DateTime.Now;
                    if (timeLeft.DayOfWeek != DayOfWeek.Sunday)
                        timeLeft = timeLeft.AddDays(7 - (byte)timeLeft.DayOfWeek);
                    timeLeft = timeLeft.AddHours(19 - timeLeft.Hour).AddMinutes(-timeLeft.Minute);
                    var toDisplay = timeLeft.Subtract(DateTime.Now);

                    GC.LocalMessage(2000, $"The Guild War will end in {toDisplay.Days} Days, {toDisplay.Hours} Hours and {toDisplay.Minutes} Minutes. Make sure you won't miss it!");
                }

               /* foreach (Game.Skill S in GC.MyChar.Skills.Values)
                {
                    if (S.ID > 2000 && S.ID < 2021 || S.ID == 1045 || S.ID == 1046 || S.ID > 2100 && S.ID < 2121)
                    {
                      //  GC.MyChar.Skills.Remove(S.ID);
                        GC.MyChar.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, S.ID, 0, 0, 109));
                    }

                }*/

                ushort Luck = (ushort)Program.Rnd.Next(2001, 2021);

                GC.MyChar.RWSkill(new Game.Skill() { ID = Luck, Lvl = 4, Exp = 0 });


                ushort Luck1 = (ushort)Program.Rnd.Next(2101, 2121);

                GC.MyChar.RWSkill(new Game.Skill() { ID = Luck1, Lvl = 4, Exp = 0 });

                if (GC.MyChar.Loc.Map == 2022)
                {
                    if (GC.MyChar.Job >= 10 && GC.MyChar.Job <= 15)
                        GC.MyChar.DisToKill = 800;
                    else if (GC.MyChar.Job >= 20 && GC.MyChar.Job <= 25)
                        GC.MyChar.DisToKill = 900;
                    else if (GC.MyChar.Job >= 40 && GC.MyChar.Job <= 45)
                        GC.MyChar.DisToKill = 1300;
                    else if (GC.MyChar.Job >= 132 && GC.MyChar.Job <= 135)
                        GC.MyChar.DisToKill = 600;
                    else if (GC.MyChar.Job >= 142 && GC.MyChar.Job <= 145)
                        GC.MyChar.DisToKill = 1000;
                    else
                        GC.MyChar.DisToKill = 800;
                }
                if (Game.World.CurrentBC.Message != null)
                    GC.AddSend(Packets.ChatMessage(GC.MessageID, Game.World.CurrentBC.Name, "ALL", Game.World.CurrentBC.Message, 2500, 0));
                GC.MyChar.CancelProtectTime = false;
                if (GC.MyChar.Loc.Map != 1038)
                    GC.MyChar.ProtectTime = DateTime.Now.AddSeconds(0);
                else GC.MyChar.ProtectTime = DateTime.Now.AddSeconds(0);
                AntiCheatPacket.SendKnownCheats(GC.MyChar);

                if (!ItemPacket.Equip.EquipPassSexReq(GC.MyChar.Equips.Get(9), GC.MyChar))
                {
                    if (GC.MyChar.Inventory.Count < 40)
                    {
                        GC.MyChar.EquipStats(9, false, false);
                        GC.MyChar.AddItem(GC.MyChar.Equips.Get(9));
                        GC.MyChar.Equips.Replace(9, new Game.Item(), GC.MyChar);
                        GC.MyChar.EquipStats(9, true, false);
                    }
                }
                foreach (Game.Enemy E in GC.MyChar.Enemies.Values.ToList())
                {
                    if (System.IO.File.Exists(Game.World.GlobalCharactersPath + E.Name + ".chr"))
                    {
                        string Acc = "";
                        Game.Character C = Database.LoadCharacter(E.Name, ref Acc);
                        if (C == null)
                            GC.MyChar.Enemies.Remove(E.UID);
                        else if (DateTime.Now > C.LastLogin.AddDays(30) || C == null)
                            GC.MyChar.Enemies.Remove(E.UID);
                    }
                    else
                        GC.MyChar.Enemies.Remove(E.UID);
                }

                foreach (Game.Friend F in GC.MyChar.Friends.Values.ToList())
                {

                    if (System.IO.File.Exists(Game.World.GlobalCharactersPath + F.Name + ".chr"))
                    {
                        string Acc = "";
                        Game.Character C = Database.LoadCharacter(F.Name, ref Acc);
                        if (DateTime.Now > C.LastLogin.AddDays(30))
                            GC.MyChar.Friends.Remove(F.UID);
                    }
                    else
                        GC.MyChar.Friends.Remove(F.UID);
                }
                if (DateTime.Now.Month == 12 && DateTime.Now.Day > 9 && GC.MyChar.Loc.Map == 1002)
                    GC.AddSend(Packets.Weather((uint)Features.Weather.CurrentWeather, Features.Weather.Intensity, Features.Weather.Appearence, Features.Weather.Direction));
            }
        }
    }
}
