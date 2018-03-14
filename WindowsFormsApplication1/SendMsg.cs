using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Commmon;

namespace WindowsFormsApplication1
{
    public class SendMsg
    {
        
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
                                                return i++;
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

    }
}
