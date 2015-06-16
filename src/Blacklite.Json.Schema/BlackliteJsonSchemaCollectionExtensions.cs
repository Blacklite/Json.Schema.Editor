using Blacklite.Json.Schema;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteJsonSchemaCollectionExtensions
    {
        public static IServiceCollection AddJsonSchemaEditor(/*[NotNull]*/ this IServiceCollection services)
        {
            services.TryAdd(BlackliteJsonSchemaServices.GetJsonSchema(services));
            return services;
        }
    }
}
