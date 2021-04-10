using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();
            
            //輸入
            config.Routes.MapHttpRoute(
                name: "輸入數字或運算符, 有body的Post",
                routeTemplate: "api/postwithbody/{userId}",
                defaults: new { controller = "Input", userId = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "Post, 回傳答案",
                routeTemplate: "api/getanswer/{userIdForAns}",
                defaults: new { controller = "Input", userIdForAns = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "Get, Clear功能",
                routeTemplate: "api/clear/{userIdForClear}",
                defaults: new { controller = "Input", userIdForClear = RouteParameter.Optional}
            );
        }
    }
}
