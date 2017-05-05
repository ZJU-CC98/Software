using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Software.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;
using Sakura.AspNetCore.Mvc;

namespace CC98.Software.Controllers
{
	public class HomeController : Controller
	{
		public async Task<IActionResult> Index([FromServices] SoftwareDbContext dbContext)
		{
			var result = await (from i in dbContext.Categories select i).ToArrayAsync();
			return View(result);
		}

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upload(UploadWare model, [FromServices] SoftwareDbContext dbContext, [FromServices]IOptions<Setting> setting)
		{
			using (var fileStream = System.IO.File.Create(System.IO.Path.Combine(setting.Value.SaveFileAddress, model.Name)))
			{
				await model.File.CopyToAsync(fileStream);
			}

			using (var graphStream = System.IO.File.Create(System.IO.Path.Combine(setting.Value.SaveGraAddress, model.Name)))
			{
				await model.Photo.CopyToAsync(graphStream);
			}
			//新开空文件 返回文件流 将IFormFile格式文件转为FileStream存入本地服务器
			var newFile = new Data.Software
			{
				Name = model.Name,
				Introduction = model.Introduction,
				Platform = model.Platform,
				FileLocation = System.IO.Path.Combine(setting.Value.SaveFileAddress, model.Name),
				PhotoLocation = System.IO.Path.Combine(setting.Value.SaveGraAddress, model.Name),
				UpdateTime = DateTimeOffset.Now,
				DownloadNum = 0,
				Filename = model.File.FileName,
			};

			dbContext.Softwares.Add(newFile);
			await dbContext.SaveChangesAsync(true);
			return View("AfterUploading");
		}
		public IActionResult ShowUpload()
		{
			return View();
		}



		public IActionResult Error()
		{
			return View();
		}

		[Authorize("Manage")]
		public async Task<IActionResult> Background([FromServices] SoftwareDbContext dbContext)
		{
			var softwares = await (from i in dbContext.Softwares select i).ToArrayAsync();
			return View(softwares);
		}

		public async Task<IActionResult> UnAccepted(int id, [FromServices] SoftwareDbContext dbContext)
		{
			var software = dbContext.Softwares.Find(id);
			if (software == null)
			{
				return NotFound();
			}
			dbContext.Softwares.Remove(software);
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("Background");
		}

		public async Task<IActionResult> Accepted(int id, [FromServices] SoftwareDbContext dbContext)
		{
			Data.Software m;
			m = dbContext.Softwares.Find(id);
			if (m == null)
			{
				return NotFound();
			}
			m.IsAccepted = true;
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("Background");
		}

		public async Task<IActionResult> NewCategory(string name, [FromServices] SoftwareDbContext dbContext)
		{
			var m = new Category
			{
				Name = name
			};
			dbContext.Categories.Add(m);
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("CategoryManagement");
		}

		public async Task<IActionResult> Delete(int id, [FromServices] SoftwareDbContext dbContext)
		{
			var category = await dbContext.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			else
			{
				dbContext.Categories.Remove(category);
			}
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("CategoryManagement");
		}

		public async Task<IActionResult> CategoryManagement([FromServices] SoftwareDbContext dbContext)
		{
			var result = from i in dbContext.Categories select i;
			var categories = await result.ToArrayAsync();
			return View(categories); 
		}

		public async Task<IActionResult> New2Category(string name, int id, [FromServices] SoftwareDbContext dbContext)
		{
			var category = new Category();
			var categoryParent = await dbContext.Categories.FindAsync(id);
			category.Name = name;
			category.Parent = categoryParent;
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("CategoryManagement");
		}

		[Authorize]
		public async Task<IActionResult> SendMessage(string receivername, string title, string content, [FromServices] SoftwareDbContext dbContext)
		{


			var newmes = new Data.Feedback
			{
				Message = content,
				ReceiverName = receivername,
				Time = DateTimeOffset.Now,
				Title = title,
				SenderName = User.Identity.Name,
			};
			dbContext.Feedbacks.Add(newmes);
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("MessageBox");
		}
		public async Task<IActionResult> MessageBox([FromServices] SoftwareDbContext dbContext,int page=1)
		{
			var name = User.Identity.Name;
		
			var result = from i in dbContext.Feedbacks where (i.ReceiverName == name || i.SenderName == name) select i;
			await dbContext.SaveChangesAsync(true);
			
			return View();
		}
		public async Task<IActionResult> MessageDetail(int id, [FromServices] SoftwareDbContext dbContext)
		{
			var m = await dbContext.Feedbacks.FindAsync(id);
			return View(m);
		}
		public async Task<IActionResult> Details(int id, [FromServices] SoftwareDbContext dbContext)
		{
			var m = await dbContext.Softwares.FindAsync(id);
			return View(m);
		}
		public async Task<IActionResult> InList(int classId, [FromServices] SoftwareDbContext dbContext, int page = 1)
		{
			ViewBag.Classid = classId;
			var b = from i in dbContext.Softwares where i.Class.Id == classId select i;
			var pagedData = await b.ToPagedListAsync(10, page);

			return View("List", pagedData);
		}

		public async Task<IActionResult> ChangeFrequencyT(int id, [FromServices]Data.SoftwareDbContext dbContext)
		{
			var p = dbContext.Softwares.Find(id);
			p.IsFrequent = true;
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("Details");
		}
		public async Task<IActionResult> ChangeFrequencyF(int id, [FromServices]Data.SoftwareDbContext dbContext)
		{
			var p = dbContext.Softwares.Find(id);
			p.IsFrequent = false;
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("Details");
		}
		public async Task<IActionResult> ChangeRecommendationT(int id, [FromServices]Data.SoftwareDbContext dbContext)
		{
			var p = dbContext.Softwares.Find(id);
			p.IsRecommended = true;
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("Details");
		}
		public async Task<IActionResult> ChangeRecommendationF(int id, [FromServices]Data.SoftwareDbContext dbContext)
		{
			var p = dbContext.Softwares.Find(id);
			p.IsRecommended = false;
			await dbContext.SaveChangesAsync(true);
			return RedirectToAction("Details");
		}
		public async Task<IActionResult> Search([FromServices]SoftwareDbContext dbcontext, string content,int page=1)
		{
			var x = from i in dbcontext.Softwares
					where i.Name.Contains(content)
					select i;
			ViewBag.Content = content;
			var pagedData = await x.ToPagedListAsync(10, page);
			return View("List",pagedData);
		}

		public async Task<IActionResult> DeleteSoftware(int id, [FromServices]SoftwareDbContext dbContext)
		{
			var x = dbContext.Softwares.Find(id);
			dbContext.Softwares.Remove(x);
			await dbContext.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public IActionResult Download(int id, [FromServices]SoftwareDbContext dbContext)
		{
			var x = dbContext.Softwares.Find(id);
			return PhysicalFile(x.FileLocation, "application/octet-stream", x.Filename);
		}

		public async Task<IActionResult> Inbox([FromServices]SoftwareDbContext dbContext,int page=1)
		{
			var name = User.Identity.Name;
		
			var result = from i in dbContext.Feedbacks where (i.ReceiverName == name ) select i;
			var pagedData = await result.ToPagedListAsync(10, page);
		
			return View(pagedData);
		}
		public async Task<IActionResult> Outbox([FromServices]SoftwareDbContext dbContext, int page = 1)
		{
			var name = User.Identity.Name;
			
			var result = from i in dbContext.Feedbacks where (i.SenderName == name) select i;
			var pagedData = await result.ToPagedListAsync(10, page);

			return View(pagedData);
		}
	}
}
