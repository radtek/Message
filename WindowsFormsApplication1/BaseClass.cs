using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class BaseClass
    {
        Bll.Sms_outbox bll = new Bll.Sms_outbox();
        Bll.BIF01022 bif = new Bll.BIF01022();

        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="message">短信模板+变量</param>
        /// <returns></returns>
        public bool sendMsg(string phone,string message)
        {
            Model.Sms_outbox model = new Model.Sms_outbox();
            model.sismsid = Guid.NewGuid().ToString();
            model.extcode = "01";
            model.destaddr = phone;
            model.messagecontent = message;
            model.reqdeliveryreport = 1;
            model.msgfmt = 15;
            model.sendmethod = 2;
            model.requesttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.applicationid = "APP128";
            if (bll.Add(model))
                return true;
            return false;
        }
        #endregion

        #region 写入日志
        public bool writeLog(string Patient_id, string Patient_name, string Item_name, string Current_result, string EmpMobileNum, string EMPNAME,int State)
        {
            Model.BIF01022 model2 = new Model.BIF01022();
            model2.Patient_id = Patient_id;
            model2.Patient_name = Patient_name;
            model2.Item_name = Item_name;
            model2.Current_result = Current_result;
            model2.EmpMobileNum = EmpMobileNum;
            model2.EMPNAME = EMPNAME;
            model2.State = State;
            model2.Add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (bif.Add(model2))
                return true;
            return false;
        }
        #endregion

        #region 更新状态
        public bool updateLog(string Patient_id, string Item_name, string Current_result, string EmpMobileNum,int State,int Stateparam)
        {
            Model.BIF01022 model3 = new Model.BIF01022();
            model3.Patient_id = Patient_id;
            model3.Item_name = Item_name;
            model3.Current_result = Current_result;
            model3.EmpMobileNum = EmpMobileNum;
            model3.State = State;
            if (bif.UpdateData(model3, Stateparam))
                return true;
            return false;
        }
        #endregion

        #region 获取记录数
        public int getRecordCount(string EmpMobileNum, string Item_name, string Current_result, string Patient_id, int state)
        {
            string _time = DateTime.Now.ToString("yyyy-MM-dd");
            string strWhere = " Add_time >= '" + _time + " 00:00:00' and Add_time<='" + _time + " 23:59:59' and EmpMobileNum='" + EmpMobileNum + "' and Item_name='" + Item_name + "' and Current_result='" + Current_result + "' and Patient_id='" + Patient_id + "' and state=" + state + " ";
            int records = bif.GetRecordCount(strWhere);
            return records;
        }
        #endregion

        #region 获取温湿度记录数
        public int getRecordCount1(string EmpMobileNum, string Item_name, string Patient_id, int state)
        {
            string _time = DateTime.Now.ToString("yyyy-MM-dd");
            string strWhere = " Add_time >= '" + _time + " 00:00:00' and Add_time<='" + _time + " 23:59:59' and EmpMobileNum='" + EmpMobileNum + "' and Item_name='" + Item_name + "' and Patient_id='" + Patient_id + "' and state=" + state + " ";
            int records = bif.GetRecordCount(strWhere);
            return records;
        }
        #endregion
    }
}
