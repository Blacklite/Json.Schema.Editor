using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace Blacklite.Json.Schema.Editors
{

    public abstract class JsonEditor
    {
        public IJsonEditorResolutionContext Context { get; }
        public EditorOptions Options { get; }
        public string Format { get; }

        public JsonEditor(IJsonEditorResolutionContext context, EditorOptions options = null)
        {
            Context = context;
            Options = options ?? new EditorOptions();

            Format = context.Schema.Format ?? Options.Format;
        }

        public abstract JsonEditorRenderer Build();

        public string GetTitle()
        {
            return Context.Schema.Title ?? Context.Key;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}:{Context.Key}";
        }
    }
}
