using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class PimModulePage : BasePage, IModulePage
    {
        public PimModulePage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "PIM";

        public bool IsLoaded()
        {
            return UrlContains("/pim/") &&
                   ElementExists(By.XPath("//a[normalize-space()='Employee List']")) &&
                   ElementExists(By.XPath("//a[normalize-space()='Add Employee']"));
        }
    }
}
