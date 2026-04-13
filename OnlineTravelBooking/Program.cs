using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineTravelBooking;
using OnlineTravelBooking.Application;
using OnlineTravelBooking.Application.Common.Mappings;
using OnlineTravelBooking.Application.DTOs.Payment;
using OnlineTravelBooking.Infrastructure;
using OnlineTravelBooking.Infrastructure.Persistence;
using OnlineTravelBooking.Infrastructure.Persistence.Seed;
using Stripe;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("StripeSettings"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.AddInfrastructureJwtAuthentication(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


#region Middlewares

var app = builder.Build();

// Seed Database
await app.SeedDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/GlobalExceptionHandler");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Redirect root to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run(); 

#endregion
