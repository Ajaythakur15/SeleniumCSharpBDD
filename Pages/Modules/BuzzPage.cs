using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class BuzzPage : BasePage, IModulePage
    {
        public BuzzPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Buzz";

        public bool IsLoaded()
        {
            return UrlContains("/buzz/") &&
                   HeaderContains("Buzz") &&
                   ElementExists(By.XPath("//textarea[contains(@placeholder,'mind') or contains(@placeholder,'share')]"));
        }
    }
}
