using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json;
using Temp.Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class StringJsonEditor : JsonEditor
    {
        protected string _inputType;

        public StringJsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null)
            : base(context, options)
        {
            _inputType = "text";
            if (!string.IsNullOrWhiteSpace(Format))
            {
                if (Format == "textarea")
                {
                    _inputType = "textarea";
                }
                else
                {
                    _inputType = Format;
                }
            }
        }

        public override JsonEditorRenderer Build()
        {
            if (Context.Options.Hidden)
                return null;

            var input = new TagBuilder(_inputType == "textarea" ? _inputType : "input");

            if (Context.Schema.MaximumLength.HasValue)
                input.Attributes.Add("maxlength", Context.Schema.MaximumLength.Value.ToString());

            if (Context.Schema.Pattern != null)
                input.Attributes.Add("pattern", Context.Schema.MaximumLength.Value.ToString());

            if (Context.Schema.MinimumLength.HasValue)
                input.Attributes.Add("pattern", $".{{{Context.Schema.MinimumLength},}}");

            if (Context.Options.ReadOnly)
            {
                input.Attributes.Add("readonly", null);
                input.Attributes.Add("disabled", null);
            }

            if (!string.IsNullOrEmpty(Format))
                input.Attributes.Add("data-editor-format", Format);

            input.Attributes.Add("type", _inputType);

            input.Attributes.Add("id", TagBuilder.CreateSanitizedId(Context.Path, "_"));
            input.Attributes.Add("name", Context.Path);

            var label = new TagBuilder("label");
            label.InnerHtml = GetTitle();
            label.Attributes.Add("for", TagBuilder.CreateSanitizedId(Context.Path, "_"));


            var description = new TagBuilder("div");
            description.InnerHtml = Context.Schema.Description;

            //if (Schema.Required)
            // change handler?

            return new JsonEditorRenderer(Context.Serializer, value =>
            {
                var newInput = new TagBuilder(input.TagName);
                newInput.MergeAttributes(input.Attributes);

                if (_inputType == "checkbox")
                {
                    newInput.Attributes.Add("value", "true");
                    if (value?.ToObject<bool>() ?? false)
                        newInput.Attributes.Add("checked", null);
                }
                else if (newInput.TagName != "input")
                {
                    newInput.InnerHtml = value?.ToObject<string>(Context.Serializer) ?? string.Empty;
                }
                else
                {
                    newInput.Attributes.Add("value", value?.ToString());
                }

                var newLabel = new TagBuilder(label.TagName);
                newLabel.MergeAttributes(label.Attributes);
                newLabel.InnerHtml = label.InnerHtml;

                var control = new TagBuilder("div");

                newInput = Context.Decorator.DecorateInput(Context, newInput);
                newLabel = Context.Decorator.DecorateLabel(Context, newLabel);
                control = Context.Decorator.DecorateContainer(Context, control, newLabel, newInput, description);
                return control.ToString();
            }, x => "");
        }
    }
}
