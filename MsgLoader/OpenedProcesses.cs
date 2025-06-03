using System;
//using ZFShield;
using System.IO;
using System.Linq;
using DeathWish.ServerSockets;
using DeathWish.Game.MsgLoader;
using TqShield;

namespace DeathWish.Game.MsgServer
{
    public class OpenedProcesses
    {


        [PacketAttribute((ushort)TqShield.Types.PacketTypes.CMsgOpenedProcesses)]
        public unsafe static void Process(Client.GameClient pClient, Packet stream)
        {
            pClient.ProCipher.Decrypt(stream);
            stream.Seek(0);
            byte[] pBuffer = stream.ReadBytes(stream.Size);

            BinaryReader BR = new BinaryReader(new MemoryStream(pBuffer));
            BR.BaseStream.Seek(4, SeekOrigin.Current);

            TqShield.Types.ProcessesType ActionType = (TqShield.Types.ProcessesType)BR.ReadInt16();

            if (ActionType == TqShield.Types.ProcessesType.Start)//Clean
            {
                pClient.OpenedProcesses.Clear();
                //Console.WriteLine("Someone requested " + pClient.Player.Name + "'s opened processes, Processing...");
            }
            else if (ActionType == TqShield.Types.ProcessesType.Insert) // Insert
            {
                Int16 ProgramLen = BR.ReadInt16();
                String Proc = System.Text.Encoding.Default.GetString(BR.ReadBytes(ProgramLen)).Replace("\0", "");
                if (!pClient.OpenedProcesses.Contains(Proc))
                {
                    pClient.OpenedProcesses.Add(Proc);
                }
            }
            else if (ActionType == TqShield.Types.ProcessesType.Finish)//Finish
            {
                //Console.WriteLine(pClient.Player.Name + "'s " + pClient.OpenedProcesses.Count + " opened processes have been received successfully, Check log files.");
                string Text = "==========================================================" + Environment.NewLine;
                Text += "Request Time: " + DateTime.Now.ToString("dddd, dd MMMM yyyy [HH:mm:ss]") + Environment.NewLine;
                var Bg = pClient.OpenedProcesses.Where(i => i.StartsWith("B"));
                var Window = pClient.OpenedProcesses.Where(i => i.StartsWith("W"));
                Text += "Visible Processes Instances Count: " + Window.Count() + Environment.NewLine + Environment.NewLine;

                foreach (var Line in Window)
                {
                    var Split = Line.Split('|');
                    try
                    {
                        if (Split[1] == "Start") return;
                        foreach (var Item in TqShield._TqShield.BlockAppsList)
                        {
                            if (Split[1].ToLower().Contains(Item))
                            {
                                CheatPacket.Detected(pClient, Split[1]);
                            }
                        }
                        Text += "---------------" + Environment.NewLine;
                        Text += "Name: '" + Split[1] + "'" + Environment.NewLine;
                    }
                    catch { }
                }

                Text += Environment.NewLine;
                Text += Environment.NewLine;
                Text += "Background Processes Instances Count: " + Bg.Count() + Environment.NewLine + Environment.NewLine;
                foreach (var Line in Bg)
                {
                    var Split = Line.Split('|');
                    try
                    {
                        if (Split[1] == "Start") return;
                        foreach (var Item in TqShield._TqShield.BlockAppsList)
                        {
                            if (Split[1].ToLower().Contains(Item))
                            {
                                CheatPacket.Detected(pClient, Split[1]);
                            }
                        }
                        Text += "---------------" + Environment.NewLine;
                        Text += "Name: '" + Split[1] + "'" + Environment.NewLine;
                    }
                    catch { }
                }
                Text += "==========================================================" + Environment.NewLine + Environment.NewLine;
                if (!System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\LoaderLogg\\"))
                    System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\LoaderLogg\\");
                string path = System.Windows.Forms.Application.StartupPath + "\\LoaderLogg\\[Processes]\\";
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                path += pClient.Player.Name.RemoveIllegalCharacters(false, true) + ".txt";
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.WriteAllLines(path, new string[0]);
                }
                using (var SW = System.IO.File.AppendText(path))
                {
                    SW.WriteLine(Text);
                    SW.Close();
                }
            }
            BR.Close();
        }
    }
}








//using System;
//using TqShield;
//using System.IO;
//using System.Linq;
//using Pharaoh.ServerSockets;
//using Pharaoh.Game.MsgLoader;

//namespace Pharaoh.Game.MsgServer
//{
//    public class OpenedProcesses
//    {
//        [Packet((ushort)_TqShield.PacketTypes.CMsgOpenedProcesses)]
//        public static void Process(Client.GameClient pClient, Packet stream)
//        {
//            pClient.ProCipher.Decrypt(stream);

//            stream.Seek(4);
//            byte[] pBuffer = stream.ReadBytes(stream.Size);
//            using (BinaryReader BR = new BinaryReader(new MemoryStream(pBuffer)))
//            {
//                _TqShield.ProcessesType ActionType = (_TqShield.ProcessesType)BR.ReadInt16();

//                if (ActionType == _TqShield.ProcessesType.Start)//Clean
//                {
//                    pClient.OpenedProcesses.Clear();
//                }
//                else if (ActionType == _TqShield.ProcessesType.Insert) // Insert
//                {
//                    Int16 ProgramLen = BR.ReadInt16();
//                    String Proc = System.Text.Encoding.Default.GetString(BR.ReadBytes(ProgramLen)).Replace("\0", "");
//                    if (!pClient.OpenedProcesses.Contains(Proc))
//                    {
//                        pClient.OpenedProcesses.Add(Proc);
//                    }
//                }
//                else if (ActionType == _TqShield.ProcessesType.Finish)//Finish
//                {
//                    var Bg = pClient.OpenedProcesses.Where(i => i.StartsWith("B"));
//                    var Window = pClient.OpenedProcesses.Where(i => i.StartsWith("W"));

//                    foreach (var Line in Window)
//                    {
//                        var Split = Line.Split('|');
//                        try
//                        {
//                            for (int i = 0; i < Split.Length; i++)
//                            {
//                                if (_TqShield.BlockAppsList.Contains(Split[i].ToLower()))
//                                {
//                                    CheatPacket.Detected(pClient, Split[1]);
//                                    break;
//                                }
//                            }
//                        }
//                        catch { }
//                    }
//                    foreach (var Line in Bg)
//                    {
//                        var Split = Line.Split('|');
//                        try
//                        {
//                            for (int i = 0; i < Split.Length; i++)
//                            {
//                                if (_TqShield.BlockAppsList.Contains(Split[i].ToLower()))
//                                {
//                                    CheatPacket.Detected(pClient, Split[1]);
//                                    break;
//                                }
//                            }
//                        }
//                        catch { }
//                    }
//                }
//                BR.Close();
//            }
//        }
//    }
//}