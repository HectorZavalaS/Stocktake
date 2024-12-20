using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stocktake.Controllers
{
    public class stockTakeController : Controller
    {
        // GET: stockTake
        public ActionResult Index()
        {
            return View();
        }
    }
}