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
    public interface IJsonEditorDecorator
    {
        TagBuilder DecorateContainer(IJsonEditorResolutionContext context, TagBuilder control, TagBuilder label, TagBuilder input, TagBuilder description);
        TagBuilder DecorateItemContainer(IJsonEditorResolutionContext context, TagBuilder tagBuilder);
        TagBuilder DecorateInput(IJsonEditorResolutionContext context, TagBuilder tagBuilder);
        TagBuilder DecorateLabel(IJsonEditorResolutionContext context, TagBuilder tagBuilder);
        TagBuilder DecorateTitle(IJsonEditorResolutionContext context, TagBuilder tagBuilder);
    }

    public class JsonEditorDecorator : IJsonEditorDecorator
    {
        public virtual TagBuilder DecorateContainer(IJsonEditorResolutionContext context, TagBuilder control, TagBuilder label, TagBuilder input, TagBuilder description)
        {
            string inputContent;
            if (input.TagName == "input")
                inputContent = input.ToString(TagRenderMode.SelfClosing);
            else
                inputContent = input.ToString();

            control.InnerHtml = $"{label}{input}{description}";
            return control;
        }

        public virtual TagBuilder DecorateItemContainer(IJsonEditorResolutionContext context, TagBuilder tagBuilder)
        {
            return tagBuilder;
        }

        public virtual TagBuilder DecorateInput(IJsonEditorResolutionContext context, TagBuilder tagBuilder)
        {
            return tagBuilder;
        }

        public virtual TagBuilder DecorateLabel(IJsonEditorResolutionContext context, TagBuilder tagBuilder)
        {
            return tagBuilder;
        }

        public virtual TagBuilder DecorateTitle(IJsonEditorResolutionContext context, TagBuilder tagBuilder)
        {
            return tagBuilder;
        }
    }
}