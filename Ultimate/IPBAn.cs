using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate
{
    public class IPBan
    {
        public static List<string> BannedIPs = new List<string>();
        public static void Load()
        {
            try
            {
                using (var reader = new StreamReader(@"C:\OldCODB\BannedIPs.txt"))
                {

                    string[] lines = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i] != "")
                            if (!BannedIPs.Contains(lines[i]))
                                BannedIPs.Add(lines[i]);
                }
            }
            catch
            {
            }
        }
        public static void Save()
        {
            File.Delete(@"C:\OldCODB\BannedIPs.txt");
            using (var writer = new StreamWriter(@"C:\OldCODB\BannedIPs.txt"))
            {
                foreach (var item in BannedIPs)
                    writer.WriteLine(item);
                writer.Close();
            }
        }
        public static void BanIP(string IP)
        {
            if (!BannedIPs.Contains(IP))
                BannedIPs.Add(IP);
        }
    }
}
