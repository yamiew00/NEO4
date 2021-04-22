using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.NewThing;
using WebApi.Objects;

namespace WebApi.DataBase
{
    /// <summary>
    /// 用戶
    /// </summary>
    public static class Users
    {
        //以下新
        private static Dictionary<int, ContentController> ContentControllerDic = new Dictionary<int, ContentController>();

        public static ContentController GetContentController(int userId)
        {
            if (ContentControllerDic.Keys.Contains(userId))
            {
                return ContentControllerDic[userId];
            }
            ContentControllerDic.Add(userId, new ContentController());
            return ContentControllerDic[userId];
        }
    }
}