using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Microsoft.AspNet.Mvc;

namespace Blacklite.Json.Schema.TagHelpers
{
    public class JsonEditorTagHelper : TagHelper
    {
        private readonly IHostingEnvironment _environment;
        private readonly IJsonEditorProvider _editorProvider;

        public JsonEditorTagHelper(IHostingEnvironment environment, IJsonEditorProvider editorProvider)
        {
            _environment = environment;
            _editorProvider = editorProvider;
        }

        [Required]
        public string Schema { get; }

        [Required]
        public object Model { get; private set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            JSchema schema;
            using (var textReader = new StreamReader(_environment.WebRootFileProvider.GetFileInfo(Schema).CreateReadStream()))
            {
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    schema = JSchema.Load(jsonReader);
                }
            }

            output.Content.Append(_editorProvider.GetJsonEditor(schema, "Features").Build().Render(Model));
        }
    }

    public class JsonEditorSchemaTagHelper : TagHelper
    {
        private readonly IJsonEditorProvider _editorProvider;

        public JsonEditorSchemaTagHelper(IJsonEditorProvider editorProvider)
        {
            _editorProvider = editorProvider;
        }

        [Required]
        public JSchema Schema { get; set; }

        [Required]
        public object Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content.Append(_editorProvider.GetJsonEditor(Schema, "Features").Build().Render(Model));
        }
    }
}
