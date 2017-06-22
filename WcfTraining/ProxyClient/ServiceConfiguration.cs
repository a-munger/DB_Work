using System.Configuration;

namespace ProxyClient
{
    class ServiceConfiguration : ConfigurationSection
    {
        public const string SectionName = "serviceSettings";

        [ConfigurationProperty("serviceUrl", IsRequired = true)]
        public string ServiceUrl
        {
            get { return (string) base["serviceUrl"]; }
        }

        [ConfigurationProperty("useSsl", IsRequired = true)]
        public bool UseSsl
        {
            get { return (bool)base["useSsl"]; }
        }
    }
}
