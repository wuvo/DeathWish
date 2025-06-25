using Ultimate.Main;
using System;
using System.Collections.Generic;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    public class NPC_30020 : NPCBase
    {
        private const uint LevelUpCost = 50000000; // 50 million silver
        private const byte LevelCap = 150;

        public NPC_30020(Main.GameClient _client)
            : base(_client)
        {
            ID = 30020;
            Face = 67; // Set an appropriate face ID
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.Level >= 130)
                        {
                            AddText("Hi. I can help you level up if you pay 50KK silver per level. How many levels would you like to gain?");
                            AddOption("1 Level (50KK silver)", 1);
                            AddOption("No thanks.", 225);
                        }
                        else
                        {
                            AddText("You need to be level 130 or above to use this service.");
                            AddOption("Okay", 255);
                        }
                        AddFinish();
                        Send();
                        break;
                    }
                case 1:
                case 5:
                case 10:
                    {
                        int levels = _linkback;
                        uint cost = LevelUpCost * (uint)levels;

                        if (GC.MyChar.Level + levels > LevelCap)
                        {
                            AddText($"You cannot level up beyond level {LevelCap}. Please choose a lower amount.");
                            AddOption("I see.", 255);
                        }
                        else if (GC.MyChar.Silvers >= cost)
                        {
                            GC.MyChar.Silvers -= cost;
                            GC.MyChar.Experience = 0; // Reset experience
                            GC.MyChar.Level += (byte)levels;

                            AddText($"Congratulations! You have gained {levels} level(s).");
                            AddOption("Thank you.", 255);
                        }
                        else
                        {
                            AddText($"You don't have enough silver. You need {cost} silver to level up {levels} level(s).");
                            AddOption("I see.", 255);
                        }
                        AddFinish();
                        Send();
                        break;
                    }
                case 255:
                    {
                        // This case ensures that the dialog closes when "Okay" or "Thank you." is selected.
                        break;
                    }
                default:
                    {
                        AddText("Come back when your more rich pleb.");
                        AddOption("Okay", 255);
                        AddFinish();
                        Send();
                        break;
                    }
            }
        }
    }
}
