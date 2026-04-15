using System;
using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages.Modules
{
    public static class ModulePageFactory
    {
        public static IModulePage Create(string moduleName, IWebDriver driver)
        {
            return moduleName.ToLowerInvariant() switch
            {
                "admin" => new AdminPage(driver),
                "pim" => new PimModulePage(driver),
                "leave" => new LeavePage(driver),
                "time" => new TimePage(driver),
                "recruitment" => new RecruitmentPage(driver),
                "my info" => new MyInfoPage(driver),
                "performance" => new PerformancePage(driver),
                "dashboard" => new DashboardPage(driver),
                "directory" => new DirectoryPage(driver),
                "maintenance" => new MaintenancePage(driver),
                "claim" => new ClaimPage(driver),
                "buzz" => new BuzzPage(driver),
                _ => throw new NotSupportedException($"No module page object exists for '{moduleName}'.")
            };
        }
    }
}
