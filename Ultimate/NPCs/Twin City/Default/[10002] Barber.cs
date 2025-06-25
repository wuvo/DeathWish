using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_10002 : NPCBase
    {
        public NPC_10002(Main.GameClient _client)
            : base(_client)
        {
            ID = 10002;
            Face = 111;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Would you like to change your hairstyle? I can offer you a change for 5,000 silvers. You can choose from the styles below.");
                        AddOption("New Styles", 1);
                        AddOption("Nostalgic Styles", 2);
                        AddOption("Special Styles", 3);
                        AddOption("No thanks.", 255);
                        GC.Paid = false;
                        break;
                    }
                #region New Styles
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 5000 || GC.Paid)
                        {
                            if (!GC.Paid)
                            {
                                GC.Paid = true;
                                GC.MyChar.Silvers -= 5000;
                            }
                            GC.Agreed = false;
                            GC.MyChar.Hair = GC.MyChar.Hair;
                            AddText("Choose the style you like the best.");
                            AddOption("New HairStyle01", 10);
                            AddOption("New HairStyle02", 11);
                            AddOption("New HairStyle03", 12);
                            AddOption("New HairStyle04", 13);
                            AddOption("New HairStyle05", 14);
                            AddOption("New HairStyle06", 15);
                            AddOption("New HairStyle07", 16);
                            AddOption("Next", 100);
                            break;
                        }
                        else
                        {
                            AddText("5,000 silvers isn't that expensive. Come again when you have that money with you.");
                            AddOption("Ok.", 255);
                            break;
                        }
                    }
                case 100:
                    {
                        if (GC.Paid)
                        {
                            AddText("Choose the style you like the best.");
                            AddOption("New HairStyle08", 17);
                            AddOption("New HairStyle09", 18);
                            AddOption("New HairStyle10", 19);
                            AddOption("New HairStyle11", 20);
                            AddOption("New HairStyle12", 21);
                            AddOption("Previous", 1);
                        }
                        break;
                    }
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                    {
                        if (GC.Paid)
                        {
                            if (!GC.Agreed)
                            {
                                GC.Agreed = true;
                                AddText("So, do you like it? Or do you want me to change it back?");
                                AddOption("Yes, I like it.", Convert.ToByte(_linkback));
                                AddOption("No, it's awful! Change it back.", 1);
                                GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Hair, ushort.Parse(Convert.ToString(GC.MyChar.Hair)[0] + (20 + _linkback).ToString())));
                            }
                            else
                                GC.MyChar.Hair = ushort.Parse(Convert.ToString(GC.MyChar.Hair)[0] + (20 + _linkback).ToString());
                        }
                        break;
                    }
                #endregion
                #region Nostalgic Styles
                case 2:
                    {
                        if (GC.MyChar.Silvers >= 5000 || GC.Paid)
                        {
                            if (!GC.Paid)
                            {
                                GC.Paid = true;
                                GC.MyChar.Silvers -= 5000;
                            }
                            GC.Agreed = false;
                            GC.MyChar.Hair = GC.MyChar.Hair;
                            AddText("Choose the style you like the best.");
                            AddOption("Nostalgic01", 30);
                            AddOption("Nostalgic02", 31);
                            AddOption("Nostalgic03", 32);
                            AddOption("Nostalgic04", 33);
                            AddOption("Nostalgic05", 34);
                            AddOption("Nostalgic06", 35);
                            AddOption("Nostalgic07", 36);
                        }
                        else
                        {
                            AddText("5,000 silvers isn't that expensive. Come again when you have that money with you.");
                            AddOption("Ok.", 255);
                        }
                        break;
                    }
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                    {
                        if (!GC.Agreed)
                        {
                            GC.Agreed = true;
                            AddText("So, do you like it? Or do you want me to change it back?");
                            AddOption("Yes, I like it.", Convert.ToByte(_linkback));
                            AddOption("No, it's awful! Change it back.", 2);
                            GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Hair, ushort.Parse(Convert.ToString(GC.MyChar.Hair)[0] + (_linkback - 20).ToString())));
                        }
                        else
                            GC.MyChar.Hair = ushort.Parse(Convert.ToString(GC.MyChar.Hair)[0] + (_linkback - 20).ToString());
                        break;
                    }
                #endregion
                #region Special Styles
                case 3:
                    {
                        if (GC.MyChar.Silvers >= 5000 || GC.Paid)
                        {
                            if (!GC.Paid)
                            {
                                GC.Paid = true;
                                GC.MyChar.Silvers -= 5000;
                            }
                            GC.Agreed = false;
                            GC.MyChar.Hair = GC.MyChar.Hair;
                            AddText("Choose the style you like the best.");
                            AddOption("Special01", 40);
                            AddOption("Special02", 41);
                            AddOption("Special03", 42);
                            AddOption("Special04", 43);
                            AddOption("Special05", 44);
                        }
                        else
                        {
                            AddText("5,000 silvers isn't that expensive. Come again when you have that money with you.");
                            AddOption("Ok.", 255);
                        }
                        break;
                    }
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                    {
                        if (!GC.Agreed)
                        {
                            GC.Agreed = true;
                            AddText("So, do you like it? Or do you want me to change it back?");
                            AddOption("Yes, I like it.", Convert.ToByte(_linkback));
                            AddOption("No, it's awful! Change it back.", 3);
                            GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Hair, ushort.Parse(Convert.ToString(GC.MyChar.Hair)[0] + (_linkback - 19).ToString())));
                        }
                        else
                            GC.MyChar.Hair = ushort.Parse(Convert.ToString(GC.MyChar.Hair)[0] + (_linkback - 19).ToString());
                        break;
                    }
                    #endregion
            }

            AddFinish();
            Send();
        }
    }
}