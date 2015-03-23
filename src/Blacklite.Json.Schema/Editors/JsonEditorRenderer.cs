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
        private readonly Func<JToken, string> _javaScript;

        public JsonEditorRenderer(JsonSerializer serializer, Func<JToken, string> renderer, Func<JToken, string> javaScript)
        {
            _serializer = serializer;
            _renderer = renderer;
            _javaScript = javaScript;
        }

        public string Render(JToken value)
        {
            return _renderer(value);
        }

        public string Render(object value)
        {
            return Render(JObject.FromObject(value, _serializer));
        }

        public string JavaScript(JToken value)
        {
            return _javaScript(value);
        }

        public string JavaScript(object value)
        {
            return Render(JObject.FromObject(value, _serializer));
        }
    }
}