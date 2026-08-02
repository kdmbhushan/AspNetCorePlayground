# Configuration Sample - Options Pattern

Demonstrates strongly-typed configuration in ASP.NET Core using the **Options Pattern** with `IOptions<T>`.

## What You'll Learn

- Binding configuration sections to POCO classes
- Using `IOptions<T>` for dependency injection
- Separating configuration from code
- Development vs Production configuration

## Project Structure

```
samples/configuration/
├── Configuration.csproj
├── Program.cs                    # App entry point with Options registration
├── appsettings.json              # Configuration with placeholders
├── appsettings.Development.json  # Development overrides (gitignored)
├── CompanySettings.cs            # Strongly-typed settings class
├── Controllers/
│   └── HomeController.cs         # Uses IOptions<CompanySettings>
├── Models/
│   └── ErrorViewModel.cs
└── Views/
    ├── Home/
    │   ├── Index.cshtml
    │   ├── CompanyDetails.cshtml # Displays settings
    │   └── Privacy.cshtml
    └── Shared/
        ├── _Layout.cshtml
        ├── _ViewImports.cshtml
        ├── _ViewStart.cshtml
        └── Error.cshtml
```

## Key Concepts

### 1. Settings Class

```csharp
public class CompanySettings
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
}
```

### 2. Registration in Program.cs

```csharp
builder.Services.Configure<CompanySettings>(
    builder.Configuration.GetSection("CompanySettings"));
```

### 3. Injection in Controller

```csharp
public class HomeController : Controller
{
    private readonly CompanySettings _companySettings;

    public HomeController(IOptions<CompanySettings> companySettings)
    {
        _companySettings = companySettings.Value;
    }
}
```

## Configuration

`appsettings.json`:
```json
{
  "CompanySettings": {
    "Name": "Your Company Name",
    "Email": "your-email@example.com",
    "Contact": "your-contact-number"
  }
}
```

For local development, create `appsettings.Development.json` with real values (this file is gitignored).

## Running

```bash
cd samples/configuration
dotnet run
```

Navigate to `/Home/CompanyDetails` to see the bound configuration values.

## Related Documentation

- [Options Pattern in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/configuration/options)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/configuration)
