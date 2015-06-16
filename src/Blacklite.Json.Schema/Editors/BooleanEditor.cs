using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class BooleanJsonEditor : StringJsonEditor
    {
        public BooleanJsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null) : base(context, options)
        {
        }

        public override JsonEditorRenderer Build()
        {
            _inputType = "checkbox";
            var baseRenderer = base.Build();

            var input = new TagBuilder("input");
            input.Attributes.Add("type", "hidden");
            input.Attributes.Add("id", TagBuilder.CreateSanitizedId(Context.Path + ".hidden", "_"));
            input.Attributes.Add("name", Context.Path);
            input.Attributes.Add("value", "false");

            var hidden = input.ToString();

            return new JsonEditorRenderer(Context.Serializer, token => $"{hidden}{baseRenderer.Render(token)}", baseRenderer.JavaScript);

        }
    }
}
