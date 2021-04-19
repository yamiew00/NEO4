using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Extensions
{
    /// <summary>
    /// Json格式相關的擴充方法
    /// </summary>
    public static class Jsons
    {
        /// <summary>
        /// 把Tsource轉成Json字串
        /// </summary>
        /// <typeparam name="T">Tsource</typeparam>
        /// <param name="obj">物件實體</param>
        /// <returns>Json字串</returns>
        public static JObject ToJson<T>(this T obj)
        {
            return JObject.FromObject(obj);
        }
    }
}