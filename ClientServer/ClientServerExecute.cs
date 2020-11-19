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
        private static Socket Client;
    
        public StringBuilder Output = new StringBuilder();
        private int _port=8080;
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
        private static int _size = 256;
       public static int Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value > 0)
                {
                    _size = value;
                }
             
            }
        }
       static byte[] Responce = new byte[Size];
       public static List<string> Errors = new List<string>();
       
       /// <summary>
       /// Listen client
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="host"></param>
       /// <param name="port"></param>
        public void Execute(string msg, string host="localhost",int port=8080, int n=256)
        {
            Errors = new List<string>();
            Port = port;
            Size = 256;
            Responce = new byte[Size];
         
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Loopback, Port);
        
            try
            {
                Client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                Client.Bind(point);
                Client.Listen(10);
                TcpClient tcl = new TcpClient();
                using (NetworkStream stream=tcl.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    Client.Connect(point);
                    Client.BeginAccept(_Accept, null);
                    Task task1 = Task.Factory.StartNew(() => _Recieve(reader));
                    task1.Wait();
                    
                }
            }
            
            catch (Exception ex)
            {
                Errors.Add(ex.Message);
            }
           // Errors = new List<string>();
           // Port = port;
           // Responce = new byte[256];
           // //IPHostEntry entry = Dns.GetHostEntry(host);
           // //IPAddress ip = entry.AddressList[0];
           // //IPEndPoint point = new IPEndPoint(ip, port);
           //IPEndPoint point = new IPEndPoint(IPAddress.Loopback, Port);
           
           // try
           // {
           //     Client = new Socket(SocketType.Stream, ProtocolType.Tcp);
           //     Client.Bind(point);
           //     Client.Listen(10);
            
           //     TcpClient cl = new TcpClient();
                
           //     using (NetworkStream stream = cl.GetStream())
           //     using (StreamReader reader = new StreamReader(stream))
           //     using (StreamWriter writer = new StreamWriter(stream))
           //     {
           //         //NetworkStream f = new NetworkStream(client);
           //         //f.
           //         Client.Connect(point);
           //         Client.Accept().Receive(Responce);
           //         Task task = Task.Factory.StartNew(() => _Recieve(reader));
           //         task.Wait();
           //         if (msg != "")
           //         {
           //             writer.WriteLine(msg);
           //             writer.Flush();
           //         }
           //         Client.Shutdown(SocketShutdown.Both);
           //         task.Wait();
           //     }
               
           //     //Client.Close();
           //     //Output.Append(server.sb);
           // }
           // catch (ObjectDisposedException)
           // {
           //     Errors.Add("Error: server closed");
               
           // }
           // catch (SocketException ex1)
           // {
           //   // ErrorsList.SocketExeption = true;
           //     Errors.Add("Socket exeption "+ex1.Message);
           // }
           // catch (IOException ex2)
           // {
           //     Errors.Add("Error: " + ex2.Message);
           // }
           // finally
           // {
                
           //     //Client.Disconnect(false);
           //     //Client.Disconnect();
                
           //     Client.Close();

           //     //Client.Dispose();
           // }
          
        }
        private async void _Accept(IAsyncResult rez)
        {

        }
        private void SocketClosing (IAsyncResult ar)
        {
            Socket s = (Socket)ar;
            s.Close();
        }
        private async Task _Recieve(StreamReader reader)
        {

            string text = "";
            Output.Append("Client requered:\n");
            while ((text = await reader.ReadLineAsync()) != null)
            {
               Output.Append(text+ "\n");
            }
        }
        public bool Validate()
        {
            return Errors.Count == 0;
        }
    }
    
    
}
