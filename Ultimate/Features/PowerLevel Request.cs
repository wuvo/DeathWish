using Ultimate.Game;
using Ultimate.PacketHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Features
{
    public class PowerLevel_Request
    {
        public static void WindowInformation(Character C, uint DialogID, bool Initial = false)
        {
            MSG_DLG_Text Txt = new MSG_DLG_Text()
            {
                DlgId = DialogID,
                Text = new List<MSG_DLG_Text.DlgTxtData>()
            };
            MSG_DLG_Text.DlgTxtData Text = new MSG_DLG_Text.DlgTxtData() { Id = 1, ypos = 100, Color = 0x2396AF, Fontsize = 13 };
            if (!Initial)
                Text.Text = $"{C.Name}, Level {C.Level.ToString()} has requested\nyou to power level him/her.\nAccept?";
            else
                Text.Text = $"Helping newbies leveling is a great\nway to increase our player base.\nWould you like to receive newbies'\nleveling requests?";

            Text.TextLength = (byte)Text.Text.Length; Text.xpos = 75; Txt.Text.Add(Text);
            Txt.TextCount = (byte)Txt.Text.Count;

            CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 26, AniWidth = 95, xpos = 71, ypos = 181, Height = 26, Width = 95, TipColor = 0, TipStr = "" };
            B.AniId = 10128;

            if (!Initial)
            {
                B.ButtonUID = (int)C.EntityID;
                foreach (Character Archer in World.Archers.Values.ToList())
                {
                    if (Archer.MyClient != null && C != null && Archer.MyTeam != null && Archer.MyTeam.Members.Count < 6 && !Archer.MyTeam.Members.Contains(C.EntityID))
                    {
                        if ((Archer.EventBase != null && (Archer.EventBase.MapEvent == Archer.Loc.Map || Archer.Loc.Map == 1616)) || (Archer.Arena != null && Archer.Arena.MapID == Archer.Loc.Map))
                            continue;

                        Archer.MyClient.AddSend(Packets.ShowDialog(30, 1));
                        Archer.MyClient.AddSend(Packets.MsgDlgText(Txt));
                        Archer.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                    }
                    else if (Archer.MyClient == null /*|| C == null */|| Archer.MyTeam == null)
                        World.Archers.Remove(Archer.EntityID);
                }
            }
            else
            {
                B.ButtonUID = B.AniId;
                C.MyClient.AddSend(Packets.MsgDlgText(Txt));
                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
            }
        }
    }
}
