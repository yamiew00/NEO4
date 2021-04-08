using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Objects;

namespace WebApi.DataBase
{
    public static class Users
    {
        public static Dictionary<int, ExpressionController> Dic = new Dictionary<int, ExpressionController>();

        public static ExpressionController GetExpressionController(int userId)
        {
            if (Dic.Keys.Contains(userId))
            {
                return Dic[userId];
            }

            Dic.Add(userId, new ExpressionController());
            return Dic[userId];
        }
    }
}