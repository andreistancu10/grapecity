using HTSS.Platform.Infrastructure.Cache;

namespace Microservice.TenantNotification
{
    public class DistributedCacheOptions
    {
        public CacheStore Provider { get; set; }
        public string Configuration { get; set; }
        public string InstanceName { get; set; }
        public string ContainerName { get; set; }
        public string DatabaseName { get; set; }
    }
}