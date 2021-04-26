using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Controllers;
using WebApi.DataBase;
using WebApi.Models.Request;
using WebApi.Models.Response;
using WebApi.FeatureStructure;

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
        private readonly static Dictionary<string, Func<char, int, IFeature>> IFeatureDic = new Dictionary<string, Func<char, int, IFeature>>()
        {
            {
                "Number", (content, userId) =>
                {
                    return new Number(userId, content);
                }
            },
            {
                "Binary", (content, userId) =>
                {
                    return new Binary(userId, content);
                }
            },
            {
                "Unary", (content, userId) =>
                {
                    return new Unary(userId, content);
                }
            },
            {
                "Equal", (content, userId) =>
                {
                    return new Equal(userId);
                }
            },
            {
                "LeftBracket", (content, userId) =>
                {
                    return new LeftBracket(userId);
                }
            },
            {
                "RightBracket", (content, userId) =>
                {
                    return new RightBracket(userId);
                }
            },
            {
                //Clear的Cast就是null?
                "Clear", (content, userId) =>
                {
                    return new Clear(userId);
                }
            },
            {
                "ClearError", (content, userId) =>
                {
                    return new ClearError(userId);
                }
            },
            {
                "BackSpace", (content, userId) =>
                {
                    return new BackSpace(userId);
                }
            }
        };

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
            return IFeatureDic[instruct.Feature].Invoke(instruct.Content, userId).GetFrameObject();
        }
    }
}