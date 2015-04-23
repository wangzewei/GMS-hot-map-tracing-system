using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerExample
{
    class serverRecieve:IViewFlush
    {
        #region 数据成员
        private TcpListener Listener;
        private Thread listenerThread;
        private delegate void writeMessageDel();
        public string IP;
        public int Port;
        public int countofConnected = 0;
        #endregion

        /// <summary>
        /// 默认方法时使用appsetting中的配置
        /// </summary>
        public serverRecieve() 
        {
            IP =  ConfigurationManager.AppSettings["Lo"].ToString();
            Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            this.Listener = new TcpListener(IPAddress.Parse(IP),Port);
            this.listenerThread = new Thread(new ThreadStart(listenerloop));
            this.listenerThread.Name = "BackGroundLoopThread";
            this.listenerThread.Start();
        }

        private void listenerloop() 
        {
            this.Listener.Start();
            while (true)
            {
                TcpClient connectors = this.Listener.AcceptTcpClient();
                countofConnected++;
                Console.WriteLine("there is {0} connectors dail in", countofConnected.ToString());
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleConnect));
                clientThread.Start(connectors);
            }

        }

        private void HandleConnect(object sender)
        {
            Console.WriteLine("Begin handle it");
            TcpClient client = (TcpClient)sender;
            NetworkStream clientStream = client.GetStream();

            byte[] buffer = new byte[4096];
            int bytestoRead;

            while(true)
            {
                bytestoRead = 0;

                try 
                {
                    bytestoRead = clientStream.Read(buffer,0,4096);
                }
                catch
                {
                    break;
                }

                if (0 == bytestoRead)
                {
                    countofConnected--;
                    // 更新链接数量到界面
                }

                string clientMessage = ASCIIEncoding.ASCII.GetString(buffer,0,4096);
                viewFlushmethod(clientMessage.Trim(),clientStream);
            }

            client.Close();            
        }

        public void viewFlushmethod(string Msg,NetworkStream ns) 
        {
            Console.WriteLine(Msg);
            ns.Write(UTF8Encoding.UTF8.GetBytes(Msg), 0, 4096);
            ns.Write(ASCIIEncoding.ASCII.GetBytes(Msg),0,4096);
            ns.Flush();
        }
    }
}
