﻿using System;
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
    public class HelloJob4 : IJob, ServiceControl
    {
        public void Execute(IJobExecutionContext context)
        {
            //MessageBox.Show("第四个任务");

            //SendJCI.SendDoor();

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
