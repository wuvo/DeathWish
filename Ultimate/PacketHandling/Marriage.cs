using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class Marriage
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint AttackType = BitConverter.ToUInt32(Data, 20);

            if (AttackType == 8)
            {
                uint TargetUID = BitConverter.ToUInt32(Data, 12);
                Game.Character TargetMarry = null;

                if (Game.World.H_Chars.ContainsKey(TargetUID))
                    TargetMarry = Game.World.H_Chars[TargetUID];
                if (TargetMarry == null)
                    return;

                if (GC.MyChar.Spouse != "None" && GC.MyChar.Spouse != " " && GC.MyChar.Spouse != "")
                {
                    GC.LocalMessage(2005, "You are married."); return;
                }
                if (TargetMarry.Spouse != "None" && TargetMarry.Spouse != " " && TargetMarry.Spouse != "")
                {
                    GC.LocalMessage(2005, "The target is married."); return;
                }
                //if ((TargetMarry.Body == 1003 || TargetMarry.Body == 1004) && (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004))
                //{
                //    GC.LocalMessage(2005, "No male and male marriage is permited!"); return;
                //}
                //if ((TargetMarry.Body == 2001 || TargetMarry.Body == 2002) && (GC.MyChar.Body == 2001 || GC.MyChar.Body == 2002))
                //{
                //    GC.LocalMessage(2005, "No female and female marriage is permited!"); return;
                //}

                TargetMarry.MyClient.AddSend(Packets.AttackPacket(GC.MyChar.EntityID, TargetMarry.EntityID, TargetMarry.Loc.X, TargetMarry.Loc.Y, 0, 8));
            }
            else if (AttackType == 9)
            {
                uint TargetUID = BitConverter.ToUInt32(Data, 12);
                Game.Character TargetMarry = null;

                if (Game.World.H_Chars.ContainsKey(TargetUID))
                    TargetMarry = Game.World.H_Chars[TargetUID];
                else
                    return;

                if (TargetMarry.Spouse == "None" && GC.MyChar.Spouse == "None")
                {

                    TargetMarry.Spouse = GC.MyChar.Name;
                    GC.MyChar.Spouse = TargetMarry.Name;
                    TargetMarry.MyClient.AddSend(Packets.StringPacket(TargetMarry.EntityID, Game.StringType.Spouse, TargetMarry.Spouse));
                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Spouse, GC.MyChar.Spouse));

                    Database.SaveCharacter(TargetMarry, TargetMarry.MyClient.AuthInfo.Account);
                    Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                    if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                    {
                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " and " + TargetMarry.Name + " are married now.", 2011, 0);
                        GC.MyChar.MyClient.AddSend(Packets.StringPacket(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, Game.StringType.Sound, "wedding"));
                    }

                    else
                    {
                        Game.World.SendMsgToAll("SYSTEM", TargetMarry.Name + " and " + GC.MyChar.Name + " are married now.", 2011, 0);
                        GC.MyChar.MyClient.AddSend(Packets.StringPacket(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, Game.StringType.Sound, "wedding"));
                    }
                }
                else
                {
                    GC.LocalMessage(2005, "Either you or the target is already married!");
                    TargetMarry.MyClient.LocalMessage(2005, "Either you or the target is already married!");
                }
            }
        }
    }
}
