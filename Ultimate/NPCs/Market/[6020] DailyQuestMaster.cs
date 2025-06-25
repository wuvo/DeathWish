using Ultimate.Main;
using System.Collections.Generic;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    public class NPC_6020 : NPCBase
    {
        public NPC_6020(GameClient client) : base(client)
        {
            ID = 6020;
            Face = 30;
        }

        public override void Run(GameClient GC, byte[] data, ushort linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (linkback)
            {
                case 0:
                    if (GC.MyChar.DailyQuestCompleted)
                    {
                        AddText("Great job! Here is your reward.");
                        AddOption("Claim reward", 2);
                    }
                    else if (GC.MyChar.DailyQuestActive)
                    {
                        AddText($"Keep hunting! {GC.MyChar.DailyQuestKills}/100 monsters defeated.");
                    }
                    else
                    {
                        AddText("Would you like a task? Kill 100 monsters anywhere today.");
                        AddOption("I'll do it", 1);
                    }
                    AddOption("Maybe later", 255);
                    break;
                case 1:
                    GC.MyChar.DailyQuestActive = true;
                    GC.MyChar.DailyQuestKills = 0;
                    GC.MyChar.DailyQuestCompleted = false;
                    GC.MyChar.DailyQuestDate = System.DateTime.Today;
                    AddText("Come back when you've killed 100 monsters.");
                    AddOption("OK", 255);
                    break;
                case 2:
                    if (GC.MyChar.DailyQuestCompleted && GC.MyChar.GetAvailableInventorySlots() > 0)
                    {
                        GC.MyChar.AddItem(1088000);
                        GC.MyChar.DailyQuestCompleted = false;
                        AddText("Enjoy your DragonBall! Come back tomorrow for a new quest.");
                    }
                    else
                    {
                        AddText("You haven't completed the quest or don't have space.");
                    }
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
