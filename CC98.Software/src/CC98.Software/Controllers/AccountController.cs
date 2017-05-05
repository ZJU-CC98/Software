using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Sakura.AspNetCore.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CC98.Software.Controllers
{
	/// <summary>
	/// 定义用户身份验证相关方法。
	/// </summary>
	public class AccountController : Controller
	{
		public AccountController(ExternalSignInManager externalSignInManager)
		{
			ExternalSignInManager = externalSignInManager;
		}

		/// <summary>
		/// 外部登录身份验证服务。
		/// </summary>
		private ExternalSignInManager ExternalSignInManager { get; }


		/// <summary>
		/// 执行登录操作。
		/// </summary>
		/// <param name="returnUrl">登录后要跳回的地址。</param>
		/// <returns>操作结果。</returns>
		[AllowAnonymous]
		public IActionResult LogOn(string returnUrl)
		{
			var authProperties = new AuthenticationProperties
			{
				IsPersistent = true,
				RedirectUri = Url.Action("LogOnCallback", "Account", new { returnUrl })
			};

			return Challenge(authProperties, CC98AuthenticationDefaults.AuthentcationScheme);
		}

		/// <summary>
		/// 当用户在第三方登录完成后执行后续操作。
		/// </summary>
		/// <param name="returnUrl">登录成功后要跳转的地址。</param>
		/// <returns></returns>
		[AllowAnonymous]
		public async Task<IActionResult> LogOnCallback(string returnUrl)
		{
			// 尝试提取第三方登录身份
			var principal = await ExternalSignInManager.SignInFromExternalCookieAsync();


			// 提取不成功，说明登录失败
			if (principal?.Identity == null)
			{
				return RedirectToAction("Index", "Home");
			}

			// 提取成功，回到登录前地址
			return ReturnToLocal(returnUrl);
		}

		/// <summary>
		/// 检查跳转地址是否为站内，如果为站内则跳转，否则回到首页。
		/// </summary>
		/// <param name="returnUrl">要尝试跳转的地址。</param>
		/// <returns>一个表示重定向操作的操作结果。</returns>
		private IActionResult ReturnToLocal(string returnUrl)
		{
			returnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "Home");
			return Redirect(returnUrl);
		}

	    public async Task<IActionResult> LogOut()
	    {
	        await ExternalSignInManager.SignOutAsync();
	        return RedirectToAction("Index","Home");
	    }

	}
}
