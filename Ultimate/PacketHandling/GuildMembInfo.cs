using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class GuildMembInfo
    {
        public static /*unsafe*/ void Handle(Main.GameClient GC, byte[] Data)
        {
            string Name = "";
            for (int i = 9; i < 28; i++)
            {
                if (Data[i] != 0)
                    Name += Convert.ToChar(Data[i]);
                else break;
            }
            foreach (KeyValuePair<byte, Dictionary<uint, Features.MemberInfo>> Members in GC.MyChar.MyGuild.Members)
            {
                foreach (Features.MemberInfo M in (GC.MyChar.MyGuild.Members[Members.Key]).Values)
                {
                    if (M.MembName == Name)
                    {
                        Data[4] = (byte)M.Donation;
                        Data[4 + 1] = (byte)(M.Donation >> 8);
                        Data[4 + 2] = (byte)(M.Donation >> 16);
                        Data[4 + 3] = (byte)(M.Donation >> 24);
                        Data[8] = (byte)M.Rank;
                        //fixed (byte* p = Data)
                        //{
                        //    *((uint*)(p + 4)) = (uint)M.Donation;
                        //    *(p + 8) = (byte)M.Rank;
                        //}
                    }
                }
            }
            
            COPacket P = new COPacket(Data);
            GC.AddSend(P);

        }
    }
}
