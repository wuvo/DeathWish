using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using System.Threading;
using Ultimate.NPCs;

namespace Ultimate.NPCs
{
    public class NPC_13671 : NPCBase
    {
        public NPC_13671(Main.GameClient _client)
            : base(_client)
        {
            ID = 13671;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.VipLevel >= 5 || GC.MyChar.InventoryContains(721774, 1))
                        {
                            AddText("Hi. You can try to open one or two socket in your gears by spamming MeteorScrolls. It may be your lucky day! More MeteorScrolls, better chance. Would you like to try?");
                            AddOption("1 MeteorScroll", 1);
                            AddOption("5 MeteorScrolls", 2);
                            AddOption("10 MeteorScrolls", 3);
                        }
                        else
                        {
                            AddText("You need to be VIP to use the SocketGod or you need to have QuestChanceA");
                            AddOption("Okay", 255);
                            GC.Agreed = false;
                        }
                        AddFinish();
                        Send();
                        break;
                    }
                case 1:
                case 2:
                case 3:
                    {
                        AddText("Choose the equipment you want to try to open socket into.");
                        AddOption("Headgear", (byte)(_linkback * 10 + 1));
                        AddOption("Necklace/Bag", (byte)(_linkback * 10 + 2));
                        AddOption("Armor", (byte)(_linkback * 10 + 3));
                        AddOption("Weapon", (byte)(_linkback * 10 + 4));
                        AddOption("Shield", (byte)(_linkback * 10 + 5));
                        AddOption("Ring", (byte)(_linkback * 10 + 6));
                        AddOption("Boots", (byte)(_linkback * 10 + 8));
                        AddFinish();
                        Send();
                        break;
                    }
                case 255:
                    {
                        // This case ensures that the dialog closes when "I see." is selected.
                        break;
                    }
                default:
                    {
                        HandleSocketSpamming(GC, _linkback);
                        break;
                    }
            }
        }

        private void HandleSocketSpamming(Main.GameClient GC, ushort _linkback)
        {
            int numScrolls;
            switch (_linkback / 10)
            {
                case 1:
                    numScrolls = 1;
                    break;
                case 2:
                    numScrolls = 5;
                    break;
                case 3:
                    numScrolls = 10;
                    break;
                default:
                    numScrolls = 0;
                    break;
            }
            int equipSlot = _linkback % 10;

            Game.Item I = GC.MyChar.Equips.Get((byte)equipSlot);
            if (I.ID == 0)
            {
                AddText("You don't have any equipment in that slot.");
                AddOption("I see.", 255); // Ensures dialog closes
                AddFinish();
                Send();
                return;
            }

            if (I.Soc1 != Game.Item.Gem.NoSocket && I.Soc2 != Game.Item.Gem.NoSocket)
            {
                AddText("This item is already two socket.");
                AddOption("I see.", 255); // Ensures dialog closes
                AddFinish();
                Send();
                return;
            }

            double sock1Chance, sock2Chance;
            switch (numScrolls)
            {
                case 1:
                    sock1Chance = DropRates.Meteor1SpamSock1;
                    sock2Chance = DropRates.Meteor1SpamSock2;
                    break;
                case 5:
                    sock1Chance = DropRates.Meteor5SpamSock1;
                    sock2Chance = DropRates.Meteor5SpamSock2;
                    break;
                case 10:
                    sock1Chance = DropRates.Meteor10SpamSock1;
                    sock2Chance = DropRates.Meteor10SpamSock2;
                    break;
                default:
                    sock1Chance = 0;
                    sock2Chance = 0;
                    break;
            }

            if (GC.MyChar.InventoryContains(720027, (byte)numScrolls))
            {
                for (byte i = 0; i < (byte)numScrolls; i++)
                {
                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720027));
                }

                GC.MyChar.EquipStats((byte)equipSlot, false, true);

                if (I.Soc1 == Game.Item.Gem.NoSocket)
                {
                    if (MyMath.ChanceSuccess(sock1Chance))
                    {
                        I.Soc1 = Game.Item.Gem.EmptySocket;
                        NotifySocketSuccess(GC, I, "first");
                    }
                    else
                    {
                        AddText("Better luck next time.");
                        AddOption("Thanks.", 255); // Ensures dialog closes
                        AddFinish();
                        Send();
                    }
                }
                else if (I.Soc2 == Game.Item.Gem.NoSocket)
                {
                    if (MyMath.ChanceSuccess(sock2Chance))
                    {
                        I.Soc2 = Game.Item.Gem.EmptySocket;
                        NotifySocketSuccess(GC, I, "second");
                    }
                    else
                    {
                        AddText("Better luck next time.");
                        AddOption("Thanks.", 255); // Ensures dialog closes
                        AddFinish();
                        Send();
                    }
                }

                GC.MyChar.Equips.Replace((byte)equipSlot, I, GC.MyChar);
                GC.MyChar.EquipStats((byte)equipSlot, true, false);
            }
            else
            {
                AddText($"You don't have enough MeteorScrolls. You need {numScrolls} MeteorScrolls.");
                AddOption("I see.", 255); // Ensures dialog closes
                AddFinish();
                Send();
            }
        }

        private void NotifySocketSuccess(Main.GameClient GC, Game.Item I, string socketNumber)
        {
            string message = $"Congratulations, You have got {socketNumber} socket into {I.DBInfo.Name}.";
            AddText(message);
            AddOption("Thank you.", 255); // Ensures dialog closes
            Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + $" has got {socketNumber} socket into his/her item", 2011, 0);
            Game.World.DebugAdd += GC.MyChar.Name + $" has got {socketNumber} socket from Meteor upp. on " + I.DBInfo.Name + $" ( {I.ID}~{I.Plus}~{I.Bless}~{I.Soc1}~{I.Soc2}~{I.Progress} ) \r\n";
            Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "congratulate")).Get);

            // Add Discord notification
            Discord DCord = new Discord();
            DCord.MesajVer3 = $"  __**{GC.MyChar.Name}**__ has got {socketNumber} socket into his/her __**{I.DBInfo.Name}**__ using the SocketMeDaddy NPC in TC!!  {DateTime.Now}";

            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721774));
            AddFinish();
            Send();
        }
    }
}
