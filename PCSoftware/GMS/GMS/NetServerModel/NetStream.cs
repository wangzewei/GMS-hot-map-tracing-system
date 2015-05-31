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
using System.Configuration;

namespace GMS
{
    public class NetStream
    {
        #region 数据成员
        private TcpListener tcpListener;
        private Thread listenThread;
        private int connectedClients = 0;
        private delegate void WriteMessageDelegate(string msg);
        private static NetStream instance = null;
        private static CmdQueue queue = CmdQueue.getinstance();
        private Commend c = null;
        string msg;
        private ASCIIEncoding encoder = new ASCIIEncoding();
        Main f1;
        #endregion

        #region 基本服务
        static public NetStream getinstance(Main f)
        {
            if (instance == null)
                instance = new NetStream(f);
            
            return instance;
        }

        NetStream(Main f)
        {
            f1 = f;
            string IP = ConfigurationManager.AppSettings["ServerAddr"].ToString();
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

                msg = encoder.GetString(message, 0, bytesRead);
                WriteMessage(msg);
                
                if(msg.Length!=0)
                    ThreadPool.QueueUserWorkItem(this.dowork);
                Echo(msg,encoder,clientStream);
            }

            tcpClient.Close();
        }

        private void dowork(object state)
        {
            
            c = CommendFactory.CommendGenerator(msg.Trim());
            if (c != null)
            {
               // c.GetType();
                c.Process();
                    //queue.AddCmd(c);
            }
        }

        #endregion

        private void WriteMessage(string msg)
        {
            if (f1.textBox2.InvokeRequired)
            {
                WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                f1.textBox2.Invoke(d, new object[] { msg });
            }
            else
            {
                f1.textBox2.AppendText(msg + Environment.NewLine);
            }
        }
        

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
