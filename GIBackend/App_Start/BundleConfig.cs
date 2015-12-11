using System.Web;
using System.Web.Optimization;

namespace GIBackend
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                        "~/Scripts/jquery-2.1.4.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/scripts/ulf").Include(
                        "~/Scripts/underscore.js",
                        "~/Scripts/rankings.js"
                        ));

            bundles.Add(new StyleBundle("~/styles/ulf").Include(
                        "~/Content/rankings.css"

            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new Bundle("~/bundles/data").Include(
                "~/Data/ludnosc.csv",
                "~/Data/pkb.csv"
                ));
        }
    }
}
