# Sending Email Sample - MailKit & IEmailSender

Demonstrates sending emails in ASP.NET Core using **MailKit/MimeKit** with a clean `IEmailSender` abstraction.

## What You'll Learn

- Abstracting email sending behind an interface
- Using MailKit/MimeKit for modern email capabilities
- Handling attachments with `IFormFile`
- Secure configuration with User Secrets
- Dependency Injection of email services

## Project Structure

```
samples/sending-email/
├── SendingEmail.csproj
├── Program.cs                         # DI registration
├── appsettings.json                   # SMTP config with placeholders
├── appsettings.Development.json       # Development overrides (gitignored)
├── Configuration/
│   └── SMTPSettings.cs                # Strongly-typed SMTP settings
├── Services/
│   ├── IEmailSender.cs                # Email sending abstraction
│   └── EmailSender.cs                 # MailKit implementation
├── Controllers/
│   ├── HomeController.cs
│   └── EmailController.cs             # Uses IEmailSender
├── Models/
│   ├── EmailViewModel.cs              # Form model with validation
│   └── ErrorViewModel.cs
└── Views/
    ├── Email/
    │   └── Index.cshtml               # Email form
    ├── Home/
    │   ├── Index.cshtml
    │   └── Privacy.cshtml
    └── Shared/
        ├── _Layout.cshtml
        ├── _ViewImports.cshtml
        ├── _ViewStart.cshtml
        └── Error.cshtml
```

## Key Concepts

### 1. Email Abstraction

```csharp
public interface IEmailSender
{
    Task SendEmailAsync(EmailViewModel model);
}
```

### 2. MailKit Implementation

```csharp
public class EmailSender : IEmailSender
{
    private readonly SMTPSettings _smtpSettings;

    public EmailSender(IOptions<SMTPSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(EmailViewModel model)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        message.To.Add(MailboxAddress.Parse(model.To));
        message.Subject = model.Subject;

        var bodyBuilder = new BodyBuilder { TextBody = model.Body };

        if (model.Attachment != null && model.Attachment.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await model.Attachment.CopyToAsync(memoryStream);
            bodyBuilder.Attachments.Add(model.Attachment.FileName, memoryStream.ToArray());
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
```

### 3. Registration

```csharp
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SMTPSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
```

### 4. Controller Usage

```csharp
public class EmailController : Controller
{
    private readonly IEmailSender _emailSender;

    public EmailController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _emailSender.SendEmailAsync(model);
            ViewBag.Message = "Email Sent Successfully";
        }
        return View("Index", model);
    }
}
```

## Configuration

`appsettings.json`:
```json
{
  "SMTPSettings": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "your-username",
    "Password": "your-app-password",
    "FromEmail": "your-email@example.com",
    "FromName": "Your Name"
  }
}
```

**Use User Secrets for passwords:**
```bash
dotnet user-secrets init
dotnet user-secrets set "SMTPSettings:Password" "your-real-app-password"
dotnet user-secrets set "SMTPSettings:Username" "your-email@gmail.com"
```

For Gmail, use an [App Password](https://support.google.com/accounts/answer/185833) instead of your account password.

## Running

```bash
cd samples/sending-email
dotnet run
```

Navigate to `/Email` to access the email form.

## Why MailKit?

- Cross-platform (Windows, Linux, macOS)
- Supports modern protocols (SMTP, IMAP, POP3)
- Async/await support
- Proper MIME handling
- Actively maintained

## Related Documentation

- [MailKit Documentation](https://github.com/jstedfast/MailKit)
- [MimeKit Documentation](https://github.com/jstedfast/MimeKit)
- [Send email in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/email)
