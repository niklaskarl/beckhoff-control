using System;
using Microsoft.Extensions.DependencyInjection;

namespace ControlServer.Services.Ads
{
    public static class AdsServiceCollectionExtensions
    {
        public static IServiceCollection AddAds(this IServiceCollection services, Uri uri, string adsNetId, int adsPort)
        {
            services.AddScoped<IAdsService>(p => new AdsService(uri, adsNetId, adsPort));
            return services;
        }
    }
}
