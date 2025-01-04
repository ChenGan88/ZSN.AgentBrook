using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.Json.Nodes;
using ZSN.AI.Entity;
using static Org.BouncyCastle.Math.Primes;
namespace ZSN.AI.Entity
{
    /// <summary>
    /// tb_workflow_node_info
    /// </summary>
    public partial class WorkflowNodeInfo
    {
        public WorkflowNodeInfo() { }
        #region AutoField
        /// <summary>
        /// NodeID
        /// </summary>
        public string NodeID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// WorkflowID
        /// </summary>
        public string WorkflowID { get; set; } = string.Empty;
        /// <summary>
        /// NodeType
        /// </summary>
        public NodeType NodeType { get; set; }
        /// <summary>
        /// NodeName
        /// </summary>
        public string NodeName { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Config
        /// </summary>
        public object? Config { get; set; }

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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum NodeType
    {
        Start = 1,
        End = 2,
        LargeModel = 3,
        MainAI = 4,
        KnowledgeBase = 5,
        Selector = 6,
        Reporter = 7,
        TimeTrigger = 8,
        Agent = 9,
        Plugins = 10,
        AgentStart = 11,
        AgentEnd = 12,

        NotNode_FileChunk = 90,
    }
    public partial class NodeConfig
    {
        public NodeConfig() { }
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; }
        public string mainid { get; set; } = string.Empty;
        public string workflowid { get; set; } = string.Empty;
        public NodeType type { get; set; }
        public object data { get; set; }
        public Position position { get; set; } = new Position();
        //上一节点类型
        public NodeType fromNodeType { get;set; }
    }

    public partial class Position
    {
        public Position() { }
        public decimal x { get; set; }
        public decimal y { get; set; }
    }

    public partial class Inputs
    {
        public Inputs() { }
        public string id { get; set;}
        public string sourceId { get; set; }
        public string varname { get; set; } = "input";
        public string value { get; set; } = string.Empty;
        public string type { get; set; } = "string";
        public string txt { get; set; } = "";
    }

    public partial class Output
    {
        public Output() { }
        public string id { get; set; }
        public string varname { get; set; } = "output";
        public string value { get; set; } = string.Empty;
        public string type { get; set; } = "string";
        public string txt { get; set; } = "";
    }

    public partial class Selector
    {
        public Selector() { }
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string varname { get; set; } = "input";
        public string comparison { get; set; }
        public string value { get; set; } = "";
        public int top { get; set; }
    }

    public partial class IntervalConfig
    {
        public IntervalConfig() { }
        /// <summary>
        /// 周期类型
        /// 每隔n秒=s
        /// 每隔n天=d
        /// 每周星期几=w
        /// 每月第几日=m
        /// </summary>
        public string LoopType { get; set; }
        /// <summary>
        /// 执行周期
        /// </summary>
        public int Interval { get; set; } = 3600;
        /// <summary>
        /// 首次执行开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 执行次数，0=无限
        /// </summary>
        public int Repeat { get; set; } = 0;
        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        public DateTime LastRunTime { get; set; } = DateTime.Now;
    }

    public partial class TimeTrigger: IntervalConfig
    {
        public TimeTrigger() { }
        public string id { get; set; } = Guid.NewGuid().ToString();
        public int top { get; set; }
    }

    public partial class StartData
    {
        public StartData() { }
        public string label { get; set; } = "Start";
        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public List<Output> output { get; set; } = new List<Output>();
        public string prompt { get; set; } = string.Empty;
    }

    public partial class AgentStartData : StartData
    {
        public AgentStartData() { }
    }

    public partial class EndData
    {
        public EndData() { }
        public string label { get; set; } = "End";
        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public string prompt { get; set; } = string.Empty;
    }
    public partial class AgentEndData : EndData
    {
        public AgentEndData() { }
        public List<Output> output { get; set; } = new List<Output>();
    }

    public partial class LargeModelData
    {
        public LargeModelData() { }
        public string label { get; set; } = "Large Model";
        public LargeModelInfo model { get; set; } = new LargeModelInfo();
        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public string prompt { get; set; } = string.Empty;
        public int temperature { get; set; } = 70;
        public int topp { get; set; } = 80;
        public List<Output> output { get; set; } = new List<Output>();

        public List<PluginsInfo> SemanticFunction { get; set; } = new List<PluginsInfo>();
        public List<PluginsInfo> NativeFunction { get; set; } = new List<PluginsInfo>();
    }

    public partial class MainAIData : LargeModelData
    {

        public MainAIData() { }
        public string label { get; set; } = "Main AI";
    }

    public partial class ReporterData : LargeModelData
    {
        public ReporterData() { }
        public string label { get; set; } = "Reporter";

        /// <summary>
        /// 一次摘要记录条数
        /// </summary>
        public int recordslength { get; set; } = 50;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool enable { get; set; } = true;
    }

    public partial class KnowledgeBaseData: LargeModelData
    {
        public KnowledgeBaseData() { }
        public string label { get; set; } = "Knowledge Base";
        public List<KnowledgeBaseInfo> knowledgeBase { get; set; } = new List<KnowledgeBaseInfo>();
        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public int relevance { get; set; } = 70;
        public List<Output> output { get; set; } = new List<Output>();
    }

    public partial class PluginsData
    {
        public PluginsData() { }
        public string label { get; set; } = "Plugins";
        public PluginsInfo plugins { get; set; } = new PluginsInfo();
        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public List<Output> output { get; set; } = new List<Output>();
    }

    public partial class SelectorData
    {
        public SelectorData() { }
        public string label { get; set; } = "Selector";
        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public List<Output> output { get; set; } = new List<Output>();
        public List<Selector> selector { get; set; } = new List<Selector>();
    }

    public partial class TimeTriggerData
    {
        public TimeTriggerData() { }
        public string label { get; set; } = "TimeTrigger";
        public string prompt { get; set; } = string.Empty;
        public TimeTrigger timeTrigger { get; set; } = new TimeTrigger();
        public List<Output> output { get; set; } = new List<Output>();
    }

    public partial class AgentData
    {
        public AgentData() { }
        public string label { get; set; } = "Agent";

        public AgentInfo agent { get; set; } = new AgentInfo();

        public List<Inputs> inputs { get; set; } = new List<Inputs>();
        public List<Output> output { get; set; } = new List<Output>();
    }
}