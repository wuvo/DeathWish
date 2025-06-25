using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [3000] Snowman
    /// </summary>
    public class NPC_3000 : NPCBase
    {
        public NPC_3000(Main.GameClient _client)
            : base(_client)
        {
            ID = 3000;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("The winter has arrived! However it seems like the weather isn't cold enough for me, I keep melting down! I'm looking for brave adventurers who are willing ");
                    AddText("to find some snowballs for me so that I can keep alive until the end of Christmas! Every 1,000 snowballs delivered I'll be giving everyone a reward!");
                    AddOption("Check Snowballs count", 1);
                    AddOption("Deliver Snowballs", 2);
                    AddOption("Just passing by", 255);
                    break;
                case 1:
                    AddText($"I still need to get {World.Snowballs} until I give the next reward!");
                    AddOption("Deliver Snowballs", 2);
                    AddOption("I see", 255);
                    break;
                case 2:
                    if (GC.MyChar.InventoryContains(720163, 1))
                    {
                        byte Count = GC.MyChar.InventoryItemIDCount(720163);
                        for (int a = 0; a < Count; a++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(720163));
                        World.Snowballs -= Count;
                        if (World.Snowballs - Count > 0)
                        {
                            AddText($"You have delivered {Count} Snowballs! I still need {World.Snowballs} before I give everyone a reward!");
                            AddOption("Alright", 255);
                        }
                        else
                        {
                            AddText($"Hurray! You have delivered {Count} Snowballs and I have finally gathered 1,000 Snowballs to keep me cold for a while! I'll give everyone a reward!");
                            AddOption("Thanks", 255);
                            World.Snowballs = 1000;

                            Random Rnd = new Random();
                            switch (Rnd.Next(0, 15))
                            {
                                case 0:
                                    World.EREvent = DateTime.Now.AddMinutes(30);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has doubled the Exp Rate for 30 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 1:
                                    World.EREvent = DateTime.Now.AddMinutes(60);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has doubled the Exp Rate for 60 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 2:
                                    World.EREvent = DateTime.Now.AddMinutes(120);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has doubled the Exp Rate for 2 hours! Merry Christmas!", 2011, 0);
                                    break;
                                case 3:
                                    World.DREvent = DateTime.Now.AddMinutes(30);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has increased the drop rates for 30 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 4:
                                    World.DREvent = DateTime.Now.AddMinutes(60);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has increased the drop rates for 60 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 5:
                                    World.DREvent = DateTime.Now.AddMinutes(120);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has increased the drop rates for 2 hours! Merry Christmas!", 2011, 0);
                                    break;
                                case 6:
                                    World.ExpMob = true;
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has spawned an EXPMob in the Promotion Center! Merry Christmas!", 2011, 0);
                                    break;
                                case 7:
                                    World.Raikou = true;
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has spawned Raikou in the MoonPlatform! Merry Christmas!", 2011, 0);
                                    break;
                                case 8:
                                    World.Capricorn = true;
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has spawned Capricorn in PhoenixCastle! Merry Christmas!", 2011, 0);
                                    break;
                                case 9:
                                    World.Tash = true;
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has spawned Tash in DesertCity! Merry Christmas!", 2011, 0);
                                    break;
                                case 10:
                                    World.ThrillingSpook = true;
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has spawned ThrillingSpook in BirdIsland! Merry Christmas!", 2011, 0);
                                    break;
                                case 11:
                                    World.EREvent = DateTime.Now.AddMinutes(60);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has doubled the Exp Rate for 60 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 12:
                                    SpawnDemonBoxes();
                                    World.DemonBoxStarted = DateTime.Now.AddMinutes(30);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has called the DemonBox NPC for 30 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 13:
                                    SpawnDemonBoxes();
                                    World.DemonBoxStarted = DateTime.Now.AddMinutes(60);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has called the DemonBox NPC for 60 minutes! Merry Christmas!", 2011, 0);
                                    break;
                                case 14:
                                    SpawnDemonBoxes();
                                    World.DemonBoxStarted = DateTime.Now.AddMinutes(120);
                                    World.SendMsgToAll("[SYSTEM]", "Snowman has received 1,000 Snowballs from everyone who helped him and has called the DemonBox NPC for 2 hours! Merry Christmas!", 2011, 0);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        AddText("You don't have any Snowballs! Go find some before I melt down!");
                        AddOption("Alright", 255);
                    }
                    break;

            }

            AddFinish();
            Send();
        }

        private void SpawnDemonBoxes()
        {
            NPC N = new NPC()
            {
                EntityID = 2084,
                Type = 1850,
                Flags = 1,
                Avatar = 0,
                Loc = new Location() { Map = 1002, X = 436, Y = 382 }
            };
            if (!World.H_NPCs.ContainsKey(N.Loc.Map))
                World.H_NPCs.Add(N.Loc.Map, new Dictionary<uint, NPC>());

            Dictionary<uint, NPC> NPCMap = World.H_NPCs[N.Loc.Map];
            if (!NPCMap.ContainsKey(N.EntityID))
            {
                NPCMap.Add(N.EntityID, N);
                World.Spawn(N);
            }
            World.DemonBoxes = true;
        }
    }
    
}
