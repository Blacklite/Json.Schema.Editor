using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Temp.Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Temp.Newtonsoft.Json.Schema;
using Microsoft.AspNet.Mvc;

namespace Blacklite.Json.Schema.TagHelpers
{
    public class JsonEditorTagHelper : TagHelper
    {
        [Required]
        public string Schema { get; }

        [Activate]
        public IHostingEnvironment Environment { get; private set; }

        [Activate]
        public IJsonEditorProvider EditorProvider { get; private set; }

        [Required]
        public object Model { get; private set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            JSchema schema;
            using (var textReader = new StreamReader(Environment.WebRootFileProvider.GetFileInfo(Schema).CreateReadStream()))
            {
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    schema = JSchema.Load(jsonReader);
                }
            }

            output.Content = EditorProvider.GetJsonEditor(schema, "Features").Build().Render(Model);
        }
    }

    public class JsonEditorSchemaTagHelper : TagHelper
    {
        [Required]
        public JSchema Schema { get; set; }

        [Activate]
        public IJsonEditorProvider EditorProvider { get; set; }

        [Required]
        public object Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Content = EditorProvider.GetJsonEditor(Schema, "Features").Build().Render(Model);
        }
    }
}
