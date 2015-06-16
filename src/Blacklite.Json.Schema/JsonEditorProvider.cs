using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;
using Blacklite.Json.Schema.Editors;
using Newtonsoft.Json;
using Microsoft.Framework.OptionsModel;

namespace Blacklite.Json.Schema
{
    public interface IJsonEditorProvider
    {
        JsonEditor GetJsonEditor(JSchema schema, string key, params IJsonEditorResolver[] resolvers);
        JsonEditor GetJsonEditor(JSchema schema, string key, IJsonEditorResolutionContext parentContext = null);
        JsonEditor GetJsonEditor(JSchema schema, string key, string prefix, IJsonEditorResolutionContext parentContext);
        JsonEditorResolutionContext GetResolutionContext(JSchema schema, string key, string prefix = "", IJsonEditorResolutionContext parentContext = null);
        JsonEditorResolutionContext GetResolutionContext(JSchema schema, string key, IEnumerable<IJsonEditorResolver> resolvers);
        JsonEditorOptions Options { get; }
    }

    public class JsonEditorOptions
    {
        public JsonSerializer Serializer { get; set; }
        public JsonEditorDecorator Decorator { get; set; }
    }

    public class JsonEditorProvider : IJsonEditorProvider
    {
        private readonly IEnumerable<IJsonEditorResolver> _resolvers;
        private readonly DefaultJsonEditorResolver _defaultResolver;

        public JsonEditorOptions Options { get; }

        public JsonEditorProvider(IEnumerable<IJsonEditorResolver> resolvers, IOptions<JsonEditorOptions> configuredOptions)
        {
            _defaultResolver = new DefaultJsonEditorResolver(this);
            _resolvers = resolvers.OrderByDescending(x => x.Priority).Union(new[] { _defaultResolver }).ToArray();

            var options = Options = new JsonEditorOptions()
            {
                Decorator = configuredOptions.Options.Decorator,
                Serializer = configuredOptions.Options.Serializer,
            };

            if (options.Serializer == null)
            {
                options.Serializer = new JsonSerializer();
                options.Serializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.Serializer.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }

            if (configuredOptions.Options.Decorator == null)
            {
                options.Decorator = new JsonEditorDecorator();
            }
        }

        public JsonEditorResolutionContext GetResolutionContext(JSchema schema, string key, string prefix = "", IJsonEditorResolutionContext parentContext = null)
        {
            return new JsonEditorResolutionContext(schema, Options.Serializer, Options.Decorator, key, prefix, parentContext?.Resolvers ?? _resolvers);
        }

        public JsonEditorResolutionContext GetResolutionContext(JSchema schema, string key, IEnumerable<IJsonEditorResolver> resolvers)
        {
            return new JsonEditorResolutionContext(schema, Options.Serializer, Options.Decorator, key, string.Empty,
                resolvers?.Concat(_resolvers.Except(new[] { _defaultResolver }))
                          .OrderByDescending(z => z.Priority)
                          .Union(new[] { _defaultResolver })
                          .ToArray() ?? _resolvers);
        }

        public JsonEditor GetJsonEditor(JSchema schema, string key, string prefix, IJsonEditorResolutionContext parentContext)
        {
            var context = GetResolutionContext(schema, key, prefix, parentContext);
            var resolver = context.Resolvers.Select(x => x.GetEditor(context)).First(x => x != null);
            return resolver;
        }

        public JsonEditor GetJsonEditor(JSchema schema, string key, IJsonEditorResolutionContext parentContext = null)
        {
            return GetJsonEditor(schema, key, string.Empty, parentContext);
        }

        public JsonEditor GetJsonEditor(JSchema schema, string key, params IJsonEditorResolver[] resolvers)
        {
            var context = GetResolutionContext(schema, key, resolvers);
            var resolver = context.Resolvers.Select(x => x.GetEditor(context)).First(x => x != null);
            return resolver;
        }
    }
}
