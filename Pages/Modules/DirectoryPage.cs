using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class DirectoryPage : BasePage, IModulePage
    {
        public DirectoryPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Directory";

        public bool IsLoaded()
        {
            return UrlContains("/directory/") &&
                   HeaderContains("Directory") &&
                   ElementExists(By.XPath("//button[normalize-space()='Search']"));
        }
    }
}
