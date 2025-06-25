using System;
using System.Linq;
using System.Text;
using Ultimate.Features;
using Ultimate.Structures;

namespace Ultimate
{
    public class PacketHandler
    {
#warning Removed unsafe declaration and unsafe code at team member location
        public static /*unsafe*/ void Handle(Main.GameClient GC, byte[] PData)
        {
            if (Program.EndSession)
                return;
            if (GC == null || !GC.Soc.Connected || PData == null) return;
            int Position = 0;
            Random Rnd = new Random();
            var a = 0;
            while (Position < PData.Length)
            {
                ushort PacketID = 0;
                ushort PacketLength = 0;
                byte[] Data = null;
                try
                {
                    PacketLength = BitConverter.ToUInt16(PData, Position);
                    PacketID = BitConverter.ToUInt16(PData, Position + 2);
                    Data = new byte[PacketLength + 8];
                    Buffer.BlockCopy(PData, Position, Data, 0, PacketLength + 8);
                    if (!NewAntiCheat.IsValidTail(GC.SignatureKey, Data, true) && PacketID != 1009)
                    {
                        Console.WriteLine($"{GC.MyChar.Name} has sent broken tail.");
                        GC.Disconnect();
                        return;
                    }
                }
                catch { return; }
                try
                {
                    switch (PacketID)
                    {

                        case 0x25B0://9648 this is  the anti cheat packet.
                            Features.Anticheat.Handle(GC, Data);
                            break;
                        case 1345:
                            PacketHandling.CustomDialog.HandleButtons(GC.MyChar, BitConverter.ToUInt32(Data, 4), BitConverter.ToInt32(Data, 8));
                            break;
                        //case 2068:
                        //    {
                        //        try
                        //        {
                        //            byte Answered = (byte)(Data[8] - 1);
                        //            int L = Environment.TickCount;
                        //            int Now = (L - GC.MyChar.QuizShowInfo.LastAnswer) / 1000 + 1;
                        //            ushort qn = Features.QuizShow.QuestionNO;

                        //            GC.MyChar.QuizShowInfo.Time += (ushort)Now;
                        //            GC.MyChar.QuizShowInfo.Score += (ushort)((30 - Now) * Features.QuizShow.Questions[(ushort)(qn - 1)].Answers[Answered].Points);
                        //            GC.MyChar.UniversityPoints += (ushort)((30 - Now) * Features.QuizShow.Questions[(ushort)(qn - 1)].Answers[Answered].Points);
                        //            GC.LocalMessage(2000, "You have won " + ((30 - Now) * Features.QuizShow.Questions[(ushort)(qn - 1)].Answers[Answered].Points).ToString() + " quiz show points for this answer.");
                        //            uint exp = GC.MyChar.ExpBallExp / 1000000;
                        //            exp = (uint)(exp * ((30 - Now) * Features.QuizShow.Questions[(ushort)(qn - 1)].Answers[Answered].Points));
                        //            GC.MyChar.IncreaseExp(exp, false, false);

                        //            if (GC.MyChar.QuizShowInfo.Score > 0)
                        //            {
                        //                int MyPlace = 500;
                        //                for (int i = 499; i >= 0; i--)
                        //                {
                        //                    if (GC.MyChar.QuizShowInfo.Score >= Features.QuizShow.Scores[i].Score)
                        //                        MyPlace--;
                        //                }
                        //                if (MyPlace < 500)
                        //                {
                        //                    for (int i = 498; i >= MyPlace; i--)
                        //                        Features.QuizShow.Scores[i + 1] = Features.QuizShow.Scores[i];
                        //                    Features.QuizShow.QuizShowScore K = new Ultimate.Features.QuizShow.QuizShowScore();
                        //                    K.EntityID = GC.MyChar.EntityID;
                        //                    K.Score = GC.MyChar.QuizShowInfo.Score;
                        //                    Features.QuizShow.Scores[MyPlace] = K;
                        //                    GC.MyChar.QuizShowInfo.Rank = (ushort)(MyPlace + 1);
                        //                    if (GC.MyChar.EntityID == Ultimate.Features.QuizShow.Scores[0].EntityID)
                        //                    {
                        //                        Program.MainQuizShowInfo.Name[0] = GC.MyChar.Name;
                        //                        Program.MainQuizShowInfo.Score[0] = GC.MyChar.QuizShowInfo.Score;
                        //                        Program.MainQuizShowInfo.Time[0] = GC.MyChar.QuizShowInfo.Time;
                        //                    }
                        //                    else if (GC.MyChar.EntityID == Ultimate.Features.QuizShow.Scores[1].EntityID)
                        //                    {
                        //                        Program.MainQuizShowInfo.Name[1] = GC.MyChar.Name;
                        //                        Program.MainQuizShowInfo.Score[1] = GC.MyChar.QuizShowInfo.Score;
                        //                        Program.MainQuizShowInfo.Time[1] = GC.MyChar.QuizShowInfo.Time;
                        //                    }
                        //                    else if (GC.MyChar.EntityID == Ultimate.Features.QuizShow.Scores[2].EntityID)
                        //                    {
                        //                        Program.MainQuizShowInfo.Name[2] = GC.MyChar.Name;
                        //                        Program.MainQuizShowInfo.Score[2] = GC.MyChar.QuizShowInfo.Score;
                        //                        Program.MainQuizShowInfo.Time[2] = GC.MyChar.QuizShowInfo.Time;
                        //                    }

                        //                    GC.AddSend(Packets.QuizShowInfo(GC.MyChar.QuizShowInfo.Score, GC.MyChar.QuizShowInfo.Time, (ushort)MyPlace));
                        //                }
                        //                foreach (Game.Character Chrr in Game.World.H_Chars.Values)
                        //                {

                        //                    Main.GameClient G2C = Chrr.MyClient;
                        //                    G2C.AddSend(Packets.QuizShowInfo(G2C.MyChar.QuizShowInfo.Score, G2C.MyChar.QuizShowInfo.Time, G2C.MyChar.QuizShowInfo.Rank));
                        //                }
                        //            }

                        //        }
                        //        catch { }
                        //        break;
                        //    }
                        case 2064:
                            {
                                uint Type = BitConverter.ToUInt32(Data, 4);

                                if (Type == 2)//Open
                                {
                                    uint Page = BitConverter.ToUInt32(Data, 8);
                                    GC.AddSend(Packets.DonateOpen(GC.MyChar));
                                    GC.AddSend(Packets.SendTopDonaters(Page));
                                    GC.AddSend(Packets.DonateOpen2(GC.MyChar));
                                }
                                else if (Type == 4)//Open2
                                {

                                }
                                else if (Type == 1)
                                {
                                    uint Donation = BitConverter.ToUInt32(Data, 8);
                                    if (Donation <= GC.MyChar.Silvers)
                                    {
                                        GC.MyChar.Silvers -= Donation;
                                        GC.MyChar.Nobility.Donation += Donation;
                                        Game.World.NewEmpire(GC.MyChar, false);
                                    }
                                }
                                else
                                {
                                }
                                break;
                            }
                        /*   case 2065:
                               {

                                   uint UID = BitConverter.ToUInt32(Data, 12);
                                   uint Type = BitConverter.ToUInt32(Data, 4);

                                   Game.Character Request = (Game.Character)Game.World.H_Chars[UID];

                                   if (Type == 1)//RequestApprentice
                                   {
                                       //15 = AddFriend, 19 = AddEnemy, 14 = Remove
                                       GC.AddSend(Packets.MentorApprenticePacket(GC.MyChar.EntityID, UID, Request.Name, 1, 1));
                                   }
                                   else if (Type == 2)//RequestMentor
                                   {
                                   }

                                   break;
                               }*/
                        case 1024:
                            {
                                byte AddStr = Data[4];
                                byte AddAgi = Data[5];
                                byte AddVit = Data[6];
                                byte AddSpi = Data[7];
                                if (AddStr != 0)
                                {
                                    if (GC.MyChar.StatPoints == 0)
                                        return;
                                    GC.MyChar.StatPoints -= 1;
                                    GC.MyChar.Str += 1;
                                }
                                else if (AddAgi != 0)
                                {
                                    if (GC.MyChar.StatPoints == 0)
                                        return;
                                    GC.MyChar.StatPoints -= 1;
                                    GC.MyChar.Agi += 1;
                                }
                                else if (AddVit != 0)
                                {
                                    if (GC.MyChar.StatPoints == 0)
                                        return;
                                    GC.MyChar.StatPoints -= 1;
                                    GC.MyChar.Vit += 1;
                                }
                                else if (AddSpi != 0)
                                {
                                    if (GC.MyChar.StatPoints == 0)
                                        return;
                                    GC.MyChar.StatPoints -= 1;
                                    GC.MyChar.Spi += 1;
                                }
                                break;
                            }
                        case 2050:
                            {
                                if (Data[4] == 3 && GC.MyChar.Silvers >= 500000)
                                {
                                    if (Game.World.BroadCastCount < 100)
                                    {
                                        GC.MyChar.Silvers -= 500000;

                                        string Message = "";
                                        for (byte i = 0; i < Data[13]; i++)
                                            Message += Convert.ToChar(Data[14 + i]);


                                        Game.BroadCastMessage B = new Ultimate.Game.BroadCastMessage();
                                        B.Name = GC.MyChar.Name + GC.AuthInfo.Status;
                                        B.Message = Message;
                                        B.Place = Game.World.BroadCastCount;
                                        Game.World.BroadCasts[Game.World.BroadCastCount] = B;
                                        Game.World.BroadCastCount++;

                                        Data[8] = B.Place;
                                        GC.AddSend(Data);
                                    }
                                }
                                break;
                            }
                        case 1027:
                            PacketHandling.SocketGem.Handle(GC, Data);
                            break;
                        case 2036:
                            PacketHandling.Compose.Handle(GC, Data);
                            break;
                        case 1101:
                            PacketHandling.PickItemUp.Handle(GC, Data);
                            break;
                        case 1023:
                            PacketHandling.TeamHandle.Handle(GC, Data);
                            break;
                        case 1022:
                            {
                                uint AttackType = BitConverter.ToUInt32(Data, 20);
                                switch (AttackType)
                                {
                                    case 8:
                                    case 9: PacketHandling.Marriage.Handle(GC, Data); break;
                                    /* case 40:
                                         GC.AddSend(Packets.Status(GC.MyChar.EntityID, Ultimate.Game.Status.Merchant, 255));
                                         GC.MyChar.Merchant = Ultimate.Game.MerchantTypes.Yes;
                                         GC.AddSend(Data);
                                         break;*/
                                    case 36:
                                        {
                                            GC.AddSend(Packets.UpdateCloudSaintJar(GC.MyChar.CurrentKills, Data));
                                            GC.MyChar.ToKill = (Cloudsaint.MonsterType)Data[26];
                                            break;
                                        }
                                    default:
                                        if (GC.MyChar.Loc.Map != 1036 && GC.MyChar.Loc.Map != 1616 && GC.MyChar.Loc.Map != 1090 && GC.MyChar.Loc.Map != 2068)
                                        {
                                            /* #region GetSkillID
                                             ushort SkillId;
                                             SkillId = Convert.ToUInt16(((long)Data[24] & 0xFF) | (((long)Data[25] & 0xFF) << 8));
                                             SkillId ^= (ushort)0x915d;
                                             SkillId ^= (ushort)GC.MyChar.EntityID;
                                             SkillId = (ushort)(SkillId << 0x3 | SkillId >> 0xd);
                                             SkillId -= 0xeb42;
                                             #endregion*/
                                            //Console.WriteLine(SkillId);
                                            // if (SkillId != 1020)
                                            PacketHandling.Attack.Handle(GC, Data);
                                            /* else
                                             {
                                                 GC.MyChar.Intensify.X = GC.MyChar.Loc.X;
                                                 GC.MyChar.Intensify.Y = GC.MyChar.Loc.Y;
                                                 System.Timers.ElapsedEventHandler start = delegate { PacketHandling.Attack.HandleFireCircle(GC, Data); };
                                                // System.Threading.Timer Fire = new System.Threading.Timer(start, null, 2000, int.MaxValue);

                                                // System.Threading.Thread T = new System.Threading.Thread(start);
                                                // MyThread Fire = new MyThread();
                                                // Fire.Execute += new Execute(start);

                                                 //T.Start();

                                                // MyThread Att = new MyThread();
                                                 //Att.Execute += new Execute(PacketHandling.Attack.HandleFireCircle());
                                                 //PacketHandling.Attack.HandleFireCircle(GC, Data);
                                             }*/
                                        }
                                        break;
                                }
                                break;
                            }
                        case 2031:
                            {
                                GC.DialogNPC = BitConverter.ToUInt32(Data, 4);
                                if (Data[12] == 3)
                                {
                                    if (GC.MyChar.Loc.Map == GC.MyChar.EntityID || GC.AuthInfo.Status == "[PM]")
                                        if (GC.DialogNPC != 2100 && GC.DialogNPC != 2101)
                                            Features.HouseTable.InitializeRemoval(GC.MyChar, GC.DialogNPC);
                                }
                                else if (GC.DialogNPC < 20007 || GC.DialogNPC >= 29999)
                                    NPCs.NPCHandler.Handle(GC, Data, GC.DialogNPC, 0);
                                //PacketHandling.NPCDialog.Handle(GC, Data, GC.DialogNPC, 0);
                                break;
                            }
                        case 2032:
                            {
                                if (Data[10] != 0)
                                    NPCs.NPCHandler.Handle(GC, Data, GC.DialogNPC, Data[10]);
                                //PacketHandling.NPCDialog.Handle(GC, Data, GC.DialogNPC, Data[10]);
                                else
                                {
                                    byte NameLength = Data[13];
                                    string Name = "";
                                    for (byte i = 0; i < NameLength; i++)
                                        Name += Convert.ToChar(Data[14 + i]);

                                    if (GC.MyChar.MyGuild != null && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader && Name != GC.MyChar.Name && GC.MyChar.MyGuild.MembOfName(Name) != null)
                                    {
                                        GC.MyChar.MyGuild.MemberLeaves(Name, true);
                                        PacketHandling.MemberList.Handle(GC);
                                    }
                                }
                                break;
                            }
                        case 1009:
                            {
                                byte PacketType = Data[12];
                                switch (PacketType)
                                {
                                    case 1:
                                        PacketHandling.ItemPacket.Shops.BuyHandle(GC, Data);
                                        break;
                                    case 2:
                                        PacketHandling.ItemPacket.Shops.SellHandle(GC, Data);
                                        break;
                                    case 37:
                                        PacketHandling.ItemPacket.DropAnItem.Handle(GC, Data);
                                        break;
                                    case 38:
                                        PacketHandling.ItemPacket.DropMoney.Handle(GC, Data);
                                        break;
                                    case 4:
                                        PacketHandling.ItemPacket.Equip.HandleEquip(GC, Data);
                                        break;
                                    case 6:
                                        PacketHandling.ItemPacket.Equip.HandleUnEquip(GC, Data);
                                        break;
                                    case 9:
                                        uint NPC = BitConverter.ToUInt32(Data, 4);
                                        if (NPC != 0)
                                        {
                                            GC.AddSend(Packets.OpenWarehouse((ushort)NPC, GC.MyChar.WHSilvers));
                                        }
                                        break;
                                    case 10:
                                        PacketHandling.Warehouse.Deposit(GC, Data);
                                        break;
                                    case 11:
                                        PacketHandling.Warehouse.Withdraw(GC, Data);
                                        break;
                                    case 14:
                                        PacketHandling.ItemPacket.Repair.Handle(Data, GC);
                                        break;
                                    case 15:
                                        PacketHandling.ItemPacket.Repair.HandleVipRepair(GC);
                                        break;
                                    case 20:
                                        PacketHandling.ItemPacket.MeteorUpgrade.Handle(GC, Data);
                                        break;
                                    case 19:
                                        PacketHandling.ItemPacket.DBUpgrade.Handle(GC, Data);
                                        break;
                                    case 21:
                                        {
                                            uint StallID = BitConverter.ToUInt32(Data, 4);
                                            if (Game.World.H_PShops.ContainsKey(StallID))
                                            {
                                                Features.PersonalShops.Shop S = (Features.PersonalShops.Shop)Game.World.H_PShops[StallID];
                                                if (Game.World.H_Chars.ContainsKey(S.Owner.EntityID))
                                                    S.SendItems(GC);
                                                else
                                                    S.Close();
                                            }
                                            break;
                                        }
                                    case 22:
                                        {
                                            if (GC.MyChar.MyShop != null)
                                            {
                                                if (GC.MyChar.MyShop.AddItem(BitConverter.ToUInt32(Data, 4), BitConverter.ToUInt32(Data, 8), 1))
                                                    GC.AddSend(Data);
                                            }
                                            break;
                                        }
                                    case 29:
                                        {
                                            if (GC.MyChar.MyShop != null)
                                            {
                                                if (GC.MyChar.MyShop.AddItem(BitConverter.ToUInt32(Data, 4), BitConverter.ToUInt32(Data, 8), 3))
                                                {
                                                    GC.AddSend(Data);
                                                    // GC.AddSend(Packets.ItemPacket(UID, NPCInfo.EntityID, 23));
                                                    //GC.AddSend(Packets.AddStallItem(I, (ItemValue)Items[UID], NPCInfo.EntityID));
                                                }
                                            }
                                            break;
                                        }
                                    case 23:
                                        {
                                            if (GC.MyChar.MyShop != null)
                                                GC.MyChar.MyShop.RemoveItem(BitConverter.ToUInt32(Data, 4), Data);
                                            break;
                                        }
                                    case 24:
                                        {
                                            uint ItemUID = BitConverter.ToUInt32(Data, 4);
                                            uint StallID = BitConverter.ToUInt32(Data, 8);

                                            if (Game.World.H_PShops.ContainsKey(StallID))
                                            {
                                                Features.PersonalShops.Shop S = (Features.PersonalShops.Shop)Game.World.H_PShops[StallID];
                                                if (Game.World.H_Chars.ContainsKey(S.Owner.EntityID))
                                                    S.Buy(ItemUID, GC.MyChar);
                                                else
                                                    S.Close();
                                            }
                                            break;
                                        }
                                    case 27:
                                        {
                                            GC.AddSend(Data);
                                            int p = (DateTime.Now - GC.ReceivePing).Seconds;
                                            if (p <= 5)
                                            {
                                                GC.ReceiveTest++;
                                                GC.LastSuspect = DateTime.Now;
                                            }
                                            GC.ReceivePing = DateTime.Now;
                                            if (GC.ReceiveTest >= 5)
                                            {
                                                //GC.Disconnect();
                                                GC.ReceiveTest = 0;//resets so the msg shows up once.
                                                AntiCheatPacket.Report(GC.MyChar.Name, "Time-packet speedhack", GC.MyChar.EntityID);
                                            }
                                            break;
                                        }
                                    case 28:
                                        PacketHandling.Enchant.Handle(GC, Data);
                                        break;
                                    case 36:
                                        {
                                            PacketHandling.SocketTalisman.HandleCPS(GC, Data);
                                            break;
                                        }
                                    case 35:
                                        {
                                            PacketHandling.SocketTalisman.HandleItems(GC, Data);
                                            break;
                                        }
                                    default:
                                        {
                                            GC.AddSend(Packets.ChatMessage(0, "SYSTEM", GC.MyChar.Name, "Unknow 1009 subtype: " + Data[12], 2001, 0));
                                            break;
                                        }
                                }
                                break;
                            }
                        case 1102:
                            {
                                PacketHandling.Warehouse.Handle(GC, Data);
                                break;
                            }
                        case 1001:
                            {
                                PacketHandling.CharacterMaking.Handle(GC, Data);
                                break;
                            }
                        case 10005:
                            {
                                PacketHandling.WalkRun.Handle(GC, Data);
                                break;
                            }
                        case 1056:
                            {
                                PacketHandling.Trade.Handle(GC, Data);
                                break;
                            }
                        case 1019:
                            {
                                PacketHandling.Friends.Handle(GC, Data);
                                break;
                            }
                        case 2066:
                            {
                                break;
                            }
                        case 2067:
                            {
                                break;
                            }
                        case 1028:
                            {
                                PacketHandling.Craft.Handle(GC, Data);
                                break;
                            }
                        case 10010:
                            {
                                switch (Data[16])
                                {
                                    case 120:
                                        {
                                            Buff B = GC.MyChar.BuffOf(SkillsClass.ExtraEffect.Fly);
                                            if (B.Eff == SkillsClass.ExtraEffect.Fly)
                                                GC.MyChar.BDelete.TryAdd(B, B.Lasts);
                                            break;
                                        }
                                    case 99:
                                        {
                                            GC.MyChar.Mining = true;
                                            break;
                                        }
                                    #region 95 Delete Character
                                    case 95: //Delete Character
                                        {
                                            string Password = BitConverter.ToUInt32(Data, 8).ToString();
                                            if (Password == GC.MyChar.WHPassword)
                                            {
                                                if ((DateTime.Now.Hour != 11 || DateTime.Now.Minute != 0) && !Program.Reseting)
                                                {
                                                    /*  try
                                                      {
                                                          foreach (Game.Enemy F in GC.MyChar.Enemies.Values)
                                                          {
                                                              if (F.Online)
                                                              {
                                                                  Game.Character C = F.Info;
                                                                  if (C.Enemies.Contains(GC.MyChar.EntityID))
                                                                  {
                                                                      C.Enemies.Remove(GC.MyChar.EntityID);
                                                                      C.MyClient.AddSend(Packets.FriendEnemyPacket(GC.MyChar.EntityID, "", 14, 0));
                                                                  }
                                                              }
                                                              else
                                                              {
                                                                  string Acc = "";
                                                                  Game.Character C = Database.LoadCharacter(F.Name, ref Acc);
                                                                  if (C != null)
                                                                      if (C.Enemies.Contains(GC.MyChar.EntityID))
                                                                          C.Enemies.Remove(GC.MyChar.EntityID);
                                                                  Database.SaveCharacter(C, Acc);
                                                              }
                                                          }
                                                          foreach (Game.Friend F in GC.MyChar.Friends.Values)
                                                          {
                                                              if (F.Online)
                                                              {
                                                                  Game.Character C = F.Info;
                                                                  if (C.Friends.Contains(GC.MyChar.EntityID))
                                                                  {
                                                                      C.Friends.Remove(GC.MyChar.EntityID);
                                                                      C.MyClient.AddSend(Packets.FriendEnemyPacket(GC.MyChar.EntityID, "", 14, 0));
                                                                  }
                                                              }
                                                              else
                                                              {
                                                                  string Acc = "";
                                                                  Game.Character C = Database.LoadCharacter(F.Name, ref Acc);
                                                                  if (C != null)
                                                                      if (C.Friends.Contains(GC.MyChar.EntityID))
                                                                          C.Friends.Remove(GC.MyChar.EntityID);
                                                                  Database.SaveCharacter(C, Acc);
                                                              }
                                                          }

                                                      }
                                                      catch (Exception Exc)
                                                      { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }*/
                                                    if (GC.MyChar.MyGuild != null)
                                                    {
                                                        if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                                                            GC.MyChar.MyGuild.Disband();
                                                        else
                                                            GC.MyChar.MyGuild.MemberLeaves(GC.MyChar.EntityID, false);
                                                    }
                                                    if (DMaps.MapOwner.ContainsKey(GC.MyChar.EntityID))
                                                        DMaps.DeleteDynamicMap(GC.MyChar.EntityID, false);
                                                    if (Game.World.EIDS.Contains(GC.MyChar.EntityID))
                                                        Game.World.EIDS.Remove(GC.MyChar.EntityID);
                                                    for (int i = 0; i < Game.World.KOBoard.Length; i++)
                                                        if (Game.World.KOBoard[i].Name == GC.MyChar.Name)
                                                        {
                                                            for (int j = i; j < 498; j++)
                                                            {
                                                                Game.World.KOBoard[j] = Game.World.KOBoard[j + 1];
                                                            }
                                                            break;
                                                        }
                                                    for (int i = 0; i < Game.World.EmpireBoard.Length; i++)
                                                        if (Game.World.EmpireBoard[i].ID == GC.MyChar.EntityID)
                                                        {
                                                            for (int j = i; j < 48; j++)
                                                            {
                                                                Game.World.EmpireBoard[j] = Game.World.EmpireBoard[j + 1];
                                                            }
                                                        }
                                                    Game.World.InfoAdd += GC.MyChar.Name + " got deleted!!! \r\n";
                                                    GC.Soc.Disconnect(false);


                                                    Database.DeleteCharacter(GC.MyChar.Name, GC.AuthInfo.Account, GC.AuthInfo.UID);
                                                }
                                                else
                                                {
                                                    GC.LocalMessage(2011, "Please wait 1 minute before deleting your character!");
                                                }
                                            }
                                            else
                                            {
                                                GC.LocalMessage(2011, "The WH password is incorrect!");
                                            }
                                            break;
                                        }
                                    #endregion
                                    case 151:
                                        {
                                            if (GC.MyChar.Silvers >= 500)
                                            {
                                                GC.MyChar.Silvers -= 500;
                                                GC.MyChar.Avatar = BitConverter.ToUInt16(Data, 8);
                                                Game.World.Spawn(GC.MyChar, false);
                                            }
                                            break;
                                        }
                                    case 148:
                                        {
                                            uint UID = BitConverter.ToUInt32(Data, 8);
                                            if (GC.MyChar.Friends.ContainsKey(UID))
                                            {
                                                Game.Friend F = (Game.Friend)GC.MyChar.Friends[UID];
                                                if (F.Online)
                                                {
                                                    if (F.Info.MyGuild != null)
                                                        GC.AddSend(Packets.StringPacket(F.Info.MyGuild.GuildID, Game.StringType.GuildName, F.Info.MyGuild.GuildName));
                                                    GC.AddSend(Packets.FriendEnemyInfo(F.Info, 0));
                                                }
                                            }
                                            break;
                                        }
                                    case 118:
                                        {
                                            Buff B = GC.MyChar.BuffOf(SkillsClass.ExtraEffect.Transform);
                                            GC.MyChar.BDelete.TryAdd(B, B.Lasts);

                                            break;
                                        }
                                    case 123:
                                        {
                                            uint UID = BitConverter.ToUInt32(Data, 8);

                                            if (GC.MyChar.Enemies.ContainsKey(UID))
                                            {
                                                Game.Enemy E = (Game.Enemy)GC.MyChar.Enemies[UID];
                                                if (E.Online)
                                                    GC.AddSend(Packets.FriendEnemyInfo(E.Info, 1));
                                            }
                                            break;
                                        }
                                    case 117:
                                        {
                                            uint key = BitConverter.ToUInt32(Data, 8);
                                            Game.Character C = null;
                                            if (Game.World.H_Chars.ContainsKey(key))
                                                C = Game.World.H_Chars[key];
                                            if (C != null)
                                            {
                                                C.Equips.SendView(C.EntityID, GC);
                                                GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.ViewEquipSpouse, C.Spouse));
                                                GC.AddSend(Packets.StringPacket(C.EntityID, Game.StringType.Effect, C.Spouse).Get);
                                                //GC.AddSend(Packets.ViewEquip(C));
                                            }
                                            break;
                                        }
                                    #region 106 Team Member Location
                                    case 106://See team member's location
                                        {
                                            uint key = BitConverter.ToUInt32(Data, 8);
                                            Game.Character C = null;
                                            if (Game.World.H_Chars.ContainsKey(key))
                                                C = Game.World.H_Chars[key];
                                            if (C != null && C.Loc.Map == GC.MyChar.Loc.Map)
                                            {
                                                Data[20] = (byte)C.Loc.X;
                                                Data[21] = (byte)(C.Loc.X >> 8);
                                                Data[22] = (byte)C.Loc.Y;
                                                Data[23] = (byte)(C.Loc.Y >> 8);
                                                //Buffer.BlockCopy(Data, 0, nData, 0, Data.Length);
                                                //Buffer.BlockCopy(Data, )
                                                //fixed (byte* p = Data)
                                                //{
                                                //    *((ushort*)(p + 20)) = C.Loc.X;
                                                //    *((ushort*)(p + 22)) = C.Loc.Y;
                                                //}
                                                GC.AddSend(Data);
                                            }
                                            break;
                                        }
                                    #endregion
                                    case 74:
                                        {
                                            PacketHandling.Teleport.Handle(GC, Data);
                                            // GC.MyChar.LoggedOn = DateTime.Now;

                                            if (GC.MyChar.DoubleExp && GC.MyChar.DoubleExpLeft > 0)
                                            {
                                                GC.MyChar.ExpPotionUsed = DateTime.Now;
                                                GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.DoubleExpTime, (ulong)GC.MyChar.DoubleExpLeft));
                                            }
                                            else
                                            {
                                                GC.MyChar.DoubleExp = false;
                                                GC.MyChar.DoubleExpLeft = 0;
                                            }
                                            if (!System.IO.File.Exists(Game.World.GlobalCharactersPath + GC.MyChar.Spouse + ".chr"))
                                                GC.MyChar.Spouse = "None";
                                            else
                                            {
                                                string Acc = "";
                                                var Spouse = Database.LoadCharacter(GC.MyChar.Spouse, ref Acc);
                                                if (Spouse == null)
                                                    GC.MyChar.Spouse = "None";
                                            }
                                            break;
                                        }
                                    case 75:
                                        {
                                            GC.AddSend(Packets.Packet2048(GC.MyChar.EntityID));
                                            GC.AddSend(Packets.Packet1032(GC.MyChar.EntityID));
                                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 75));
                                            break;
                                        }
                                    case 76:
                                        {
                                            if (!GC.LoginDataSent)
                                            {
                                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has just joined the server!", 2005, 0);
                                                GC.LoginDataSent = true;
                                                foreach (Game.Friend F in GC.MyChar.Friends.Values)
                                                {
                                                    GC.AddSend(Packets.FriendEnemyPacket(F.UID, F.Name, 15, Convert.ToByte(F.Online)));
                                                    if (F.Online)
                                                    {
                                                        F.Info.MyClient.AddSend(Packets.FriendEnemyPacket(GC.MyChar.EntityID, GC.MyChar.Name, 14, 1));
                                                        F.Info.MyClient.AddSend(Packets.FriendEnemyPacket(GC.MyChar.EntityID, GC.MyChar.Name, 15, 1));
                                                        F.Info.MyClient.LocalMessage(2005, "Your friend " + GC.MyChar.Name + " has logged on.");
                                                    }
                                                }
                                                foreach (Game.Enemy E in GC.MyChar.Enemies.Values)
                                                    GC.AddSend(Packets.FriendEnemyPacket(E.UID, E.Name, 19, Convert.ToByte(E.Online)));

                                                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 76));

