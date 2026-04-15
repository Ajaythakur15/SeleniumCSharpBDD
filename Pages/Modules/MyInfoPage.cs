using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class MyInfoPage : BasePage, IModulePage
    {
        public MyInfoPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "My Info";

        public bool IsLoaded()
        {
            return UrlContains("/pim/viewPersonalDetails") &&
                   HeaderContains("Personal Details") &&
                   ElementExists(By.XPath("//a[normalize-space()='Personal Details']"));
        }
    }
}
