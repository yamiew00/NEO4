using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi
{
    /// <summary>
    /// 組態
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 設定路由器
        /// </summary>
        /// <param name="config">config</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 路由
            config.MapHttpAttributeRoutes();
            
            //傳feature與content進來
            config.Routes.MapHttpRoute(
                name: "Post, 整合",
                routeTemplate: "api/integrated/{userId}",
                defaults: new { controller = "Request", userId = RouteParameter.Optional }
            );
        }
    }
}
