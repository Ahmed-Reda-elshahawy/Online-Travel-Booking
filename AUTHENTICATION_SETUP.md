# Authentication Setup Guide

## Overview
This solution implements separate authentication mechanisms for the API and MVC projects:
- **API Project (OnlineTravelBooking)**: JWT Token-based authentication
- **MVC Project (OnlineTravelBooking.MVC)**: Identity Cookie-based authentication

Both projects share the same `ApplicationDbContext` and `ApplicationUser` entity.

---

## Architecture

### Infrastructure Layer (`OnlineTravelBooking.Infrastructure`)
The infrastructure project now provides THREE authentication configuration methods:

#### 1. `AddInfrastructure(IConfiguration)`
- Configures core services (DbContext, Identity, repositories, services)
- **Does NOT configure authentication schemes**
- Must be called by both API and MVC projects

#### 2. `AddInfrastructureJwtAuthentication(IConfiguration)` ? **For API**
- Configures JWT Bearer token authentication
- Uses JWT claims for user identification
- Perfect for RESTful APIs and mobile clients
- Call this in **API Project's Program.cs**

#### 3. `AddInfrastructureIdentityAuthentication(IConfiguration)` ? **For MVC**
- Configures ASP.NET Identity cookie-based authentication
- Uses secure cookies for session management
- Perfect for traditional web applications
- Call this in **MVC Project's Program.cs**

#### 4. `AddInfrastructureAuthentication(IConfiguration)` (Legacy)
- Configures BOTH JWT and Identity schemes
- Use only if you need both in a single project
- Not recommended for clean separation

---

## API Project Setup (`OnlineTravelBooking/Program.cs`)

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. Add basic services and infrastructure
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);  // Core services only

// 2. Add JWT authentication specifically
builder.Services.AddInfrastructureJwtAuthentication(builder.Configuration);

var app = builder.Build();

// 3. Use authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```

### API Authentication Flow
1. User calls `/api/auth/login` with credentials
2. System validates credentials against database
3. Server generates JWT token using `IIdentityService.GenerateTokenAsync()`
4. Client receives token and includes it in Authorization header: `Bearer <token>`
5. Server validates token and allows access to protected endpoints

### API Example Controller
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FlightsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFlights()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // ... get flights
    }
}
```

---

## MVC Project Setup (`OnlineTravelBooking.MVC/Program.cs`)

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. Add services and infrastructure
builder.Services.AddInfrastructure(builder.Configuration);      // Core services
builder.Services.AddApplication();
builder.Services.AddControllersWithViews();

// 2. Add Identity cookie authentication specifically
builder.Services.AddInfrastructureIdentityAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// 3. Use authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### MVC Authentication Flow
1. User navigates to `/auth/login`
2. User submits login form with credentials
3. `AdminAuthService.LoginAsync()` validates credentials
4. System uses `SignInManager<ApplicationUser>.SignInAsync()` to create secure cookie
5. Browser automatically sends cookie with subsequent requests
6. Server validates cookie and allows access to protected pages

### MVC Example Controller
```csharp
public class BookingsController : Controller
{
    [Authorize]  // Uses default cookie authentication
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // ... get bookings
    }
}
```

---

## Key Differences

| Aspect | API (JWT) | MVC (Identity Cookies) |
|--------|-----------|----------------------|
| **Authentication Method** | Bearer token in Authorization header | Secure HTTP-only cookie |
| **Token Duration** | Short-lived (configured in appsettings) | Session-based (configurable) |
| **Refresh Mechanism** | Refresh token endpoint | Auto-managed by middleware |
| **Client Storage** | Local/session storage | Browser cookies |
| **CORS Support** | Required for cross-origin requests | Not needed for same-origin |
| **Stateless** | Yes (token contains all claims) | No (depends on server session) |
| **Use Case** | Mobile apps, SPAs, microservices | Traditional web apps, server-rendered pages |

---

## Configuration Files

### appsettings.json (API & MVC)
```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-minimum-32-characters-long",
    "Issuer": "OnlineTravelBookingAPI",
    "Audience": "OnlineTravelBookingUsers",
    "ExpiryMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=OnlineTravelBooking;..."
  }
}
```

---

## User Login

### API Login Flow
```csharp
// POST /api/auth/login
{
    "email": "user@example.com",
    "password": "password123"
}

