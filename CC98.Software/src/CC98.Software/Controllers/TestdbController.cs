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
        public IActionResult AddDb([FromServices]Data.SoftwareDbContext dbContext)
        {

            for (var i = 1; i <= 100; i++)
            {
	            var s = new Data.Software
	            {
		            Name = i.ToString(),
		            DownloadNum = 1,
		            IsAccepted = false,
		            IsFrequent = false,
		            Size = 1,
		            UpdateTime = DateTimeOffset.Now,
		            Platform = Data.Platform.Android
	            };
	            dbContext.Softwares.Add(s);

            }
            dbContext.SaveChanges(true);
            return RedirectToAction("index", "home");
        }
        public IActionResult AddDbcomment([FromServices]Data.SoftwareDbContext dbContext)
        {
            for (var i = 1; i <= 100; i++)
            {
                var s = new Data.Comment();
                s.Time = DateTimeOffset.Now;
                s.Content = i + "abc";
                var p = new Data.Software();
                p.Id = i / 3 + 1;
                s.Software = p;
                dbContext.Comments.Add(s);
            }
            dbContext.SaveChanges(true);
            return RedirectToAction("index", "home");
        }
        public IActionResult AddDbfeedback([FromServices]Data.SoftwareDbContext dbContext)
        {
            for (var i = 1; i <= 100; i++)
            {
                var s = new Data.Feedback();
                s.Time = DateTimeOffset.Now;
                s.Title = i + "abc";
                s.Message = i + "abcdefg";
                dbContext.Feedbacks.Add(s);
            }
            dbContext.SaveChanges(true);
            return RedirectToAction("index", "home");
        }
    }
}
