using Shop2.Common;
using System.Web.Optimization;

namespace Shop2.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // gộp các file css ,js vào 1 file để tối tưu hóa cho website
            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/Assets/client/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/js/plugins").Include(
                 "~/Assets/admin/libs/jquery-ui/jquery-ui.min.js",
                 "~/Assets/admin/libs/mustache/mustache.js",
                 "~/Assets/admin/libs/numeral/numeral.js",
                 "~/Assets/admin/libs/jquery-validation/dist/jquery.validate.js",
                 "~/Assets/admin/libs/jquery-validation/dist/additional-methods.min.js",
                 "~/Assets/client/js/common.js"

            ));



            bundles.Add(new StyleBundle("~/css/base")
               .Include("~/Assets/client/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/css/style.css", new CssRewriteUrlTransform())
               .Include("~/Assets/client/font-awesome-4.6.3/css/font-awesome.css", new CssRewriteUrlTransform())
               .Include("~/Assets/admin/libs/jquery-ui/themes/smoothness/jquery-ui.min.css", new CssRewriteUrlTransform())
               .Include("~/Assets/client/css/style.css", new CssRewriteUrlTransform())
               .Include("~/Assets/client/client/Custom.css", new CssRewriteUrlTransform())
               );

            BundleTable.EnableOptimizations =  bool.Parse(ConfigHelper.GetByKey("EnableBundles"));
        }
    }
}
