using NUnit.Framework;
using AventStack.ExtentReports;
using SeleniumCSharpBDD.Utils;

namespace SeleniumCSharpBDD
{
    public class SimpleExtentTest
    {
        [Test]
        public void TestReport()
        {
            var extent = ExtentReportManager.GetInstance();
            var test = extent.CreateTest("POC Test");

            test.Pass("Report is working");

            extent.Flush();
        }
    }
}