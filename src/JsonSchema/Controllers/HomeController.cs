using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using JsonSchema.Renderer;
using Microsoft.AspNet.Hosting;
using Temp.Newtonsoft.Json.Schema;
using System.IO;
using Temp.Newtonsoft.Json;
using Temp.Newtonsoft.Json.Linq;
using Blacklite.Json.Schema;

namespace JsonSchema.Controllers
{
    public class HomeController : Controller
    {
        private readonly JSchema _schema;
        private readonly IJsonEditorProvider _editorProvider;
        public HomeController(IHostingEnvironment hostingEnv, IJsonEditorProvider editorProvider)
        {
            using (var streamReader = new StreamReader(hostingEnv.WebRootFileProvider.GetFileInfo("/schema/schema.json").CreateReadStream()))
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    _schema = JSchema.Load(reader);
                }
            }
            _editorProvider = editorProvider;
        }

        private static string __json = @"{
            ""featureA"": {
                ""enabled"": true,
                ""featureD"": {
                    ""enabled"": true,
                }
            },
            ""featureB"": {
                ""enabled"": true,
            },
            ""featureC"": {
                ""enabled"": true,
                ""options"": {
                    ""database"": ""path-to-db"",
                    ""dsername"": ""efgh"",
                    ""dassword"": ""abcd""
                }
            }
        }";

        public IActionResult Index()
        {
            // TODO: Make options so that the serializer can be changed.
            var _serializer = new JsonSerializer();
            _serializer.Converters.Add(new Temp.Newtonsoft.Json.Converters.StringEnumConverter());
            _serializer.ContractResolver = new Temp.Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            return View(null,
                _editorProvider.GetJsonEditor(_schema, "Features").Build().Render(JObject.Parse(__json))
            );

            //return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}