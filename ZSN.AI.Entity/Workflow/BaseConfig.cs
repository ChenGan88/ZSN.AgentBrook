using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
using ZSN.AI.Entity.Model;

namespace ZSN.AI.Entity.Workflow
{
    public partial class BaseConfig
    {
        public BaseConfig() { }
        public List<LargeModelInfo> largeModelList { get; set; } = new List<LargeModelInfo>();
        public List<KnowledgeBaseInfo> knowledgeBaseList { get; set; } = new List<KnowledgeBaseInfo>();
        public List<PluginsInfo> pluginsList { get; set; } = new List<PluginsInfo>();
        public List<AgentInfo> agentList { get; set; } = new List<AgentInfo>();
    }
}
