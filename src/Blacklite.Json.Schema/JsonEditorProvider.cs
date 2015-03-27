using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json.Schema;
using Blacklite.Json.Schema.Editors;
using Temp.Newtonsoft.Json;
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
    }

    public class JsonEditorOptions
    {
        public JsonSerializer Serializer { get; set; }
        public JsonEditorDecorator Decorator { get; set; }
    }

    public class JsonEditorProvider : IJsonEditorProvider
    {
        private readonly IEnumerable<IJsonEditorResolver> _resolvers;
        private readonly JsonSerializer _serializer;
        private readonly IJsonEditorDecorator _decorator;
        private readonly DefaultJsonEditorResolver _defaultResolver;

        public JsonEditorProvider(IEnumerable<IJsonEditorResolver> resolvers, IOptions<JsonEditorOptions> configuredOptions)
        {
            _defaultResolver = new DefaultJsonEditorResolver(this);
            _resolvers = resolvers.OrderByDescending(x => x.Priority).Union(new[] { _defaultResolver }).ToArray();

            if (configuredOptions.Options.Serializer == null)
            {
                _serializer = new JsonSerializer();
                _serializer.Converters.Add(new Temp.Newtonsoft.Json.Converters.StringEnumConverter());
                _serializer.ContractResolver = new Temp.Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }
            else
            {
                _serializer = configuredOptions.Options.Serializer;
            }
            if (configuredOptions.Options.Decorator == null)
            {
                _decorator = new JsonEditorDecorator();
            }
            else
            {
                _decorator = configuredOptions.Options.Decorator;
            }
        }

        public JsonEditorResolutionContext GetResolutionContext(JSchema schema, string key, string prefix = "", IJsonEditorResolutionContext parentContext = null)
        {
            return new JsonEditorResolutionContext(schema, _serializer, _decorator, key, prefix, parentContext?.Resolvers ?? _resolvers);
        }

        public JsonEditorResolutionContext GetResolutionContext(JSchema schema, string key, IEnumerable<IJsonEditorResolver> resolvers)
        {
            return new JsonEditorResolutionContext(schema, _serializer, _decorator, key, string.Empty,
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
