using System.Collections.Generic;
using System.Linq;
using WebApi.FeatureStructure;

namespace WebApi.DataBase
{
    /// <summary>
    /// 用戶
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// 用戶資訊字典
        /// </summary>
        public static Dictionary<int, UserInfo> UserInfoDic = new Dictionary<int, UserInfo>();

        /// <summary>
        /// 取得特定用戶的全資訊
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns>特定用戶的全資訊</returns>
        public static UserInfo GetUserInfoDic(int userId)
        {
            if (UserInfoDic.Keys.Contains(userId))
            {
                return UserInfoDic[userId];
            }
            UserInfoDic.Add(userId, new UserInfo());
            return UserInfoDic[userId];
        }
    }
}