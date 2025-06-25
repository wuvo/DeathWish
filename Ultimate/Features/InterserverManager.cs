using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewestCOServer.Main.Sockets;
using System.Timers;

namespace NewestCOServer.Features
{
    public static class InterserverManager
    {
        //These are not the real IPs, replace them with what I send you once I'm done testing.
        public static string NA_Server = "na.conqueronline.info";
        public static System.Net.IPAddress[] NAIPs = System.Net.Dns.GetHostAddresses(NA_Server);

        public static string EU_Server = "eu.conqueronline.info";
        public static System.Net.IPAddress[] EUIPs = System.Net.Dns.GetHostAddresses(EU_Server);

        public static string INTERSERVER_AMERICA = NAIPs.FirstOrDefault().ToString();   
        public static string INTERSERVER_EUROPE = EUIPs.FirstOrDefault().ToString();
        public static bool SEND_NA = true;//Make a command or something to toggle this. its which server people will be forced to by default.
        //I also suggest making a NPC which lets people choose which they want (keep in mind that war only runs on one though, thats up to you to manage)
        public static InterserverClient AMERICA;
        public static InterserverClient EUROPE;

        static InterserverManager()
        {
           var _timer = new Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(AMERICA == null || !AMERICA.Connected)
            AMERICA = new InterserverClient(INTERSERVER_AMERICA, 5818);
            if (EUROPE == null || !EUROPE.Connected)
                EUROPE = new InterserverClient(INTERSERVER_EUROPE, 5818);
        }        

        public static bool Send(byte[] data, bool isNA = true)
        {
            var server = isNA ? AMERICA : EUROPE;
            if (server == null ||!server.Connected)
                return false;
            server.Send(data);
            return true;
        }
    }
}
