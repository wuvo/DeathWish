using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;
using Ultimate.PacketHandling;
using Ultimate.Features;

namespace Ultimate.NPCs
{
    public class NPC_2 : NPCBase
    {
        public NPC_2(Main.GameClient _client)
            : base(_client)
        {
            ID = 2;
            Face = 1;
        }
        public static List<uint> RegularGarments = new List<uint>() { 181305, 181315, 181325, 181405, 181415, 181425, 181505, 181515, 181525, 181605, 181615, 181625, 181705, 181715, 181725, 181805, 181815, 181825, 181905, 181915, 181925 };
        public static List<uint> RareGarments = new List<uint>() { 181345, 181355, 181365, 181375, 181385, 181395, 181435, 182405, 181795, 182315, 182325, 191605 };
        public static List<uint> SpecialGarments = new List<uint>() { 192695, 188335, 192615, 194210, 193075, 193625, 188705, 187975, 192345, 188945, 193355, 194320, 192575, 187575 };//193115, 192785, 193195, 192635, 187315, 188175, 188165, 188265, 187965, 192435, 193015, 187775

        public override void Run(GameClient _client, byte[] Data, ushort _linkback)
        {
            _client.MyChar.ArenaPage = 0;
            _client.MyChar.Female = false;
            _client.AddSend(Packets.ShowDialog(32, 1));
            _client.AddSend(Packets.StringPacket(_client.MyChar.Loc.X, _client.MyChar.Loc.Y, Game.StringType.Sound, "DlgOpen"));
            //TestWindow(_client.MyChar, 32);
            WindowsInformation(_client.MyChar, 32);
        }
        public static void Confirmation(Character C, uint DialogID, uint ButtonID)
        {
            C.MyClient.AddSend(Packets.RemoveButton((int)33, -1));
            if (ButtonID == 10139)
            {
                CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 201, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };
                B.AniId = 10134;
                B.ButtonUID = (int)10134;
                C.MyClient.AddSend(Packets.DynamicButton((int)33, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 358, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };
                B.AniId = 10137;
                B.ButtonUID = (int)10137;
                C.MyClient.AddSend(Packets.DynamicButton((int)33, B));

                MSG_DLG_Text Txt = new MSG_DLG_Text()
                {
                    TextCount = 1,
                    DlgId = 33,
                    Text = new List<MSG_DLG_Text.DlgTxtData>()
                };

                MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 220, xpos = 200, Color = 0xd8b14a, Fontsize = 12 };
                Name.Text = $"Would you like to exchange 1 VIP Card\nfor 2 GarmentTickets?\nYou currently have {C.InventoryItemIDCount(710213).ToString()} GarmentTickets."; Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
                C.MyClient.AddSend(Packets.MsgDlgText(Txt));
            }
            else if (ButtonID == 10134)
            {
                MSG_DLG_Text Txt = new MSG_DLG_Text()
                {
                    TextCount = 1,
                    DlgId = 33,
                    Text = new List<MSG_DLG_Text.DlgTxtData>()
                };
                MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 220, xpos = 200, Color = 0x744824, Fontsize = 12 };
                CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 276, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };

