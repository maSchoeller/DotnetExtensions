using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTinkerforge(this IServiceCollection services)
        {

            return services;
        }
    }
}
