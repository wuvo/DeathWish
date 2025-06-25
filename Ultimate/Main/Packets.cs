using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ultimate.Features;
using static System.Windows.Forms.AxHost;

namespace Ultimate
{
    public /*unsafe*/ class COPacket
    {
        private readonly byte[] PData;
        public ushort PType;
        public int Count;

#warning Removed unsafe declarations and pointers and commented Ptr, InLenght and Type
        //private byte* Ptr
        //{
        //    get
        //    {
        //        fixed (byte* p = PData)
        //            return p;
        //    }
        //}

        public byte[] Get
        {
            get
            {
                return PData;
            }
        }
        //public ushort InLength
        //{
        //    get
        //    {
        //        return *((ushort*)(Ptr));
        //    }
        //}
        //public ushort Type
        //{
        //    get
        //    {
        //        return *((ushort*)(Ptr + 2));
        //    }
        //}
        public /*unsafe*/ COPacket(byte[] Data)
        {
            Count = 0;
            PData = Data;
        }
        public /*unsafe*/ void WriteByte(byte val)
        {
            PData[Count] = (byte)val;
            //*((byte*)(Ptr + Count)) = val;
            Count++;
        }
        public /*unsafe*/ void WriteInt16(ushort val)
        {
            try
            {
                PData[Count] = (byte)val;
                PData[Count + 1] = (byte)(val >> 8);
                //*((ushort*)(Ptr + Count)) = val;
                Count += 2;
            }
            catch (AccessViolationException e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
            catch (Exception e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
        }
        public /*unsafe*/ void WriteInt32(uint val)
        {
            try
            {
                PData[Count] = (byte)val;
                PData[Count + 1] = (byte)(val >> 8);
                PData[Count + 2] = (byte)(val >> 16);
                PData[Count + 3] = (byte)(val >> 24);
                //*((uint*)(Ptr + Count)) = val;
                Count += 4;
            }
            catch (AccessViolationException e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
            catch (Exception e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
        }
        public /*unsafe*/ void WriteInt32(int val)
        {
            try
            {
                PData[Count] = (byte)val;
                PData[Count + 1] = (byte)(val >> 8);
                PData[Count + 2] = (byte)(val >> 16);
                PData[Count + 3] = (byte)(val >> 24);
                //*((uint*)(Ptr + Count)) = val;
                Count += 4;
            }
            catch (AccessViolationException e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
            catch (Exception e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
        }
        public /*unsafe*/ void WriteInt64(ulong val)
        {
            try
            {
                PData[Count] = (byte)val;
                PData[Count + 1] = (byte)(val >> 8);
                PData[Count + 2] = (byte)(val >> 16);
                PData[Count + 3] = (byte)(val >> 24);
                PData[Count + 4] = (byte)(val >> 32);
                PData[Count + 5] = (byte)(val >> 40);
                PData[Count + 6] = (byte)(val >> 48);
                PData[Count + 7] = (byte)(val >> 56);
                //*((ulong*)(Ptr + Count)) = val;
                Count += 8;
            }
            catch (AccessViolationException E)
            {
                Game.World.AntiCheatAdd += E + "CRASHED THE SERVER !";
                Program.WriteLogs();
            }
            catch (Exception e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
        }
        public /*unsafe*/ void WriteString(string val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                try
                {
                    PData[Count] = (byte)val[i];
                    //*((byte*)(Ptr + Count)) = Convert.ToByte(val[i]);
                }
                catch
                {
                    Program.WriteLogs();
                }//For weird letters that cannot be converted into byte...

                Count++;
            }
        }
        public /*unsafe*/ void WriteString(string val, int MaxLength)
        {
            try
            {
                if (val.Length <= MaxLength)
                    for (int i = 0; i < val.Length; i++)
                    {
                        PData[Count] = (byte)val[i];
                        //*((byte*)(Ptr + Count)) = Convert.ToByte(val[i]);
                        Count++;
                    }
                else
                    for (int i = 0; i < MaxLength; i++)
                    {
                        PData[Count] = (byte)val[i];
                        //*((byte*)(Ptr + Count)) = Convert.ToByte(val[i]);
                        Count++;
                    }
            }
            catch (AccessViolationException E) { Game.World.ExcAdd += E; Program.WriteLogs(); }
            catch (Exception e)
            {
                Game.World.AntiCheatAdd += e;
                Program.WriteLogs();
            }
        }
        public /*unsafe*/ void WriteBytes(byte[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                PData[Count] = (byte)val[i];
                //*((byte*)(Ptr + Count)) = val[i];
                Count++;
            }
        }
        public void Move(int count)
        {
            Count += count;
        }
    }
    public class COPacket2
    {
        private readonly byte[] buf;
        int pos;
        public COPacket2(byte[] _start) { buf = _start; }
        public bool WriteUInt16(int value)
        {
            try
            {
                buf[pos] = (byte)(value); pos++;
                buf[pos] = (byte)(value >> 8); pos++;
                return true;
            }
            catch { return false; }
        }
        public bool WriteUInt32(uint value)
        {
            try
            {
                buf[pos] = (byte)(value); pos++;
                buf[pos] = (byte)(value >> 8); pos++;
                buf[pos] = (byte)(value >> 16); pos++;
                buf[pos] = (byte)(value >> 24); pos++; return true;
            }
            catch { return false; }
        }
        public bool WriteUInt64(ulong value)
        {
            try
            {
                buf[pos] = (byte)(value); pos++;
                buf[pos] = (byte)(value >> 8); pos++;
                buf[pos] = (byte)(value >> 16); pos++;
                buf[pos] = (byte)(value >> 24); pos++;
                buf[pos] = (byte)(value >> 32); pos++;
                buf[pos] = (byte)(value >> 40); pos++;
                buf[pos] = (byte)(value >> 48); pos++;
                buf[pos] = (byte)(value >> 56); pos++;
                return true;
            }
            catch { return false; }
        }
        public bool WriteByte(byte value)
        {
            try
            {
                buf[pos] = (byte)(value); pos++;
                return true;
            }
            catch { return false; }
        }
        public bool WriteStringWithLength(string value)
        {
            try
            {
                buf[pos] = (byte)value.Length;
                pos++;
                ushort i = 0;
                while (i < value.Length)
                {
                    buf[(ushort)(i + pos)] = (byte)value[i];
                    i = (ushort)(i + 1);
                }
                pos += i;
                return true;
            }
            catch { return false; }
        }
        public bool WriteString(string value)
        {
            try
            {
                ushort i = 0;
                while (i < value.Length)
                {
                    buf[(ushort)(i + pos)] = (byte)value[i];
                    i = (ushort)(i + 1);
                }
                pos += i;
                return true;
            }
            catch { return false; }
        }
        public bool WriteUInt16(ushort value, int offset)
        {
            try
            {
                buf[offset] = (byte)(value);
                buf[offset + 1] = (byte)(value >> 8);
                return true;
            }
            catch { return false; }
        }
        public bool WriteUInt32(uint value, int offset)
        {
            try
            {
                buf[offset] = (byte)(value); pos++;
                buf[offset + 1] = (byte)(value >> 8); pos++;
                buf[offset + 2] = (byte)(value >> 16); pos++;
                buf[offset + 3] = (byte)(value >> 24); pos++; return true;
            }
            catch { return false; }
        }
        public bool WriteUInt64(ulong value, int offset)
        {
            try
            {
                buf[offset] = (byte)(value);
                buf[offset + 1] = (byte)(value >> 8);
                buf[offset + 2] = (byte)(value >> 16);
                buf[offset + 3] = (byte)(value >> 24);
                buf[offset + 4] = (byte)(value >> 32);
                buf[offset + 5] = (byte)(value >> 40);
                buf[offset + 6] = (byte)(value >> 48);
                buf[offset + 7] = (byte)(value >> 56);
                return true;
            }
            catch { return false; }
        }
        public bool WriteByte(byte value, int offset)
        {
            try
            {
                buf[offset] = (byte)(value);
                return true;
            }
            catch { return false; }
        }
        public bool WriteStringWithLength(string value, int offset)
        {
            try
            {
                buf[offset] = (byte)value.Length;
                offset++;
                ushort i = 0;
                while (i < value.Length)
                {
                    buf[(ushort)(i + offset)] = (byte)value[i];
                    i = (ushort)(i + 1);
                }
                return true;
            }
            catch { return false; }
        }
        public bool WriteString(string value, int offset)
        {
            try
            {
                ushort i = 0;
                while (i < value.Length)
                {
                    buf[(ushort)(i + offset)] = (byte)value[i];
                    i = (ushort)(i + 1);
                }
                return true;
            }
            catch { return false; }
        }
        public bool StepOver(int length)
        {
            pos += length;
            return true;
        }
        public int Position
        {
            get { return pos; }
        }
        public byte[] Packet
        {
            get { return buf; }
        }
        public bool WriteTQServer()
        {
            pos = buf.Length - 8;
            WriteString("TQServer");
            return true;
        }
    }
    public class Packets
    {
        public static COPacket Donators(Game.Character C)
        {
            string str = C.EntityID.ToString() + " " + C.Nobility.Donation.ToString() + " " + ((byte)C.Nobility.Rank).ToString() + " " + C.Nobility.ListPlace.ToString();
            byte[] Packet = new byte[8 + 33 + str.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)2064);
            P.WriteInt32(3);
            P.WriteInt32(C.EntityID);
            P.Move(16);
            P.WriteByte(1);
            P.WriteByte((byte)str.Length);
            P.WriteString(str);
            return P;
        }
        public static COPacket DonateOpen(Game.Character C)
        {
            byte[] Packet = new byte[32 + 8];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));//length
            P.WriteInt16(2064);
            P.WriteInt32(4);
            P.WriteInt32(12);

            return P;
        }
        public static COPacket DonateOpen2(Game.Character C)
        {
            byte[] Packet = new byte[32 + 8];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));//length
            P.WriteInt16(2064);
            P.WriteInt32(4);
            if (C.Nobility.Rank < Game.Ranks.Duke)
                P.WriteInt32((uint)(Game.World.EmpireBoard[49].Donation + 1));
            else
            {
                if (C.Nobility.Rank == Ultimate.Game.Ranks.Duke)
                    P.WriteInt32((uint)(Game.World.EmpireBoard[15].Donation + 1));
                else if (C.Nobility.Rank == Ultimate.Game.Ranks.Prince)
                    P.WriteInt32((uint)(Game.World.EmpireBoard[3].Donation + 1));
                else
                    P.WriteInt32(0);
            }
            P.Move(8);
            P.WriteInt32(60);
            P.WriteInt32(uint.MaxValue);

            return P;
        }
        public static COPacket SendTopDonaters(uint Page)
        {
            string Str = "";
            for (int i = (int)(Page * 10); i < Page * 10 + 10; i++)
            {
                if (Game.World.EmpireBoard[i].Donation != 0)
                {
                    int PotGet = 7;
                    if (i < 15) PotGet = 9;
                    if (i < 3) PotGet = 12;

                    string nStr = Game.World.EmpireBoard[i].ID + " 0 0 " + Game.World.EmpireBoard[i].Name + " " + Game.World.EmpireBoard[i].Donation + " " + PotGet + " " + i;
                    nStr = Convert.ToChar((byte)nStr.Length) + nStr;
                    Str += nStr;
                }
            }
            byte[] Packet = new byte[32 + Str.Length + 8];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));//length
            P.WriteInt16(2064);
            P.WriteInt32(2);
            P.WriteInt16((ushort)Page);
            P.WriteInt32(5);
            P.Move(14);
            P.WriteByte(10);
            P.WriteString(Str);
            P.Move(3);

            return P;
        }


