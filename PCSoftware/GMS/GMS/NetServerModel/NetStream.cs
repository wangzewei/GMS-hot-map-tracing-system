using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GMS
{
    public class NetStream
    {
        #region 数据成员
        private TcpListener tcpListener;
        private Thread listenThread;
        private int connectedClients = 0;
        private delegate void WriteMessageDelegate(string msg);
        static readonly private NetStream instance = new NetStream();
        #endregion

        #region 基本服务
        public NetStream getinstance()
        {
            return instance;
        }

        public NetStream()
        {
            string IP = System.Configuration.ConfigurationManager.AppSettings["ServerAddr"].ToString();
            string PORT = System.Configuration.ConfigurationManager.AppSettings["ServerPort"].ToString();
            this.tcpListener = new TcpListener(IPAddress.Parse(IP), int.Parse(PORT));
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();

                connectedClients++; 
                //lblNumberOfConnections.Text = connectedClients.ToString();

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    connectedClients--;
                    //lblNumberOfConnections.Text = connectedClients.ToString();
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();

                string msg = encoder.GetString(message, 0, bytesRead);
                //WriteMessage(msg);
                
                // using cmd factory to create subclass cmd and add it to queue
                CommendFactory.CommendGenerator(msg);

                Echo(msg, encoder, clientStream);
            }

            tcpClient.Close();
        }
        #endregion

        //private void WriteMessage(string msg)
        //{
        //    if (this.rtbServer.InvokeRequired)
        //    {
        //        WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
        //        this.rtbServer.Invoke(d, new object[] { msg });
        //    }
        //    else
        //    {
        //        this.rtbServer.AppendText(msg + Environment.NewLine);
        //    }
        //}

        #region 特殊服务
        private void Echo(string msg, ASCIIEncoding encoder, NetworkStream clientStream)
        {
            // Now Echo the message back
            byte[] buffer = encoder.GetBytes(msg);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
        #endregion
    }
}
