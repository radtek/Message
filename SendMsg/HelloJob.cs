using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Windows.Forms;
using Topshelf;
using Commmon;
using System.Data;

namespace SendMsg
{
    public class HelloJob : IJob, ServiceControl
    {
        public void Execute(IJobExecutionContext context)
        {        
            MessageBox.Show("dfdf");

            //WebReference.Service1 _client = new WebReference.Service1();
            //DataTable dt = new DataTable();
            //DataTable dt2 = new DataTable();
            //DataSet ds_report = _client.GetReport();
            //string patient_id = "", phone = "", doctor = "", patient = "", Item = "", score = "", limit = "";
            //if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            //{
            //    dt = _client.GetReport().Tables[0];
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        patient_id = dr["Patient_id"].ToString();
            //        patient = dr["Patient_name"].ToString();
            //        Item = dr["Item_name"].ToString();
            //        score = dr["Current_result"].ToString();
            //        limit = dr["Limit_ref"].ToString();
            //        if (!String.IsNullOrEmpty(patient_id))
            //        {
            //            DataSet ds = _client.GetCardIDToDoctor(patient_id);
            //            if (ds != null && ds.Tables[0].Rows.Count != 0)
            //            {
            //                dt2 = _client.GetCardIDToDoctor(patient_id).Tables[0];
            //                if (dt2 != null)
            //                {
            //                    foreach (DataRow dr2 in dt2.Rows)
            //                    {
            //                        //phone = dr2["EmpMobileNum"].ToString();
            //                        phone = "15261277153";
            //                        doctor = dr2["EMPNAME"].ToString();
            //                        Guid guid = Guid.NewGuid();
            //                        string id = guid.ToString();
            //                        string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                        if (!string.IsNullOrEmpty(patient) && !string.IsNullOrEmpty(Item) && !string.IsNullOrEmpty(score) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(doctor))
            //                        {
            //                            string strSql = string.Format("insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES('{0}', '','{1}', '5272718510513|{2}|{3}|{4}|{5}|{6}',1,15,2, '{7}', 'APP128')", id, phone, doctor, patient, Item, score, limit, _add_time);
            //                            DbHelperMySQL.ExecuteSql(strSql);
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

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
