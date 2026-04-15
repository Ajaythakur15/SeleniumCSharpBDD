using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class AdminPage : BasePage, IModulePage
    {
        public AdminPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Admin";

        public bool IsLoaded()
        {
            return UrlContains("/admin/") &&
                   HeaderContains("User Management") &&
                   ElementExists(By.XPath("//label[normalize-space()='Username']")) &&
                   ElementExists(By.XPath("//button[normalize-space()='Search']"));
        }
    }
}
