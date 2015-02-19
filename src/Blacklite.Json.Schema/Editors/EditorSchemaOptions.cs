using Temp.Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Temp.Newtonsoft.Json.Schema;
using Temp.Newtonsoft.Json.Linq;

namespace Blacklite.Json.Schema.Editors
{
    public class EditorSchemaOptions
    {
        public EditorSchemaOptions()
        {

        }

        public EditorSchemaOptions(JSchema schema, JsonSerializer serializer)
        {
            if (schema.ExtensionData != null)
            {
                if (schema.ExtensionData.ContainsKey("options"))
                {
                    var schemaOptions = schema.ExtensionData["options"];
                    serializer.Populate(schemaOptions.CreateReader(), this);
                }

                if ((schema.ExtensionData.ContainsKey("readOnly") && schema.ExtensionData["readOnly"].ToObject<bool>(serializer) == true) ||
                    (schema.ExtensionData.ContainsKey("readonly") && schema.ExtensionData["readonly"].ToObject<bool>(serializer) == true))
                    this.ReadOnly = true;
            }
        }

        public bool Hidden { get; set; } = false;
        public bool ReadOnly { get; set; } = false;
        public bool ShowHeader { get; set; } = true;
        public string[] ValueDisplay { get; set; }
    }
}