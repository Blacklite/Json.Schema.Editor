using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class NullJsonEditor : JsonEditor
    {
        public NullJsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null) : base (context, options)
        {
        }

        public override JsonEditorRenderer Build()
        {
            return new JsonEditorRenderer(Context.Serializer, x => "", x => "");
        }
    }
}
