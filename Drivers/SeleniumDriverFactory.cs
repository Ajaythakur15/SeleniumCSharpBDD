using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SeleniumCSharpBDD.Utils;

namespace SeleniumCSharpBDD.Drivers
{
    public static class SeleniumDriverFactory
    {
        public static IWebDriver GetDriver()
        {
            if (FrameworkConfig.Browser != "chrome")
            {
                throw new NotSupportedException($"Browser '{FrameworkConfig.Browser}' is not supported yet.");
            }

            var options = BuildChromeOptions();

            var driver = CreateDriverWithRetry(options);
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

            return driver;
        }

        private static IWebDriver CreateDriverWithRetry(ChromeOptions options)
        {
            Exception lastException = null;

            for (var attempt = 1; attempt <= FrameworkConfig.DriverStartupRetries + 1; attempt++)
            {
                try
                {
                    return string.IsNullOrWhiteSpace(FrameworkConfig.SeleniumRemoteUrl)
                        ? CreateLocalChromeDriver(options)
                        : new RemoteWebDriver(new Uri(FrameworkConfig.SeleniumRemoteUrl), options.ToCapabilities(), TimeSpan.FromSeconds(120));
                }
                catch (WebDriverException ex) when (IsTransientStartupFailure(ex) && attempt <= FrameworkConfig.DriverStartupRetries)
                {
                    lastException = ex;
                    Thread.Sleep(TimeSpan.FromSeconds(attempt * 2));
                }
            }

            throw lastException ?? new WebDriverException("Unable to create WebDriver session.");
        }

        private static IWebDriver CreateLocalChromeDriver(ChromeOptions options)
        {
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new ChromeDriver(service, options, TimeSpan.FromSeconds(120));
        }

        private static bool IsTransientStartupFailure(WebDriverException exception)
        {
            var message = exception.Message;

            return message.Contains("DevToolsActivePort", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("Chrome failed to start", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("session not created", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("disconnected", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("Unable to connect", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("connection refused", StringComparison.OrdinalIgnoreCase);
        }

        private static ChromeOptions BuildChromeOptions()
        {
            var options = new ChromeOptions();
            var userDataDir = string.IsNullOrWhiteSpace(FrameworkConfig.ChromeUserDataDir)
                ? Path.Combine(Path.GetTempPath(), $"orangehrm-chrome-{Guid.NewGuid():N}")
                : FrameworkConfig.ChromeUserDataDir;

            if (!string.IsNullOrWhiteSpace(FrameworkConfig.ChromeBinaryPath))
            {
                options.BinaryLocation = FrameworkConfig.ChromeBinaryPath;
            }

            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-breakpad");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-crash-reporter");
            options.AddArgument("--no-default-browser-check");
            options.AddArgument("--no-first-run");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--remote-allow-origins=*");
            options.AddArgument("--remote-debugging-port=0");
            options.AddArgument($"--user-data-dir={userDataDir}");
            options.AddArgument("--window-size=1920,1080");

            if (FrameworkConfig.Headless)
            {
                options.AddArgument("--headless=new");
            }

            return options;
        }
    }
}
