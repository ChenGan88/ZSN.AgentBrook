using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZSN.AgentBrook.AutoJob;
using ZSN.Utils.Core.Helpers;
using static Quartz.Logging.OperationName;

namespace ZSN.AgentBrook.AutoJob
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public QuartzHostedService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var _jobs = ConfigHelper.GetSection("Job").GetChildren().ToArray();
            int LoopTimerSeconds = 0;

            foreach (var _job in _jobs)
            {
                if (_job.GetSection("JobName").Value == "TimeTrigger")
                {
                    LoopTimerSeconds = int.Parse(_job.GetSection("LoopTimerSeconds").Value??"1000");
                    var _Worker = JobBuilder.Create<TimeTrigger>().Build();
                    var _Worker_trigger = TriggerBuilder.Create().StartNow().WithIdentity("JobEvent_TimeTrigger", "JobEvent_TimeTrigger").WithSimpleSchedule(t => t.WithIntervalInSeconds(LoopTimerSeconds).RepeatForever()).Build();
                    // 将任务添加到调度器
                    await _scheduler.ScheduleJob(_Worker, _Worker_trigger, cancellationToken);
                    // 启动调度器
                    await _scheduler.Start(cancellationToken);
                }
                if (_job.GetSection("JobName").Value == "AIDispatcher")
                {
                    LoopTimerSeconds = int.Parse(_job.GetSection("LoopTimerSeconds").Value ?? "1000");
                    var _Worker = JobBuilder.Create<AIDispatcher>().Build();
                    var _Worker_trigger = TriggerBuilder.Create().StartNow().WithIdentity("JobEvent_AIDispatcher", "JobEvent_AIDispatcher").WithSimpleSchedule(t => t.WithIntervalInSeconds(LoopTimerSeconds).RepeatForever()).Build();
                    // 将任务添加到调度器
                    await _scheduler.ScheduleJob(_Worker, _Worker_trigger, cancellationToken);
                    // 启动调度器
                    await _scheduler.Start(cancellationToken);
                }
                if (_job.GetSection("JobName").Value == "FileChunk")
                {
                    LoopTimerSeconds = int.Parse(_job.GetSection("LoopTimerSeconds").Value ?? "1000");
                    var _Worker = JobBuilder.Create<FileChunkJob>().Build();
                    var _Worker_trigger = TriggerBuilder.Create().StartNow().WithIdentity("JobEvent_FileChunk", "JobEvent_FileChunk").WithSimpleSchedule(t => t.WithIntervalInSeconds(LoopTimerSeconds).RepeatForever()).Build();
                    // 将任务添加到调度器
                    await _scheduler.ScheduleJob(_Worker, _Worker_trigger, cancellationToken);
                    // 启动调度器
                    await _scheduler.Start(cancellationToken);
                }
                if (_job.GetSection("JobName").Value == "Node")
                {
                    LoopTimerSeconds = int.Parse(_job.GetSection("LoopTimerSeconds").Value ?? "1000");
                    var _Worker = JobBuilder.Create<NodeJob>().Build();
                    var _Worker_trigger = TriggerBuilder.Create().StartNow().WithIdentity("JobEvent_Node", "JobEvent_Node").WithSimpleSchedule(t => t.WithIntervalInSeconds(LoopTimerSeconds).RepeatForever()).Build();
                    // 将任务添加到调度器
                    await _scheduler.ScheduleJob(_Worker, _Worker_trigger, cancellationToken);
                    // 启动调度器
                    await _scheduler.Start(cancellationToken);
                }
            }

            Console.WriteLine("Quartz Scheduler started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // 停止调度器
            await _scheduler.Shutdown(cancellationToken);
            Console.WriteLine("Quartz Scheduler stopped.");
        }
    }
}