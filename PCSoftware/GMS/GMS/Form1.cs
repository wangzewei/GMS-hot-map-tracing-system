using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Commend c1 = new cmdtest("a,B,C");
            //c1.Process();
            //NetStream n1 = new NetStream();
            //n1.Server();
        }
    }
}
