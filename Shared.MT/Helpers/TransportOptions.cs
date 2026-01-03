namespace Shared.MT.Helpers;

public class TransportOptions
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = null!;
        public string VirtualHost { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public RabbitMqOptions? RabbitMq { get; set; }
    public bool IsInMemoryTransport { get; set; }
}
