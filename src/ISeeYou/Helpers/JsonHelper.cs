using Newtonsoft.Json;

namespace ISeeYou.Helpers
{
    public static class JsonHelper
    {
         public static string ToJson(object o)
         {
             return JsonConvert.SerializeObject(o);
         }
    }
}