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
    /// Handles NPC usage for [3001] ConductorDarwen
    /// </summary>
    public class NPC_3001 : NPCBase
    {
        public NPC_3001(Main.GameClient _client)
            : base(_client)
        {
            ID = 3001;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Christmas is here! We need to go outside and share the love in this season! I want to gather up some friends and sing from door to door ");
                    AddText("but I'll need a Music Book containing all the Christmas Carols! I heard the monsters are holding some pages! Can you get them for me?");
                    AddOption("I will", 255);
                    AddOption("Here's your Music Book", 1);
                    AddOption("Just passing by", 255);
                    break;
                case 1:
                    if (GC.MyChar.InventoryContains(720156, 1))
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(720156));
                        Random Rnd = new Random();
                        switch (Rnd.Next(0, 27))
                        {
                            case 0:
                                GC.MyChar.AddItem(721954);
                                AddText("Hurray ! Take this TransformationCandy as a reward for your hardwork!\n");
                                break;
                            case 1:
                                GC.MyChar.AddItem(720165);
                                AddText("Hurray ! Take this ChristmasTree as a reward for your hardwork!\n");
                                break;
                            case 2:
                                GC.MyChar.AddItem(720166);
                                AddText("Hurray ! Take this ChristmasTree as a reward for your hardwork!\n");
                                break;
                            case 3:
                                GC.MyChar.AddItem(720167);
                                AddText("Hurray ! Take this ChristmasTree as a reward for your hardwork!\n");
                                break;
                            case 4:
                                GC.MyChar.AddItem(720164);
                                AddText("Hurray ! Take this Snowman as a reward for your hardwork!\n");
                                break;
                            case 5:
                                GC.MyChar.AddItem(720650);
                                AddText("Hurray ! Take this DemonBox as a reward for your hardwork!\n");
                                break;
                            case 6:
                                GC.MyChar.AddItem(720651);
                                AddText("Hurray ! Take this AncientDemonBox as a reward for your hardwork!\n");
                                break;
                            case 7:
                                GC.MyChar.AddItem(115000);
                                AddText("Hurray ! Take this ChristmasCap as a reward for your hardwork!\n");
                                break;
                            case 8:
                                GC.MyChar.AddItem(115010);
                                AddText("Hurray ! Take this GiftHat as a reward for your hardwork!\n");
                                break;
                            case 9:
                                GC.MyChar.AddItem(722700);
                                AddText("Hurray ! Take this MiniExpPotion as a reward for your hardwork!\n");
                                break;
                            case 10:
                                GC.MyChar.AddItem(722384);
                                AddText("Hurray ! Take this ProficiencyToken as a reward for your hardwork!\n");
                                break;
                            case 11:
                                GC.MyChar.AddItem(720392);
                                AddText("Hurray ! Take this ChristmasTree as a reward for your hardwork!\n");
                                break;
                            case 12:
                                GC.MyChar.AddItem(720391);
                                AddText("Hurray ! Take this ChristmasWreath as a reward for your hardwork!\n");
                                break;
                            case 13:
                                GC.MyChar.AddItem(720664);
                                AddText("Hurray ! Take this SuperBall as a reward for your hardwork!\n");
                                break;
                            case 14:
                                GC.MyChar.AddItem(720658);
                                AddText("Hurray ! Take this MagicBall as a reward for your hardwork!\n");
                                break;
                            case 15:
                                GC.MyChar.AddItem(720670);
                                AddText("Hurray ! Take this UltraBall as a reward for your hardwork!\n");
                                break;
                            case 16:
                                GC.MyChar.AddItem(721261);
                                AddText("Hurray ! Take this Bomb as a reward for your hardwork!\n");
                                break;
                            case 17:
                                GC.MyChar.AddItem(722114);
                                AddText("Hurray ! Take this MasqueBox as a reward for your hardwork!\n");
                                break;
                            case 18:
                                GC.MyChar.AddItem(722107);
                                AddText("Hurray ! Take this PheasantPlate as a reward for your hardwork!\n");
                                break;
                            case 19:
                                GC.MyChar.AddItem(722108);
                                AddText("Hurray ! Take this TurtledovePlate as a reward for your hardwork!\n");
                                break;
                            case 20:
                                GC.MyChar.AddItem(722109);
                                AddText("Hurray ! Take this RobinPlate as a reward for your hardwork!\n");
                                break;
                            case 21:
                                GC.MyChar.AddItem(722110);
                                AddText("Hurray ! Take this ApparitionPlate as a reward for your hardwork!\n");
                                break;
                            case 22:
                                GC.MyChar.AddItem(722111);
                                AddText("Hurray ! Take this WingedSnakePlate as a reward for your hardwork!\n");
                                break;
                            case 23:
                                GC.MyChar.AddItem(722113);
                                AddText("Hurray ! Take this RatlingPlate as a reward for your hardwork!\n");
                                break;
                            case 24:
                                GC.MyChar.AddItem(1088000);
                                AddText("Hurray ! Take this DragonBall as a reward for your hardwork!\n");
                                break;
                            case 25:
                                GC.MyChar.AddItem(1080001);
                                AddText("Hurray ! Take this Emerald as a reward for your hardwork!\n");
                                break;
                            case 26:
                                GC.MyChar.AddItem(723017);
                                AddText("Hurray ! Take this ExpPotion as a reward for your hardwork!\n");
                                break;
                        }
                        AddText($"Maybe you want to come with me and sing some happy songs to everyone?");
                        AddOption("I will", 255);
                        AddOption("Maybe next year", 255);
                    }
                    else
                    {
                        AddText("You don't have a MusicBook! Go find or there'll be no true Christmas this year!");
                        AddOption("Alright", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }

}