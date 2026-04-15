using OpenQA.Selenium;
using SeleniumCSharpBDD.Drivers;

namespace SeleniumCSharpBDD.Utils
{
    public static class DriverFactory
    {
        public static IWebDriver GetDriver()
        {
            return SeleniumDriverFactory.GetDriver();
        }
    }
}
