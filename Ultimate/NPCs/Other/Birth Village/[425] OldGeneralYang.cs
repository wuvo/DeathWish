using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.NPCs
{
    public class NPC_425 : NPCBase
    {
        public NPC_425(Main.GameClient _client)
            : base(_client)
        {
            ID = 425;
            Face = 9;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.Job == 10 || GC.MyChar.Job == 20)
                        {
                            AddText("KnowItAll~on~the~bridge~will~teleport~you~to~Twin~City~to~start~playing.~I`ve~a~strong~feeling~that~you`ll~be~a~hero.");
                            AddOption("I~have~questions~still.", 1);
                            AddOption("Let`s~say~goodbye.", 255);
                        }
                        else
                        {
                            AddText("I~am~here~to~teach~weapon~skill~to~Warrior~and~Trojan.~Sorry~that~I~am~unable~to~help~you.");
                            AddOption("Thanks~anyway.", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Well,~what~questions?");
                        AddOption("Where~to~get~weapons~from?", 2);
                        AddOption("Where~to~learn~skills~from?", 3);
                        AddOption("No~problem.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("Pedlar~in~Twin~City~sells~all~sorts~of~elementary~weapons.~For~better~weapons~you~need~buy~from~Blacksmith.");
                        AddText("If~you~are~lucky~enough~you~may~get~weapons~after~the~monsters~you`ve~killed.");
                        AddOption("Thank~you.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("NPCs~in~Job~Center~of~Twin~City~will~teach~you~skills~and~spells.~Monsters~may~drop~skill~books~too.~Pharmacist~in");
                        AddText("the~Market~also~sell~them~at~considerable~price.");
                        AddOption("Thank~you.", 255);
                        break;
                    }
                    
            }

            AddFinish();
            Send();
        }
    }
}
