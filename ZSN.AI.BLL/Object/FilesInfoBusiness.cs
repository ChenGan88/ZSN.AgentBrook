using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
using ZSN.AI.DAL;
using ZSN.Utils.Core.Utils;

namespace ZSN.AI.BLL
{
    public partial class FilesInfoBussiness
    {
	    #region 基础信息
        private const string ConnectionName = "ObjectDb";
        #endregion
		#region tbFilesInfo
		/// <summary>
        /// 增加一条数据
        /// </summary>
		public static int Add(FilesInfo model)
		{
			return DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_Add(model);
		}
		/// <summary>
        /// 更新一条数据
        /// </summary>
		public static bool Update(FilesInfo model)
		{
			return DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_Update(model);
		}
        /// <summary>
        /// 删除一条数据
        /// </summary>
		public static bool Delete(string FilesCode)
		{
			return DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_Delete(FilesCode);
		}
        /// <summary>
        /// 批量删除数据
        /// </summary>
		public static bool DeleteList(string FilesCodelist)
		{
            if (FilesCodelist.Trim() != "")
            {
                FilesCodelist = ZSN.Utils.Core.Utils.StringUtil.QuoteSeparatedItems(FilesCodelist, ',', '\'');

                return DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_DeleteList(FilesCodelist);
            }
            else
            {
                return false;
            }
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
		public static ZSN.AI.Entity.FilesInfo GetModel(string FilesCode)
		{
			return DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_GetModel(FilesCode);
		}
        /// <summary>
        /// 获得数据列表
        /// </summary>
		public static List<FilesInfo> GetList(string strWhere = "")
        {
            return FilesInfoDataSet_ToList(DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_GetList(strWhere).Tables[0]);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
		public static List<FilesInfo> GetList(int top, string strWhere, string filedOrder)
        {
            return FilesInfoDataSet_ToList(DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_GetList(top, strWhere, filedOrder).Tables[0]);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
		public static int GetRecordCount(string strWhere = "")
        {
            return DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
		public static List<FilesInfo> GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex)
        {
            return FilesInfoDataSet_ToList(DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_GetListByPage(strWhere, orderBy, startIndex, endIndex).Tables[0]);
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
		public static List<FilesInfo> GetListByPage(int pageSize, int pageIndex, string strWhere, out int pagetotal, out int total, int orderType = 1, string showName = "*", string orderKey = "FilesCode")
		{
            return FilesInfoDataSet_ToList(DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_GetListByPage(pageSize, pageIndex, strWhere, out pagetotal, out total, orderType, showName, orderKey));
        }
		private static List<FilesInfo> FilesInfoDataSet_ToList(DataTable dt)
		{
			var rows = dt.Rows;
            var list = new List<FilesInfo>();
            foreach (DataRow r in rows)
            {
                list.Add(DatabaseProvider.GetFilesInfo(ConnectionName).FilesInfo_DataRowToModel(r));
            }
            return list;
		}
		#endregion 
	}
}
