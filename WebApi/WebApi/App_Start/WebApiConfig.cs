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
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();
            
            //輸入數字或運算符用的api，會傳入body
            config.Routes.MapHttpRoute(
                name: "輸入數字或運算符, 有body的Post",
                routeTemplate: "api/postwithbody/{userId}",
                defaults: new { controller = "Input", userId = RouteParameter.Optional}
            );
            
            //回傳答案用的api
            config.Routes.MapHttpRoute(
                name: "Post, 回傳答案",
                routeTemplate: "api/getanswer/{userIdForAns}",
                defaults: new { controller = "Input", userIdForAns = RouteParameter.Optional}
            );

            //Clear現有功能
            config.Routes.MapHttpRoute(
                name: "Get, Clear功能",
                routeTemplate: "api/clear/{userIdForClear}",
                defaults: new { controller = "Input", userIdForClear = RouteParameter.Optional}
            );
        }
    }
}