        //public static COPacket QuizShowStart(ushort qCount)
        //{
        //    byte[] Packet = new byte[20 + 8];
        //    COPacket P = new COPacket(Packet);
        //    P.WriteInt16(20);
        //    P.WriteInt16(2068);
        //    P.WriteInt16(1);//quiztype
        //    P.WriteInt16(31);//countdown
        //    P.WriteInt16(qCount);//questioncount
        //    P.WriteInt16(30);//questiontime
        //    P.WriteInt16(1800);//1st prize
        //    P.WriteInt16(1200);//2nd prize
        //    P.WriteInt16(600);//3rdprize
        //    return P;
        //}
        //public static COPacket QuizQuestion(uint currentscore, ushort timetaken, ushort prize, ushort rlq, ushort qn, string question, string answer1, string answer2, string answer3, string answer4)
        //{
        //    byte[] packet = new byte[19 + question.Length + 1 + answer1.Length + 1 + answer2.Length + 1 + answer3.Length + 1 + answer4.Length + 1 + 8];
        //    COPacket Packet = new COPacket(packet);
        //    Packet.WriteInt16((ushort)(packet.Length - 8));//length
        //    Packet.WriteInt16(2068);//packettype
        //    Packet.WriteInt16(2);//quiztype
        //    Packet.WriteInt16(qn);//questionid
        //    Packet.WriteInt16(0);//last question right answer
        //    Packet.WriteInt16(prize);//prize so far
        //    Packet.WriteInt16(timetaken);//time taken so far
        //    Packet.WriteInt32(currentscore);//current score
        //    Packet.WriteByte(5);
        //    char length = (char)question.Length;
        //    Packet.WriteString(length.ToString());
        //    Packet.WriteString(question);
        //    Packet.WriteByte((byte)answer1.Length);
        //    Packet.WriteString(answer1);
        //    Packet.WriteByte((byte)answer2.Length);
        //    Packet.WriteString(answer2);
        //    Packet.WriteByte((byte)answer3.Length);
        //    Packet.WriteString(answer3);
        //    Packet.WriteByte((byte)answer4.Length);
        //    Packet.WriteString(answer4);
        //    return Packet;
        //}
        //public static COPacket QuizShowInfo(ushort score, ushort timetaken, ushort rank)
        //{
        //    int llenght = 0;
        //    for (int x = 0; x < 3; x++)
        //    {
        //        int length = Program.MainQuizShowInfo.Name[x].Length + 1 + Program.MainQuizShowInfo.Score[x].ToString().Length + 1 + Program.MainQuizShowInfo.Time[x].ToString().Length;
        //        llenght += length;
        //    }
        //    byte[] packet = new byte[22 + llenght + 8];
        //    COPacket Packet = new COPacket(packet);
        //    Packet.WriteInt16((ushort)(packet.Length - 8));//length
        //    Packet.WriteInt16(2068);//packettype
        //    Packet.WriteInt16(4);//quiztype
        //    Packet.WriteInt16(score);//doesntwork
        //    Packet.WriteInt16(timetaken);//doesntwork
        //    Packet.WriteInt16(rank);//doesnt work.
        //    Packet.WriteInt32(0);//unknown
        //    Packet.WriteInt16(0);//unknown
        //    Packet.WriteByte(3);//leaders
        //    for (int x = 0; x < 3; x++)
        //    {
        //        int length = Program.MainQuizShowInfo.Name[x].Length + 1 + Program.MainQuizShowInfo.Score[x].ToString().Length + 1 + Program.MainQuizShowInfo.Time[x].ToString().Length;
        //        Packet.WriteByte((byte)length);
        //        Packet.WriteString(Program.MainQuizShowInfo.Name[x]);
        //        Packet.WriteByte(0x20);
        //        Packet.WriteString(Program.MainQuizShowInfo.Score[x].ToString());
        //        Packet.WriteByte(0x20);
        //        Packet.WriteString(Program.MainQuizShowInfo.Time[x].ToString());
        //    }
        //    return Packet;
        //}
        //public static COPacket QuizShowEnd(string Name, ushort score, ushort time, ushort rank, ushort prize)
        //{
        //    int length = Name.Length + 1 + score.ToString().Length + 1 + time.ToString().Length;
        //    byte[] packet = new byte[20 + length + 8];
        //    COPacket Packet = new COPacket(packet);
        //    Packet.WriteInt16((ushort)(20 + length));//length
        //    Packet.WriteInt16(2068);//packettype
        //    Packet.WriteInt16(5);//quiztype
        //    Packet.WriteInt16(rank);//rank
        //    Packet.WriteInt16(prize);//0
        //    Packet.WriteInt16(time);//time
        //    Packet.WriteInt16(score);//score
        //    Packet.WriteInt32(0);
        //    Packet.WriteByte(1);
        //    Packet.WriteByte((byte)length);
        //    Packet.WriteString(Name);//history name
        //    Packet.WriteByte(0x20);
        //    Packet.WriteString(score.ToString());//history score
        //    Packet.WriteByte(0x20);
        //    Packet.WriteString(time.ToString());//history time
        //    return Packet;
        //}
        public static COPacket AddStallItem(Game.Item I, Features.PersonalShops.ItemValue Val, uint StallID)
        {
            byte[] Packet = new byte[8 + 56];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x454);
            P.WriteInt32(I.UID);
            P.WriteInt32(StallID);
            P.WriteInt32(Val.Value);
            P.WriteInt32(I.ID);
            P.WriteInt16(I.CurDur);
            P.WriteInt16(I.MaxDur);
            P.WriteInt32(Val.MoneyType);
            P.WriteInt32(0);
            P.WriteByte((byte)I.Soc1);
            P.WriteByte((byte)I.Soc2);
            P.WriteInt16((ushort)I.Effect);
            P.WriteByte(I.Plus);
            P.WriteInt16(I.Bless);
            P.WriteByte(I.Enchant);
            if (I.RestrainType == 0)
                P.Move(6);
            else
            {
                P.WriteInt32(I.RestrainType);
                P.Move(2);
            }
            P.WriteInt16(0);
            P.WriteInt32((uint)I.Color);
            P.WriteInt32(I.Progress);

