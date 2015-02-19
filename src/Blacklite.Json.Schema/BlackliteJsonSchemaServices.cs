using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Json.Schema
{
    public static class BlackliteJsonSchemaServices
    {
        internal static IEnumerable<IServiceDescriptor> GetJsonSchema(IServiceCollection services, IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.Singleton<IJsonEditorProvider, JsonEditorProvider>();
        }
    }
}
