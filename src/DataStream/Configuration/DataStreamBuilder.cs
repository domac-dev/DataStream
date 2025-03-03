using Microsoft.Extensions.DependencyInjection;

namespace DataStream.Configuration
{
    public static class DataStreamBuilder
    {
        public static IServiceCollection AddDataStream(this IServiceCollection services, Action<DataStreamOptions>? configureOptions = null)
        {
            var options = DataStreamOptions.Default;
            configureOptions?.Invoke(options);
            services.AddSingleton(options);

            return services;
        }
    }
}
