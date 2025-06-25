using Ultimate.Game;
using Ultimate.PacketHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Features
{
    public class Changelog
    {
        public static void WindowsInformation(Character C, uint DialogID, ushort Version, bool Arrows = false)
        {
            MSG_DLG_Text Txt = new MSG_DLG_Text()
            {
                DlgId = DialogID,
                Text = new List<MSG_DLG_Text.DlgTxtData>()
            };

            switch (Version)
            {
                case 1039:
                    MSG_DLG_Text.DlgTxtData Text = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 200, Color = 0xF1FFC9, Fontsize = 14 };
                    Text.Text = "- Added ingame changelog;\n";
                    Text.Text += "- Same sex marriage is now\nallowed;\n";
                    Text.Text += "- VIP Aura is now activated\nby default. Type /vipaura to\nenable/disable it;\n";
                    Text.Text += "- Added Arena Leaderboard;\n";
                    Text.Text += "- Added Arena Matches view;\n";
                    Text.Text += "- Added Garment Effect for\nArena Top 3 Winners;";

                    Text.TextLength = (byte)Text.Text.Length; Text.xpos = 175; Txt.Text.Add(Text);

                   Text = new MSG_DLG_Text.DlgTxtData() { Id = 2, ypos = 200, Color = 0xF1FFC9, Fontsize = 14 };
                    Text.Text = "- Banned players no longer\nshow in guilds' lists;\n";
                    Text.Text += "- Pots can now be dropped\nat Adventure Zone;\n";
                    Text.Text += "- Fixed the Nobility effects;\n";
                    Text.Text += "- EpicRobe garment looks\nis now back to normal;\n";
                    Text.Text += "- Fixed a bug with CCGW\ngates HP;";

                    Text.TextLength = (byte)Text.Text.Length; Text.xpos = 420; Txt.Text.Add(Text);
                    break;
                case 1040:
                    Text = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 200, Color = 0xF1FFC9, Fontsize = 14 };
                    Text.Text = "- Added Power-Level Request\nDialogs;\n";
                    Text.Text += "- Both the arena button and\nthe events invitations\nrequest dialog popup once\nyou reach level 15;\n";
                    
                    Text.TextLength = (byte)Text.Text.Length; Text.xpos = 175; Txt.Text.Add(Text);

                    Text = new MSG_DLG_Text.DlgTxtData() { Id = 2, ypos = 200, Color = 0xF1FFC9, Fontsize = 14 };
                    Text.Text = "- Fixed an error on character\ncreation allowing blocked names;\n";
                    Text.Text += "- Fixed an issue displaying\nClassPK Time;\n";
                    Text.Text += "- Minimap for AdvancedZone\nnow shows properly;\n";

                    Text.TextLength = (byte)Text.Text.Length; Text.xpos = 420; Txt.Text.Add(Text);
                    break;
                case 1041:
                    Text = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 200, Color = 0xF1FFC9, Fontsize = 14 };
                    Text.Text = "- Added Costumer Dialog;\n";
                    Text.Text += "- Changed the garments\nselling at Costumer;\n";
                    Text.Text += "- Added /arena command\nto remove the arena button;\n";
                    Text.Text += "- Added Easter Quests' NPCs;\n";

                    Text.TextLength = (byte)Text.Text.Length; Text.xpos = 175; Txt.Text.Add(Text);

                    Text = new MSG_DLG_Text.DlgTxtData() { Id = 2, ypos = 200, Color = 0xF1FFC9, Fontsize = 14 };
                    Text.Text = "- Newbies no longer can be\nkilled inside the crafting\narea;\n";

                    Text.TextLength = (byte)Text.Text.Length; Text.xpos = 420; Txt.Text.Add(Text);
                    break;
            }
            Txt.TextCount = (byte)Txt.Text.Count;
            C.MyClient.AddSend(Packets.MsgDlgText(Txt));
            
            if (Arrows)
            {
                CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 30, AniWidth = 30, xpos = 150, ypos = 90, Height = 30, Width = 30, TipColor = 0, TipStr = "" };
                B.AniId = 10127;
                B.ButtonUID = B.AniId;
                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 33, AniWidth = 26, xpos = 50, ypos = 210, Height = 33, Width = 26, TipColor = 0, TipStr = "" };
                B.AniId = 10122;
                B.ButtonUID = 10125;
                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 33, AniWidth = 26, xpos = 700, ypos = 210, Height = 33, Width = 26, TipColor = 0, TipStr = "" };
                B.AniId = 10123;
                B.ButtonUID = 10126;
                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
            }

        }
    }
}
