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
    public class NPC_2082 : NPCBase
    {
        public NPC_2082(Main.GameClient _client)
            : base(_client)
        {
            ID = 2082;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        List<string> Message = new List<string>() { "Wicked Witches Convention Tonight!",
                            "If you want a tasty sweet... Be sure to holler trick or treat!", "Join us at the ghost post.",
                        "Eat, drink and be scary.", "Bats all folks!", "Be the ghostess with the mostess.", "Boo to you from our crew.",
                            "The littlest pumpkins have the biggest grins.", "Jack-o-lanterns are on the cutting edge.",
                        "Happy Halloween whatever you are!", "The graveyard shift is best.  No bones about it!", "Are you a good witch or are you a bad witch?",
                        "Bugs & Hisses to you!", "Witches brew is good for you, sit for a spell, let yourself jell, and drink a lot from my pot."};
                        
                        AddText("Trick or treat? Halloween is here ! " + Message[Program.Rnd.Next(0, Message.Count)]);
                        AddOption("I witch you a Happy Halloween", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_2083 : NPC_2082
    {
        public NPC_2083(Main.GameClient _client)
            : base(_client)
        {
            ID = 2083;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2084 : NPC_2082
    {
        public NPC_2084(Main.GameClient _client)
            : base(_client)
        {
            ID = 2084;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2085 : NPC_2082
    {
        public NPC_2085(Main.GameClient _client)
            : base(_client)
        {
            ID = 2085;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2086 : NPC_2082
    {
        public NPC_2086(Main.GameClient _client)
            : base(_client)
        {
            ID = 2086;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2087 : NPC_2082
    {
        public NPC_2087(Main.GameClient _client)
            : base(_client)
        {
            ID = 2087;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2088 : NPC_2082
    {
        public NPC_2088(Main.GameClient _client)
            : base(_client)
        {
            ID = 2088;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2089 : NPC_2082
    {
        public NPC_2089(Main.GameClient _client)
            : base(_client)
        {
            ID = 2089;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}