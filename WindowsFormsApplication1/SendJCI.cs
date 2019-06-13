using Commmon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class SendJCI
    {
        #region JCI文件提醒
        public static void Send()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            string phone = "", revise_time = "", long_time = "";
            string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string dsNow = DateTime.Now.ToString("yyyy-MM-dd");
            //string dsNow = "2019-12-30";
            DbHelperSQLP db = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionString3"));
            string sql = "select tel, revise_time,long_time from FileInfo where tel is not null and filestate=0";
            DataSet ds = db.Query(sql);
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    phone = dr["tel"] + "";
                    //phone = "15261277153";
                    revise_time = !String.IsNullOrEmpty(dr["revise_time"] + "")?Convert.ToDateTime(dr["revise_time"]).ToString("yyyy-MM-dd"):"";
                    long_time = !String.IsNullOrEmpty(dr["long_time"] + "")?Convert.ToDateTime(dr["long_time"]).ToString("yyyy-MM-dd"):"";
                    if ((dsNow == revise_time || dsNow == long_time)&&!String.IsNullOrEmpty(phone))
                    {
                        Model.Sms_outbox model = new Model.Sms_outbox();
                        model.sismsid = Guid.NewGuid().ToString();
                        model.extcode = "01";
                        model.destaddr = phone;
                        model.messagecontent = "5272718510011";
                        model.reqdeliveryreport = 1;
                        model.msgfmt = 15;
                        model.sendmethod = 2;
                        model.requesttime = _add_time;
                        model.applicationid = "APP128";
                        if (bll.Add(model))
                        {
                            Model.BIF01022 model2 = new Model.BIF01022();
                            model2.Patient_id = "JCI";
                            model2.Patient_name = "JCI";
                            model2.Item_name = "JCI";
                            model2.Current_result = revise_time;
                            model2.EmpMobileNum = phone;
                            model2.EMPNAME = long_time;
                            model2.State = 10;
                            model2.Add_time = _add_time;
                            bll2.Add(model2);
                        }
                    }
                }
            }
        }
        #endregion

        #region 门禁提醒
        public static void SendDoor()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            string _add_time = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = "select BuildingNO,FloorNO,GateName,GateState from AlarmInfo";
            DataSet ds_report = DbHelperSQL.Query(sql);
            if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds_report.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    string BuildingNO = dr["BuildingNO"] + "";
                    string FloorNO = dr["FloorNO"] + "";
                    string GateName = dr["GateName"] + "";
                    string GateState = dr["GateState"] + "";

                    if (!String.IsNullOrEmpty(BuildingNO) && !String.IsNullOrEmpty(FloorNO) && !String.IsNullOrEmpty(GateName) && !String.IsNullOrEmpty(GateState))
                    {
                        string sql2 = "select ContactPhoneNO from AlarmContact where State=1";
                        DataSet ds = DbHelperSQL.Query(sql2);
                        DataTable dt_phone = ds.Tables[0];
                        foreach (DataRow dr2 in dt_phone.Rows)
                        {
                            string ContactPhoneNO = dr2["ContactPhoneNO"] + "";
                            if (!string.IsNullOrEmpty(ContactPhoneNO))
                            {
                                Model.Sms_outbox model = new Model.Sms_outbox();
                                model.sismsid = Guid.NewGuid().ToString();
                                model.extcode = "01";
                                model.destaddr = ContactPhoneNO;
                                model.messagecontent = "5272718510012|" + BuildingNO + "|" + FloorNO + "|" + GateName + "|" + GateState;
                                model.reqdeliveryreport = 1;
                                model.msgfmt = 15;
                                model.sendmethod = 2;
                                model.requesttime = _add_time;
                                model.applicationid = "APP128";
                                bll.Add(model);
                            }

                        }

                    }
                }

            }
        }
        #endregion

    }
}
