using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    class CharacterMaking
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            string CharName = "";
            for (int i = 0; i < 16; i++)
                if (Data[20 + i] != 0)
                    CharName += Convert.ToChar(Data[20 + i]);

            ushort Body = BitConverter.ToUInt16(Data, 52);
            byte Job = Data[54];
            if (Job != 50)
            {
                bool valid = true;
                for (int i = 0; i < Game.World.BanChars.Count; i++)
                {
                    if (((string)(Game.World.BanChars[i])).ToLower() == CharName.ToLower())
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid && !CharName.ToLower().Contains("None") && !CharName.ToLower().Contains("event") && !CharName.ToLower().Equals("system") && !CharName.ToLower().Contains("moderator") && !CharName.ToLower().Contains("admin"))
                {
                    if (GC.ValidName(CharName))
                    {
                        GC.AddSend(Packets.PopUpMessage(GC.MessageID, Database.CreateCharacter(GC.AuthInfo.Account, CharName, Body, Job, GC.AuthInfo.UID)));
                    }
                    else
                        GC.AddSend(Packets.PopUpMessage(GC.MessageID, "Invalid character name or lenght!"));
                }
                else GC.AddSend(Packets.PopUpMessage(GC.MessageID, "Your name contains invalid names"));
            }
            else
                GC.AddSend(Packets.PopUpMessage(GC.MessageID, "There are no ninjas in Utimate Conquer!"));
        }
    }
}