                if (!C.InventoryContains(780001, 1) || C.Inventory.Count > 38)
                {
                    if (C.Inventory.Count > 38)
                        Name.Text = $"You don't have enough space in your inventory.\nPlease clear some space first!";
                    else
                        Name.Text = $"You don't a VIP Card to exchange it for\nGarmentTickets.\nPlease come back when you got a VIP Card!";
                    B.AniId = 10137;
                    B.ButtonUID = (int)10137;
                }
                else
                {
                    Name.Text = $"You have successfully exchanged a VIP Card\nfor 2 GarmentTickets!\nMake a good use of them!";
                    B.AniId = 10138;
                    B.ButtonUID = (int)10137;

                    C.RemoveItem(C.NextItem(780001));
                    for (int a = 0; a < 2; a++)
                        C.AddItem(710213);
                    WindowsInformation(C, 32, C.CostumerPage, C.ArenaPage);
                }
                Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
                C.MyClient.AddSend(Packets.MsgDlgText(Txt));
                C.MyClient.AddSend(Packets.DynamicButton((int)33, B));
            }
            else if (DialogID == 32)
            {
                C.MyClient.AddSend(Packets.ShowDialog(33, 1));

                CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 201, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };
                B.AniId = 10136;
                B.ButtonUID = (int)ButtonID;
                C.MyClient.AddSend(Packets.DynamicButton((int)33, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 358, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };
                B.AniId = 10137;
                B.ButtonUID = (int)10137;
                C.MyClient.AddSend(Packets.DynamicButton((int)33, B));

                MSG_DLG_Text Txt = new MSG_DLG_Text()
                {
                    TextCount = 1,
                    DlgId = 33,
                    Text = new List<MSG_DLG_Text.DlgTxtData>()
                };
                byte Price = 0;
                if (NPCs.NPC_2.RegularGarments.Contains(ButtonID))
                    Price = 1;
                else if (NPCs.NPC_2.RareGarments.Contains(ButtonID))
                    Price = 2;
                else if (NPCs.NPC_2.SpecialGarments.Contains(ButtonID))
                    Price = 3;
                string GarmName = NPCs.NPC_2.GetItemName(ButtonID);

                MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 220, xpos = 200, Color = 0xd8b14a, Fontsize = 12 };
                Name.Text = $"Are you sure that you want to buy {GarmName}\nfor {Price.ToString()} GarmentTickets?\nYou currently have {C.InventoryItemIDCount(710213).ToString()} GarmentTickets."; Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
                C.MyClient.AddSend(Packets.MsgDlgText(Txt));
            }
            else if (DialogID == 33)
            {
                byte Price = 0;
                if (NPCs.NPC_2.RegularGarments.Contains(ButtonID))
                    Price = 1;
                else if (NPCs.NPC_2.RareGarments.Contains(ButtonID))
                    Price = 2;
                else if (NPCs.NPC_2.SpecialGarments.Contains(ButtonID))
                    Price = 3;
                string GarmName = NPCs.NPC_2.GetItemName(ButtonID);

                if (C.InventoryItemIDCount(710213) >= Price)
                {
                    for (int a = 0; a < Price; a++)
                        C.RemoveItem(C.NextItem(710213));
                    C.AddItem(ButtonID);
                    MSG_DLG_Text Txt = new MSG_DLG_Text()
                    {
                        TextCount = 1,
                        DlgId = 33,
                        Text = new List<MSG_DLG_Text.DlgTxtData>()
                    };
                    MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 220, xpos = 200, Color = 0xd8b14a, Fontsize = 12 };
                    Name.Text = $"You've successfully purchased\n{GarmName} for {Price.ToString()} GarmentTickets!\nYou currently have {C.InventoryItemIDCount(710213).ToString()} GarmentTickets."; Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
                    C.MyClient.AddSend(Packets.MsgDlgText(Txt));

                    CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 276, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };
                    B.AniId = 10138;
                    B.ButtonUID = (int)10137;
                    C.MyClient.AddSend(Packets.DynamicButton((int)33, B));
                    WindowsInformation(C, 32, C.CostumerPage, C.ArenaPage);
                }
                else
                {
                    MSG_DLG_Text Txt = new MSG_DLG_Text()
                    {
                        TextCount = 1,
                        DlgId = 33,
                        Text = new List<MSG_DLG_Text.DlgTxtData>()
                    };

                    MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 220, xpos = 200, Color = 0x744824, Fontsize = 12 };
                    Name.Text = $"You don't have enough GarmentTickets\nto purchase {GarmName}.\nPlease come back when you got\n{Price.ToString()} GarmentTickets!"; Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
                    C.MyClient.AddSend(Packets.MsgDlgText(Txt));

                    CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 44, AniWidth = 107, xpos = 276, ypos = 290, Height = 44, Width = 107, TipColor = 0, TipStr = "" };
                    B.AniId = 10137;
                    B.ButtonUID = (int)10137;
                    C.MyClient.AddSend(Packets.DynamicButton((int)33, B));
                }
            }
        }
        public static void WindowsInformation(Character C, uint DialogID, uint ButtonID = 10131, byte page = 0)
        {
            List<uint> Windows = new List<uint>();
            if (ButtonID == 10131 || ButtonID == 10132 || ButtonID == 10133)
                C.CostumerPage = ButtonID;

            if (C.CostumerPage == 10131)
                Windows = RegularGarments;
            else if (C.CostumerPage == 10132)
                Windows = RareGarments;
            else if (C.CostumerPage == 10133)
                Windows = SpecialGarments;

            if (Windows.Count / 8 < page)
            {
                C.ArenaPage--;
                return;
            }
            Windows = Windows.OrderBy(x => x).ToList();

            C.MyClient.AddSend(Packets.RemoveButton((int)DialogID, -1));

            MSG_DLG_Text Txt = new MSG_DLG_Text()
            {
                DlgId = DialogID,
                Text = new List<MSG_DLG_Text.DlgTxtData>()
            };

            MSG_DLG_IMAGE Img = new MSG_DLG_IMAGE()
            {
                DlgId = DialogID,
                Images = new List<MSG_DLG_IMAGE.DlgImgData>()
            };

            MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 30, xpos = 170, Color = 0xFFFFFF, Fontsize = 12 };
            Name.Text = C.InventoryItemIDCount(710213).ToString(); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

            CustomDialog.DlgBtnData B;
            B = new CustomDialog.DlgBtnData() { AniHeight = 27, AniWidth = 34, xpos = 603, ypos = 25, Height = 27, Width = 34, TipColor = 0, TipStr = "" };

            if (!C.Female)
                B.AniId = 10141;
            else
                B.AniId = 10140;

            B.ButtonUID = B.AniId;
            C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

            try
            {
                int a = (page * 8);
                int b = (page * 8);
                int ClientSex = C.Female ? 1 : 0;
                foreach (uint UID in Windows)
                {
                    if (Windows.Count > a && a < (page * 8 + 8))
                    {
                        if (PacketHandling.ItemPacket.Equip.EquipPassSexReq(UID, ClientSex))
                        {
                            if (a <= (page * 8) + 3)
                            {
                                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = (uint)(Windows[b] + ClientSex), xpos = (ushort)(58 + (a - (page * 8)) * 147), ypos = 70, Width = 106, Height = 140 });//Information Window
                                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 512, xpos = (ushort)(53 + (a - (page * 8)) * 147), ypos = 57, Width = 115, Height = 157 });//Information Window

                                Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a), ypos = 217, xpos = (ushort)(130 - MeasureStringMin(GetItemName(Windows[b]), 12) + (a - (page * 8)) * 147), Color = 0xFFD700, Fontsize = 12 };
                                Name.Text = GetItemName(Windows[b]); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

                                B = new CustomDialog.DlgBtnData() { AniHeight = 41, AniWidth = 108, xpos = (ushort)(57 + (a - (page * 8)) * 147), ypos = 231, Height = 41, Width = 108, TipColor = 0, TipStr = "" };
                                B.AniId = 10135;
                                B.ButtonUID = (int)Windows[b];
                                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                            }
                            else
                            {
                                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = (uint)(Windows[b] + ClientSex), xpos = (ushort)(58 + ((a - 4) - (page * 8)) * 147), ypos = 300, Width = 106, Height = 140 });//Information Window
                                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 512, xpos = (ushort)(53 + ((a - 4) - (page * 8)) * 147), ypos = 287, Width = 115, Height = 157 });//Information Window

                                Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a), ypos = 448, xpos = (ushort)(130 - MeasureStringMin(GetItemName(Windows[b]), 12) + ((a - 4) - (page * 8)) * 147), Color = 0xFFD700, Fontsize = 12 };
                                Name.Text = GetItemName(Windows[b]); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

                                B = new CustomDialog.DlgBtnData() { AniHeight = 41, AniWidth = 108, xpos = (ushort)(57 + ((a - 4) - (page * 8)) * 147), ypos = 461, Height = 41, Width = 108, TipColor = 0, TipStr = "" };
                                B.AniId = 10135;
                                B.ButtonUID = (int)Windows[b];
                                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                            }
                            a++;
                        }
                        b++;
                    }
                }
                //for (int a = (page * 8); a < (page * 8 + 8); a++)
                //{
                //    if (Windows.Count > a)
                //    {
                //        if (a <= (page * 8) + 3)
                //        {
                //            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Windows[a], xpos = (ushort)(58 + (a - (page * 8)) * 147), ypos = 70, Width = 106, Height = 140 });//Information Window
                //            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 512, xpos = (ushort)(53 + (a - (page * 8)) * 147), ypos = 57, Width = 115, Height = 157 });//Information Window

                //            Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a), ypos = 217, xpos = (ushort)(130 - MeasureStringMin(GetItemName(Windows[a]), 12) + (a - (page * 8)) * 147), Color = 0xFFD700, Fontsize = 12 };
                //            Name.Text = GetItemName(Windows[a]); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

                //            B = new CustomDialog.DlgBtnData() { AniHeight = 41, AniWidth = 108, xpos = (ushort)(57 + (a - (page * 8)) * 147), ypos = 231, Height = 41, Width = 108, TipColor = 0, TipStr = "" };
                //            B.AniId = 10135;
                //            B.ButtonUID = (int)Windows[a];
                //            C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                //        }
                //        else
                //        {
                //            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Windows[a], xpos = (ushort)(58 + ((a - 4) - (page * 8)) * 147), ypos = 300, Width = 106, Height = 140 });//Information Window
                //            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 512, xpos = (ushort)(53 + ((a - 4) - (page * 8)) * 147), ypos = 287, Width = 115, Height = 157 });//Information Window

                //            Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a), ypos = 448, xpos = (ushort)(130 - MeasureStringMin(GetItemName(Windows[a]), 12) + ((a - 4) - (page * 8)) * 147), Color = 0xFFD700, Fontsize = 12 };
                //            Name.Text = GetItemName(Windows[a]); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

                //            B = new CustomDialog.DlgBtnData() { AniHeight = 41, AniWidth = 108, xpos = (ushort)(57 + ((a - 4) - (page * 8)) * 147), ypos = 461, Height = 41, Width = 108, TipColor = 0, TipStr = "" };
                //            B.AniId = 10135;
                //            B.ButtonUID = (int)Windows[a];
                //            C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                //        }
                //    }
                //    else
                //        break;
                //}
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
            }

            Img.ImgCount = (byte)Img.Images.Count;
            C.MyClient.AddSend(Packets.MsgDlgImage(Img));

            Txt.TextCount = (byte)Txt.Text.Count;
            C.MyClient.AddSend(Packets.MsgDlgText(Txt));
        }

        //public static void WindowsInformation(Character C, uint DialogID, uint ButtonID = 10131, byte page = 0)
        //{
        //    //ButtonID = 10133;
        //    List<uint> Windows = new List<uint>();
        //    if (ButtonID == 10131 || ButtonID == 10132 || ButtonID == 10133)
        //        C.CostumerPage = ButtonID;

        //    if (C.CostumerPage == 10131)
        //        Windows = RegularGarments;
        //    else if (C.CostumerPage == 10132)
        //        Windows = RareGarments;
        //    else if (C.CostumerPage == 10133)
        //        Windows = SpecialGarments;

        //    if (Windows.Count / 8 < page)
        //    {
        //        C.ArenaPage--;
        //        return;
        //    }

        //    C.MyClient.AddSend(Packets.RemoveButton((int)DialogID, -1));

        //    MSG_DLG_Text Txt = new MSG_DLG_Text()
        //    {
        //        DlgId = DialogID,
        //        Text = new List<MSG_DLG_Text.DlgTxtData>()
        //    };

        //    MSG_DLG_IMAGE Img = new MSG_DLG_IMAGE()
        //    {
        //        DlgId = DialogID,
        //        Images = new List<MSG_DLG_IMAGE.DlgImgData>()
        //    };

        //    MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 36, xpos = 145, Color = 0xFFFFFF, Fontsize = 12 };
        //    Name.Text = C.InventoryItemIDCount(710213).ToString(); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

        //    CustomDialog.DlgBtnData B;

        //    try
        //    {
        //        for (int a = (page * 8); a < (page * 8 + 8); a++)
        //        {
        //            if (Windows.Count > a)
        //            {
        //                if (a <= (page * 8) + 3)
        //                {
        //                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Windows[a], xpos = (ushort)(86 + (a - (page * 8)) * 130), ypos = 72, Width = 100, Height = 120 });//Information Window
        //                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 512, xpos = (ushort)(79 + (a - (page * 8)) * 130), ypos = 63, Width = 110, Height = 130 });//Information Window

        //                    Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a), ypos = 193, xpos = (ushort)(150 - MeasureStringMin(GetItemName(Windows[a]), 12) + (a - (page * 8)) * 130), Color = 0xFFFFFF, Fontsize = 12 };
        //                    Name.Text = GetItemName(Windows[a]); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

        //                    B = new CustomDialog.DlgBtnData() { AniHeight = 26, AniWidth = 90, xpos = (ushort)(90 + (a - (page * 8)) * 130), ypos = 210, Height = 26, Width = 90, TipColor = 0, TipStr = "" };
        //                    B.AniId = 10135;
        //                    B.ButtonUID = (int)Windows[a];
        //                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
        //                }
        //                else
        //                {
        //                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Windows[a], xpos = (ushort)(86 + ((a - 4) - (page * 8)) * 130), ypos = 250, Width = 100, Height = 120 });//Information Window
        //                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 512, xpos = (ushort)(79 + ((a - 4) - (page * 8)) * 130), ypos = 241, Width = 110, Height = 130 });//Information Window

        //                    Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a), ypos = 370, xpos = (ushort)(150 - MeasureStringMin(GetItemName(Windows[a]), 12) + ((a - 4) - (page * 8)) * 130), Color = 0xFFFFFF, Fontsize = 12 };
        //                    Name.Text = GetItemName(Windows[a]); Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);

        //                    B = new CustomDialog.DlgBtnData() { AniHeight = 26, AniWidth = 90, xpos = (ushort)(90 + ((a - 4) - (page * 8)) * 130), ypos = 387, Height = 26, Width = 90, TipColor = 0, TipStr = "" };
        //                    B.AniId = 10135;
        //                    B.ButtonUID = (int)Windows[a];
        //                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
        //                }
        //            }
        //            else
        //                break;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        World.ExcAdd += e + "\r\n";
        //    }

        //    Img.ImgCount = (byte)Img.Images.Count;
        //    C.MyClient.AddSend(Packets.MsgDlgImage(Img));

        //    Txt.TextCount = (byte)Txt.Text.Count;
        //    C.MyClient.AddSend(Packets.MsgDlgText(Txt));
        //}
        public static string GetItemName(uint UID)
        {
            if (Database.DatabaseItems.ContainsKey(UID))
                return Database.DatabaseItems[UID].Name;
            else
                return "ERROR";
        }
        private static float MeasureStringMin(string Text, float Size)
        {
            //set font, size & style
            System.Drawing.Font f = new System.Drawing.Font("Arial", Size);

            //create a bmp / graphic to use MeasureString on
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(2200, 2200);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);

            //measure the string
            System.Drawing.SizeF sizeOfString = new System.Drawing.SizeF();
            sizeOfString = g.MeasureString(Text, f);
            f.Dispose();
            b.Dispose();
            g.Dispose();

            return sizeOfString.Width / 2;
        }
        //public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        //{
        //    Responses = new List<COPacket>();
        //    AddAvatar();
        //    switch (_linkback)
        //    {
        //        case 0:
        //            {
        //                AddText("I have hid in the shadows saving the most amazing costumes. Now, I have returned with the goal of dressing up everyone");
        //                AddText(" the best I can. How can I help you with?");
        //                AddOption("Obtain GarmentTickets", 1);
        //                AddOption("Exchange GarmentTickets", 2);
        //                AddOption("I see", 255);
        //                break;
        //            }
        //        case 1:
        //            {
        //                AddText("Right now I am only trading my GarmentTickets for VIP Cards! If you have one I can give you two GarmentTickets in return!");
        //                AddOption("Exchange VIP Card", 3);
        //                AddOption("I'll think about it", 255);
        //                break;
        //            }
        //        case 2:
        //            {
        //                AddText("Alright! First let me tell you about my prices. I'm selling Cheap Garments for 1 ticket, Regular");
        //                AddText(" Garments for 2 tickets and Special Garments for 3 tickets! What do you want to buy?");
        //                AddOption("Cheap Garments", 4);
        //                AddOption("Regular Garments", 5);
        //                AddOption("Special Garments", 6);
        //                AddOption("Nevermind", 255);
        //                break;
        //            }
        //        case 3:
        //            {
        //                if (GC.MyChar.InventoryContains(780001, 1))
        //                {
        //                    if (GC.MyChar.Inventory.Count >= 38)
        //                    {
        //                        AddText("Please make some room in your inventory!");
        //                        AddOption("I see", 255);
        //                    }
        //                    else
        //                    {
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(780001));
        //                        for (int a = 0; a < 2; a++)
        //                            GC.MyChar.AddItem(710213);
        //                        AddText("Here you go! Enjoy your two GarmentTickets!");
        //                        AddOption("Thanks", 255);
        //                    }
        //                }
        //                else
        //                {
        //                    AddText("I'm sorry but you don't have any VIP Cards!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 4:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 1))
        //                {
        //                    AddText("What kind of Garment would you like to purchase?");
        //                    AddOption("Celestial", 50);
        //                    AddOption("Elegance", 60);
        //                    AddOption("Phoenix", 70);
        //                    AddOption("Let me think it over", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        #region CheapGarments
        //        case 50:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("WhiteCelestial", 51);
        //                AddOption("BrownCelestial", 52);
        //                AddOption("BlackCelestial", 53);
        //                AddOption("RedCelestial", 54);
        //                AddOption("GreenCelestial", 55);
        //                AddOption("BlueCelestial", 56);
        //                AddOption("PurpleCelestial", 57);
        //                AddOption("Let me think it over", 255);
        //                break;
        //            }
        //        case 60:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("WhiteElegance", 61);
        //                AddOption("BrownElegance", 62);
        //                AddOption("BlackElegance", 63);
        //                AddOption("RedElegance", 64);
        //                AddOption("GreenElegance", 65);
        //                AddOption("BlueElegance", 66);
        //                AddOption("PurpleElegance", 67);
        //                AddOption("Let me think it over", 255);
        //                break;
        //            }
        //        case 70:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("WhitePhoenix", 71);
        //                AddOption("BrownPhoenix", 72);
        //                AddOption("BlackPhoenix", 73);
        //                AddOption("RedPhoenix", 74);
        //                AddOption("GreenPhoenix", 75);
        //                AddOption("BluePhoenix", 76);
        //                AddOption("PurplePhoenix", 77);
        //                AddOption("Let me think it over", 255);
        //                break;
        //            }
        //        case 51:
        //        case 52:
        //        case 53:
        //        case 54:
        //        case 55:
        //        case 56:
        //        case 57:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 1))
        //                {
        //                    for (int a = 0; a < 1; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    uint ID = (uint)(181225 + ((_linkback - 50) * 100));
        //                    GC.MyChar.AddItem(ID);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 61:
        //        case 62:
        //        case 63:
        //        case 64:
        //        case 65:
        //        case 66:
        //        case 67:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 1))
        //                {
        //                    for (int a = 0; a < 1; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    uint ID = (uint)(181215 + ((_linkback - 60) * 100));
        //                    GC.MyChar.AddItem(ID);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 71:
        //        case 72:
        //        case 73:
        //        case 74:
        //        case 75:
        //        case 76:
        //        case 77:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 1))
        //                {
        //                    for (int a = 0; a < 1; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    uint ID = (uint)(181205 + ((_linkback - 70) * 100));
        //                    GC.MyChar.AddItem(ID);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                    break;
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                    break;
        //                }
        //            }
        //        #endregion
        //        #region Regular Garments
        //        case 5:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("ColorfulDress", 10);
        //                AddOption("DarkWizard", 11);
        //                AddOption("PrairieWind", 12);
        //                AddOption("SongofTianshan", 13);
        //                AddOption("RoyalDignity", 14);
        //                AddOption("UglyDuck", 15);
        //                AddOption("WeddingGown", 16);
        //                AddOption("Next Page", 7);
        //                break;
        //            }
        //        case 7:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("SouthofCloud", 17);
        //                AddOption("Daisy", 18);
        //                AddOption("BonfireNight", 19);
        //                AddOption("AngelicalDress", 20);
        //                AddOption("GoodLuck", 21);
        //                AddOption("Previous Page", 5);
        //                AddOption("Nevermind", 255);
        //                break;
        //            }
        //        case 10:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181345);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 11:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181355);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 12:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181365);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 13:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181375);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 14:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181385);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 15:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181395);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 16:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181435);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 17:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(182405);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 18:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(181795);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 19:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(182315);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 20:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(182325);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 21:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 2))
        //                {
        //                    for (int a = 0; a < 2; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(191305);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        #endregion
        //        #region SpecialGarments
        //        case 6:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("FlameDragon", 121);
        //                AddOption("FairyTale", 122);
        //                AddOption("XmasBunny", 111);
        //                AddOption("FlameDance", 112);
        //                AddOption("FreedomSuit", 113);
        //                AddOption("Winner-take-all", 114);
        //                //AddOption("ChristmasSuit (Blue)", 109);
        //                //AddOption("ChristmasGarment (Red)", 110);
        //                //AddOption("IndianLegend", 47);
        //                //AddOption("TenderFlame", 48);
        //                //AddOption("DancingDress", 49);
        //                //AddOption("PoliceUniform", 100);
        //                //AddOption("IvoryRobe", 101);
        //                //AddOption("SwordShadow", 102);
        //                //AddOption("AkatsukiCloak", 43);
        //                //AddOption("NarutoVest", 44);
        //                //AddOption("ErrantryRobe", 45);
        //                //AddOption("BloodThirst", 46);
        //                //if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 7)
        //                //{
        //                //    AddOption("EpicRobe", 30);
        //                //    AddOption("FlameRobe", 31);
        //                //    AddOption("PunkRocker", 41);
        //                //}
        //                //else if (DateTime.Now.Day >= 8 && DateTime.Now.Day <= 14)
        //                //{
        //                //    AddOption("CaribbeanPirate", 42);
        //                //    AddOption("FatalAllure", 32);
        //                //}
        //                //AddOption("BeachSuit", 33);
        //                AddOption("Next Page", 35);
        //                AddOption("Nevermind", 255);
        //                break;
        //            }
        //        case 35:
        //            {
        //                AddText("Which Garment would you like to purchase?");
        //                AddOption("MonkeyKingGown", 115);
        //                AddOption("IvoryRobe", 116);
        //                AddOption("MatadorSuit", 117);
        //                AddOption("DreamGarment", 118);
        //                AddOption("DelightofSpeed", 119);
        //                AddOption("SpringShirt", 120);
        //                //AddOption("AncientGeneral", 103);
        //                //AddOption("Winner-take-all", 104);
        //                //AddOption("ColorOfWind", 105);
        //                //AddOption("AssassinSuit", 106);
        //                //AddOption("DreamyFairySuit", 107);
        //                //AddOption("SoberDark", 108);
        //                //AddOption("FancyAzure", 36);
        //                //AddOption("TaekwondoUniform", 37);
        //                //AddOption("Spartan`sPride", 34);
        //                //AddOption("SamuraiLegacy", 38);
        //                //AddOption("DuskRomance", 39);
        //                //AddOption("Evernight", 40);
        //                AddOption("Previous Page", 6);
        //                AddOption("Nevermind", 255);
        //                break;
        //            }
        //        case 30:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(191405);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 31:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(183475);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 32:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(184305);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 33:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(184345);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 34:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(184395);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 36:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(183425);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 37:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(183345);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 38:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(183315);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 39:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(183325);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 40:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(184325);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 41:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(184335);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 42:
        //            {
        //                if (GC.MyChar.InventoryContains(710213, 3))
        //                {
        //                    for (int a = 0; a < 3; a++)
        //                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                    GC.MyChar.AddItem(183375);
        //                    AddText("Here you go! Enjoy!");
        //                    AddOption("Thanks", 255);
        //                }
        //                else
        //                {
        //                    AddText("You don't have enough GarmentTickets!");
        //                    AddOption("I see", 255);
        //                }
        //                break;
        //            }
        //        case 43:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(193255);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 44:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(193265);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 45:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188155);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 46:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187475);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 47:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188545);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 48:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188495);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 49:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188285);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 100:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188255);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 101:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188175);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 102:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187505);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 103:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187465);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 104:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192635);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 105:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192425);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 106:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192185);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 107:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192125);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 108:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187665);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 109:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(183465);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 110:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187515);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 111:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(193115);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 112:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192785);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 113:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(193195);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 114:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192635);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 115:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187315);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 116:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188175);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 117:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188165);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 118:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(188265);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 119:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187965);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 120:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(192435);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 121:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(193015);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //        case 122:
        //            if (GC.MyChar.InventoryContains(710213, 3))
        //            {
        //                for (int a = 0; a < 3; a++)
        //                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(710213));
        //                GC.MyChar.AddItem(187775);
        //                AddText("Here you go! Enjoy!");
        //                AddOption("Thanks", 255);
        //            }
        //            else
        //            {
        //                AddText("You don't have enough GarmentTickets!");
        //                AddOption("I see", 255);
        //            }
        //            break;
        //            #endregion

        //    }

        //    AddFinish();
        //    Send();
        //}
    }
}