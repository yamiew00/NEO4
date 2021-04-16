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
            
            //輸入數字或運算符用的api，會傳入body
            config.Routes.MapHttpRoute(
                name: "輸入數字或運算符, 有body的Post",
                routeTemplate: "api/postwithbody/{userId}",
                defaults: new { controller = "Request", userId = RouteParameter.Optional}
            );
            
            //回傳答案用的api
            config.Routes.MapHttpRoute(
                name: "Post, 回傳答案",
                routeTemplate: "api/getanswer/{userIdForAns}",
                defaults: new { controller = "Request", userIdForAns = RouteParameter.Optional}
            );

            //Clear現有功能
            //config.Routes.MapHttpRoute(
            //    name: "Get, Clear功能",
            //    routeTemplate: "api/clear/{userIdForClear}",
            //    defaults: new { controller = "Request", userIdForClear = RouteParameter.Optional}
            //);




            //以下新的
            config.Routes.MapHttpRoute(
                name: "Post, Number功能",
                routeTemplate: "api/number/{userIdWithNumber}",
                defaults: new { controller = "Request", userIdWithNumber = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "Post, Binary功能",
                routeTemplate: "api/binary/{userIdWithBinary}",
                defaults: new { controller = "Request", userIdWithBinary = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "Get, Equal功能",
                routeTemplate: "api/equal/{userIdWithEqual}",
                defaults: new { controller = "Request", userIdWithEqual = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Get, LeftBracket功能",
                routeTemplate: "api/LeftBracket/{userIdWithLeftBracket}",
                defaults: new { controller = "Request", userIdWithLeftBracket = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Get, RightBracket功能",
                routeTemplate: "api/RightBracket/{userIdWithRightBracket}",
                defaults: new { controller = "Request", userIdWithRightBracket = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Get, Clear功能",
                routeTemplate: "api/clear/{userIdWithClear}",
                defaults: new { controller = "Request", userIdWithClear = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Get, ClearError功能",
                routeTemplate: "api/clearerror/{userIdWithClearError}",
                defaults: new { controller = "Request", userIdWithClearError = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Get, BackSpace功能",
                routeTemplate: "api/backspace/{userIdWithBackSpace}",
                defaults: new { controller = "Request", userIdWithBackSpace = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Post, Unary功能",
                routeTemplate: "api/unary/{userIdWithUnary}",
                defaults: new { controller = "Request", userIdWithUnary = RouteParameter.Optional }
            );

        }
    }
}
