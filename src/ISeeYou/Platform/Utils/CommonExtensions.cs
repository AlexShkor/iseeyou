
using ISeeYou.Platform.Extensions;

namespace ISeeYou.Platform.Utils
{
    public static class StringExtensions
    {
       public static string HasValueOr(this string source, string anotherValue)
       {
           return source.HasValue() ? source : anotherValue;
       }
    }
}