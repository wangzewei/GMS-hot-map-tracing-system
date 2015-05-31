using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GMS
{
    /**
     * create a list to menage the tobe-processed cmd
     * generate: by staruml
     * design: by DrZhang
     * date: 22/4/15
     */
    public class CmdQueue
    {
        //singleton ref

        private static readonly CmdQueue instance = new CmdQueue();
        Thread process = null;
        private object thislock=null;
        
        static public CmdQueue getinstance()
        {
            return instance;
        }

        private  CmdQueue()
        {
            LoC = new List<Commend>();
            LoCready = new List<Commend>();
            process = new Thread(new ThreadStart(Excute));
            thislock = new object();
        }

        #region 方法成员
        public bool AddCmdEmg(Commend cmd,int index=2)
        {
            lock (thislock)
            {
                LoC.Insert(index, cmd);
            }
            return CheckUp(cmd, LoC.Count - 1);
        }

        public bool AddCmd(Commend cmd)
        {
            lock (thislock)
            {
                LoC.Add(cmd);
            }
            return CheckUp(cmd,LoC.Count-1);
        }

        public bool Remove(int index)
        {
            Commend tmp = LoC[index];
            lock (thislock)
            {
                LoC.RemoveAt(index);
            }
            return LoC[index].Equals(tmp) ? false : true;
        }

        public void RemoveAll() 
        {
            lock(thislock)
            {
                LoC.Clear();
            }
        }

        private bool CheckUp(Commend _cmd, int index)
        {
            return LoC[index].Equals(_cmd);
        }

        public void Excute() 
        {
            lock(thislock)
            {
                foreach (Commend i in LoC)
                {
                    LoCready.Add(i);
                }
                LoC.Clear();
            }
            foreach (Commend i in LoCready)
            {
                i.Process();
            }
        }
        #endregion

        #region 数据成员

        private List<Commend> LoC;
        private List<Commend> LoCready;
        #endregion
    }

}