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
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //private IScheduler _myScheduler = null;
        //public IScheduler MyScheduler
        //{
        //    get
        //    {
        //        if (_myScheduler == null)
        //        {
        //            var schFactory = new StdSchedulerFactory();
        //            _myScheduler = schFactory.GetScheduler();
        //        }
        //        return _myScheduler;
        //    }
        //}
        public Form1()
        {
            //MyScheduler.Start();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            //log.Info("定时任务启动");
            //ReserveJob.Send();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (MyScheduler.IsStarted)
            //{
            //    MyScheduler.Shutdown(true);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMsg.SendLis();
            //label1.Text = getBirthDay("0012428559");
        }
        private static string getBirthDay(string empid)
        {
            WebReference.Service1 _client = new WebReference.Service1();
            string s = _client.GetPatiInfoByCardID(empid);
            XElement root = XElement.Parse(@"" + s + "");
            string DDBH = root.Element("Nuerse").Element("BirthDate").Value;
            if (!String.IsNullOrEmpty(DDBH) && DDBH.Length > 10)
            {
                return DDBH.Substring(0, 10);
            }
            return "";
        }
    }
}
