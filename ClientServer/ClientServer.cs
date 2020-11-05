using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientServerApplication.ClientServer
{
    public class ClientServer  
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public Socket WorkSocket = null;
        

        private static int _Size;
        public int Size
        {
            set
            {
                if (value > 0)
                {
                    _Size = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            get
            {
                return _Size;
            }
        }
        private string _Msg;
        public string Message { get 
            {
                return _Msg;
            }
            set
            {
                if (value.Length > 0)
                {
                    _Msg = value;
                }
                else
                {
                    IsValid = false;
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
        public byte[] Buffer = new byte[_Size];
        private static bool IsValid = true;
        
      

        /// <summary>
        /// this is sync method
        /// </summary>
        /// <returns></returns>
        //public static async Task<String> SendMessageFromSocket(string msg,int port=8080, string host="localhost")
        //{
        //    byte[] bytes = new byte[Size];
            
        //    IPHostEntry entry = Dns.GetHostEntry(host);
        //    IPAddress ip = entry.AddressList[0];
        //    IPEndPoint point = new IPEndPoint(ip, port);
        //    Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //    socket.Connect(point);
        //    SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        //    //set buffer
        //    e.SetBuffer(bytes, 0, bytes.Length);

        //    //int bytesend = await socket.SendAsync(Encoding.GetEncoding(865).GetBytes(msg));
        //    //int byterec= 
        //    return "";
        //}
        public static bool Validate()
        {
            return IsValid;
        }
        public static void StartClient(string msg, int port = 8080, string host = "localhost")
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(host);
                IPAddress ip = entry.AddressList[0];
                IPEndPoint point = new IPEndPoint(ip, port);
                Socket client = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(point, new AsyncCallback(ConnectCallback),client);
                connectDone.WaitOne();
                byte[] data = Encoding.ASCII.GetBytes(msg);
                 #region send message
                client.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallBack), client);
                connectDone.WaitOne();
                

                #endregion
                #region get message
                ClientServer cs = new ClientServer();
                cs.WorkSocket = client;
                client.BeginReceive(cs.Buffer,0, cs.Size, 0, new AsyncCallback(reciew)
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            connectDone.Set();
        }
        private static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesend = client.EndSend(ar);
                sendDone.Set();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
        private static void RecieveCallBack(IAsyncResult ar)
        {
            ClientServer cs = (ClientServer)ar.AsyncState;
            Socket client = cs.WorkSocket;
            int bytesread = client.EndReceive(ar);
            if (bytesread > 0)
            {
                cs.Message = Encoding.ASCII.GetString(cs.Buffer, 0, bytesread);
                client.BeginReceive
            }

        }
    }
   
}
