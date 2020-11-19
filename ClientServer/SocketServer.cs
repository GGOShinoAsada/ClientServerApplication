using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ClientServerApplication.ClientServer
{
    class SocketServer
        
    {
        public StringBuilder sb = new StringBuilder();
        private readonly Socket _listen;
        public ErrorsList ErrorsList;
        public SocketServer(int port)
        {
            ErrorsList = new ErrorsList();
            IPEndPoint listenpoint = new IPEndPoint(IPAddress.Loopback, port);
            _listen = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listen.Bind(listenpoint);
            _listen.Listen(1);

        }
        private async void _Accept(IAsyncResult rez)
        {
            try
            {
                using (Socket client = _listen.EndAccept(rez))
                using (NetworkStream stream = new NetworkStream(client))
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string data;
                    int size = 0;
                    sb.Append("Server response: \n");
                    while ((data = await reader.ReadLineAsync()) != null)
                    {
                        
                        sb.Append(Encoding.GetEncoding(866).GetBytes(data)+ "\n");
                        writer.WriteLine(data);
                        
                    }

                }
                _listen.BeginAccept(_Accept, null);
            }
            catch (ObjectDisposedException)
            {
                ErrorsList.IsServerCloled = true;
            }
            catch (SocketException ex)
            {
                ErrorsList.SocketExeption = true;
            }
        }
    }
}
