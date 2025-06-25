using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Dbase
{
    public class Portal
    {
        public ushort PortalMapID { get; set; }
        public ushort PortalX { get; set; }
        public ushort PortalY { get; set; }
        public ushort DestinationMapID { get; set; }
        public ushort DestinationX { get; set; }
        public ushort DestinationY { get; set; }
    }
}
