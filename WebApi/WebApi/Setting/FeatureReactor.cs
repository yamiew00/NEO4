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
        private static Dictionary<string, Func<int, char, IFeature>> IFeatureDic = new Dictionary<string, Func<int, char, IFeature>>();

        /// <summary>
        /// 載入行為字典
        /// </summary>
        public static void LoadIFeature()
        {
            var types = typeof(IFeature).GetAllTypes();
            foreach (var type in types)
            {
                var reflect = Activator.CreateInstance(type);

                Func<int, char, IFeature> func = (Func<int, char, IFeature>)type.GetMethod("Create").Invoke(reflect, null);
                IFeatureDic.Add(type.Name, func);
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
            if (!IFeatureDic.Keys.Contains(instruct.Feature))
            {
                throw new Exception("無此Feature");
            }
            return IFeatureDic[instruct.Feature].Invoke(userId, instruct.Content).GetFrameObject();
        }
    }
}