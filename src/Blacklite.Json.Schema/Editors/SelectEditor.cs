using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class SelectJsonEditor : JsonEditor
    {
        public SelectJsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null) : base(context, options)
        {
        }

        public override JsonEditorRenderer Build()
        {
            if (Context.Options.Hidden)
                return null;

            var input = new TagBuilder("select");

            if (Context.Options.ReadOnly)
            {
                input.Attributes.Add("readonly", null);
                input.Attributes.Add("disabled", null);
            }

            if (!string.IsNullOrEmpty(Format))
                input.Attributes.Add("data-editor-format", Format);

            input.Attributes.Add("id", TagBuilder.CreateSanitizedId(Context.Path, "_"));
            input.Attributes.Add("name", Context.Path);

            DecorateTagBuilder(input);

            var displayValues = Context.Options.ValueDisplay;
            var options = Context.Schema.Enum.Select((x, index) =>
            {
                var selectValue = x.ToString();

                return new JsonEditorRenderer(Context.Serializer, value =>
                {
                    var option = new TagBuilder("option");
                    option.Attributes.Add("value", selectValue);

                    if (selectValue == value?.ToString())
                        option.Attributes.Add("selected", null);

                    if (displayValues != null && displayValues.Length > index)
                        option.InnerHtml = displayValues[index] ?? Context.Schema.Enum[index].ToString();
                    else
                        option.InnerHtml = Context.Schema.Enum[index].ToString();

                    return option.ToString();
                }, z => "");
            });

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
                newInput.InnerHtml = string.Join("", options.Select(x => x.Render(value)));

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

        protected virtual void DecorateTagBuilder(TagBuilder builder) { }
    }
}
