using MySql.Data.MySqlClient;
using Ultimate.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ultimate.Features
{
    public class HouseTable
    {
        #region Properties
        //static string server = "localhost";
        //static string database = "characterinfo";
        //static string uid = "root";
        //static string password = "joao11x12";
        //static readonly string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        // database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        //static MySqlCommand Cmd_MySQL;
        //static MySqlDataReader DataRead_MySQL;
        //static MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);
#endregion

        /// <summary>
        /// Called when the server is started - loads all the furnitures in the database
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="LHouse"></param>
        public static void LoadFurnitures()  
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("furniture");
                MySQL.MySqlReader Furniture = new MySQL.MySqlReader(Cmd);

                while (Furniture.Read())
                {
                    //string History = "SELECT uid,npcid,id,type,x,y FROM furniture"/* WHERE no= + nrChars[a]*/;
                    AddFurniture(Furniture.ReadUInt32("uid"), Furniture.ReadString("npcid"), Furniture.ReadString("id"), Furniture.ReadString("type"), Furniture.ReadString("x"), Furniture.ReadString("y"), true);
                    World.Furnitures.Add(Furniture.ReadUInt32("npcid"));
                }
                //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);

                //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                //    Connect_MySQL.Open();

                //string nr = "Select COUNT(*) FROM furniture";
                //Cmd_MySQL = new MySqlCommand(nr, Connect_MySQL);

                //var nrLines = Convert.ToInt32(Cmd_MySQL.ExecuteScalar().ToString());
                //if (nrLines == 0)
                //{
                //    Cmd_MySQL.Dispose();
                //    return;
                //}
                //else
                //    Cmd_MySQL.Dispose();


                //var nrChars = new int[nrLines];
                //string ids = "SELECT no FROM furniture";
                //Cmd_MySQL = new MySqlCommand(ids, Connect_MySQL);
                //DataRead_MySQL = Cmd_MySQL.ExecuteReader();
                //int NrLineChar = 0;
                //while (DataRead_MySQL.Read())
                //{
                //    nrChars[NrLineChar] = Convert.ToInt32(DataRead_MySQL.GetString(0));
                //    NrLineChar++;
                //}
                //DataRead_MySQL.Close();

                //for (int a = 0; a < nrLines; a++)
                //{
                //string History = "SELECT uid,npcid,id,type,x,y FROM furniture"/* WHERE no= + nrChars[a]*/;
                //    Cmd_MySQL = new MySqlCommand(History, Connect_MySQL);
                //    DataRead_MySQL = Cmd_MySQL.ExecuteReader();

                //    string[] Total = new string[6];
                //    while (DataRead_MySQL.Read())
                //    {
                //        Total[0] = DataRead_MySQL.GetString(0);
                //        Total[1] = DataRead_MySQL.GetString(1);
                //        Total[2] = DataRead_MySQL.GetString(2);
                //        Total[3] = DataRead_MySQL.GetString(3);
                //        Total[4] = DataRead_MySQL.GetString(4);
                //        Total[5] = DataRead_MySQL.GetString(5);
                //        AddFurniture(Convert.ToUInt32(Total[0]), Total[1], Total[2], Total[3], Total[4], Total[5], true);
                //        World.Furnitures.Add(Convert.ToUInt32(Total[1]));

                //        //if (Convert.ToUInt32(Total[0]) > 1000000)
                //        //{
                //        //    if (DMaps.MapOwner.ContainsKey(Convert.ToUInt32(Total[0])))
                //        //    {
                //        //        AddFurniture(Convert.ToUInt32(Total[0]), "2100", "8206", "2", "20", "40", true);
                //        //        if (DMaps.HouseUpgrade(Convert.ToUInt32(Total[0])))
                //        //            AddFurniture(Convert.ToUInt32(Total[0]), "2101", "8206", "2", "32", "38", true);
                //        //    }
                //        //}
                //    }


                //    DataRead_MySQL.Close();
                ////}
                //Connect_MySQL.Close();
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Adds a furniture to the game server and spawns it to nearby players with all its info
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="NPCID"></param>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Loaded"></param>
        public static void AddFurniture(uint UID, string NPCID, string ID, string Type, string X, string Y, bool Loaded = false)
        {
            if (DMaps.H_DMaps.ContainsKey(Convert.ToUInt32(UID)))
            {
                DMap D = (DMap)DMaps.H_DMaps[Convert.ToUInt32(UID)];
                NPC NPCInfo = new NPC();
                NPCInfo.EntityID = Convert.ToUInt32(NPCID);
                NPCInfo.Type = Convert.ToUInt16(ID);
                NPCInfo.Flags = Convert.ToByte(Type);
                NPCInfo.Avatar = 188;
                NPCInfo.Loc = new Location();
                NPCInfo.Loc.Map = UID;

                NPCInfo.Loc.X = Convert.ToUInt16(X);
                NPCInfo.Loc.Y = Convert.ToUInt16(Y);

                NPCInfo.Direction = 0;

                if (Loaded)
                    NPCInfo.Database = true;

                if (!World.H_NPCs.ContainsKey(NPCInfo.Loc.Map))
                {
                    World.H_NPCs.Add(NPCInfo.Loc.Map, new Dictionary<uint, NPC>());
                }
                Dictionary<uint, NPC> NPCMap = World.H_NPCs[NPCInfo.Loc.Map];
                if (!NPCMap.ContainsKey(NPCInfo.EntityID))
                {
                    NPCMap.Add(NPCInfo.EntityID, NPCInfo);
                    World.Spawn(NPCInfo);
                }
            }
        }

        /// <summary>
        /// Adds a furniture to the game server and spawns it to nearby players using the placement method
        /// </summary>
        /// <param name="C"></param>
        /// <param name="N"></param>
        public static void AddFurniture(Character C, byte[] N)
        {
            if (C.Loc.Map == C.EntityID || C.MyClient.AuthInfo.Status == "[PM]")
            {
                DMap D = (DMap)DMaps.H_DMaps[Convert.ToUInt32(C.Loc.Map)];
                NPC NPCInfo = new NPC();
                NPCInfo.EntityID = Convert.ToUInt16(Program.Rnd.Next(20007, 29999));
                NPCInfo.Type = (ushort)(N[12] + N[13] + (255 * N[13]));
                NPCInfo.Flags = Convert.ToByte(N.Length - 2);
                NPCInfo.Avatar = N[4];
                NPCInfo.Loc = new Location();
                NPCInfo.Loc.Map = C.Loc.Map;

                NPCInfo.Loc.X = (ushort)(Convert.ToUInt16(N[8]) + (N[9] * 255) + N[9]);
                NPCInfo.Loc.Y = (ushort)(Convert.ToUInt16(N[10]) + (N[11] * 255) + N[11]);

                if ((D != null && D.GetCell(NPCInfo.Loc.X, NPCInfo.Loc.Y).NoAccess && NPCInfo.Type != 570) && C.MyClient.AuthInfo.Status != "[PM]")
                {
                    C.MyClient.LocalMessage(2005, "You can't place the furniture here!");
                    return;
                }

                World.Furnitures.Add(NPCInfo.EntityID);

                while (World.Furnitures.Contains(NPCInfo.EntityID))
                    NPCInfo.EntityID = Convert.ToUInt16(Program.Rnd.Next(20007, 29999));

                if (!World.H_NPCs.ContainsKey(NPCInfo.Loc.Map))
                {
                    World.H_NPCs.Add(NPCInfo.Loc.Map, new Dictionary<uint, NPC>());
                }
                Dictionary<uint, NPC> NPCMap = World.H_NPCs[NPCInfo.Loc.Map];
               
                
                if (!NPCMap.ContainsKey(NPCInfo.EntityID))
                {
                    if ((DMaps.HouseLevel(C.EntityID) == 2 && NPCMap.Count < 12) || (DMaps.HouseLevel(C.EntityID) == 1 && NPCMap.Count < 5) || C.MyClient.AuthInfo.Status == "[PM]")
                    {
                        uint ID = 0;
                        if (NPCInfo.Type == 230)
                            ID = 721228;
                        else if (NPCInfo.Type == 220)
                            ID = 721227;
                        else if (NPCInfo.Type == 210)
                            ID = 721226;
                        else if (NPCInfo.Type == 200)
                            ID = 721225;
                        else if (NPCInfo.Type == 190)
                            ID = 721235;
                        else if (NPCInfo.Type == 180)
                            ID = 721234;
                        else if (NPCInfo.Type == 170)
                            ID = 721233;
                        else if (NPCInfo.Type == 160)
                            ID = 721232;
                        else if (NPCInfo.Type == 150)
                            ID = 721231;
                        else if (NPCInfo.Type == 140)
                            ID = 721230;
                        else if (NPCInfo.Type == 130)
                            ID = 721229;
                        else if (NPCInfo.Type == 540)
                            ID = 720392;
                        else if (NPCInfo.Type == 570)
                            ID = 720391;
                        else if (NPCInfo.Type == 121)
                            ID = 721188;
                        else if (NPCInfo.Type == 41)
                            ID = 721180;
                        else if (NPCInfo.Type == 111)
                            ID = 721187;
                        else if (NPCInfo.Type == 101)
                            ID = 721186;
                        else if (NPCInfo.Type == 91)
                            ID = 721185;
                        else if (NPCInfo.Type == 81)
                            ID = 721184;
                        else if (NPCInfo.Type == 71)
                            ID = 721183;
                        else if (NPCInfo.Type == 61)
                            ID = 721182;
                        else if (NPCInfo.Type == 51)
                            ID = 721181;
                        else if (NPCInfo.Type == 37)
                            ID = 721179;
                        else if (NPCInfo.Type == 21)
                            ID = 721178;
                        else if (NPCInfo.Type == 10)
                            ID = 721177;
                        else if (NPCInfo.Type == 410)
                            ID = 720164;
                        else if (NPCInfo.Type == 380)
                            ID = 720165;
                        else if (NPCInfo.Type == 390)
                            ID = 720166;
                        else if (NPCInfo.Type == 400)
                            ID = 720167;

                        C.RemoveItem(C.NextItem(ID));
                        NPCMap.Add(NPCInfo.EntityID, NPCInfo);
                        World.Spawn(NPCInfo);
                        //AddDatabase(C.EntityID, NPCInfo.EntityID, NPCInfo.Type, NPCInfo.Flags, NPCInfo.Loc.X, NPCInfo.Loc.Y);
                        World.Furnitures.Add(NPCInfo.EntityID);
                    }
                    else
                        C.MyClient.LocalMessage(2005, "You can't have more furnitures in your current house!");
                }
            }
            else
                C.MyClient.LocalMessage(2005, "You can only place furniture inside your house.");
        }

        /// <summary>
        /// Called when the server is closed to save all the furnitures that are not in the database
        /// </summary>
        public static void SaveFurnitures()
        {

            foreach (Dictionary<uint, NPC> N2 in World.H_NPCs.Values)
            {
                foreach (uint N in World.Furnitures)
                {
                    if (N2.ContainsKey(N) && !N2[N].Database)
                        AddDatabase(N2[N].Loc.Map, N2[N].EntityID, N2[N].Type, N2[N].Flags, N2[N].Loc.X, N2[N].Loc.Y);
                }
            }
        }
        
        /// <summary>
        /// Adds the furniture to the database
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="NPCID"></param>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void AddDatabase(uint UID, uint NPCID, uint ID, int Type, ushort X, ushort Y)
        {
            try
            {
                MySQL.MySqlCommand Furniture = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                Furniture.Insert("furniture").Insert("uid", UID).Insert("npcid", NPCID).Insert("id", ID).Insert("type", Type).Insert("x", X).Insert("y", Y).Execute();
                //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);

                //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                //    Connect_MySQL.Open();

                //string History = "INSERT INTO furniture (uid,npcid,id,type,x,y) VALUES ('" + UID + "','" + NPCID + "','" + ID + "','" + Type + "','" + X + "','" + Y + "')";
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
        /// Initializes the Furniture Removal by sending a popup asking the player if he wants to remove the furniture
        /// </summary>
        /// <param name="C"></param>
        /// <param name="UID"></param>
        public static void InitializeRemoval(Character C, uint UID)
        {
            C.RemoveFurniture = UID;
            C.MyClient.DialogNPC = 2054;
            NPCs.NPCHandler.Handle(C.MyClient, null, 2054, 0);
        }

        /// <summary>
        /// Finalizes the Furniture Removal process - removes from both server, clients and database
        /// </summary>
        /// <param name="C"></param>
        /// <param name="UID"></param>
        public static void RemoveFurniture(Character C, uint UID)
        {
            NPC N = null;
            Dictionary<uint, NPC> MapNPC = World.H_NPCs[C.Loc.Map];
            if (MapNPC != null)
                if (MapNPC.ContainsKey(UID))
                    N = (NPC)MapNPC[UID];
                else
                    return;
            else
                return;

            MapNPC.Remove(UID);
            World.Action(N, Packets.GeneralData(UID, 0, 0, 0, 135).Get);
            C.MyClient.AddSend(Packets.Remove(N));
            if (N.Database)
                RemoveDatabase(UID);
            if (World.Furnitures.Contains(UID))
                World.Furnitures.Remove(UID);
        }

        /// <summary>
        /// Called when a furniture that have been loaded from the database is removed and removes its entry from the database
        /// </summary>
        /// <param name="ID"></param>
        public static void RemoveDatabase(uint ID)
        {
            try
            {
                MySQL.MySqlCommand Furniture = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                Furniture.Delete("furniture", "npcid", ID).Execute();
                //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);

                //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                //    Connect_MySQL.Open();

                //string del = "Delete from furniture where npcid=" + ID + "";
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
    }
}