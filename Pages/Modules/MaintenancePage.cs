using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class MaintenancePage : BasePage, IModulePage
    {
        public MaintenancePage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Maintenance";

        public bool IsLoaded()
        {
            return UrlContains("/maintenance/") &&
                   ElementExists(By.XPath("//*[contains(normalize-space(),'Administrator Access')]"));
        }
    }
}
