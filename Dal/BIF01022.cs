
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Commmon;

namespace DAL
{
	/// <summary>
	/// 数据访问类:BIF01022
	/// </summary>
	public partial class BIF01022
	{
		public BIF01022()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.BIF01022 model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BIF01022(");
			strSql.Append("Patient_id,Patient_name,Item_name,Current_result,EmpMobileNum,EMPNAME,State,Add_time)");
			strSql.Append(" values (");
			strSql.Append("@Patient_id,@Patient_name,@Item_name,@Current_result,@EmpMobileNum,@EMPNAME,@State,@Add_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@Patient_id", SqlDbType.VarChar,50),
					new SqlParameter("@Patient_name", SqlDbType.VarChar,50),
					new SqlParameter("@Item_name", SqlDbType.VarChar,50),
					new SqlParameter("@Current_result", SqlDbType.VarChar,50),
					new SqlParameter("@EmpMobileNum", SqlDbType.VarChar,50),
					new SqlParameter("@EMPNAME", SqlDbType.VarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Add_time", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Patient_id;
			parameters[1].Value = model.Patient_name;
			parameters[2].Value = model.Item_name;
			parameters[3].Value = model.Current_result;
			parameters[4].Value = model.EmpMobileNum;
			parameters[5].Value = model.EMPNAME;
			parameters[6].Value = model.State;
			parameters[7].Value = model.Add_time;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.BIF01022 model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BIF01022 set ");
			//strSql.Append("Patient_id=@Patient_id,");
			//strSql.Append("Patient_name=@Patient_name,");
			//strSql.Append("Item_name=@Item_name,");
			//strSql.Append("Current_result=@Current_result,");
			//strSql.Append("EmpMobileNum=@EmpMobileNum,");
			//strSql.Append("EMPNAME=@EMPNAME,");
			strSql.Append("State=@State ");
			//strSql.Append("Add_time=@Add_time");
			strSql.Append(" where EmpMobileNum=@EmpMobileNum and Item_name=@Item_name and Current_result=@Current_result and Patient_id=@Patient_id and state=0 ");
			SqlParameter[] parameters = {
					new SqlParameter("@Patient_id", SqlDbType.VarChar,50),
					new SqlParameter("@Patient_name", SqlDbType.VarChar,50),
					new SqlParameter("@Item_name", SqlDbType.VarChar,50),
					new SqlParameter("@Current_result", SqlDbType.VarChar,50),
					new SqlParameter("@EmpMobileNum", SqlDbType.VarChar,50),
					new SqlParameter("@EMPNAME", SqlDbType.VarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Add_time", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Patient_id;
			parameters[1].Value = model.Patient_name;
			parameters[2].Value = model.Item_name;
			parameters[3].Value = model.Current_result;
			parameters[4].Value = model.EmpMobileNum;
			parameters[5].Value = model.EMPNAME;
			parameters[6].Value = model.State;
			parameters[7].Value = model.Add_time;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BIF01022 ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.BIF01022 GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Patient_id,Patient_name,Item_name,Current_result,EmpMobileNum,EMPNAME,State,Add_time from BIF01022 ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Model.BIF01022 model=new Model.BIF01022();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.BIF01022 DataRowToModel(DataRow row)
		{
			Model.BIF01022 model=new Model.BIF01022();
			if (row != null)
			{
				if(row["Patient_id"]!=null)
				{
					model.Patient_id=row["Patient_id"].ToString();
				}
				if(row["Patient_name"]!=null)
				{
					model.Patient_name=row["Patient_name"].ToString();
				}
				if(row["Item_name"]!=null)
				{
					model.Item_name=row["Item_name"].ToString();
				}
				if(row["Current_result"]!=null)
				{
					model.Current_result=row["Current_result"].ToString();
				}
				if(row["EmpMobileNum"]!=null)
				{
					model.EmpMobileNum=row["EmpMobileNum"].ToString();
				}
				if(row["EMPNAME"]!=null)
				{
					model.EMPNAME=row["EMPNAME"].ToString();
				}
				if(row["State"]!=null && row["State"].ToString()!="")
				{
					model.State=int.Parse(row["State"].ToString());
				}
				if(row["Add_time"]!=null)
				{
					model.Add_time=row["Add_time"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Patient_id,Patient_name,Item_name,Current_result,EmpMobileNum,EMPNAME,State,Add_time ");
			strSql.Append(" FROM BIF01022 ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" Patient_id,Patient_name,Item_name,Current_result,EmpMobileNum,EMPNAME,State,Add_time ");
			strSql.Append(" FROM BIF01022 ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM BIF01022 ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T. desc");
			}
			strSql.Append(")AS Row, T.*  from BIF01022 T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "BIF01022";
			parameters[1].Value = "";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod
        public bool Exists(string EmpMobileNum,string Item_name,string Current_result, string Patient_id,int state)
        {
            string sql = "select * from BIF01022 where EmpMobileNum='"+ EmpMobileNum + "' and Item_name='"+ Item_name + "' and Current_result='"+Current_result+"' and Patient_id='"+Patient_id+"' and state="+state+" ";
            return DbHelperSQL.Exists(sql);
        }

        public string GetState(string EmpMobileNum, string Item_name, string Current_result, string Patient_id, int state)
        {
            string sql = "select state from BIF01022 where EmpMobileNum='" + EmpMobileNum + "' and Item_name='" + Item_name + "' and Current_result='" + Current_result + "' and Patient_id='" + Patient_id + "' and state=" + state + " ";
            return DbHelperSQL.GetSingle(sql)+"";
        }
        #endregion  ExtensionMethod
    }
}

