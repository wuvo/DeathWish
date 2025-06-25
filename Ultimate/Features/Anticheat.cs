using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class Anticheat
    {
        static string CSFullScreenHashVerify = "b43903240a1046fb4f9e09329a1aec75";//done
        static string CSHashVerify = "253573ff749644e4418e8de2efc536de";//done

        static string PHashFullScreenVerify = "b7ed88097b006d8adf7326489de9758b";//done
        static string PHashVerify = "89efbbd227a0ff6cedaf36cc209460f0";//done
        static string PHashVerifyWeird1 = "41454c304dd400f950a4eafc34d0824f";
        static string PHashVerifyWeird2 = "01c36a02281cb5bfbe896d5e8498677c";
        static string PHashVerifyWeird3 = "cab7accf2d253994de30f32922d989b1";
        static string PHashVerifyWeird4 = "4fe461cd765d3e680329cfa36aa60b80";
        static string PHashVerifyWeird5 = "cd403a2ba1d902f3fc97b2e2402c6078";

        static string FHashVerify = "0b2936661168ea693e5131774b84d793";//done
        public static void Handle(GameClient GC, byte[] Data)
        {
            //You should be getting this packet every 20 seconds from the player. 
            //If you don't get the packet at all or you get the packet delayed say i don't know 20 seconds give or take then something is wrong. 
            //Now you have the option to kick ban or warn the player depending on the values received but it's up to you what you do. 
            //Time Stamp is used to extract the info from the number of packets sent/ received 
            //isdebuggerpresent if this is "true" for any reason then the player is attempting or is currently debugging the client. you want this to be false.
            //SentMsgCount the number of packets the client sent in total.
            //RecvMsgCount total of packets received by the client.
            //CESpeedHack If theis is "true" the player is using the Cheat Engine speed hack. Ban his ass 
            //CodeSectionHash This is the md5 hash of the entire code section of the exe. IF this changes the somone is injecting or patching stuff in the client. 
            //Remember the CodeSectionHash has to be updated whenever you receive a new version/copy of the dll from me. I'll show you how to obtain it.
            //PatchesHash This is the md5 hash of every pache that I applied to the exe it has to always be the same unless you get a new copy of the dll (same as CodeSectionHash). This is used to make sure player don't undo out patches.
            //FileHash This is the md5 hash for the jump file and (Magictype/itemtpe) never change unless you edit the files yourself then you would get the new hash value and use it instead of the old one.
            ushort Size = BitConverter.ToUInt16(Data, 0);//115
            int Timestamp = BitConverter.ToInt32(Data, 4);
            int SentMsgCount = BitConverter.ToInt32(Data, 8) ^ Timestamp; //Is this it?yes
            int RecvMsgCount = BitConverter.ToInt32(Data, 12) ^ Timestamp;
            bool CESpeedHack = BitConverter.ToBoolean(Data, 16);
            bool Debugger = BitConverter.ToBoolean(Data, 17);
            string CodeSectionHash = ASCIIEncoding.ASCII.GetString(Data, 18, 32);
            string PatchesHash = ASCIIEncoding.ASCII.GetString(Data, 50, 32);
            string FileHash = ASCIIEncoding.ASCII.GetString(Data, 82, 32);
            //It's broken down as follows 

            //Console.WriteLine("{0}, {1}, {2}", CodeSectionHash, PatchesHash, FileHash);
            //GC.LocalMessage(2000, "CodeSectionHash: " + CodeSectionHash + " , PatchesHash: " + PatchesHash + " , FileHash: " + FileHash);
            if (GC.AuthInfo.Status == "[PM]" || GC.AuthInfo.Status == "[GM]")
            {
                if (GC.MyChar != null)
                    //CheckAntiCheat(GC.MyChar, FileHash, PatchesHash, CodeSectionHash);
                    GC.MyChar._anticheat = DateTime.Now;
                //Console.WriteLine("CodeSectionHash: " + CodeSectionHash + " PatchesHash: " + PatchesHash + " FileHash: " + FileHash);
            }
            else
            {
                //if (GC.MyChar != null)
                //    CheckAntiCheat(GC.MyChar, FileHash, PatchesHash, CodeSectionHash);
                GC.MyChar._anticheat = DateTime.Now;

                //if (Debugger)
                //    Botjail(GC.MyChar, true, false, false);
                //if (CESpeedHack)
                //    Botjail(GC.MyChar, false, true, false);
            }
        }
        static void CheckAntiCheat(Character C, string FHash, string PHash, string CSHash)
        {
            if (!String.Equals(CSHash, CSHashVerify, StringComparison.Ordinal) && !String.Equals(CSHash, CSFullScreenHashVerify, StringComparison.Ordinal))
            {
                Botjail(C, false, false, true);
                Game.World.AntiCheatAdd += "CodeSectionHash was diff " + "(" + CSHash + ") for " + C.Name + " at " + DateTime.Now + " \r\n";
            }
            else if (String.Equals(CSHash, CSHashVerify, StringComparison.Ordinal))
                C.Screen = Enum.ScreenSize.Windowed;
            else if (String.Equals(CSHash, CSFullScreenHashVerify, StringComparison.Ordinal))
                C.Screen = Enum.ScreenSize.Fullscreen;

            if (!String.Equals(PHash, PHashVerify, StringComparison.Ordinal) &&
                !String.Equals(PHash, PHashFullScreenVerify, StringComparison.Ordinal) &&
                !String.Equals(PHash, PHashVerifyWeird1, StringComparison.Ordinal) &&
                !String.Equals(PHash, PHashVerifyWeird2, StringComparison.Ordinal) &&
                !String.Equals(PHash, PHashVerifyWeird3, StringComparison.Ordinal) &&
                !String.Equals(PHash, PHashVerifyWeird4, StringComparison.Ordinal) &&
                !String.Equals(PHash, PHashVerifyWeird5, StringComparison.Ordinal))
            {
                if (!World.Anticheat.ContainsKey(PHash))
                    World.Anticheat.Add(PHash, 0);
                else
                    World.Anticheat[PHash]++;
                //Game.World.AntiCheatAdd += "PatchesHash was diff " + "(" + PHash + ") for " + Name + " at " + DateTime.Now + " \r\n";
                //MyClient.Disconnect();
            }
            if (!String.Equals(FHash, FHashVerify, StringComparison.Ordinal))
            {
                Game.World.AntiCheatAdd += "FileHash was diff " + "(" + FHash + ") for " + C.Name + " at " + DateTime.Now + " \r\n";
                C.MyClient.Disconnect();
            }
            C._anticheat = DateTime.Now;
        }
        static void Botjail(Character C, bool Debug, bool Speedhack, bool Bot)
        {
            if (!C.BOTJailed)
            {
                //C.BOTJailed = true;
                C.BOTJailedDays = 3;
                C.Teleport(6003, 30, 72);
                if (Debug)
                {
                    Game.World.SendMsgToAll("ANTI-CHEAT", C.Name + " was botjailed for trying to debug the client!", 2000, 0);
                    Game.World.AntiCheatAdd += C.Name + " botjailed for " + C.BOTJailedDays + " for trying to debug the client at: " + DateTime.Now + " \r\n";
                }
                if (Speedhack)
                {
                    Game.World.SendMsgToAll("ANTI-CHEAT", C.Name + " was botjailed for speedhacking!", 2000, 0);
                    Game.World.AntiCheatAdd += C.Name + " botjailed for " + C.BOTJailedDays + " for speedhacking at: " + DateTime.Now + " \r\n";
                }
                if (Bot)
                {
                    World.SendMsgToAll("ANTI-CHEAT", C.Name + " was botjailed for botting!", 2000, 0);
                    World.AntiCheatAdd += C.Name + " botjailed for " + C.BOTJailedDays + " for botting at: " + DateTime.Now + " \r\n";
                }
                C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " days!");
                C.MyClient.LocalMessage(2105, "http://www.fb.com/Ultimateconquerfb");
            }
            else
                C.MyClient.Disconnect();
        }
    }
}
