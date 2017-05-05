using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CC98.Software
{
    public class HotSoftwaresViewComponent:Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke([FromServices] Data.SoftwareDbContext dbContext)
        {
            Data.Software[] m;
            var result = from i in dbContext.Softwares orderby i.DownloadNum descending select i;
            var c = result.Take(10);
            m = c.ToArray();
            return View(m);
        }
    }
}
