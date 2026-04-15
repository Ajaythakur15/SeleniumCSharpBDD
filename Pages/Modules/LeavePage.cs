using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class LeavePage : BasePage, IModulePage
    {
        public LeavePage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Leave";

        public bool IsLoaded()
        {
            return UrlContains("/leave/") &&
                   HeaderContains("Leave") &&
                   ElementExists(By.XPath("//label[normalize-space()='From Date']")) &&
                   ElementExists(By.XPath("//button[normalize-space()='Search']"));
        }
    }
}