// Response
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64encodedrefreshtoken",
    "expiresIn": 3600
}

// Subsequent API calls
GET /api/flights HTTP/1.1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### MVC Login Flow
```html
<!-- POST /auth/login -->
<form method="post">
    <input name="Email" type="email" />
    <input name="Password" type="password" />
    <button type="submit">Login</button>
</form>

<!-- Browser automatically sends cookies with subsequent requests -->
<!-- GET /bookings -->
<!-- Cookie: .AspNetCore.Identity.Application=... -->
```

---

## Logout

### API Logout
```csharp
// POST /api/auth/logout
// Just discard the token on client side or call refresh token revocation

// In IdentityService
public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
{
    var user = await _userManager.Users
        .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    
    if (user == null) return false;
    
    user.RefreshToken = null;
    user.RefreshTokenExpiry = null;
    
    var result = await _userManager.UpdateAsync(user);
    return result.Succeeded;
}
```

### MVC Logout
```csharp
[HttpPost]
public async Task<IActionResult> Logout()
{
    await _adminAuthService.LogoutAsync();
    // Calls SignInManager<ApplicationUser>.SignOutAsync()
    return RedirectToAction("Login");
}
```

---

## Database Considerations

Both API and MVC share the same:
- **ApplicationDbContext** 
- **ApplicationUser** entity
- **User roles and claims**

This means:
? Users created in API can log in via MVC
? Users created in MVC can authenticate with API
? Roles and permissions are synchronized
? Single source of truth for user data

---

## Security Best Practices

### For API (JWT)
1. **Always use HTTPS** in production (set `RequireHttpsMetadata = true`)
2. **Keep tokens short-lived** (30 mins to 1 hour)
3. **Use refresh tokens** for obtaining new access tokens
4. **Validate token signature** and expiry (already configured)
5. **Store tokens securely** on client (not localStorage if possible)
6. **Implement CORS properly** to prevent unauthorized token usage

### For MVC (Cookies)
1. **Set cookie flags**: `HttpOnly`, `Secure`, `SameSite`
2. **Use HTTPS** in production
3. **Implement CSRF protection** for form submissions
4. **Set appropriate session timeouts**
5. **Validate user permissions** in controllers with `[Authorize]`
6. **Log suspicious login attempts**

---

## Troubleshooting

### Issue: "No authentication scheme was specified"
**Solution**: Ensure you called the correct authentication method:
- API: `AddInfrastructureJwtAuthentication()`
- MVC: `AddInfrastructureIdentityAuthentication()`

### Issue: JWT tokens not being validated
**Solution**: Check `appsettings.json` JWT configuration matches:
- Issuer matches `TokenValidationParameters.ValidIssuer`
- Audience matches `TokenValidationParameters.ValidAudience`
- SecretKey is at least 32 characters

### Issue: MVC login not working
**Solution**: Ensure:
- `AddInfrastructureIdentityAuthentication()` is called
- `UseAuthentication()` is called before `UseAuthorization()`
- User exists and is verified

### Issue: CORS errors in API
**Solution**: Add CORS middleware in API Program.cs:
```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

app.UseCors("AllowAll");
app.UseAuthentication();
```

---

## Testing

### Test API JWT Authentication
```bash
# Login and get token
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password"}'

# Use token in subsequent requests
curl -X GET https://localhost:5001/api/flights \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Test MVC Cookie Authentication
```bash
# Login (creates cookie)
curl -X POST https://localhost:5000/auth/login \
  -d "Email=user@example.com&Password=password" \
  -c cookies.txt

# Use cookie in subsequent requests
curl -X GET https://localhost:5000/bookings \
  -b cookies.txt
```

---

## Summary

? **API Project**: JWT Bearer tokens for stateless authentication
? **MVC Project**: Identity cookies for session-based authentication
? **Shared Database**: Single user store for both projects
? **Separate Configuration**: Each project configures its own authentication scheme
? **Independent Operation**: API and MVC can run on separate servers

Each project now works with its appropriate authentication mechanism while maintaining a unified user database!
