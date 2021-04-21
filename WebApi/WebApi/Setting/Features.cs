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
                    //拿到該用戶的caster
                    //CommandCaster commandCaster = Users.GetCommandCaster(userId);
                    //commandCaster.AddNumber(content);
                    ContentController contentController = new ContentController(userId);
                    return contentController.AddNumber(content);
                }
            },
            {
                "Binary", (content, userId) =>
                {
                    //連續Binary時要計算結果還沒做
                    ContentController contentController = new ContentController(userId);
                    return contentController.AddBinary(content);
                }
            },
            {
                "Unary", (content, userId) =>
                {
                    //對負數做根號的處理還沒做
                    ContentController contentController = new ContentController(userId);
                    return contentController.AddUnary(content);
                }
            },
            {
                "Equal", (content, userId) =>
                {
                    //content用不到?
                    ContentController contentController = new ContentController(userId);
                    return contentController.Equal();
                }
            },
            {
                "LeftBracket", (content, userId) =>
                {
                    //content用不到?
                    ContentController contentController = new ContentController(userId);
                    var response = contentController.LeftBracket();
                    return response;
                }
            },
            {
                "RightBracket", (content, userId) =>
                {
                    //content用不到?
                    ContentController contentController = new ContentController(userId);
                    return contentController.RightBracket();
                }
            },
            {
                //Clear的Cast就是null?
                "Clear", (content, userId) =>
                {
                    //content用不到?
                    ContentController contentController = new ContentController(userId);
                    return contentController.Clear();
                }
            },
            {
                "ClearError", (content, userId) =>
                {
                    //content用不到?
                    ContentController contentController = new ContentController(userId);
                    return contentController.ClearError();
                }
            },
            {
                "BackSpace", (content, userId) =>
                {
                    //content用不到?
                    ContentController contentController = new ContentController(userId);
                    return contentController.BackSpace();
                }
            }
        };
    }
}