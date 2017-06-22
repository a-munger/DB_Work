using System.Configuration;

namespace HttpClient
{
    class ServiceConfiguration : ConfigurationSection
    {
        public const string SectionName = "serviceSettings";

        [ConfigurationProperty("serviceUrl", IsRequired = true)]
        public string ServiceUrl
        {
            get { return (string) base["serviceUrl"]; }
        }

        [ConfigurationProperty("testServiceMethod", IsRequired = true)]
        public string TestServiceMethod
        {
            get { return (string) base["testServiceMethod"]; }
        }

        [ConfigurationProperty("serviceMethod", IsRequired = true)]
        public string ServiceMethod
        {
            get { return (string)base["serviceMethod"]; }
        }

        [ConfigurationProperty("useSsl", IsRequired = true)]
        public bool UseSsl
        {
            get { return (bool)base["useSsl"]; }
        }
    }
}
