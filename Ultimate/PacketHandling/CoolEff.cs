using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    class CoolEffect
    {
        public static void ActiveCool(Main.GameClient MyClient)
        {
            byte counter = 0;
            bool superarmor = false;
            for (byte i = 1; i < 9; i++)
            {
                if (i == 7) i++;
                Game.Item I = MyClient.MyChar.Equips.Get(i);
                if (I.ID != 0)
                {
                    Game.ItemIDManipulation Q = new Ultimate.Game.ItemIDManipulation(I.ID);
                    if (Q.Quality == Game.Item.ItemQuality.Super)
                    {
                        counter += 1;
                        if (i == 3)
                            superarmor = true;
                    }
                }
            }

            if (MyClient.MyChar.Job >= 100)
                if (counter == 6)
                    counter = 7;
            if (MyClient.MyChar.Job >= 40 && MyClient.MyChar.Job <= 45)
                if (counter == 6)
                {
                    Game.Item I = MyClient.MyChar.Equips.Get(5);
                    I.ID = MyClient.MyChar.Equips.LeftHand.ID;
                    if (I.ID == 0 || Game.Item.IsArrow(I.ID))
                        counter = 7;
                }
            if (counter == 7)
            {
                if (MyClient.MyChar.Job >= 10 && MyClient.MyChar.Job <= 15)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "warrior"));
                else if (MyClient.MyChar.Job >= 20 && MyClient.MyChar.Job <= 25)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "fighter"));
                else if (MyClient.MyChar.Job >= 100)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "taoist"));
                else if (MyClient.MyChar.Job >= 39 && MyClient.MyChar.Job <= 46)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "archer"));
            }
            else if (superarmor)
            {
                if (MyClient.MyChar.Job >= 10 && MyClient.MyChar.Job <= 15)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "warrior-s"));
                else if (MyClient.MyChar.Job >= 20 && MyClient.MyChar.Job <= 25)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "fighter-s"));
                else if (MyClient.MyChar.Job >= 100)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "taoist-s"));
                else if (MyClient.MyChar.Job >= 39 && MyClient.MyChar.Job <= 46)
                    MyClient.MyChar.SendScreen(Packets.StringPacket(MyClient.MyChar.EntityID, Game.StringType.Effect, "archer-s"));
            }

            MyClient.MyChar.Action = 100;

        }
    }
}