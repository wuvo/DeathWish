using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.PacketHandling
{
    public class CustomDialog
    {
        public int AniId;
        public ushort x;
        public ushort y;
        public ushort Width;
        public ushort Height;
        public bool Permanent;
        public bool PopUp;
        public bool SystemMenu;
        public ushort ButtonCount;
        public List<DlgBtnData> Buttons = new List<DlgBtnData>();
        
        public struct DlgBtnData
        {
            public int ButtonUID;
            public int AniId;
            public ushort AniWidth;
            public ushort AniHeight;
            public ushort xpos;
            public ushort ypos;
            public ushort Width;
            public ushort Height;
            public int TipColor;
            public string TipStr; //64 chars long
        };

        /// <summary>
        /// Loads all the dialogs from mysql database and adds them to the World.Dialogs Dictionary
        /// </summary>
        /// <param name="GC"></param>
        public static void GetDialogs(Main.GameClient GC)
        {
            foreach (CustomDialog Dialog in Game.World.Dialogs.Values)
            {
                double HeightPercentage = (Dialog.y * 100.0) / 1080;
                double WidthPercentage = (Dialog.x * 100.0) / 1920;
                
                var a = (GC.AuthInfo.Width - 1024) / 2;//478
                var b = (1920 - 1024) / 2;//448
                var c = b - a;//-30

                double WidthMultiplier = ((GC.AuthInfo.Width - (1920 - Dialog.x - c)) * 100.0) / GC.AuthInfo.Width;
                WidthPercentage = WidthMultiplier;

                if (HeightPercentage > 50)
                {
                    double HeightMultiplier = ((GC.AuthInfo.Height - (1080 - Dialog.y)) * 100.0) / GC.AuthInfo.Height;
                    HeightPercentage = HeightMultiplier;
                }
                if (WidthPercentage > 50 && GC.AuthInfo.Width == 800)
                {
                    WidthPercentage = WidthPercentage - 12;
                }
                //if (WidthPercentage > 50)
                //{
                //    //var a = (GC.AuthInfo.Width - 1024) / 2;//478
                //    //var b = (1920 - 1024) / 2;//448
                //    //var c = b - a;//-30

                //    double testingTwo = ((GC.AuthInfo.Width - (1920 - Dialog.x - c)) * 100.0) / GC.AuthInfo.Width;
                //    WidthPercentage = testingTwo;
                //}

                double toDivideWidth = (1920.0 / Dialog.x);
                double toDivideHeight = (1080.0 / Dialog.y);
                
                ushort X = (ushort)((GC.AuthInfo.Width) * (WidthPercentage / 100.0));
                ushort Y = (ushort)((GC.AuthInfo.Height) * (HeightPercentage / 100.0));
                X = (ushort)(X - (Dialog.Width / 2));
                Y = (ushort)(Y - (Dialog.Height / 2));
                //if (Dialog.x > 1000)
                //{

                //}
                GC.AddSend(Packets.CustomDialog(Dialog, X, Y));
            }
            //for (int a = 1; a < 7; a++)
            //{
            //    CustomDialog Dialog = new CustomDialog();
            //    Dialog.AniId = a;
            //    Dialog.Width = 424;
            //    Dialog.Height = 497;
            //    Dialog.x = (ushort)((GC.AuthInfo.Width / 2) - (Dialog.Width / 2));
            //    Dialog.y = (ushort)((GC.AuthInfo.Height / 2) - (Dialog.Height / 2));
            //    Dialog.Permanent = true;
            //    Dialog.PopUp = false;
            //    Dialog.SystemMenu = true;
            //    Dialog.ButtonCount = 1;
            //    Dialog.Buttons = new List<CustomDialog.DlgBtnData>();
            //    CustomDialog.DlgBtnData Button = new CustomDialog.DlgBtnData();
            //    if (Dialog.AniId == 1)
            //    {
            //        Button.AniId = 10100;
            //        Button.AniWidth = 121;
            //        Button.xpos = (ushort)((Dialog.Width / 10) + (Button.AniWidth / 8));
            //    }
            //    else
            //    {
            //        Button.AniId = 10102;
            //        Button.AniWidth = 112;
            //        Button.xpos = (ushort)((Dialog.Width / 2) - (Button.AniWidth / 2));
            //    }
            //    Button.AniHeight = 29;
            //    Button.ypos = (ushort)((Dialog.Height / 2) + (Dialog.Height / 3));
            //    Button.Width = Button.AniWidth;
            //    Button.Height = 29;
            //    Button.TipColor = 0xFF0000;
            //    Button.TipStr = "";
            //    Dialog.Buttons.Add(Button);
            //    Game.World.Dialogs.Add(Dialog.AniId, Dialog);

            //    GC.AddSend(Packets.CustomDialog(Dialog));
            //}
        }

        /// <summary>
        /// Loads all buttons from mysql database and adds them to their respective dialog
        /// </summary>
        public static void LoadDialogs()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("customdialogs");
                MySQL.MySqlReader Dialogs = new MySQL.MySqlReader(Cmd);

                while (Dialogs.Read())
                {
                    CustomDialog Dialog = new CustomDialog()
                    {
                        AniId = Dialogs.ReadInt32("UID"),
                        x = Dialogs.ReadUInt16("X"),
                        y = Dialogs.ReadUInt16("Y"),
                        Width = Dialogs.ReadUInt16("Width"),
                        Height = Dialogs.ReadUInt16("Height"),
                        Permanent = Dialogs.ReadBoolean("Permanent"),
                        PopUp = Dialogs.ReadBoolean("PopUp"),
                        SystemMenu = Dialogs.ReadBoolean("SystemMenu"),
                        ButtonCount = Dialogs.ReadUInt16("ButtonCount")
                    };

                    MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("custombuttons").Where("DialogID", Dialog.AniId);
                    MySQL.MySqlReader Buttons = new MySQL.MySqlReader(Cmd2);

                    while (Buttons.Read())
                    {
                        DlgBtnData Button = new DlgBtnData()
                        {
                            ButtonUID = Buttons.ReadInt32("UID"),
                            AniId = Buttons.ReadInt32("UID"),
                            AniWidth = Buttons.ReadUInt16("AniWidth"),
                            AniHeight = Buttons.ReadUInt16("AniHeight"),
                            xpos = Buttons.ReadUInt16("xpos"),
                            ypos = Buttons.ReadUInt16("ypos"),
                            Width = Buttons.ReadUInt16("Width"),
                            Height = Buttons.ReadUInt16("Height"),
                            TipColor = Buttons.ReadInt32("TipColor"),
                            TipStr = Buttons.ReadString("TipStr")
                        };
                        Dialog.Buttons.Add(Button);
                    }
                    Game.World.Dialogs.Add(Dialog.AniId, Dialog);
                }
            }
            catch (Exception e)
            {
                Game.World.ExcAdd += e + "\r\n";
            }
        }

        /// <summary>
        /// Handles the usage for custom buttons
        /// </summary>
        /// <param name="C"></param>
        /// <param name="ID"></param>
        public static void HandleButtons(Game.Character C, uint ID, int DialogID)
        {
            if (ID >= 1000001)
            {
                if (DialogID == 30)
                {
                    if (Game.World.H_Chars.ContainsKey(ID) && C.MyTeam.Members.Count < 6 && !C.MyTeam.Members.Contains(ID))
                    {
                        if (!((C.EventBase != null && (C.EventBase.MapEvent == C.Loc.Map || C.Loc.Map == 1616 || C.Loc.Map == 2068)) || (C.Arena != null && C.Arena.MapID == C.Loc.Map) || (DMaps.EventMaps.ContainsKey(C.EntityID))))
                        {
                            Game.Character C2 = Game.World.H_Chars[ID];
                            if (!((C2.EventBase != null && (C2.EventBase.MapEvent == C2.Loc.Map || C2.Loc.Map == 1616 || C2.Loc.Map == 2068)) || (C2.Arena != null && C2.Arena.MapID == C2.Loc.Map) || (DMaps.EventMaps.ContainsKey(C.EntityID))))
                            {
                                if (C2.noobPlvl)
                                {
                                    C2.noobPlvl = false;
                                    C2.Teleport(C.Loc.Map, C.Loc.X, C.Loc.Y);
                                    C.MyTeam.Joins(C2);
                                }
                            }
                        }
                    }
                    C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                }
                else
                {
                    if (Game.World.H_Chars.ContainsKey(ID))
                        C.MyClient.AddSend(Packets.SpawnViewed(Game.World.H_Chars[ID], 1));
                }
            }
            else if (NPCs.NPC_2.RegularGarments.Contains(ID) || NPCs.NPC_2.RareGarments.Contains(ID) || NPCs.NPC_2.SpecialGarments.Contains(ID))
            {
                NPCs.NPC_2.Confirmation(C, (uint)DialogID, ID);
                C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
            }
            else
                switch (ID)
                {
                    case 1000:
                        break;
                    case 10100:
                        C.Invitations = true;
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10102:
                        if (C.EventBase == null)
                        {
                            if (DialogID == 18)
                                C.Teleport(1002, 452, 294);
                            else if (DialogID == 19)
                                C.Teleport(1002, 430, 247);
                            else
                            {
                                foreach (Events.Events E in Game.World.Events)
                                    if (E.DialogID == DialogID)
                                        if (E.AddPlayer(C))
                                            C.EventBase = E;
                            }
                        }
                        else
                            C.MyClient.LocalMessage(2000, "You have already joined an event!");
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10103:
                        C.Invitations = false;
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10104:
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "DlgClose"));
                        if (DialogID == 26)
                        {
                            if (C.Level >= 15)
                            {
                                C.MyClient.AddSend(Packets.ShowDialog(1, 1));
                                C.MyClient.AddSend(Packets.ShowDialog(21, 1));
                            }
                        }
                        break;
                    case 10105:
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10106:
                    case 10108:
                        Features.ArenaQualifier.AddPlayer(C);
                        C.MyClient.AddSend(Packets.RemoveButton(20, 10106));
                        if (ID == 10106)
                            Features.ArenaQualifier.WindowInformation(C, 20);
                        else
                            C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10107:
                        if (C.ArenaQualifier != null && C.ArenaQualifier.Status == Features.MatchStatus.Fighting)
                            C.MyClient.AddSend(Packets.ShowDialog(25, 1));

                        else if (!(C.ArenaQualifier != null && C.ArenaQualifier.Status == Features.MatchStatus.Finish))
                        {
                            C.MyClient.AddSend(Packets.ShowDialog(20, 1));
                            //Features.ElitePKStats.WindowInformation(C);
                            Features.ArenaQualifier.WindowInformation(C, 20);
                        }
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        break;
                    case 10109:
                    case 10111:
                        if (C.ArenaQualifier != null)
                        {
                            //C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                            if (Features.ArenaQualifier.PlayersInWaiting.ContainsKey(C.EntityID) && C.ArenaQualifier.Status == Features.MatchStatus.None)
                            {
                                Features.ArenaQualifier.PlayersInWaiting.Remove(C.EntityID);
                                C.ArenaQualifier = null;
                            }
                            else if (C.ArenaQualifier.Status == Features.MatchStatus.Countdown || C.ArenaQualifier.Status == Features.MatchStatus.WaitingForOpponent || C.ArenaQualifier.Status == Features.MatchStatus.Fighting)
                                C.ArenaQualifier.RemovePlayer(C, true);
                        }
                        if (ID == 10111)
                        {
                            C.MyClient.AddSend(Packets.RemoveButton(20, 10111));
                            Features.ArenaQualifier.WindowInformation(C, 20);
                        }
                        else
                            C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10110:
                        if (C.ArenaQualifier != null)
                        {
                            if (C.Alive)
                            {
                                if (C.ArenaQualifier.Opponent != null && C.ArenaQualifier.Opponent.ArenaQualifier != null)
                                    C.ArenaQualifier.AcceptMatch(C);
                                else
                                {
                                    if (C.ArenaQualifier.Opponent != null)
                                    {
                                        var EntityID = C.ArenaQualifier.Opponent.EntityID;
                                        Game.World.H_Chars[EntityID].ArenaQualifier = null;
                                        Features.ArenaQualifier.AddPlayer(Game.World.H_Chars[EntityID]);
                                    }
                                    C.ArenaQualifier = null;
                                    Features.ArenaQualifier.AddPlayer(C);
                                }
                                C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                            }
                            else
                                C.MyClient.LocalMessage(2005, "You can't fight as a ghost!");
                        }
                        else
                        {
                            Features.ArenaQualifier.AddPlayer(C);
                            C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        }
                        break;
                    case 10113:
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        C.MyClient.AddSend(Packets.RemoveButton(DialogID, -1));
                        C.MyClient.AddSend(Packets.DelDynImg(DialogID, -1));
                        C.MyClient.AddSend(Packets.DelDynTxt(DialogID, -1));
                        Features.ElitePKStats.WindowInformation(C);
                        break;
                    case 10114:
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        C.MyClient.AddSend(Packets.RemoveButton(DialogID, -1));
                        C.MyClient.AddSend(Packets.DelDynImg(DialogID, -1));
                        C.MyClient.AddSend(Packets.DelDynTxt(DialogID, -1));
                        Features.ArenaQualifier.WindowInformation(C, (uint)DialogID);
                        break;
                    case 10115:
                        C.MyClient.AddSend(Packets.ShowDialog(28, 1));
                        Features.Anniversary.WindowInformation(C, 28);
                        break;
                    case 10119:
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        C.MyClient.AddSend(Packets.ShowDialog(29, 1));
                        C.ArenaPage = 0;
                        Features.ArenaQualifier.WindowInformation(C, 29, 0);
                        break;
                    case 10122:
                        if (C.ArenaPage > 0)
                            C.ArenaPage--;
                        else
                            return;
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "nextmenu2"));
                        WindowsInformation(C, ID, DialogID);
                        break;
                    case 10123:
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "nextmenu2"));
                        C.ArenaPage++;
                        WindowsInformation(C, ID, DialogID);
                        if (DialogID == 20)
                            C.MyClient.AddSend(Packets.RemoveButton(DialogID, -1));
                        //Features.ArenaQualifier.WindowInformation(C, (uint)DialogID, C.ArenaPage);
                        break;
                    case 10124:
                        if (C.Garment == 0)
                            Features.ArenaQualifier.Garment(C, 183275, true);
                        else
                            Features.ArenaQualifier.Garment(C, 183275, false);

                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10125:
                        if (C.Version > 1039)
                        {
                            C.MyClient.AddSend(Packets.DelDynTxt(DialogID, -1));
                            C.Version--;
                            Features.Changelog.WindowsInformation(C, (uint)26, C.Version, false);
                        }
                        //else
                        //    Features.Changelog.WindowsInformation(C, (uint)26, 1039, false);
                        break;
                    case 10126:
                        if (C.Version < (Game.World._serverVersion - 1))
                        {
                            C.MyClient.AddSend(Packets.DelDynTxt(DialogID, -1));
                            C.Version++;
                            Features.Changelog.WindowsInformation(C, (uint)26, C.Version, false);
                        }
                        break;
                    case 10127:
                        C.MyClient.LocalMessage(2105, "http://www.facebook.com/Ultimateconquerfb");
                        break;
                    case 10128:
                        if (C.MyTeam != null && C.TeamLeader && !Game.World.Archers.ContainsKey(C.EntityID))
                            Game.World.Archers.TryAdd(C.EntityID, C);
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10129:
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        Features.PowerLevel_Request.WindowInformation(C, 30);
                        //C.MyClient.AddSend(Packets.ShowDialog(30, 1));
                        break;
                    case 10130:
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;
                    case 10131:
                    case 10132:
                    case 10133:
                        C.ArenaPage = 0;
                        NPCs.NPC_2.WindowsInformation(C, 32, ID);
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        break;
                    case 10134:
                        NPCs.NPC_2.Confirmation(C, 33, ID);
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        break;
                    case 10137:
                        C.MyClient.AddSend(Packets.ShowDialog(33, 0));
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        //NPCs.NPC_2.WindowsInformation(C, 32, C.CostumerPage, C.ArenaPage);
                        break;
                    case 10139:
                        C.MyClient.AddSend(Packets.ShowDialog(33, 1));
                        NPCs.NPC_2.Confirmation(C, 33, ID);
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        break;
                    case 10140:
                    case 10141:
                        C.Female = !C.Female;
                        C.ArenaPage = 0;
                        NPCs.NPC_2.WindowsInformation(C, 32, C.CostumerPage);
                        C.MyClient.AddSend(Packets.StringPacket(C.Loc.X, C.Loc.Y, Game.StringType.Sound, "BtnClick"));
                        break;
                    case 10142:
                        if (Game.World.CurrentBoss == "ThrillingSpook")
                            C.Teleport(1015, 710, 925);
                        else if (Game.World.CurrentBoss == "Capricorn")
                            C.Teleport(1011, 799, 465);
                        else if (Game.World.CurrentBoss == "Tash")
                            C.Teleport(1000, 496, 301);
                        else if (Game.World.CurrentBoss == "Raikou")
                            C.Teleport(1002, 375, 415);

                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));

                        break;
                    case 10143:
                        C.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                        break;

                    default:
                        C.MyClient.LocalMessage(2000, "Invalid button interaction. ID: " + ID);
                        break;
                }
        }

        public static void WindowsInformation(Game.Character C, uint ButtonID, int DialogID)
        {
            switch (DialogID)
            {
                case 20:
                case 29:
                    Features.ArenaQualifier.WindowInformation(C, (uint)DialogID, C.ArenaPage);
                    break;
                case 32:
                    NPCs.NPC_2.WindowsInformation(C, (uint)DialogID, ButtonID, C.ArenaPage);
                    break;
            }
        }

        public static float MeasureStringMin(string Text, float Size)
        {
            //set font, size & style
            System.Drawing.Font f = new System.Drawing.Font("Arial", Size);

            //create a bmp / graphic to use MeasureString on
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(2200, 2200);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);

            //measure the string
            System.Drawing.SizeF sizeOfString = new System.Drawing.SizeF();
            sizeOfString = g.MeasureString(Text, f);

            return sizeOfString.Width / 2;
        }
    }
    public class MSG_DLG_IMAGE
    {
        public uint DlgId;
        public byte ImgCount;

        public List<DlgImgData> Images = new List<DlgImgData>();
        public struct DlgImgData
        {
            public uint AniId;
            public ushort xpos;
            public ushort ypos;
            public ushort Width;
            public ushort Height;
        };
    }
    public class MSG_DLG_Text
    {
        public uint DlgId;
        public byte TextCount;

        public List<DlgTxtData> Text = new List<DlgTxtData>();
        public struct DlgTxtData
        {
            public uint Id;
            public ushort xpos;
            public ushort ypos;
            public byte Fontsize;
            public uint Color;
            public byte TextLength;
            public string Text;
        };
    }
}
