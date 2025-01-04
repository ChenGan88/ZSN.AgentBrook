using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
using ZSN.AI.DAL;
namespace ZSN.AI.BLL
{
    public partial class WorkflowNodeExcutionRecordInfoBussiness
    {
	    #region 基础信息
        private const string ConnectionName = "WorkflowDb";
        #endregion
		#region tb_workflow_node_excution_record_info
		/// <summary>
        /// 增加一条数据
        /// </summary>
		public static string Add(WorkflowNodeExcutionRecordInfo model)
		{
			return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_Add(model);
		}
        public static bool Update(string RecordID, ExcutionRecordStatus Status, object Outputs, object Logs)
        {
            return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_Update( RecordID,  Status,  Outputs,  Logs);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(WorkflowNodeExcutionRecordInfo model)
		{
			return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_Update(model);
		}
        /// <summary>
        /// 删除一条数据
        /// </summary>
		public static bool Delete(string recordID)
		{
			return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_Delete(recordID);
		}
        /// <summary>
        /// 批量删除数据
        /// </summary>
		public static bool DeleteList(string recordIDlist)
		{
			return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_DeleteList(recordIDlist);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
		public static ZSN.AI.Entity.WorkflowNodeExcutionRecordInfo GetModel(string recordID)
		{
			return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetModel(recordID);
		}
        public static List<WorkflowNodeExcutionRecordInfo> GetListBySessionIDProcessesID(string SessionID,string ProcessesID)
        {
            string strWhere = $" SessionID='{SessionID}' and ProcessesID='{ProcessesID}' order by StartTime asc";
            return WorkflowNodeExcutionRecordInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetList(strWhere).Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
		public static List<WorkflowNodeExcutionRecordInfo> GetList(string strWhere = "")
        {
            return WorkflowNodeExcutionRecordInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetList(strWhere).Tables[0]);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
		public static List<WorkflowNodeExcutionRecordInfo> GetList(int top, string strWhere, string filedOrder)
        {
            return WorkflowNodeExcutionRecordInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetList(top, strWhere, filedOrder).Tables[0]);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
		public static int GetRecordCount(string strWhere = "")
        {
            return DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
		public static List<WorkflowNodeExcutionRecordInfo> GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex)
        {
            return WorkflowNodeExcutionRecordInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetListByPage(strWhere, orderBy, startIndex, endIndex).Tables[0]);
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
		public static List<WorkflowNodeExcutionRecordInfo> GetListByPage(int pageSize, int pageIndex, string strWhere, out int pagetotal, out int total, int orderType = 1, string showName = "*", string orderKey = "RecordID")
		{
            return WorkflowNodeExcutionRecordInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_GetListByPage(pageSize, pageIndex, strWhere, out pagetotal, out total, orderType, showName, orderKey));
        }
		private static List<WorkflowNodeExcutionRecordInfo> WorkflowNodeExcutionRecordInfoDataSet_ToList(DataTable dt)
		{
			var rows = dt.Rows;
            var list = new List<WorkflowNodeExcutionRecordInfo>();
            foreach (DataRow r in rows)
            {
                list.Add(DatabaseProvider.GetWorkflowNodeExcutionRecordInfo(ConnectionName).WorkflowNodeExcutionRecordInfo_DataRowToModel(r));
            }
            return list;
		}
		#endregion 
	}
}
