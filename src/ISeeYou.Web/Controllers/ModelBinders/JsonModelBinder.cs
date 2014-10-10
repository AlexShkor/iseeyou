using System.Web.Mvc;

namespace ISeeYou.Web.Controllers.ModelBinders
{
    public class JsonModelBinder : IModelBinder
    {
        private readonly static System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var stringified = controllerContext.HttpContext.Request[bindingContext.ModelName];
            if (string.IsNullOrEmpty(stringified))
                return null;
            return serializer.Deserialize(stringified, bindingContext.ModelType);
        }
    }
}