using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineTravelBooking.Application.Common.Behaviors;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Application.Services;
using System.Reflection;

namespace OnlineTravelBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        services.AddScoped<IAttachmentService,AttachmentService>();

        // Pipeline Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
