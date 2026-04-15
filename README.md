# Selenium C# BDD POC

This POC uses C#, NUnit, SpecFlow Gherkin features, Selenium WebDriver, ExtentReports, and Docker.

In the .NET ecosystem, SpecFlow provides the Cucumber-style Gherkin BDD layer.

## Project Structure

- `Features/` - Gherkin feature files.
- `StepDefinitions/` - Step bindings for feature steps.
- `Pages/` - Page object classes.
- `Pages/Modules/` - Module-specific page objects for Admin, PIM, Leave, Time, Recruitment, My Info, Performance, Dashboard, Directory, Maintenance, Claim, and Buzz.
- `Drivers/` - WebDriver creation for local Chrome or Selenium Grid.
- `Hooks/` - SpecFlow hooks for browser setup, teardown, and reports.
- `Utils/` - Reporting and screenshot helpers.

## Covered OrangeHRM Flows

- Login as the demo administrator.
- Smoke navigation for Admin, PIM, Leave, Time, Recruitment, My Info, Performance, Dashboard, Directory, Maintenance, Claim, and Buzz.
- PIM employee create, search, and delete flow.
- Extent report generation and failure screenshots.

## Run Locally

Install Chrome on the machine, then run:

```powershell
dotnet test
```

To run Chrome headless:

```powershell
$env:HEADLESS="true"
dotnet test
```

To write reports to the project-level `TestResults` folder:

```powershell
$env:REPORT_DIR="C:\QA\SeleniumCSharpBDD\TestResults"
dotnet test
```

## Configuration

The framework reads configuration from environment variables.

| Variable | Default | Purpose |
| --- | --- | --- |
| `APP_URL` | `https://opensource-demo.orangehrmlive.com/` | Application URL under test. |
| `BROWSER` | `chrome` | Browser name. Chrome is currently supported. |
| `HEADLESS` | `true` | Runs Chrome in headless mode when true. |
| `SELENIUM_REMOTE_URL` | empty | Empty means local Chrome. Use a Selenium Grid URL for Docker/Grid execution. |
| `REPORT_DIR` | `<project root>/TestResults` | Extent report and screenshot output folder. |
| `RUN_NAME` | timestamp | Report run folder name when `REPORT_DIR` is not set. |
| `DRIVER_STARTUP_RETRIES` | `2` | Retries only transient WebDriver startup/Grid connection failures. |
| `EXPLICIT_WAIT_SECONDS` | `20` | Default explicit wait timeout. |
| `CHROME_BINARY_PATH` | empty | Optional path to a specific local Chrome executable. |
| `CHROME_USER_DATA_DIR` | temp folder | Optional Chrome profile folder for local troubleshooting. |

Use `.env.example` as the starting point for local values.

## Chrome Startup Issues On Windows

If Chrome fails with `crashpad`, `DevToolsActivePort`, or `Access is denied`, clean up local browser state first:

```powershell
Get-Process chrome,chromedriver -ErrorAction SilentlyContinue | Stop-Process -Force
```

Then run with an isolated Chrome profile:

```powershell
$env:HEADLESS="true"
$env:CHROME_USER_DATA_DIR="C:\Temp\orangehrm-chrome-profile"
dotnet test
```

If it still fails, delete the temporary profile folder and retry:

```powershell
Remove-Item "C:\Temp\orangehrm-chrome-profile" -Recurse -Force
```

The framework already passes Chrome-safe startup options:

- isolated `--user-data-dir`
- `--headless=new`
- `--disable-crash-reporter`
- `--disable-breakpad`
- `--remote-debugging-port=0`
- `--no-first-run`
- `--no-default-browser-check`

## Run Against Docker Grid From Host

Start Selenium Grid only:

```powershell
docker compose up -d selenium
```

Run tests from Windows against the Docker browser:

```powershell
$env:SELENIUM_REMOTE_URL="http://localhost:4444/wd/hub"
$env:HEADLESS="true"
$env:REPORT_DIR="C:\QA\SeleniumCSharpBDD\TestResults"
dotnet test
```

Open Grid UI:

```text
http://localhost:4444/ui
```

Open the noVNC browser view:

```text
http://localhost:7900
```

## Docker Desktop Troubleshooting On Windows

If Docker reports that the API pipe is unavailable, Docker Desktop or the Linux engine is not running. Check it with:

```powershell
docker version
docker context ls
```

Recommended recovery steps:

1. Start Docker Desktop.
2. Wait until it says the engine is running.
3. Use the default desktop Linux context:

```powershell
docker context use desktop-linux
```

4. Restart WSL if Docker is still stuck:

```powershell
wsl --shutdown
```

5. Start Docker Desktop again and retry:

```powershell
docker compose up --build --abort-on-container-exit
```

If `http://localhost:4444/ui` does not open, check container health:

```powershell
docker compose ps
docker compose logs selenium
```

The Grid status endpoint should respond here:

```text
http://localhost:4444/status
```

## Run With Docker

Run the Selenium Chrome container and test container:

```powershell
docker compose up --build --abort-on-container-exit
```

Selenium Grid is available at:

```text
http://localhost:4444
```

The browser noVNC view is available at:

```text
http://localhost:7900
```

## Reports

ExtentReports output is written to:

```text
TestResults/<run timestamp>/ExtentReport.html
```

Failure screenshots are written to:

```text
TestResults/<run timestamp>/Screenshots/
```

Set `REPORT_DIR` when you need a fixed output folder, for example in Docker or CI.

## Test Tags

Use SpecFlow/NUnit categories to run focused suites:

```powershell
dotnet test --filter "TestCategory=smoke"
dotnet test --filter "TestCategory=pim"
dotnet test --filter "TestCategory=docker"
dotnet test --filter "TestCategory=regression"
```

## CI

GitHub Actions Docker Grid execution is defined in:

```text
.github/workflows/e2e-docker.yml
```
