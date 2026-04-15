using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class DashboardPage : BasePage, IModulePage
    {
        public DashboardPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Dashboard";

        public bool IsLoaded()
        {
            return UrlContains("/dashboard/") &&
                   HeaderContains("Dashboard") &&
                   ElementExists(By.XPath("//*[contains(normalize-space(),'Time at Work') or contains(normalize-space(),'Quick Launch')]"));
        }
    }
}
