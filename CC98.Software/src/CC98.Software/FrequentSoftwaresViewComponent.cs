using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CC98.Software
{
    public class FrequentSoftwaresViewComponent: Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke([FromServices] Data.SoftwareDbContext dbContext)
        {
            Data.Software[] m;
            var result = from i in dbContext.Softwares where i.IsFrequent select i;
            m = result.ToArray();
            return View(m);
        }
    }

}
