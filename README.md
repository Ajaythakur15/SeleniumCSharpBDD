# Selenium C# BDD Framework with SpecFlow, Docker Grid, and Extent Reports

This repository is a production-style UI automation framework built with:

- C#
- Selenium WebDriver
- SpecFlow for BDD
- NUnit
- Dockerized Selenium Grid
- Extent Reports
- GitHub Actions CI

It uses the OrangeHRM demo application as the system under test:

- `https://opensource-demo.orangehrmlive.com/`

## Why This Project Matters

This project is designed to demonstrate the kind of automation framework expected in a real QA or SDET environment:

- readable BDD scenarios
- maintainable Page Object Model structure
- local and Docker Grid execution
- CI-ready workflow
- environment-based configuration
- reusable driver setup
- automatic reporting and screenshots
- test data cleanup after execution

## Key Features

- SpecFlow feature files with tagged execution
- Selenium Grid support through Docker
- Local Chrome and remote Grid browser switching
- Explicit waits and resilient click handling for CI/headless stability
- Extent report generation with screenshots on failure
- Timestamped test output folders
- GitHub Actions workflow for automated execution
- Module-specific page objects for OrangeHRM
- PIM employee create/search/delete end-to-end scenario

## Current Test Coverage

### Login

- Valid login with admin credentials

### Module Validation

- Admin
- PIM
- Leave
- Time
- Recruitment
- My Info
- Performance
- Dashboard
- Directory
- Maintenance
- Claim
- Buzz

### End-to-End Flow

- Create employee in PIM
- Search employee in PIM
- Delete employee in PIM
- Verify employee no longer appears in results

## Framework Architecture

```text
SeleniumCSharpBDD
├── .github/workflows/        # GitHub Actions CI
├── Drivers/                  # Local/Grid WebDriver creation
├── Features/                 # SpecFlow feature files
├── Hooks/                    # Before/After scenario hooks
├── Pages/                    # Page objects
│   └── Modules/              # Module-specific page objects
├── StepDefinitions/          # Step bindings
├── Utils/                    # Config, reporting, screenshots, test data helpers
├── docker-compose.yml        # Selenium Grid + test runner
├── Dockerfile                # Test execution image
├── appsettings.json          # Default framework settings
└── SeleniumCSharpBDD.csproj  # .NET test project
```

## Tech Stack

| Area | Technology |
| --- | --- |
| Language | C# (.NET 8) |
| Test Runner | NUnit |
| BDD | SpecFlow |
| UI Automation | Selenium WebDriver |
| Reporting | Extent Reports |
| Container Execution | Docker + Selenium Grid |
| CI | GitHub Actions |

## Configuration

The framework supports both `appsettings.json` and environment-variable overrides.

### `appsettings.json`

Default settings live in:

```text
appsettings.json
```

### Supported Environment Variables

| Variable | Default | Purpose |
| --- | --- | --- |
| `APP_URL` | `https://opensource-demo.orangehrmlive.com/` | Target application URL |
| `BROWSER` | `chrome` | Browser name |
| `HEADLESS` | `true` | Headless browser execution |
| `SELENIUM_REMOTE_URL` | empty | Empty = local execution, populated = Grid execution |
| `REPORT_DIR` | `TestResults/<timestamp>` | Report and screenshot output folder |
| `RUN_NAME` | timestamp | Run folder name |
| `DRIVER_STARTUP_RETRIES` | `2` | Retry count for transient browser/Grid startup issues |
| `EXPLICIT_WAIT_SECONDS` | `20` | Default explicit wait timeout |
| `CHROME_BINARY_PATH` | empty | Optional explicit Chrome path |
| `CHROME_USER_DATA_DIR` | temp profile | Optional Chrome user data directory |

Use this file as a local template:

```text
.env.example
```

## Run the Framework

### 1. Local execution

```powershell
dotnet test
```

### 2. Local headless execution

```powershell
$env:HEADLESS="true"
dotnet test
```

### 3. Local execution with fixed report folder

```powershell
$env:REPORT_DIR="C:\QA\SeleniumCSharpBDD\TestResults"
dotnet test
```

### 4. Run against Docker Grid from host

Start Grid:

```powershell
docker compose up -d selenium
```

Run tests from Windows against the Grid:

```powershell
$env:SELENIUM_REMOTE_URL="http://localhost:4444/wd/hub"
$env:HEADLESS="true"
$env:REPORT_DIR="C:\QA\SeleniumCSharpBDD\TestResults"
dotnet test
```

### 5. Run everything with Docker Compose

```powershell
docker compose up --build --abort-on-container-exit
```

## Selenium Grid URLs

- Grid UI: `http://localhost:4444/ui`
- Grid status: `http://localhost:4444/status`
- noVNC browser view: `http://localhost:7900`

## Test Tags

Run targeted test sets using NUnit category filters generated from SpecFlow tags:

```powershell
dotnet test --filter "TestCategory=smoke"
dotnet test --filter "TestCategory=regression"
dotnet test --filter "TestCategory=pim"
dotnet test --filter "TestCategory=docker"
dotnet test --filter "TestCategory=login"
```

## Reports

The framework generates:

- Extent HTML report
- Failure screenshots

Default output:

```text
TestResults/<run timestamp>/ExtentReport.html
TestResults/<run timestamp>/Screenshots/
```

## CI Pipeline

GitHub Actions workflow:

```text
.github/workflows/e2e-docker.yml
```

What it does:

- checks out the repository
- restores and builds the .NET test project
- starts Selenium standalone Chrome as a service
- runs the tagged test suite
- uploads the Extent report as a workflow artifact

## Stability Enhancements Included

- explicit waits instead of fragile timing assumptions
- local/Grid driver switching from config
- retry logic for transient WebDriver startup failures
- scroll and JavaScript click fallback for intercepted clicks
- isolated Chrome profile handling
- test data registry and cleanup support

## Common Windows Troubleshooting

### Chrome crash / crashpad / access denied

```powershell
Get-Process chrome,chromedriver -ErrorAction SilentlyContinue | Stop-Process -Force
```

Then retry with an isolated Chrome profile:

```powershell
$env:HEADLESS="true"
$env:CHROME_USER_DATA_DIR="C:\Temp\orangehrm-chrome-profile"
dotnet test
```

### Docker engine or API pipe issue

```powershell
docker version
docker context ls
docker context use desktop-linux
```

If Docker still appears stuck:

```powershell
wsl --shutdown
```

Then reopen Docker Desktop and retry.

## Portfolio / Interview Talking Points

This project demonstrates:

- building a reusable automation framework from scratch
- integrating BDD with SpecFlow and NUnit
- designing maintainable page-object architecture
- supporting local and containerized execution
- handling flaky UI issues in CI/headless environments
- setting up GitHub Actions for browser automation
- generating and preserving test execution artifacts

## Recommended Next Improvements

- move feature files and generated outputs into a conventional `src/test` style layout if the project grows
- add richer test data builders for employee and admin flows
- add API or database-backed cleanup where available
- split smoke and regression workflows into separate CI jobs
- add screenshots or badges to the GitHub landing page

## Repository

- GitHub: [ajay9761/SeleniumCSharpBDD](https://github.com/ajay9761/SeleniumCSharpBDD)
