using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
namespace ZSN.AI.DAL
{
    public partial interface IWorkflowNodeExcutionRecordInfoManage
    {
        string SetConnectionName(string connName);
        #region tb_workflow_node_excution_record_info
        string WorkflowNodeExcutionRecordInfo_Add(WorkflowNodeExcutionRecordInfo model);
        bool WorkflowNodeExcutionRecordInfo_Update(string RecordID, ExcutionRecordStatus Status, object Outputs, object Logs);
        bool WorkflowNodeExcutionRecordInfo_Update(WorkflowNodeExcutionRecordInfo model);
        bool WorkflowNodeExcutionRecordInfo_Delete(string recordID); 
        bool WorkflowNodeExcutionRecordInfo_DeleteList(string recordIDlist);
        WorkflowNodeExcutionRecordInfo WorkflowNodeExcutionRecordInfo_DataRowToModel(DataRow row);
        WorkflowNodeExcutionRecordInfo WorkflowNodeExcutionRecordInfo_GetModel(string recordID); 
        DataSet WorkflowNodeExcutionRecordInfo_GetList(string strWhere);
        DataSet WorkflowNodeExcutionRecordInfo_GetList(int top, string strWhere, string filedOrder);
        int WorkflowNodeExcutionRecordInfo_GetRecordCount(string strWhere);
        DataSet WorkflowNodeExcutionRecordInfo_GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex);
        DataTable WorkflowNodeExcutionRecordInfo_GetListByPage(int pageSize, int pageIndex, string strWhere, out int pagetotal, out int total, int orderType, string showName, string orderKey);
        #endregion
    }
}
