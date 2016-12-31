using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Software.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CC98.Software
{
	/// <summary>
	/// 应用程序的启动类型。
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// 初始化 <see cref="Startup"/> 对象的新实例。
		/// </summary>
		/// <param name="env">应用程序宿主环境对象。</param>
		[UsedImplicitly]
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			// 如果在测试环境中则添加测试用设定
			if (env.IsDevelopment())
			{
				builder.AddUserSecrets();
			}

			// 生成应用程序配置
			Configuration = builder.Build();
		}

		/// <summary>
		/// 获取应用程序的配置对象。
		/// </summary>

		private IConfigurationRoot Configuration { get; }

		/// <summary>
		/// 配置应用程序的相关服务。
		/// </summary>
		/// <param name="services">应用程序的服务表。</param>
		[UsedImplicitly]
		public void ConfigureServices(IServiceCollection services)
		{
			// 添加数据库功能
			services.AddDbContext<SoftwareDbContext>(options =>
			{
				// 使用配置文件中的连接字符串连接到数据库
				options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
			});

			// 添加 MVC 服务
			services.AddMvc();
		}

		/// <summary>
		/// 配置应用程序的功能。
		/// </summary>
		/// <param name="app">应用程序宿主对象。</param>
		/// <param name="env">承载环境对象。</param>
		/// <param name="loggerFactory">日志工厂对象。</param>
		[UsedImplicitly]
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			// 添加控制台调试日志
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));

			// 添加 VS 调试窗口日志
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				// 在开发环境中显示详细代码错误
				app.UseDeveloperExceptionPage();
				// 在开发环境中使用浏览器监视器
				app.UseBrowserLink();
			}
			else
			{
				// 在生产环境中只显示通用错误
				app.UseExceptionHandler("/Home/Error");
			}

			// 允许网站显示静态内容（如脚本和样式表）
			app.UseStaticFiles();

			// 配置路由规则		
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
