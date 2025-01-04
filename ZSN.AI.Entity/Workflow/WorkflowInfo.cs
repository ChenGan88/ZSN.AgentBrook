using NPOI.HSSF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
namespace ZSN.AI.Entity
{
    /// <summary>
    /// tb_workflow_info
    /// </summary>
    public partial class WorkflowInfo
    {
        public WorkflowInfo() { }
        #region AutoField
        /// <summary>
        /// WorkflowID
        /// </summary>
        public string WorkflowID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// MainType
        /// </summary>
        public Int32 MainType { get; set; } = 0;
        /// <summary>
        /// MainID
        /// </summary>
        public string MainID { get; set; }
        public string WorkflowName { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// SystemStatus
        /// </summary>
        public Int32? SystemStatus { get; set; } = 0;
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime? LastUpdateTime { get; set; } = DateTime.Now;
        #endregion
    }

    public partial class WorkFlow
    {
        public WorkFlow() { }
        public string WorkflowID { get; set; } = "";
        public string MainID { get; set; } = "";
        public int MainType { get; set; } = 0;
        public WorkflowInfo Info { get; set; } = new WorkflowInfo();
        public List<WorkflowNodeInfo> Nodes { get; set; } = new List<WorkflowNodeInfo>();
        public List<WorkflowEdgeInfo> Edges { get; set; } = new List<WorkflowEdgeInfo>();
    }

    public partial class WorkFlowProcesses
    {
        public WorkFlowProcesses() { }
        public string WorkflowID { get; set; }
        public string ProcessesID { get; set; }
    }
}


