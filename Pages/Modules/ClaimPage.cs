using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class ClaimPage : BasePage, IModulePage
    {
        public ClaimPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Claim";

        public bool IsLoaded()
        {
            return UrlContains("/claim/") &&
                   HeaderContains("Claim") &&
                   ElementExists(By.XPath("//a[normalize-space()='Submit Claim' or normalize-space()='Employee Claims']"));
        }
    }
}
