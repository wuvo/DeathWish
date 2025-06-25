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
    public class NPC_800001 : NPCBase
    {
        public NPC_800001(Main.GameClient _client)
            : base(_client)
        {
            ID = 800001;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do not go ahead, otherwise, you will enter a mysterious tactic. It's very dangerous. However, if you want I can teleport there. ");
                        AddText("You must pick up the CommandTokens dropped from the monsters in order to get out. If you don't find a token, you'll only be able to leave if you die.");
                        AddOption("Take me inside", 1);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    {
                        int x = Program.Rnd.Next(1, 7);
                        if (MyMath.ChanceSuccess(50))
                            x = Program.Rnd.Next(1, 8);

                        if (GC.MyChar.InventoryContains(721010, 1) && GC.MyChar.InventoryContains(721011, 1) && GC.MyChar.InventoryContains(721012, 1) && GC.MyChar.InventoryContains(721013, 1) && GC.MyChar.InventoryContains(721014, 1) && GC.MyChar.InventoryContains(721015, 1))
                            if (MyMath.ChanceSuccess(50))
                                x = 7;
                        switch (x)
                        {
                            case 1:
                                GC.MyChar.Teleport(1043, 207, 159);
                                break;
                            case 2:
                                GC.MyChar.Teleport(1044, 207, 159);
                                break;
                            case 3:
                                GC.MyChar.Teleport(1045, 207, 159);
                                break;
                            case 4:
                                GC.MyChar.Teleport(1046, 207, 159);
                                break;
                            case 5:
                                GC.MyChar.Teleport(1047, 207, 159);
                                break;
                            case 6:
                                GC.MyChar.Teleport(1048, 207, 159);
                                break;
                            case 7:
                                GC.MyChar.Teleport(1049, 207, 159);
                                break;
                        }

                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}