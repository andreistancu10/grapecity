namespace DigitNow.Microservice.DocumentManagement.configurations.Consul;

public sealed class ConsulOptions
{
    public string AgentName { get; set; }
    public string Scheme { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}