using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
namespace GMS
{
    /**
     * Abstract commend for each request during runtime
     * generate: by staruml
     * design: by DrZhang
     * date: 22/4/15
     * last modify 23/4/15
     */
    static public class CommendFactory
    {
        static public void CommendGenerator(string rawMsg)
        {
            string [] cmdmsg = rawMsg.Split(',');
            string Source = cmdmsg[1];
            string Destination = cmdmsg[2];
            string Type = System.Configuration.ConfigurationManager.AppSettings[cmdmsg[0]];
            string assemblyname = "GMS";
            string classname = assemblyname + "." + Type;

            Commend CMD =(Commend) Assembly.Load(assemblyname).CreateInstance(classname);
            CmdQueue queue= CmdQueue.getinstance();
            queue.AddCmd(CMD);

        }
    }
}