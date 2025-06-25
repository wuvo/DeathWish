using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [2095] TCCaptain
    /// </summary>
    public class NPC_2095 : NPCBase
    {
        public NPC_2095(Main.GameClient _client)
            : base(_client)
        {
            ID = 2095;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    if (Cloudsaint.Available((byte)(ID - 2094)).Count > 0 && !GC.MyChar.InventoryContains(750000, 1) && GC.MyChar.Level < 120)
                    {
                        AddText("Glad to see you here! This city is being besieged by the monsters recently. If you can help us out protecting the city from the monsters there will be plenty of rewards for you. Are you interested?");
                        AddOption("Yeah", 30);
                        AddOption("Just passing by", 255);
                    }
                    else if (Cloudsaint.Available((byte)(ID - 2094)).Count > 0 && GC.MyChar.InventoryContains(750000, 1))
                    {
                        if (GC.MyChar.CurrentKills >= Cloudsaint.SelectCount((byte)GC.MyChar.ToKill))
                        {
                            AddText("Amazing ! It seems like you have filled the jar with the monsters souls! Do you want to get your reward?");
                            AddOption("Yeah", 32);
                            AddOption("Just passing by", 255);
                        }
                        else
                        {
                            AddText("Why do you hurry to come back? What happened?");
                            AddText("The monsters are still invading the city! Aren't you going to help me?");
                            AddOption("I'll get back to it", 255);
                            AddOption("I give up", 31);
                        }
                    }
                    else
                    {
                        AddText("You're already too strong for this job, there are more cities that could use your help, you should visit them!");
                        AddOption("I see", 255);
                    }
                    break;
                case 30:
                    AddText("Great ! Which of these monsters are you willing to kill?");
                    foreach (var Monster in Cloudsaint.Available((byte)(ID - 2094)))
                    {
                        if (Monster.Key > GC.MyChar.Level - 5 && Monster.Key <= GC.MyChar.Level + 7) //27 >= 22 && 27 <= 30
                            AddOption($"{Monster.Value} L{Monster.Key}", (byte)Monster.Value);
                    }
                    if (GC.MyChar.Level < 27 && GC.MyChar.Loc.Map != 1002)
                    {
                        AddText("You need to go Twincity for CloudSaint Event. Do you want to go TwinCity?");
                        AddOption("Yeah", 40);
                    }
                    else if (GC.MyChar.Level >= 27 && GC.MyChar.Level < 47 && GC.MyChar.Loc.Map != 1011)
                    {
                        AddText("You need to go PhoenixCastle for CloudSaint Event. Do you want to go PhoenixCastle?");
                        AddOption("Yeah", 41);
                    }
                    else if (GC.MyChar.Level >= 47 && GC.MyChar.Level < 67 && GC.MyChar.Loc.Map != 1020)
                    {
                        AddText("You need to go ApeCity for CloudSaint Event. Do you want to go ApeCity?");
                        AddOption("Yeah", 42);
                    }
                    else if (GC.MyChar.Level >= 67 && GC.MyChar.Level < 87 && GC.MyChar.Loc.Map != 1000)
                    {
                        AddText("You need to go DesertCity for CloudSaint Event. Do you want to go DesertCity?");
                        AddOption("Yeah", 43);
                    }
                    else if (GC.MyChar.Level >= 87 && GC.MyChar.Level < 102 && GC.MyChar.Loc.Map != 1015)
                    {
                        AddText("You need to go BirdIsland for CloudSaint Event. Do you want to go BirdIsland?");
                        AddOption("Yeah", 44);
                    }
                    else if (GC.MyChar.Level >= 102 && GC.MyChar.Level < 120 && GC.MyChar.Loc.Map != 1000)
                    {
                        AddText("You need to go MysticCastle for CloudSaint Event. Do you want to go MysticCastle?");
                        AddOption("Yeah", 45);
                    }
                    AddOption("Just passing by", 255);
                    break;
                case 40:
                    GC.MyChar.Teleport(1002, 437, 438);
                    break;
                case 41:
                    GC.MyChar.Teleport(1011, 229, 258);
                    break;
                case 42:
                    GC.MyChar.Teleport(1020, 569, 620);
                    break;
                case 43:
                    GC.MyChar.Teleport(1000, 476, 634);
                    break;
                case 44:
                    GC.MyChar.Teleport(1015, 791, 568);
                    break;
                case 45:
                    GC.MyChar.Teleport(1000, 85, 323);
                    break;
                case 31:
                    if (GC.MyChar.InventoryContains(750000, 1))
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(750000));
                    AddText("You have given up the task! Let me know if you're interested ever again!");
                    AddOption("I will", 255);
                    break;
                case 32:
                    if (Cloudsaint.Award(GC.MyChar, (byte)GC.MyChar.ToKill))
                    {
                        AddText("Here you go ! Enjoy your reward !");
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("Please confirm that you have enough space in your inventory and that you have the correct jar with all the souls.");
                        AddOption("I will", 255);
                    }
                    break;
                default:
                    if (_linkback != 255)
                    {
                        if (Cloudsaint.SelectMonster(GC.MyChar, (byte)_linkback))
                        {
                            AddText("There you go ! Come back to me when you've killed enough monsters and filled the jar with the souls you collected from them!");
                            AddOption("Alright", 255);
                        }
                        else
                        {
                            AddText("Please finish your current mission first!");
                            AddOption("I see", 255);
                        }
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}