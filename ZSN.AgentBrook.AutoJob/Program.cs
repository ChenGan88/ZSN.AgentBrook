using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Topshelf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using ZSN.Utils.Core.Helpers;
using ZSN.AgentBrook.AutoJob;
using System.Reflection;
using ZSN.AI.Core.Common.DependencyInjection;
using ZSN.AI.Core.Service;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ZSN.AI.Service.Base;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ZSN.AgentBrook.AutoJob
{
    //xx.exe install
    //xx.exe uninstall
    public class Program
    {
        public static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<MainService>(s =>
                {
                    s.ConstructUsing(name => new MainService(args));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("ZSN.AgentBrook.Job");
                x.SetDisplayName("ZSN.AgentBrook.Job");
                x.SetServiceName("ZSN.AgentBrook.Job");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;

        }
    }
    public class MainService
    {
        private string[] args;
        public MainService(string[] vs)
        {
            args = vs;
        }
        public  void Start()
        {
            var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // 注册自定义 JobFactory
                services.AddSingleton<IJobFactory, JobFactory>();

                // 注册 Quartz 调度器
                services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                services.AddSingleton(provider =>
                {
                    var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();
                    var scheduler = schedulerFactory.GetScheduler().Result;

                    // 使用自定义 JobFactory
                    scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();
                    return scheduler;
                });

                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                        });
                });

                // 从程序集加载类型并注册到容器
                services.AddServicesFromAssemblies("ZSN.AI.Core");
                services.AddServicesFromAssemblies("ZSN.AI.Plugins");

                services.AddServicesFromAssemblies("ZSN.AgentBrook.AutoJob");

                services.AddTransient<AIDispatcher>();
                services.AddTransient<TimeTrigger>();
                services.AddTransient<FileChunkJob>();
                services.AddTransient<NodeJob>();

                services.AddSingleton(sp => new FunctionService(sp, [typeof(ZSN.AI.Plugins.BasePlugin).Assembly]));

                services.AddSignalR();

                // 启动调度服务
                services.AddHostedService<QuartzHostedService>();


            })
            .Build();

            host.RunAsync();

            NLogHelper.WriteInfo("定时任务启动！");

            Console.Read();
        }

        public void Stop()
        {
        }

    }

}
