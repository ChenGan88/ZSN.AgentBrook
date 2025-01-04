using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSN.AgentBrook.AutoJob
{
    public class JobFactory: IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_serviceProvider.GetRequiredService(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            // 可以选择在这里处理 Job 的销毁逻辑
        }
    }
}
