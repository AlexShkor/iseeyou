using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ISeeYou.Web.Controllers.ActionResults
{
    public class JsonNetResult : ActionResult
    {
        public object Data { get; private set; }
        public JsonNetResult(object data)
        {
            Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = "application/json";
            if (Data != null)
            {

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowerCamelCaseContractResolver()
                };

                settings.Converters.Add(new JavaScriptDateTimeConverter());

                response.Write(JsonConvert.SerializeObject(Data, Formatting.None, settings));
            }
        }
    }
}