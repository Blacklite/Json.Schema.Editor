using Temp.Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Temp.Newtonsoft.Json.Schema;
using Temp.Newtonsoft.Json.Linq;

namespace Blacklite.Json.Schema.Editors
{

    public class JsonEditorRenderer
    {
        private readonly JsonSerializer _serializer;
        private readonly Func<JToken, string> _renderer;
        public JsonEditorRenderer(JsonSerializer serializer, Func<JToken, string> renderer)
        {
            _serializer = serializer;
            _renderer = renderer;
        }

        public string Render(JToken value)
        {
            return _renderer(value);
        }

        public string Render(object value)
        {
            return Render(JObject.FromObject(value, _serializer));
        }
    }
}