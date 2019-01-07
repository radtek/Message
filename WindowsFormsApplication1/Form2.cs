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
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            int second = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Second"]);
            int second2 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Second2"]);
            int second3 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Second3"]);
            int second4 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Second4"]);
            //从工厂中获取一个调度器实例化
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.Start();       //开启调度器

            //推送Lis危急值
            #region 推送Lis危急值
            IJobDetail job1 = JobBuilder.Create<HelloJob>()
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
            #endregion

            //推送冷链
            #region 推送冷链
            IJobDetail job2 = JobBuilder.Create<HelloJob2>()  //创建一个作业
                .WithIdentity("作业名称2", "作业组2")
                .Build();
            ITrigger trigger2 = TriggerBuilder.Create()
                                        .WithIdentity("触发器名称2", "触发器组2")
                                        .StartNow()
                                        .WithSimpleSchedule(x => x
                                            .WithIntervalInSeconds(second2)
                                            .RepeatForever())
                                        .Build();
            scheduler.ScheduleJob(job2, trigger2);
            #endregion

            //推送pacs-【暂停】
            #region 推送pacs
            //IJobDetail job3 = JobBuilder.Create<HelloJob3>()  //创建一个作业
            //    .WithIdentity("作业名称3", "作业组3")
            //    .Build();
            //ITrigger trigger3 = TriggerBuilder.Create()
            //                            .WithIdentity("触发器名称3", "触发器组3")
            //                            .StartNow()
            //                            .WithSimpleSchedule(x => x
            //                                .WithIntervalInSeconds(second3)
            //                                .RepeatForever())
            //                            .Build();
            //scheduler.ScheduleJob(job3, trigger3);
            #endregion

            //推送JCI文件
            #region 推送JCI文件
            IJobDetail job4 = JobBuilder.Create<JCIJob>()  //创建一个作业
                .WithIdentity("作业名称4", "作业组4")
                .Build();
            //ITrigger trigger4 = TriggerBuilder.Create()
            //                            .WithIdentity("触发器名称4", "触发器组4")
            //                            .StartNow()
            //                            .WithSimpleSchedule(x => x
            //                                .WithIntervalInHours(24)
            //                                .RepeatForever())
            //                            .Build();
            ITrigger trigger4 = TriggerBuilder.Create()
                                        .WithIdentity("触发器名称4", "触发器组4")
                                        .WithCronSchedule("0 15 8 * * ?")
                                        .Build();
            scheduler.ScheduleJob(job4, trigger4);
            #endregion

            //推送门禁-【暂停】
            #region 推送门禁
            //IJobDetail jboDoor = JobBuilder.Create<HelloJob4>()  //创建一个作业
            //    .WithIdentity("作业名称door", "作业组door")
            //    .Build();
            //ITrigger tgDoor = TriggerBuilder.Create()
            //                            .WithIdentity("触发器名称tgDoor", "触发器组tgDoor")
            //                            .StartNow()
            //                            .WithSimpleSchedule(x => x
            //                                .WithIntervalInSeconds(second4)
            //                                .RepeatForever())
            //                            .Build();
            //scheduler.ScheduleJob(jboDoor, tgDoor);
            #endregion

            //推送营养会诊
            #region 推送营养会诊
            IJobDetail jobMetting = JobBuilder.Create<MettingJob>()  //创建一个作业
                .WithIdentity("作业名称Metting", "作业组Metting")
                .Build();
            ITrigger tgMetting = TriggerBuilder.Create()
                                        .WithIdentity("触发器名称tgMetting", "触发器组tgMetting")
                                        .StartNow()
                                        .WithSimpleSchedule(x => x
                                            .WithIntervalInHours(1)
                                            .RepeatForever())
                                        .Build();
            scheduler.ScheduleJob(jobMetting, tgMetting);
            #endregion

            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                x.Service<ServiceRunner>();

                x.SetDescription("推送危急值短信");
                x.SetDisplayName("Dln短信服务");
                x.SetServiceName("Dln短信服务");

                x.EnablePauseAndContinue();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("您确认退出，将不再推送危急值？", "系统提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                e.Cancel = false;
                System.Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
