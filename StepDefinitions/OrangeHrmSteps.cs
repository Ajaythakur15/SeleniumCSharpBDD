using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumCSharpBDD.Pages;
using SeleniumCSharpBDD.Pages.Modules;
using SeleniumCSharpBDD.Utils;
using TechTalk.SpecFlow;

namespace SeleniumCSharpBDD.StepDefinitions
{
    [Binding]
    public class OrangeHrmSteps
    {
        private readonly ScenarioContext context;
        private readonly IWebDriver driver;
        private readonly LoginPage loginPage;
        private readonly OrangeHrmHomePage homePage;
        private readonly PimPage pimPage;
        private readonly ExtentTest test;

        public OrangeHrmSteps(ScenarioContext context)
        {
            this.context = context;
            driver = context["Driver"] as IWebDriver;
            loginPage = new LoginPage(driver);
            homePage = new OrangeHrmHomePage(driver);
            pimPage = new PimPage(driver);
            test = context["ExtentTest"] as ExtentTest;
        }

        [Given(@"I am logged into OrangeHRM as an administrator")]
        public void GivenIAmLoggedIntoOrangeHrmAsAnAdministrator()
        {
            loginPage.Navigate();
            loginPage.Login("Admin", "admin123");
            Assert.That(loginPage.IsDashboardDisplayed(), Is.True);
        }

        [When(@"I open the ""(.*)"" module")]
        public void WhenIOpenTheModule(string moduleName)
        {
            test.Info($"Opening {moduleName}");
            homePage.OpenModule(moduleName);
        }

        [Then(@"the ""(.*)"" module should be displayed")]
        public void ThenTheModuleShouldBeDisplayed(string moduleName)
        {
            var modulePage = ModulePageFactory.Create(moduleName, driver);

            Assert.That(modulePage.IsLoaded(), Is.True, $"{modulePage.ModuleName} module did not load correctly.");
        }

        [When(@"I create a new employee in PIM")]
        public void WhenICreateANewEmployeeInPim()
        {
            var employee = pimPage.CreateEmployee();
            context["EmployeeId"] = employee.EmployeeId;
            TestDataRegistry.AddEmployee(context, employee.EmployeeId);
            test.Info($"Created employee {employee.FirstName} {employee.LastName} with ID {employee.EmployeeId}");
        }

        [Then(@"the employee should be searchable in PIM")]
        public void ThenTheEmployeeShouldBeSearchableInPim()
        {
            var employeeId = context["EmployeeId"] as string;

            Assert.That(pimPage.SearchEmployeeById(employeeId), Is.True);
            Assert.That(pimPage.IsEmployeePresent(employeeId), Is.True);
        }

        [When(@"I delete the employee from PIM")]
        public void WhenIDeleteTheEmployeeFromPim()
        {
            var employeeId = context["EmployeeId"] as string;
            pimPage.DeleteEmployee(employeeId);
            TestDataRegistry.RemoveEmployee(context, employeeId);
        }

        [Then(@"the employee should not appear in PIM search results")]
        public void ThenTheEmployeeShouldNotAppearInPimSearchResults()
        {
            var employeeId = context["EmployeeId"] as string;

            Assert.That(pimPage.SearchEmployeeById(employeeId), Is.True);
            Assert.That(pimPage.IsEmployeePresent(employeeId), Is.False);
        }
    }
}
