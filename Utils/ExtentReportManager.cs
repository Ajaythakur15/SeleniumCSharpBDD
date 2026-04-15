using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SeleniumCSharpBDD.Utils
{
    public class ExtentReportManager
    {
        private static ExtentReports extent;

        public static ExtentReports GetInstance()
        {
            if (extent == null)
            {
                var reportsDirectory = FrameworkConfig.ReportDirectory;
                Directory.CreateDirectory(reportsDirectory);

                var path = Path.Combine(reportsDirectory, "ExtentReport.html");

                var htmlReporter = new ExtentSparkReporter(path);
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
            }
            return extent;
        }
    }
}
