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
    public class NPC_1061 : NPCBase
    {
        public NPC_1061(Main.GameClient _client)
            : base(_client)
        {
            ID = 1061;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Howdy! I'm the Warehouse Guardian! I help players protecting their accounts. What can I do for you?");
                        if (GC.MyChar.WHPassword == "0")
                        {
                            AddOption("Put a password in my warehouse", 1);
                        }
                        else
                        {
                            AddOption("Remove Password", 4);
                            AddOption("Change Password", 6);
                        }
                        AddOption("Let me think", 255);
                        break;
                    }
                case 1:
                    {
                        GC.MyChar.TempPass = "";
                        AddText("Please put your password. Min characters 4 and max 10 characters. Only numbers allowed.");
                        AddInput("Password:", 2);
                        AddOption("Let me think.", 255);
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.TempPass = ReadString(_data);
                        AddText("Please confirm your password.");
                        AddInput("Retype Password:", 3);
                        AddOption("Cancel it.", 255);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.TempPass == ReadString(_data))
                        {
                            if (GC.MyChar.TempPass.Length >= 4 && GC.MyChar.TempPass.Length <= 10)
                            {
                                int pass;
                                if (int.TryParse(GC.MyChar.TempPass, out pass))
                                {
                                    GC.MyChar.WHPassword = GC.MyChar.TempPass;
                                    GC.MyChar.WHOpen = false;
                                    AddText("Congratulations! You have now assigned a password to your warehouse!");
                                    AddOption("Thanks!", 255);
                                }
                                else
                                {
                                    AddText("Only numbers allowed!");
                                    AddOption("I see", 255);
                                }
                            }
                            else
                            {
                                AddText("Min 4 characters and max 10 characters!");
                                AddOption("Sorry!", 255);
                            }
                        }
                        else
                        {
                            AddText("The passwords do not match! Please try again!");
                            AddInput("New Password:", 3);
                            AddOption("Nevermind", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        AddText("In order to remove your password we must confirm which one it is.");
                        AddInput("Current Password:", 5);
                        AddOption("Let me think", 255);
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.WHPassword == ReadString(_data))
                        {
                            GC.MyChar.WHPassword = "0";
                            GC.MyChar.WHOpen = false;
                            AddText("Congratulations! You have successfully removed your password!");
                            AddOption("Thanks!", 255);
                            break;
                        }
                        else
                        {
                            AddText("I'm sorry but the password is not correct!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 6:
                    {
                        AddText("In order to change your password you must provide me your current password for security reasons.");
                        AddInput("Old Password:", 7);
                        break;
                    }
                case 7:
                    {
                        if (GC.MyChar.WHPassword == ReadString(_data))
                        {
                            AddText("Confirmed! Please type your desired password now!");
                            AddInput("New Password:", 2);
                        }
                        else
                        {
                            AddText("I'm sorry but the password is not correct!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}