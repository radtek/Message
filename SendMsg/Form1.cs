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

namespace SendMsg
{
    public partial class Form1 : Form
    {
        public Form1()
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
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("dfdf");

            //string phone = "15261277153";

            WebReference.Service1 _client = new WebReference.Service1();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds_report = _client.GetReport();
            string patient_id = "", phone = "", doctor = "", patient = "", Item = "", score = "", limit = "";
            if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            {
                dt = _client.GetReport().Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    patient_id = dr["Patient_id"].ToString();
                    patient = dr["Patient_name"].ToString();
                    Item = dr["Item_name"].ToString();
                    score = dr["Current_result"].ToString();
                    limit = dr["Limit_ref"].ToString();
                    if (!String.IsNullOrEmpty(patient_id))
                    {
                        DataSet ds = _client.GetCardIDToDoctor(patient_id);
                        if (ds != null && ds.Tables[0].Rows.Count != 0)
                        {
                            dt2 = _client.GetCardIDToDoctor(patient_id).Tables[0];
                            if (dt2 != null)
                            {
                                foreach (DataRow dr2 in dt2.Rows)
                                {
                                    //phone = dr2["EmpMobileNum"].ToString();
                                    phone = "15261277153";
                                    doctor = dr2["EMPNAME"].ToString();

                                    //string sms = "5272718510496|" + doctor + "|" + patient + "|" + "在" + Item + "项目上分值为" + score;
                                    //string strSql = "insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES('" + id + "', '','" + phone + "', '" + sms + "',1,15,2, '" + _add_time + "', 'APP128')";
                                    //string strSql = "insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES('" + id + "', '','" + phone + "', '5272718510496|张三|李四|在xx项目上分值为20',1,15,2, '" + _add_time + "', 'APP128')";
                                    //string strSql = "insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES('" + id + "', '','15261277153', '5272718510496|耿山山|宋成霞|纤维蛋白原分值为20.2',1,15,2, '" + _add_time + "', 'APP128')";
                                    Guid guid = Guid.NewGuid();
                                    string id = guid.ToString();
                                    string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (!string.IsNullOrEmpty(patient) && !string.IsNullOrEmpty(Item) && !string.IsNullOrEmpty(score) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(doctor))
                                    {
                                        string strSql = string.Format("insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES('{0}', '','{1}', '5272718510513|{2}|{3}|{4}|{5}|{6}',1,15,2, '{7}', 'APP128')", id, phone, doctor, patient, Item, score, limit, _add_time);
                                        int i = DbHelperMySQL.ExecuteSql(strSql);
                                        if (i > 0)
                                        {
                                            label1.Text = "成功！";
                                        }
                                        else
                                        {
                                            label1.Text = "失败！";
                                        }
                                        return;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                                            .WithIntervalInSeconds(55)
                                            .RepeatForever())              //不间断重复执行
                                        .Build();


            scheduler.ScheduleJob(job1, trigger1);      //把作业，触发器加入调度器。
        }
    }
}
