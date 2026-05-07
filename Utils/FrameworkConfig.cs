using System;
using System.IO;
using System.Text.Json;

namespace SeleniumCSharpBDD.Utils
{
    public static class FrameworkConfig
    {
        private static readonly Lazy<AppSettings> Settings = new(LoadSettings);

        private static readonly Lazy<string> CurrentRunName = new(() => Get("RUN_NAME", DateTime.Now.ToString("yyyyMMdd_HHmmss")));

        public static string AppUrl => Get("APP_URL", Settings.Value.AppUrl).TrimEnd('/');

        public static bool Headless => GetBool("HEADLESS", Settings.Value.Headless);

        public static string SeleniumRemoteUrl => Get("SELENIUM_REMOTE_URL", Settings.Value.SeleniumRemoteUrl);

        public static string Browser => Get("BROWSER", Settings.Value.Browser).ToLowerInvariant();

        public static string ChromeBinaryPath => Get("CHROME_BINARY_PATH", Settings.Value.ChromeBinaryPath);

        public static string ChromeUserDataDir => Get("CHROME_USER_DATA_DIR", Settings.Value.ChromeUserDataDir);

        public static string ReportDirectory => Path.GetFullPath(
            Path.Combine(ReportBaseDirectory, RunName));

        public static string ReportBaseDirectory => Path.GetFullPath(
            Get("REPORT_DIR", Path.Combine(ProjectRoot, "TestResults")));

        public static string RunName => CurrentRunName.Value;

        public static int DriverStartupRetries => GetInt("DRIVER_STARTUP_RETRIES", Settings.Value.DriverStartupRetries);

        public static int ExplicitWaitSeconds => GetInt("EXPLICIT_WAIT_SECONDS", Settings.Value.ExplicitWaitSeconds);

        public static string ProjectRoot => ResolveProjectRoot();

        private static string Get(string key, string defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        private static bool GetBool(string key, bool defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private static int GetInt(string key, int defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return int.TryParse(value, out var parsedValue) ? parsedValue : defaultValue;
        }

        private static AppSettings LoadSettings()
        {
            var appSettingsPath = Path.Combine(ProjectRoot, "appsettings.json");

            if (!File.Exists(appSettingsPath))
            {
                return new AppSettings();
            }

            var json = File.ReadAllText(appSettingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new AppSettings();
        }

        private static string ResolveProjectRoot()
        {
            var configuredRoot = Environment.GetEnvironmentVariable("PROJECT_ROOT");

            if (!string.IsNullOrWhiteSpace(configuredRoot))
            {
                return Path.GetFullPath(configuredRoot);
            }

            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (currentDirectory != null)
            {
                if (File.Exists(Path.Combine(currentDirectory.FullName, "SeleniumCSharpBDD.csproj")))
                {
                    return currentDirectory.FullName;
                }

                currentDirectory = currentDirectory.Parent;
            }

            return Directory.GetCurrentDirectory();
        }
    }

    public class AppSettings
    {
        public string AppUrl { get; set; } = "https://opensource-demo.orangehrmlive.com/";

        public string Browser { get; set; } = "chrome";

        public bool Headless { get; set; } = true;

        public string SeleniumRemoteUrl { get; set; } = string.Empty;

        public string ChromeBinaryPath { get; set; } = string.Empty;

        public string ChromeUserDataDir { get; set; } = string.Empty;

        public int DriverStartupRetries { get; set; } = 2;

        public int ExplicitWaitSeconds { get; set; } = 20;
    }
}
