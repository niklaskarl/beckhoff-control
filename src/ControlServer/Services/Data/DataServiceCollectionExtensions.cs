using System;
using Microsoft.Extensions.DependencyInjection;

namespace ControlServer.Services.Data
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection services, string path)
        {
            services.AddSingleton<IDataService>(p => new DataService(path));
            return services;
        }
    }
}
