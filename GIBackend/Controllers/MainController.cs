using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace GIBackend.Controllers
{
    public class MainController : Controller
    {
        public static Dictionary<int, string> CountryCodes = new Dictionary<int, string>();

        public MainController()
        {
            //var path = HttpContext.Server.MapPath("~/Data/country-codes.csv");
            if (!CountryCodes.Any())
            {
                var path = HostingEnvironment.MapPath("~/Data/country-codes.csv");
                var countryCodesLines = System.IO.File.ReadAllLines(path);
                foreach (var line in countryCodesLines)
                {
                    if (line.Contains("\""))
                        continue;
                    var values = line.Split(',');
                    CountryCodes.Add(int.Parse(values[4]), values[0]);
                }
            }
        }

        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ranking(int? id)
        {
            if (id.HasValue)
            {
                if (CountryCodes.ContainsKey(id.Value))
                {
                    var countryName = CountryCodes[id.Value];
                }
            }

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

        public JsonResult GetGDPRanking(int? countryCode)
        {
            var path = HttpContext.Server.MapPath("~/Data/pkb.csv");
            var data = System.IO.File.ReadAllLines(path);

            List<GDPRanking> items = new List<GDPRanking>();

            for (int i = 1; i < data.Length; i++)//first row has only columns names
            {
                var columnValues = data[i].Split(',');

                items.Add(new GDPRanking()
                {
                    country = columnValues[0],
                    year = int.Parse(columnValues[1]),
                    value = int.Parse(columnValues[2]),
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        class GDPRanking
        {
            public string country { get; set; }
            public int year { get; set; }
            public int value { get; set; }
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