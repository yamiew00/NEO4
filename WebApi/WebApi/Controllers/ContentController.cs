using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataBase;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.NewThing;

namespace WebApi.Controllers
{
    public class ContentController
    {
        //這個controller之後要做成用戶專屬

        CommandCaster CommandCaster;

        public int UserId;

        public ContentController(int userId)
        {
            CommandCaster = Users.GetCommandCaster(userId);
            UserId = userId;
        }

        public FrameResponse AddNumber(char content)
        {
            var numberUpdate = CommandCaster.NewAddNumber(content);

            var numberString = numberUpdate.NumberString;

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = numberUpdate.Refresh(completeExpression);
            
            //panel, subpanel設定
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.Panel = numberString;
            newFrameResponse.SubPanel = newExpression.Substring(0, newExpression.Length - numberString.Length);

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }

        //「計算當前結果」還沒做
        public FrameResponse AddBinary(char content)
        {
            var update = CommandCaster.NewAddBinary(content);

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }


        public FrameResponse AddUnary(char content)
        {
            var update = CommandCaster.NewAddUnary(content);

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;
            newFrameResponse.Panel = update.NumberString;

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }
        

        public FrameResponse Equal()
        {
            var update = CommandCaster.NewEqual();

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;
            newFrameResponse.Panel = update.NumberString;

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }

        public FrameResponse LeftBracket()
        {
            var update = CommandCaster.NewLeftBracket();

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定。subpanel強制給0
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;
            newFrameResponse.Panel = "0";

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }

        //右括號之後要計算當前答案還沒有做
        public FrameResponse RightBracket()
        {
            var update = CommandCaster.NewRightBracket();

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定。
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }

        public FrameResponse Clear()
        {
            CommandCaster.NewClear();

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = string.Empty;

            //panel, subpanel設定。
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;
            newFrameResponse.Panel = "0";

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }

        public FrameResponse ClearError()
        {
            var update = CommandCaster.NewClearError();

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定。
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;
            newFrameResponse.Panel = "0";

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }

        public FrameResponse BackSpace()
        {
            var update = CommandCaster.NewBackSpace();

            //完整運算式的刷新
            var completeExpression = Users.GetFrameAttribute(UserId).CompleteExpression;
            var newExpression = update.Refresh(completeExpression);

            //panel, subpanel設定。
            var newFrameResponse = Users.GetFrameAttribute(UserId).FrameResponse;
            newFrameResponse.SubPanel = newExpression;
            newFrameResponse.Panel = update.Refresh(newFrameResponse.Panel);

            //更新用戶資料
            Users.SetFrameAttribute(UserId, new FrameAttribute(newFrameResponse, newExpression));

            return Users.GetFrameAttribute(UserId).FrameResponse;
        }
    }
}