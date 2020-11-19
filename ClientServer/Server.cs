using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClientServerApplication.ClientServer
{
    class Server:IDisposable
    {
        public StringBuilder Output = new StringBuilder();
        public List<string> ServerErrors = new List<string>();
        private readonly Socket listen;
        public Server(int port)
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Loopback, port);
            listen = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listen.Bind(point);
            listen.Listen(1);
            listen.BeginAccept(_Accept, null);
        }
        public void Stop()
        {
            listen.Close();
        }
        private async void _Accept(IAsyncResult result)
        {
            try
            {
                using (Socket client = listen.EndAccept(result))
                using (NetworkStream stream = new NetworkStream(client))
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    Output.Append("SERVER: accepted new clien");
                    string text;
                    while ((text = await reader.ReadLineAsync()) != null)
                    {
                        Output.Append($"SERVER received: {text}");
                        writer.WriteLine(text);
                        writer.Flush();
                    }
                }
                listen.BeginAccept(_Accept, null);
            }
            catch (ObjectDisposedException ex)
            {
                ServerErrors.Add("ERROR: server was closed: " + ex.Message);
            }
            catch (SocketException ex2)
            {
                ServerErrors.Add("ERROR: other socket error" + ex2.Message);
            }
        }
       
        public void Restart()
        {
            ServerErrors = new List<string>();
            Output = new StringBuilder();
        }
        public void Dispose()
        {
            ServerErrors.Clear();
            Output.Clear();
        }
    }
}
