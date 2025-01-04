using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tsavorite.core;
using ZSN.AI.Entity.Model;
namespace ZSN.AI.Entity
{
    public enum LoopType
    {
        NOLoop = 0,
        Second = 1,
        Day = 2,
        Week = 3,
        Month = 4
    }

    public enum TaskState
    {
        Waiting = 0,
        Processing = 1,
        Completed = 2,
        Failure = -1
    }

    /// <summary>
    /// tb_task_info
    /// </summary>
    public partial class TaskInfo
    {
        public TaskInfo() { }
        #region AutoField
        /// <summary>
        /// TaskID
        /// </summary>
        public string TaskID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// TaskType
        /// </summary>
        public NodeType TaskType { get; set; }
        /// <summary>
        /// TaskConfig
        /// </summary>
        public TaskConfig TaskConfig { get; set; } = new TaskConfig();
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// UpdateTime
        /// </summary>
		public DateTime UpdateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// State
        /// </summary>
        public TaskState State { get; set; } = TaskState.Waiting;
        /// <summary>
        /// Results
        /// </summary>
        public Results Results { get; set; } = new Results();

        public LoopType LoopType { get; set; } = 0;
        public IntervalValue IntervalValue { get; set; } = new IntervalValue();
        public int RepeatValue { get; set; } = 0;
        public int RedoCount { get; set; } = 0;

        public string FromTaskID { get; set; } = string.Empty;
        public string FromMainTaskID {  get; set; } = string.Empty;

        #endregion
    }

    public partial class Results
    {
        public Results() { }
        public object Data { get; set; }
    }
    public partial class ChatSummaryData
    {
        public ChatSummaryData() { }
        public string SummaryID { get; set; }
    }
    public partial class TaskData
    {
        public TaskData() { }
        public string AppID { get; set; }
        public string TaskID { get; set; }
        public string SessionID { get; set; }
        public string ProcessesID {  get; set; }
        /// <summary>
        /// 在APP工作流中定义的Agent节点的NodeID
        /// </summary>
        public string AgentNodeID { get; set; }
        public List<Inputs> Inputs { get; set; } = new List<Inputs>();

        public string FromMainTaskID { get; set; } = string.Empty;
    }
    public partial class TaskConfig
    {
        public TaskConfig() { }
        public NodeConfig NodeConfig { get; set; } = new NodeConfig();

        public object? NotNodeConfig {  get; set; }

        public T NodeData<T>(T defaultValue = default(T))
        {

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(this.NodeConfig.data));
        }
        public TaskData Data { get; set; } = new TaskData();
    }
    public partial class IntervalValue
    { 
        public IntervalValue() { }

        public List<int> Value { get; set; }
    }

    public partial class FileChunkConfig
    {
        public FileChunkConfig() { }
        public string KnowledgeBaseID { get; set; }
        public string FileID { get; set; }

        public ImportKMSTaskReq ImportKMSTask { get; set; }
    }
}
