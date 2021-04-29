using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Controllers;
using WebApi.DataBase;
using WebApi.Models.Request;
using WebApi.Models.Response;
using WebApi.FeatureStructure;
using WebApi.Extensions;

namespace WebApi.Setting
{
    /// <summary>
    /// 功能(Feature)的對應動作。
    /// </summary>
    public class FeatureReactor
    {
        /// <summary>
        /// 執行特定Feature所對應的方法，並回傳FrameResponse物件
        /// </summary>
        private static Dictionary<string, Func<char, Feature>> FeatureDic = new Dictionary<string, Func<char, Feature>>();
        
        /// <summary>
        /// 載入行為字典
        /// </summary>
        public static void LoadIFeature()
        {
            var types = typeof(Feature).GetAllTypes();
            foreach (var type in types)
            {
                //找建構子
                var constructorInfo = type.GetConstructor(new Type[1] { typeof(char) });
                if (constructorInfo == null)
                {
                    constructorInfo = type.GetConstructor(Type.EmptyTypes);
                    FeatureDic.Add(type.Name, (content) =>
                    {
                        return (Feature)constructorInfo.Invoke(Type.EmptyTypes);
                    });
                }
                else
                {
                    FeatureDic.Add(type.Name, (content) =>
                    {
                        return (Feature)constructorInfo.Invoke(new object[1] { content });
                    });
                }
            }
        }

        /// <summary>
        /// 呼叫字典中的方法以取得FrameObject
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="instruct">按鈕功能組合</param>
        /// <returns>FrameObject</returns>
        public static FrameObject GetFrameObject(int userId, Instruct instruct)
        {
            if (!FeatureDic.Keys.Contains(instruct.Feature))
            {
                throw new Exception("無此Feature");
            }

            var featureConstructor = FeatureDic[instruct.Feature];
            var feature = featureConstructor.Invoke(instruct.Content);
            var userinfo = Users.GetUserInfo(userId);
            return feature.GetAnswer(userinfo);
        }
    }
}