using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CC98.Software
{
    public class SearchViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke([FromServices] Data.SoftwareDbContext q, Data.SearchModel model)
        {
            var x = from i in q.Softwares

                    where i.Name.Contains(model.Content)

                    select i;

            return View(x.ToArray());
        }
    }
}
