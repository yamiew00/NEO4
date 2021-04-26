using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Controllers;
using WebApi.DataBase;
using WebApi.Models.Request;
using WebApi.Models.Response;
using WebApi.NewThings;

namespace WebApi.Setting
{
    /// <summary>
    /// 功能(Feature)的對應動作。
    /// </summary>
    public class Features
    {
        /// <summary>
        /// 執行特定Feature所對應的方法，並回傳FrameResponse物件
        /// </summary>
        private readonly static Dictionary<string, Func<char, int, FrameObject>> ActionDic = new Dictionary<string, Func<char, int, FrameObject>>()
        {
            {
                "Number", (content, userId) =>
                {
                    NumberObject numberObject = new NumberObject(userId, content);
                    return numberObject.Layer1();
                    //return Users.GetFreamObjectFactory(userId).AddNumber(content);
                }
            },
            {
                "Binary", (content, userId) =>
                {
                    BinaryObject binaryObject = new BinaryObject(userId, content);
                    return binaryObject.Layer1();
                    //return Users.GetFreamObjectFactory(userId).AddBinary(content);
                }
            },
            {
                "Unary", (content, userId) =>
                {
                    return Users.GetFreamObjectFactory(userId).AddUnary(content);
                }
            },
            {
                "Equal", (content, userId) =>
                {
                    EqualObject equalObject = new EqualObject(userId, content);
                    return equalObject.Layer1();
                    //return Users.GetFreamObjectFactory(userId).Equal();
                }
            },
            {
                "LeftBracket", (content, userId) =>
                {
                    return Users.GetFreamObjectFactory(userId).LeftBracket();
                }
            },
            {
                "RightBracket", (content, userId) =>
                {
                    return Users.GetFreamObjectFactory(userId).RightBracket();
                }
            },
            {
                //Clear的Cast就是null?
                "Clear", (content, userId) =>
                {
                    return Users.GetFreamObjectFactory(userId).Clear();
                }
            },
            {
                "ClearError", (content, userId) =>
                {
                    return Users.GetFreamObjectFactory(userId).ClearError();
                }
            },
            {
                "BackSpace", (content, userId) =>
                {
                    return Users.GetFreamObjectFactory(userId).BackSpace();
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
            if (!ActionDic.Keys.Contains(instruct.Feature))
            {
                throw new Exception("無此Feature");
            }
            return ActionDic[instruct.Feature].Invoke(instruct.Content, userId);
        }
    }
}