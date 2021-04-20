using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        /// <summary>
        /// 用戶id對運算控制的字典
        /// </summary>
        private static Dictionary<int, CommandCaster> CasterDic = new Dictionary<int, CommandCaster>();

        /// <summary>
        /// 取得用戶的功能執行者
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns>功能執行者</returns>
        public static CommandCaster GetCommandCaster(int userId)
        {
            if (CasterDic.Keys.Contains(userId))
            {
                return CasterDic[userId];
            }

            CasterDic.Add(userId, new CommandCaster());
            return CasterDic[userId];
        }

        private static Dictionary<int, FrameAttribute> FrameDic = new Dictionary<int, FrameAttribute>();

        public static FrameAttribute GetFrameAttribute(int userId)
        {
            if (FrameDic.Keys.Contains(userId)){
                return FrameDic[userId];
            }

            FrameDic.Add(userId, new FrameAttribute());
            return FrameDic[userId];
        }

        public static void SetFrameAttribute(int userId, FrameAttribute frameAttribute)
        {
            if (!FrameDic.Keys.Contains(userId))
            {
                throw new Exception("無此用戶");
            }

            FrameDic[userId] = frameAttribute;
        }
    }
}