using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIBackend.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ranking()
        {
            return View();
        }

        public JsonResult GetGDPData()
        {
            
        }
    }
}