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
    public class NPC_30161 : NPCBase
    {
        public NPC_30161(Main.GameClient _client)
            : base(_client)
        {
            ID = 30161;
            Face = 188;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Welcome to Twin City Furniture Store. Do you wanna take a look at out store?");
                        AddOption("Sure", 1);
                        AddOption("I am not interested", 255);
                        break;
                    }
                case 1:
                    {
                        GC.MyChar.Teleport(1511, 51, 70);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_3113 : NPCBase
    {
        public NPC_3113(Main.GameClient _client)
            : base(_client)
        {
            ID = 3113;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            string FurnitureName = "";
            uint FurnitureCost = 0;
            uint FurnitureId = 0;
            switch (ID)
            {
                case 3113:
                    FurnitureName = "Lamp";
                    FurnitureId = 721177;
                    FurnitureCost = 100000;
                    break;
                case 3114:
                    FurnitureName = "LowShelf";
                    FurnitureId = 721178;
                    FurnitureCost = 100000;
                    break;
                case 3115:
                    FurnitureName = "Cabinet";
                    FurnitureId = 721179;
                    FurnitureCost = 100000;
                    break;
                case 3116:
                    FurnitureName = "BombeChest";
                    FurnitureId = 721181;
                    FurnitureCost = 100000;
                    break;
                case 3117:
                    FurnitureName = "RosewoodCabinet";
                    FurnitureId = 721182;
                    FurnitureCost = 100000;
                    break;
                case 3118:
                    FurnitureName = "HighCabinet";
                    FurnitureId = 721183;
                    FurnitureCost = 8000;
                    break;
                case 3119:
                    FurnitureName = "FoldingScreen";
                    FurnitureId = 721184;
                    FurnitureCost = 150000;
                    break;
                case 3120:
                    FurnitureName = "Dresser";
                    FurnitureId = 721185;
                    FurnitureCost = 150000;
                    break;
                case 3121:
                    FurnitureName = "BasinRack";
                    FurnitureId = 721186;
                    FurnitureCost = 75000;
                    break;
                case 3122:
                    FurnitureName = "Chair";
                    FurnitureId = 721187;
                    FurnitureCost = 75000;
                    break;
                case 3123:
                    FurnitureName = "EndTable";
                    FurnitureId = 721188;
                    FurnitureCost = 75000;
                    break;
                case 3124:
                    FurnitureName = "ItemBox";
                    FurnitureId = 721189;
                    FurnitureCost = 100000;
                    break;
                case 3125:
                    FurnitureName = "HighShelf";
                    FurnitureId = 721180;
                    FurnitureCost = 50000;
                    break;
            }
            switch (_linkback)
            {
                case 0:
                    {
                        if (FurnitureCost < 100000)
                            AddText("Do you want this " + FurnitureName + "? It costs only " + FurnitureCost + " silvers.");
                        else
                            AddText("Do you want this " + FurnitureName + "? It is expensive, and costs " + FurnitureCost + " silvers.");
                        AddOption("Yes.", 1);
                        AddOption("No.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= FurnitureCost)
                        {
                            if (GC.MyChar.Inventory.Count < GC.MyChar.Inventory.Capacity)
                            {
                                GC.MyChar.Silvers -= FurnitureCost;
                                GC.MyChar.AddItem(FurnitureId);
                                GC.LocalMessage(2005, "You got a " + FurnitureName + ".");
                            }
                            else
                            {
                                AddText("Please prepare one slot in your inventory.");
                                AddOption("Alright.", 255);
                            }
                        }
                        else
                        {
                            AddText("You do not have " + FurnitureCost + " silvers with you.");
                            AddOption("I am sorry.", 255);
                        }
                        break;
                    }
            }
            AddFinish();
            Send();
        }
    }
    public class NPC_3114 : NPC_3113
    {
        public NPC_3114(Main.GameClient _client)
            : base(_client)
        {
            ID = 3114;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3115 : NPC_3113
    {
        public NPC_3115(Main.GameClient _client)
            : base(_client)
        {
            ID = 3115;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3116 : NPC_3113
    {
        public NPC_3116(Main.GameClient _client)
            : base(_client)
        {
            ID = 3116;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3117 : NPC_3113
    {
        public NPC_3117(Main.GameClient _client)
            : base(_client)
        {
            ID = 3117;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3118 : NPC_3113
    {
        public NPC_3118(Main.GameClient _client)
            : base(_client)
        {
            ID = 3118;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3119 : NPC_3113
    {
        public NPC_3119(Main.GameClient _client)
            : base(_client)
        {
            ID = 3119;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3120 : NPC_3113
    {
        public NPC_3120(Main.GameClient _client)
            : base(_client)
        {
            ID = 3120;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3121 : NPC_3113
    {
        public NPC_3121(Main.GameClient _client)
            : base(_client)
        {
            ID = 3121;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3122 : NPC_3113
    {
        public NPC_3122(Main.GameClient _client)
            : base(_client)
        {
            ID = 3122;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3123 : NPC_3113
    {
        public NPC_3123(Main.GameClient _client)
            : base(_client)
        {
            ID = 3123;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3124 : NPC_3113
    {
        public NPC_3124(Main.GameClient _client)
            : base(_client)
        {
            ID = 3124;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
    public class NPC_3125 : NPC_3113
    {
        public NPC_3125(Main.GameClient _client)
            : base(_client)
        {
            ID = 3125;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            base.Run(GC, Data, _linkback);
        }
    }
}