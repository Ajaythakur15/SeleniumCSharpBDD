using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using AventStack.ExtentReports;
using SeleniumCSharpBDD.Pages;

namespace SeleniumCSharpBDD.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver driver;
        private readonly LoginPage loginPage;
        private readonly ExtentTest test;

        public LoginSteps(ScenarioContext context)
        {
            driver = context["Driver"] as IWebDriver;
            loginPage = new LoginPage(driver);
            test = context["ExtentTest"] as ExtentTest;
        }

        [Given(@"I navigate to login page")]
        public void GivenINavigateToLoginPage()
        {
            test.Info("Navigating to login page");
            loginPage.Navigate();
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            test.Info("Entering credentials");
            loginPage.Login("Admin", "admin123");
        }

        [Then(@"I should see dashboard")]
        public void ThenIShouldSeeDashboard()
        {
            Assert.That(loginPage.IsDashboardDisplayed(), Is.True);
            test.Pass("Dashboard verified successfully");
        }
    }
}
