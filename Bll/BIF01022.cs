
using System;
using System.Data;
using System.Collections.Generic;

namespace Bll
{
	/// <summary>
	/// BIF01022
	/// </summary>
	public class BIF01022
	{
		private readonly DAL.BIF01022 dal=new DAL.BIF01022();
		public BIF01022()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.BIF01022 model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.BIF01022 model)
		{
			return dal.Update(model);
		}

        public bool Update2(Model.BIF01022 model)
        {
            return dal.Update2(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.BIF01022 GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel();
		}


		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.BIF01022> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.BIF01022> DataTableToList(DataTable dt)
		{
			List<Model.BIF01022> modelList = new List<Model.BIF01022>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.BIF01022 model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  ExtensionMethod
        public bool Exists(string EmpMobileNum, string Item_name, string Current_result, string Patient_id,int state)
        {
            return dal.Exists(EmpMobileNum,Item_name,Current_result,Patient_id, state);
        }

        public string getUpdate_time(string EmpMobileNum, string Item_name, string Current_result, string Patient_id, int state)
        {
            return dal.getUpdate_time(EmpMobileNum, Item_name, Current_result, Patient_id, state);
        }

        public string GetState(string EmpMobileNum, string Item_name, string Current_result, string Patient_id, int state)
        {
            return dal.GetState(EmpMobileNum,Item_name,Current_result,Patient_id,state);
        }
            #endregion  ExtensionMethod
        }
}

