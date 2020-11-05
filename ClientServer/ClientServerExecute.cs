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
    class ClientServerExecute
    {
        private readonly Socket _listsockets;

        private int _port=8080;
        public StringBuilder Output = new StringBuilder();
       
        ErrorsList ErrorsList = new ErrorsList();
       
        public int Port
        {
            set
            {
                if (value > 0)
                {
                    _port = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            get
            {
                return _port;
            }
        }
        public ClientServerExecute()
        {
            IPEndPoint listenpoint = new IPEndPoint(IPAddress.Loopback, Port);
            ErrorsList.SocketExeption = false;
            ErrorsList.IsServerCloled = false;
            _listsockets = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listsockets.Bind(listenpoint);
            _listsockets.Listen(1);
            _listsockets.BeginAccept(_Accept, null);
            ErrorsList.IsServerCloled = false;
        }
        private async void _Accept(IAsyncResult rez)
        {
            try
            {
                using (Socket client = _listsockets.EndAccept(rez))
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
                _listsockets.BeginAccept(_Accept, null);
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
        public async Task StartServiceAsync(int port, string msg)
        {
            Port = port;
            //Task.Delay(100);
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Loopback, Port);
            socket.Connect(point);
            using (NetworkStream stream = new NetworkStream(socket))
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                _ = Task.Delay(100);
                Task task1 = Task.Factory.StartNew(() => _Recieve(reader));
                   // _Recieve(reader);
                if (msg != "")
                {
                    writer.WriteLine(msg);
                    writer.Flush();
                }
                socket.Shutdown(SocketShutdown.Send);
                task1.Wait();
            }
            _listsockets.Close();
          
        }
        private async Task _Recieve(StreamReader reader)
        {

            string text = "";
            while ((text = await reader.ReadLineAsync()) != null)
            {
                Output.Append(text);
                
            }
          //  return text;
        }
    }
    
    
}