            return P;
        }
        public static COPacket MapStatus(uint Map, uint Status)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x456);
            P.WriteInt32(Map);
            P.WriteInt32(Map);
            P.WriteInt32(Status);

            return P;
        }
        public static COPacket FriendEnemyInfo(Game.Character C, byte Enemy)
        {
            int Leng = (16 - C.Spouse.Length);
            byte[] Packet = new byte[8 + 40];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f1);
            P.WriteInt32(C.EntityID);
            P.WriteInt32(uint.Parse(C.Avatar.ToString() + C.Body.ToString()));
            P.WriteByte(C.Level);
            P.WriteByte(C.Job);
            P.WriteInt16(C.PKPoints);
            if (C.MyGuild != null)
                P.WriteInt16(C.MyGuild.GuildID);
            else
                P.Move(2);

            P.WriteByte(0);
            P.WriteByte((byte)(C.GuildRank));
            P.WriteString(C.Spouse);
            for (int i = 0; i < Leng; i++)
            {
                P.WriteByte(0);
            }
            P.WriteInt32(Enemy);

            return P;
        }
        public static COPacket FriendEnemyPacket(uint uid, string name, byte Mode, byte Online)
        {
            byte[] Packet = new byte[8 + 36];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3fb);
            P.WriteInt32(uid);
            P.WriteByte(Mode);
            P.WriteByte(Online);
            P.Move(2);
            P.Move(4);
            P.WriteInt32(1);
            P.WriteString(name);

            return P;
        }
        public static COPacket TradePacket(uint UID, byte Type)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x420);
            P.WriteInt32(UID);
            P.WriteInt32(Type);

            return P;
        }
        public static COPacket TradeItem(Game.Item I)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f0);
            P.WriteInt32(I.UID);
            P.WriteInt32(I.ID);
            P.WriteInt16(I.CurDur);
            P.WriteInt16(I.MaxDur);
            P.WriteInt16(2);
            P.WriteInt16(0);
            P.WriteInt32(I.TalismanProgress);
            P.Move(0);
            P.WriteByte((byte)I.Soc1);
            P.WriteByte((byte)I.Soc2);
            P.WriteInt16((ushort)I.Effect);
            P.WriteByte(I.Plus);
            P.WriteByte(I.Bless);
            if (I.FreeItem)
                P.WriteByte(1);
            else
                P.WriteByte(0);
            P.WriteByte(I.Enchant);
            if (I.RestrainType == 0)
                P.Move(6);
            else
            {
                P.WriteInt32(I.RestrainType);
                P.Move(2);
            }
            P.WriteInt16(0);
            P.WriteInt32((uint)I.Color);

            P.WriteInt32(I.Progress);

            return P;
        }
        public static COPacket AddWHItem(Game.Character C, ushort NPC, Game.Item I)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44e);
            P.WriteInt32(NPC);
            P.Move(4);
            P.WriteInt32(1);
            if (I.ID != 0)
            {
                P.WriteInt32(I.UID);
                P.WriteInt32(I.ID);
                P.Move(1);
                P.WriteByte((byte)I.Soc1);
                P.WriteByte((byte)I.Soc2);
                P.WriteInt16((ushort)I.Effect);//0
                P.WriteByte(I.Plus);
                P.WriteByte(I.Bless);
                if (I.FreeItem)
                    P.WriteByte(1);
                else P.Move(1);
                P.WriteInt16(I.Enchant);
                P.WriteInt16((ushort)I.Effect);
                P.WriteByte(0);//<-- suspicious
                P.WriteByte(0);
                P.WriteByte(0);
                P.WriteByte((byte)I.Color);
                P.WriteInt32(I.TalismanProgress);
            }
            return P;
        }
        public static COPacket RemoveWHItem(Game.Character C, ushort NPC, Game.Item I)
        {
            byte[] Packet = new byte[48 + 8];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44e);
            P.WriteInt32(NPC);
            P.WriteInt32(2);
            P.WriteInt32(I.UID);
            P.Move(32);
            return P;
        }
        public static COPacket SendWarehouse(Game.Character C, ushort NPC)
        {
            List<Game.Item> Warehouse = null;
            switch (NPC)
            {
                case 8: { Warehouse = C.Warehouses.TCWarehouse; break; }
                case 10012: { Warehouse = C.Warehouses.PCWarehouse; break; }
                case 10028: { Warehouse = C.Warehouses.ACWarehouse; break; }
                case 10011: { Warehouse = C.Warehouses.DCWarehouse; break; }
                case 10027: { Warehouse = C.Warehouses.BIWarehouse; break; }
                case 44: { Warehouse = C.Warehouses.MAWarehouse; break; }
                case 46: { Warehouse = C.Warehouses.MAWarehouse2; break; }
                case 4101: { Warehouse = C.Warehouses.SCWarehouse; break; }
                case 2100: { Warehouse = C.Warehouses.HouseWH1; break; }
                case 2101: { Warehouse = C.Warehouses.HouseWH2; break; }
                default: return new COPacket(new byte[0]);
            }
            uint length = 0;
            try
            {
                length = (uint)Math.Min(20, Warehouse.Count);//(Warehouse.Count > 20 ? 20 : Warehouse.Count);
            }
            catch { }
            byte[] Packet = new byte[8 + 16 + (32 * length)];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44e);
            P.WriteInt32(NPC);
            P.Move(4);
            P.WriteInt32(length);
            uint count = 0;
            foreach (Game.Item I in Warehouse)
            {
                if (I.ID != 0)
                {
                    count++;
                    if (count == length + 1)
                        return P;
                    if (count == 21)
                        return P;
                    P.WriteInt32(I.UID);
                    P.WriteInt32(I.ID);
                    P.Move(1);
                    P.WriteByte((byte)I.Soc1);
                    P.WriteByte((byte)I.Soc2);
                    P.WriteInt16((ushort)I.Effect);//0
                    P.WriteByte(I.Plus);
                    P.WriteByte(I.Bless);
                    if (I.FreeItem)
                        P.WriteByte(1);
                    else P.Move(1);
                    P.WriteInt16(I.Enchant);
                    P.WriteInt16((ushort)I.Effect);
                    P.WriteByte(0);//<-- suspicious
                    P.WriteByte(0);
                    P.WriteByte(0);//scoate asta
                    P.WriteByte((byte)I.Color);
                    P.WriteInt32(I.TalismanProgress);
                    P.WriteInt32(0);
                }
            }
            return P;
        }
        public static COPacket OpenWarehouse(ushort NPCID, uint Money)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f1);
            P.WriteInt32(NPCID);
            P.WriteInt32(Money);
            P.WriteInt32(0x09);
            P.WriteInt32((uint)Environment.TickCount);

            return P;
        }
        public static COPacket NPCSay(string Text)
        {
            byte[] Packet = new byte[8 + 17 + Text.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f0);
            P.Move(6);
            P.WriteByte(0xff);
            P.WriteByte(0x01);
            P.WriteByte(0x01);
            P.WriteByte((byte)Text.Length);
            P.WriteString(Text);
            P.Move(3);

            return P;
        }
        public static COPacket NPCLink(string Text, byte DialNr)
        {
            byte[] Packet = new byte[8 + 17 + Text.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f0);
            P.Move(6);
            P.WriteByte(DialNr);
            P.WriteByte(0x02);
            //5- listline
            //4- avatar
            //100 - create   0x064
            //101 - answer 0x065
            //102 - delete guild member 0x066
            P.WriteByte(0x01);
            P.WriteByte((byte)Text.Length);
            P.WriteString(Text);
            P.Move(3);

            return P;
        }
        public static COPacket NPCLink2(string Text, byte DialNr)
        {
            byte[] Packet = new byte[8 + 17 + Text.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f0);
            P.Move(6);
            P.WriteByte(DialNr);
            P.WriteByte(0x03);
            P.WriteByte(0x01);
            P.WriteByte((byte)Text.Length);
            P.WriteString(Text);
            P.Move(3);

            return P;
        }
        public static COPacket NPCSetFace(ushort Face)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f0);
            P.WriteInt16(0x0a);
            P.WriteInt16(0x0a);
            P.WriteInt16(Face);
            P.WriteByte(0xff);
            P.WriteByte(0x04);
            P.Move(4);

            return P;
        }
        public static COPacket PopUp(string Text, byte DialNr)
        {
            byte[] Packet = new byte[12 + 3 + Text.Length + 8];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f0);
            //P.Move(6);
            P.WriteInt32(0x0a);
            P.WriteInt16(0);
            P.WriteByte(DialNr);
            P.WriteByte(0x06);
            P.WriteByte((byte)(Text.Length));
            P.WriteString(1 + Text);
            P.Move(3);

            return P;
        }
        public static COPacket NPCFinish()
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7f0);
            P.Move(6);
            P.WriteByte(0xff);
            P.WriteByte(0x64);
            P.Move(4);

            return P;
        }
        public static COPacket ViewEquip(Game.Character C)
        {
            byte[] Packet = new byte[8 + 11 + C.Spouse.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f7);
            P.WriteInt32(C.EntityID);
            P.WriteByte(0x0a);//10 0x0a - effect // 0x10 16 vieweq  
            P.WriteByte(0x01);
            P.WriteByte((byte)(C.Spouse.Length));
            P.WriteString(C.Spouse);

            return P;
        }
        public static COPacket AddViewItem(uint Viewed, Game.Item I, byte Pos)
        {
            byte[] Packet = new byte[56];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f0);
            P.WriteInt32(Viewed);
            P.WriteInt32(I.ID);
            P.WriteInt16(I.CurDur);
            P.WriteInt16(I.MaxDur);
            P.WriteInt16(4);
            P.WriteInt16(Pos);
            P.WriteInt32(I.TalismanProgress);
            P.WriteByte((byte)I.Soc1);
            P.WriteByte((byte)I.Soc2);
            P.WriteInt16((ushort)I.Effect);//move 2 bytes
            P.WriteByte(I.Plus);
            P.WriteByte(I.Bless);
            if (I.FreeItem)
                P.WriteByte(1);
            else
                P.WriteByte(0);
            P.WriteByte(I.Enchant);
            if (I.RestrainType == 0)
                P.Move(6);
            else
            {
                P.WriteInt32(I.RestrainType);
                P.Move(2);
            }
            P.WriteInt16(0);
            if (I.Color == 0)
                I.Color = (Game.Item.ArmorColor)new Random().Next(1, 9);
            P.WriteInt32((uint)I.Color);

            P.WriteInt32(I.Progress);
            return P;
        }
        public static COPacket SpawnNamedNPC(Game.NPC NPC, string Name)
        {
            byte[] Packet = new byte[8 + 36 + Name.Length];

            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x455);
            P.WriteInt32(NPC.EntityID);
            P.Move(8);
            P.WriteInt16(NPC.Loc.X);
            P.WriteInt16(NPC.Loc.Y);
            P.WriteInt16((ushort)(NPC.Type + NPC.Direction));
            P.WriteInt16(NPC.Flags);
            P.WriteInt16(0);
            P.WriteByte(1);
            P.WriteByte((byte)Name.Length);
            P.WriteString(Name);

            return P;
        }
        public static COPacket SpawnNPCWithHP(uint EntityID, ushort Type, ushort Flags, Game.Location Loc, uint CurHP, uint MaxHP)
        {
            byte[] Packet = new byte[8 + 34];

            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1109);
            P.WriteInt32(EntityID);
            P.WriteInt32(MaxHP);
            P.WriteInt32(CurHP);
            P.WriteInt16(Loc.X);
            P.WriteInt16(Loc.Y);
            P.WriteInt16((ushort)Type);
            P.WriteInt16(Flags);
            P.WriteInt16(10);

            return P;
        }
        public static COPacket SpawnNPCWithHP(uint EntityID, ushort Type, ushort Flags, Game.Location Loc, bool Named, string Name, uint CurHP, uint MaxHP)
        {
            byte[] Packet = new byte[8 + 36 + Name.Length];

            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1109);
            P.WriteInt32(EntityID);
            P.WriteInt32(MaxHP);
            P.WriteInt32(CurHP);
            P.WriteInt16(Loc.X);
            P.WriteInt16(Loc.Y);
            P.WriteInt16((ushort)Type);
            P.WriteInt16(Flags);
            P.WriteInt16(11);
            if (Named)
            {
                P.WriteByte(1);
                P.WriteByte((byte)Name.Length);
                P.WriteString(Name);
            }

            return P;
        }
        public static COPacket SpawnNPCWithHP(Game.NPC NPC)
        {
            byte[] Packet = new byte[8 + 34];

            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x455);
            P.WriteInt32(NPC.EntityID);
            P.WriteInt32(NPC.MaxHP);
            P.WriteInt32(NPC.CurHP);
            P.WriteInt16(NPC.Loc.X);
            P.WriteInt16(NPC.Loc.Y);
            P.WriteInt16((ushort)(NPC.Type + NPC.Direction));
            P.WriteInt16(NPC.Flags);
            P.WriteInt16(0);

            return P;
        }
        public static COPacket SpawnNamedNPC2(Game.NPC NPC, string Name)
        {
            byte[] Packet = new byte[8 + 20 + Name.Length];

            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7ee);
            P.WriteInt32(NPC.EntityID);
            P.WriteInt16(NPC.Loc.X);
            P.WriteInt16(NPC.Loc.Y);
            P.WriteInt16((ushort)(NPC.Type + NPC.Direction));
            P.WriteInt32(NPC.Flags);
            P.WriteByte(1);
            P.WriteByte((byte)Name.Length);
            P.WriteString(Name);

            return P;
        }
        public static COPacket SpawnNPC(Game.NPC N)
        {
            byte[] Packet = new byte[8 + 36 + N.Name.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x7ee);
            P.WriteInt32(N.EntityID);
            P.WriteInt16(N.Loc.X);
            P.WriteInt16(N.Loc.Y);
            P.WriteInt16((ushort)(N.Type + N.Direction));
            P.WriteInt16(N.Flags);
            if (N.Name == "")
                P.WriteInt32(1);
            else
            {
                P.WriteInt16(0);
                P.WriteByte(1);
                P.WriteByte((byte)N.Name.Length);
                P.WriteString(N.Name);

            }
            return P;
        }
        public static COPacket ItemDrop(Game.DroppedItem I)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44d);
            P.WriteInt32(I.UID);
            P.WriteInt32(I.Info.ID);
            P.WriteInt16(I.Loc.X);
            P.WriteInt16(I.Loc.Y);
            P.WriteInt16(0x03);
            P.WriteInt16(0x01);

            return P;
        }
        public static COPacket ItemDropRemove(uint ItemUID, uint ItemID, ushort X, ushort Y)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44d);
            P.WriteInt32(ItemUID);
            P.WriteInt32(ItemID);
            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteInt16(0x03);
            P.WriteInt16(0x02);

            return P;
        }
        public static COPacket CastTrap(Game.MapEffect I)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44d);
            P.WriteInt32(I.UID);
            P.WriteInt32(I.Info.ID);
            P.WriteInt16(I.Loc.X);
            P.WriteInt16(I.Loc.Y);
            P.WriteInt16(0x03);
            P.WriteInt16(10);

            return P;
        }
        public static COPacket MapEffect(Game.MapEffect I)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44d);
            P.WriteInt32(I.UID);
            P.WriteInt32(I.Info.ID);
            P.WriteInt16(I.Loc.X);
            P.WriteInt16(I.Loc.Y);
            P.WriteInt16(0x03);
            P.WriteInt16(11);

            return P;
        }
        public static COPacket MapEffectRemove(uint ID, uint UID, ushort X, ushort Y)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44d);
            P.WriteInt32(ID);
            P.WriteInt32(UID);
            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteInt16(0x03);
            P.WriteInt16(12);

            return P;
        }
        public static COPacket TeamPacket(uint CharID, byte Mode)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3ff);
            P.WriteInt32(Mode);
            P.WriteInt32(CharID);

            return P;
        }
        public static COPacket PlayerJoinsTeam(Game.Character C)
        {
            byte[] Packet = new byte[8 + 36];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x402);
            P.WriteByte(0);
            P.WriteByte(1);
            P.WriteByte(1);
            P.WriteByte(1);
            P.WriteString((C.Name + C.MyClient.AuthInfo.Status));
            P.Move(16 - (C.Name + C.MyClient.AuthInfo.Status).Length);
            P.WriteInt32(C.EntityID);
            //P.WriteInt32(uint.Parse(C.Avatar.ToString() + C.Body.ToString()));
            P.WriteInt32(C.Mesh);

            P.WriteInt16(C.MaxHP);
            P.WriteInt16(C.CurHP);

            return P;
        }
        public static COPacket SkillUse(uint EntityID, uint Target, uint Damage, ushort SkillId, byte SkillLvl, ushort X, ushort Y)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x451);
            P.WriteInt32(EntityID);
            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteInt16(SkillId);
            P.WriteInt16(SkillLvl);
            P.WriteInt32(1);
            P.WriteInt32(Target);
            P.WriteInt32(Damage);


            return P;
        }
        public static COPacket SkillUse(Features.SkillsClass.SkillUse SU)
        {
            byte[] Packet = new byte[8 + 20 + (SU.MobTargets.Count + SU.PlayerTargets.Count + SU.NPCTargets.Count + SU.MiscTargets.Count + SU.CompTargets.Count) * 12];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x451);
            P.WriteInt32(SU.User.EntityID);
            P.WriteInt16(SU.AimX);
            P.WriteInt16(SU.AimY);
            P.WriteInt16(SU.Info.ID);
            P.WriteInt16(SU.Info.Level);
            P.WriteInt32((uint)(SU.MobTargets.Count + SU.PlayerTargets.Count + SU.NPCTargets.Count + SU.MiscTargets.Count + SU.CompTargets.Count));
            foreach (KeyValuePair<Game.Mob, uint> DE in SU.MobTargets)
            {
                P.WriteInt32(((Game.Mob)DE.Key).EntityID);
                P.WriteInt32((uint)DE.Value);
                P.Move(4);
            }
            foreach (KeyValuePair<Game.Character, uint> DE in SU.PlayerTargets)
            {
                P.WriteInt32(((Game.Character)DE.Key).EntityID);
                P.WriteInt32((uint)DE.Value);
                P.Move(4);
            }
            foreach (KeyValuePair<Game.NPC, uint> DE in SU.NPCTargets)
            {
                P.WriteInt32(((Game.NPC)DE.Key).EntityID);
                P.WriteInt32((uint)DE.Value);
                P.Move(4);
            }
            foreach (KeyValuePair<uint, uint> DE in SU.MiscTargets)
            {
                P.WriteInt32((uint)DE.Key);
                P.WriteInt32((uint)DE.Value);
                P.Move(4);
            }
            foreach (KeyValuePair<Game.Companion, uint> DE in SU.CompTargets)
            {
                P.WriteInt32(((Game.Companion)DE.Key).EntityID);
                P.WriteInt32((uint)DE.Value);
                P.Move(4);
            }
            //foreach (KeyValuePair<Game.AI, uint> DE in SU.AITargets)
            //{
            //    P.WriteInt32(((Game.AI)DE.Key).EntityID);
            //    P.WriteInt32((uint)DE.Value);
            //    P.Move(4);
            //}

            return P;
        }
        public static COPacket Traps(Features.SkillsClass.SkillUse SU, Game.Character C)
        {
            byte[] Packet = new byte[8 + 20 + 13];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x451);
            P.Move(4);
            P.Move(4);
            //P.WriteInt32(SU.User.EntityID);
            //P.WriteInt16(SU.AimX);
            //P.WriteInt16(SU.AimY);
            P.WriteInt16(SU.Info.ID);
            P.WriteInt16(SU.Info.Level);
            P.WriteInt32((uint)1);
            P.WriteInt32(C.EntityID);
            P.WriteInt32((uint)SU.Info.Damage);
            P.Move(4);

            return P;
        }
        public static COPacket Traps(uint Damage, Game.Character C)
        {
            byte[] Packet = new byte[8 + 20 + 13];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x451);
            P.Move(4);
            P.Move(4);
            //P.WriteInt32(SU.User.EntityID);
            //P.WriteInt16(SU.AimX);
            //P.WriteInt16(SU.AimY);
            P.WriteInt16(0);
            P.WriteInt16(0);
            P.WriteInt32((uint)1);
            P.WriteInt32(C.EntityID);
            P.WriteInt32(Damage);
            P.Move(4);

            return P;
        }
        public static COPacket ShakeScreen(uint Target)
        {
            byte[] Packet = new byte[8 + 20 + 13];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x451);
            P.Move(4);
            P.Move(4);
            P.WriteInt16(10183);
            P.WriteInt16(0);
            P.WriteInt32((uint)1);
            P.WriteInt32(Target);
            P.WriteInt32(0);
            P.Move(4);

            return P;
        }
        public static COPacket ItemPacket(uint UID, uint pos, byte type)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f1);
            P.WriteInt32(UID);
            P.WriteInt32(pos);
            P.WriteInt32(type);
            P.Move(12);

            return P;
        }
        //public static COPacket StringGuild(uint UID, byte Type, string name, byte Count)
        //{
        //    byte[] Packet = new byte[8 + 12 + name.Length];
        //    COPacket P = new COPacket(Packet);

        //    P.WriteInt16((ushort)(Packet.Length - 8));
        //    P.WriteInt16((ushort)0x3f7);
        //    P.WriteInt32(UID);
        //    P.WriteByte(Type);
        //    P.WriteByte(Count);
        //    //P.WriteByte((byte)(name.Length));
        //    P.WriteString(name);
        //    P.Move(2);

        //    return P;
        //}
        public static COPacket GuildInfo(Features.Guild TheGuild, Game.Character Player)
        {
            byte[] Packet = new byte[8 + 40];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x452);
            P.WriteInt32(TheGuild.GuildID);
            P.WriteInt32(Player.GuildDonation);
            P.WriteInt32(TheGuild.Fund);
            P.WriteInt32((uint)TheGuild.MembersCount);
            P.WriteByte((byte)Player.GuildRank);
            P.WriteString(TheGuild.Creator.MembName);
            P.Move(19 - TheGuild.Creator.MembName.Length);

            return P;
        }
        public static COPacket SendGuild(uint GuildID, byte Type)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x453);
            P.WriteInt32(Type);
            P.WriteInt32(GuildID);

            return P;
        }
        public static COPacket SpawnEntity(Game.Character C)
        {
            byte[] Packet = new byte[8 + 138 + (C.Name + C.MyClient.AuthInfo.Status).Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10014);
            P.WriteInt32(C.Mesh);
            P.WriteInt32(C.EntityID);

            if (C.MyGuild != null)
            {
                P.WriteInt16(C.MyGuild.GuildID);//Guild ID
                P.Move(1);//Guild Branch ID maybe
                P.WriteByte((byte)C.GuildRank);
            }
            else
                P.Move(4);
            P.WriteInt64((ulong)C.StatEff.Value);

            if (C.Alive)
            {
                P.WriteInt32(C.Equips.HeadGear.ID);
                if (C.EventBase != null && C.EventBase.Teams.Count > 0)
                {
                    foreach (KeyValuePair<uint, Dictionary<uint, Game.Character>> T in C.EventBase.Teams)
                        if (T.Value.ContainsKey(C.EntityID))
                            P.WriteInt32(T.Key);
                }
                else if (C.Garment != 0)
                    P.WriteInt32(C.Garment);
                else
                    P.WriteInt32(C.Equips.Garment.ID);

                P.WriteInt32(C.Equips.Armor.ID);
                P.WriteInt32(C.Equips.LeftHand.ID);
                P.WriteInt32(C.Equips.RightHand.ID);
                P.WriteInt32(C.Equips.Steed.ID);
            }
            else P.Move(24);
            P.WriteInt32(12);
            P.WriteInt16((ushort)C.CurHP);
            P.WriteInt16(0);
            if (C.Alive)
                P.WriteInt16(C.Hair);
            else P.Move(2);
            P.WriteInt16(C.Loc.X);
            P.WriteInt16(C.Loc.Y);

            P.WriteByte(C.Direction);

            P.WriteByte(C.Action);
            P.Move(4);
            P.WriteByte(C.Reborns);
            P.WriteInt16(C.Level);
            P.WriteByte(0);//type 0 = screen / 1 = window
            P.Move(16);
            P.WriteInt32((byte)C.Nobility.Rank);
            P.WriteInt16((ushort)C.Equips.Armor.Color);
            P.WriteInt16((ushort)C.Equips.LeftHand.Color);
            P.WriteInt16((ushort)C.Equips.HeadGear.Color);
            P.WriteInt32(C.UniversityPoints);
            P.WriteInt16(C.Equips.Steed.Plus);
            P.WriteInt32(0);//Not sure
            P.WriteInt32(C.Equips.Steed.TalismanProgress);
            P.Move(24);
            P.WriteByte(1);
            P.WriteByte((byte)(C.Name + C.MyClient.AuthInfo.Status).Length);
            P.WriteString((C.Name + C.MyClient.AuthInfo.Status));

            return P;
        }
        //public static COPacket SpawnEntity(Game.AI C)
        //{
        //    byte[] Packet = new byte[8 + 138 + (C.Name + "").Length];
        //    COPacket P = new COPacket(Packet);

        //    P.WriteInt16((ushort)(Packet.Length - 8));
        //    P.WriteInt16((ushort)10014);
        //    P.WriteInt32(C.Mesh);
        //    P.WriteInt32(C.EntityID);

        //    P.Move(4);
        //    P.WriteInt64((ulong)C.StatEff.Value);

        //    if (C.Alive)
        //    {
        //        P.WriteInt32(C.Equips.HeadGear.ID);
        //        P.WriteInt32(C.Equips.Garment.ID);
        //        P.WriteInt32(C.Equips.Armor.ID);
        //        P.WriteInt32(C.Equips.LeftHand.ID);
        //        P.WriteInt32(C.Equips.RightHand.ID);
        //        P.WriteInt32(C.Equips.Steed.ID);
        //    }
        //    else P.Move(24);
        //    P.WriteInt32(12);
        //    P.WriteInt16(0);
        //    P.WriteInt16(0);
        //    if (C.Alive)
        //        P.WriteInt16(C.Hair);
        //    else P.Move(2);
        //    P.WriteInt16(C.Loc.X);
        //    P.WriteInt16(C.Loc.Y);

        //    P.WriteByte(C.Direction);

        //    P.WriteByte(C.Action);
        //    P.Move(4);
        //    P.WriteByte(0);
        //    P.WriteInt16(C.Level);
        //    P.WriteByte(0);//type 0 = screen / 1 = window
        //    P.Move(16);
        //    P.WriteInt32((byte)0);
        //    P.WriteInt16((ushort)C.Equips.Armor.Color);
        //    P.WriteInt16((ushort)C.Equips.LeftHand.Color);
        //    P.WriteInt16((ushort)C.Equips.HeadGear.Color);
        //    P.WriteInt32(0);
        //    P.WriteInt16(C.Equips.Steed.Plus);
        //    P.WriteInt32(0);//Not sure
        //    P.WriteInt32(C.Equips.Steed.TalismanProgress);
        //    P.Move(24);
        //    P.WriteByte(1);
        //    P.WriteByte((byte)(C.Name + "").Length);
        //    P.WriteString((C.Name + ""));

        //    return P;
        //}
        public static COPacket SpawnViewed(Game.Character C, byte Type)
        {
            byte[] Packet = new byte[8 + 138 + (C.Name + C.MyClient.AuthInfo.Status).Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10014);//0x3F7
            P.WriteInt32(C.Mesh);
            P.WriteInt32(C.EntityID);

            if (C.MyGuild != null)
            {
                P.WriteInt16(C.MyGuild.GuildID);//Guild ID
                P.Move(1);//Guild Branch ID maybe
                P.WriteByte((byte)C.GuildRank);
            }
            else
                P.Move(4);
            P.WriteInt64((ulong)C.StatEff.Value);

            if (C.Alive)
            {
                P.WriteInt32(C.Equips.HeadGear.ID);
                if (C.EventBase != null && C.EventBase.Teams.Count > 0)
                {
                    foreach (KeyValuePair<uint, Dictionary<uint, Game.Character>> T in C.EventBase.Teams)
                        if (T.Value.ContainsKey(C.EntityID))
                            P.WriteInt32(T.Key);
                }
                else if (C.Garment != 0)
                    P.WriteInt32(C.Garment);
                else
                    P.WriteInt32(C.Equips.Garment.ID);

                P.WriteInt32(C.Equips.Armor.ID);
                P.WriteInt32(C.Equips.LeftHand.ID);
                P.WriteInt32(C.Equips.RightHand.ID);
                P.WriteInt32(C.Equips.Steed.ID);
            }
            else P.Move(24);
            P.WriteInt32(12);
            P.WriteInt16(0);
            P.WriteInt16(0);
            if (C.Alive)
                P.WriteInt16(C.Hair);
            else P.Move(2);
            P.WriteInt16(C.Loc.X);
            P.WriteInt16(C.Loc.Y);

            P.WriteByte(C.Direction);
            P.WriteByte(C.Action);
            P.Move(4);
            P.WriteByte(C.Reborns);
            P.WriteInt16(C.Level);
            P.WriteByte(Type);//type 0 = screen / 1 = window
            P.Move(16);
            P.WriteInt32((byte)C.Nobility.Rank);
            P.WriteInt16((ushort)C.Equips.Armor.Color);
            P.WriteInt16((ushort)C.Equips.LeftHand.Color);
            P.WriteInt16((ushort)C.Equips.HeadGear.Color);
            P.WriteInt32(C.UniversityPoints);
            P.WriteInt16(C.Equips.Steed.Plus);
            P.WriteInt32(0);//Not sure
            P.WriteInt32(C.Equips.Steed.TalismanProgress);
            P.Move(24);
            P.WriteByte(1);
            P.WriteByte((byte)(C.Name + C.MyClient.AuthInfo.Status).Length);
            P.WriteString((C.Name + C.MyClient.AuthInfo.Status));
            return P;
        }
        public static COPacket AttackPacket(uint Attacker, uint Attacked, ushort X, ushort Y, uint Damage, byte AttackType)
        {
            byte[] Data = new byte[8 + 32];
            COPacket P = new COPacket(Data);

            P.WriteInt16((ushort)(Data.Length - 8));
            P.WriteInt16((ushort)0x3FE);
            P.Move(4);
            P.WriteInt32(Attacker);
            if (Damage != 0)
                P.WriteInt32(Attacked);
            else
                P.Move(4);
            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteInt32(AttackType);
            if (Damage != 0)
                P.WriteInt32(Damage);
            else
                P.Move(4);
            P.Move(4);

            return P;
        }
        public static COPacket Movement(uint UID, byte Dir)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10005);
            P.WriteInt32((byte)(Dir + 8 * Program.Rnd.Next(7)));
            P.WriteInt32(UID);
            P.WriteInt32((uint)Environment.TickCount);

            return P;
        }
        //public static COPacket String(uint UID, byte Type, string str)
        //{
        //    byte[] Packet = new byte[8 + 11 + str.Length];
        //    COPacket P = new COPacket(Packet);

        //    P.WriteInt16((ushort)(Packet.Length - 8));
        //    P.WriteInt16((ushort)0x3f7);
        //    P.WriteInt32(UID);
        //    P.WriteByte(Type);
        //    P.WriteByte(1);
        //    P.WriteByte((byte)str.Length);
        //    P.WriteString(str);

        //    return P;
        //}
        public static COPacket StringPacket(uint UID, Game.StringType Type, string str, bool WhisperWindow = false)
        {
            byte[] Packet = new byte[8 + 11 + str.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f7);
            P.WriteInt32(UID);
            P.WriteByte((byte)Type);
            var NetStringPacker = str.Split(' ');
            P.WriteByte((byte)NetStringPacker.Length);
            foreach (string Word in NetStringPacker.ToList())
            {
                P.WriteByte((byte)Word.Length);
                string Word2 = Word;
                if (WhisperWindow)
                    Word2 = Word2.Replace('~', ' ');
                P.WriteString(Word2);
            }

            return P;
        }
        public static COPacket StringPacket(ushort X, ushort Y, Game.StringType Type, string str)
        {
            if (Type == Game.StringType.Sound)
            {
                str = $"sound/{str}.wav";
                str += " 1";
            }
            byte[] Packet = new byte[8 + 11 + str.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f7);
            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteByte((byte)Type);
            var NetStringPacker = str.Split(' ');
            P.WriteByte((byte)NetStringPacker.Length);
            foreach (string Word in NetStringPacker.ToList())
            {
                P.WriteByte((byte)Word.Length);
                P.WriteString(Word);
            }

            return P;
        }
        public static COPacket SpawnEntity(Game.Mob C)
        {
            byte[] Packet = new byte[8 + 138 + C.Name.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10014);
            P.WriteInt32(C.Mesh);
            P.WriteInt32(C.EntityID);
            P.WriteInt32(0);
            P.WriteInt64(0);//Status Effect   C.PoisonedInfo == null ? 0 : (ulong)0x2
            P.Move(28);
            P.WriteInt16((ushort)C.CurrentHP);
            P.WriteInt16(C.Level);
            P.Move(2);//Hair
            P.WriteInt16(C.Loc.X);
            P.WriteInt16(C.Loc.Y);
            P.WriteByte(C.Direction);
            P.WriteByte(C.Action);
            P.Move(72);
            P.WriteByte(1);
            P.WriteByte((byte)C.Name.Length);
            P.WriteString(C.Name);

            return P;
        }
        public static COPacket SpawnEntity(ushort Mesh, string Name, Game.Location Loc)
        {
            byte[] Packet = new byte[8 + 97 + Name.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10014);
            P.WriteInt32(Mesh);
            P.WriteInt32((uint)Program.Rnd.Next(400000, 500000));
            P.WriteInt64(0);//Status Effect
            P.Move(28);
            P.WriteInt16((ushort)65535);
            P.WriteInt16(130);
            P.Move(2);//Hair
            P.WriteInt16(Loc.X);
            P.WriteInt16(Loc.Y);
            P.WriteByte(0);
            P.WriteByte(100);
            P.Move(35);
            P.WriteByte(1);
            P.WriteByte((byte)Name.Length);
            P.WriteString(Name);

            return P;
        }
        public static COPacket SpawnEntity(Game.Companion Cmp)
        {
            byte[] Packet = new byte[8 + 138 + Cmp.Name.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10014);
            P.WriteInt32(Cmp.Mesh);
            P.WriteInt32(Cmp.EntityID);
            P.WriteInt32(0);
            P.WriteInt64(0);//Status Effect
            P.Move(28);
            P.WriteInt16((ushort)Cmp.CurHP);
            P.WriteInt16(Cmp.Level);
            P.Move(2);//Hair
            P.WriteInt16(Cmp.Loc.X);
            P.WriteInt16(Cmp.Loc.Y);
            P.WriteByte(Cmp.Direction);
            P.WriteByte(100);
            P.Move(72);
            P.WriteByte(1);
            P.WriteByte((byte)Cmp.Name.Length);
            P.WriteString(Cmp.Name);

            return P;
        }
        public static COPacket GeneralData(uint Identifier, uint Value1, ushort Value2, ushort Value3, ushort Type, uint Time)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10010);
            P.WriteInt32(Identifier);
            P.WriteInt32(Value1);
            P.WriteInt32((uint)Time);
            P.WriteInt32(Type);
            P.WriteInt16(Value2);
            P.WriteInt16(Value3);
            P.Move(4);
            return P;
        }
