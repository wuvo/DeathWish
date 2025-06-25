using NewestCOServer.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.Features
{
    public class Lottery
    {
        public static PrizeInfo[] AllPrizes = new PrizeInfo[0];
        public static void LoadLotto()
        {        
            if (File.Exists(@"C:\OldCODB\Lottery.txt"))
            {
                string[] Lines = File.ReadAllLines(@"C:\OldCODB\Lottery.txt");
                AllPrizes = new PrizeInfo[Lines.Count()];
                byte Pos = 0;
                foreach (string _line in Lines)
                {
                    string[] Info = _line.Split(' ');
                    Features.Lottery.PrizeInfo P = new Features.Lottery.PrizeInfo();
                    P.Id = Convert.ToUInt32(Info[4]);
                    P.Rank = Convert.ToByte(Info[1]);
                    P.Chance = Convert.ToByte(Info[2]);
                    P.Name = Convert.ToString(Info[3]);
                    P.Socket = Convert.ToByte(Info[5]);
                    P.Plus = Convert.ToByte(Info[6]);
                    AllPrizes[Pos] = P;
                    Pos++;
                }
            }
        }
        public unsafe struct PrizeInfo
        {
            public uint Id;
            public byte Rank;
            public byte Chance;
            public string Name;
            public byte Socket;
            public byte Plus;
        }
        public static Boolean GenerateLoto(Character Player)
        {
            if (Player.Inventory.Count >= 40)
                return false;

            byte Rank = 8;
            if (MyMath.ChanceSuccess(35.0))
                Rank = 7;
            else if (MyMath.ChanceSuccess(25.0))
                Rank = 6;
            else if (MyMath.ChanceSuccess(15.0))
                Rank = 5;
            else if (MyMath.ChanceSuccess(10.0))
                Rank = 4;
            else if (MyMath.ChanceSuccess(5.0))
                Rank = 3;
            else if (MyMath.ChanceSuccess(2.5))
                Rank = 2;
            else if (MyMath.ChanceSuccess(1.0))
                Rank = 1;

            PrizeInfo[] Prizes = GetPrizes(Rank);
            if (Prizes.Length <= 0)
                return false;

            Int32 Pos = Program.Rnd.Next(0, Prizes.Length - 1);
            while (!MyMath.ChanceSuccess(Prizes[Pos].Chance))
                Pos = MyMath.Rnd.Next(0, Prizes.Length - 1);

            byte Gem1 = 0;
            byte Gem2 = 0;
            if (Prizes[Pos].Socket > 0)
                Gem1 = 255;
            if (Prizes[Pos].Socket > 1)
                Gem2 = 255;

            Item I = new Item();
            I.ID = Prizes[Pos].Id;
            I.Plus = Prizes[Pos].Plus;
            I.Soc1 = (Game.Item.Gem)Gem1;
            I.Soc2 = (Game.Item.Gem)Gem2;

            Player.AddItem(I);

            Player.Teleport(1036, 212, 196);
            Player.MyClient.LocalMessage(2005, "You won a " + I.DBInfo.Name + " from Lottery!");
            if (Rank <= 6)
                World.SendMsgToAll("SYSTEM", Player.Name + " has won a " + Prizes[Pos].Name + " from Lottery!", 2000, 0);
            return true;
        }
        private static PrizeInfo[] GetPrizes(Byte Rank)
        {
            List<PrizeInfo> Prizes = new List<PrizeInfo>();

            foreach (PrizeInfo Prize in AllPrizes)
            {
                if (Prize.Rank == Rank)
                    Prizes.Add(Prize);
            }
            return Prizes.ToArray();
        }
    }
}
