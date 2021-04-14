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
        /// <summary>
        /// 轉換成Json的方法
        /// </summary>
        /// <typeparam name="T">特定類別</typeparam>
        /// <param name="someObject">類別物件</param>
        /// <returns>Json格式的字串</returns>
        public static string ToJson<T>(this T someObject)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(someObject);
        }
    }
}
