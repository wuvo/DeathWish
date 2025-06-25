using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewestCOServer.Main.Sockets
{
        public delegate void ProxyClientConnection();
        public delegate void ProxyClientReceive(byte[] buffer);    
}
