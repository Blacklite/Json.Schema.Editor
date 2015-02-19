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
        JsonEditor GetJsonEditor(JSchema schema, string key, string prefix = "");
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

        public JsonEditorProvider(IEnumerable<IJsonEditorResolver> resolvers, IOptions<JsonEditorOptions> configuredOptions)
        {
            _resolvers = resolvers.OrderByDescending(x => x.Priority).Union(new[] { new DefaultJsonEditorResolver(this) });

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

        public JsonEditor GetJsonEditor(JSchema schema, string key, string prefix = "")
        {
            var context = new JsonEditorResolutionContext(schema, _serializer, _decorator, key, prefix);
            var resolver = _resolvers.Select(x => x.GetEditor(context)).First(x => x != null);
            return resolver;
        }
    }
}
