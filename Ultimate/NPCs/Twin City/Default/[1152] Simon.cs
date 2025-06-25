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
    public class NPC_1152 : NPCBase
    {
        public NPC_1152(Main.GameClient _client)
            : base(_client)
        {
            ID = 1152;
            Face = 63;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Great rewards will attract many brave poeple. I am looking for brave poeple to help me take my patrimony back. Can you help me? The rewards are handsome.");
                        AddOption("Please tell me more.", 2);
                        AddOption("I have some diamonds", 1);
                        break;
                    }
                case 1:
                    {
                        AddText("What diamonds do you wish to exchange?");
                        AddOption("15 SunDiamonds", 3);
                        AddOption("13 MoonDiamonds", 4);
                        AddOption("12 StarDiamonds", 5);
                        AddOption("10 CloudDiamonds", 6);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(721533, 15))
                        {
                            for (int i = 0; i < 15; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721533));
                            GC.MyChar.AddItem(721541);
                            AddText("Congratulations! You have received a SunBox in exchange for your 15 SunDiamonds!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems you don't have 15 SunDiamonds!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.InventoryContains(721534, 13))
                        {
                            for (int i = 0; i < 13; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721534));
                            GC.MyChar.AddItem(721542);
                            AddText("Congratulations! You have received a WaningMoonBox in exchange for your 13 MoonDiamonds!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems you don't have 13 MoonDiamonds!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.InventoryContains(721535, 12))
                        {
                            for (int i = 0; i < 12; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721535));
                            GC.MyChar.AddItem(721543);
                            AddText("Congratulations! You have received a StarBox in exchange for your 12 StarDiamonds!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems you don't have 12 StarDiamonds!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 6:
                    {
                        if (GC.MyChar.InventoryContains(721536, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721536));
                            GC.MyChar.AddItem(721544);
                            AddText("Congratulations! You have received a CloudBox in exchange for your 10 CloudDiamonds!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems you don't have 10 CloudDiamonds!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        AddText("My ancestors built a Labyrinth long before. But it was occupied by fiece monsters soon. They expelled our clansmen and kept the treasure. Now, I'm looking for brave players who are willing to fight them for rewards!");
                        AddOption("What rewards?", 7);
                        AddOption("I have no interest.", 255);
                        break;
                    }
                case 7:
                    {
                        AddText("I'll be giving 1 SunBox for 15 SunDiamonds, 1 WaningMoonBox for 13 MoonDiamonds, 1 StarBox for 12 StarDiamonds and 1 CloudBox for 10 CloudDiamonds!");
                        AddText("These boxes contain amazing treasures such as Meteors, Dragonballs or even socket gears!");
                        AddOption("Send me in", 8);
                        AddOption("I shall stay", 255);
                        break;
                    }
                case 8:
                    {
                        AddText("In order to take you inside, you'll have to give me 2,000 virtue points. You can also give me 5 Online Points if you are active enough!");
                        AddOption("Take my virtue points", 9);
                        AddOption("I have 5 Online Points", 10);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 9:
                    {
                        if (GC.MyChar.Level >= 70)
                        {
                            if (GC.MyChar.VP >= 2000)
                            {
                                GC.MyChar.Teleport(1351, 20, 130);
                                GC.MyChar.VP -= 2000;
                            }
                            else
                            {
                                AddText("I'm sorry but it seems you don't have 2,000 VPs!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you have to be level 70 to enter the Labyrinth!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        if (GC.MyChar.Level >= 70)
                        {
                            if (GC.MyChar.ClassicPoints >= 5)
                            {
                                GC.MyChar.Teleport(1351, 20, 130);
                                GC.MyChar.ClassicPoints -= 5;
                            }
                            else
                            {
                                AddText("I'm sorry but it seems you don't have 5 Online Points!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you have to be level 70 to enter the Labyrinth!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_1153 : NPCBase
    {
        public NPC_1153(Main.GameClient _client)
            : base(_client)
        {
            ID = 1153;
            Face = 63;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello, I see that you are a great warrior! If you bring me a SkyToken, I'll show you the way to the next floor. Otherwise, I can take you back to TwinCity!");
                        AddOption("Take me to the next stage", 1);
                        AddOption("Take me to TwinCity", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721537, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721537));
                            GC.MyChar.Teleport(1352, 029, 230);
                        }
                        else
                        {
                            AddText("It seems you don't have a SkyToken...");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.Teleport(1002, 431, 379);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_1154 : NPCBase
    {
        public NPC_1154(Main.GameClient _client)
            : base(_client)
        {
            ID = 1154;
            Face = 63;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello, I see that you are a great warrior! If you bring me an EarthToken, I'll show you the way to the next floor. Otherwise, I can take you back to TwinCity!");
                        AddOption("Take me to the next stage", 1);
                        AddOption("Take me to TwinCity", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721538, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721538));
                            GC.MyChar.Teleport(1353, 028, 270);
                        }
                        else
                        {
                            AddText("It seems you don't have an EarthToken...");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.Teleport(1002, 431, 379);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_1155 : NPCBase
    {
        public NPC_1155(Main.GameClient _client)
            : base(_client)
        {
            ID = 1155;
            Face = 63;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello, I see that you are a great warrior! If you bring me a SoulToken, I'll show you the way to the next floor. Otherwise, I can take you back to TwinCity!");
                        AddOption("Take me to the next stage", 1);
                        AddOption("Take me to TwinCity", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721539, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721539));
                            GC.MyChar.Teleport(1354, 009, 290);
                        }
                        else
                        {
                            AddText("It seems you don't have a SoulToken...");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.Teleport(1002, 431, 379);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_1156 : NPCBase
    {
        public NPC_1156(Main.GameClient _client)
            : base(_client)
        {
            ID = 1156;
            Face = 63;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello, I see that you are a great warrior! Would you like me to take you to TwinCity?");
                        AddOption("Take me to TwinCity", 1);
                        AddOption("I'll stay here for a while", 255);
                        break;
                    }
                case 1:
                    {
                        GC.MyChar.Teleport(1002, 431, 379);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}