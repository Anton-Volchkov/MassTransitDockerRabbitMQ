using System;
using System.Threading.Tasks;
using MassTransit;
using TestNotification;

namespace WebApplication2.Consumers
{
    public class TestConsumer : IConsumer<INotification>
    {
        public Task Consume(ConsumeContext<INotification> context)
        {
            Console.WriteLine("123");
            return Task.CompletedTask;
        }
    }
}