                                                if (GC.MyChar.Top != 0)
                                                {
                                                    if (GC.MyChar.Top == 1 || GC.MyChar.Top == 2)
                                                        if (GC.MyChar.MyGuild != null)
                                                        {
                                                            if (Features.GuildWars.War || Features.GuildWars.LastWinner.GuildName != GC.MyChar.MyGuild.GuildName)
                                                                GC.MyChar.Top = 0;
                                                        }
                                                        else GC.MyChar.Top = 0;
                                                }
                                                if (GC.MyChar.TopFB == 1)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopFBSS);
                                                else if (GC.MyChar.TopFB == 2)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.Top3FBSS);

                                                if (GC.MyChar.Top == 1)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopGuildLeader);
                                                else if (GC.MyChar.Top == 2)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopDeputyLeader);
                                                else if (GC.MyChar.Top == 3)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopTrojan);
                                                else if (GC.MyChar.Top == 4)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopArcher);
                                                else if (GC.MyChar.Top == 5)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopWarrior);
                                                else if (GC.MyChar.Top == 6)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopFireTaoist);
                                                else if (GC.MyChar.Top == 7)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.TopWaterTaoist);
                                                else if (GC.MyChar.Top == 8)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.MonthlyPKChampion);
                                                else if (GC.MyChar.Top == 9)
                                                    GC.MyChar.StatEff.Add(Game.StatusEffectEn.WeeklyPKChampion);


                                                GC.MyChar.CancelProtectTime = false;
                                                if (GC.MyChar.Loc.Map != 1038)
                                                    GC.MyChar.ProtectTime = DateTime.Now.AddSeconds(0);
                                                else GC.MyChar.ProtectTime = DateTime.Now.AddSeconds(0);

                                            }
                                            break;
                                        }
                                    case 77:
                                        {
                                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 77));
                                            break;
                                        }
                                    case 78:
                                        {
                                            GC.AddSend(Packets.Packet1025());
                                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 78));
                                            break;
                                        }
                                    case 96:
                                        {
                                            if (!GC.DoneLoading)
                                            {

                                                // GC.MyChar.ScreenItems = new ConcurrentDictionary<uint, Game.DroppedItem>(300);
                                                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, (byte)GC.MyChar.PKMode, 0, 0, 96));
                                                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 77));
                                                GC.DoneLoading = true;

                                                if (!Game.World.H_Chars.ContainsKey(GC.MyChar.EntityID))
                                                {
                                                    if (GC.AuthInfo.Status != "[PH]" && GC.AuthInfo.Status != "[GM]" && GC.AuthInfo.Status != "[PM]" && GC.AuthInfo.Status.Length > 0)
                                                        GC.AuthInfo.Status = "";

                                                    Game.World.H_Chars.TryAdd(GC.MyChar.EntityID, GC.MyChar);
                                                    //for (int i = 0; i < 7; i++)
                                                    // Program.ThreadInfo.Modified = true;
                                                    Game.World.Spawns(GC.MyChar, false);

                                                }
                                                // ConcurrentDictionary<uint, Game.Character> Map = (ConcurrentDictionary<uint, Game.Character>)Game.World.PlayersInMap[GC.MyChar.Loc.Map];
                                                if (Game.World.PlayersInMap.ContainsKey(GC.MyChar.Loc.Map))
                                                {
                                                    if (!Game.World.PlayersInMap[GC.MyChar.Loc.Map].ContainsKey(GC.MyChar.EntityID))
                                                        Game.World.PlayersInMap[GC.MyChar.Loc.Map].TryAdd(GC.MyChar.EntityID, GC.MyChar);
                                                }
                                            }
                                            else
                                            {
                                                GC.MyChar.PKMode = (Game.PKMode)Data[8];
                                                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, Data[8], 0, 0, 96));

                                            }
                                            break;
                                        }
                                    case 97:
                                        {
                                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 97));
                                            break;
                                        }
                                    case 137:
                                        {
                                            PacketHandling.Jump.Handle(GC, Data);
                                            break;
                                        }
                                    case 146:
                                        {
                                            if (GC.MyChar.Loc.Map == 1036)
                                                GC.MyChar.Teleport(GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y);
                                            if (GC.MyChar.Loc.Map == 1616)
                                                GC.MyChar.Teleport(GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y);
                                            if (GC.MyChar.Loc.Map == 2068)
                                                GC.MyChar.Teleport(GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y);
                                            break;
                                        }
                                    case 85:
                                        {
                                            PacketHandling.Portal.Handle(GC, Data);
                                            break;
                                        }
                                    case 79:
                                        {
                                            GC.MyChar.Direction = Data[22];
                                            Game.World.Action(GC.MyChar, Data);
                                            break;
                                        }
                                    case 81:
                                        {
                                            GC.MyChar.Action = Data[8];
                                            Game.World.Action(GC.MyChar, Data);
                                            GC.MyChar.AtkMem.Attacking = false;
                                            GC.MyChar.AtkMem.Target = 0;
                                            break;
                                        }
                                    case 94:
                                        {
                                            try { PacketHandling.Revive.Handle(GC); }
                                            catch (Exception e) { Game.World.GMChatAdd += e; }

                                            break;
                                        }
                                    case 111:
                                        {
                                            Game.Location CarpetLoc = new Game.Location();
                                            CarpetLoc.X = BitConverter.ToUInt16(Data, 8);
                                            CarpetLoc.Y = BitConverter.ToUInt16(Data, 10);
                                            CarpetLoc.Map = GC.MyChar.Loc.Map;
                                            Game.Location NPCLoc = GC.MyChar.Loc;
                                            NPCLoc.X -= 2;

                                            Game.NPC N = Game.World.NPCFromLoc(NPCLoc);
                                            if (N != null)
                                            {
                                                bool Taken = false;
                                                foreach (Features.PersonalShops.Shop S in Game.World.H_PShops.Values)
                                                    if (S.NPCInfo.Loc.X == CarpetLoc.X && S.NPCInfo.Loc.Y == CarpetLoc.Y)
                                                    {
                                                        Taken = true;
                                                        break;
                                                    }
                                                if (!Taken)
                                                {
                                                    if (!GC.GM || GC.PM)
                                                    {
                                                        GC.MyChar.MyShop = new Ultimate.Features.PersonalShops.Shop(GC.MyChar, BitConverter.ToUInt32(Data, 4));
                                                        GC.MyChar.Direction = 4;
                                                    }
                                                }
                                                else
                                                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 111));
                                            }
                                            break;
                                        }
                                    case 114:
                                        {
                                            if (GC.MyChar.MyShop != null)
                                                GC.MyChar.MyShop.Close();
                                            break;
                                        }
                                    case 132:
                                        {
                                            if (GC.MyChar.MyGuild != null)
                                            {
                                                foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                                {
                                                    if (GC.MyChar.MyGuild.Allies.ContainsKey(G.GuildID))
                                                    {
                                                        GC.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildAllies, G.GuildName));
                                                    }
                                                    if (GC.MyChar.MyGuild.Enemies.ContainsKey(G.GuildID))
                                                    {
                                                        GC.AddSend(Packets.StringPacket(G.GuildID, Game.StringType.GuildEnemies, G.GuildName));
                                                    }
                                                }
                                            }
                                            GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Merchant, 255));
                                            // GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 132, 0));
                                            break;
                                        }
                                    case 54:
                                        {
                                            Game.Character C = null;
                                            uint key = BitConverter.ToUInt32(Data, 8);
                                            if (Game.World.H_Chars.ContainsKey(key))
                                                C = Game.World.H_Chars[key];
                                            if (C != null)
                                            {
                                                GC.AddSend(Packets.SpawnViewed(C, 1));
                                            }
                                            break;
                                        }
                                    case 145:
                                        {
                                            /* uint UID = BitConverter.ToUInt32(Data, 8);

                                             if (Game.World.H_Chars.ContainsKey(UID))
                                             {
                                                 Game.Character C = Game.World.H_Chars[UID];
                                                 if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28) && C.Loc.Map == GC.MyChar.Loc.Map)
                                                 {
                                                     GC.AddSend(Packets.SpawnEntity(C));
                                                     if (C.MyGuild != null)
                                                         GC.AddSend(Packets.StringPacket(C.MyGuild.GuildID, Game.StringType.GuildName, C.MyGuild.GuildName));
                                                     if (!GC.MyChar.ScreenChars.ContainsKey(UID))
                                                         GC.MyChar.ScreenChars.Add(UID, C);

                                                 }
                                                 Game.World.PacketAdd += "Unknown 10010 subtype: " + 145 + " map: " + GC.MyChar.Loc.Map + " map to see: " + C.Loc.Map + " Screen Count: " + GC.MyChar.ScreenChars.Count + " UID to see: " + UID + " at: " + DateTime.Now + "\r\n";
                                             }*/
                                            break;
                                        }
                                    case 102:
                                        {
                                            uint UID = BitConverter.ToUInt32(Data, 8);

                                            if (Game.World.H_Chars.ContainsKey(UID))
                                            {
                                                Game.Character C = Game.World.H_Chars[UID];
                                                if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28) && C.Loc.Map == GC.MyChar.Loc.Map)
                                                //  if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 18 && C.Loc.Map == GC.MyChar.Loc.Map)
                                                {
                                                    if (!C.Invisible)
                                                    {
                                                        if (C.MyGuild != null)
                                                            GC.AddSend(Packets.StringPacket(C.MyGuild.GuildID, Game.StringType.GuildName, C.MyGuild.GuildName));
                                                        GC.AddSend(Packets.SpawnEntity(C));
                                                    }
                                                    if (!GC.MyChar.Invisible)
                                                    {
                                                        if (GC.MyChar.MyGuild != null) // last edit
                                                            C.MyClient.AddSend(Packets.StringPacket(GC.MyChar.MyGuild.GuildID, Game.StringType.GuildName, GC.MyChar.MyGuild.GuildName)); // last edit
                                                        C.MyClient.AddSend(Packets.SpawnEntity(GC.MyChar));// last edit
                                                    }
                                                    if (!GC.MyChar.ScreenChars.ContainsKey(UID))
                                                        GC.MyChar.ScreenChars.TryAdd(UID, C);
                                                    if (!C.ScreenChars.ContainsKey(GC.MyChar.EntityID))// last edit
                                                        C.ScreenChars.TryAdd(GC.MyChar.EntityID, GC.MyChar);// last edit
                                                    //a  Game.World.PacketAdd += "Unknown 10010 subtype: " + 102 + " map: " + GC.MyChar.Loc.Map + " map to see: " + C.Loc.Map + " Screen Count: " + GC.MyChar.ScreenChars.Count + " UID to see: " + UID + " at: " + DateTime.Now + "\r\n";
                                                }
                                                // Game.World.PacketAdd += "Unknown 10010 subtype: " + 102 + " map: " + GC.MyChar.Loc.Map + " map to see: " + C.Loc.Map + " Screen Count: " + GC.MyChar.ScreenChars.Count + " UID to see: " + UID + " at: " + DateTime.Now + "\r\n";
                                            }
                                            else if (Game.World.H_Companions.ContainsKey(UID))
                                            {
                                                Game.Companion C = Game.World.H_Companions[UID];
                                                //  if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 18 && C.Loc.Map == GC.MyChar.Loc.Map)
                                                if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, GC.MyChar.Range()) && C.Loc.Map == GC.MyChar.Loc.Map)
                                                {
                                                    GC.AddSend(Packets.SpawnEntity(C));
                                                    // Console.WriteLine("Spawned Companion");
                                                }
                                            }
                                            else if (Game.World.H_Mobs.ContainsKey(GC.MyChar.Loc.Map))
                                            {
                                                if ((Game.World.H_Mobs[GC.MyChar.Loc.Map]).ContainsKey(UID))
                                                {
                                                    Game.Mob C = (Game.Mob)(Game.World.H_Mobs[GC.MyChar.Loc.Map])[UID];
                                                    // if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 18)
                                                    if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, GC.MyChar.Range()))
                                                    {
                                                        GC.AddSend(Packets.SpawnEntity(C));
                                                        // Console.WriteLine("Spawned Mob");
                                                    }
                                                }
                                            }
                                            /* else if (Game.World.H_Items.ContainsKey(GC.MyChar.Loc.Map))
                                             {
                                                 if (((ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[GC.MyChar.Loc.Map]).ContainsKey(UID))
                                                 {
                                                     Game.DroppedItem C = ((ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[GC.MyChar.Loc.Map])[UID];
                                                     if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 18)
                                                     {
                                                         GC.AddSend(Packets.ItemDrop(C));
                                                         Console.WriteLine("Spawned Item");
                                                     }
                                                 }
                                             }*/
                                            break;
                                        }
                                    case 93:
                                        {
                                            // GC.MyChar.XPKO = 0;
                                            break;
                                        }
                                    default:
                                        {
                                            if (GC.GM)
                                                GC.AddSend(Packets.ChatMessage(0, "SYSTEM", GC.MyChar.Name, "Unknown 10010 subtype: " + Data[16], 2001, 0));
                                            Game.World.PacketAdd += "Unknown 10010 subtype: " + Data[16] + " map: " + GC.MyChar.Loc.Map + " at: " + DateTime.Now + "\r\n";
                                            break;
                                        }
                                }
                                break;
                            }
                        case 1012:
                            {
                                break;
                            }
                        case 1004:
                            {
                                PacketHandling.Chat.Handle(GC, Data);
                                break;
                            }
                        case 1107:
                            {
                                PacketHandling.Guild.Handle(GC, Data);
                                break;
                            }
                        case 1111://Message Board
                            {
                                PacketHandling.MessageBoard.Handle(GC, Data);
                                break;
                            }
                        case 1112:
                            {
                                PacketHandling.GuildMembInfo.Handle(GC, Data);
                                break;
                            }
                        case 1015:
                            {
                                if (Data.Count() >= 8)
                                {
                                    if (Data[8] == 11)
                                        PacketHandling.MemberList.Handle(GC);
                                    else if (Data[8] == 17)
                                        DiceKing.AddPlayer(GC, Data);
                                    else if (Data[8] == 18)
                                        DiceKing.RemovePlayer(GC, Data);
                                    else if (GC.AuthInfo.Status == "[PM]")
                                        GC.AddSend(Packets.ChatMessage(0, "SYSTEM", GC.MyChar.Name, "Unknown Main ID: " + PacketID + " - Type: " + Data[8].ToString(), 2001, 0));
                                    //else if (Data[8] == 26)
                                    //    GC.AddSend(Data);

                                    //string Name = "";
                                    //for (byte i = 0; i < Data[10]; i++)
                                    //    Name += Convert.ToChar(Data[11 + i]);
                                    //GC.LocalMessage(2000, Name);
                                }

                                break;
                            }
                        case 1150:
                            {
                                //tulip roses and other handling
                                // PacketHandling.TulipRoses.Handle(GC, Data);
                                break;
                            }
                        case 1801:
                            break;//  brb

                        case 1855:
                            AntiCheatPacket.CheatPacketHandler(Data, GC);
                            break;
                        case 1810:
                            AntiCheatPacket.CheatPacketHandler2(Data, GC);
                            break;

                        case 2030:
                            {

                                if (Data[16] != 0)
                                    Game.SOB.GuildStatue.AddStatue(GC.MyChar, Data);
                                else
                                    Features.HouseTable.AddFurniture(GC.MyChar, Data);
                                break;
                            }
                        case 2102:
                            break;
                        case 1113:
                            MsgDice.Process(Data, GC);
                            //GC.AddSend(Data);
                            break;
                        default:
                            {
                                if (GC != null)
                                    if (GC.MyChar != null)
                                        if (GC.GM)
                                            GC.AddSend(Packets.ChatMessage(0, "SYSTEM", GC.MyChar.Name, "Unknown Main ID: " + PacketID, 2001, 0));
                                break;
                            }
                    }
                }
                catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                Position += PacketLength + 8;
                a++;
                if (a > 100000)
                {
                    Console.WriteLine("Something went wrong in the PacketHandler");
                    Game.World.ExcAdd += "Something went wrong in the PacketHandler\r\n";
                }
            }
            // GC.EndSend();
        }
    }
}
