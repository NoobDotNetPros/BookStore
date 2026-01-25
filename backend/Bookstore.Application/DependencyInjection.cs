using System.Reflection;
using Bookstore.Application.Behaviors;
using FluentValidation;
using MediatR;  // ← ADD THIS (correct namespace)
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Behaviors (Fixed - removed MediatR. prefix)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
