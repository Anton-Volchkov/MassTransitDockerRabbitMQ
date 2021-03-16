using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Properties;

namespace WebApplication1
{
    public static class ServiceCollectionExtension
    {
        internal static void AddServiceBus(this IServiceCollection services, ServiceBusSettings serviceBusSettings = default)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();

                cfg.AddBus(ctx => CreateRabbitMqBus(serviceBusSettings) );
            });
        }

        private static IBusControl CreateRabbitMqBus(ServiceBusSettings serviceBusSettings)
        {
            return Bus.Factory.CreateUsingRabbitMq();
        }
    }

}
