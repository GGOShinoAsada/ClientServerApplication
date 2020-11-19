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
    public class Client
    {
        public StringBuilder Output = new StringBuilder();
        private int _port = 8080;
        public int Port
        {
            set
            {
                if (value > 0)
                {
                    _port = value;
                }
            }
            get
            {
                return _port;
            }
        }
        public Client(int port, string msg)
        {
            Port = port;
            Server server = new Server(Port);
            Socket remote = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Loopback, Port);
            remote.Connect(point);
            using (NetworkStream stream = new NetworkStream(remote))
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                Task task1 = Task.Factory.StartNew(() => _Recieve(reader));
                //task1.Start();

                if (msg != "")
                {
                    writer.WriteLine(msg);
                    writer.Flush();
                }
                remote.Shutdown(SocketShutdown.Send);
                task1.Wait();
               
            }
            server.Stop();
        }
        private async Task _Recieve(StreamReader reader)
        {

            string text = "";
            while ((text = await reader.ReadLineAsync()) != null)
            {
                Output.Append(text);

            }

        }
    }
}
