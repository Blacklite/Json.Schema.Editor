using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Temp.Newtonsoft.Json.Schema;

namespace Blacklite.Json.Schema.Editors
{
    public class EditorOptions
    {
        public bool Compact { get; set; }

        public string Format { get; set; }
    }
}
