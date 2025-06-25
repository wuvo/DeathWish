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
    public class NPC_800016 : NPCBase
    {
        public NPC_800016(Main.GameClient _client)
            : base(_client)
        {
            ID = 800016;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("We have been trapped here for many years. Few challengers have a chance to arrive here. You are a real conqueror.");
                    AddOption("Who are you?", 1);
                    break;
                case 1:
                    AddText("We were from an old Kingdom. Our rivals made the mystic tactics. We failed to break through them and died.");
                    AddOption("I see", 2);
                    break;
                case 2:
                    AddText("I'm trapped here and cannot get revived. Since you have passed the tactics, would you like to help me get reborn?");
                    AddOption("How can I help you?", 3);
                    break;
                case 3:
                    AddText("We cannot get reborn unless someone would like to take us out. One person can take only one ghost at a time. Will you help me?");
                    AddOption("No problem", 4);
                    AddOption("Sorry, I'm very busy", 255);
                    break;
                case 4:
                    AddText("You only need to spare a little time. I want to get reborn very much. Please kindly have mercy for me.");
                    AddOption("I shall help you", 5);
                    AddOption("I want to leave here", 10);
                    break;
                case 5:
                    AddText("I appreciate your help. Now I can get reborn when I teleport you out. I'll give you a MoonBox as a reward for your help. Use it to get my rare items.");
                    AddOption("Thank you, let me go now", 6);
                    AddOption("I shall leave later", 255);
                    break;
                case 6:
                    {
                        if (GC.MyChar.InventoryContains(721072, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721072));
                            GC.MyChar.AddItem(721080);
                            GC.MyChar.Teleport(1042, 26, 36);
                            AddText("Thank you once again! Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            GC.MyChar.Teleport(1042, 26, 36);
                            AddText("How did you get in here without a SoulJade?! You must bring me a SoulJade!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        AddText("Are you sure you want to leave without getting your reward?");
                        AddOption("I shall stay", 255);
                        AddOption("I am sure", 11);
                        break;
                    }
                case 11:
                    {
                        GC.MyChar.Teleport(1042, 26, 36);
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(721072));
                        AddText("You have been teleported back to Maggie!");
                        AddOption("Thank you", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_800017 : NPC_800016
    {
        public NPC_800017(Main.GameClient _client)
            : base(_client)
        {
            ID = 800017;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}