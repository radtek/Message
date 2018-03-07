using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Commmon;

namespace Dal
{
    public class Sms_outbox
    {
        public bool Add(Model.Sms_outbox model)
        {
            string sql = "insert into sms_outbox (sismsid, extcode, destaddr, messagecontent, reqdeliveryreport,msgfmt, sendmethod, requesttime, applicationid)VALUES(?sismsid, ?extcode, ?destaddr, ?messagecontent, ?reqdeliveryreport,?msgfmt, ?sendmethod, ?requesttime, ?applicationid)";
            //string sql = "insert into S_Admin(UserName,Password,Remark,Mail,DepartId,Power)values(?UserName,?Password,?Remark,?Mail,?DepartId,?Power)";
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = DbHelperMySQL.connectionString;//此处设置链接字符串
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.Add("?sismsid", MySqlDbType.VarChar, 100).Value = model.sismsid;
            command.Parameters.Add("?extcode", MySqlDbType.VarChar, 100).Value = model.extcode;
            command.Parameters.Add("?destaddr", MySqlDbType.VarChar, 100).Value = model.destaddr;
            command.Parameters.Add("?messagecontent", MySqlDbType.VarChar, 100).Value = model.messagecontent;
            command.Parameters.Add("?reqdeliveryreport", MySqlDbType.Int32, 4).Value = model.reqdeliveryreport;
            command.Parameters.Add("?msgfmt", MySqlDbType.Int32, 4).Value = model.msgfmt;
            command.Parameters.Add("?sendmethod", MySqlDbType.Int32, 4).Value = model.sendmethod;
            command.Parameters.Add("?requesttime", MySqlDbType.VarChar, 100).Value = model.requesttime;
            command.Parameters.Add("?applicationid", MySqlDbType.VarChar, 100).Value = model.applicationid;
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            return rowsAffected > 0;
        }

        public bool Exists(string extcode,string phone)
        {
            string add_time=DateTime.Now.ToString("yyyy-MM-dd");
            //string add_time = "2017-10-30";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from sms_inbox");
            strSql.Append(" where extcode=@extcode and sourceaddr=@sourceaddr and DATE_FORMAT(receivetime,'%Y-%m-%d')=@receivetime  ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@extcode", MySqlDbType.VarChar,100),
                    new MySqlParameter("@sourceaddr", MySqlDbType.VarChar,100),
                    new MySqlParameter("@receivetime", MySqlDbType.VarChar,100)};
            parameters[0].Value = extcode;
            parameters[1].Value = phone;
            parameters[2].Value = add_time;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }
        public string getSentSms()
        {
            string sql = "select requesttime from sms_sent order by requesttime DESC limit 1 ";
            return DbHelperMySQL.GetSingle(sql) + "";
        }
        
        public bool ExistMinute(string phone,string beginTime,string endTime)
        {
            string sql = "select count(1) from sms_inbox where extcode='01' and sourceaddr='"+phone+ "' and receivetime between '" + beginTime+ "' and '" + endTime + "' ";
            return DbHelperMySQL.Exists(sql);
        }

    }
}
