using Blacklite.Json.Schema.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema
{
    public class DefaultJsonEditorResolver : IJsonEditorResolver
    {
        private readonly IJsonEditorProvider _editorProvider;
        public DefaultJsonEditorResolver(IJsonEditorProvider editorProvider)
        {
            _editorProvider = editorProvider;
        }

        public int Priority { get; } = 0;

        public JsonEditor GetEditor(IJsonEditorResolutionContext context)
        {
            // below is based on https://github.com/jdorn/json-editor
            var schema = context.Schema;

            // Not yet supported
            // TODO: Requires UI side work
            //if (schema.OneOf.Any() || !schema.Type.HasValue)
            //    return new AnyJsonEditor(context);

            if (schema.Type == JSchemaType.Array && schema.Items.Count == 1 && schema.UniqueItems &&
                schema.Items[0].Enum.Any() && schema.Items[0].Type.HasValue &&
                (schema.Items[0].Type.Value.HasFlag(JSchemaType.String) || schema.Items[0].Type.Value.HasFlag(JSchemaType.Number) || schema.Items[0].Type.Value.HasFlag(JSchemaType.Integer)))
                return new MultiselectJsonEditor(context);

            if (schema.Enum.Any())
            {
                // Not yet supported
                // TODO: Requires UI side work
                //if (schema.Type == JSchemaType.Array || schema.Type == JSchemaType.Object)
                //{
                //    return new EnumJsonEditor(context);
                //}
                //else
                if (schema.Type == JSchemaType.Number || schema.Type == JSchemaType.Integer || schema.Type == JSchemaType.String)
                {
                    return new SelectJsonEditor(context);
                }
            }

            if (schema.Type == JSchemaType.String)
                return new StringJsonEditor(context);

            if (schema.Type == JSchemaType.Number)
                return new FloatJsonEditor(context);

            if (schema.Type == JSchemaType.Integer)
                return new IntegerJsonEditor(context);

            if (schema.Type == JSchemaType.Boolean)
                return new BooleanJsonEditor(context);

            if (schema.Type == JSchemaType.Object)
                return new ObjectJsonEditor(context, _editorProvider);

            if (schema.Type == JSchemaType.Array)
                return new ArrayJsonEditor(context, _editorProvider);

            if (schema.Type == JSchemaType.Null)
                return new NullJsonEditor(context);

            return null;
            // Not yet supported
            // TODO: Requires UI side work
            //return new AnyJsonEditor(context);
        }
    }
}
