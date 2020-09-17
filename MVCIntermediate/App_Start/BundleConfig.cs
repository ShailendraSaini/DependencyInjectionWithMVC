namespace MVCIntermediate
{
    using System.Web.Optimization;

    /// <summary>
    ///     BundleConfig Class
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        ///     Register Bundle method
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundle(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/scripts/*.js"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
