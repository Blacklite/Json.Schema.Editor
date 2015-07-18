using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class ArrayJsonEditor : JsonEditor
    {
        private readonly IJsonEditorProvider _editorProvider;
        public ArrayJsonEditor(IJsonEditorResolutionContext context, IJsonEditorProvider editorProvider, EditorOptions options = null) : base(context, options)
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

            if (Context.Schema.Items.Count > 1)
            {
                var renderers = Context.Schema.Items.Select((x, index) => GetItemRenderer($"[{index}]", x)).ToArray();
                return new JsonEditorRenderer(Context.Serializer, value =>
                {
                    var arrValue = value as JArray;
                    if (arrValue == null)
                        throw new Exception();

                    var result = new TagBuilder("div");
                    result.MergeAttributes(container.Attributes);

                    result.InnerHtml = string.Join("", renderers.Select((x, index) => x.Render(arrValue[index])));

                    return result.ToString();
                }, value => string.Join("", renderers.Select((x, index) => x.JavaScript(value[index]))));
            }
            else
            {
                var rendererCache = new Dictionary<int, JsonEditorRenderer>();
                var itemSchema = Context.Schema.Items.First();
                return new JsonEditorRenderer(Context.Serializer, value =>
                {
                    var arrValue = value as JArray;
                    if (arrValue == null)
                        throw new Exception();

                    var result = new TagBuilder("div");
                    result.MergeAttributes(container.Attributes);

                    result.InnerHtml = string.Join("", arrValue.Select((x, index) =>
                    {
                        JsonEditorRenderer renderer;
                        if (!rendererCache.TryGetValue(index, out renderer))
                        {
                            renderer = GetItemRenderer($"[{index}]", itemSchema);
                            rendererCache.Add(index, renderer);
                        }
                        return renderer;
                    })
                    .Select((x, index) => x.Render(arrValue[index])));

                    return result.ToString();
                }, value =>
                {
                    var arrValue = value as JArray;
                    var renderers = arrValue.Select((x, index) =>
                    {
                        JsonEditorRenderer renderer;
                        if (!rendererCache.TryGetValue(index, out renderer))
                        {
                            renderer = GetItemRenderer($"[{index}]", itemSchema);
                            rendererCache.Add(index, renderer);
                        }
                        return renderer;
                    });

                    return string.Join("", renderers.Select((x, index) => x.JavaScript(value[index])));
                });
            }
        }

        private JsonEditorRenderer GetItemRenderer(string index, JSchema schema)
        {
            var editor = _editorProvider.GetJsonEditor(schema, index, Context.Path, Context);

            if (editor.Context.Options.Hidden)
                return null;

            var builder = editor?.Build();
            return builder;
        }
    }
}
