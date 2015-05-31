using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace GMS
{
    class L:Commend 
    {
        public override bool Process() 
        {
            MessageBox.Show(base.cmdmsg[1], base.cmdmsg[2]);
            return true; 
        }
    }
}
