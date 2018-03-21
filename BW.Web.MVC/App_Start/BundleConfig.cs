using System.Web.Optimization;

namespace BW.Web.MVC
{
    public class BundleConfig
    {
        public static void RegisterBundle(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/font-awesome.css",
                "~/Content/css/style.css",
                "~/Content/css/swipebox.css",
                "~/Content/css/Site.css",
                "~/Content/css/default.css",
                "~/Content/css/layout.css",
                "~/Content/css/media-queries.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-3.3.1.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.swipebox.min.js",
                "~/Scripts/modernizr-2.6.2.js",
                "~/Scripts/modernizr.custom.69142.js",
                "~/Scripts/app.js",
                "~/Scripts/modernizr.js",
                "~/Scripts/main.js",
                "~/Scripts/jquery-migrate-1.2.1.min.js"
            ));
        }
    }
}