namespace WebApplication1.Properties
{
    public class ServiceBusSettings
    {
        public string Host { get; set; } = "rabbitmq://";
        public bool IsEnabled { get; set; } = true;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public int TranPerSer { get; set; } = 15;
        public string Type { get; set; } = "RabbitMq";
    }
}
