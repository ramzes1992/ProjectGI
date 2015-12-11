using System;
using System.Collections.Generic;
using System.Globalization;
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
                    CountryCode = int.Parse(columnValues[0]),
                    Year = int.Parse(columnValues[1]),
                    Value = double.Parse(columnValues[2], CultureInfo.InvariantCulture),
                    ValueFootnotes = columnValues[3],
                    CountryName = columnValues[4],
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        class GDPDataItem
        {
            public int CountryCode { get; set; }
            public int Year { get; set; }
            public double Value { get; set; }
            public string ValueFootnotes { get; set; }
            public string CountryName { get; set; }
        }

    }
}