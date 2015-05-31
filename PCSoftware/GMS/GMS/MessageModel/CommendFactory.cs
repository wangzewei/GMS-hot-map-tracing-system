using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Net.Sockets;
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
        static public Commend CommendGenerator(string rawMsg)
        {
            string [] cmdmsg;
            string Source ;
            string Destination;
            string Type=null;
            if (rawMsg.Length != 0)
                cmdmsg = rawMsg.Split(',');
            else
                return null;
            if (cmdmsg.Length > 3)
            {
                Type = System.Configuration.ConfigurationManager.AppSettings[cmdmsg[0]];
                Source = cmdmsg[1];
                Destination = cmdmsg[2];
            }
            if (Type == null)
                return null;
            string assemblyname = "GMS";
            string classname = assemblyname + "." + Type;
            Commend CMD =(Commend) Assembly.Load(assemblyname).CreateInstance(classname);
            CMD.appendcontext(cmdmsg);
            return CMD;
        }
    }
}