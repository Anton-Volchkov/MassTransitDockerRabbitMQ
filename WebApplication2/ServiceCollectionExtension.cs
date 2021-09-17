using System;
using System.Transactions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Properties;
using WebApplication2.Consumers;

namespace WebApplication2
{
    public static class ServiceCollectionExtension
    {
        internal static void AddServiceBus(this IServiceCollection services)
        {
            var serviceBusSettings = new ServiceBusSettings();
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumers(typeof(TestConsumer).Assembly);
                cfg.SetKebabCaseEndpointNameFormatter();

                cfg.AddBus(ctx => CreateRabbitMqBus(serviceBusSettings, services));

            });

            services.AddMassTransitHostedService();
        }

        private static IBusControl CreateRabbitMqBus(ServiceBusSettings serviceBusSettings, IServiceCollection services)
        {
           var res = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;
           var env = res ? "rabbitmq" : "localhost";
            return Bus.Factory.CreateUsingRabbitMq(cfg => {
                cfg.Host(new Uri(serviceBusSettings.Host + env ), h => {
                    h.Username(serviceBusSettings.UserName);
                    h.Password(serviceBusSettings.Password);
                });

                IServiceProvider provider = services.BuildServiceProvider();
                ConfigureReceiveEndpoints(cfg, provider, serviceBusSettings.TranPerSer);
            });
        }

        private static void ConfigureReceiveEndpoints(IReceiveConfigurator cfg, IServiceProvider provider,
                                                      int transactionTimeoutInSeconds)
        {
            // Configure endpoints
            cfg.ReceiveEndpoint("test-consumer", c =>
            {
                c.UseTransaction(t => ConfigureReceiveEndpointTransaction(t, transactionTimeoutInSeconds));
                c.Consumer<TestConsumer>(provider);
            });

        }

        private static void ConfigureReceiveEndpointTransaction(ITransactionConfigurator transactionConfigurator,
                                                                int transactionTimeoutInSeconds)
        {
            transactionConfigurator.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionConfigurator.Timeout = TimeSpan.FromSeconds(transactionTimeoutInSeconds);
        }
    }
}
