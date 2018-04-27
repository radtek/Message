
using System;
namespace Model
{
	/// <summary>
	/// BIF01022:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BIF01022
	{
		public BIF01022()
		{}
		#region Model
		private string _patient_id;
		private string _patient_name;
		private string _item_name;
		private string _current_result;
		private string _empmobilenum;
		private string _empname;
		private int? _state;
		private string _add_time;
        private string _update_time;
        /// <summary>
        /// 
        /// </summary>
        public string Patient_id
		{
			set{ _patient_id=value;}
			get{return _patient_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Patient_name
		{
			set{ _patient_name=value;}
			get{return _patient_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Item_name
		{
			set{ _item_name=value;}
			get{return _item_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Current_result
		{
			set{ _current_result=value;}
			get{return _current_result;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EmpMobileNum
		{
			set{ _empmobilenum=value;}
			get{return _empmobilenum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EMPNAME
		{
			set{ _empname=value;}
			get{return _empname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Add_time
		{
			set{ _add_time=value;}
			get{return _add_time;}
		}
        public string Update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
        #endregion Model

    }
}

