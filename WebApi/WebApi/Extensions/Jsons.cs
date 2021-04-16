using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Extensions
{
    public static class Jsons
    {
        public static JObject ToJson<T>(this T obj)
        {
            return JObject.FromObject(obj); ;
        }
    }
}