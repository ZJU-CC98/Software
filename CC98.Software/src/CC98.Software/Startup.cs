using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Authentication;
using CC98.Software.Data;
using JetBrains.Annotations;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sakura.AspNetCore.Mvc;

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
				builder.AddUserSecrets<Startup>();
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
            services.Configure<Setting>(Configuration.GetSection("WebsiteAddress"));
			services.AddBootstrapPagerGenerator(options =>
			{
				// Use default pager options.
				options.ConfigureDefault();
			});

			// 添加数据库功能
			services.AddDbContext<SoftwareDbContext>(options =>
			{
				// 使用配置文件中的连接字符串连接到数据库
				options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
			});

			// 添加 MVC 服务
			services.AddMvc();

			// HTTP 上下文核心服务
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			// 配置第三方身份验证相关的设置
			services.Configure<SharedAuthenticationOptions>(options =>
			{
				// 将第三方身份验证信息暂存入网站的外部 Cookie 中
				options.SignInScheme = new IdentityOptions().Cookies.ExternalCookieAuthenticationScheme;
			});


			// 添加外部身份验证管理器功能
			services.AddExternalSignInManager(identityOptions =>
			{
				// 应用程序的主 Cookie 设置
				identityOptions.Cookies.ApplicationCookie.CookieHttpOnly = true;
				identityOptions.Cookies.ApplicationCookie.CookieSecure = CookieSecurePolicy.None;
				identityOptions.Cookies.ApplicationCookie.LoginPath = new PathString("/Account/LogOn");
				identityOptions.Cookies.ApplicationCookie.LogoutPath = new PathString("/Account/LogOff");
                identityOptions.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/Account/AccessDenied");
				identityOptions.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
				identityOptions.Cookies.ApplicationCookie.AutomaticChallenge = true;

				// 外部 Cookie（和其他身份验证交互使用的临时票证）设置
				identityOptions.Cookies.ExternalCookie.CookieHttpOnly = true;
				identityOptions.Cookies.ExternalCookie.CookieSecure = CookieSecurePolicy.None;
				identityOptions.Cookies.ExternalCookie.AutomaticAuthenticate = false;
				identityOptions.Cookies.ExternalCookie.AutomaticChallenge = false;
			});

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Manage", builder =>
                {
                    builder.RequireRole("Software Administrators", "Software Operators");
                });
            });

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


			app.UseAllCookies();
			app.UseCC98Authentication(new CC98AuthenticationOptions
			{
				ClientId = Configuration["Authentication:CC98:ClientId"],
				ClientSecret = Configuration["Authentication:CC98:ClientSecret"],
				AllowInsecureHttp = true
			});

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
