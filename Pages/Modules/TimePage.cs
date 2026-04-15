using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class TimePage : BasePage, IModulePage
    {
        public TimePage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Time";

        public bool IsLoaded()
        {
            return UrlContains("/time/") &&
                   HeaderContains("Timesheets") &&
                   ElementExists(By.XPath("//label[normalize-space()='Employee Name']"));
        }
    }
}
