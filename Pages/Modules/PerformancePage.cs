using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class PerformancePage : BasePage, IModulePage
    {
        public PerformancePage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Performance";

        public bool IsLoaded()
        {
            return UrlContains("/performance/") &&
                   HeaderContains("Performance") &&
                   ElementExists(By.XPath("//button[normalize-space()='Search']"));
        }
    }
}
