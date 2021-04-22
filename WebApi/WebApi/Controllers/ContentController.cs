using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataBase;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.NewThing;

namespace WebApi.Controllers
{
    public class ContentController
    {
        
        CommandCaster CommandCaster;

        //public int UserId;

        public ContentController()
        {
            CommandCaster = new CommandCaster();
            FrameResponse = new FrameResponse();
            completeExpression = string.Empty;
            //UserId = userId;
        }


        //搬屬性
        //private FrameAttribute FrameAttribute;
        private FrameResponse FrameResponse;
        private string completeExpression;


        //只要catch到OrderException→直接回傳現有畫面
        private FrameResponse CatchException(Func<FrameResponse> func)
        {
            try
            {
                return func.Invoke();
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    //發生OrderException→畫面維持原狀
                    return FrameResponse;
                }
                else if (exception is BracketException)
                {
                    //寫在這裡好嗎
                    //發生BracketException→畫面維持原狀
                    return FrameResponse;
                }
                else if (exception is TooLongDigitException)
                {
                    //寫在這裡好嗎
                    //發生BracketException→畫面維持原狀
                    return FrameResponse;
                }
                else if (exception.Message.Equals("嘗試以零除。"))
                {
                    var subpanel = FrameResponse.SubPanel;
                    //清除
                    CommandCaster.Clear();
                    FrameResponse = new FrameResponse();
                    completeExpression = string.Empty;

                    return new FrameResponse(panel: "無法除以零。", subPanel: subpanel);
                }
                else if (exception is SquareRootException)
                {
                    //寫在這裡好嗎
                    var subpanel = FrameResponse.SubPanel;

                    //清除
                    CommandCaster.Clear();
                    FrameResponse = new FrameResponse();
                    completeExpression = string.Empty;

                    return new FrameResponse(panel: exception.Message, subPanel: subpanel);
                }
                else
                {
                    throw new Exception("未處理狀況:" + exception.Message);
                }
            }
        }

        public FrameResponse AddNumber(char content)
        {
            return CatchException(() =>
            {
                var numberUpdate = CommandCaster.AddNumber(content);

                var numberString = numberUpdate.NumberString;

                //完整運算式的刷新
                completeExpression = numberUpdate.Refresh(completeExpression);

                //panel, subpanel設定
                FrameResponse.Panel = numberString;
                FrameResponse.SubPanel = completeExpression.Substring(0, completeExpression.Length - numberString.Length);

                return FrameResponse;
            });
        }

        //「計算當前結果」還沒做
        public FrameResponse AddBinary(char content)
        {
            return CatchException(() =>
            {
                var update = CommandCaster.AddBinary(content);

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = ((BinaryUpdate)update).NumberString;

                return FrameResponse;
            });
        }


        public FrameResponse AddUnary(char content)
        {
            return CatchException(() =>
            {
                var update = CommandCaster.AddUnary(content);

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = update.NumberString;
                
                return FrameResponse;
            });
        }
        

        public FrameResponse Equal()
        {
            return CatchException(() =>
            {
                var update = CommandCaster.Equal();

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = update.NumberString;
                
                return FrameResponse;
            });
        }

        public FrameResponse LeftBracket()
        {
            return CatchException(() =>
            {
                var update = CommandCaster.LeftBracket();

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定。subpanel強制給0
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = "0";
                
                return FrameResponse;
            });
        }

        //右括號之後要計算當前答案還沒有做
        public FrameResponse RightBracket()
        {
            return CatchException(() =>
            {
                var update = CommandCaster.RightBracket();

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定。
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = update.NumberString;
                
                return FrameResponse;
            });
        }

        public FrameResponse Clear()
        {
            return CatchException(() =>
            {
                CommandCaster.Clear();

                //完整運算式的刷新
                completeExpression = string.Empty;

                //panel, subpanel設定。
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = "0";
                
                return FrameResponse;
            });
        }

        public FrameResponse ClearError()
        {
            return CatchException(() =>
            {
                var update = CommandCaster.ClearError();

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定。
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = "0";
                
                return FrameResponse;
            });
        }

        public FrameResponse BackSpace()
        {
            return CatchException(() =>
            {
                var update = CommandCaster.BackSpace();

                //完整運算式的刷新
                completeExpression = update.Refresh(completeExpression);

                //panel, subpanel設定。
                FrameResponse.SubPanel = completeExpression;
                FrameResponse.Panel = update.Refresh(FrameResponse.Panel);
                
                return FrameResponse;
            });
        }
    }
}