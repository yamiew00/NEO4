using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.NewThing;
using WebApi.Objects;

namespace WebApi.DataBase
{
    /// <summary>
    /// 用戶
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// 用戶id對運算控制的字典
        /// </summary>
        private static Dictionary<int, ExpressionController> Dic = new Dictionary<int, ExpressionController>();

        /// <summary>
        /// 取得用戶的運算控制器
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns>運算控制器</returns>
        public static ExpressionController GetExpressionController(int userId)
        {
            if (Dic.Keys.Contains(userId))
            {
                return Dic[userId];
            }

            Dic.Add(userId, new ExpressionController());
            return Dic[userId];
        }


        /// <summary>
        /// 用戶id對運算控制的字典
        /// </summary>
        private static Dictionary<int, CommandCaster> Dic2 = new Dictionary<int, CommandCaster>();


        //thread-safe?
        public static CommandCaster GetCommandCaster(int userId)
        {
            if (Dic2.Keys.Contains(userId))
            {
                return Dic2[userId];
            }

            Dic2.Add(userId, new CommandCaster());
            return Dic2[userId];
        }
    }
}