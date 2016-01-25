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
        public static List<GDPDataItem> GDPDataItems = new List<GDPDataItem>();


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

            if (!GDPDataItems.Any())
            {
                var path = HostingEnvironment.MapPath("~/Data/GDPEurope.csv");
                var countryCodesLines = System.IO.File.ReadAllLines(path);
                foreach (var line in countryCodesLines.Skip(1))
                {
                    var values = line.Replace("\"", "").Split(',');
                    var item = new GDPDataItem();
                    item.CountryCode = CountryCodes.First(i => i.Value.Equals(values[0])).Key;
                    item.CountryName = values[0];
                    item.Year = int.Parse(values[1]);
                    item.Value = double.Parse(values[2], CultureInfo.InvariantCulture);
                    item.ValueFootnotes = "";

                    GDPDataItems.Add(item);
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
                    ViewData["ConutryCode"] = id.Value;
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
            var result = GDPDataItems.Where(i => i.Year.Equals(2014)).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPopulationRanking(int? id)
        {
            var path = HttpContext.Server.MapPath("~/Data/ludnosc.csv");
            var data = System.IO.File.ReadAllLines(path);

            List<RankingData> items = new List<RankingData>();

            for (int i = 1; i < data.Length; i++)//first row has only columns names
            {
                var columnValues = data[i].Split(',');

                items.Add(new RankingData()
                {
                    country = columnValues[0],
                    year = int.Parse(columnValues[1]),
                    value = int.Parse(columnValues[2]),
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGDPRanking(int? id)
        {
            var result = new List<RankingData>();

            var data = GDPDataItems
                        .Where(i => i.CountryCode.Equals(id.Value))
                        .ToList();

            foreach (var item in data)
            {
                var same_year = GDPDataItems.Where(i => i.Year.Equals(item.Year)).ToList();
                var sorderd = same_year.OrderByDescending(i => i.Value).ToList();
                var value = sorderd.IndexOf(same_year.Single(s => s.CountryCode.Equals(id.Value))) + 1;

                result.Add(new RankingData()
                {
                    country = item.CountryName,
                    year = item.Year,
                    value = value,
                });
            }

            result = result.OrderBy(i => i.year).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }

    class RankingData
    {
        public string country { get; set; }
        public int year { get; set; }
        public int value { get; set; }
    }

    public class GDPDataItem
    {
        public int CountryCode { get; set; }
        public int Year { get; set; }
        public double Value { get; set; }
        public string ValueFootnotes { get; set; }
        public string CountryName { get; set; }
    }
}