
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using ZSN.AgentBrook.Web;
using ZSN.Utils.Core.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ZSN.AI.Core.Utils;
using Microsoft.AspNetCore.Components;
using ZSN.AI.Core.Common.DependencyInjection;
using ZSN.AI.Core.Service;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using ZSN.AI.Service.Base;


CreateHostBuilder(args).Build().Run();
static IHostBuilder CreateHostBuilder(string[] args)
{
    var host = Host.CreateDefaultBuilder(args);
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        host = host.UseWindowsService();
    }
    return host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole(options =>
        {
            options.LogToStandardErrorThreshold = LogLevel.Error;
        });
    }).ConfigureWebHostDefaults(webBuilder =>
    {
        var port = ConfigHelper.GetInt("ServicePort");//设置服务端口
        webBuilder.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Listen(IPAddress.Any, port);
            serverOptions.Limits.MaxRequestBodySize = null;
        });
        webBuilder.UseStartup<Startup>();
    });
}