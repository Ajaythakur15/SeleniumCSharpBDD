using System.Linq;
using TechTalk.SpecFlow;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using SeleniumCSharpBDD.Drivers;
using SeleniumCSharpBDD.Pages;
using SeleniumCSharpBDD.Utils;

namespace SeleniumCSharpBDD.Hooks
{
    [Binding]
    public class ReportHooks
    {
        private static ExtentReports extent;

        private readonly ScenarioContext _context;
        private ExtentTest test;

        public ReportHooks(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeTestRun]
        public static void Setup()
        {
            extent = ExtentReportManager.GetInstance();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _context["Driver"] = SeleniumDriverFactory.GetDriver();
            test = extent.CreateTest(_context.ScenarioInfo.Title);
            _context["ExtentTest"] = test;
        }

        [AfterStep]
        public void AfterStep()
        {
            var stepType = _context.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepName = _context.StepContext.StepInfo.Text;

            if (_context.TestError == null)
            {
                test.Pass($"{stepType}: {stepName}");
            }
            else
            {
                var driver = _context.TryGetValue("Driver", out IWebDriver existingDriver)
                    ? existingDriver
                    : null;

                var failedStep = test.Fail($"{stepType}: {stepName}");

                if (driver != null)
                {
                    var screenshotPath = ScreenshotHelper.TakeScreenshot(driver);
                    failedStep.AddScreenCaptureFromPath(screenshotPath);
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_context.TryGetValue("Driver", out IWebDriver driver))
            {
                CleanupCreatedTestData(driver);
                driver.Quit();
                driver.Dispose();
            }
        }

        private void CleanupCreatedTestData(IWebDriver driver)
        {
            var employeeIds = TestDataRegistry.GetEmployees(_context);

            if (!employeeIds.Any())
            {
                return;
            }

            var pimPage = new PimPage(driver);

            foreach (var employeeId in employeeIds)
            {
                try
                {
                    if (pimPage.SearchEmployeeById(employeeId) && pimPage.IsEmployeePresent(employeeId))
                    {
                        pimPage.DeleteEmployee(employeeId);
                    }
                }
                catch (WebDriverException ex)
                {
                    test.Warning($"Unable to clean up employee '{employeeId}': {ex.Message}");
                }
            }
        }

        [AfterTestRun]
        public static void TearDown()
        {
            extent.Flush();
        }
    }
}
