# Contributing

Thanks for contributing to this automation framework.

## Prerequisites

- .NET 8 SDK
- Docker Desktop
- Google Chrome for local execution

## Local Setup

Restore and build:

```powershell
dotnet restore
dotnet build --no-restore
```

Run locally:

```powershell
dotnet test
```

Run with Docker:

```powershell
docker compose up --build --abort-on-container-exit
```

## Project Structure

- `Features/` - SpecFlow feature files
- `StepDefinitions/` - scenario bindings
- `Pages/` - page objects and reusable UI interactions
- `Pages/Modules/` - module-specific OrangeHRM page validations
- `Drivers/` - local and Grid WebDriver setup
- `Hooks/` - scenario lifecycle and reporting hooks
- `Utils/` - configuration, screenshots, reports, and test-data helpers

## Contribution Guidelines

- Keep changes focused on the requested behavior.
- Follow the existing Page Object Model and SpecFlow patterns.
- Prefer explicit waits over `Thread.Sleep`.
- Add or update tags when adding new feature coverage.
- Reuse shared helpers before introducing new abstractions.
- Keep generated files and build output out of version control.

## Adding New Tests

1. Add or update a `.feature` file in `Features/`.
2. Add step bindings in `StepDefinitions/`.
3. Add or extend page objects in `Pages/` or `Pages/Modules/`.
4. Reuse `FrameworkConfig`, hooks, and reporting helpers where possible.
5. Run the impacted flow locally before pushing.

## Pull Request Checklist

- `dotnet build --no-restore` passes
- local or Docker test execution was validated for the changed flow
- report output still lands under `TestResults/<timestamp>/`
- screenshots and artifacts still work on failure
- README updated if execution or framework behavior changed
