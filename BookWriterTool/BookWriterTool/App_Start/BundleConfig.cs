﻿using System.Web;
using System.Web.Optimization;

namespace BookWriterTool
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
     bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jquery.mobile*"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/customcss").Include("~/Content/Book.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                  "~/Content/themes/base/jquery.ui.core.css",
                  "~/Content/themes/base/jquery.ui.resizable.css",
                  "~/Content/themes/base/jquery.ui.selectable.css",
                  "~/Content/themes/base/jquery.ui.accordion.css",
                  "~/Content/themes/base/jquery.ui.autocomplete.css",
                  "~/Content/themes/base/jquery.ui.button.css",
                  "~/Content/themes/base/jquery.ui.dialog.css",
                  "~/Content/themes/base/jquery.ui.slider.css",
                  "~/Content/themes/base/jquery.ui.tabs.css",
                  "~/Content/themes/base/jquery.ui.datepicker.css",
                  "~/Content/themes/base/jquery.ui.progressbar.css",
                  "~/Content/themes/base/jquery.ui.theme.css"));
            bundles.Add(new StyleBundle("~/Content/themes/sunny/css").Include(
                 "~/Content/themes/sunny/jquery.ui.core.css",
                 "~/Content/themes/sunny/jquery.ui.resizable.css",
                 "~/Content/themes/sunny/jquery.ui.selectable.css",
                 "~/Content/themes/sunny/jquery.ui.accordion.css",
                 "~/Content/themes/sunny/jquery.ui.autocomplete.css",
                 "~/Content/themes/sunny/jquery.ui.button.css",
                 "~/Content/themes/sunny/jquery.ui.dialog.css",
                 "~/Content/themes/sunny/jquery.ui.slider.css",
                 "~/Content/themes/sunny/jquery.ui.tabs.css",
                 "~/Content/themes/sunny/jquery.ui.datepicker.css",
                 "~/Content/themes/sunny/jquery.ui.progressbar.css",
                 "~/Content/themes/sunny/jquery.ui.theme.css"));
            bundles.Add(new StyleBundle("~/Content/themes/black-tie/css").Include(
      "~/Content/themes/black-tie/jquery.ui.core.css",
      "~/Content/themes/black-tie/jquery.ui.resizable.css",
      "~/Content/themes/black-tie/jquery.ui.selectable.css",
      "~/Content/themes/black-tie/jquery.ui.accordion.css",
      "~/Content/themes/black-tie/jquery.ui.autocomplete.css",
      "~/Content/themes/black-tie/jquery.ui.button.css",
      "~/Content/themes/black-tie/jquery.ui.dialog.css",
      "~/Content/themes/black-tie/jquery.ui.slider.css",
      "~/Content/themes/black-tie/jquery.ui.tabs.css",
      "~/Content/themes/black-tie/jquery.ui.datepicker.css",
      "~/Content/themes/black-tie/jquery.ui.progressbar.css",
      "~/Content/themes/black-tie/jquery.ui.theme.css"));
        }
    }
}