using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
using ZSN.AI.DAL;
namespace ZSN.AI.BLL
{
    public partial class BaseDictionaryInfoBussiness
    {
	    #region 基础信息
        private const string ConnectionName = "BaseDb";
        #endregion
		#region base_dictionary_info
		/// <summary>
        /// 增加一条数据
        /// </summary>
		public static int Add(BaseDictionaryInfo model)
		{
			return DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_Add(model);
		}
		/// <summary>
        /// 更新一条数据
        /// </summary>
		public static bool Update(BaseDictionaryInfo model)
		{
			return DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_Update(model);
		}
        /// <summary>
        /// 删除一条数据
        /// </summary>
		public static bool Delete(Int32 dicId)
		{
			return DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_Delete(dicId);
		}
        /// <summary>
        /// 批量删除数据
        /// </summary>
		public static bool DeleteList(string dicIdlist)
		{
			return DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_DeleteList(dicIdlist);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
		public static ZSN.AI.Entity.BaseDictionaryInfo GetModel(Int32 dicId)
		{
			return DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetModel(dicId);
		}
        /// <summary>
        /// 获得数据列表
        /// </summary>
		public static List<BaseDictionaryInfo> GetList(string strWhere = "")
        {
            return BaseDictionaryInfoDataSet_ToList(DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetList(strWhere).Tables[0]);
        }
        public static List<BaseDictionaryInfo> GetChildList(string Name = "")
        {
            return BaseDictionaryInfoDataSet_ToList(DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetChildList(Name).Tables[0]);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
		public static List<BaseDictionaryInfo> GetList(int top, string strWhere, string filedOrder)
        {
            return BaseDictionaryInfoDataSet_ToList(DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetList(top, strWhere, filedOrder).Tables[0]);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
		public static int GetRecordCount(string strWhere = "")
        {
            return DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
		public static List<BaseDictionaryInfo> GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex)
        {
            return BaseDictionaryInfoDataSet_ToList(DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetListByPage(strWhere, orderBy, startIndex, endIndex).Tables[0]);
        }
		/// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页标</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pagetotal">总页数</param>
        /// <param name="total">总数</param>
        /// <param name="orderType">排序规则， 默认降序，1降序，0升序</param>
        /// <param name="showName">显示字段，默认全部</param>
        /// <param name="orderKey">排序key，默认主键</param>
        /// <returns></returns>
		public static List<BaseDictionaryInfo> GetListByPage(int pageSize, int pageIndex, string strWhere, out int pagetotal, out int total, int orderType = 1, string showName = "*", string orderKey = "DicId")
		{
            return BaseDictionaryInfoDataSet_ToList(DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_GetListByPage(pageSize, pageIndex, strWhere, out pagetotal, out total, orderType, showName, orderKey));
        }
		private static List<BaseDictionaryInfo> BaseDictionaryInfoDataSet_ToList(DataTable dt)
		{
			var rows = dt.Rows;
            var list = new List<BaseDictionaryInfo>();
            foreach (DataRow r in rows)
            {
                list.Add(DatabaseProvider.GetBaseDictionaryInfo(ConnectionName).BaseDictionaryInfo_DataRowToModel(r));
            }
            return list;
		}
		#endregion 
	}
}
