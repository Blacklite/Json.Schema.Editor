using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json;
using Temp.Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class IntegerJsonEditor : StringJsonEditor
    {
        public IntegerJsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null) : base(context, options)
        {
        }

        public override JsonEditorRenderer Build()
        {
            _inputType = "number";
            return base.Build();
        }
    }
}
