using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumCSharpBDD.Utils;

namespace SeleniumCSharpBDD.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(FrameworkConfig.ExplicitWaitSeconds));
        }

        protected string AppUrl => FrameworkConfig.AppUrl;

        public void OpenModule(string moduleName)
        {
            var menuItem = WaitForVisible(By.XPath($"//span[normalize-space()='{moduleName}']/ancestor::a"));
            menuItem.Click();
        }

        protected IWebElement WaitForVisible(By locator)
        {
            return Wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed ? element : null;
            });
        }

        protected IWebElement WaitForClickable(By locator)
        {
            return Wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            });
        }

        protected void Click(By locator)
        {
            var element = WaitForClickable(locator);

            try
            {
                ScrollIntoView(element);
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                element = WaitForClickable(locator);
                ScrollIntoView(element);

                try
                {
                    element.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    JavaScriptClick(element);
                }
            }
            catch (StaleElementReferenceException)
            {
                element = WaitForClickable(locator);
                ScrollIntoView(element);
                element.Click();
            }
        }

        protected void Type(By locator, string value)
        {
            var element = WaitForVisible(locator);
            element.Clear();
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            element.SendKeys(value);
        }

        protected bool IsTextVisible(string text)
        {
            return Wait.Until(driver => driver.PageSource.Contains(text, StringComparison.OrdinalIgnoreCase));
        }

        protected bool UrlContains(string value)
        {
            return Wait.Until(driver => driver.Url.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        protected bool HeaderContains(string value)
        {
            return Wait.Until(driver => driver.FindElements(By.CssSelector("h6"))
                .Any(header => header.Displayed && header.Text.Contains(value, StringComparison.OrdinalIgnoreCase)));
        }

        protected bool ElementExists(By locator)
        {
            return Wait.Until(driver => driver.FindElements(locator).Any(element => element.Displayed));
        }

        private void ScrollIntoView(IWebElement element)
        {
            if (Driver is IJavaScriptExecutor jsExecutor)
            {
                jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);
            }
        }

        private void JavaScriptClick(IWebElement element)
        {
            if (Driver is not IJavaScriptExecutor jsExecutor)
            {
                throw new InvalidOperationException("Driver does not support JavaScript execution.");
            }

            jsExecutor.ExecuteScript("arguments[0].click();", element);
        }
    }
}
