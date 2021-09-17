using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Properties;

namespace WebApplication1
{
    public static class ServiceCollectionExtension
    {
        internal static void AddServiceBus(this IServiceCollection services)
        {
            var serviceBusSettings = new ServiceBusSettings();
            services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();

                cfg.AddBus(ctx => CreateRabbitMqBus(serviceBusSettings) );
            });

            services.AddMassTransitHostedService();
        }

        private static IBusControl CreateRabbitMqBus(ServiceBusSettings serviceBusSettings)
        {
            var res = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;
            var env = res ? "rabbitmq" : "localhost";

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(serviceBusSettings.Host+env), h => {
                    h.Username(serviceBusSettings.UserName);
                    h.Password(serviceBusSettings.Password);
                });
            });
        }
    }

}
