using Ultimate.Game;
using Ultimate.PacketHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Features
{
    public class Anniversary
    {
        public static Dictionary<string, uint> AnniversaryQuest = new Dictionary<string, uint>();

        /// <summary>
        /// Sends the arena qualifier information to player window
        /// </summary>
        /// <param name="C"></param>
        public static void WindowInformation(Character C, uint DialogID)
        {
            var myList = AnniversaryQuest.ToList();
            myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            MSG_DLG_Text Txt = new MSG_DLG_Text()
            {
                DlgId = DialogID,
                Text = new List<MSG_DLG_Text.DlgTxtData>()
            };

            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 1, xpos = 60, ypos = 100, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº1", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 2, xpos = 60, ypos = 115, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº2", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 3, xpos = 60, ypos = 130, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº3", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 4, xpos = 60, ypos = 145, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº4", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 5, xpos = 60, ypos = 160, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº5", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 6, xpos = 60, ypos = 175, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº6", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 7, xpos = 60, ypos = 190, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº7", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 8, xpos = 60, ypos = 205, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº8", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 9, xpos = 60, ypos = 220, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº9", TextLength = 3 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 10, xpos = 60, ypos = 235, Color = 0xFFFFFF00, Fontsize = 12, Text = "Nº10", TextLength = 4 });
            //Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 21, xpos = 60, ypos = 250, Color = 0xFFFFFF00, Fontsize = 12, Text = "A", TextLength = 1 });

            for (int a = 0; a < 10; a++)
            {
                MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(a), xpos = 55, ypos = (ushort)(100 + (a * 15)), Color = 0xFFFFFF, Fontsize = 12, Text = "Nº" + (a+1) + "     " };
                if (a == 9)
                    Name.Text = "Nº" + (a + 1) + "   ";
                if (myList.Count > a)
                    Name.Text += myList[a].Key;
                else
                    Name.Text += "None";

                Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);


                Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(11 + a), xpos = 200, ypos = (ushort)(100 + (a * 15)), Color = 0xFFFFFF, Fontsize = 12 };
                if (myList.Count > a)
                    Name.Text = myList[a].Value.ToString();
                else
                    Name.Text = "0";

                Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
            }
            //for (int a = 0; a < 10; a++)
            //{

            //    MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(31 + a), xpos = 120, ypos = (ushort)(100 + (a * 15)), Color = 0xFFFFFF, Fontsize = 12 };
            //    if (AnniversaryQuest.Count > a)
            //        Name.Text = AnniversaryQuest.ToList()[a].Key;
            //    else
            //        Name.Text = "0";

            //    Name.TextLength = (byte)Name.Text.Length; Txt.Text.Add(Name);
            //}
            Txt.TextCount = (byte)Txt.Text.Count;
            C.MyClient.AddSend(Packets.MsgDlgText(Txt));
        }
    }
}
