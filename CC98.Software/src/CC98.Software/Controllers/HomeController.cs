using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Software.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CC98.Software.Controllers
{
    public class HomeController : Controller
    {
        public int amount;
        public IActionResult Index([FromServices] SoftwareDbContext q)
        {
            Data.Category[] m;
            var result = from i in q.Categories select i;
            m = result.ToArray();
            return View(m);
        }

        public IActionResult Upload(UploadWare m, [FromServices] SoftwareDbContext q)
        {
            System.IO.FileStream a = System.IO.File.OpenWrite(System.IO.Path.Combine("File", m.File.FileName));
            m.File.CopyTo(a);
            System.IO.FileStream b = System.IO.File.OpenWrite(System.IO.Path.Combine("File", m.File.FileName));
            m.Photo.CopyTo(b);
            //新开空文件 返回文件流 将IFormFile格式文件转为FileStream存入本地服务器
            Data.Software newfile = new Data.Software
            {

                Introduction = m.Introduction,
                Platform = m.Platform,
                FileLocation = System.IO.Path.Combine("File", m.File.FileName),
                PhotoLocation = System.IO.Path.Combine("File", m.Photo.FileName),
                UpdateTime = DateTimeOffset.Now,
                DownloadNum = 0,
            };

            q.Softwares.Add(newfile);
            q.SaveChanges(true);
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


        public IActionResult Background([FromServices] SoftwareDbContext q)
        {
            Data.Software[] m;
            var result = from i in q.Softwares select i;
            m = result.ToArray();
            return View(m);
        }

        public IActionResult UnAccepted(int id, [FromServices] SoftwareDbContext q)
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
            q.SaveChanges(true);
            return RedirectToAction("houtai");
        }

        public IActionResult Accepted(int id, [FromServices] SoftwareDbContext q)
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
            q.SaveChanges(true);
            return RedirectToAction("Background");
        }

        public IActionResult NewCategory(string name, [FromServices] SoftwareDbContext q)
        {
            Data.Category m = new Category();
            m.Name = name;
            return RedirectToAction("Background");
        }

        public IActionResult Delete(int id, [FromServices] SoftwareDbContext q)
        {
            Category m;
            m = q.Categories.Find(id);
            if (m == null)
            {
                return NotFound();
            }
            else
            {
                q.Categories.Remove(m);
            }
            q.SaveChanges(true);
            return RedirectToAction("CategoryManagement");
        }

        public IActionResult CategoryManagement([FromServices] SoftwareDbContext q)
        {
            Category[] m;
            var result = from i in q.Categories select i;
            m = result.ToArray();
            return View(m); ;
        }

        public IActionResult New2Category(string name, int id, [FromServices] SoftwareDbContext q)
        {
            Data.Category m = new Category();
            Data.Category n;
            n = q.Categories.Find(id);
            m.Name = name;
            m.Parent = n;
            q.SaveChanges(true);
            return RedirectToAction("Background");
        }

        [Authorize]
        public IActionResult SendMessage(SMessage p, [FromServices] SoftwareDbContext q)
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
            q.SaveChanges(true);
            return RedirectToAction("Messagebox");
        }
        public IActionResult Messagebox([FromServices] SoftwareDbContext q)
        {
            Data.Feedback[] m;
            string name = User.Identity.Name;
            var result = from i in q.Feedbacks where (i.ReceiverName == name || i.SenderName == name) select i;
            m = result.ToArray();
            q.SaveChanges(true);
            return View(m);
        }
        public IActionResult MessageDetail(int id, [FromServices] SoftwareDbContext q)
        {
            Data.Feedback m = q.Feedbacks.Find(id);
            return View(m);
        }
        public IActionResult Details(int id, [FromServices] SoftwareDbContext q)
        {
            Data.Software m = q.Softwares.Find(id);
            return View(m);
        }
        public IActionResult InList(int page, int classid, [FromServices] SoftwareDbContext q)
        {
            page++;
            ViewBag.curPage = page;   
            var b = from i in q.Softwares where i.Class.Id == classid select i;
            var c = b.ToArray();
            ViewBag.amount = c.Count();
            var m = c.Skip(10 * (page - 1)).Take(10);
            return View(m.ToArray());
        }
       
        public IActionResult changeFrequencyT(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.IsFrequent = true;
            q.SaveChanges(true);
            return RedirectToAction("Details");
        }
        public IActionResult changeFrequencyF(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.IsFrequent = false;
            q.SaveChanges(true);
            return RedirectToAction("Details");
        }
        public IActionResult changeRecommendationT(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.isRecommended = true;
            q.SaveChanges(true);
            return RedirectToAction("Details");
        }
        public IActionResult changeRecommendationF(int id, [FromServices]Data.SoftwareDbContext q)
        {
            Data.Software p = q.Softwares.Find(id);
            p.isRecommended = false;
            q.SaveChanges(true);
            return RedirectToAction("Details");
        }
        public IActionResult Search([FromServices]SoftwareDbContext dbcontext, SearchModel model)
        {
            var x = from i in dbcontext.Softwares
                    where i.Name.Contains(model.Content)
                    select i;
            return View(x.ToArray());
        }

    }
}
