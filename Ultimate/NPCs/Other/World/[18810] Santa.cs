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
    public class NPC_18810 : NPCBase
    {
        public NPC_18810(Main.GameClient _client)
            : base(_client)
        {
            ID = 18810;
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
                        if (GC.MyChar.Inventory.Count < 39)
                        {
                            NPC N = null;
                            Dictionary<uint, NPC> MapNPC = World.H_NPCs[GC.MyChar.Loc.Map];
                            if (MapNPC != null && MapNPC.ContainsKey(ID)/* || NPC == 12*/)
                                N = (NPC)MapNPC[ID];
                            MapNPC.Remove(ID);
                            Game.World.Action(N, Packets.GeneralData(ID, 0, 0, 0, 135).Get);
                            Game.World.Found = true;
                            #region Prizes
                            int i = Program.Rnd.Next(0, 6);
                            #region 2kk
                            if (i == 0)
                            {
                                World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received 550,000 silvers!", 2011, 0);
                                AddText("You received 550,000 silvers.");
                                AddOption("Thanks.", 255);
                                GC.MyChar.Silvers += 550000;
                            }
                            #endregion
                            #region DB
                            else if (i == 1)
                            {
                                World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received a DragonBall!", 2011, 0);
                                AddText("You received a DragonBall!");
                                AddOption("Thanks", 255);
                                GC.MyChar.AddItem(1088000);
                            }
                            #endregion
                            #region MB
                            else if (i == 2)
                            {
                                World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received a MoonBox!", 2011, 0);
                                AddText("You received a MoonBox!");
                                AddOption("Thanks", 255);
                                GC.MyChar.AddItem(721080);
                            }
                            #endregion
                            #region //ChristmasCap
                            //else if (i == 3)
                            //{
                            //    AddText("You received a ChristmasCap!");
                            //    AddOption("Thanks.", 255);
                            //    GC.MyChar.AddItem(115000);
                            //}
                            #endregion
                            #region Gem
                            else if (i == 3)
                            {
                                int j = Program.Rnd.Next(0, 8);
                                uint Item = (uint)(700002 + (j * 10));
                                if (MyMath.ChanceSuccess(0.08))
                                {
                                    Item += 1;
                                    World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received a Super Gem!", 2011, 0);
                                }
                                else
                                    World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received a Refined Gem!", 2011, 0);
                                AddText("You received a Gem!");
                                AddOption("Thanks", 255);
                                GC.MyChar.AddItem(Item);
                            }
                            #endregion
                            #region FruitPack
                            else if (i == 4)
                            {
                                World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received an FruitPack!", 2011, 0);
                                AddText("You received an FruitPack!");
                                AddOption("Thanks", 255);
                                GC.MyChar.AddItem(720142);
                            }
                            #endregion
                            #region CleanWater
                            else if (i == 5)
                            {
                                World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has found Santa and received a CleanWater!", 2011, 0);
                                AddText("You received a CleanWater!");
                                AddOption("Thanks.", 255);
                                GC.MyChar.AddItem(721258);
                            }
                            #endregion
                            #endregion
                        }
                        else
                        {
                            AddText("Please make some room in your inventory.");
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