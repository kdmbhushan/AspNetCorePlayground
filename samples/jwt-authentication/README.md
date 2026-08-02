# JWT Authentication Sample - ASP.NET Core Identity & JWT Bearer

Demonstrates JWT-based authentication and authorization in ASP.NET Core using **ASP.NET Core Identity** with Entity Framework Core.

## What You'll Learn

- JWT Bearer token authentication
- User registration and login endpoints
- Role-based authorization
- ASP.NET Core Identity with EF Core
- Secure token configuration with User Secrets
- Protected API endpoints

## Project Structure

```
samples/jwt-authentication/
├── JwtAuthentication.csproj
├── Program.cs                            # Auth & Identity configuration
├── appsettings.json                      # JWT & DB config with placeholders
├── appsettings.Development.json          # Development overrides (gitignored)
├── DatabaseContext/
│   └── ApplicationDbContext.cs           # EF Core Identity DbContext
├── Controllers/
│   ├── UserAuthController.cs             # /api/UserAuth/login, /register
│   └── WeatherForecastController.cs      # Protected endpoint example
├── Models/
│   ├── UserLogin.cs
│   ├── UserRegistration.cs
│   └── WeatherForecast.cs
└── Properties/
    └── launchSettings.json
```

## Key Concepts

### 1. Identity & EF Core Setup

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

### 2. JWT Bearer Authentication

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
```

### 3. Login Endpoint (Generates JWT)

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] UserLogin model)
{
    var user = await _userManager.FindByNameAsync(model.Username!);
    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in await _userManager.GetRolesAsync(user))
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256));

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
    }
    return Unauthorized();
}
```

### 4. Protected Endpoint

```csharp
[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IEnumerable<WeatherForecast> Get() => ...
}
```

## Configuration

`appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JwtAuthenticationDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "your-super-secret-key-at-least-32-characters-long",
    "Issuer": "JwtAuthenticationServer",
    "Audience": "JwtAuthenticationClient",
    "Subject": "JwtAuthenticationAccessToken"
  }
}
```

**Use User Secrets for the JWT key:**
```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "your-super-secret-key-at-least-32-characters-long"
```

> The JWT Key must be at least 32 characters (256 bits) for HMAC-SHA256.

## Database Setup

The sample uses SQL Server LocalDB by default. Run migrations:

```bash
cd samples/jwt-authentication
dotnet ef database update
```

Or change the connection string to point to your SQL Server instance.

## Running

```bash
cd samples/jwt-authentication
dotnet run
```

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/UserAuth/register` | Register new user |
| POST | `/api/UserAuth/login` | Login and get JWT token |
| GET | `/WeatherForecast` | Protected endpoint (requires Bearer token) |

### Testing with curl

```bash
# Register
curl -X POST http://localhost:5xxx/api/UserAuth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"Password123!"}'

# Login
curl -X POST http://localhost:5xxx/api/UserAuth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Password123!"}'

# Access protected endpoint (use token from login)
curl -X GET http://localhost:5xxx/WeatherForecast \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

## Security Notes

- **Never hardcode JWT keys** - Use User Secrets or environment variables
- **Use HTTPS in production** - Set `RequireHttpsMetadata = true`
- **Rotate keys periodically** - Implement key rotation strategy
- **Set appropriate expiration** - Short-lived access tokens, refresh tokens for longer sessions
- **Validate all claims** - Check issuer, audience, lifetime, signing key

## Related Documentation

- [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
- [JWT Bearer Authentication](https://learn.microsoft.com/aspnet/core/security/authentication/jwt)
- [Entity Framework Core](https://learn.microsoft.com/ef/core)
- [RFC 7519 - JWT](https://tools.ietf.org/html/rfc7519)
