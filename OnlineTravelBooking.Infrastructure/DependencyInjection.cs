using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.User;
using OnlineTravelBooking.Infrastructure.Identity.Models;
using OnlineTravelBooking.Infrastructure.Identity.Services;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Infrastructure.Persistence;
using OnlineTravelBooking.Infrastructure.Services;
using System.Text;
using OnlineTravelBooking.Infrastructure.Persistence.Repositories.TourRepos;
using OnlineTravelBooking.Infrastructure.Persistence.Repositories.Base;
using OnlineTravelBooking.Infrastructure.Persistence.Repositories.CarRepo;
using OnlineTravelBooking.Infrastructure.Persistence.Repositories.FlightsRepos;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Infrastructure.Persistence.Repositories.HotelRepos;

namespace OnlineTravelBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                 x => x.UseNetTopologySuite() // Enable spatial support
            ));
        services.AddScoped<IBookingTourRepository, BookingTourRepo>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<ICarRepository, CarRepository>();
        // Register IApplicationDbContext
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IPaymentApplicationService, PaymentApplicationService>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IReviewService, ReviewService>();
        //specification executer DI
        services.AddScoped<ISpecificationQueryExecutor, SpecificationQueryExecutor>();

        // Identity - Core setup (shared by both API and MVC)
        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
        {
            // Configure password and user options as needed
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Jwt Settings - used by API
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        // Services
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Repositories
        services.AddScoped<IFlightRepository, FlightRepository>();
        services.AddScoped<IFlightBookingsRepository, FlightBookingsRepository>();
        services.AddScoped<IPassengerRepository, PassengerRepository>();
        services.AddScoped<IHotelRepo, HotelRepository>();
        services.AddScoped<IRoomRepo, RoomRepo>();

        // Email Configuration
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();

        // Register IHttpContextAccessor for services that need to sign-in/out
        services.AddHttpContextAccessor();

        // Register admin auth service for MVC cookie signin
        services.AddScoped<IAdminAuthService, AdminAuthService>();

        return services;
    }

    /// <summary>
    /// Adds JWT authentication for API projects.
    /// Call this in your API's Program.cs INSTEAD of AddInfrastructureAuthentication.
    /// </summary>
    public static IServiceCollection AddInfrastructureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection("Jwt").Get<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt?.Issuer,
                ValidAudience = jwt?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt?.SecretKey ?? string.Empty))
            };
        });

        return services;
    }

    /// <summary>
    /// Adds Identity cookie-based authentication for MVC/Razor Pages projects.
    /// Call this in your MVC's Program.cs INSTEAD of AddInfrastructureAuthentication.
    /// NOTE: Identity already registers the application cookie when AddIdentity() is called.
    /// Do not re-register the same scheme. Use ConfigureApplicationCookie to customize options.
    /// </summary>
    public static IServiceCollection AddInfrastructureIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Ensure AddIdentity has already been called (AddInfrastructure is expected to be called before this).
        // Do not call AddCookie here because AddIdentity already registers the Identity.Application cookie scheme.
        // Instead, configure the existing application cookie if customization is needed.
        services.ConfigureApplicationCookie(options =>
        {
            // Example defaults - adjust as needed
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
            // You can set Cookie settings here if needed:
            // options.Cookie.HttpOnly = true;
            // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            // options.Cookie.SameSite = SameSiteMode.Strict;
        });

        return services;
    }

    /// <summary>
    /// Adds both JWT and Identity authentication (for backward compatibility).
    /// Use this only if you need both authentication schemes in a single project.
    /// </summary>
    public static IServiceCollection AddInfrastructureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection("Jwt").Get<JwtSettings>();

        // Register authentication: make Identity application cookie the default, and also register JWT for API use
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt?.Issuer,
                ValidAudience = jwt?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt?.SecretKey ?? string.Empty))
            };
        });

        return services;
    }
}
