using Elastic.Clients.Elasticsearch.Cluster;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSN.AI.Core.Interface;
using ZSN.AI.Node;
using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.AI.Service.WebHelpers;

namespace ZSN.AgentBrook.AutoJob
{
    public class NodeJob : JobBase, IJob
    {

        private readonly IChatService _chatService;
        public NodeJob(IChatService chatService) { 
            _chatService = chatService;
        }
        Task IJob.Execute(IJobExecutionContext context)
        {
            var res = Auto();
            return res;
        }
        public async Task<int> Auto()
        {
            Console.WriteLine("ZSN.AI.AutoJob.Job![JobEvent_NodeJob]");
            int num = 0;
            try
            {
                //获取需要AI执行的任务
                List<NodeType> nodeTypes = new List<NodeType>() { NodeType.Start, NodeType.AgentStart, NodeType.End, NodeType.AgentEnd, NodeType.LargeModel, NodeType.Agent, NodeType.Plugins, NodeType.MainAI, NodeType.Selector, NodeType.KnowledgeBase };
                List<TaskInfo> tasks = TaskInfoBussiness.GetList(0, nodeTypes, DateTime.Now, 1, 100);

                if (tasks != null && tasks.Count > 0)
                {
                    foreach (var task in tasks)
                    {
                        if (task != null)
                        {
                            num++;
                            await this.AIWorkerAsync_Node(task);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                num = -1;
                DefaultLogService.AddOperationLog(ErrorId, e.Message);
            }
            return await Task.FromResult(num);
        }

        private async Task AIWorkerAsync_Node(TaskInfo task)
        {
            TaskConfig taskConfig = task.TaskConfig;
            task.Results = new Results();
            try
            {
                if (taskConfig.NodeConfig != null)
                {
                    if (taskConfig.NodeConfig.data != null)
                    {
                        TaskData taskData = taskConfig.Data;
                        taskData.TaskID = task.TaskID;
                        string re = "";
                        NodeType nodeType = taskConfig.NodeConfig.type;

                        Excution excutionNode = new Excution(_chatService);
                        switch (nodeType)
                        {
                            case NodeType.Start:
                                re =excutionNode.StartNode(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.AgentStart:
                                re = excutionNode.AgentStartNode(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.End:
                                re = await excutionNode.EndNodeAsync(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.AgentEnd:
                                re = await excutionNode.AgentEndNodeAsync(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.Agent:
                                re = await excutionNode.AgentNodeAsync(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.Plugins:
                                re = await excutionNode.PluginsNodeAsync(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.KnowledgeBase:
                                re = await excutionNode.KnowledgeBaseNodeAsync(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.MainAI:
                                re = await excutionNode.MainAINodeAsync(taskConfig.NodeConfig, taskData);
                                break;
                            case NodeType.Selector:
                                re = await excutionNode.SelectorNodeAsync(taskConfig.NodeConfig, taskData);
                                break;

                        }
                        task.Results.Data = re;
                        task.State = TaskState.Completed;
                    }
                }
            }
            catch (Exception ex)
            {
                
                task.Results.Data = ex;
                task.State = TaskState.Failure;
            }
            task.UpdateTime = DateTime.Now;
            TaskInfoBussiness.Update(task);
        }
    }
}
