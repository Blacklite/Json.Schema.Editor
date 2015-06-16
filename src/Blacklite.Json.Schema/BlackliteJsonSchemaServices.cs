using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Json.Schema
{
    public static class BlackliteJsonSchemaServices
    {
        internal static IEnumerable<ServiceDescriptor> GetJsonSchema(IServiceCollection services)
        {
            yield return ServiceDescriptor.Singleton<IJsonEditorProvider, JsonEditorProvider>();
        }
    }
}
