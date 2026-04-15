using OpenQA.Selenium;
using System;
using System.IO;

namespace SeleniumCSharpBDD.Utils
{
    public class ScreenshotHelper
    {
        public static string TakeScreenshot(IWebDriver driver)
        {
            var folder = Path.Combine(FrameworkConfig.ReportDirectory, "Screenshots");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var fullPath = Path.Combine(folder, fileName);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            return fullPath;
        }
    }
}
