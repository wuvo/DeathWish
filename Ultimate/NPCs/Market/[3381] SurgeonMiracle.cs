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
    public class NPC_3381 : NPCBase
    {
        public NPC_3381(Main.GameClient _client)
            : base(_client)
        {
            ID = 3381;
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
                        AddText("Hello, are you satisfield with your stature? I can change your body size for 1 DragonBall  and your gender for 1 DBScroll. What would you like to do?");
                        AddOption("Change my size", 1);
                        AddOption("Change my gender", 2);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(1088000, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                            if (GC.MyChar.Body == 1003)
                                GC.MyChar.Body++;
                            else if (GC.MyChar.Body == 1004)
                                GC.MyChar.Body--;
                            else if (GC.MyChar.Body == 2001)
                                GC.MyChar.Body++;
                            else
                                GC.MyChar.Body--;
                            AddText("It's done. Wonderful don't you think?");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have a DragonBall.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {

                        if (GC.MyChar.InventoryContains(720028, 1))
                        {
                            if (GC.MyChar.Equips.Garment.ID == 0)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720028));
                                if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
                                {
                                    GC.MyChar.Body += 998;
                                    GC.MyChar.Avatar = 201;
                                }
                                else if (GC.MyChar.Body == 2001 || GC.MyChar.Body == 2002) //1003 - small male, 1004 - big male, 2001 - small female, 2002 - big female
                                {
                                    GC.MyChar.Body -= 998;
                                    GC.MyChar.Avatar = 1;
                                }
                                World.Spawn(GC.MyChar, false);
                                AddText("It's done. Wonderful don't you think?");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("You have to remove your garment before you can change your gender!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have a DBScroll.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}