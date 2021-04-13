using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.Objects;
using System.Text;
using System.Threading.Tasks;
using Calculator.Setting;

namespace Calculator.Extensions
{
    /// <summary>
    /// Json擴充字元
    /// </summary>
    public static class ToJsons
    {
        public static string ToJson<T>(this T someObject)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(someObject);
        }
    }
}
