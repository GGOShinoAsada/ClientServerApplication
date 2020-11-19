using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace ClientServerApplication.ClientServer
{
    class Client
    {
        private int _port = 8080;
        # region all service
        public List<string> AllErrors = new List<string>();
        public StringBuilder AllOutput = new StringBuilder();
        #endregion
        #region server parameters
        private List<string> ServerErrors = new List<string>();
        private StringBuilder ServerOutput = new StringBuilder();
        #endregion
        #region client parameters
        public List<string> ClientErrors = new List<string>();
        public StringBuilder ClientOutput = new StringBuilder();
        private string _message;
        public string Message
        {
            set
            {
                if (value.Length >0)
                {
                    _message = value;
                }
            }
            get
            {
                return _message;
            }
        }
        #endregion
        public int Port
        {
            set
            {
                if (value > 0)
                    _port = value;
            }
            get
            {
                return _port;
            }
        }
        public void Connect(string msg, int port=8080)
        {
            Port = port;
            Message = msg;
            Server server = new Server(Port);
            ServerOutput = server.Output;
            ServerErrors = server.ServerErrors;
            Socket remote = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remotepoint = new IPEndPoint(IPAddress.Loopback, Port);
            remote.Connect(remotepoint);
            using (NetworkStream stream = new NetworkStream(remote))
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                Task recievetask = _Recieve(reader);
                
                if (Message.Length > 0)
                {
                    List<string> data = Message.Split('\n').Cast<string>().ToList();
                    foreach (string t in data)
                    {
                        writer.WriteLine(t);
                        writer.Flush();
                    }
                    remote.Shutdown(SocketShutdown.Send);
                    recievetask.Wait(1000);
                }
                AllErrors = (List<string>)ClientErrors.Concat(ServerErrors);

            }

        }
        private async Task _Recieve(StreamReader reader)
        {
            string text;
            while ((text = await reader.ReadLineAsync()) != null)
            {
                ClientOutput.Append($"CLIENT received: {text}");

            }
        }
        public bool Validate()
        {
            return AllErrors.Count == 0;
        }

    }
}
