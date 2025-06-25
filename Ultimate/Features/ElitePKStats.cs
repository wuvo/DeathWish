using Ultimate.Game;
using Ultimate.PacketHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Features
{
    public class ElitePKStats
    {
        public class Rank
        {
            public string Name = "None";
            public uint UID;
            public ushort Face;
            public bool Claimed;
        }
        public class Match
        {
            public uint MapID;
            public uint PlayerAScore;
            public uint PlayerBScore;
            public List<uint> Players = new List<uint>();
        }

        public static Rank First = new Rank();
        public static Rank Second = new Rank();
        public static Rank Third = new Rank();
        public static Rank Fourth = new Rank();

        //public static Dictionary<byte, Rank> Ranking = new Dictionary<byte, Rank>();
        //public static Dictionary<byte, string> Finals = new Dictionary<byte, string>();
        
        //public static Dictionary<uint, List<string>> DuelHistory = new Dictionary<uint, List<string>>();
        public static Dictionary<byte, Rank> Brackets = new Dictionary<byte, Rank>();
        public static List<uint> WaitingList = new List<uint>();
        public static Dictionary<uint, Match> mapsPairs = new Dictionary<uint, Match>();

        public static DateTime Finish = new DateTime();

        public static bool Running = false;
        
        public static void WindowInformation(Character C)
        {
            CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 140, xpos = 81, ypos = 423, Height = 37, Width = 140, TipColor = 0, TipStr = "" };
            B.AniId = (int)ButtonType.Qualifier;
            B.ButtonUID = B.AniId;
            C.MyClient.AddSend(Packets.DynamicButton((int)20, B));

            B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 140, xpos = 262, ypos = 423, Height = 37, Width = 140, TipColor = 0, TipStr = "" };
            B.AniId = (int)ButtonType.SkillPK;
            B.ButtonUID = B.AniId;

            C.MyClient.AddSend(Packets.DynamicButton((int)20, B));

            B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 140, xpos = 444, ypos = 423, Height = 37, Width = 140, TipColor = 0, TipStr = "" };
            B.AniId = (int)ButtonType.TeamPK;
            B.ButtonUID = B.AniId;

            C.MyClient.AddSend(Packets.DynamicButton((int)20, B));

            if (Running)
            {
                if (Brackets.Count > 0)
                {
                    MSG_DLG_IMAGE Img = new MSG_DLG_IMAGE()
                    {
                        DlgId = 20,
                        Images = new List<MSG_DLG_IMAGE.DlgImgData>()
                    };
                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 507, xpos = 244, ypos = 11, Width = 168, Height = 44 });//Window Heading

                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 513, xpos = 73, ypos = 82, Width = 503, Height = 177 });//Ladder

                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = First.Face, xpos = 292, ypos = 300, Width = 64, Height = 64 });//Champion
                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Second.Face, xpos = 132, ypos = 300, Width = 64, Height = 64 });//2ndPlace
                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Third.Face, xpos = 449, ypos = 301, Width = 64, Height = 64 });//3rd Place
                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 514, xpos = 238, ypos = 250, Width = 180, Height = 160 });//Champion
                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 515, xpos = 130, ypos = 282, Width = 70, Height = 105 });//2ndPlace
                    Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 516, xpos = 447, ypos = 282, Width = 70, Height = 105 });//3rd Place

                    if (Brackets.Count > 0)
                    {
                        if (Brackets.ContainsKey(10))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 517, xpos = 165, ypos = 119, Width = 42, Height = 23 });//Fire Line - Bracket[1]
                        else if (Brackets.ContainsKey(11))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 518, xpos = 165, ypos = 136, Width = 42, Height = 23 });//Fire Line - Bracket[2]

                        if (Brackets.ContainsKey(12))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 517, xpos = 165, ypos = 180, Width = 42, Height = 23 });//Fire Line - Bracket[3]
                        else if (Brackets.ContainsKey(13))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 518, xpos = 165, ypos = 197, Width = 42, Height = 23 });//Fire Line - Bracket[4]

                        if (Brackets.ContainsKey(14))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 519, xpos = 447, ypos = 119, Width = 42, Height = 23 });//Fire Line - Bracket[5]
                        else if (Brackets.ContainsKey(15))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 520, xpos = 447, ypos = 136, Width = 42, Height = 23 });//Fire Line - Bracket[6]

                        if (Brackets.ContainsKey(16))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 519, xpos = 447, ypos = 180, Width = 42, Height = 23 });//Fire Line - Bracket[7]
                        else if (Brackets.ContainsKey(17))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 520, xpos = 447, ypos = 197, Width = 42, Height = 23 });//Fire Line - Bracket[8]

                        if (Brackets.ContainsKey(18))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 521, xpos = 203, ypos = 135, Width = 36, Height = 36 });//Fire Line - Bracket[17]
                        else if (Brackets.ContainsKey(19))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 522, xpos = 203, ypos = 168, Width = 36, Height = 36 });//Fire Line - Bracket[18]

                        if (Brackets.ContainsKey(20))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 523, xpos = 415, ypos = 135, Width = 36, Height = 36 });//Fire Line - Bracket[19]
                        else if (Brackets.ContainsKey(21))
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 524, xpos = 415, ypos = 168, Width = 36, Height = 36 });//Fire Line - Bracket[20]

                        if ((Brackets.ContainsKey(18) || Brackets.ContainsKey(19)) && (Brackets.ContainsKey(20) || Brackets.ContainsKey(21)) && Third.Name != "None")
                            Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 525, xpos = 231, ypos = 147, Width = 190, Height = 45 });//Fire Line

                    }

                    Img.ImgCount = (byte)Img.Images.Count;
                    C.MyClient.AddSend(Packets.MsgDlgImage(Img));

                    MSG_DLG_Text Txt = new MSG_DLG_Text()
                    {
                        DlgId = 20,
                        Text = new List<MSG_DLG_Text.DlgTxtData>()
                    };

                    for (byte a = 0; a < 8; a++)
                    {
                        MSG_DLG_Text.DlgTxtData T;
                        if (Brackets.ContainsKey(a))
                        {
                            switch (a)
                            {
                                case 0:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 113, Color = 0xFFFFFF, Fontsize = 12 };//first
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(142 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 1:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 2, ypos = 148, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(142 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 2:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 3, ypos = 175, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(142 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 3:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 4, ypos = 210, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(142 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 4:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 5, ypos = 113, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(530 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 5:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 6, ypos = 148, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(530 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 6:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 7, ypos = 175, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(530 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                                case 7:
                                    T = new MSG_DLG_Text.DlgTxtData() { Id = 8, ypos = 210, Color = 0xFFFFFF, Fontsize = 12 };
                                    T.Text = Brackets[a].Name; T.TextLength = (byte)T.Text.Length; T.xpos = (ushort)(530 - CustomDialog.MeasureStringMin(T.Text, T.Fontsize)); Txt.Text.Add(T);
                                    break;
                            }
                        }
                    }

                    MSG_DLG_Text.DlgTxtData N = new MSG_DLG_Text.DlgTxtData() { Text = First.Name, ypos = 370, Color = 0xffd700, Fontsize = 15, Id = 50 };//Champion
                    N.TextLength = (byte)N.Text.Length;
                    N.xpos = (ushort)(339 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                    Txt.Text.Add(N);

                    N = new MSG_DLG_Text.DlgTxtData() { Text = Second.Name, ypos = 367, Color = 0xFFFFFF, Fontsize = 14, Id = 51 };//Champion
                    N.TextLength = (byte)N.Text.Length;
                    N.xpos = (ushort)(175 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                    Txt.Text.Add(N);

                    N = new MSG_DLG_Text.DlgTxtData() { Text = Third.Name, ypos = 370, Color = 0xFFFFFF, Fontsize = 12, Id = 52 };//Champion
                    N.TextLength = (byte)N.Text.Length;
                    N.xpos = (ushort)(495 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                    Txt.Text.Add(N);

                    Txt.TextCount = (byte)Txt.Text.Count;
                    C.MyClient.AddSend(Packets.MsgDlgText(Txt));
                }
                else
                {

                }
            }
            else
            {
                MSG_DLG_IMAGE Img = new MSG_DLG_IMAGE()
                {
                    DlgId = 20,
                    Images = new List<MSG_DLG_IMAGE.DlgImgData>()
                };
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 507, xpos = 244, ypos = 11, Width = 168, Height = 44 });//Window Heading

                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 526, xpos = 70, ypos = 60, Width = 515, Height = 85 });//Window Heading
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 527, xpos = 70, ypos = 150, Width = 515, Height = 85 });//Window Heading
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 528, xpos = 70, ypos = 240, Width = 515, Height = 85 });//Window Heading
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 529, xpos = 70, ypos = 330, Width = 515, Height = 85 });//Window Heading

                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = First.Face, xpos = 475, ypos = 70, Width = 54, Height = 54 });//Champion
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Second.Face, xpos = 475, ypos = 160, Width = 54, Height = 54 });//2ndPlace
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Third.Face, xpos = 475, ypos = 250, Width = 54, Height = 54 });//3rd Place
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Fourth.Face, xpos = 475, ypos = 340, Width = 54, Height = 54 });//3rd Place
                
                Img.ImgCount = (byte)Img.Images.Count;
                C.MyClient.AddSend(Packets.MsgDlgImage(Img));

                MSG_DLG_Text Txt = new MSG_DLG_Text()
                {
                    DlgId = 20,
                    Text = new List<MSG_DLG_Text.DlgTxtData>()
                };

                MSG_DLG_Text.DlgTxtData N = new MSG_DLG_Text.DlgTxtData() { Text = First.Name, ypos = 125, Color = 0x000, Fontsize = 12, Id = 1 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = (ushort)(510 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = Second.Name, ypos = 215, Color = 0xFFFFFF, Fontsize = 12, Id = 2 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = (ushort)(510 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = Third.Name, ypos = 305, Color = 0xFFFFFF, Fontsize = 12, Id = 3 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = (ushort)(510 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = Fourth.Name, ypos = 395, Color = 0xFFFFFF, Fontsize = 12, Id = 4 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = (ushort)(510 - CustomDialog.MeasureStringMin(N.Text, N.Fontsize));
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = "This is a text sample that will include all the\nrewards for the player ending on this ranking", ypos = 87, Color = 0xFFFFFF, Fontsize = 12, Id = 5 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = 160;
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = "This is a text sample that will include all the\nrewards for the player ending on this ranking", ypos = 177, Color = 0xFFFFFF, Fontsize = 12, Id = 5 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = 160;
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = "This is a text sample that will include all the\nrewards for the player ending on this ranking", ypos = 267, Color = 0xFFFFFF, Fontsize = 12, Id = 5 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = 160;
                Txt.Text.Add(N);

                N = new MSG_DLG_Text.DlgTxtData() { Text = "This is a text sample that will include all the\nrewards for the player ending on this ranking", ypos = 357, Color = 0xFFFFFF, Fontsize = 12, Id = 5 };//Champion
                N.TextLength = (byte)N.Text.Length;
                N.xpos = 160;
                Txt.Text.Add(N);

                Txt.TextCount = (byte)Txt.Text.Count;
                C.MyClient.AddSend(Packets.MsgDlgText(Txt));
            }
        }
    }
}
