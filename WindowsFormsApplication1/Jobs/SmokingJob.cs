/***********************************************************
**文 件 名：ReserveJob 
**命名空间：WindowsFormsApplication1.Jobs 
**内    容： 
**功    能： 
**文件关系： 
**作    者：小胡  
**生成日期：2019-06-06 10:11:10 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/


using Commmon;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class SmokingJob: IJob
    {
        public void Send()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            string phone = "",Name="",AddTime="";
            string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string dsNow = DateTime.Now.ToString("yyyy-MM-dd");
            //string dsNow = "2019-12-30";
            DbHelperSQLP db = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionStringOnline"));
            string sql = "select name, tel,addTime from Smoke where datediff(day, dateadd(dd,0,addTime),getdate())=1 and tel!=''";
            DataSet ds = db.Query(sql);
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    AddTime = dr["addTime"] + "";
                    Name = dr["name"] + "";
                    phone = dr["tel"] + "";
                    //phone = "15261277153";
                    Model.Sms_outbox model = new Model.Sms_outbox();
                    model.sismsid = Guid.NewGuid().ToString();
                    model.extcode = "01";
                    model.destaddr = phone;
                    model.messagecontent = "5272718510019";
                    model.reqdeliveryreport = 1;
                    model.msgfmt = 15;
                    model.sendmethod = 2;
                    model.requesttime = _add_time;
                    model.applicationid = "APP128";
                    if (bll.Add(model))
                    {
                        Model.BIF01022 model2 = new Model.BIF01022();
                        model2.Patient_id = Name;
                        model2.Patient_name = Name;
                        model2.Item_name = Name;
                        model2.Current_result = AddTime;
                        model2.EmpMobileNum = phone;
                        model2.EMPNAME = "戒烟";
                        model2.State = 12;
                        model2.Add_time = _add_time;
                        bll2.Add(model2);
                    }
                }
            }
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Send();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
