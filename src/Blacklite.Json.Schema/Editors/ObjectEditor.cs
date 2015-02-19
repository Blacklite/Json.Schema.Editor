using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json;
using Temp.Newtonsoft.Json.Schema;
using Microsoft.AspNet.Mvc.Rendering;

namespace Blacklite.Json.Schema.Editors
{
    public class ObjectJsonEditor : JsonEditor
    {
        private readonly IJsonEditorProvider _editorProvider;
        public ObjectJsonEditor(IJsonEditorResolutionContext context, IJsonEditorProvider editorProvider, EditorOptions options = null) : base(context, options)
        {
            _editorProvider = editorProvider;
        }

        public override JsonEditorRenderer Build()
        {
            var container = new TagBuilder("div");
            container.Attributes.Add("data-editor-type", this.ToString());

            container = Context.Decorator.DecorateItemContainer(Context, container);

            if (Context.Options.ShowHeader)
            {
                var title = new TagBuilder("h3");
                title.InnerHtml = this.GetTitle();
                title = Context.Decorator.DecorateTitle(Context, title);
                container.InnerHtml += title.ToString();
            }

            var propertyTagRenderes = Context.Schema.Properties
                .Select(property => new { Key = property.Key, Value = GetPropertyTagBuilder(property.Key, property.Value) })
                .Where(x => x.Value != null)
                .ToArray();

            return new JsonEditorRenderer(Context.Serializer, value =>
            {
                var result = new TagBuilder("div");
                result.MergeAttributes(container.Attributes);
                result.InnerHtml = container.InnerHtml;

                foreach (var property in propertyTagRenderes)
                    result.InnerHtml += property.Value.Render(value?[property.Key]);

                return result.ToString();
            });
        }

        private JsonEditorRenderer GetPropertyTagBuilder(string key, JSchema schema)
        {
            var editor = _editorProvider.GetJsonEditor(schema, key, Context.Path);

            if (editor.Context.Options.Hidden)
                return null;

            var builder = editor?.Build();
            return builder;
        }
    }
}
