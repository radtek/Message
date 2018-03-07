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
using Topshelf;
using Commmon;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<HelloJob>();
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("Stuff");                       //9
            }); 
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int second =Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Second"]);
            //从工厂中获取一个调度器实例化
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();       //开启调度器

            //==========例子1（简单使用）===========

            IJobDetail job1 = JobBuilder.Create<HelloJob>()  //创建一个作业
                .WithIdentity("作业名称", "作业组")
                .Build();

            ITrigger trigger1 = TriggerBuilder.Create()
                                        .WithIdentity("触发器名称", "触发器组")
                                        .StartNow()                        //现在开始
                                        .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                            .WithIntervalInSeconds(second)
                                            .RepeatForever())              //不间断重复执行
                                        .Build();


            scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            DateTime beginTime = Convert.ToDateTime(bll.getSentSms());
            DateTime endTime = beginTime.AddMinutes(5); 
            bool s = bll.ExistMinute("13951190130", "2018-03-01 17:23:55", "2018-03-01 17:28:55");
            if (s == true)
            {
                //Model.BIF01022 model = new Model.BIF01022();
                //model.Patient_id = "1";
                //model.Item_name = "3";
                //model.Current_result = "4";
                //model.EmpMobileNum = "13951190130";
                //model.State = 1;
                //if (bll2.Update(model))
                //{
                //    label1.Text = "更新成功";
                //}

                string Patient_id = "1";
                string Item_name = "3";
                string Current_result = "4";
                string EmpMobileNum = "13951190130";
                if (bll2.Exists(EmpMobileNum, Item_name, Current_result, Patient_id,1))
                {
                    label1.Text = "下次不用发了";
                }

            }
            //Model.BIF01022 model = new Model.BIF01022();
            //model.Patient_id = "12";
            //model.Patient_name="张三";
            //model.Item_name = "项目";
            //model.Current_result = "99";
            //model.EmpMobileNum = "15261277153";
            //model.EMPNAME = "医生";
            //model.State = 0;
            //model.Add_time = "2018-03-05";
            //if (bll2.Add(model))
            //{
            //    label1.Text = "success";
            //}
            //WebReference.Service1 _client = new WebReference.Service1();
            //DataSet ds_report = _client.GetReport();
            //if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            //{
            //    int i = SendMsg.Send();
            //    if (i > 0)
            //    {
            //        label1.Text = i+"条发送成功！";
            //    }
            //    else
            //    {
            //        label1.Text = "发送失败！";
            //    }
            //}
            //else
            //{
            //    label1.Text = "暂无数据";
            //}

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }


        
    }
}
