using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Ultimate.Features
{
    public enum GuildRank : byte
    {
        Member = 50,
        InternMgr = 60,
        DeputyMgr = 70,
        BranchMgr = 80,
        DeputyManager = 90,
        GuildLeader = 100
    }
    public class MemberInfo
    {
        public uint MembID;
        public string MembName;
        public uint Donation;
        public byte Level;
        public GuildRank Rank;
        public ushort MyGuildID;

        public string MemberString
        {
            get
            {
                string e = "";
                e += MembName + "~" + Level + "~" + Convert.ToByte(World.H_Chars.ContainsKey(MembID));
                //e = Convert.ToChar((byte)e.Length) + e;
                return e;
            }
        }
        public Character Info
        {
            get
            {
                if (World.H_Chars.ContainsKey(MembID))
                    return World.H_Chars[MembID];
                return null;
            }
        }
        //public void WriteThis(BinaryWriter BW)
        //{
        //    BW.Write(MembID);
        //    BW.Write(MembName);
        //    BW.Write(Donation);
        //    BW.Write(Level);
        //    BW.Write((byte)Rank);
        //    BW.Write(MyGuildID);
        //}
        public void ReadThis(BinaryReader BR)
        {
            try
            {
                MembID = BR.ReadUInt32();
                MembName = BR.ReadString();
                Donation = BR.ReadUInt32();
                Level = BR.ReadByte();
                Rank = (GuildRank)BR.ReadByte();
                MyGuildID = BR.ReadUInt16();
            }
            catch
            {
                MembID = 0;
                MembName = "";
                Donation =0;
                Level = 0;
                Rank = GuildRank.Member;
                MyGuildID = 0;
            }
        }
    }
    class Guilds
    {
        public static Dictionary<ushort, Guild> AllTheGuilds = new Dictionary<ushort, Guild>();

        public static bool ValidName(string Name)
        {
            if (Name.Length < 3 || Name.Length > 16)
                return false;
            foreach (Guild G in AllTheGuilds.Values)
                if (G.GuildName == Name)
                    return false;
            return true;
        }
        public static void SaveGuilds()
        {
            try
            {
                foreach (Guild G in AllTheGuilds.Values.ToList())
                {
                    byte LastWinner = 0;
                    if (GuildWars.LastWinner != null && GuildWars.LastWinner.GuildID == G.GuildID)
                        LastWinner = 1;

                    MySQL.MySqlCommand Guilds = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    Guilds.Insert("guilds").Insert("ID", G.GuildID).Insert("Name", G.GuildName).Insert("LeaderID", G.Creator.MembID).Insert("LeaderName", G.Creator.MembName).Insert("Bulletin", G.Bulletin).Insert("Fund", G.Fund).Insert("Wins", G.Wins).Insert("Members", G.MembersCount).Insert("LastWinner",LastWinner).Execute();

                    foreach (Dictionary<uint, MemberInfo> H in G.Members.Values)
                        foreach (MemberInfo M in H.Values)
                        {
                            MySQL.MySqlCommand Member = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                            Member.Insert("guildmembers").Insert("MemberID", M.MembID).Insert("Name", M.MembName).Insert("Donation", M.Donation).Insert("Level", M.Level).Insert("Rank", (byte)M.Rank).Insert("GuildID", M.MyGuildID).Execute();
                        }
                    //if (G.SaveAssociates)
                    //{
                    //    MySQL.MySqlCommand Associates = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                    //    Associates.Delete("guildrelations", "guilduid", G.GuildID).Execute();

                    //    foreach (KeyValuePair<uint, string> Associate in G.Allies)
                    //    {
                    //        if (Associate.Key != 0)
                    //        {
                    //            MySQL.MySqlCommand Allies = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    //            Allies.Insert("guildrelations").Insert("guilduid", G.GuildID).Insert("associateuid", Associate.Key).Insert("associatename", Associate.Value).Insert("type", true).Execute();
                    //        }
                    //    }
                    //    foreach (KeyValuePair<uint, string> Associate in G.Enemies)
                    //    {
                    //        if (Associate.Key != 0)
                    //        {
                    //            MySQL.MySqlCommand Enemies = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    //            Enemies.Insert("guildrelations").Insert("guilduid", G.GuildID).Insert("associateuid", Associate.Key).Insert("associatename", Associate.Value).Insert("type", false).Execute();
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }
        }
        public static void LoadGuilds()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("guilds");
                MySQL.MySqlReader Guilds = new MySQL.MySqlReader(Cmd);
                while (Guilds.Read())
                {
                    Guild G = new Guild();
                    G.GuildID = Guilds.ReadUInt16("ID");
                    G.GuildName = Guilds.ReadString("Name");

                    Dictionary<uint, MemberInfo> CreatorHt = new Dictionary<uint, MemberInfo>();
                    Dictionary<uint, MemberInfo> DLs = new Dictionary<uint, MemberInfo>();
                    Dictionary<uint, MemberInfo> NMs = new Dictionary<uint, MemberInfo>();
                    
                    MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("guildmembers").Where("GuildID", G.GuildID);
                    MySQL.MySqlReader Members = new MySQL.MySqlReader(Cmd2);

                    while (Members.Read())
                    {
                        MemberInfo M = new MemberInfo();
                        M.MembID = Members.ReadUInt32("MemberID");
                        M.MembName = Members.ReadString("Name");
                        M.Donation = Members.ReadUInt32("Donation");
                        M.Level = Members.ReadByte("Level");
                        M.Rank = (GuildRank)Members.ReadByte("Rank");
                        M.MyGuildID = G.GuildID;

                        if (M.Rank == GuildRank.GuildLeader)
                        {
                            if (!CreatorHt.ContainsKey(M.MembID) && M.MembID != 0)
                            {
                                CreatorHt.Add(M.MembID, M);
                                G.Creator = M;
                            }
                        }
                        else if (M.Rank == GuildRank.DeputyManager)
                        {
                            if (!DLs.ContainsKey(M.MembID) && M.MembID != 0)
                                DLs.Add(M.MembID, M);
                        }
                        else if (M.Rank == GuildRank.Member)
                            if (!NMs.ContainsKey(M.MembID) && M.MembID != 0)
                                NMs.Add(M.MembID, M);
                        MySQL.MySqlCommand GU = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                        GU.Update("characters").Set("GuildID", G.GuildID).Where("UID", M.MembID).Execute();
                    }

                    G.Members.Add((byte)100, CreatorHt);
                    G.Members.Add((byte)90, DLs);
                    G.Members.Add((byte)50, NMs);

                    G.Bulletin = Guilds.ReadString("Bulletin");
                    G.Fund = Guilds.ReadUInt32("Fund");
                    G.Wins = Guilds.ReadUInt32("Wins");

                    MySQL.MySqlCommand Cmd3 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("guildrelations").Where("guilduid", G.GuildID);
                    MySQL.MySqlReader Associates = new MySQL.MySqlReader(Cmd3);

                    while (Associates.Read())
                    {
                        try
                        {
                            if (Associates.ReadBoolean("type"))
                                G.Allies.Add(Associates.ReadUInt32("associateuid"), Associates.ReadString("associatename"));
                            else if (!Associates.ReadBoolean("type"))
                                G.Enemies.Add(Associates.ReadUInt32("associateuid"), Associates.ReadString("associatename"));
                        }
                        catch(Exception e)
                        {
                            World.ExcAdd += e + " GuildID= " + G.GuildID + "\r\n";
                            Console.WriteLine(e + " GuildID= " + G.GuildID);
                        }
                    }
                    if (G.GuildID != 0)
                    {
                        AllTheGuilds.Add(G.GuildID, G);
                        if (Guilds.ReadBoolean("LastWinner"))
                            GuildWars.LastWinner = G;
                    }
                    else
                        World.ExcAdd += G.GuildName + " was not added! " + G.Creator.MembName + " was the GL! \r\n";
                }

                foreach (Guild G in AllTheGuilds.Values.ToList())
                {
                    if (G.Creator == null)
                        G.Disband();
                    else
                    {
                        foreach (ushort UID in G.Allies.Keys.ToList())
                            if (!AllTheGuilds.ContainsKey(UID))
                            {
                                G.Allies.Remove(UID);
                                MySQL.MySqlCommand Del = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                                Del.Delete("guildrelations", "associateuid", UID).Execute();
                                Del = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                                Del.Delete("guildrelations", "guilduid", UID).Execute();
                            }
                        foreach (ushort UID in G.Enemies.Keys.ToList())
                            if (!AllTheGuilds.ContainsKey(UID))
                            {
                                G.Enemies.Remove(UID);
                                MySQL.MySqlCommand Del = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                                Del.Delete("guildrelations", "associateuid", UID).Execute();
                                Del = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                                Del.Delete("guildrelations", "guilduid", UID).Execute();
                            }
                    }
                }
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }
        }
        public static void CreateNewGuild(string GName, ushort GID, Character Creator)
        {
            Guild G = new Guild(GID, GName);
            MemberInfo M = new MemberInfo();
            M.Rank = GuildRank.GuildLeader;
            M.MembID = Creator.EntityID;
            M.Level = Creator.Level;
            M.MembName = Creator.Name;
            M.Donation = 1000000;
            M.MyGuildID = GID;
            Creator.MyGuild = G;
            Creator.GuildDonation = 1000000;
            Creator.GuildRank = GuildRank.GuildLeader;
            G.Creator = M;
            Creator.MembInfo = M;
            G.Members[(byte)100].Add(M.MembID, M);
            AllTheGuilds.Add(G.GuildID, G);

            MySQL.MySqlCommand Guilds = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
            Guilds.Insert("guilds").Insert("ID", G.GuildID).Insert("Name", G.GuildName).Insert("LeaderID", G.Creator.MembID).Insert("LeaderName", G.Creator.MembName).Insert("Bulletin", G.Bulletin).Insert("Fund", G.Fund).Insert("Wins", G.Wins).Insert("Members", G.MembersCount).Insert("LastWinner", 0).Execute();

            MySQL.MySqlCommand Member = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
            Member.Insert("guildmembers").Insert("MemberID", M.MembID).Insert("Name", M.MembName).Insert("Donation", M.Donation).Insert("Level", M.Level).Insert("Rank", (byte)M.Rank).Insert("GuildID", M.MyGuildID).Execute();
            //if (!World.SaveGuilds)
            //    World.SaveGuilds = true;

            //Creator.MyClient.AddSend(Packets.GuildInfo(Creator.MyGuild, Creator));
            //Creator.MyClient.AddSend(Packets.StringPacket(Creator.MyGuild.GuildID, StringType.GuildName, Creator.MyGuild.GuildName));
            //World.Spawn(Creator, false);
            //World.Spawn(Creator, false);
            //World.Spawn(Creator, false);
            //World.Spawn(Creator, false);
            //World.Spawn(Creator, false);
            //foreach (Character C in Creator.ScreenChars.Values)
            //{
            //    //C.MyClient.AddSend(Packets.GeneralData(Creator.EntityID, 0, 0, 0, 135).Get);
            //    C.MyClient.AddSend(Packets.StringPacket(Creator.MyGuild.GuildID, Game.StringType.GuildName, Creator.MyGuild.GuildName));
            //    C.MyClient.AddSend(Packets.SpawnEntity(Creator));
            //    //M.Info.MyClient.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildName, G.GuildName));
            //}
            //G.Allies.Add(0, "");
            //G.Enemies.Add(0, "");
            // Features.Guilds.SaveGuilds();
        }
    }
    public class Guild
    {
        public MemberInfo Creator;
        public Dictionary<byte, Dictionary<uint, MemberInfo>> Members = new Dictionary<byte, Dictionary<uint, MemberInfo>>();
        public uint Fund;
        public uint Wins;
        public ushort GuildID;
        public string GuildName;
        public string Bulletin = "Post the guild bulletin here.";
        public bool SaveAssociates = true;
        public Dictionary<uint, string> Allies = new Dictionary<uint, string>();
        public Dictionary<uint, string> Enemies = new Dictionary<uint, string>();

        public Guild(ushort guildid, string guildname)
        {
            Fund = 1000000;
            GuildID = guildid;
            GuildName = guildname;

            Dictionary<uint, MemberInfo> CreatorHt = new Dictionary<uint, MemberInfo>();
            Dictionary<uint, MemberInfo> DLs = new Dictionary<uint, MemberInfo>();
            Dictionary<uint, MemberInfo> Membs = new Dictionary<uint, MemberInfo>();
            Members.Add((byte)100, CreatorHt);
            Members.Add((byte)90, DLs);
            Members.Add((byte)50, Membs);
        }
        public Guild()
        {

        }
        public int MembersCount
        {
            get
            {
                int e = 0;
                foreach (Dictionary<uint, MemberInfo> H in Members.Values)
                    foreach (MemberInfo M in H.Values)
                        e++;
                return e;
            }
        }
        public void Disband()
        {
            foreach (KeyValuePair<uint, MemberInfo> DE in Members[(byte)50])
            {
                MemberInfo M = (MemberInfo)DE.Value;
                Character C = M.Info;
                if (C != null)
                {
                    foreach (uint UID in Allies.Keys)
                        C.MyClient.AddSend(Packets.SendGuild(UID, 8));
                    foreach (uint UID in Enemies.Keys)
                        C.MyClient.AddSend(Packets.SendGuild(UID, 10));
                    if (C.Top == 1 || C.Top == 2)
                    {
                        if (C.StatEff.Contains(Game.StatusEffectEn.TopDeputyLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopDeputyLeader);
                        else if (C.StatEff.Contains(Game.StatusEffectEn.TopGuildLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopGuildLeader);
                        C.Top = 0;
                    }
                    C.MyClient.AddSend(Packets.SendGuild(GuildID, 19));
                    C.MyGuild = null;
                    C.GuildRank = 0;
                    C.GuildDonation = 0;
                    World.Spawn(C, false);
                }
            }
            foreach (KeyValuePair<uint, MemberInfo> DE in Members[(byte)90])
            {
                MemberInfo M = (MemberInfo)DE.Value;
                Character C = M.Info;
                if (C != null)
                {
                    foreach (uint UID in Allies.Keys)
                        C.MyClient.AddSend(Packets.SendGuild(UID, 8));
                    foreach (uint UID in Enemies.Keys)
                        C.MyClient.AddSend(Packets.SendGuild(UID, 10));
                    if (C.Top == 1 || C.Top == 2)
                    {
                        if (C.StatEff.Contains(Game.StatusEffectEn.TopDeputyLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopDeputyLeader);
                        else if (C.StatEff.Contains(Game.StatusEffectEn.TopGuildLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopGuildLeader);
                        C.Top = 0;
                    }
                    C.MyClient.AddSend(Packets.SendGuild(GuildID, 19));
                    C.MyGuild = null;
                    C.GuildRank = 0;
                    C.GuildDonation = 0;
                    World.Spawn(C, false);
                }
            }
            foreach (KeyValuePair<uint, MemberInfo> DE in Members[(byte)100])
            {
                MemberInfo M = (MemberInfo)DE.Value;
                Character C = M.Info;
                if (C != null)
                {
                    foreach (uint UID in Allies.Keys)
                        C.MyClient.AddSend(Packets.SendGuild(UID, 8));
                    foreach (uint UID in Enemies.Keys)
                        C.MyClient.AddSend(Packets.SendGuild(UID, 10));
                    if (C.Top == 1 || C.Top == 2)
                    {
                        if (C.StatEff.Contains(Game.StatusEffectEn.TopDeputyLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopDeputyLeader);
                        else if (C.StatEff.Contains(Game.StatusEffectEn.TopGuildLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopGuildLeader);
                        C.Top = 0;
                    }
                    C.MyClient.AddSend(Packets.SendGuild(GuildID, 19));
                    C.MyGuild = null;
                    C.GuildRank = 0;
                    C.GuildDonation = 0;
                    World.Spawn(C, false);
                }
            }

            Allies = null;
            Enemies = null;
            Members = null; 
            Creator = null;
                

            World.SendMsgToAll("SYSTEM", GuildName + " has been disbanded.", 2000, 0);
            Guilds.AllTheGuilds.Remove(GuildID);

            foreach (KeyValuePair<ushort, Guild> G in Guilds.AllTheGuilds.ToList())
                if (G.Value.Allies.ContainsKey(GuildID))
                    G.Value.Allies.Remove(GuildID);
                else if (G.Value.Enemies.ContainsKey(GuildID))
                    G.Value.Enemies.Remove(GuildID);

            MySQL.MySqlCommand D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
            D.Delete("guildrelations", "guilduid", GuildID).Execute();
            D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
            D.Delete("guildrelations", "associateuid", GuildID).Execute();
            D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
            D.Delete("guilds", "ID", GuildID).Execute();
            D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
            D.Delete("guildmembers", "GuildID", GuildID).Execute();

            //if (!World.SaveGuilds)
            //    World.SaveGuilds = true;
        }

        public bool AddAlly(Guild G)
        {
            if (!Allies.ContainsKey(G.GuildID))
            {
                foreach (Dictionary<uint, Features.MemberInfo> H in Members.Values)
                {
                    foreach (Features.MemberInfo M in H.Values)
                        if (M.Info != null)
                        {
                            //M.Info.MyClient.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildAllies, G.GuildName));
                            M.Info.MyClient.AddSend(Packets.SendGuild(G.GuildID, 7));
                        }
                }
                foreach (Dictionary<uint, Features.MemberInfo> H in G.Members.Values)
                {
                    foreach (Features.MemberInfo M in H.Values)
                        if (M.Info != null)
                        {
                            M.Info.MyClient.AddSend(Packets.SendGuild(GuildID, 7));
                            //M.Info.MyClient.AddSend(Packets.StringPacket(GuildID, Game.StringType.GuildAllies, GuildName));
                        }
                }
                Allies.Add(G.GuildID, G.GuildName);
                if (!G.Allies.ContainsKey(GuildID))
                    G.Allies.Add(GuildID, GuildName);

                MySQL.MySqlCommand Ally = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Ally.Insert("guildrelations").Insert("guilduid", GuildID).Insert("associateuid", G.GuildID).Insert("associatename", G.GuildName).Insert("type", true).Execute();
                
                Ally = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Ally.Insert("guildrelations").Insert("guilduid", G.GuildID).Insert("associateuid", GuildID).Insert("associatename", GuildName).Insert("type", true).Execute();

                return true;
            }
            else
                return false;
        }
        public bool RemoveAlly(Guild G)
        {
            if (Allies.ContainsKey(G.GuildID))
            {
                foreach (Dictionary<uint, MemberInfo> H in Members.Values)
                {
                    foreach (MemberInfo M in H.Values)
                        if (M.Info != null)
                        {
                            M.Info.MyClient.AddSend(Packets.SendGuild(G.GuildID, 8));
                            M.Info.MyClient.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildName, G.GuildName));
                            //M.Info.MyClient.AddSend(Packets.SendGuild(G.GuildID, 10));
                        }
                }
                if (G.Allies.ContainsKey(GuildID))
                {
                    G.Allies.Remove(GuildID);
                    foreach (Dictionary<uint, MemberInfo> H in G.Members.Values)
                    {
                        foreach (MemberInfo M in H.Values)
                            if (M.Info != null)
                            {
                                M.Info.MyClient.AddSend(Packets.SendGuild(GuildID, 8));
                                M.Info.MyClient.AddSend(Packets.StringPacket(GuildID, Game.StringType.GuildName, GuildName));
                                //M.Info.MyClient.AddSend(Packets.SendGuild(GuildID, 10));
                            }
                    }
                }

                Allies.Remove(G.GuildID);
                MySQL.MySqlCommand D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                D.Delete("guildrelations", "guilduid", GuildID).And("associateuid", G.GuildID).Execute();
                D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                D.Delete("guildrelations", "guilduid", G.GuildID).And("associateuid", GuildID).Execute();
            }
            else
                return false;

            return true;
        }
        public bool AddEnemy(Guild G)
        {
            if (!Enemies.ContainsKey(G.GuildID))
            {
                foreach (Dictionary<uint, Features.MemberInfo> H in Members.Values)
                {
                    foreach (Features.MemberInfo M in H.Values)
                        if (M.Info != null)
                            M.Info.MyClient.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildEnemies, G.GuildName));
                }
                Enemies.Add(G.GuildID, G.GuildName);

                MySQL.MySqlCommand Enemy = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Enemy.Insert("guildrelations").Insert("guilduid", GuildID).Insert("associateuid", G.GuildID).Insert("associatename", G.GuildName).Insert("type", false).Execute();
                return true;
            }
            else
                return false;
        }
        public bool RemoveEnemy(Guild G)
        {
            if (Enemies.ContainsKey(G.GuildID))
            {
                foreach (Dictionary<uint, MemberInfo> H in Members.Values)
                {
                    foreach (MemberInfo M in H.Values)
                        if (M.Info != null)
                        {
                            M.Info.MyClient.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildName, G.GuildName));
                            M.Info.MyClient.AddSend(Packets.SendGuild(G.GuildID, 10));
                        }
                }
                if (G.Enemies.ContainsKey(GuildID))
                {
                    G.Enemies.Remove(GuildID);
                    foreach (Dictionary<uint, MemberInfo> H in G.Members.Values)
                    {
                        foreach (MemberInfo M in H.Values)
                            if (M.Info != null)
                            {
                                M.Info.MyClient.AddSend(Packets.StringPacket(GuildID, Game.StringType.GuildName, GuildName));
                                M.Info.MyClient.AddSend(Packets.SendGuild(GuildID, 10));
                            }
                    }
                }
                Enemies.Remove(G.GuildID);

                MySQL.MySqlCommand D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                D.Delete("guildrelations", "guilduid", GuildID).And("associateuid", G.GuildID).Execute();
            }
            else
                return false;

            return true;
        }

        public bool AddMember(MemberInfo M, bool New)
        {
            if (Find(M.MembID) == null)
            {
                (Members[(byte)50]).Add(M.MembID, M);
                if (New)
                {
                    GuildMsg("SYSTEM", "ALL", M.MembName + " has joined our guild.", 0);

                    MySQL.MySqlCommand Member = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    Member.Insert("guildmembers").Insert("MemberID", M.MembID).Insert("Name", M.MembName).Insert("Donation", M.Donation).Insert("Level", M.Level).Insert("Rank", (byte)M.Rank).Insert("GuildID", M.MyGuildID).Execute();

                    //if (!World.SaveGuilds)
                    //World.SaveGuilds = true;
                }
                return true;
            }
            else return false;
        }
        public void NewBulletin(string B)
        {
            Bulletin = B;
            foreach (Dictionary<uint, MemberInfo> H in Members.Values)
            {
                foreach (MemberInfo M in H.Values)
                {
                    if (World.H_Chars.ContainsKey(M.MembID))
                    {
                        Character C = World.H_Chars[M.MembID];
                        C.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, "SYSTEM", C.Name, B, 2111, 0x83f));
                    }
                }
            }
        }
        public MemberInfo Find(uint UID)
        {
            foreach (Dictionary<uint, MemberInfo> H in Members.Values)
            {
                if (H.ContainsKey(UID))
                    return (MemberInfo)H[UID];
            }
            return null;
        }
        public void MemberLeaves(uint MID, bool Kick)
        {
            MemberInfo M = Find(MID);
            if (M != null)
            {
                if (M.Rank == GuildRank.GuildLeader)
                {
                    if (Members[(byte)90].Count > 0)
                    {
                        Features.MemberInfo M3 = Members[90][0];

                        if (M3.Rank == Features.GuildRank.DeputyManager)
                        {
                            Creator = M3;
                            M3.Rank = Features.GuildRank.GuildLeader;
                            Members[(byte)90].Remove(M3.MembID);
                            Members[(byte)100].Add(M3.MembID, M3);
                        }
                        Game.Character C2 = M3.Info;
                        if (C2 != null)
                        {
                            C2.GuildRank = Features.GuildRank.GuildLeader;
                            Game.World.Spawn(C2, false);
                            C2.MyClient.AddSend(Packets.GuildInfo(this, C2));
                        }
                    }
                    else
                        Disband();
                }
                else
                {
                    Members[(byte)M.Rank].Remove(MID);
                    

                    if (Kick)
                    {
                        GuildMsg("SYSTEM", "ALL", M.MembName + " has been kicked out of our guild.", 0);
                        MySQL.MySqlCommand UpdateChar = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                        UpdateChar.Update("characters").Set("GuildID", 0).Where("UID", MID).Execute();
                    }
                    else
                        GuildMsg("SYSTEM", "ALL", M.MembName + " has left our guild.", 0);

                    Character C = M.Info;
                    if (C != null)
                    {
                        foreach (uint UID in Allies.Keys)
                            C.MyClient.AddSend(Packets.SendGuild(UID, 8));
                        foreach (uint UID in Enemies.Keys)
                            C.MyClient.AddSend(Packets.SendGuild(UID, 10));
                        if (C.Top == 1 || C.Top == 2)
                        {
                            if (C.StatEff.Contains(Game.StatusEffectEn.TopDeputyLeader))
                                C.StatEff.Remove(Game.StatusEffectEn.TopDeputyLeader);
                            else if (C.StatEff.Contains(Game.StatusEffectEn.TopGuildLeader))
                                C.StatEff.Remove(Game.StatusEffectEn.TopGuildLeader);
                            C.Top = 0;
                        }
                        //string Name = C.MyGuild.GuildName;
                        //ushort ID = C.MyGuild.GuildID;
                        C.MyClient.AddSend(Packets.SendGuild(C.MyGuild.GuildID, 19));
                        //C.MyClient.AddSend(Packets.StringPacket(ID, Game.StringType.GuildName, Name));
                        //World.Spawns(C, false);
                        C.MyGuild = null;
                        C.GuildDonation = 0;
                        C.GuildRank = 0;
                        //C.MyClient.AddSend(Packets.SendGuild(0, 19));
                        World.Spawn(C, false);
                    }

                    MySQL.MySqlCommand D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                    D.Delete("guildmembers", "MemberID", MID).Execute();

                }

                //if (!World.SaveGuilds)
                //    World.SaveGuilds = true;
                // Features.Guilds.SaveGuilds();
            }
        }
        public void MemberLeaves(string Name, bool Kick)
        {
            MemberInfo M = MembOfName(Name);
            if (M != null)
            {
                Members[(byte)M.Rank].Remove(M.MembID);
                if (Kick)
                {
                    GuildMsg("SYSTEM", "ALL", M.MembName + " has been kicked out of our guild.", 0);

                    MySQL.MySqlCommand UpdateChar = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    UpdateChar.Update("characters").Set("GuildID", 0).Where("UID", M.MembID).Execute();
                }
                else
                    GuildMsg("SYSTEM", "ALL", M.MembName + " has left our guild.", 0);

                Character C = M.Info;
                if (C != null)
                {
                    if (C.Top == 1 || C.Top == 2)
                    {
                        if (C.StatEff.Contains(Game.StatusEffectEn.TopDeputyLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopDeputyLeader);
                        else if (C.StatEff.Contains(Game.StatusEffectEn.TopGuildLeader))
                            C.StatEff.Remove(Game.StatusEffectEn.TopGuildLeader);
                        C.Top = 0;
                    }
                    C.MyClient.AddSend(Packets.SendGuild(C.MyGuild.GuildID, 19));
                    C.MyGuild = null;
                    C.GuildDonation = 0;
                    C.GuildRank = 0;
                    World.Spawn(C, false);
                }

                MySQL.MySqlCommand D = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                D.Delete("guildmembers", "MemberID", M.MembID).Execute();
                
                //if (!World.SaveGuilds)
                //    World.SaveGuilds = true;
                // Features.Guilds.SaveGuilds();
            }
        }
        public void GuildMsg(string From, string To, string Msg, uint Mesh)
        {
            foreach (Dictionary<uint, MemberInfo> H in Members.Values)
            {
                foreach (MemberInfo M in H.Values)
                {
                    if (World.H_Chars.ContainsKey(M.MembID))
                    {
                        Character C = World.H_Chars[M.MembID];
                        C.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, From, To, Msg, 2004, Mesh));
                    }
                }
            }
        }
        public void GuildMsg(COPacket Data, uint Sender)
        {
            foreach (Dictionary<uint, MemberInfo> H in Members.Values)
            {
                foreach (MemberInfo M in H.Values)
                {
                    if (World.H_Chars.ContainsKey(M.MembID))
                    {
                        Character C = World.H_Chars[M.MembID];

                        if (C.EntityID != Sender)
                            C.MyClient.AddSend(Data);
                    }
                }
            }
        }
        public MemberInfo MembOfName(string Name)
        {
            foreach(MemberInfo M in (Members[(byte)50]).Values)            
                if (M.MembName == Name)
                    return M;
            foreach (MemberInfo M in (Members[(byte)90]).Values)
                if (M.MembName == Name)
                    return M;
            foreach (MemberInfo M in (Members[(byte)100]).Values)
                if (M.MembName == Name)
                    return M;  
            return null;
        }

        //public void SaveThis(BinaryWriter BW)
        //{
        //    Creator.WriteThis(BW);
        //    BW.Write(MembersCount);
        //    foreach (Dictionary<uint, MemberInfo> H in Members.Values)
        //        foreach (MemberInfo M in H.Values)
        //            M.WriteThis(BW);

        //    BW.Write(Fund);
        //    BW.Write(GuildID);
        //    BW.Write(GuildName);
        //    BW.Write(Bulletin);
        //    BW.Write(Wins);
        //    BW.Write((int)Allies.Count);
        //    foreach (KeyValuePair<uint, string> kvp in Allies)
        //    {
        //        BW.Write(Convert.ToUInt32(kvp.Key));
        //        BW.Write(Convert.ToString(kvp.Value));
        //    }
        //    BW.Write((int)Enemies.Count);
        //    foreach (KeyValuePair<uint, string> kvp in Enemies)
        //    {
        //        BW.Write(Convert.ToUInt32(kvp.Key));
        //        BW.Write(Convert.ToString(kvp.Value));
        //    }
        //}
        //public Guild(BinaryReader BR)
        //{
        //    Creator = new MemberInfo();
        //    Creator.ReadThis(BR);
        //    int MembCount = BR.ReadInt32();
        //    Dictionary<uint, MemberInfo> CreatorHt = new Dictionary<uint, MemberInfo>();
        //    Dictionary<uint, MemberInfo> DLs = new Dictionary<uint, MemberInfo>();
        //    Dictionary<uint, MemberInfo> NMs = new Dictionary<uint, MemberInfo>();
        //    for (int i = 0; i < MembCount; i++)
        //    {
        //        MemberInfo M = new MemberInfo();
        //        M.ReadThis(BR);
        //        if (M.Rank == GuildRank.GuildLeader)
        //        {
        //            if (!CreatorHt.ContainsKey(M.MembID) && M.MembID != 0)
        //                CreatorHt.Add(M.MembID, M);
        //        }
        //        else if (M.Rank == GuildRank.DeputyManager)
        //        {
        //            if (!DLs.ContainsKey(M.MembID) && M.MembID != 0)
        //                DLs.Add(M.MembID, M);
        //        }
        //        else if (M.Rank == GuildRank.Member)
        //            if (!NMs.ContainsKey(M.MembID) && M.MembID != 0)
        //                NMs.Add(M.MembID, M);
        //    }
        //    Members.Add((byte)100, CreatorHt);
        //    Members.Add((byte)90, DLs);
        //    Members.Add((byte)50, NMs);
        //    Fund = BR.ReadUInt32();
        //    GuildID = BR.ReadUInt16();
        //    GuildName = BR.ReadString();
        //    Bulletin = BR.ReadString();
        //    Wins = BR.ReadUInt32();
        //    int a = BR.ReadInt32();
        //    {
        //        for (int i = 0; i < a; i++)
        //            Allies.Add(BR.ReadUInt32(), BR.ReadString());
        //    }
        //    int e = BR.ReadInt32();
        //    {
        //        for (int i = 0; i < e; i++)
        //            Enemies.Add(BR.ReadUInt32(), BR.ReadString());
        //    }
        //}
    }
}