using MySql.Data.MySqlClient;
using Ultimate.Features;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Game
{
    public enum Looks
    {
        None,
        Statue,
        Pole = 1137,
        LeftGate = 240,
        RightGate = 270
    }
    public class SOB
    {
        public uint EntityID;
        public Location Loc;
        public uint MaxHP;
        public uint CurHP;
        public uint Mesh;
        public Guild LastWinner;
        public bool War = false;
        public Looks Type;

        public string Name;
        public byte Direction;
        public ushort GuildID;
        public byte GuildRank;
        public uint Headgear;
        public uint Necklace;
        public uint Ring;
        public uint RightHand;
        public uint LeftHand;
        public uint Armor;
        public uint Garment;
        public ushort Hair;
        public ushort Frame;
        public byte Action;
        public ushort ArmorColor;
        public ushort LeftHandColor;
        public ushort HeadgearColor;
        public bool Database = false;

        public bool IsPole()
        {
            if (Type == Looks.Pole)
                return true;
            return false;
        }
        public bool IsGate()
        {
            if (Type == Looks.LeftGate || Type == Looks.RightGate)
                return true;
            return false;
        }
        public List<uint> DependantGates;
        public void AddSOB()
        {
            if (!World.H_SOBs.ContainsKey(EntityID))
                World.H_SOBs.AddOrUpdate(EntityID, this, (oldkey, oldvalue) => this);
        }

        public bool Opened
        {
            set
            {
                if (value)
                {
                    if (Type == Looks.LeftGate)
                        Mesh = 250;
                    else if (Type == Looks.RightGate)
                        Mesh = 280;
                }
                else
                {
                    if (Type == Looks.LeftGate)
                        Mesh = (uint)Looks.LeftGate;
                    else if (Type == Looks.RightGate)
                        Mesh = (uint)Looks.RightGate;
                }
            }
            get
            {
                if (Mesh == 250 || Mesh == 280)
                    return true;
                else
                    return false;
            }
        }

        public void Spawn(Character C, bool Check)
        {
            if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, C.Range()) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, C.Range()) || !Check))
            {
                if (IsPole())
                {
                    if (LastWinner == null)
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, "Pole", CurHP, MaxHP));
                    else
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, LastWinner.GuildName, CurHP, MaxHP));
                }
                else if (IsGate())
                {
                    if (CurHP == 0)
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", 1, MaxHP));
                    else
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
                }
                else
                {
                    C.MyClient.AddSend(Packets.SpawnStatue(Name, Mesh, EntityID, GuildID, GuildRank, Headgear, Necklace, Ring, RightHand, LeftHand, Armor, Garment, Hair, Loc.X, Loc.Y, Frame, Direction, Action, ArmorColor, LeftHandColor, HeadgearColor, (ushort)CurHP, (ushort)MaxHP));
                }
            }
        }
        public void ReSpawn()
        {
            foreach (Character C in World.H_Chars.Values)
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, C.Range()))
                {
                    if (IsPole())
                    {
                        if (LastWinner == null)
                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, "Pole", CurHP, MaxHP));
                        else
                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, LastWinner.GuildName, CurHP, MaxHP));
                    }
                    else if (IsGate())
                    {
                        if (CurHP == 0)
                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", 1, MaxHP));
                        else
                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
                    }
                    else
                    {
                        C.MyClient.AddSend(Packets.SpawnStatue(Name, Mesh, EntityID, GuildID, GuildRank, Headgear, Necklace, Ring, RightHand, LeftHand, Armor, Garment, Hair, Loc.X, Loc.Y, Frame, Direction, Action, ArmorColor, LeftHandColor, HeadgearColor, (ushort)CurHP, (ushort)MaxHP));
                    }

                }
        }

        /// <summary>
        /// Handles characters attacks against SOBs
        /// </summary>
        /// <param name="C"></param>
        /// <param name="Damage"></param>
        /// <param name="AtkType"></param>
        public void TakeAttack(Character C, uint Damage, byte AtkType)
        {
            if (Type == Looks.Statue)
                Damage = 10;
            if (AtkType != 21)
                World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
            if (IsPole())
                if (LastWinner != null)
                    if (LastWinner.Fund == 0)
                        Damage *= 2;
            if (Damage >= CurHP)
            {
                CurHP = 0;
                C.XPKO++;
                if (IsPole())
                {
                    if (LastWinner != null && C.MyGuild != null)
                    {

                        if (LastWinner.Fund > CurHP / 45)
                        {
                            LastWinner.Fund -= (CurHP / 45);
                            if (Damage >= 500)
                                C.Silvers += 10;
                        }
                        else
                            LastWinner.Fund = 0;

                        C.MyGuild.Fund += (CurHP / 50);
                        C.GuildDonation += (CurHP / 50);

                    }
                    if (C.MyGuild != null)
                        C.MyClient.AddSend(Packets.GuildInfo(C.MyGuild, C));

                    C.AtkMem.Attacking = false;
                    C.AtkMem.Target = 0;

                    if (War && CounterClock.War && Loc.Map == 1844)
                    {
                        if (C.MyGuild != null)
                            CounterClock.AddScore(C.MyGuild, CurHP);
                        CounterClock.PoleTakedown();
                    }
                    if (War && CityWarTc.War && Loc.Map == 8505)
                    {
                        if (C.MyGuild != null)
                            CityWarTc.AddScore(C.MyGuild, CurHP);
                        CityWarTc.PoleTakedown();
                    }
                    if (War && CityWarPc.War && Loc.Map == 8509)
                    {
                        if (C.MyGuild != null)
                            CityWarPc.AddScore(C.MyGuild, CurHP);
                        CityWarPc.PoleTakedown();
                    }
                    if (War && CityWarAc.War && Loc.Map == 8506)
                    {
                        if (C.MyGuild != null)
                            CityWarAc.AddScore(C.MyGuild, CurHP);
                        CityWarAc.PoleTakedown();
                    }
                    if (War && CityWarDc.War && Loc.Map == 8508)
                    {
                        if (C.MyGuild != null)
                            CityWarDc.AddScore(C.MyGuild, CurHP);
                        CityWarDc.PoleTakedown();
                    }
                    if (War && CityWarBi.War && Loc.Map == 8507)
                    {
                        if (C.MyGuild != null)
                            CityWarBi.AddScore(C.MyGuild, CurHP);
                        CityWarBi.PoleTakedown();
                    }
                    if (War && TCGuildWars.War && Loc.Map == 10200)
                    {
                        if (C.MyGuild != null)
                            TCGuildWars.AddScore(C.MyGuild, CurHP);
                        TCGuildWars.PoleTakedown();
                    }

                    else if (War && GuildWars.War && Loc.Map == 1038)
                    {
                        if (C.MyGuild != null)
                            GuildWars.AddScore(C.MyGuild, CurHP);
                        GuildWars.PoleTakedown();
                    }
                    if (War && PoleWarTC.War && Loc.Map == 1002)
                    {
                        if (C.MyGuild != null)
                            PoleWarTC.AddScore(C.MyGuild, CurHP);
                        PoleWarTC.PoleTakedown();
                    }
                    if (War && PoleWarPC.War && Loc.Map == 1011)
                    {
                        if (C.MyGuild != null)
                            PoleWarPC.AddScore(C.MyGuild, CurHP);
                        PoleWarPC.PoleTakedown();
                    }
                    if (War && PoleWarAC.War && Loc.Map == 1020)
                    {
                        if (C.MyGuild != null)
                            PoleWarAC.AddScore(C.MyGuild, CurHP);
                        PoleWarAC.PoleTakedown();
                    }
                    if (War && PoleWarDC.War && Loc.Map == 1000)
                    {
                        if (C.MyGuild != null)
                            PoleWarDC.AddScore(C.MyGuild, CurHP);
                        PoleWarDC.PoleTakedown();
                    }
                    if (War && PoleWarBI.War && Loc.Map == 1015)
                    {
                        if (C.MyGuild != null)
                            PoleWarBI.AddScore(C.MyGuild, CurHP);
                        PoleWarBI.PoleTakedown();
                    }
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, 14).Get);
                }
                else if (IsGate())
                {
                    if (!Opened)
                    {
                        if (DependantGates != null)
                        {
                            List<SOB> _ToKill = new List<SOB>();
                            foreach (uint UID in DependantGates)
                                if (World.H_SOBs.ContainsKey(UID))
                                    _ToKill.Add(World.H_SOBs[UID]);

                            bool _multiGates = true;
                            for (int a = 0; a < _ToKill.Count; a++)
                                if (_ToKill[a].CurHP > 0) _multiGates = false;

                            if (_multiGates)
                            {
                                Opened = true;
                                ReSpawn();
                                foreach (SOB S in _ToKill)
                                {
                                    S.Opened = true;
                                    S.ReSpawn();
                                }
                            }
                            else if (MyMath.ChanceSuccess(5))
                                C.MyClient.LocalMessage(2000, "You must take down all the gates in order to open them!");
                        }
                        else
                        {
                            Opened = true;
                            ReSpawn();
                        }
                    }
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                }
                else if (Type == Looks.Statue)
                    GuildStatue.RemoveStatue(this);
                //World.Action(this, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);

            }
            else
            {
                if (IsPole())
                {
                    if (LastWinner != null && C.MyGuild != null)
                    {

                        if (LastWinner.Fund > Damage / 40)
                        {
                            LastWinner.Fund -= Damage / 40;
                            if (Damage >= 500)
                                C.Silvers += Damage / 50;

                            C.MyGuild.Fund += Damage / 50;
                            C.GuildDonation += Damage / 50;
                        }
                        else
                            LastWinner.Fund = 0;
                    }
                    if (C.MyGuild != null)
                        C.MyClient.AddSend(Packets.GuildInfo(C.MyGuild, C));

                    if (CounterClock.War && War && Loc.Map == 1844 && C.MyGuild != null)
                        CounterClock.AddScore(C.MyGuild, Damage);
                    if (TCGuildWars.War && War && Loc.Map == 10200 && C.MyGuild != null)
                        TCGuildWars.AddScore(C.MyGuild, Damage);
                    if (CityWarTc.War && War && Loc.Map == 8505 && C.MyGuild != null)
                        CityWarTc.AddScore(C.MyGuild, Damage);
                    if (CityWarPc.War && War && Loc.Map == 8509 && C.MyGuild != null)
                        CityWarPc.AddScore(C.MyGuild, Damage);
                    if (CityWarAc.War && War && Loc.Map == 8506 && C.MyGuild != null)
                        CityWarAc.AddScore(C.MyGuild, Damage);
                    if (CityWarDc.War && War && Loc.Map == 8508 && C.MyGuild != null)
                        CityWarDc.AddScore(C.MyGuild, Damage);
                    if (CityWarBi.War && War && Loc.Map == 8507 && C.MyGuild != null)
                        CityWarBi.AddScore(C.MyGuild, Damage);
                    if (PoleWarTC.War && War && Loc.Map == 1002 && C.MyGuild != null)
                        PoleWarTC.AddScore(C.MyGuild, Damage);
                    if (PoleWarPC.War && War && Loc.Map == 1011 && C.MyGuild != null)
                        PoleWarPC.AddScore(C.MyGuild, Damage);
                    if (PoleWarAC.War && War && Loc.Map == 1020 && C.MyGuild != null)
                        PoleWarAC.AddScore(C.MyGuild, Damage);
                    if (PoleWarDC.War && War && Loc.Map == 1000 && C.MyGuild != null)
                        PoleWarDC.AddScore(C.MyGuild, Damage);
                    if (PoleWarBI.War && War && Loc.Map == 1015 && C.MyGuild != null)
                        PoleWarBI.AddScore(C.MyGuild, Damage);
                    else if (GuildWars.War && War && Loc.Map == 1038 && C.MyGuild != null)
                        GuildWars.AddScore(C.MyGuild, Damage);
                }

                uint CurHP2 = CurHP;
                if (CurHP > Damage)
                    CurHP -= Damage;
                else CurHP = 0;
                if (CurHP > MaxHP)
                {
                    World.ExcAdd += "Pole HP: " + CurHP + "\r\n";
                    Console.WriteLine("SOB PROBLEM! SOB HP: " + CurHP);
                    if (CurHP2 < MaxHP)
                        CurHP = CurHP2;
                    else CurHP = MaxHP / 2;
                }
            }
        }

        /// <summary>
        /// Handles companions attacks against SOBs
        /// </summary>
        /// <param name="C"></param>
        /// <param name="Damage"></param>
        /// <param name="AtkType"></param>
        public void TakeAttack(Companion C, uint Damage, byte AtkType)
        {
            if (AtkType != 21)
                World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
            if (Damage >= CurHP)
            {
                CurHP = 0;
                C.Owner.AtkMem.Attacking = false;
                C.Owner.AtkMem.Target = 0;
                if (Type == Looks.Statue)
                {
                    GuildStatue.RemoveStatue(this);
                    return;
                }
                World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, 14).Get);
                Opened = true;
                ReSpawn();
            }
            else
            {
                CurHP -= Damage;
                World.Action(C, Packets.SkillUse(C.EntityID, EntityID, Damage, (ushort)C.SkillUses, 0, Loc.X, Loc.Y).Get);
            }
        }


        public class GuildStatue
        {
            //static string server = "localhost";
            //static string database = "characterinfo";
            //static string uid = "root";
            //static string password = "joao11x12";
            //static string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            // database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            //static MySqlCommand Cmd_MySQL;
            //static MySqlDataReader DataRead_MySQL;
            //static MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Throne"].ConnectionString);

            /// <summary>
            /// Gets the X,Y location for the statue
            /// </summary>
            /// <returns></returns>
            public static KeyValuePair<ushort, ushort> StatueLocations()
            {
                KeyValuePair<ushort, ushort> StatueLocation = new KeyValuePair<ushort, ushort>();
                Dictionary<byte, KeyValuePair<ushort, ushort>> Locations = new Dictionary<byte, KeyValuePair<ushort, ushort>>()
                {
                    {0, new KeyValuePair<ushort, ushort>(144,124)},
                    {1, new KeyValuePair<ushort, ushort>(130, 138)},
                    {2, new KeyValuePair<ushort, ushort>(130, 147)},
                    {3, new KeyValuePair<ushort, ushort>(153, 124)},
                    {4, new KeyValuePair<ushort, ushort>(130, 155)},
                    {5, new KeyValuePair<ushort, ushort>(161, 124)}
                };
                foreach (SOB S in World.H_SOBs.Values)
                    if (S.Type == Looks.Statue && S.Loc.Map == 1038)
                        foreach (KeyValuePair<byte, KeyValuePair<ushort, ushort>> Pairs in Locations.ToList())
                            if (S.Loc.X == Pairs.Value.Key && S.Loc.Y == Pairs.Value.Value)
                                Locations.Remove(Pairs.Key);

                for (int a = 0; a < 6; a++)
                    if (Locations.ContainsKey((byte)a))
                    {
                        StatueLocation = Locations[(byte)a];
                        break;
                    }
                return StatueLocation;
            }

            /// <summary>
            /// Adds the statue to the game server and clients using the placement method
            /// </summary>
            /// <param name="C"></param>
            /// <param name="N"></param>
            public static void AddStatue(Character C, byte[] N)
            {
                SOB Statue = new SOB();
                Statue.EntityID = 100001;
                while (World.H_SOBs.ContainsKey(Statue.EntityID))
                    Statue.EntityID = Statue.EntityID + 1;
                Statue.Name = C.Name;
                Statue.Mesh = C.Mesh;
                Statue.Type = Looks.Statue;
                if (C.MyGuild != null)
                    Statue.GuildID = C.MyGuild.GuildID;
                else
                    Statue.GuildID = 0;
                Statue.GuildRank = (byte)C.GuildRank;
                Statue.Headgear = C.Equips.HeadGear.ID;
                Statue.Necklace = C.Equips.Necklace.ID;
                Statue.Ring = C.Equips.Ring.ID;
                Statue.RightHand = C.Equips.RightHand.ID;
                Statue.Armor = C.Equips.Armor.ID;
                Statue.LeftHand = C.Equips.LeftHand.ID;
                Statue.Garment = C.Equips.Garment.ID;
                Statue.Hair = C.Hair;
                Statue.ArmorColor = (ushort)C.Equips.Armor.Color;
                Statue.LeftHandColor = (ushort)C.Equips.LeftHand.Color;
                Statue.HeadgearColor = (ushort)C.Equips.HeadGear.Color;

                Statue.Direction = N[12];
                Statue.Frame = N[14];
                Statue.Action = N[16];

                Statue.Loc = new Location();
                Statue.Loc.Map = C.Loc.Map;

                if (C.GuildRank == Features.GuildRank.GuildLeader)
                {
                    Statue.Loc.X = 140;
                    Statue.Loc.Y = 134;
                }
                else
                {
                    Statue.Loc.X = StatueLocations().Key;
                    Statue.Loc.Y = StatueLocations().Value;
                }
                if (Statue.Loc.X == 0 || Statue.Loc.Y == 0)
                {
                    C.MyClient.LocalMessage(2005, "The maximum numbers of statues have been spawned.");
                    return;
                }
                //if ((ushort) (Convert.ToUInt16(N[8]) + (N[9]*255) + N[9]) != 0)
                //{
                //    Statue.Loc.X = (ushort)(Convert.ToUInt16(N[8]) + (N[9] * 255) + N[9]);
                //    Statue.Loc.Y = (ushort)(Convert.ToUInt16(N[10]) + (N[11] * 255) + N[11]);
                //}
                //else
                //{
                //    Statue.Loc.X = (ushort) (C.Loc.X + 6);
                //    Statue.Loc.Y = C.Loc.Y;
                //}

                if (C.GuildRank == Features.GuildRank.GuildLeader)
                {
                    Statue.MaxHP = 50000;
                    Statue.CurHP = 50000;
                }
                else
                {
                    Statue.MaxHP = 15000;
                    Statue.CurHP = 15000;
                }

                if (!World.H_SOBs.ContainsKey(Statue.EntityID))
                {
                    if (C.InventoryContains(720020, 1))
                    {
                        C.RemoveItem(C.NextItem(720020));
                        World.H_SOBs.TryAdd(Statue.EntityID, Statue);
                        Statue.ReSpawn();
                    }
                    else
                        C.MyClient.LocalMessage(2005, "You don't have a StatueScroll");
                }
            }

            /// <summary>
            /// Adds a previously created statue to the game server
            /// </summary>
            /// <param name="Statue"></param>
            public static void AddStatue(SOB Statue)
            {
                if (!World.H_SOBs.ContainsKey(Statue.EntityID))
                {
                    World.H_SOBs.TryAdd(Statue.EntityID, Statue);
                    Statue.ReSpawn();
                }
            }

            /// <summary>
            /// Removes an existant statue to the game server
            /// </summary>
            /// <param name="S"></param>
            public static void RemoveStatue(SOB S)
            {
                World.Action(S, Packets.GeneralData(S.EntityID, 0, 0, 0, 135).Get);
                if (World.H_SOBs.ContainsKey(S.EntityID))
                    World.H_SOBs.Remove(S.EntityID);
                if (S.Database)
                    RemoveDatabase(S.EntityID);
            }

            /// <summary>
            /// Removes a statue entry from the database
            /// </summary>
            /// <param name="EntityID"></param>
            public static void RemoveDatabase(uint EntityID)
            {
                try
                {
                    MySQL.MySqlCommand SOB = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                    SOB.Delete("guildstatues", "uid", EntityID).Execute();
                    //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Throne"].ConnectionString);

                    //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                    //    Connect_MySQL.Open();

                    //string del = "Delete from guildstatues where uid=" + EntityID + "";
                    //Cmd_MySQL = new MySqlCommand(del, Connect_MySQL);
                    //Cmd_MySQL.ExecuteNonQuery();

                    //Connect_MySQL.Close();
                }
                catch (Exception e)
                {
                    World.ExcAdd += e + "\r\n";
                    Console.WriteLine(e);
                }
            }

            /// <summary>
            /// Adds a new entry to the database containing all the statue information
            /// </summary>
            /// <param name="Name"></param>
            /// <param name="Mesh"></param>
            /// <param name="EntityID"></param>
            /// <param name="GuildID"></param>
            /// <param name="GuildRank"></param>
            /// <param name="Headgear"></param>
            /// <param name="Necklace"></param>
            /// <param name="Ring"></param>
            /// <param name="RightHand"></param>
            /// <param name="LeftHand"></param>
            /// <param name="Armor"></param>
            /// <param name="Garment"></param>
            /// <param name="Hair"></param>
            /// <param name="Map"></param>
            /// <param name="X"></param>
            /// <param name="Y"></param>
            /// <param name="Frame"></param>
            /// <param name="Direction"></param>
            /// <param name="Action"></param>
            /// <param name="ArmorColor"></param>
            /// <param name="LeftHandColor"></param>
            /// <param name="HeadgearColor"></param>
            /// <param name="CurHP"></param>
            /// <param name="MaxHP"></param>
            public static void AddDatabase(string Name, uint Mesh, uint EntityID, ushort GuildID, byte GuildRank, uint Headgear, uint Necklace, uint Ring, uint RightHand, uint LeftHand, uint Armor, uint Garment, ushort Hair, ushort Map, ushort X, ushort Y, ushort Frame, byte Direction, byte Action, ushort ArmorColor, ushort LeftHandColor, ushort HeadgearColor, uint CurHP, uint MaxHP)
            {
                try
                {
                    MySQL.MySqlCommand Statues = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    Statues.Insert("guildstatues").Insert("uid", EntityID).Insert("name", Name).Insert("mesh", Mesh).Insert("guildid", GuildID).Insert("guildrank", GuildRank).Insert("headgear", Headgear).Insert("necklace", Necklace).Insert("ring", Ring).Insert("righthand", RightHand).Insert("lefthand", LeftHand).Insert("garment", Garment).Insert("armor", Armor).Insert("hair", Hair).Insert("map", Map).Insert("x", X).Insert("y", Y).Insert("frame", Frame).Insert("direction", Direction).Insert("action", Action).Insert("armorcolor", ArmorColor).Insert("lefthandcolor", LeftHandColor).Insert("headgearcolor", HeadgearColor).Insert("curhp", CurHP).Insert("maxhp", MaxHP).Execute();
                    //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Throne"].ConnectionString);

                    //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                    //    Connect_MySQL.Open();

                    //string History = "INSERT INTO guildstatues (uid,name,mesh,guildid,guildrank,headgear,necklace,ring,righthand,lefthand,garment,armor,hair,map,x,y,frame,direction,action,armorcolor,lefthandcolor,headgearcolor,curhp,maxhp) VALUES ('" + EntityID + "','" + Name + "','" + Mesh + "','" + GuildID + "','" + GuildRank + "','" + Headgear + "','" + Necklace + "','" + Ring + "','" + RightHand + "','" + LeftHand + "','" + Garment + "','" + Armor + "','" + Hair + "','" + Map + "','" + X + "','" + Y + "','" + Frame + "','" + Direction + "','" + Action + "','" + ArmorColor + "','" + LeftHandColor + "','" + HeadgearColor + "','" + CurHP + "','" + MaxHP + "')";
                    //Cmd_MySQL = new MySqlCommand(History, Connect_MySQL);
                    //Cmd_MySQL.ExecuteNonQuery();

                    //Connect_MySQL.Close();
                }
                catch (Exception e)
                {
                    World.ExcAdd += e + "\r\n";
                    Console.WriteLine(e);
                }
            }

            /// <summary>
            /// Called when the server is closed - saves all the statues
            /// </summary>
            public static void SaveStatues()
            {
                foreach (SOB Statue in World.H_SOBs.Values)
                {
                    if (Statue.Type == Looks.Statue && !Statue.Database)
                    {
                        try
                        {
                            AddDatabase(Statue.Name, Statue.Mesh, Statue.EntityID, Statue.GuildID, Statue.GuildRank, Statue.Headgear, Statue.Necklace, Statue.Ring, Statue.RightHand, Statue.LeftHand, Statue.Armor, Statue.Garment, Statue.Hair, (ushort)Statue.Loc.Map, Statue.Loc.X, Statue.Loc.Y, Statue.Frame, Statue.Direction, Statue.Action, Statue.ArmorColor, Statue.LeftHandColor, Statue.HeadgearColor, Statue.CurHP, Statue.MaxHP);
                        }
                        catch (Exception e)
                        {
                            World.ExcAdd += e + "\r\n";
                        }
                    }
                }
            }

            /// <summary>
            /// Called when the server is started - loads all the statues in the database and adds them to the game server
            /// </summary>
            public static void LoadStatues()
            {
                try
                {
                    MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("guildstatues");
                    MySQL.MySqlReader Statues = new MySQL.MySqlReader(Cmd);

                    while (Statues.Read())
                    {
                        SOB Statue = new SOB()
                        {
                            EntityID = Statues.ReadUInt32("uid"),
                            Name = Statues.ReadString("name"),
                            Mesh = Statues.ReadUInt32("mesh"),
                            GuildID = Statues.ReadUInt16("guildid"),
                            GuildRank = Statues.ReadByte("guildrank"),
                            Headgear = Statues.ReadUInt32("headgear"),
                            Necklace = Statues.ReadUInt32("necklace"),
                            Ring = Statues.ReadUInt32("ring"),
                            RightHand = Statues.ReadUInt32("righthand"),
                            LeftHand = Statues.ReadUInt32("lefthand"),
                            Garment = Statues.ReadUInt32("garment"),
                            Armor = Statues.ReadUInt32("armor"),
                            Hair = Statues.ReadUInt16("hair"),
                            Type = Looks.Statue,
                            Database = true,
                            Loc = new Location()
                            {
                                Map = Statues.ReadUInt32("map"),
                                X = Statues.ReadUInt16("x"),
                                Y = Statues.ReadUInt16("y")
                            },
                            Frame = Statues.ReadUInt16("frame"),
                            Direction = Statues.ReadByte("direction"),
                            Action = Statues.ReadByte("action"),
                            ArmorColor = Statues.ReadUInt16("armorcolor"),
                            LeftHandColor = Statues.ReadUInt16("lefthandcolor"),
                            HeadgearColor = Statues.ReadUInt16("headgearcolor"),
                            CurHP = Statues.ReadUInt32("curhp"),
                            MaxHP = Statues.ReadUInt32("maxhp")
                        };

                        if (!World.H_SOBs.ContainsKey(Statue.EntityID))
                        {
                            World.H_SOBs.TryAdd(Statue.EntityID, Statue);
                            Statue.ReSpawn();
                        }
                    }


                    //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Throne"].ConnectionString);

                    //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                    //    Connect_MySQL.Open();

                    //string nr = "Select COUNT(*) FROM guildstatues";
                    //Cmd_MySQL = new MySqlCommand(nr, Connect_MySQL);
                    //var nrLines = Convert.ToInt32(Cmd_MySQL.ExecuteScalar().ToString());
                    //if (nrLines == 0)
                    //{
                    //    Cmd_MySQL.Dispose();
                    //    return;
                    //}
                    //else
                    //    Cmd_MySQL.Dispose();
                    ////for (int a = 0; a < nrLines; a++)
                    ////{
                    //string History = "SELECT uid,name,mesh,guildid,guildrank,headgear,necklace,ring,righthand,lefthand,garment,armor,hair,map,x,y,frame,direction,action,armorcolor,lefthandcolor,headgearcolor,curhp,maxhp FROM guildstatues";
                    //Cmd_MySQL = new MySqlCommand(History, Connect_MySQL);
                    //DataRead_MySQL = Cmd_MySQL.ExecuteReader();

                    //while (DataRead_MySQL.Read())
                    //{
                    //    SOB Statue = new SOB();
                    //    Statue.EntityID = Convert.ToUInt32(DataRead_MySQL.GetString(0));
                    //    Statue.Name = DataRead_MySQL.GetString(1);
                    //    Statue.Mesh = Convert.ToUInt32(DataRead_MySQL.GetString(2));
                    //    Statue.GuildID = Convert.ToUInt16(DataRead_MySQL.GetString(3));
                    //    Statue.GuildRank = Convert.ToByte(DataRead_MySQL.GetString(4));
                    //    Statue.Headgear = Convert.ToUInt32(DataRead_MySQL.GetString(5));
                    //    Statue.Necklace = Convert.ToUInt32(DataRead_MySQL.GetString(6));
                    //    Statue.Ring = Convert.ToUInt32(DataRead_MySQL.GetString(7));
                    //    Statue.RightHand = Convert.ToUInt32(DataRead_MySQL.GetString(8));
                    //    Statue.LeftHand = Convert.ToUInt32(DataRead_MySQL.GetString(9));
                    //    Statue.Garment = Convert.ToUInt32(DataRead_MySQL.GetString(10));
                    //    Statue.Armor = Convert.ToUInt32(DataRead_MySQL.GetString(11));
                    //    Statue.Hair = Convert.ToUInt16(DataRead_MySQL.GetString(12));
                    //    Statue.Type = Looks.Statue;
                    //    Statue.Database = true;

                    //    Statue.Loc.Map = Convert.ToUInt32(DataRead_MySQL.GetString(13));
                    //    Statue.Loc.X = Convert.ToUInt16(DataRead_MySQL.GetString(14));
                    //    Statue.Loc.Y = Convert.ToUInt16(DataRead_MySQL.GetString(15));

                    //    Statue.Frame = Convert.ToUInt16(DataRead_MySQL.GetString(16));
                    //    Statue.Direction = Convert.ToByte(DataRead_MySQL.GetString(17));
                    //    Statue.Action = Convert.ToByte(DataRead_MySQL.GetString(18));

                    //    Statue.ArmorColor = Convert.ToUInt16(DataRead_MySQL.GetString(19));
                    //    Statue.LeftHandColor = Convert.ToUInt16(DataRead_MySQL.GetString(20));
                    //    Statue.HeadgearColor = Convert.ToUInt16(DataRead_MySQL.GetString(21));

                    //    Statue.CurHP = Convert.ToUInt32(DataRead_MySQL.GetString(22));
                    //    Statue.MaxHP = Convert.ToUInt32(DataRead_MySQL.GetString(23));
                    //    if (!World.H_SOBs.ContainsKey(Statue.EntityID))
                    //    {
                    //        World.H_SOBs.TryAdd(Statue.EntityID, Statue);
                    //        Statue.ReSpawn();
                    //    }
                    //}

                    //DataRead_MySQL.Close();
                    //Connect_MySQL.Close();
                    //}
                }
                catch (Exception e)
                {
                    World.ExcAdd += e + "\r\n";
                    Console.WriteLine(e);
                }
            }
        }
    }
}
