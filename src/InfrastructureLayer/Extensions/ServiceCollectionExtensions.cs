using ApplicationLayer.Services;
using DomainLayer.Repositories;
using InfrastructureLayer.Database;
using InfrastructureLayer.Database.Outbox;
using InfrastructureLayer.Database.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<DomainEventsInterceptor>();
        services.AddDbContext<ApplicationDbContext>(
           (sp, optionsBuilder) =>
           {
               var inteceptor = sp.GetService<DomainEventsInterceptor>();
               string connectionString = configuration.GetConnectionString("Database")!;
               optionsBuilder.UseSqlServer(connectionString)
                   .AddInterceptors(inteceptor!);

                
           });
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OutboxMessageWrapperConsumer>();
            x.AddEntityFrameworkOutbox<ApplicationDbContext>(cfg =>
            {
                cfg.UseSqlServer();
                cfg.UseBusOutbox(y =>
                {
                    //y.DisableDeliveryService();
                });
            });

            x.UsingInMemory((ctx, cfg) =>
            {
                cfg.ConfigureEndpoints(ctx);
                cfg.UseMessageRetry(retryCfg =>
                {
                    retryCfg.Interval(5, TimeSpan.FromSeconds(5));
                });
            });
        });

        services.AddMassTransit<IExternalBus>(x =>
        {
            x.AddConsumers(typeof(ApplicationLayer.Extensions.ServiceCollectionExtensions).Assembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetSection("RabbitMq:Host").Value, configuration.GetSection("RabbitMq:VirtualHost").Value, h =>
                {
                    h.Username(configuration.GetSection("RabbitMq:Username").Value!);
                    h.Password(configuration.GetSection("RabbitMq:Password").Value!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddSignalRCore();
        services.AddScoped<IEventDispatcher, EventDispatcher>();


        return services;
    }
}
