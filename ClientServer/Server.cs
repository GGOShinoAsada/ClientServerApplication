using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace ClientServerApplication.ClientServer
{
    public class Server
    {
        private readonly Socket socket;
        ErrorsList ErrorsList = new ErrorsList();
   
        public StringBuilder Output = new StringBuilder();
       
        public Server(int port)
        {
          
            IPEndPoint point = new IPEndPoint(IPAddress.Loopback, port);
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(point);
            socket.Listen(1);
            socket.BeginAccept(_Accept, null);
        }
        private async void _Accept (IAsyncResult result)
        {
            try
            {
                using (Socket client = socket.EndAccept(result))
                using (NetworkStream stream = new NetworkStream(client))
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string data;
                    while ((data = await reader.ReadLineAsync()) != null)
                    {
                        Output.Append(data);
                        writer.WriteLine(data);
                        writer.Flush();
                    }

                }
                socket.BeginAccept(_Accept, null);
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
        public void Stop()
        {
            socket.Close();
        }
    }
}
