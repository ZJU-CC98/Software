using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC98.Software.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CC98.Software.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult desktop()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        
        public IActionResult Upload(UploadWare m,[FromServices]SoftwareDbContext q)
        {
            System.IO.FileStream a=System.IO.File.OpenWrite(System.IO.Path.Combine("File", m.File.FileName)) ;
            m.File.CopyTo(a);
            System.IO.FileStream b = System.IO.File.OpenWrite(System.IO.Path.Combine("File", m.File.FileName));
            m.Photo.CopyTo(b);
            //新开空文件 返回文件流 将IFormFile格式文件转为FileStream存入本地服务器
            Data.Software newfile = new Data.Software
            {
                
                Introduction = m.Introduction,
                File = m.File,
                Platform = m.Platform,
                Size=m.File.Length, 
                FileLocation= System.IO.Path.Combine("File", m.File.FileName) ,
                PhotoLocation= System.IO.Path.Combine("File", m.Photo.FileName),
                UpdateTime=  DateTimeOffset .Now,
                DownloadNum=0,
            };
      

            q.Softwares.Add( newfile);
            return View();
        }

        public IActionResult game()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult android()
        {
            return View();
        }

        public IActionResult apple()
        {
            return View();
        }

        public IActionResult houtai()
        {
            return View();
        }
    }
}
