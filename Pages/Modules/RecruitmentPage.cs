using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public class RecruitmentPage : BasePage, IModulePage
    {
        public RecruitmentPage(IWebDriver driver)
            : base(driver)
        {
        }

        public string ModuleName => "Recruitment";

        public bool IsLoaded()
        {
            return UrlContains("/recruitment/") &&
                   HeaderContains("Recruitment") &&
                   ElementExists(By.XPath("//a[normalize-space()='Candidates']")) &&
                   ElementExists(By.XPath("//button[normalize-space()='Add']"));
        }
    }
}
