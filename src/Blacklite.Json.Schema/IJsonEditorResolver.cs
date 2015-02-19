using Blacklite.Json.Schema.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema
{
    public interface IJsonEditorResolver
    {
        JsonEditor GetEditor(IJsonEditorResolutionContext context);
        int Priority { get; }
    }
}
