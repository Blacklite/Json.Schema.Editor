using Blacklite.Json.Schema.Editors;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json;
using Temp.Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema
{
    public interface IJsonEditorResolutionContext
    {
        JSchema Schema { get; }
        JsonSerializer Serializer { get; }
        string Key { get; }
        string Prefix { get; }
        string Path { get; }
        Type ValueType { get; }
        IJsonEditorDecorator Decorator { get; }
        EditorSchemaOptions Options { get; }
        IDictionary<string, object> Data { get; }
    }

    public class JsonEditorResolutionContext : IJsonEditorResolutionContext
    {
        public JsonEditorResolutionContext(JSchema schema, JsonSerializer serializer, IJsonEditorDecorator decorator, string key, string prefix = "")
        {
            Schema = schema;
            Serializer = serializer;
            Key = key;
            Prefix = prefix;
            Decorator = decorator;
            Options = new EditorSchemaOptions(schema, serializer);
            Data = new Dictionary<string, object>();

            if (schema.Type.HasValue)
            {
                var value = schema.Type.Value;
                if (value.HasFlag(JSchemaType.Float))
                    ValueType = typeof(float);
                if (value.HasFlag(JSchemaType.Integer))
                    ValueType = typeof(int);
                if (value.HasFlag(JSchemaType.String))
                    ValueType = typeof(string);
                if (value.HasFlag(JSchemaType.Boolean))
                    ValueType = typeof(bool);
            }
        }

        public JSchema Schema { get; }

        public string Key { get; }

        public JsonSerializer Serializer { get; }

        public string Prefix { get; }

        public string Path
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Prefix))
                    return $"{Prefix}.{Key}";
                return Key;
            }
        }

        public Type ValueType { get; }

        public IJsonEditorDecorator Decorator { get; }
        public EditorSchemaOptions Options { get; }

        public IDictionary<string, object> Data { get; }
    }
}
