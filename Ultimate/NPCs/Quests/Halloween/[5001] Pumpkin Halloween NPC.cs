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
    public class NPC_5001 : NPCBase
    {
        public NPC_5001(Main.GameClient _client)
            : base(_client)
        {
            ID = 5001;
            Face = 3700;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("When black cats prowl and pumpkins gleam, may luck be yours on Halloween.");
/*                        AddText("I'm looking for brave players to collect them for me and paying 1 Pumpkin Poin for 2 Pumpkins! ");*/
/*                        AddText("Alternatively, you can also give me 5 Pumpkin Seeds in exchange for a Big Pumpkin!");*/
                        //AddOption("Check my Pumpkin Points", 1);
                        //AddOption("Exchange Pumpkins", 2);
                        //AddOption("I want to get my prize!", 3);
                        //AddOption("I have 5 Pumpkin Seeds!", 8);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points.");
                    AddOption("Exchange Pumpkins", 2);
                    AddOption("I want to get my prize!", 3);
                    AddOption("Just passing by.", 255);
                    break;
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(722176, 2))
                        {
                            for (int a = 0; a < 2; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722176));

                            GC.MyChar.PumpkinPoints++;
                            AddText("Congratulations! You have " + GC.MyChar.PumpkinPoints + " pumpkin points now!");
                            AddOption("Thanks.", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have 2 Pumpkins!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points! What prize would you like to get?");
                        AddOption("Halloween Regular Reward (10 Pts)", 4);
                        AddOption("Halloween Bronze Reward (20 Pts)", 5);
                        AddOption("Halloween Silver Reward (30 Pts)", 6);
                        AddOption("Halloween Gold Reward (50 Pts)", 7);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 4:
                    if (GC.MyChar.PumpkinPoints >= 10)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 10;
                            GC.MyChar.AddItem(720139);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 5:
                    if (GC.MyChar.PumpkinPoints >= 20)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 20;
                            GC.MyChar.AddItem(720138);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 6:
                    if (GC.MyChar.PumpkinPoints >= 30)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 30;
                            GC.MyChar.AddItem(720137);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 7:
                    if (GC.MyChar.PumpkinPoints >= 50)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 50;
                            GC.MyChar.AddItem(720136);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 8:
                    if (GC.MyChar.InventoryContains(710587, 5))
                    {
                        for (int a = 0; a < 5; a++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710587));
                        GC.MyChar.AddItem(721960);
                        AddText("Here's your Big Pumpkin! Make sure you use it in a hidden place");
                        AddOption("Thanks!", 255);
                    }
                    else
                    {
                        AddText("I'm sorry but you don't have 5 Pumpkin Seeds!");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }

    }
    public class NPC_5002 : NPCBase
    {
        public NPC_5002(Main.GameClient _client)
            : base(_client)
        {
            ID = 5002;
            Face = 3700;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("say boo and scary on.");
/*                        AddText("I'm looking for brave players to collect them for me and paying 1 Pumpkin Poin for 2 Pumpkins! ");*/
/*                        AddText("Alternatively, you can also give me 5 Pumpkin Seeds in exchange for a Big Pumpkin!");*/
                        //AddOption("Check my Pumpkin Points", 1);
                        //AddOption("Exchange Pumpkins", 2);
                        //AddOption("I want to get my prize!", 3);
                        //AddOption("I have 5 Pumpkin Seeds!", 8);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points.");
                    AddOption("Exchange Pumpkins", 2);
                    AddOption("I want to get my prize!", 3);
                    AddOption("Just passing by.", 255);
                    break;
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(722176, 2))
                        {
                            for (int a = 0; a < 2; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722176));

                            GC.MyChar.PumpkinPoints++;
                            AddText("Congratulations! You have " + GC.MyChar.PumpkinPoints + " pumpkin points now!");
                            AddOption("Thanks.", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have 2 Pumpkins!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points! What prize would you like to get?");
                        AddOption("Halloween Regular Reward (10 Pts)", 4);
                        AddOption("Halloween Bronze Reward (20 Pts)", 5);
                        AddOption("Halloween Silver Reward (30 Pts)", 6);
                        AddOption("Halloween Gold Reward (50 Pts)", 7);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 4:
                    if (GC.MyChar.PumpkinPoints >= 10)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 10;
                            GC.MyChar.AddItem(720139);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 5:
                    if (GC.MyChar.PumpkinPoints >= 20)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 20;
                            GC.MyChar.AddItem(720138);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 6:
                    if (GC.MyChar.PumpkinPoints >= 30)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 30;
                            GC.MyChar.AddItem(720137);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 7:
                    if (GC.MyChar.PumpkinPoints >= 50)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 50;
                            GC.MyChar.AddItem(720136);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 8:
                    if (GC.MyChar.InventoryContains(710587, 5))
                    {
                        for (int a = 0; a < 5; a++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710587));
                        GC.MyChar.AddItem(721960);
                        AddText("Here's your Big Pumpkin! Make sure you use it in a hidden place");
                        AddOption("Thanks!", 255);
                    }
                    else
                    {
                        AddText("I'm sorry but you don't have 5 Pumpkin Seeds!");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }

    }
    public class NPC_5003 : NPCBase
    {
        public NPC_5003(Main.GameClient _client)
            : base(_client)
        {
            ID = 5003;
            Face = 3700;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Keep calm and eat more candy.");
/*                        AddText("I'm looking for brave players to collect them for me and paying 1 Pumpkin Poin for 2 Pumpkins! ");*/
/*                        AddText("Alternatively, you can also give me 5 Pumpkin Seeds in exchange for a Big Pumpkin!");*/
                        //AddOption("Check my Pumpkin Points", 1);
                        //AddOption("Exchange Pumpkins", 2);
                        //AddOption("I want to get my prize!", 3);
                        //AddOption("I have 5 Pumpkin Seeds!", 8);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points.");
                    AddOption("Exchange Pumpkins", 2);
                    AddOption("I want to get my prize!", 3);
                    AddOption("Just passing by.", 255);
                    break;
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(722176, 2))
                        {
                            for (int a = 0; a < 2; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722176));

                            GC.MyChar.PumpkinPoints++;
                            AddText("Congratulations! You have " + GC.MyChar.PumpkinPoints + " pumpkin points now!");
                            AddOption("Thanks.", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have 2 Pumpkins!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points! What prize would you like to get?");
                        AddOption("Halloween Regular Reward (10 Pts)", 4);
                        AddOption("Halloween Bronze Reward (20 Pts)", 5);
                        AddOption("Halloween Silver Reward (30 Pts)", 6);
                        AddOption("Halloween Gold Reward (50 Pts)", 7);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 4:
                    if (GC.MyChar.PumpkinPoints >= 10)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 10;
                            GC.MyChar.AddItem(720139);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 5:
                    if (GC.MyChar.PumpkinPoints >= 20)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 20;
                            GC.MyChar.AddItem(720138);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 6:
                    if (GC.MyChar.PumpkinPoints >= 30)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 30;
                            GC.MyChar.AddItem(720137);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 7:
                    if (GC.MyChar.PumpkinPoints >= 50)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 50;
                            GC.MyChar.AddItem(720136);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 8:
                    if (GC.MyChar.InventoryContains(710587, 5))
                    {
                        for (int a = 0; a < 5; a++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710587));
                        GC.MyChar.AddItem(721960);
                        AddText("Here's your Big Pumpkin! Make sure you use it in a hidden place");
                        AddOption("Thanks!", 255);
                    }
                    else
                    {
                        AddText("I'm sorry but you don't have 5 Pumpkin Seeds!");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }

    }
    public class NPC_5005 : NPCBase
    {
        public NPC_5005(Main.GameClient _client)
            : base(_client)
        {
            ID = 5005;
            Face = 3700;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("the only thing we have to fear if fear itself.");
/*                        AddText("I'm looking for brave players to collect them for me and paying 1 Pumpkin Poin for 2 Pumpkins! ");*/
/*                        AddText("Alternatively, you can also give me 5 Pumpkin Seeds in exchange for a Big Pumpkin!");*/
                        //AddOption("Check my Pumpkin Points", 1);
                        //AddOption("Exchange Pumpkins", 2);
                        //AddOption("I want to get my prize!", 3);
                        //AddOption("I have 5 Pumpkin Seeds!", 8);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points.");
                    AddOption("Exchange Pumpkins", 2);
                    AddOption("I want to get my prize!", 3);
                    AddOption("Just passing by.", 255);
                    break;
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(722176, 2))
                        {
                            for (int a = 0; a < 2; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722176));

                            GC.MyChar.PumpkinPoints++;
                            AddText("Congratulations! You have " + GC.MyChar.PumpkinPoints + " pumpkin points now!");
                            AddOption("Thanks.", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have 2 Pumpkins!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points! What prize would you like to get?");
                        AddOption("Halloween Regular Reward (10 Pts)", 4);
                        AddOption("Halloween Bronze Reward (20 Pts)", 5);
                        AddOption("Halloween Silver Reward (30 Pts)", 6);
                        AddOption("Halloween Gold Reward (50 Pts)", 7);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 4:
                    if (GC.MyChar.PumpkinPoints >= 10)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 10;
                            GC.MyChar.AddItem(720139);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 5:
                    if (GC.MyChar.PumpkinPoints >= 20)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 20;
                            GC.MyChar.AddItem(720138);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 6:
                    if (GC.MyChar.PumpkinPoints >= 30)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 30;
                            GC.MyChar.AddItem(720137);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 7:
                    if (GC.MyChar.PumpkinPoints >= 50)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 50;
                            GC.MyChar.AddItem(720136);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 8:
                    if (GC.MyChar.InventoryContains(710587, 5))
                    {
                        for (int a = 0; a < 5; a++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710587));
                        GC.MyChar.AddItem(721960);
                        AddText("Here's your Big Pumpkin! Make sure you use it in a hidden place");
                        AddOption("Thanks!", 255);
                    }
                    else
                    {
                        AddText("I'm sorry but you don't have 5 Pumpkin Seeds!");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }

    }
    public class NPC_5006 : NPCBase
    {
        public NPC_5006(Main.GameClient _client)
            : base(_client)
        {
            ID = 5006;
            Face = 3700;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("everyday is halloween, isn't it? for some of us.");
/*                        AddText("I'm looking for brave players to collect them for me and paying 1 Pumpkin Poin for 2 Pumpkins! ");*/
/*                        AddText("Alternatively, you can also give me 5 Pumpkin Seeds in exchange for a Big Pumpkin!");*/
                        //AddOption("Check my Pumpkin Points", 1);
                        //AddOption("Exchange Pumpkins", 2);
                        //AddOption("I want to get my prize!", 3);
                        //AddOption("I have 5 Pumpkin Seeds!", 8);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points.");
                    AddOption("Exchange Pumpkins", 2);
                    AddOption("I want to get my prize!", 3);
                    AddOption("Just passing by.", 255);
                    break;
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(722176, 2))
                        {
                            for (int a = 0; a < 2; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722176));

                            GC.MyChar.PumpkinPoints++;
                            AddText("Congratulations! You have " + GC.MyChar.PumpkinPoints + " pumpkin points now!");
                            AddOption("Thanks.", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have 2 Pumpkins!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        AddText("You currently have " + GC.MyChar.PumpkinPoints + " pumpkin points! What prize would you like to get?");
                        AddOption("Halloween Regular Reward (10 Pts)", 4);
                        AddOption("Halloween Bronze Reward (20 Pts)", 5);
                        AddOption("Halloween Silver Reward (30 Pts)", 6);
                        AddOption("Halloween Gold Reward (50 Pts)", 7);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 4:
                    if (GC.MyChar.PumpkinPoints >= 10)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 10;
                            GC.MyChar.AddItem(720139);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 5:
                    if (GC.MyChar.PumpkinPoints >= 20)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 20;
                            GC.MyChar.AddItem(720138);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 6:
                    if (GC.MyChar.PumpkinPoints >= 30)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 30;
                            GC.MyChar.AddItem(720137);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 7:
                    if (GC.MyChar.PumpkinPoints >= 50)
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.PumpkinPoints -= 50;
                            GC.MyChar.AddItem(720136);
                            AddText("Here's your reward!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough points!");
                        AddOption("I see", 255);
                    }
                    break;
                case 8:
                    if (GC.MyChar.InventoryContains(710587, 5))
                    {
                        for (int a = 0; a < 5; a++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710587));
                        GC.MyChar.AddItem(721960);
                        AddText("Here's your Big Pumpkin! Make sure you use it in a hidden place");
                        AddOption("Thanks!", 255);
                    }
                    else
                    {
                        AddText("I'm sorry but you don't have 5 Pumpkin Seeds!");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }

    }


}