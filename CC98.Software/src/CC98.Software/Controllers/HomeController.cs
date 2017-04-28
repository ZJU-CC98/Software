using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Software.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace CC98.Software.Controllers
{
    public class HomeController : Controller
    {
        public int amount;
        public async Task<IActionResult> Index([FromServices] SoftwareDbContext q)
        {
            Data.Category[] m;
            var result = from i in q.Categories select i;
            m = await result.ToArrayAsync();
            return View(m);
        }

        public async Task<IActionResult> Upload(UploadWare m, [FromServices] SoftwareDbContext q)
        {
            System.IO.FileStream a = System.IO.File.OpenWrite(System.IO.Path.Combine("C:\\Test\\", m.Name));
            await m.File.CopyToAsync(a);
            System.IO.FileStream b = System.IO.File.OpenWrite(System.IO.Path.Combine("C:\\Test\\Gra", m.Name));
            await m.Photo.CopyToAsync(b);
            //新开空文件 返回文件流 将IFormFile格式文件转为FileStream存入本地服务器
            Data.Software newfile = new Data.Software
            {
                Name=m.Name,
                Introduction = m.Introduction,
                Platform = m.Platform,
                FileLocation = System.IO.Path.Combine("C:\\Test\\", m.Name),
                PhotoLocation = System.IO.Path.Combine("C:\\Test\\Gra", m.Name),
                UpdateTime = DateTimeOffset.Now,
                DownloadNum = 0,
                Filename=m.File.FileName,
            };

            q.Softwares.Add(newfile);
            await q.SaveChangesAsync(true);
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
        public async Task<IActionResult> Background([FromServices] SoftwareDbContext q)
        {
            Data.Software[] m;
            var result = from i in q.Softwares select i;
            m = await result.ToArrayAsync();
            return View(m);
        }

        public async Task<IActionResult> UnAccepted(int id, [FromServices] SoftwareDbContext q)
        {
            Data.Software m;
            m = q.Softwares.Find(id);
            if (m == null)
            {
                return NotFound();
            }
            else
            {
                q.Softwares.Remove(m);
            }
            await q.SaveChangesAsync(true);
            return RedirectToAction("Background");
        }

        public async Task<IActionResult> Accepted(int id, [FromServices] SoftwareDbContext q)
        {
            Data.Software m;
            m = q.Softwares.Find(id);
            if (m == null)
            {
                return NotFound();
            }
            else
            {
                m.IsAccepted = true;
            }
            await q.SaveChangesAsync(true);
            return RedirectToAction("Background");
        }

        public async Task<IActionResult> NewCategory(string name, [FromServices] SoftwareDbContext q)
        {
            Data.Category m = new Category();
            m.Name = name;
            q.Categories.Add(m);
            await q.SaveChangesAsync(true); 
            return RedirectToAction("CategoryManagement");
        }

        public async Task<IActionResult> Delete(int id, [FromServices] SoftwareDbContext q)
        {
            Category m;
            m =await  q.Categories.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            else
            {
                q.Categories.Remove(m);
            }
            await q.SaveChangesAsync(true);
            return RedirectToAction("CategoryManagement");
        }

        public async Task<IActionResult> CategoryManagement([FromServices] SoftwareDbContext q)
        {
            Category[] m;
            var result = from i in q.Categories select i;
           m =   await result.ToArrayAsync();
            return View(m); ;
        }

        public async Task<IActionResult> New2Category(string name, int id, [FromServices] SoftwareDbContext q)
        {
            Data.Category m = new Category();
            Data.Category n;
            n = await q.Categories.FindAsync(id);
            m.Name = name;
            m.Parent = n;
            await q.SaveChangesAsync(true);
            return RedirectToAction("CategoryManagement");
        }

        [Authorize]
        public async Task<IActionResult> SendMessage(SMessage p, [FromServices] SoftwareDbContext q)
        {


            Data.Feedback newmes = new Data.Feedback
            {
                Message = p.Content,
                ReceiverName = p.Receivername,
                Time = DateTimeOffset.Now,
                Title = p.Title,
                SenderName = User.Identity.Name,
            };
            q.Feedbacks.Add(newmes);
            await q.SaveChangesAsync(true);
            return RedirectToAction("Messagebox");
        }
        public async Task<IActionResult> Messagebox([FromServices] SoftwareDbContext q)
        {
            Data.Feedback[] m;
            string name = User.Identity.Name;
            var result = from i in q.Feedbacks where (i.ReceiverName == name || i.SenderName == name) select i;
            m =  await result.ToArrayAsync();
            await q.SaveChangesAsync(true);
            return View(m);
        }
        public async Task<IActionResult> MessageDetail(int id, [FromServices] SoftwareDbContext q)
        {
            Data.Feedback m =await q.Feedbacks.FindAsync(id);
            return View(m);
        }
        public async Task<IActionResult> Details(int id, [FromServices] SoftwareDbContext q)
        {
            Data.Software m = await q.Softwares.FindAsync(id);
            return View(m);
        }
        public async Task<IActionResult> InList(int page, int classid, [FromServices] SoftwareDbContext q)
        {
            page++;
            ViewBag.curPage = page;   
            var b = from i in q.Softwares where i.Class.Id == classid select i;
            ViewBag.amount =await  b.CountAsync();
            var m = b.Skip(10 * (page - 1)).Take(10);
            return View(await m.ToArrayAsync());
        }
       
        public async Task<IActionResult> changeFrequencyT(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.IsFrequent = true;
           await  q.SaveChangesAsync(true);
            return RedirectToAction("Details");
        }
        public async Task<IActionResult> changeFrequencyF(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.IsFrequent = false;
           await  q.SaveChangesAsync(true);
            return RedirectToAction("Details");
        }
        public async Task<IActionResult> changeRecommendationT(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.IsRecommended = true;
           await q.SaveChangesAsync(true);
            return RedirectToAction("Details");
        }
        public async Task<IActionResult> changeRecommendationF(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.IsRecommended = false;
            await q.SaveChangesAsync(true);
            return RedirectToAction("Details");
        }
        public async Task<IActionResult> Search([FromServices]SoftwareDbContext dbcontext, string content)
        {
            var x = from i in dbcontext.Softwares
                    where i.Name.Contains(content)
                    select i;
            return View(await x.ToArrayAsync());
        }

        public async Task<IActionResult> DeleteSoftware(int id,[FromServices]SoftwareDbContext q)
        {
            var x = q.Softwares.Find(id);
            q.Softwares.Remove(x);
            await q.SaveChangesAsync();
            return View("Index");
        }

        public IActionResult Download(int id,[FromServices]SoftwareDbContext q)
        {
            var x = q.Softwares.Find(id);
            return PhysicalFile(x.FileLocation, "application/octet-stream",x.Filename);
        }
    }
}
