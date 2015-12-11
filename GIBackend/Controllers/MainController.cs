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

        public ActionResult Map()
        {
            return View();
        }

        public JsonResult GetGDPData()
        {
            var path = HttpContext.Server.MapPath("~/Data/kraje.csv");
            var data = System.IO.File.ReadAllLines(path);

            List<GDPDataItem> items = new List<GDPDataItem>();

            for (int i = 1; i < data.Length; i++)//first row has only columns names
            {
                var columnValues = data[i].Split(',');

                items.Add(new GDPDataItem()
                {
                    CountryCode = columnValues[0],
                    Year = columnValues[1],
                    Value = columnValues[2],
                    ValueFootnotes = columnValues[3],
                    CountryName = columnValues[4]
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        class GDPDataItem
        {
            public string CountryCode { get; set; }
            public string Year { get; set; }
            public string Value { get; set; }
            public string ValueFootnotes { get; set; }
            public string CountryName { get; set; }
        }

    }
}