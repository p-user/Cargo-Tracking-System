using Microsoft.Extensions.Configuration;

namespace SharedKernel.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T BindOptions<T>(this IConfiguration configuration, string sectionName = null!) where T : new()
        {
            var instance = new T();
            var section = string.IsNullOrEmpty(sectionName)
                ? configuration.GetSection(typeof(T).Name)
                : configuration.GetSection(sectionName);

            section.Bind(instance);
            return instance;
        }
    }
}
