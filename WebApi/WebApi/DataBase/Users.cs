using System.Collections.Generic;
using System.Linq;
using WebApi.Frames;

namespace WebApi.DataBase
{
    /// <summary>
    /// 用戶
    /// </summary>
    public static class Users
    {
        //以下新
        private static Dictionary<int, FrameObjectFactory> FrameResponseFactoryDic = new Dictionary<int, FrameObjectFactory>();

        public static FrameObjectFactory GetFreamObjectFactory(int userId)
        {
            if (FrameResponseFactoryDic.Keys.Contains(userId))
            {
                return FrameResponseFactoryDic[userId];
            }
            FrameResponseFactoryDic.Add(userId, new FrameObjectFactory());
            return FrameResponseFactoryDic[userId];
        }
    }
}