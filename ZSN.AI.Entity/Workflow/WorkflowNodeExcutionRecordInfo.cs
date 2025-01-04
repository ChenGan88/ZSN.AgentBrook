using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace ZSN.AI.Entity
{
    public enum ExcutionRecordStatus
    {
        Fail = -1,
        Running = 0,
        Success = 1
    }
    public enum ProcessStatus
    {
        Fail = -1,
        Running = 0,
        Success = 1
    }
    /// <summary>
    /// tb_workflow_node_excution_record_info
    /// </summary>
    public partial class WorkflowNodeExcutionRecordInfo
    {
        public WorkflowNodeExcutionRecordInfo() { }
        #region AutoField
		/// <summary>
        /// RecordID
        /// </summary>
		public string RecordID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// SessionID
        /// </summary>
        public string SessionID { get; set; } = string.Empty;
        /// <summary>
        /// ProcessesID
        /// </summary>
        public string ProcessesID {  get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// WorkflowID
        /// </summary>
        public string WorkflowID { get; set; } = string.Empty;
		/// <summary>
        /// NodeID
        /// </summary>
		public string NodeID { get; set; } = string.Empty;
        /// <summary>
        /// NodeName
        /// </summary>
        public string NodeName { get; set; } = string.Empty;
        /// <summary>
        /// NextNodeID
        /// </summary>
        public string NextNodeID { get; set; } = string.Empty;
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime? StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime? EndTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Status
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ExcutionRecordStatus Status { get; set; } 
		/// <summary>
        /// Inputs
        /// </summary>
		public object Inputs { get; set; } 
		/// <summary>
        /// Outputs
        /// </summary>
		public object Outputs { get; set; } 
		/// <summary>
        /// Logs
        /// </summary>
		public object Logs { get; set; } 
        #endregion
    }

    public partial class ProcessInfo { 
        public ProcessInfo() { }
        public string ProcessID { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProcessStatus Status { get; set; }

        public object Results {  get; set; }
        public List<WorkflowNodeExcutionRecordInfo> ExcutionRecordInfos { get; set; }
    }
}
