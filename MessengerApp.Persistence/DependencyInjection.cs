using MessengerApp.Application.Interfaces.Context;
using MessengerApp.Application.Interfaces.Services;
using MessengerApp.Domain.Entities;
using MessengerApp.Persistence.Context;
using MessengerApp.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerApp.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext,ApplicationDbContext>();
        services.AddScoped<ICustomerServices,ICustomerDbService>();
        services.AddScoped<IMessageInfoServices, MessageInfoDbService>();

        return services;
    }
}