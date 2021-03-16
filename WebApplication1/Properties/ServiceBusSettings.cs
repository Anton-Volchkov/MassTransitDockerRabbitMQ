using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Properties
{
    public class ServiceBusSettings
    {
        public string Host { get; set; } = "rabbitmq://localhost";
        public bool IsEnabled { get; set; } = true;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public int TranPerSer { get; set; } = 15;
        public string Type { get; set; } = "RabbitMq";
    }
}
