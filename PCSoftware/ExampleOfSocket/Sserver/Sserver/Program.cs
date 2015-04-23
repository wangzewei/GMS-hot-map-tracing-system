using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sserver
{
    class Program
    {
        const string IP = "172.17.133.10";
        const int PORT = 7200;
        Socket serverSocket, clientSocket;

        static void Main(string[] args)
        {
            string serverInfo = this.Startup();
            Console.WriteLine("Server started at:" + serverInfo);

            serverInfo = server.Listen();
            Console.WriteLine(serverInfo);

            string datatosend = Console.ReadLine();
            server.SendData(datatosend);

            serverInfo = server.ReceiveData();
            Console.WriteLine(serverInfo);

            Console.ReadLine();
            
        }
        public static bool SendData(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return false;
            if (string.Equals("exit", msg.Trim().ToLower()))
                return false;

            var clientEP = new IPEndPoint(IPAddress.Parse(IP),PORT);
            clientSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
            clientSocket.Connect(clientEP);
            byte[] buffer = UTF8Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(buffer);
            clientSocket.Close();

            return true;
        }
        public static string reciData(string msg)
        {
            byte[] buffer = new byte[256];
            Socket recivSocket = serverSocket.Accept();
            var bytetoread = recivSocket.Receive(buffer);
            recivSocket.Close();
            return UTF8Encoding.UTF8.GetString(buffer);
        }

        public static string startup() 
        {
            var ServerEP = new IPEndPoint(IPAddress.Parse(IP), PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ServerEP);
            return serverSocket.LocalEndPoint.ToString();
        }

        public static string ListenLoop() 
        {
            int backlog = 0;
            try 
            {
                serverSocket.Listen(backlog);
                return "Server working";
            }
            catch(Exception ex)
            {
                return "Failed to listen" + ex.ToString();
            }
        }
    }
}
