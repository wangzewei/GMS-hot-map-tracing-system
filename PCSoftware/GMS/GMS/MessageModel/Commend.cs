using System;
using System.Collections.Generic;
using System.Text;

namespace GMS
{
    abstract public class Commend :IProcess
    {
        public Commend(string MsgContext);
        public bool Process();
        protected string[] cmdmsg;
    }
}
