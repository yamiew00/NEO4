using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Controllers;
using WebApi.DataBase;
using WebApi.Models.Response;
using WebApi.NewThing;

namespace WebApi.Setting
{
    public class Features
    {
        public static Dictionary<string, Func<char, int, FrameResponse>> ActionDic =new Dictionary<string, Func<char, int, FrameResponse>>()
        {
            {
                "Number", (content, userId) =>
                {
                    return Users.GetContentController(userId).AddNumber(content);
                }
            },
            {
                "Binary", (content, userId) =>
                {
                    //連續Binary時要計算結果還沒做
                    return Users.GetContentController(userId).AddBinary(content);
                }
            },
            {
                "Unary", (content, userId) =>
                {
                    //對負數做根號的處理還沒做
                    return Users.GetContentController(userId).AddUnary(content);
                }
            },
            {
                "Equal", (content, userId) =>
                {
                    return Users.GetContentController(userId).Equal();
                }
            },
            {
                "LeftBracket", (content, userId) =>
                {
                    return Users.GetContentController(userId).LeftBracket();
                }
            },
            {
                "RightBracket", (content, userId) =>
                {
                    //右括號之後要計算當前答案還沒有做
                    return Users.GetContentController(userId).RightBracket();
                }
            },
            {
                //Clear的Cast就是null?
                "Clear", (content, userId) =>
                {
                    return Users.GetContentController(userId).Clear();
                }
            },
            {
                "ClearError", (content, userId) =>
                {
                    return Users.GetContentController(userId).ClearError();
                }
            },
            {
                "BackSpace", (content, userId) =>
                {
                    return Users.GetContentController(userId).BackSpace();
                }
            }
        };
    }
}