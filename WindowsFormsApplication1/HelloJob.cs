using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Windows.Forms;
using Topshelf;
using Commmon;
using System.Data;
using log4net;
using Quartz.Impl;

namespace WindowsFormsApplication1
{
    public class HelloJob : IJob
    {
        //private readonly ILog _logger = LogManager.GetLogger(typeof(HelloJob));
        //_logger.InfoFormat("HelloJob测试");
        public void Execute(IJobExecutionContext context)
        {

            //MessageBox.Show("HelloJob");
            try
            {
                SendMsg.SendLis();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
