using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace BlogWebsite.App_Start
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
                "~/Content/css/swipebox.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-3.3.1.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.swipebox.min.js",
                "~/Scripts/modernizr-2.6.2.js",
                "~/Scripts/modernizr.custom.69142.js",
                "~/Scripts/app.js",
                "~/Scripts/jquery-3.3.1.intellisense.js"

            ));
        }
    }
}