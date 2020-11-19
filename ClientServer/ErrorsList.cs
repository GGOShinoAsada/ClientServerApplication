using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApplication.ClientServer
{
    public struct ErrorsList
    {
        public bool IsServerCloled {get; set;}
        public bool SocketExeption { get; set; }
        public bool SocketClosed { get; set; }

    }
}
