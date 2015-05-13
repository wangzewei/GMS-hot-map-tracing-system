using System;
using System.Collections.Generic;
using System.Text;

namespace GMS
{
    public class cmdtest :  Commend
    {      

        public new bool Process()
        {
            System.Windows.Forms.MessageBox.Show(string.Format("{1}",cmdmsg));
            return true;
        }
    }
}
