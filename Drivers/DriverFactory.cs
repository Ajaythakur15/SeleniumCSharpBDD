using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Drivers
{
    public class DriverFactory
    {
        public static IWebDriver GetDriver()
        {
            return SeleniumDriverFactory.GetDriver(); /*
                new Uri("http://127.0.0.1:4444/wd/hub"), // 👈 MUST
                options
                 Console.WriteLine("Running on Selenium Grid...");
            */
        }
    }
}
