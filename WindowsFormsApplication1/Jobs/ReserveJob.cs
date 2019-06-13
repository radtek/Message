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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class ReserveJob
    {
        public static void Send()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            string phone = "", revise_time = "", PatiID="",Name="";
            string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string dsNow = DateTime.Now.ToString("yyyy-MM-dd");
            //string dsNow = "2019-12-30";
            DbHelperSQLP db = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionStringBright_CB"));
            string sql = "select AddTime,Name,Tel,PatiID from BloodPatient where Tel!='' and AddTime!='' and len(Tel)>12";
            DataSet ds = db.Query(sql);
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    PatiID = dr["PatiID"] + "";
                    Name = dr["Name"] + "";
                    phone = dr["Tel"] + "";
                    //phone = "15261277153";
                    revise_time = Convert.ToDateTime(dr["AddTime"]).AddDays(85).ToString("yyyy-MM-dd");
                    if (dsNow == revise_time)
                    {
                        Model.Sms_outbox model = new Model.Sms_outbox();
                        model.sismsid = Guid.NewGuid().ToString();
                        model.extcode = "01";
                        model.destaddr = phone.Split('|')[0] ;
                        model.messagecontent = "5272718510018";
                        model.reqdeliveryreport = 1;
                        model.msgfmt = 15;
                        model.sendmethod = 2;
                        model.requesttime = _add_time;
                        model.applicationid = "APP128";
                        if (bll.Add(model))
                        {
                            Model.BIF01022 model2 = new Model.BIF01022();
                            model2.Patient_id = PatiID;
                            model2.Patient_name = Name;
                            model2.Item_name = Name;
                            model2.Current_result = revise_time;
                            model2.EmpMobileNum = phone;
                            model2.EMPNAME = "预约回诊";
                            model2.State = 11;
                            model2.Add_time = _add_time;
                            bll2.Add(model2);
                        }
                        //break;
                    }
                }
            }
        }
    }
}
