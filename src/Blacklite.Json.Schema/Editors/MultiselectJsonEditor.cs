using System;
using Microsoft.AspNet.Mvc.Rendering;

namespace Blacklite.Json.Schema.Editors
{
    public class MultiselectJsonEditor : SelectJsonEditor
    {
        public MultiselectJsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null) : base (context, options)
        {
        }

        protected override void DecorateTagBuilder(TagBuilder builder)
        {
            builder.Attributes.Add("multiple", null);
        }
    }
}