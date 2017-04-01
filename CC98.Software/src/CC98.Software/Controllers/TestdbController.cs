using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CC98.Software.Controllers
{
    public class TestdbController : Controller
    { 
        // GET: /<controller>/
        public IActionResult AddDb([FromServices]Data.SoftwareDbContext q)
        {
           
            for (int i = 1; i <= 100; i++)
            {
                Data.Software s = new Data.Software();
                s.Name = i.ToString();
                s.DownloadNum = 1;
                s.IsAccepted = false;
                s.IsFrequent = false;
                s.Size = 1;
                s.UpdateTime = DateTimeOffset.Now;
                s.Platform = Data.Platform.Android;
                q.Softwares.Add(s);
              
            }
            q.SaveChanges(true);
            return RedirectToAction("index","home");
        }
    }
}
