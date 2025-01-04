using System.IO;
using ZSN.AI.Service.Base;
using ZSN.Utils.Core.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Rewrite;
using ZSN.AI.Core.Common.DependencyInjection;
using ZSN.AI.Core.Service;
using Microsoft.AspNetCore.Components;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ZSN.AI.Core.Utils;

namespace ZSN.AgentBrook.Web
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
            //第一个参数为配置文件路径，默认为项目目录下config.json
            //第二个参数为是否缓存配置文件，默认false
            //services.AddUEditorService();
            services.AddSession();
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
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            var ueditorDir = "ueditor";
            var ueditorPath = PathHelper.Combine(Directory.GetCurrentDirectory(), ueditorDir);
            if (!Directory.Exists(ueditorPath))
            {
                Directory.CreateDirectory(ueditorPath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(ueditorPath),
                RequestPath = $"/{ueditorDir}",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });

            app.UseStaticFiles();

            app.Init();

            // 添加URL重写中间件
            //var rewriteOptions = new RewriteOptions();
            //rewriteOptions.AddRewrite(@"^Admin/Information_([^/]+)$", "Admin/Information/index?classid=$1", skipRemainingRules: true);
            //app.UseRewriter(rewriteOptions);

            app.UseRouting();

            app.UseAuthorization();

            StartupHelper.ConfigureInit(app, env);

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Index}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Index}/{action=Index}/{id?}");
            });

            app.UseSession();

            app.UseCors();
        }
    }
}
