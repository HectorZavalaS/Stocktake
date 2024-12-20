using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stocktake.Controllers
{
    public class reportController : Controller
    {
        // GET: report
        public ActionResult Index()
        {
            return View();
        }
    }
}