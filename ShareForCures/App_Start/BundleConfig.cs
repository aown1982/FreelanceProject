using System.Web;
using System.Web.Optimization;

namespace ShareForCures
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));*/


            bundles.Add(new StyleBundle("~/Bundle/assets/css").Include(
                      "~/assets/css/animate.min.css",
                      "~/assets/css/bootstrap.min.css",
                      "~/assets/css/bootstrapValidator.css",
                      "~/assets/css/datatables.min.css",
                      "~/assets/css/daterangepicker.css",
                      "~/assets/css/font-awesome.min.css",
                      //"~/assets/css/build.css",
                      "~/assets/css/SocialMedia.css",
                      "~/assets/css/main.css",
                      "~/assets/css/custom.css"));

            bundles.Add(new StyleBundle("~/assets/revolutionCSS").Include(
                     "~/assets/revolution/css/settings.css",
                     "~/assets/revolution/css/layers.css",
                     "~/assets/revolution/css/navigation.css"));

            bundles.Add(new StyleBundle("~/assets/UserCSS").Include(
                     "~/assets/css/kendo/kendo.common.min.css",
                     "~/assets/css/kendo/kendo.flat.min.css",
                     "~/assets/jstree/themes/default/style.css",
                     "~/assets/css/kendo/kendo.default.mobile.min.css"));

            bundles.Add(new ScriptBundle("~/assets/MainJS").Include(
                    "~/Scripts/jquery-3.1.0.min.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/bootstrapValidator.js",
                    "~/Scripts/jquery.blockUI.js",
                    "~/Scripts/Custom/custom.js",
                    "~/Scripts/animatedModal.js",
                    "~/assets/js/jquery.dropotron.min.js",
                    "~/assets/js/jquery-migrate-3.0.0.min.js",
                    "~/assets/js/skel.min.js",
                    "~/assets/js/util.js",
                    "~/assets/js/main.js",
                    "~/assets/revolution/js/jquery.themepunch.tools.min.js",
                    "~/assets/revolution/js/jquery.themepunch.revolution.min.js"));

            bundles.Add(new ScriptBundle("~/assets/UserJS").Include(
                    "~/assets/jstree/jstree.js",
                    "~/assets/kendo/kendo.all.min.js",
                    "~/Scripts/moment.min.js",
                    "~/Scripts/daterangepicker.js",
                    "~/Scripts/datatables.min.js"));

        }
    }
}
