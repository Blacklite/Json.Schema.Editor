using Blacklite.Json.Schema;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteJsonSchemaCollectionExtensions
    {
        public static IServiceCollection AddJsonSchemaEditor(
            /*[NotNull]*/ this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(BlackliteJsonSchemaServices.GetJsonSchema(services, configuration));
            return services;
        }
    }
}
