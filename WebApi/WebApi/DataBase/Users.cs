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
        //UserInfo
        public static Dictionary<int, UserInfo> UserInfoDic = new Dictionary<int, UserInfo>();

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