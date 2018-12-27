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
    public class HelloJob3 : IJob, ServiceControl
    {
        public void Execute(IJobExecutionContext context)
        {
            //MessageBox.Show("第三个任务");

            try
            {
                //SendMsg.SendPACSValue();
            }
            catch (Exception)
            {

                throw;
            }
            

        }



        public bool Start(HostControl hostControl)
        {
            throw new NotImplementedException();
        }

        public bool Stop(HostControl hostControl)
        {
            throw new NotImplementedException();
        }
    }
}
