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
        #region 温湿度报警
        public static int SendJob()
        {
            BaseClass bc = new BaseClass();
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            DbHelperSQLP db = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionString2"));
            string sql = "select B.EMDID, BuildingName,orgname,equipmentname,TemperatureValue,HumidityValue,Phone,MonitoringTime,NEXTTIME,B.NODEIndex,Phone2"
                        + " from MonitorITEM A"
                        + " INNER join EMDMAIN B  on A.EMDID = B.EMDID AND A.NODEIndex = B.NODEIndex"
                        + " where (TemperatureValue>(select top 1 EMDMAIN.MaxTemperatureValue from EMDMAIN where EMDMAIN.EMDID=B.EMDID) or TemperatureValue<(select top 1 EMDMAIN.MinTemperatureValue from EMDMAIN where EMDMAIN.EMDID=B.EMDID)) "
                        + " and MonitoringTime between convert(char(19), DATEADD(hour, -1, GETDATE()), 120) and CONVERT(varchar, GETDATE(), 120)";
            //'2018-04-03 00:00:00' and '2018-04-03 02:59:59'
            DataSet ds_report = db.Query(sql);
            int i = 0, NODEIndex = 0;
            DataTable dt = null;
            string BuildingName = "", orgname = "", equipmentname = "", TemperatureValue = "", HumidityValue = "", Phone = "", EMDID = "", NEXTTIME = "", report_phone = "";
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
                    //Phone = dr["Phone"].ToString();
                    Phone = "15261277153";
                    report_phone = dr["Phone2"] + "";
                    EMDID = dr["EMDID"].ToString();
                    NODEIndex = Convert.ToInt32(dr["NODEIndex"].ToString());
                    NEXTTIME = dr["NEXTTIME"].ToString();
                    Guid guid = Guid.NewGuid();
                    string id = guid.ToString();
                    string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (!string.IsNullOrEmpty(BuildingName) && !string.IsNullOrEmpty(orgname) && !string.IsNullOrEmpty(equipmentname) && !string.IsNullOrEmpty(TemperatureValue) && !string.IsNullOrEmpty(HumidityValue) && !string.IsNullOrEmpty(Phone))
                    {
                        string messageContent = messageID2 + "|" + BuildingName + "|" + orgname + "|" + equipmentname + "|" + TemperatureValue;
                        DateTime beginTime = Convert.ToDateTime(_add_time).AddMinutes(-5);
                        bool s1 = bll.ExistMinute(Phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), _add_time);
                        bool hasValue = bll2.Exists1(Phone, TemperatureValue, equipmentname, 5);
                        if (s1 == true)//五分钟之内
                        {
                            if (hasValue)//存在
                            {
                                string updateTime = bll2.getUpdate_time1(Phone, TemperatureValue, equipmentname, 5);
                                Model.BIF01022 model = new Model.BIF01022();
                                model.Patient_id = equipmentname;
                                model.Item_name = TemperatureValue;
                                model.Current_result = HumidityValue;
                                model.EmpMobileNum = Phone;
                                model.State = 6;
                                if (!String.IsNullOrEmpty(updateTime))
                                {
                                    bll2.Update1(model);
                                }
                                else
                                {
                                    model.Update_time = bll.getReceivetime(Phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), _add_time);
                                    bll2.Update3(model);
                                }

                            }
                            if (!bll2.Exists1(Phone, TemperatureValue, equipmentname, 6))
                            {
                                //发送短信
                                bool flag =bc.sendMsg(Phone, messageContent);
                                if (flag)
                                {
                                    bc.writeLog(equipmentname,"", TemperatureValue, HumidityValue, Phone,"",5);
                                }

                            }
                        }
                        else
                        {
                            if (hasValue && !String.IsNullOrEmpty(report_phone))
                            {
                                if (!bll2.Exists1(Phone, TemperatureValue, equipmentname, 6))
                                {
                                    string[] tel = new string[2];
                                    tel[0] = report_phone; tel[1] = "15366973170";
                                    for (int j = 0; j < tel.Length; j++)
                                    {
                                        report_phone = tel[j];
                                        //发送短信

                                        bool flag = bc.sendMsg(report_phone, messageContent);
                                        if (flag)
                                        {
                                            bool s3 = bll.ExistMinute(report_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), _add_time);
                                            if (s3 == true)
                                            {
                                                Model.BIF01022 model3 = new Model.BIF01022();
                                                model3.Patient_id = equipmentname;
                                                model3.Item_name = TemperatureValue;
                                                model3.Current_result = HumidityValue;
                                                model3.EmpMobileNum = Phone;
                                                model3.State = 6;
                                                bll2.Update1(model3);
                                            }

                                        }
                                    }
                                }
                                    
                            }
                            if (!bll2.Exists1(Phone, TemperatureValue, equipmentname, 6))//不存在或者状态为0 发送
                            {
                                //发送短信
                                bool flag = bc.sendMsg(Phone, messageContent);
                                if (flag)
                                {
                                    bc.writeLog(equipmentname, "", TemperatureValue, HumidityValue, Phone, "", 5);
                                }

                            }
                        }
                    }
                }
            }

            return i;
        }
        #endregion

        #region LIS危急值
        public static int SendLis()
        {
            DbHelperSQLP db = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionString2"));
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            BaseClass bc = new BaseClass();
            int i = 0;
            WebReference.Service1 _client = new WebReference.Service1();
            DataTable dt = null;
            DataSet ds_report = _client.GetReport();
            string patient_id = "", phone = "", doctor = "", patient = "", Item = "", score = "", limit = "", Source_doctor = "";
            string messageID = System.Configuration.ConfigurationManager.AppSettings["MessageID"].ToString();
            string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (ds_report != null && ds_report.Tables[0].Rows.Count != 0)
            {
                dt = _client.GetReport().Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    patient_id = dr["Patient_id"].ToString();
                    doctor = dr["Source_doctor"].ToString();
                    string s = dr["Patient_name"].ToString();
                    patient = s.Length > 5 ? s.Substring(0, 5) : s;
                    Item = dr["Item_name"].ToString();
                    score = dr["Current_result"].ToString();
                    limit = dr["Limit_ref"].ToString();
                    Source_doctor = dr["Source_doctor"].ToString();
                    string Patient_state = dr["Patient_state"].ToString();
                    string Diagorgid = dr["Diagorgid"] + "";

                    string messageContent = messageID + "|" + doctor + "|" + patient + "|" + Item + "|" + score + "|" + limit;
                    if (Patient_state == "BIW")
                    {
                        phone = getPhoneByDiagOrgID(Diagorgid);
                        //phone = "15261277153";
                        if (!String.IsNullOrEmpty(phone))
                        {
                            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime beginTime = Convert.ToDateTime(nowTime).AddMinutes(-10);
                            bool s1 = bll.ExistMinute(phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                            bool hasValue = bll2.Exists(phone, Item, score, patient_id, 0);
                            if (s1 == true)
                            {
                                if (hasValue)//存在
                                {
                                    Model.BIF01022 model = new Model.BIF01022();
                                    model.Patient_id = patient_id;
                                    model.Item_name = Item;
                                    model.Current_result = score;
                                    model.EmpMobileNum = phone;
                                    model.State = 1;
                                    bll2.Update(model);
                                    //更新LIS回复时间
                                    string receiveTime = bll.getReceivetime(phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                    string sql = "update TB_REPORTLIMIT set ReceiveTime='" + receiveTime + "' where patient_id='" + patient_id + "' and patient_name='" + patient + "' and item_name='" + Item + "' and current_result='" + score + "'";
                                    db.ExecuteSql(sql);
                                }
                                else
                                {
                                    int rows = bc.getRecordCount(phone, Item, score, patient_id, 0);//获取已经发送没回复的行数
                                    if (!bll2.Exists(phone, Item, score, patient_id, 1)&&rows<4)//不存在或者状态为0 发送
                                    {
                                        //发送短信
                                        bool flag = bc.sendMsg(phone, messageContent);
                                        if (flag)
                                        {
                                            bc.writeLog(patient_id, patient, Item, score, phone, doctor, 0);
                                        }

                                    }

                                }
                            }
                            else
                            {

                                int rows = bc.getRecordCount(phone, Item, score, patient_id, 0);//获取已经发送没回复的行数
                                if (!bll2.Exists(phone, Item, score, patient_id, 1) && rows < 4)//不存在或者状态为0 发送
                                {
                                    //发送短信
                                    bool flag = bc.sendMsg(phone, messageContent);
                                    if (flag)
                                    {
                                        bc.writeLog(patient_id, patient, Item, score, phone, doctor, 0);
                                    }

                                }

                            }
                        }

                    }
                    else
                    {
                        phone = getPhone(Source_doctor);
                        string[] tel = new string[2];
                        tel[0] = phone; tel[1] = "17851839909";
                        for (int j = 0; j < tel.Length; j++)
                        {
                            phone = tel[j];
                            if (!String.IsNullOrEmpty(phone))
                            {
                                string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                DateTime beginTime = Convert.ToDateTime(nowTime).AddMinutes(-10);
                                bool s1 = bll.ExistMinute(tel[0], beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                bool s2 = bll.ExistMinute(tel[1], beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                bool hasValue = bll2.Exists(tel[0], Item, score, patient_id, 0);
                                if (s1 == true|| s2 == true)
                                {
                                    if (hasValue)//存在
                                    {
                                        Model.BIF01022 model = new Model.BIF01022();
                                        model.Patient_id = patient_id;
                                        model.Item_name = Item;
                                        model.Current_result = score;
                                        model.EmpMobileNum = tel[0];
                                        model.State = 1;
                                        bll2.Update(model);
                                        //更新LIS回复时间
                                        string receiveTime = bll.getReceivetime(phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                        string sql = "update TB_REPORTLIMIT set ReceiveTime='"+receiveTime+"' where patient_id='"+patient_id+"' and patient_name='"+patient+"' and item_name='"+Item+"' and current_result='"+score+"'";
                                        db.ExecuteSql(sql);
                                    }
                                    else
                                    {
                                        int rows = bc.getRecordCount(tel[0], Item, score, patient_id, 0);//获取已经发送没回复的行数
                                        if (!bll2.Exists(tel[0], Item, score, patient_id, 1)&&rows<7)//不存在或者状态为0 发送
                                        {
                                            bool flag = bc.sendMsg(phone, messageContent); //发送短信
                                            if (flag)
                                            {
                                                bc.writeLog(patient_id, patient, Item, score, tel[0], doctor, 0);
                                            }
                                        }
                                                                                            
                                            

                                    }
                                }
                                else
                                {

                                    int rows = bc.getRecordCount(tel[0], Item, score, patient_id, 0);//获取已经发送没回复的行数
                                    if (!bll2.Exists(tel[0], Item, score, patient_id, 1) && rows < 7)//不存在或者状态为0 发送
                                    {
                                        //发送短信
                                        bool flag = bc.sendMsg(phone, messageContent);
                                        if (flag)
                                        {
                                            bc.writeLog(patient_id, patient, Item, score, tel[0], doctor, 0);
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
            }

            return i;
        }
        #endregion

        #region PACS危急值
        public static int SendPACSValue()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            BaseClass bc = new BaseClass();
            int i = 0;

            DataTable dt = null;
            string _time = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = "select patientID,cardID,patientName,birthDate,inspect,reportDoc,ApplyTypeID from BIF01021 where reportDate >= '" + _time + " 00:00:00' and reportDate<='" + _time + " 23:59:59'";
            //string sql = "select * from BIF01021 where reportDate between '2018-06-04 00:00:00' and '2018-06-04 23:59:59' order by reportDate desc";
            DataSet ds_report = DbHelperSQL.Query(sql);
            string patientID = "", birthDate = "", inspect = "无", patientName = "", reportDoc = "", ApplyTypeID = "", inspect_Doc = "", report_Doc = "", inspect_phone = "", report_phone = "", report_Name = "";
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
                    string cardid = dr["cardID"].ToString();
                    string biw = getBIW(cardid);
                    
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
                        
                        string messageContent = messageID + "|" + inspect_Doc + "|" + patientName + "|" + birthDate + "|" + ApplyTypeID;
                        if (biw == "BIW")
                        {
                            inspect_phone = getPhoneByDiagOrgID(getORGID(cardid));//获取所在病区手机号码
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
                                            string receiveTime = bll.getReceivetime(inspect_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                            model.Update_time = receiveTime;
                                            bll2.Update2(model);
                                            string sql2 = "update BIF01021 set ReceiveTime ='" + receiveTime + "' where patientID='" + patientID + "'";
                                            DbHelperSQL.ExecuteSql(sql2);
                                        }

                                    }
                                    int rows = bc.getRecordCount(inspect_phone, inspect, "", patientID, 0);//获取已经发送没回复的行数
                                    if (!bll2.Exists(inspect_phone, inspect, "", patientID, 1)&&rows<4)//不存在或者状态为0 发送
                                    {
                                        //发送短信
                                        bool flag = bc.sendMsg(inspect_phone, messageContent);
                                        if (flag)
                                        {
                                            bc.writeLog(patientID, patientName, inspect, "", inspect_phone, inspect_Doc, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    int rows = bc.getRecordCount(inspect_phone, inspect, "", patientID, 0);//获取已经发送没回复的行数
                                    if (!bll2.Exists(inspect_phone, inspect, "", patientID, 1) && rows < 4)//不存在或者状态为0 发送
                                    {
                                        //发送短信
                                        bool flag = bc.sendMsg(inspect_phone, messageContent);
                                        if (flag)
                                        {
                                            bc.writeLog(patientID, patientName, inspect, "", inspect_phone, inspect_Doc, 0);
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            inspect_phone = getPhone(inspect_Doc);//GetDoctorPhone
                            report_Doc = (inspect_phone == report_Doc) ? "" : report_Doc;
                            report_phone = getPhone(report_Doc);
                            report_Name = getName(report_Doc);
                            //inspect_phone = "15261277153";
                            //report_Name = "吴晓磊";
                            //report_phone = "18936979026";
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
                                        string receiveTime= bll.getReceivetime(inspect_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                        model.Update_time = receiveTime;
                                        bll2.Update2(model);
                                        string sql2 = "update BIF01021 set ReceiveTime ='" + receiveTime + "' where patientID='" + patientID + "'";
                                        DbHelperSQL.ExecuteSql(sql2);
                                    }

                                }
                                int rows = bc.getRecordCount(inspect_phone, inspect, "", patientID, 0);//获取已经发送没回复的行数
                                if (!bll2.Exists(inspect_phone, inspect, "", patientID, 1) && rows < 7)//不存在或者状态为0 发送
                                {
                                    string[] tel = new string[2];
                                    tel[0] = inspect_phone; tel[1] = "17851839909";
                                    for (int j = 0; j < tel.Length; j++)
                                    {
                                        //发送短信
                                        bool flag = bc.sendMsg(tel[j], messageContent);
                                        if (flag)
                                        {
                                            bc.writeLog(patientID, patientName, inspect, "", tel[0], inspect_Doc, 0);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int rows = bc.getRecordCount(inspect_phone, inspect, "", patientID, 0);//获取已经发送没回复的行数
                                if (hasValue && !String.IsNullOrEmpty(report_phone) && rows < 7)//状态为0 发送给报告医生
                                {
                                    
                                    //发送短信
                                    bool flag = bc.sendMsg(report_phone, "5272718510009|" + report_Name + "|" + inspect_Doc);
                                    if (flag)
                                    {
                                        bool writeOK = bc.writeLog(patientID, patientName, inspect, "", report_phone, report_Name, 2);
                                        if (writeOK)
                                        {
                                            bool s3 = bll.ExistMinute(report_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                            bool s4 = bll.ExistMinute("17851839909", beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                            if (s3 == true||s4==true)
                                            {
                                                Model.BIF01022 model3 = new Model.BIF01022();
                                                model3.Patient_id = patientID;
                                                model3.Item_name = inspect;
                                                model3.Current_result = "";
                                                model3.EmpMobileNum = inspect_phone;
                                                model3.State = 1;
                                                if (bll2.Update(model3))
                                                {
                                                    string receiveTime = bll.getReceivetime(report_phone, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), nowTime);
                                                    string sql2 = "update BIF01021 set ReceiveTime ='" + receiveTime + "' where patientID='" + patientID + "'";
                                                    DbHelperSQL.ExecuteSql(sql2);
                                                }
                                                
                                            }

                                        }

                                    }
                                }
                                if (!bll2.Exists(inspect_phone, inspect, "", patientID, 1) && rows < 7)//不存在或者状态为0 发送
                                {
                                    string[] tel = new string[2];
                                    tel[0] = inspect_phone; tel[1] = "17851839909";
                                    for (int j = 0; j < tel.Length; j++)
                                    {
                                        //发送短信
                                        bool flag = bc.sendMsg(tel[j], messageContent);
                                        if (flag)
                                        {
                                            bc.writeLog(patientID, patientName, inspect, "", tel[0], inspect_Doc, 0);
                                        }
                                    }
                                }
                            }
                        }
                        

                    }
                }
            }

            return i;
        }
        #endregion

        #region LIS危急值暂停使用
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
                    patient = s.Length > 5 ? s.Substring(0, 5) : s;
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
                                    phone = getPhoneByDiagOrgID(dr2["DiagOrgID"] + "", dr2["EmpMobileNum"] + "");

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
                                            model.sismsid = Guid.NewGuid().ToString();
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
        #endregion

        #region PACS危急值暂停使用
        public static int SendPACS()
        {
            Bll.Sms_outbox bll = new Bll.Sms_outbox();
            Bll.BIF01022 bll2 = new Bll.BIF01022();
            BaseClass bc = new BaseClass();
            int i = 0;

            DataTable dt = null;
            string _time = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = "select patientID,patientName,birthDate,inspect,reportDoc,ApplyTypeID from BIF01021 where reportDate >= '" + _time + " 00:00:00' and reportDate<='" + _time + " 23:59:59'";
            DataSet ds_report = DbHelperSQL.Query(sql);
            string patientID = "", birthDate = "", inspect = "无", patientName = "", reportDoc = "", ApplyTypeID = "", inspect_Doc = "", report_Doc = "", inspect_phone = "", report_phone = "", report_Name = "";
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
                    //string cardid= dr["cardID"].ToString();
                    //string biw = getBIW(cardid);
                    //if (biw == "BIW")
                    //{
                    //    inspect_Doc=
                    //}
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
                        inspect_phone = getPhone(inspect_Doc);//GetDoctorPhone
                        report_Doc = (inspect_phone == report_Doc) ? "" : report_Doc;
                        report_phone = getPhone(report_Doc);
                        report_Name = getName(report_Doc);
                        //inspect_phone = "15261277153";
                        //report_Name = "吴晓磊";
                        //report_phone = "18936979026";
                        Guid guid = Guid.NewGuid();
                        string id = guid.ToString();
                        string _add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string messageContent = messageID + "|" + inspect_Doc + "|" + patientName + "|" + birthDate + "|" + ApplyTypeID + "|" + inspect;
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
                                    //发送短信
                                    bool flag = bc.sendMsg(inspect_phone, messageContent);
                                    if (flag)
                                    {
                                        bc.writeLog(patientID, patientName, inspect, "", inspect_phone, inspect_Doc, 0);
                                    }
                                }
                            }
                            else
                            {
                                if (hasValue && !String.IsNullOrEmpty(report_phone))//状态为0 发送给报告医生
                                {
                                    //发送短信
                                    bool flag = bc.sendMsg(report_phone, "5272718510009|" + report_Name + "|" + inspect_Doc);
                                    if (flag)
                                    {
                                        bool writeOK = bc.writeLog(patientID, patientName, inspect, "", report_phone, report_Name, 2);
                                        if (writeOK)
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
                                    //发送短信
                                    bool flag = bc.sendMsg(inspect_phone, messageContent);
                                    if (flag)
                                    {
                                        bc.writeLog(patientID, patientName, inspect, "", inspect_phone, inspect_Doc, 0);
                                    }
                                }
                            }
                        }

                    }
                }
            }

            return i;
        }
        #endregion
        private static string getPhoneByDiagOrgID(string id,string phone)
        {
            string sql = "select * from OrgPhone where orgid='"+id+"'";
            DataSet ds=DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            if (ds!=null&&dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    return  dr["OrgTell"]+"";
                }
            }
            return phone;
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
        private static string getPhoneByDiagOrgID(string id)
        {
            string sql = "select * from OrgPhone where orgid='" + id + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            if (ds != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    return dr["OrgTell"] + "";
                }
            }
            return "";
        }
        private static string getBIW(string cardid)
        {
            WebReference.Service1 _client = new WebReference.Service1();
            return _client.PatientState(cardid);
        }

        private static string getORGID(string cardid)
        {
            WebReference.Service1 _client = new WebReference.Service1();
            return _client.getORGID(cardid);
        }
    }
}
