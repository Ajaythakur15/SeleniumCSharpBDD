using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumCSharpBDD.Utils;

namespace SeleniumCSharpBDD.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(FrameworkConfig.ExplicitWaitSeconds));
        }

        private IWebElement Username => WaitForVisible(By.Name("username"));
        private IWebElement Password => WaitForVisible(By.Name("password"));
        private IWebElement LoginBtn => WaitForClickable(By.CssSelector("button[type='submit']"));

        public void Navigate()
        {
            driver.Navigate().GoToUrl(FrameworkConfig.AppUrl);
            WaitForVisible(By.Name("username"));
        }

        public void Login(string user, string pass)
        {
            Username.Clear();
            Username.SendKeys(user);
            Password.Clear();
            Password.SendKeys(pass);
            LoginBtn.Click();
        }

        public bool IsDashboardDisplayed()
        {
            return wait.Until(webDriver =>
                webDriver.Url.Contains("dashboard", StringComparison.OrdinalIgnoreCase));
        }

        private IWebElement WaitForVisible(By locator)
        {
            return wait.Until(webDriver =>
            {
                var elements = webDriver.FindElements(locator);
                var element = elements.FirstOrDefault(candidate => candidate.Displayed);
                return element;
            });
        }

        private IWebElement WaitForClickable(By locator)
        {
            return wait.Until(webDriver =>
            {
                var elements = webDriver.FindElements(locator);
                var element = elements.FirstOrDefault(candidate => candidate.Displayed && candidate.Enabled);
                return element;
            });
        }
    }
}
