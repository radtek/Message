using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Commmon;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public class SendMsg
    {
        public static int SendJob()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            DbHelperSQLP db = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionString2"));
            //string sql = "select B.EMDID, BuildingName,orgname,equipmentname,TemperatureValue,HumidityValue,Phone,MonitoringTime"
            //            + " from MonitorITEM A"
            //            + " INNER join EMDMAIN B  on A.EMDID = B.EMDID AND A.NODEIndex = B.NODEIndex"
            //            + " where(TemperatureValue < 2 or TemperatureValue > 8)"
            //            + " and MonitoringTime between convert(char(19), DATEADD(hour, -1, GETDATE()), 120) and CONVERT(varchar, GETDATE(), 120)";

            string sql = "select B.EMDID, BuildingName,orgname,equipmentname,TemperatureValue,HumidityValue,Phone,MonitoringTime,NEXTTIME,B.NODEIndex"
                        + " from MonitorITEM A"
                        + " INNER join EMDMAIN B  on A.EMDID = B.EMDID AND A.NODEIndex = B.NODEIndex"
                        + " where(TemperatureValue < 2 or TemperatureValue > 8) and NEXTTIME< GETDATE()"
                        + " and MonitoringTime between convert(char(19), DATEADD(hour, -1, GETDATE()), 120) and CONVERT(varchar, GETDATE(), 120)";
            DataSet ds_report = db.Query(sql);
            int i = 0, NODEIndex = 0;
            DataTable dt = null;
            string BuildingName = "", orgname = "", equipmentname = "", TemperatureValue = "", HumidityValue = "", Phone = "", EMDID="" , NEXTTIME="";
            string messageID2 = System.Configuration.ConfigurationManager.AppSettings["MessageID2"].ToString();
            if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            {
                dt = ds_report.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    BuildingName = dr["BuildingName"].ToString();
                    orgname = dr["orgname"].ToString();
                    equipmentname = dr["equipmentname"].ToString();
                    TemperatureValue = dr["TemperatureValue"].ToString();
                    HumidityValue = dr["HumidityValue"].ToString();
                    Phone = dr["Phone"].ToString();
                    //Phone = "15261277153";

                    EMDID= dr["EMDID"].ToString();
                    NODEIndex = Convert.ToInt32(dr["NODEIndex"].ToString());
                    NEXTTIME= dr["NEXTTIME"].ToString();
                    Guid guid = Guid.NewGuid();
                    string id = guid.ToString();
                    string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (!string.IsNullOrEmpty(BuildingName) && !string.IsNullOrEmpty(orgname) && !string.IsNullOrEmpty(equipmentname) && !string.IsNullOrEmpty(TemperatureValue) && !string.IsNullOrEmpty(HumidityValue) && !string.IsNullOrEmpty(Phone))
                    {
                        if (!bll.Exists("01", Phone))
                        {
                            Model.Sms_outbox model = new Model.Sms_outbox();
                            model.sismsid = id;
                            model.extcode = "01";
                            model.destaddr = Phone;
                            model.messagecontent = messageID2 + "|" + BuildingName + "|" + orgname + "|" + equipmentname + "|" + TemperatureValue + "|" + HumidityValue;
                            model.reqdeliveryreport = 1;
                            model.msgfmt = 15;
                            model.sendmethod = 2;
                            model.requesttime = _add_time;
                            model.applicationid = "APP128";
                            if (bll.Add(model))
                            {
                                string date = Convert.ToDateTime(NEXTTIME).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                                string sql2 = "update A set NEXTTIME='"+ date + "' from MonitorITEM A "
                                            + " INNER join EMDMAIN B  on A.EMDID = B.EMDID AND A.NODEIndex = B.NODEIndex"
                                            + " where(TemperatureValue < 2 or TemperatureValue > 8) and NEXTTIME< GETDATE()"
                                            + " and MonitoringTime between convert(char(19), DATEADD(hour, -1, GETDATE()), 120) and CONVERT(varchar, GETDATE(), 120) and B.EMDID = '"+ EMDID + "' and B.NODEIndex = "+ NODEIndex + "";
                                db.ExecuteSql(sql2);
                                //return i++;
                                i++;
                            }

                        }


                    }
                }
            }

            return i;
        }
        public static int Send()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            int i = 0;
            WebReference.Service1 _client = new WebReference.Service1();
            DataTable dt = null;
            DataTable dt2 = null;
            DataSet ds_report = _client.GetReport();
            string patient_id = "", phone = "", doctor = "", patient = "", Item = "", score = "", limit = "";
            string messageID = System.Configuration.ConfigurationManager.AppSettings["MessageID"].ToString();
            if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            {
                dt = _client.GetReport().Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    patient_id = dr["Patient_id"].ToString();
                    //patient_id = "0012528999";
                    //patient ="管爱红之子20180131";
                    string s = dr["Patient_name"].ToString();
                    patient = s.Length>5?s.Substring(0,5):s;
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
                                    phone = dr2["EmpMobileNum"].ToString();
                                    //phone = "15261277153";
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
                                        if (!bll.Exists("01", phone))
                                        {
                                            Model.Sms_outbox model = new Model.Sms_outbox();
                                            model.sismsid = id;
                                            model.extcode = "01";
                                            model.destaddr = phone;
                                            model.messagecontent = messageID + "|" + doctor + "|" + patient + "|" + Item + "|" + score + "|" + limit;
                                            model.reqdeliveryreport = 1;
                                            model.msgfmt = 15;
                                            model.sendmethod = 2;
                                            model.requesttime = _add_time;
                                            model.applicationid = "APP128";
                                            if (bll.Add(model))
                                            {
                                                //return i++;
                                                i++;
                                            }

                                        }

                                        //DateTime beginTime = Convert.ToDateTime(bll.getSentSms());
                                        //DateTime endTime = beginTime.AddMinutes(5);
                                        //bool s1 = bll.ExistMinute(phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        //bool hasValue = bll2.Exists(phone, Item, score, patient_id, 0);
                                        //if (s1 == true)//五分钟之内
                                        //{
                                        //    if (hasValue)//存在
                                        //    {
                                        //        Model.BIF01022 model = new Model.BIF01022();
                                        //        model.Patient_id = patient_id;
                                        //        model.Item_name = Item;
                                        //        model.Current_result = score;
                                        //        model.EmpMobileNum = phone;
                                        //        model.State = 1;
                                        //        bll2.Update(model);
                                        //    }
                                        //    else
                                        //    {
                                        //        Model.Sms_outbox model = new Model.Sms_outbox();
                                        //        model.sismsid = id;
                                        //        model.extcode = "01";
                                        //        model.destaddr = phone;
                                        //        model.messagecontent = messageID + "|" + doctor + "|" + patient + "|" + Item + "|" + score + "|" + limit;
                                        //        model.reqdeliveryreport = 1;
                                        //        model.msgfmt = 15;
                                        //        model.sendmethod = 2;
                                        //        model.requesttime = _add_time;
                                        //        model.applicationid = "APP128";
                                        //        if (bll.Add(model))
                                        //        {
                                        //            Model.BIF01022 model2 = new Model.BIF01022();
                                        //            model2.Patient_id = patient_id;
                                        //            model2.Patient_name = patient;
                                        //            model2.Item_name = Item;
                                        //            model2.Current_result = score;
                                        //            model2.EmpMobileNum = phone;
                                        //            model2.EMPNAME = doctor;
                                        //            model2.State = 0;
                                        //            model2.Add_time = _add_time;
                                        //            bll2.Add(model2);
                                        //            return ++i;
                                        //        }

                                        //    }
                                        //}
                                        //else
                                        //{

                                        //    if (!bll2.Exists(phone, Item, score, patient_id, 1))//不存在或者状态为0 发送
                                        //    {
                                        //        Model.Sms_outbox model = new Model.Sms_outbox();
                                        //        model.sismsid = id;
                                        //        model.extcode = "01";
                                        //        model.destaddr = phone;
                                        //        model.messagecontent = messageID + "|" + doctor + "|" + patient + "|" + Item + "|" + score + "|" + limit;
                                        //        model.reqdeliveryreport = 1;
                                        //        model.msgfmt = 15;
                                        //        model.sendmethod = 2;
                                        //        model.requesttime = _add_time;
                                        //        model.applicationid = "APP128";
                                        //        if (bll.Add(model))
                                        //        {
                                        //            Model.BIF01022 model2 = new Model.BIF01022();
                                        //            model2.Patient_id = patient_id;
                                        //            model2.Patient_name = patient;
                                        //            model2.Item_name = Item;
                                        //            model2.Current_result = score;
                                        //            model2.EmpMobileNum = phone;
                                        //            model2.EMPNAME = doctor;
                                        //            model2.State = 0;
                                        //            model2.Add_time = _add_time;
                                        //            bll2.Add(model2);
                                        //            return ++i;
                                        //        }

                                        //    }

                                        //}
                                        //string strSql = string.Format("insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES('{0}', '','{1}', '5272718510513|{2}|{3}|{4}|{5}|{6}',1,15,2, '{7}', 'APP128')", id, phone, doctor, patient, Item, score, limit, _add_time);
                                        //i=DbHelperMySQL.ExecuteSql(strSql);
                                        //return i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return i;
        }


        public static int SendPACS()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            int i = 0;
            
            DataTable dt = null;
            DataTable dt2 = null;
            string _time = DateTime.Now.ToString("yyyy-MM-dd");
            //DataSet ds_report = _client.GetReport();
            string sql = "select patientID,patientName,birthDate,inspect,reportDoc,ApplyTypeID from BIF01021 where reportDate >= '2018-05-01 00:00:00' and reportDate<='" + _time + " 23:59:59'";
            DataSet ds_report = DbHelperSQL.Query(sql);
            string patientID = "", birthDate = "", inspect = "无", patientName = "", reportDoc = "", ApplyTypeID = "",inspect_Doc="",report_Doc="", inspect_phone="", report_phone = "", report_Name = "";
            string messageID = System.Configuration.ConfigurationManager.AppSettings["MessageID_inspect"].ToString();


            if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            {
                dt = ds_report.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    patientID = dr["patientID"].ToString();
                    string s = dr["inspect"].ToString();
                    foreach (Match match in new Regex(@"[\u4e00-\u9fa5]+", RegexOptions.Singleline).Matches(s))
                    {
                        s = match.ToString();
                    }
                    inspect = s.Length > 20 ? s.Substring(0, 20) : s;
                    patientName = dr["patientName"].ToString();
                    reportDoc = dr["reportDoc"].ToString();
                    string[] username = reportDoc.Split(',');
                    if (username.Length == 2)
                    {
                        inspect_Doc = username[0];
                        report_Doc = username[1];
                    }
                    else
                    {
                        inspect_Doc = reportDoc;
                    }
                    ApplyTypeID = dr["ApplyTypeID"].ToString();
                    string[] typeID = ApplyTypeID.Split('-');
                    if (typeID.Length == 2)
                    {
                        ApplyTypeID = typeID[1];
                    }
                    birthDate = dr["birthDate"].ToString();
                    if (!String.IsNullOrEmpty(patientID) && !String.IsNullOrEmpty(inspect) && !String.IsNullOrEmpty(patientName) && !String.IsNullOrEmpty(birthDate) && !String.IsNullOrEmpty(ApplyTypeID))
                    {
                        //inspect_phone = getPhone(inspect_Doc);//GetDoctorPhone
                        report_Doc = (inspect_phone == report_Doc) ? "" : report_Doc;
                        report_phone = getPhone(report_Doc);
                        report_Name = getName(report_Doc);
                        inspect_phone = "15261277153";
                        //report_Name = "吴晓磊";
                        //report_phone = "18936979026";
                        Guid guid = Guid.NewGuid();
                        string id = guid.ToString();
                        string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        if (!String.IsNullOrEmpty(inspect_phone))
                        {
                            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime beginTime = Convert.ToDateTime(nowTime).AddMinutes(-15);
                            bool s1 = bll.ExistMinute(inspect_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                            bool hasValue = bll2.Exists(inspect_phone, inspect, "", patientID, 0);
                            if (s1 == true)//十五分钟之内
                            {
                                if (hasValue)//存在
                                {
                                    string updateTime = bll2.getUpdate_time(inspect_phone, inspect, "", patientID, 0);
                                    Model.BIF01022 model = new Model.BIF01022();
                                    model.Patient_id = patientID;
                                    model.Item_name = inspect;
                                    model.Current_result = "";
                                    model.EmpMobileNum = inspect_phone;
                                    model.State = 1;
                                    if (!String.IsNullOrEmpty(updateTime))
                                    {
                                        bll2.Update(model);
                                    }
                                    else
                                    {
                                        model.Update_time = bll.getReceivetime(inspect_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                        bll2.Update2(model);
                                    }
                                   
                                }
                                if (!bll2.Exists(inspect_phone, inspect, "", patientID, 1))//不存在或者状态为0 发送
                                {
                                    Model.Sms_outbox model = new Model.Sms_outbox();
                                    model.sismsid = id;
                                    model.extcode = "01";
                                    model.destaddr = inspect_phone;
                                    model.messagecontent = messageID + "|" + inspect_Doc + "|" + patientName + "|" + birthDate + "|" + ApplyTypeID + "|" + inspect;
                                    model.reqdeliveryreport = 1;
                                    model.msgfmt = 15;
                                    model.sendmethod = 2;
                                    model.requesttime = _add_time;
                                    model.applicationid = "APP128";
                                    if (bll.Add(model))
                                    {
                                        Model.BIF01022 model2 = new Model.BIF01022();
                                        model2.Patient_id = patientID;
                                        model2.Patient_name = patientName;
                                        model2.Item_name = inspect;
                                        model2.Current_result = "";
                                        model2.EmpMobileNum = inspect_phone;
                                        model2.EMPNAME = inspect_Doc;
                                        model2.State = 0;
                                        model2.Add_time = _add_time;
                                        bll2.Add(model2);
                                        //return ++i;
                                    }

                                }
                            }
                            else
                            {
                                if (hasValue&&!String.IsNullOrEmpty(report_phone))//状态为0 发送给报告医生
                                {
                                    Model.Sms_outbox model = new Model.Sms_outbox();
                                    model.sismsid = id;
                                    model.extcode = "01";
                                    model.destaddr = report_phone;
                                    model.messagecontent = "5272718510009|" + report_Name + "|" + inspect_Doc;
                                    model.reqdeliveryreport = 1;
                                    model.msgfmt = 15;
                                    model.sendmethod = 2;
                                    model.requesttime = _add_time;
                                    model.applicationid = "APP128";
                                    if (bll.Add(model))
                                    {
                                        Model.BIF01022 model2 = new Model.BIF01022();
                                        model2.Patient_id = patientID;
                                        model2.Patient_name = patientName;
                                        model2.Item_name = inspect;
                                        model2.Current_result = "";
                                        model2.EmpMobileNum = report_phone;
                                        model2.EMPNAME = report_Name;
                                        model2.State = 2;
                                        model2.Add_time = _add_time;
                                        if (bll2.Add(model2))
                                        {
                                            bool s3 = bll.ExistMinute(report_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                            if (s3 == true)
                                            {
                                                Model.BIF01022 model3 = new Model.BIF01022();
                                                model3.Patient_id = patientID;
                                                model3.Item_name = inspect;
                                                model3.Current_result = "";
                                                model3.EmpMobileNum = inspect_phone;
                                                model3.State = 1;
                                                bll2.Update(model3);
                                            }
                                            
                                        }

                                    }
                                }
                                if (!bll2.Exists(inspect_phone, inspect, "", patientID, 1))//不存在或者状态为0 发送
                                {
                                    Model.Sms_outbox model = new Model.Sms_outbox();
                                    model.sismsid = id;
                                    model.extcode = "01";
                                    model.destaddr = inspect_phone;
                                    model.messagecontent = messageID + "|" + inspect_Doc + "|" + patientName + "|" + birthDate + "|" + ApplyTypeID + "|" + inspect;
                                    model.reqdeliveryreport = 1;
                                    model.msgfmt = 15;
                                    model.sendmethod = 2;
                                    model.requesttime = _add_time;
                                    model.applicationid = "APP128";
                                    if (bll.Add(model))
                                    {
                                        Model.BIF01022 model2 = new Model.BIF01022();
                                        model2.Patient_id = patientID;
                                        model2.Patient_name = patientName;
                                        model2.Item_name = inspect;
                                        model2.Current_result = "";
                                        model2.EmpMobileNum = inspect_phone;
                                        model2.EMPNAME = inspect_Doc;
                                        model2.State = 0;
                                        model2.Add_time = _add_time;
                                        bll2.Add(model2);
                                        //return ++i;
                                    }

                                }
                                //return i;
                            }
                        }

                        
                        
                        //return i;

                    }
                }
            }

            return i;
        }
        private static string getPhone(string empid)
        {
            WebReference.Service1 _client = new WebReference.Service1();
            DataSet ds = _client.GetDoctorPhone(empid);
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    return dr["EmpMobileNum"].ToString();
                }
            }
            return "";
        }
        private static string getName(string empid)
        {
            WebReference.Service1 _client = new WebReference.Service1();
            DataSet ds = _client.GetDoctorPhone(empid);
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    return dr["EMPNAME"].ToString();
                }
            }
            return "";
        }
    }
}
