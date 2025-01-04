using System;
using System.Collections.Generic;
using System.Data;
using ZSN.AI.Entity;
namespace ZSN.AI.DAL
{
    public partial class DatabaseProvider
    {
        private static IWorkflowNodeExcutionRecordInfoManage WorkflowNodeExcutionRecordInfoInstance;
        private static readonly object WorkflowNodeExcutionRecordInfoLockObj = new object();
        public static IWorkflowNodeExcutionRecordInfoManage GetWorkflowNodeExcutionRecordInfo(string connectionName)
        {
            if (WorkflowNodeExcutionRecordInfoInstance == null)
            {
                lock (WorkflowNodeExcutionRecordInfoLockObj)
                {
                    if (WorkflowNodeExcutionRecordInfoInstance == null)
                    {
                        GetWorkflowNodeExcutionRecordInfoProvider(connectionName);
                    }
                }
            }
            return WorkflowNodeExcutionRecordInfoInstance;
        }
        private static void GetWorkflowNodeExcutionRecordInfoProvider(string connectionName)
        {
            try
            {
                var db = DbConfig.GetDbInfo(connectionName);
                var type = Type.GetType(
                    $"ZSN.AI.DAL." + db.DbType + ".WorkflowNodeExcutionRecordInfoManage, ZSN.AI.DAL." + db.DbType,
                    false, true);
                var provider = (IWorkflowNodeExcutionRecordInfoManage)Activator.CreateInstance(type);
	provider.SetConnectionName(connectionName);
                WorkflowNodeExcutionRecordInfoInstance = provider;
            }
            catch (Exception e)
            {
                throw new DbException();
            }
        }
    }
}
