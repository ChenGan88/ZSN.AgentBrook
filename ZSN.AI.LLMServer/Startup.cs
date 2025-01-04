using ZSN.AI.Service.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using ZSN.AI.LLMServer.ConfigureSwagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Microsoft.AspNetCore.Rewrite;
using System;
using Microsoft.AspNetCore.Components;
using ZSN.AI.Core.Common.DependencyInjection;
using ZSN.AI.Core.Service;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ZSN.AI.Core.Utils;

namespace ZSN.AI.LLMServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "ZSNAppSession"; // Session的Cookie名称
                options.IdleTimeout = TimeSpan.FromSeconds(3600); // Session过期时间
                options.Cookie.HttpOnly = true; // 只通过HTTP访问Session Cookie
            });
            //services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();

            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(sp.GetService<NavigationManager>()!.BaseUri)
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

            services.AddServicesFromAssemblies("ZSN.AI.Core");
            services.AddServicesFromAssemblies("ZSN.AI.Plugins");

            services.AddSingleton(sp => new FunctionService(sp, [typeof(ZSN.AI.Plugins.BasePlugin).Assembly]));

            services.AddSignalR();
            //注册Swagger服务
            services.ConfigureSwaggerUp();
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
                //options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
            });

            //全局配置Json序列化处理
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            });

            services.AddMvc().AddJsonOptions(options =>
            {
                //JSON首字母小写解决
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //此设定解决JsonResult中文被编码的问题
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConvert());
            });

            StartupHelper.ServicesInit(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint($"/swagger/V1-Public/swagger.json", "V1-Public");
                c.SwaggerEndpoint($"/swagger/V1-Member/swagger.json", "V1-Member");
                c.RoutePrefix = "doc";
            });
            app.UseStaticFiles();

            // 添加URL重写中间件//api/File/Get?fileCode=66&w=0&h=0
            app.UseRewriter(new RewriteOptions()
                .AddRewrite(@"^api/File/Get/([^/]+)/(\d+)/(\d+)$", "api/File/Get?filecode=$1&w=$2&h=$3", skipRemainingRules: true)
            );

            app.UseRouting();

            app.UseAuthorization();

            StartupHelper.ConfigureInit(app, env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSession();
        }
    }
}
