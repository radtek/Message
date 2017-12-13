using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quartz;
using Quartz.Impl;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private IScheduler _myScheduler = null;
        public IScheduler MyScheduler
        {
            get
            {
                if (_myScheduler == null)
                {
                    var schFactory = new StdSchedulerFactory();
                    _myScheduler = schFactory.GetScheduler();
                }
                return _myScheduler;
            }
        }
        public Form1()
        {
            MyScheduler.Start();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MyScheduler.IsStarted)
            {
                MyScheduler.Shutdown(true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
