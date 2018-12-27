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
    public class HelloJob2 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //MessageBox.Show("第二个任务");
            //温湿度报警定时任务
            try
            {
                SendMsg.SendJob();
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
