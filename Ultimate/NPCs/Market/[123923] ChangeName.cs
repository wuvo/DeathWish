using Ultimate.Main;
using System;
using System.Collections.Generic;
using Ultimate.Game;
using Ultimate.Features;
using System.IO;

namespace Ultimate.NPCs
{
    public class NPC_123923 : NPCBase
    {
        public NPC_123923(GameClient _client)
            : base(_client)
        {
            ID = 123923;
            Face = 6;
        }

        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            switch (_linkback)
            {
                case 0:
                    {
                        GC.AddSend(Packets.NPCSay("Would you like to change name? Costs 200 million."));
                        GC.AddSend(Packets.NPCLink2("Enter name:", 2));
                        GC.AddSend(Packets.NPCLink("Thank you!", 255));
                        GC.AddSend(Packets.NPCSetFace(80));
                        GC.AddSend(Packets.NPCFinish());
                        break;
                    }
                case 2:
                    {
                        string name = ReadString(_data);
                        if (GC.MyChar.Silvers < 200000000)
                        {
                            GC.LocalMessage(2000, "You need 200kk");
                            return;
                        }
                        string path = @"C:\\OldCODB\\Users\\Characters\\" + name + ".chr";
                        string olpdath = @"C:\\OldCODB\\Users\\Characters\\" + GC.MyChar.Name + ".chr";
                        string newpath = @"C:\\OldCODB\\Users\\Characters\\" + GC.MyChar.Name + ".oldchr";
                        string newpath1 = @"C:\\OldCODB\\Users\\Characters\\" + name + ".chr";
                        string newpath2 = @"C:\\OldCODB\\Users\\Characters\\" + GC.MyChar.Name + ".oldchr1";

                        if (File.Exists(path))
                        {
                            GC.LocalMessage(2000, "This name is already used.");
                            return;
                        }
                        if (!GC.ValidName(name) || name.Length < 4 || name.Length > 12)
                        {
                            GC.LocalMessage(2000, "This name is not valid name.");
                            return;
                        }
                        if (File.Exists(newpath))
                        {
                            File.Move(newpath, newpath2);
                            GC.LocalMessage(2000, "Old backup was deleted.");

                        }
                        GC.MyChar.NewName = name;
                        GC.MyChar.Silvers -= 500000000;
                        try
                        {
                            for (int i = 0; i < 49; i++)
                            {
                                if (Game.World.EmpireBoard[i].Name == GC.MyChar.Name)
                                {
                                    Game.World.EmpireBoard[i].Name = GC.MyChar.NewName;
                                    break;
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        Database.SaveCharacter(GC.MyChar, GC.MyChar.MyClient.AuthInfo.Account);
                        File.Move(olpdath, newpath);
                        File.Copy(newpath, newpath1);
                        string newName = name, oldName = GC.MyChar.Name;
                        // World.NameAdd += "[Name Change] " + oldName + " has changed his name to " + newName + "\r\n";
                        MySQL.MySqlCommand Cmd2;
                        Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                        Cmd2.Update("characters").Set("Name", newName).Where("UID", GC.MyChar.EntityID).Execute();

                        MySQL.MySqlCommand changename = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        changename.Insert("changename").Insert("oldname", GC.MyChar.Name).Insert("newname", newName).Insert("date", DateTime.Now).Execute();

                        GC.Disconnect();
                        break;
                    }
            }
        }
    }
}