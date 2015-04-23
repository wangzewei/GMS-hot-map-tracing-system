using System;
using System.Collections.Generic;
using System.Text;

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

        static public CmdQueue getinstance()
        {
            return instance;
        }

        private  CmdQueue()
        {
            LoC = new List<Commend>();
        }

        #region 方法成员
        public bool AddCmdEmg(Commend cmd,int index=2)
        {
            LoC.Insert(index,cmd);
            return CheckUp(cmd, LoC.Count - 1);
        }

        public bool AddCmd(Commend cmd)
        {
            LoC.Add(cmd);
            return CheckUp(cmd,LoC.Count-1);
        }

        public bool Remove(int index)
        {
            Commend tmp = LoC[index];
            LoC.RemoveAt(index);
            return LoC[index].Equals(tmp)?false:true;
        }

        public void RemoveAll() 
        {
            LoC.Clear();
        }

        private bool CheckUp(Commend _cmd, int index)
        {
            return LoC[index].Equals(_cmd);
        }

        public void Excute() 
        {
            foreach (Commend i in LoC)
            {
                i.Process();
            }
        }
        #endregion

        #region 数据成员

        private List<Commend> LoC;

        #endregion
    }

}