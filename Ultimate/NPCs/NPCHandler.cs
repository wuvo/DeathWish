using Ultimate.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.NPCs
{
    public static class NPCHandler
    {
        public static string ReadString(byte[] Data)
        {
            string Name = "";
            for (int i = 14; i < 14 + Data[13]; i++)
                Name += Convert.ToChar(Data[i]);
            return Name;
        }
        public static void Handle(Main.GameClient _client, byte[] Data, uint _npc, ushort _linkback)
        {
            try
            {
                if (_client.DialogNPC == 9999997 && _client.WaitingKillCaptcha)
                {
                    if (_linkback == 255) return;
                    string Id = (ReadString(Data));

                    if (Id == _client.KillCountCaptcha)
                    {
                        _client.SolveCaptcha();
                    }
                    else
                    {
                        _client.AddSend(Packets.NPCSay("Input the current text: " + _client.KillCountCaptcha + " to verify your humanity."));
                        _client.AddSend(Packets.NPCLink2("Captcha message:", (byte)_client.KillCountCaptcha.Length));
                        _client.AddSend(Packets.NPCLink("Just passing by", 255));
                        _client.AddSend(Packets.NPCSetFace(30));
                        _client.AddSend(Packets.NPCFinish());

                    }
                    return;
                }
                var type = Type.GetType("Ultimate.NPCs.NPC_" + _npc);
                var npc = Activator.CreateInstance(type, _client) as NPCBase;

                if (_client.AuthInfo.Status == "[PM]" && npc != null)
                    _client.LocalMessage(2000, $"NpcId: {npc.ID}");
                NPC N = null;
                if (World.H_NPCs.ContainsKey(_client.MyChar.Loc.Map))
                {
                    Dictionary<uint, NPC> MapNPC = World.H_NPCs[_client.MyChar.Loc.Map];
                    if (MapNPC != null && MapNPC.ContainsKey(_npc))
                        N = (NPC)MapNPC[_npc];
                    else if (_npc < 6700 || _npc > 6705)
                    {
                        bool found = false;
                        foreach (Dictionary<uint, NPC> H in World.H_NPCs.Values)
                        {
                            foreach (NPC NN in H.Values)
                            {
                                if (NN.EntityID == _npc)
                                {
                                    N = NN;
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                                break;
                        }
                    }
                }
                if (!(_client.MyChar.Loc.Map == N?.Loc.Map && MyMath.PointDistance(_client.MyChar.Loc.X, _client.MyChar.Loc.Y, N.Loc.X, N.Loc.Y) <= 22) && !npc.IsGlobal)
                {
                    // Program.WriteLine("{0} trying to use non global npc {1} while not on screen!", _client.Name, _npc); 
                    return;
                }
                if (N != null)
                    if (N.Loc.Map == _client.MyChar.EntityID)
                        if (N.Flags == 26)
                            return;
                _client.CurrentNPC = npc;
                _client.DialogNPC = _npc;
                npc.Run(_client, Data, _linkback);
            }
            catch (Exception)
            {
                if (_npc != 0 && _npc != 9999997)
                    Console.WriteLine("Could not load npc script for npc ID: " + _npc);
            }
        }
    }
}
