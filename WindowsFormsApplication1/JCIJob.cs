using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Windows.Forms;
using Topshelf;
using Commmon;
using System.Data;

namespace WindowsFormsApplication1
{
    public class JCIJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //MessageBox.Show("JCIJob");

            SendJCI.Send();

        }
    }
}
