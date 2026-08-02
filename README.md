# ASP.NET Core Playground

A curated collection of modern, production-ready ASP.NET Core samples demonstrating best practices for common development scenarios.

## Samples

| Sample | Description | Key Concepts |
|--------|-------------|--------------|
| [Configuration](./samples/configuration) | Strongly-typed configuration using the Options Pattern | `IOptions<T>`, `appsettings.json`, Dependency Injection |
| [Sending Email](./samples/sending-email) | Email sending with MailKit and `IEmailSender` abstraction | `IEmailSender`, MailKit/MimeKit, `IFormFile`, User Secrets |
| [JWT Authentication](./samples/jwt-authentication) | JWT-based authentication with ASP.NET Core Identity | JWT Bearer tokens, Identity, EF Core, Role-based auth |

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Visual Studio 2022 / VS Code / Rider
- SQL Server LocalDB (for JWT sample) or any SQL Server instance

### Running a Sample

```bash
# Clone the repository
git clone https://github.com/your-username/AspNetCorePlayground.git
cd AspNetCorePlayground

# Navigate to a sample
cd samples/configuration

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

### Configuration

Each sample uses `appsettings.json` for configuration with placeholder values. For local development:

1. Copy `appsettings.json` to `appsettings.Development.json`
2. Update with your actual values
3. For secrets (passwords, API keys), use **User Secrets**:

```bash
dotnet user-secrets init
dotnet user-secrets set "SMTPSettings:Password" "your-real-password"
dotnet user-secrets set "Jwt:Key" "your-super-secret-key-at-least-32-characters-long"
```

> **Never commit real secrets to source control.** All samples include `.gitignore` rules to exclude `appsettings.Development.json` and user secrets.

## Repository Structure

```
AspNetCorePlayground/
├── AspNetCorePlayground.sln          # Root solution file
├── .gitignore                        # .NET/VS ignore rules
├── README.md                         # This file
├── .github/
│   └── workflows/
│       └── build.yml                 # CI/CD pipeline
└── samples/
    ├── configuration/                # Options Pattern demo
    ├── sending-email/                # Email with MailKit demo
    └── jwt-authentication/           # JWT Auth with Identity demo
```

## Common Patterns Across Samples

- **Modern .NET 8** with nullable reference types and implicit usings
- **Minimal APIs / Controllers** with file-scoped namespaces
- **Options Pattern** for strongly-typed configuration
- **Dependency Injection** throughout
- **Security-first**: No hardcoded secrets, User Secrets integration
- **Clean architecture** within each sample

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Ensure all samples build: `dotnet build AspNetCorePlayground.sln`
5. Submit a Pull Request

## License

MIT License - feel free to use these samples in your projects.

## Resources

- [ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core)
- [Options Pattern](https://learn.microsoft.com/aspnet/core/fundamentals/configuration/options)
- [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
- [JWT Bearer Authentication](https://learn.microsoft.com/aspnet/core/security/authentication/jwt)
- [MailKit Documentation](https://github.com/jstedfast/MailKit)
