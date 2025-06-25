using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;

namespace Ultimate.Features
{
    class GemEffect
    {
        //public static void BlessEffects(Game.Item Item, Main.GameClient MyClient)
        //{
        //    if (Item.Bless > 0)
        //    {
        //        if (MyMath.ChanceSuccess(60))
        //        {
        //            MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, 10, "reddragon"));
        //        }
        //    }
        //}
        public static void GemEffects(Game.Item.Gem Gem, Main.GameClient MyClient, Character C)
        {
            switch (Gem)
            {
                #region GemEffects

                case Game.Item.Gem.SuperDragonGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "goldendragon"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperPhoenixGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "phoenix"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperRainbowGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "rainbow"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperTortoiseGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "recovery"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperMoonGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "moon"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperVioletGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "purpleray"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperFuryGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "fastflash"));
                            }
                        }
                        break;
                    }
                case Game.Item.Gem.SuperKylinGem:
                    {
                        if (C.GemEffectsRemove == true)
                        {
                            if (MyMath.ChanceSuccess(2.5))
                            {
                                MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, StringType.Effect, "goldenkylin"));
                            }
                        }
                        break;
                    }
                    #endregion
            }
        }
    }
}