#warning Removed unsafe declaration and unsafe code
        public static /*unsafe*/ COPacket DHKeyPacket(string Key, byte[] ServerIV, byte[] ClientIV)
        {
            byte[] Junk = new byte[Program.Rnd.Next(8, 16)];

            for (int i = 0; i < 8; i++)
                Junk[i] = (byte)Program.Rnd.Next(byte.MaxValue);

            //fixed (byte* p = Junk)
            //{
            //    for (int i = 0; i < Junk.Length; i++)
            //        *(p + i) = (byte)Program.Rnd.Next(byte.MaxValue);
            //}

            byte[] Packet = new byte[321 + Junk.Length];
            COPacket P = new COPacket(Packet);
            try
            {
                for (int i = 0; i < 11; i++)
                    P.WriteByte((byte)Program.Rnd.Next(byte.MaxValue));
                P.WriteInt32((uint)(Packet.Length - 11));
                P.WriteInt32((uint)Junk.Length);
                P.WriteBytes(Junk);
                P.WriteInt32(8);
                P.WriteBytes(ServerIV);
                P.WriteInt32(8);
                P.WriteBytes(ClientIV);
                P.WriteInt32(128);
                P.WriteString("A320A85EDD79171C341459E94807D71D39BB3B3F3B5161CA84894F3AC3FC7FEC317A2DDEC83B66D30C29261C6492643061AECFCF4A051816D7C359A6A7B7D8FB");
                P.WriteInt32(2);
                P.WriteString("05");
                P.WriteInt32(128);
                P.WriteString(Key);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }

            return P;
        }
        public static COPacket Packet2048(uint CharUID)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)2048);
            P.WriteInt32(CharUID);
            P.WriteInt32(4);
            P.WriteInt32(0);

            return P;
        }
        public static COPacket Packet1032(uint CharUID)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1032);
            P.WriteInt32(CharUID);
            P.WriteInt32(0x1f);
            P.WriteInt32(0);

            return P;
        }
        public static COPacket GeneralData(uint Identifier, uint Value1, ushort Value2, ushort Value3, ushort Type, byte Direction)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10010);
            P.WriteInt32(Identifier);
            P.WriteInt32(Value1);
            P.WriteInt32(Native.timeGetTime());
            P.WriteInt16(Type);
            P.WriteInt16(Direction);
            P.WriteInt16(Value2);
            P.WriteInt16(Value3);
            P.Move(4);
            return P;
        }
        public static COPacket GeneralData(uint Identifier, uint Value1, ushort Value2, ushort Value3, ushort Type)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10010);
            P.WriteInt32(Identifier);
            P.WriteInt32(Value1);
            P.WriteInt32((uint)Native.timeGetTime());
            P.WriteInt32(Type);
            P.WriteInt16(Value2);
            P.WriteInt16(Value3);
            P.Move(4);
            return P;
        }
        public static COPacket GeneralData(uint Identifier, ushort Value1, ushort Value2, ushort Value3, ushort Value4, byte Direction, ushort Type)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10010);
            P.WriteInt32(Identifier);
            P.WriteInt16(Value1);
            P.WriteInt16(Value2);
            P.WriteInt32((uint)Native.timeGetTime());
            P.WriteInt32(Type);
            P.WriteInt16(Value3);
            P.WriteInt16(Value4);
            P.WriteInt16(Direction);
            return P;
        }

        public static COPacket AddItem(Game.Item I, byte Pos)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            try
            {
                P.WriteInt16((ushort)(Packet.Length - 8));
                P.WriteInt16((ushort)0x3f0);
                P.WriteInt32(I.UID);
                P.WriteInt32(I.ID);
                P.WriteInt16(I.CurDur);
                P.WriteInt16(I.MaxDur);
                P.WriteInt16(1);
                P.WriteInt16(Pos);
                P.WriteInt32(I.TalismanProgress);
                P.WriteByte((byte)I.Soc1);
                P.WriteByte((byte)I.Soc2);
                P.WriteInt16((ushort)I.Effect);
                P.WriteByte(I.Plus);
                P.WriteByte(I.Bless);
                if (I.FreeItem)
                    P.WriteByte(1);
                else
                    P.WriteByte(0);
                P.WriteByte(I.Enchant);
                if (I.RestrainType == 0)
                    P.Move(6);
                else
                {
                    P.WriteInt32(I.RestrainType);
                    P.Move(2);
                }
                P.WriteInt16(0);
                if (I.Color == 0)
                    I.Color = (Game.Item.ArmorColor)new Random().Next(1, 9);
                P.WriteInt32((uint)I.Color);
                P.WriteInt32(I.Progress);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            return P;
        }
        public static COPacket OverwriteGarment(uint ID)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            try
            {
                P.WriteInt16((ushort)(Packet.Length - 8));
                P.WriteInt16((ushort)0x3f0);
                P.WriteInt32(1);
                P.WriteInt32(ID);
                P.Move(4);
                P.WriteInt16(1);
                P.WriteInt16(9);
                P.Move(22);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            return P;
        }

        public static COPacket OverwriteWeapon(uint ID)
        {
            var Packet = new byte[8 + 48];
            var P = new COPacket(Packet);
            try
            {
                P.WriteInt16((ushort)(Packet.Length - 8));
                P.WriteInt16(0x3f0);
                P.WriteInt32(1);
                P.WriteInt32(ID);
                P.Move(4);
                P.WriteInt16(1);
                P.WriteInt16(4);
                P.Move(22);
            }
            catch (Exception Exc)
            {
                Game.World.ExcAdd += Exc.ToString() + "\r\n";
            }

            return P;
        }

        public static COPacket OverHand(uint ID)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            try
            {
                P.WriteInt16((ushort)(Packet.Length - 8));
                P.WriteInt16((ushort)0x3f0);
                P.WriteInt32(1);
                P.WriteInt32(ID);
                P.Move(4);
                P.WriteInt16(1);
                P.WriteInt16(4);
                P.Move(4);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            return P;
        }
        public static COPacket OverHand2(uint ID)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            try
            {
                P.WriteInt16((ushort)(Packet.Length - 8));
                P.WriteInt16((ushort)0x3f0);
                P.WriteInt32(1);
                P.WriteInt32(ID);
                P.Move(4);
                P.WriteInt16(1);
                P.WriteInt16(5);
                P.Move(15);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            return P;
        }
        public static COPacket UpdateItem(Game.Item I, byte Pos)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            try
            {
                P.WriteInt16((ushort)(Packet.Length - 8));
                P.WriteInt16((ushort)0x3f0);
                P.WriteInt32(I.UID);
                P.WriteInt32(I.ID);
                P.WriteInt16(I.CurDur);
                P.WriteInt16(I.MaxDur);
                P.WriteInt16(3);
                P.WriteInt16(Pos);
                P.WriteInt32(I.TalismanProgress);
                P.WriteByte((byte)I.Soc1);
                P.WriteByte((byte)I.Soc2);
                P.WriteInt16((ushort)I.Effect);
                P.WriteByte(I.Plus);
                P.WriteByte(I.Bless);
                if (I.FreeItem)
                    P.WriteByte(1);
                else
                    P.WriteByte(0);
                P.WriteByte(I.Enchant);
                if (I.RestrainType == 0)
                    P.Move(6);
                else
                {
                    P.WriteInt32(I.RestrainType);
                    P.Move(2);
                }
                P.WriteInt16(0);
                if (I.Color == 0)
                    I.Color = (Game.Item.ArmorColor)new Random().Next(1, 9);
                P.WriteInt32((uint)I.Color);
                P.WriteInt32(I.Progress);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            return P;
        }
        public static COPacket CharacterInfo(Game.Character C)
        {
            byte[] Packet = new byte[98 + (C.Spouse.Length) + (C.Name + C.MyClient.AuthInfo.Status).Length + 4];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3ee);
            P.WriteInt32(C.EntityID);
            P.WriteInt32(C.Mesh);
            P.WriteInt16(C.Hair);
            P.WriteInt32(C.Silvers);
            P.WriteInt32(C.CPs);
            P.WriteInt64((ulong)C.Experience);
            P.Move(20);
            P.WriteInt16(C.Str);
            P.WriteInt16(C.Agi);
            P.WriteInt16(C.Vit);
            P.WriteInt16(C.Spi);
            P.WriteInt16(C.StatPoints);
            P.WriteInt16((ushort)C.CurHP);
            P.WriteInt16((ushort)C.CurMP);
            P.WriteInt16(C.PKPoints);
            P.WriteByte(C.Level);
            P.WriteByte(C.Job);
            P.WriteByte(0);
            P.WriteByte(C.Reborns);
            P.WriteByte(0);
            P.WriteInt32(C.UniversityPoints);
            P.Move(12);
            P.WriteByte(2);
            P.WriteByte((byte)(C.Name + C.MyClient.AuthInfo.Status).Length);
            P.WriteString((C.Name + C.MyClient.AuthInfo.Status));
            P.WriteByte((byte)C.Spouse.Length);
            P.WriteString(C.Spouse);

            return P;
        }
        public static COPacket Skill(Game.Skill S)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x44f);
            P.WriteInt32(S.Exp);
            P.WriteInt16(S.ID);
            P.WriteInt16(S.Lvl);

            return P;
        }
        public static COPacket Prof(Game.Prof Prof)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x401);
            P.WriteInt32(Prof.ID);
            P.WriteInt32(Prof.Lvl);
            P.WriteInt32(Prof.Exp);

            return P;
        }
        public static COPacket Packet1012(uint UID)
        {
            byte[] Packet = new byte[8 + 32];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1012);
            P.WriteInt32(UID);
            P.Move(4);
            byte[] bb = new byte[16];
            for (int i = 0; i < 16; i++)
                bb[i] = (byte)Program.Rnd.Next(255);
            P.WriteBytes(bb);
            P.Move(4);

            return P;
        }
        public static COPacket Packet1025()
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1025);
            P.Move(4);
            P.WriteInt32(1);
            P.Move(4);

            return P;
        }
        public static COPacket Packet1012Time(uint UID)
        {
            byte[] Packet = new byte[8 + 32];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1012);
            P.WriteInt32(UID);
            P.WriteInt32(Native.timeGetTime());
            byte[] bb = new byte[16];
            for (int i = 0; i < 16; i++)
                bb[i] = (byte)Program.Rnd.Next(255);
            P.WriteBytes(bb);
            P.WriteInt32(0xf76d);

            return P;
        }
        public static COPacket Status56(uint UID, Game.Status Type, ulong Value)
        {
            byte[] Packet = new byte[8 + 48];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10017);
            P.WriteInt32(UID);
            P.WriteInt32(0x02);
            P.WriteInt32(10000000);
            P.Move(8);
            P.WriteInt32((uint)Type);
            P.WriteInt64((ulong)Value);
            P.Move(8);

            return P;
        }
        public static COPacket Status(uint UID, Game.Status Type, ulong Value)
        {
            byte[] Packet = new byte[8 + 36];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8)); // try it why would it be 16 sometimes the co packets get cut and an easy fix is doubleing there length + these pointers are messy i know but i don't have any ideea what pointers do/mean...
            P.WriteInt16((ushort)10017);//seems like it won't let me log-in cuz of the 16... it never got to this point // start it from here
            P.WriteInt32(UID);
            P.WriteInt32(0x01);
            P.WriteInt32((uint)Type);
            P.WriteInt64((ulong)Value);
            P.Move(12);

            return P;
        }
        public static COPacket Status2(uint UID, Game.Status Type, ulong Value, byte Type2)
        {
            byte[] Packet = new byte[8 + 36];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10017);
            P.WriteInt32(UID);
            P.WriteInt32(Type2);
            P.WriteInt32((uint)Type);
            P.WriteInt64((ulong)Value);
            P.Move(12);

            return P;
        }
        public static COPacket ChatMessage(uint MessageID, string From, string To, string Message, ushort Type, uint Mesh)
        {


            byte[] Packet = new byte[8 + 34 + Message.Length + From.Length + To.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3ec);
            P.WriteBytes(new byte[] { 0xff, 0xff, 0xff, 0x00 });
            P.WriteInt32(Type);
            P.WriteInt32(MessageID);
            P.WriteInt32(Mesh);
            P.WriteInt32(Mesh);
            P.WriteByte(4);//4
            P.WriteByte((byte)From.Length);
            P.WriteString(From);
            P.WriteByte((byte)To.Length);
            P.WriteString(To);
            P.Move(1);
            if (Message.Length < 255)
                P.WriteByte((byte)(Message.Length));
            else
                P.WriteByte(255);

            P.WriteString(Message, 255);
            P.Move(5);//6

            return P;
        }
        public static COPacket ChatMessage(uint MessageID, string From, string To, string Message, ushort Type, uint MeshTo, uint MeshFrom)
        {


            byte[] Packet = new byte[8 + 34 + Message.Length + From.Length + To.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3ec);
            //  P.WriteBytes(new byte[] { 0xff, 0xff, 0xff, 0x00 });

            P.WriteInt32(0x00FFFF);
            P.WriteInt32(Type);
            P.WriteInt32(MessageID);
            P.WriteInt32(MeshTo);
            P.WriteInt32(MeshFrom);
            P.WriteByte(4);
            P.WriteByte((byte)From.Length);
            P.WriteString(From);
            P.WriteByte((byte)To.Length);
            P.WriteString(To);
            //P.Move(1);
            P.WriteByte(0);
            P.WriteString("");
            if (Message.Length < 255)
                P.WriteByte((byte)(Message.Length));
            else
                P.WriteByte(255);

            P.WriteString(Message, 255);
            //P.Move(6);

            return P;
        }
        public static COPacket Time()
        {
            byte[] Packet = new byte[8 + 36];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1033);
            P.Move(4);
            P.WriteInt32((uint)(DateTime.Now.Year - 1900));
            P.WriteInt32((uint)(DateTime.Now.Month - 1));
            P.WriteInt32((uint)(DateTime.Now.DayOfYear));
            P.WriteInt32((uint)(DateTime.Now.Day));
            P.WriteInt32((uint)(DateTime.Now.Hour));
            P.WriteInt32((uint)(DateTime.Now.Minute));
            P.WriteInt32((uint)(DateTime.Now.Second));

            return P;
        }
        public static COPacket Vigor(uint Amount)
        {
            byte[] Packet = new byte[8 + 36];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1033);
            P.WriteInt32(2);
            P.WriteInt32(Amount);

            return P;
        }
        public static COPacket SystemMessage(uint MessageID, string Message)
        {
            byte[] Packet = new byte[8 + 50 + Message.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1004);
            P.WriteBytes(new byte[] { 0xff, 0xff, 0xff, 0x00 });
            P.WriteInt32(0x835);
            P.WriteInt32(MessageID);
            P.Move(8);
            P.WriteByte(4);
            P.WriteByte(6);
            P.WriteString("SYSTEM");
            P.WriteByte(8);
            P.WriteString("ALLUSERS");
            P.Move(1);
            P.WriteByte((byte)Message.Length);
            P.WriteString(Message);
            P.Move(7);

            return P;
        }
        public static COPacket PopUpMessage(uint MessageID, string Message)
        {
            byte[] Packet = new byte[8 + 43 + Message.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3ec);
            P.WriteBytes(new byte[] { 0xff, 0xff, 0xff, 0x00 });
            P.WriteInt32(0x834);
            P.WriteInt32(MessageID);
            P.Move(8);
            P.WriteByte(4);
            P.WriteByte(6);
            P.WriteString("SYSTEM");
            P.WriteByte(8);
            P.WriteString("ALLUSERS");
            P.Move(1);
            P.WriteByte((byte)Message.Length);
            P.WriteString(Message);
            P.Move(3);

            return P;
        }

        //public static COPacket InterserverAuthentication(uint id, uint data, string ip, ushort port)
        //{
        //    byte[] Packet = new byte[40];
        //    COPacket P = new COPacket(Packet);
        //    P.WriteInt16((ushort)(Packet.Length - 8));//2
        //    P.WriteInt16(0x41f);//4
        //    P.WriteInt32(id);//8
        //    P.WriteInt32(data);//12
        //    P.WriteString(ip);//28
        //    P.Move(16 - ip.Length);//Padding
        //    P.WriteInt32(port);//32
        //    P.Move(Packet.Length - 8 - P.Count);
        //    P.WriteString("TQServer");//40
        //    return P;
        //}
        public static COPacket SendAuthentication(string ip, ulong hash)
        {
            byte[] Packet = new byte[32];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)Packet.Length);
            P.WriteInt16(0x41f);
            P.WriteInt64(hash);
            P.WriteString(ip);
            P.Move(16 - ip.Length);
            if (!Game.World.LowRatedServer)
                P.WriteInt16(5816);
            else
                P.WriteInt16(5817);
            return P;
        }
        public static COPacket WrongAuth()
        {
            byte[] Packet = new byte[32];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)Packet.Length);
            P.WriteInt16(0x41f);
            P.Move(4);
            P.WriteInt32((uint)1);
            P.WriteByte(0xd5);
            P.WriteByte(0xca);
            P.WriteByte(0xba);
            P.WriteByte(0xc5);
            P.WriteByte(0xc3);
            P.WriteByte(0xfb);
            P.WriteByte(0xbb);
            P.WriteByte(0xf2);
            P.WriteByte(0xbf);
            P.WriteByte(0xda);
            P.WriteByte(0xc1);
            P.WriteByte(0xee);
            P.WriteByte(0xb4);
            P.WriteByte(0xed);

            return P;
        }
        public static COPacket Weather(uint Type, uint Intensity, uint Direction, uint Appearence)
        {
            byte[] Packet = new byte[8 + 20];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3f8);
            P.WriteInt32(Type);
            P.WriteInt32(Intensity);
            P.WriteInt32(Direction);
            P.WriteInt32(Appearence);
            return P;
        }
        public static COPacket WindowWhisper(uint MessageID, string From, string To, string Message, ushort Type, uint MeshTo, uint MeshFrom)
        {
            //string[] var = Message.Split(' ');
            byte[] Packet = new byte[8 + 26 + Message.Length + From.Length + To.Length];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)0x3ec);

            P.WriteInt32(0x00FFFF);
            P.WriteInt32(Type);
            P.WriteInt32(MessageID);
            P.WriteInt32(MeshTo);
            P.WriteInt32(MeshFrom);
            P.WriteByte(4);
            P.WriteByte((byte)From.Length);
            P.WriteString(From);
            P.WriteByte((byte)To.Length);
            P.WriteString(To);
            //P.Move(1);
            P.WriteByte(0);
            P.WriteString("");
            if (Message.Length < 255)
                P.WriteByte((byte)(Message.Length));
            else
                P.WriteByte(255);

            P.WriteString(Message, 255);
            //P.Move(6);

            return P;
        }
        public static COPacket PlaceNPC(Game.NPC N)
        {
            byte[] Packet = new byte[8 + 16];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)2031);
            P.WriteInt32(0);
            P.WriteInt32(N.Type); //= 540 (0x1C, 0x02) or 10, 20, etc
            P.WriteInt16(5);
            P.WriteInt16(0x19); //= 25 (0x19)
            P.WriteInt16(0);
            return P;
        }
        public static COPacket Remove(Game.NPC N)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)2031);
            P.WriteInt32(N.EntityID);
            P.WriteInt32(N.Type);
            P.WriteByte(3);
            P.WriteInt16(0);

            return P;
        }
        public static COPacket UpdateCloudSaintJar(ushort CurrentKills, byte[] Data)
        {
            COPacket P = new COPacket(Data);

            P.Move(28);
            P.WriteInt16(CurrentKills);
            //Something

            return P;
        }
        public static COPacket UpdateCloudSaintJar(uint EntityID, byte MonsterType, ushort CurrentKills)
        {
            byte[] Packet = new byte[8 + 32];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)1022);

            uint unixTimestamp = (uint)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            P.WriteInt32(unixTimestamp);
            P.WriteInt32(EntityID);
            P.WriteInt32(EntityID);
            P.WriteInt16(MonsterType);
            P.Move(2);
            P.WriteInt16(36);
            P.Move(4);
            P.WriteInt16(MonsterType);
            //P.Move(1);
            P.WriteInt16(CurrentKills);
            //Something

            return P;
        }
        public static COPacket StatueWindow(Game.Character C, uint Subtype, uint ID, ushort Action, ushort Sort)
        {
            byte[] Packet = new byte[36];
            COPacket P = new COPacket(Packet);
            P.WriteInt16(28);
            P.WriteInt16(2031);
            P.WriteInt32(Subtype);
            P.WriteInt32(ID);
            P.WriteInt16(Action);
            P.WriteInt16(Sort);
            return P;
        }
        public static COPacket SpawnStatue(string Name, uint Mesh, uint ID, ushort GuildID, byte GuildRank, uint Headgear, uint Necklace, uint Ring, uint RightHand, uint LeftHand, uint Armor, uint Garment, ushort Hair, ushort X, ushort Y, ushort Frame, byte Direction, byte Action, ushort ArmorColor, ushort LeftHandColor, ushort HeadgearColor, ushort CurHP, ushort MaxHP)
        {
            byte[] Packet = new byte[8 + 138 + Name.Length];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16((ushort)10014);
            P.WriteInt32(Mesh);
            P.WriteInt32(ID);

            P.WriteInt16(GuildID);//Guild ID
            P.Move(1);//Guild Branch ID maybe
            P.WriteByte(GuildRank);

            if (GuildRank == (byte)Features.GuildRank.GuildLeader)
                P.WriteInt64(17179869184);
            else if (GuildRank == (byte)Features.GuildRank.DeputyManager)
                P.WriteInt64(34359738368);
            else
                P.Move(8);

            P.WriteInt32(Headgear);
            P.WriteInt32(Garment);
            P.WriteInt32(Armor);
            P.WriteInt32(LeftHand);
            P.WriteInt32(RightHand);
            P.WriteInt32(0);

            P.WriteInt16(12);
            P.Move(1);
            P.WriteInt16(CurHP);
            P.Move(1);
            P.WriteInt16(Frame);
            P.WriteInt16(Hair);

            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteByte(Direction);
            P.WriteByte(Action);

            P.Move(28);//16
            P.WriteInt16(ArmorColor);
            P.WriteInt16(LeftHandColor);
            P.WriteInt16(HeadgearColor);
            P.Move(38);//24
            P.WriteByte(1);
            P.WriteByte((byte)Name.Length);
            P.WriteString(Name);

            return P;
        }
        public static COPacket MsgDice(Features.MsgDice Dice)
        {
            byte[] Packet = new byte[8 + 28];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));//0
            P.WriteInt16((ushort)1113);//2
            P.WriteByte((byte)Dice.Action);//4
            P.WriteByte(Dice.Seconds);//5
            P.Move(2);
            P.WriteInt32(Dice.ID);//8
            P.WriteInt32(Dice.Number);//12
            if (Dice.Amount > 0)
                P.WriteInt32(Dice.Amount);//16
            else
            {
                P.WriteByte(Dice.Dice);
                P.WriteByte(Dice.Dice2);
                P.WriteByte(Dice.Dice3);
                P.WriteByte(Dice.UnKnown);
            }

            return P;
        }
        public static COPacket CustomDialog(PacketHandling.CustomDialog CustomDialog, ushort X, ushort Y)
        {
            byte[] Packet = new byte[8 + 21 + CustomDialog.ButtonCount * (24 + 64)];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1343);//2
            P.WriteInt32((uint)CustomDialog.AniId);
            P.WriteInt16(X);
            P.WriteInt16(Y);
            P.WriteInt16(CustomDialog.Width);
            P.WriteInt16(CustomDialog.Height);
            P.WriteByte(Convert.ToByte(CustomDialog.Permanent));
            P.WriteByte(Convert.ToByte(CustomDialog.PopUp));
            P.WriteByte(Convert.ToByte(CustomDialog.SystemMenu));
            P.WriteInt16(CustomDialog.ButtonCount);

            foreach (PacketHandling.CustomDialog.DlgBtnData Button in CustomDialog.Buttons)
            {
                P.WriteInt32((uint)Button.ButtonUID);
                P.WriteInt32((uint)Button.AniId);
                P.WriteInt16(Button.AniWidth);
                P.WriteInt16(Button.AniHeight);
                P.WriteInt16(Button.xpos);
                P.WriteInt16(Button.ypos);
                P.WriteInt16(Button.Width);
                P.WriteInt16(Button.Height);
                P.WriteInt32((uint)Button.TipColor);
                if (Button.TipStr.Length == 64)
                    P.WriteString(Button.TipStr);
                else
                {
                    P.WriteString(Button.TipStr);
                    P.Move(64 - Button.TipStr.Length);
                }

            }

            return P;
        }

        public static COPacket ShowDialog(int ID, byte Action)
        {
            byte[] Packet = new byte[9 + 8];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1344);//2
            P.WriteInt32((uint)ID);
            P.WriteByte(Action);

            return P;
        }

        public static COPacket MsgDlgImage(PacketHandling.MSG_DLG_IMAGE Image)
        {
            byte[] Packet = new byte[8 + 9 + (Image.ImgCount * 12)];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1350);//2
            P.WriteInt32(Image.DlgId);
            P.WriteByte(Image.ImgCount);
            foreach (PacketHandling.MSG_DLG_IMAGE.DlgImgData Img in Image.Images)
            {
                P.WriteInt32(Img.AniId);
                P.WriteInt16(Img.xpos);
                P.WriteInt16(Img.ypos);
                P.WriteInt16(Img.Width);
                P.WriteInt16(Img.Height);
            }

            return P;
        }
        public static COPacket DelDynImg(int DialogID, int ImgID)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1352);//2

            P.WriteInt32(DialogID);
            P.WriteInt32(ImgID);

            return P;
        }
        public static COPacket MsgDlgText(PacketHandling.MSG_DLG_Text Text)
        {
            uint count = 0;
            foreach (PacketHandling.MSG_DLG_Text.DlgTxtData Sentence in Text.Text)
                count += Sentence.TextLength;
            byte[] Packet = new byte[8 + 9 + (Text.TextCount * 14) + count];

            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1351);//2
            P.WriteInt32(Text.DlgId);
            P.WriteByte(Text.TextCount);
            foreach (PacketHandling.MSG_DLG_Text.DlgTxtData Sentence in Text.Text)
            {
                P.WriteInt32(Sentence.Id);
                P.WriteInt16(Sentence.xpos);
                P.WriteInt16(Sentence.ypos);
                P.WriteByte(Sentence.Fontsize);
                P.WriteInt32(Sentence.Color);
                P.WriteByte(Sentence.TextLength);
                P.WriteString(Sentence.Text);
            }

            return P;
        }
        public static COPacket DelDynTxt(int DialogID, int TxtID)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1353);//2

            P.WriteInt32(DialogID);
            P.WriteInt32(TxtID);

            return P;
        }

        public static COPacket PingPacket(byte[] Data)
        {
            byte[] Packet = new byte[Data.Length + 8];
            COPacket P = new COPacket(Packet);

            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(0x3F1);
            P.WriteInt32(BitConverter.ToUInt32(Data, 4));
            P.WriteInt32(BitConverter.ToUInt32(Data, 8));
            P.WriteInt32(27);
            //P.WriteInt32((uint)Environment.TickCount);
            //P.WriteInt32((uint)(/*BitConverter.ToUInt32(Data, 16)*/timeStamp + ((DateTime.Now.Ticks - Ticks) / 10000)));
            P.WriteInt32((uint)(BitConverter.ToUInt32(Data, 16) + new Random().Next(10, 20)));

            return P;
            //DateTime oldDate = new DateTime(1970, 1, 1).AddMilliseconds(BitConverter.ToUInt32(Data, 16));
            //DateTime curDate = DateTime.Now;

            //uint TimeFrame = (uint)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds - (uint)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds - BitConverter.ToUInt32(Data, 16);
            //P.WriteInt32((uint)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds - (uint)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds - BitConverter.ToUInt32(Data, 16));
            //uint FromClient = BitConverter.ToUInt32(Data, 16);
            //DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(FromClient);
            //DateTime dtDateTime2 = dtDateTime.AddSeconds();
            //DateTime dt =;
            //uint Diff1 = (uint)(DateTime.Now.Subtract(dtDateTime).TotalSeconds);
            //uint CurTime = (uint)DateTime.Now.To
            //DateTime dtOne = new DateTime(1970, 1, 1).AddMilliseconds(FromClient);
            //GC.LocalMessage(2000, $"{dtOne}");
            //uint Difference = ;
            //DateTime dtTwo = new DateTime(1970, 1, 1).AddSeconds();
            //GC.LocalMessage(2000, $"Server Time: {Difference}");
            //GC.LocalMessage(2000, $"Difference: {Difference - FromClient}");
            //GC.LocalMessage(2000, $"EID: {GC.MyChar.EntityID}");
            //dtDateTime = dtDateTime.AddSeconds(Difference).ToUniversalTime();
            //uint Ping = Difference - FromClient;
            //P.WriteInt32(Difference);
            //int Year = DateTime.Now.Year - 1900;
            //int Month = DateTime.Now.Month - 1;
            //uint unixTimestamp = (uint)(DateTime.Now.Subtract(new DateTime(Year, Month, 0))).TotalSeconds;
            //P.WriteInt32(TimeFrame);
            //P.WriteInt32(unixTimestamp);
        }

        public static COPacket DynamicButton(int DialogID, PacketHandling.CustomDialog.DlgBtnData Button)
        {
            byte[] Packet = new byte[8 + 24 + 64];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1354);//2

            P.WriteInt32((uint)DialogID);
            P.WriteInt32((uint)Button.ButtonUID);
            P.WriteInt32((uint)Button.AniId);
            P.WriteInt16(Button.AniWidth);
            P.WriteInt16(Button.AniHeight);
            P.WriteInt16(Button.xpos);
            P.WriteInt16(Button.ypos);
            P.WriteInt16(Button.Width);
            P.WriteInt16(Button.Height);
            P.WriteInt32((uint)Button.TipColor);
            if (Button.TipStr.Length == 64)
                P.WriteString(Button.TipStr);
            else
            {
                P.WriteString(Button.TipStr);
                P.Move(64 - Button.TipStr.Length);
            }

            return P;
        }
        public static COPacket RemoveButton(int DialogID, int ButtonID)
        {
            byte[] Packet = new byte[8 + 12];
            COPacket P = new COPacket(Packet);
            P.WriteInt16((ushort)(Packet.Length - 8));
            P.WriteInt16(1355);//2

            P.WriteInt32((uint)DialogID);
            P.WriteInt32(ButtonID);

            return P;
        }

        /* public static COPacket SpawnEntity(Game.Companion Cmp)
         {
             byte[] Packet = new byte[8 + 97 + Cmp.Name.Length];
             COPacket P = new COPacket(Packet);

             P.WriteInt16((ushort)(Packet.Length - 8));
             P.WriteInt16((ushort)10014);
             P.WriteInt32(Cmp.Mesh);
             P.WriteInt32(Cmp.EntityID);
             P.WriteInt64(0);//Status Effect
             P.Move(28);
             P.WriteInt16(Cmp.CurHP);
             P.WriteInt16(Cmp.Level);
             P.Move(2);//Hair
             P.WriteInt16(Cmp.Loc.X);
             P.WriteInt16(Cmp.Loc.Y);
             P.WriteByte(0);
             P.WriteByte(100);
             P.Move(35);
             P.WriteByte(1);
             P.WriteByte((byte)Cmp.Name.Length);
             P.WriteString(Cmp.Name);

             return P;
         } */
        /*  public static COPacket BoardMessage(ushort Board, byte Action,ushort Size, ConcurrentDictionary<uint, Game.MessageBoard> Dict)
          {
              ushort NSize = Size;
              foreach (Game.MessageBoard MB in Dict.Values)
              {
                  NSize += (ushort)MB.Name.Length;
                  NSize += (ushort)Math.Min(31, MB.Msg.Length);
                  NSize += (ushort)10;
              }
              byte[] Packet = new byte[8 + 11 + NSize];
              COPacket P = new COPacket(Packet);
              P.WriteInt16((ushort)(Packet.Length - 8));
              P.WriteInt16((ushort)0x457);
              P.WriteInt16(0);
              P.WriteInt16(0x899);//Board
              P.WriteByte(Action);//Action
              P.WriteInt16(Size);
              foreach (Game.MessageBoard MB in Dict.Values)
              {
                  P.WriteByte((byte)MB.Name.Length);
                  P.WriteString(MB.Name);
                  int Len = MB.Msg.Length;
                  string Time = MB.Time.ToString();
                  P.WriteByte((byte)Math.Min(31, Len));
                  if (Len > 31)
                      P.WriteString(MB.Msg.Remove(31));
                  else P.WriteString(MB.Msg);
                  P.WriteByte((byte)Time.Length);
                  P.WriteString(Time);
              }
              return P;
          }*/
        //public static COPacket WhisperWindow(uint UID, byte Type, string[] str)
        //{
        //    byte[] Packet = new byte[8 + 13 + str[0].Length + str[1].Length];
        //    COPacket P = new COPacket(Packet);

        //    P.WriteInt16((ushort)(Packet.Length - 8));
        //    P.WriteInt16((ushort)1015);
        //    P.WriteInt32(UID);
        //    P.WriteByte(Type);
        //    P.WriteByte(Convert.ToByte(str.Length));
        //    for (int a = 0; a < str.Length; a++)
        //    {
        //        P.WriteByte((byte)str[a].Length);
        //        P.WriteString(str[a]);
        //    }

        //    return P;
        //}
        //public static COPacket GuildMemberPacket(Features.Guild G)
        //{
        //    var _memberList = new List<MemberInfo>();
        //    foreach (KeyValuePair<byte, Dictionary<uint, MemberInfo>> D in G.Members)
        //    {
        //        foreach (MemberInfo M in D.Value.Values)
        //            if (M.IsOnline)
        //                _memberList.Add(M);
        //    }
        //    var count = 0;
        //    foreach (MemberInfo M in _memberList)
        //    {
        //        count += M.MembName.Length;
        //        count += 5;
        //    }

        //    byte[] Packet = new byte[count + 16 + 8];
        //    COPacket P = new COPacket(Packet);

        //    P.WriteInt16((ushort)(Packet.Length - 8));
        //    P.WriteInt16(0x836);//2102
        //    P.WriteInt32(0);//Unknown 4
        //    P.WriteInt32(0);//Page
        //    P.WriteInt32(12);//Amount
        //    foreach (MemberInfo M in _memberList)
        //    {
        //        P.WriteString(M.MembName);
        //        P.WriteByte((byte)M.Rank);
        //        P.WriteInt32((uint)M.Donation);
        //    }

        //    return P;

        //}

        //public static void SendMembers(Main.GameClient client, ushort page)
        //{
        //    System.IO.MemoryStream strm = new MemoryStream();
        //    BinaryWriter wtr = new BinaryWriter(strm);
        //    wtr.Write((ushort)0);
        //    wtr.Write((ushort)2102);
        //    wtr.Write((uint)0);
        //    wtr.Write((uint)page);
        //    int left = (int)MemberCount - page;
        //    if (left > 12)
        //        left = 12;
        //    if (left < 0)
        //        left = 0;
        //    wtr.Write((uint)left);
        //    int count = 0;
        //    int maxmem = page + 12;
        //    int minmem = page;
        //    List<Features.MemberInfo> online = new List<Features.MemberInfo>(250);
        //    List<Features.MemberInfo> offline = new List<Features.MemberInfo>(250);
        //    foreach (KeyValuePair<byte, Dictionary<uint, Features.MemberInfo>> D in client.MyChar.MyGuild.Members)
        //    {
        //        foreach (Features.MemberInfo M in D.Value.Values)
        //        {
        //            if (Game.World.H_Chars.ContainsKey(M.MembID))
        //                online.Add(M);
        //            else
        //                offline.Add(M);
        //        }
        //    }
        //    var unite = online.Union<Features.MemberInfo>(offline);
        //    foreach (Features.MemberInfo member in unite)
        //    {
        //        if (count >= minmem && count < maxmem)
        //        {
        //            for (int i = 0; i < 16; i++)
        //            {
        //                if (i < member.MembName.Length)
        //                    wtr.Write((byte)member.MembName[i]);
        //                else
        //                    wtr.Write((byte)0);
        //            }
        //            //wtr.Write((uint)member.NobilityRank);
        //            //wtr.Write((uint)(member.Gender + 1));
        //            //wtr.Write((uint)member.Level);
        //            wtr.Write((uint)member.Rank);
        //            //wtr.Write((uint)0);
        //            wtr.Write((uint)member.Donation);
        //            //wtr.Write((uint)(member.IsOnline ? 1 : 0));
        //            //wtr.Write((uint)0);
        //        }
        //        count++;
        //    }
        //    foreach (Features.MemberInfo member in Members.Values)
        //    {
        //        if (count >= minmem && count < maxmem)
        //        {
        //            for (int i = 0; i < 16; i++)
        //            {
        //                if (i < member.Name.Length)
        //                {
        //                    wtr.Write((byte)member.Name[i]);
        //                }
        //                else
        //                    wtr.Write((byte)0);
        //            }
        //            wtr.Write((uint)member.NobilityRank);
        //            wtr.Write((uint)(member.Gender + 1));
        //            wtr.Write((uint)member.Level);
        //            wtr.Write((uint)member.Rank);
        //            wtr.Write((uint)0);
        //            wtr.Write((uint)member.SilverDonation);
        //            wtr.Write((uint)(member.IsOnline ? 1 : 0));
        //            wtr.Write((uint)0);
        //        }
        //        count++;
        //    }
        //    int packetlength = (int)strm.Length;
        //    strm.Position = 0;
        //    wtr.Write((ushort)packetlength);
        //    strm.Position = strm.Length;
        //    wtr.Write(ASCIIEncoding.ASCII.GetBytes("TQServer"));
        //    strm.Position = 0;
        //    byte[] buf = new byte[strm.Length];
        //    strm.Read(buf, 0, buf.Length);
        //    wtr.Close();
        //    strm.Close();
        //    client.Send(buf);
        //}
    }
}
