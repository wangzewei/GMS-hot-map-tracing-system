using System;
using System.Collections.Generic;
using System.Text;

namespace GMS
{
    abstract public class Commend 
    {
        public Commend(){}
        public abstract bool Process();
        public void appendcontext(string[] contex)
        {
            cmdmsg.AddRange(contex);
        }
        public List<string> cmdmsg= new List<string>();
    }
}
