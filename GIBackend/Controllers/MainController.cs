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
        public static List<DataItem> GDPDataItems = new List<DataItem>();
        public static List<DataItem> PopulationDataItems = new List<DataItem>();
        public static List<DataItem> GDPPerCapitaDataItems = new List<DataItem>();

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
                    var item = new DataItem();
                    item.CountryCode = CountryCodes.First(i => i.Value.Equals(values[0])).Key;
                    item.CountryName = values[0];
                    item.Year = int.Parse(values[1]);
                    item.Value = double.Parse(values[2], CultureInfo.InvariantCulture);
                    item.ValueFootnotes = "";

                    GDPDataItems.Add(item);
                }
            }

            if (!PopulationDataItems.Any())
            {
                var path = HostingEnvironment.MapPath("~/Data/PopulationEurope.csv");
                var countryCodesLines = System.IO.File.ReadAllLines(path);
                foreach (var line in countryCodesLines.Skip(1))
                {
                    if (!line.Contains("Medium variant"))
                        continue;

                    var values = line.Replace("\"", "").Split(',');
                    var item = new DataItem();
                    item.CountryCode = CountryCodes.First(i => i.Value.Equals(values[0])).Key;
                    item.CountryName = values[0];
                    item.Year = int.Parse(values[1]);
                    item.Value = double.Parse(values[3], CultureInfo.InvariantCulture);
                    item.ValueFootnotes = "";

                    PopulationDataItems.Add(item);
                }
            }

            if (!GDPPerCapitaDataItems.Any())
            {
                var path = HostingEnvironment.MapPath("~/Data/GDPPerCapitaEuroper.csv");
                var countryCodesLines = System.IO.File.ReadAllLines(path);
                foreach (var line in countryCodesLines.Skip(1))
                {
                    var values = line.Replace("\"", "").Split(',');
                    var item = new DataItem();
                    item.CountryCode = CountryCodes.First(i => i.Value.Equals(values[0])).Key;
                    item.CountryName = values[0];
                    item.Year = int.Parse(values[1]);
                    item.Value = double.Parse(values[3], CultureInfo.InvariantCulture);
                    item.ValueFootnotes = "";

                    GDPPerCapitaDataItems.Add(item);
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
                    ViewData["CountryName"] = countryName;
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

        public JsonResult GetPopulationData()
        {
            var result = PopulationDataItems.Where(i => i.Year.Equals(2015)).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGDPPerCapitaData()
        {
            var result = GDPPerCapitaDataItems.Where(i => i.Year.Equals(2013)).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        int take = 10;
        public JsonResult GetPopulationRanking(int? id)
        {
            var result = new List<RankingDataItem>();

            var data = PopulationDataItems
                        .Where(i => i.CountryCode.Equals(id.Value))
                        .ToList();

            foreach (var item in data)
            {
                var same_year = PopulationDataItems.Where(i => i.Year.Equals(item.Year)).ToList();
                var sorted = same_year.OrderByDescending(i => i.Value).ToList();
                var value = sorted.IndexOf(item) + 1;

                result.Add(new RankingDataItem()
                {
                    country = item.CountryName,
                    year = item.Year,
                    value = value,
                });
            }

            result = result.OrderBy(i => i.year).Skip(result.Count - take).Take(take).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGDPRanking(int? id)
        {
            var result = new List<RankingDataItem>();

            var data = GDPDataItems
                        .Where(i => i.CountryCode.Equals(id.Value))
                        .ToList();

            foreach (var item in data)
            {
                var same_year = GDPDataItems.Where(i => i.Year.Equals(item.Year)).ToList();
                var sorted = same_year.OrderByDescending(i => i.Value).ToList();
                var value = sorted.IndexOf(item) + 1;

                result.Add(new RankingDataItem()
                {
                    country = item.CountryName,
                    year = item.Year,
                    value = value,
                });
            }

            result = result.OrderBy(i => i.year).Skip(result.Count - take).Take(take).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGDPValues(int? id)
        {
            var result = new List<RankingDataItem>();

            var data = GDPDataItems
                        .Where(i => i.CountryCode.Equals(id.Value))
                        .ToList();

            foreach (var item in data)
            {
                var same_year = GDPDataItems.Where(i => i.Year.Equals(item.Year)).ToList();
                var sorted = same_year.OrderByDescending(i => i.Value).ToList();
                var value = (int)(item.Value/1000000);//sorted.IndexOf(item) + 1;

                result.Add(new RankingDataItem()
                {
                    country = item.CountryName,
                    year = item.Year,
                    value = value,
                });
            }

            result = result.OrderBy(i => i.year).Skip(result.Count - take).Take(take).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGDPPerCapitaRanking(int? id)
        {
            var result = new List<RankingDataItem>();

            var data = GDPPerCapitaDataItems
                        .Where(i => i.CountryCode.Equals(id.Value))
                        .ToList();

            foreach (var item in data)
            {
                var same_year = GDPPerCapitaDataItems.Where(i => i.Year.Equals(item.Year)).ToList();
                var sorted = same_year.OrderByDescending(i => i.Value).ToList();
                var value = sorted.IndexOf(item) + 1;

                result.Add(new RankingDataItem()
                {
                    country = item.CountryName,
                    year = item.Year,
                    value = value,
                });
            }

            result = result.OrderBy(i => i.year).Skip(result.Count - take).Take(take).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BarChart(int? id)
        {
            if (id.HasValue)
            {
                if (CountryCodes.ContainsKey(id.Value))
                {
                    var countryName = CountryCodes[id.Value];
                    ViewData["CountryCode"] = id.Value;
                    ViewData["CountryName"] = countryName;
                }
            }

            return View();
        }
    }



    class RankingDataItem
    {
        public string country { get; set; }
        public int year { get; set; }
        public int value { get; set; }
    }

    public class DataItem
    {
        public int CountryCode { get; set; }
        public int Year { get; set; }
        public double Value { get; set; }
        public string ValueFootnotes { get; set; }
        public string CountryName { get; set; }
    }
}