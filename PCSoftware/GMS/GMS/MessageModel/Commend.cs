using System;
using System.Collections.Generic;
using System.Text;

namespace GMS
{
    abstract public class Commend :IProcess
    {
        public Commend(){}
        public bool Process() { return false; }
        public void appendcontext(string[] contex)
        {
            cmdmsg.AddRange(contex);
        }
        public List<string> cmdmsg;
    }
}
