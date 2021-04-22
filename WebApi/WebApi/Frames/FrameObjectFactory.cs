using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.Frames
{
    /// <summary>
    /// 製造FrameObject的工廠。每個用戶都有唯一一個專屬的工廠。
    /// </summary>
    public class FrameObjectFactory
    {
        /// <summary>
        /// 記錄當下的Frame內容
        /// </summary>
        private static FrameObject FrameObject;

        /// <summary>
        /// 記錄當下的完整運算式
        /// </summary>
        private static string CompleteExpression;

        /// <summary>
        /// 執行順序判斷器。製作過程完全仰賴此物件回傳的結果。
        /// </summary>
        private static OrderingChecker OrderingChecker;
        
        /// <summary>
        /// 建構子
        /// </summary>
        public FrameObjectFactory()
        {
            FrameObject = new FrameObject();
            CompleteExpression = string.Empty;
            OrderingChecker = new OrderingChecker();
        }

        /// <summary>
        /// 例外狀況處理的字典
        /// </summary>
        private static Dictionary<Type, Func<FrameObject>> ExceptionDic = new Dictionary<Type, Func<FrameObject>>()
        {
            {
                typeof(OrderException), () =>
                {
                    //OrderException:命令順序有誤→畫面維持原狀
                    return FrameObject;
                }
            },
            {
                typeof(BracketException), () =>
                {
                    //BracketException:括號數量不正確→畫面維持原狀
                    return FrameObject;
                }
            },
            {
                typeof(TooLongDigitException), () =>
                {
                    //TooLongDigitException:輸入位數限制→畫面維持原狀
                    return FrameObject;
                }
            },
            {
                typeof(SquareRootException), () =>
                {
                    //對負號做根號運算→回傳錯訊
                    var subpanel = FrameObject.SubPanel;

                    //清除
                    OrderingChecker.Clear();
                    FrameObject = new FrameObject();
                    CompleteExpression = string.Empty;

                    return new FrameObject(panel: "無效的輸入", subPanel: subpanel);
                }
            },
            {
                typeof(DivideByZeroException), () =>
                {
                    //零在分母→回傳錯訊
                    var subpanel = FrameObject.SubPanel;

                    //清除
                    OrderingChecker.Clear();
                    FrameObject = new FrameObject();
                    CompleteExpression = string.Empty;

                    return new FrameObject(panel: "無法除以零。", subPanel: subpanel);
                }
            }
        };

        /// <summary>
        /// 製作過程中各種Exception的處理
        /// </summary>
        /// <param name="func">生產過程</param>
        /// <returns>對應情況的FrameObject</returns>
        private FrameObject CatchException(Func<FrameObject> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception exception)
            {
                Type type = exception.GetType();
                if (ExceptionDic.Keys.Contains(type))
                {
                    return ExceptionDic[type]();
                }
                else
                {
                    //其餘的Exception(未處理)
                    throw new Exception("未處理狀況:" + exception.Message);
                }
            }
        }

        /// <summary>
        /// Feature:Number。輸入為數字或小數點。
        /// </summary>
        /// <param name="content">數字或小數點</param>
        /// <returns>FrameObject</returns>
        public FrameObject AddNumber(char content)
        {
            return CatchException(() =>
            {
                //OrderingChecker會回傳FrameUpdate
                FrameUpdate frameUpdate = OrderingChecker.AddNumber(content);
                string answer = frameUpdate.Answer;

                //完整運算式的刷新
                CompleteExpression = frameUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定
                FrameObject.SubPanel = CompleteExpression.Substring(0, CompleteExpression.Length - answer.Length);
                FrameObject.Panel = answer;

                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:Binary。輸入為Setting中的雙元運算子。
        /// </summary>
        /// <param name="content">雙元運算子</param>
        /// <returns>FrameObject</returns>
        public FrameObject AddBinary(char content)
        {
            return CatchException(() =>
            {
                //OrderingChecker會回傳Update與要給Panel的numberString
                FrameUpdate frameUpdate = OrderingChecker.AddBinary(content);

                //完整運算式的刷新
                CompleteExpression = frameUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = frameUpdate.Answer;

                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:Unary。輸入為Setting中的單元運算子。
        /// </summary>
        /// <param name="content">單元運算子</param>
        /// <returns>FrameObject</returns>
        public FrameObject AddUnary(char content)
        {
            return CatchException(() =>
            {
                //OrderingChecker會回傳FrameUpdate
                FrameUpdate frameUpdate = OrderingChecker.AddUnary(content);

                //完整運算式的刷新
                CompleteExpression = frameUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = frameUpdate.Answer;
                
                return FrameObject;
            });
        }
        
        /// <summary>
        /// Feature:Equal。輸入等號
        /// </summary>
        /// <returns>FrameObject</returns>
        public FrameObject Equal()
        {
            return CatchException(() =>
            {
                FrameUpdate frameUpdate = OrderingChecker.Equal();

                //完整運算式的刷新
                CompleteExpression = frameUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = frameUpdate.Answer;
                
                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:LeftBracket。輸入左括號
        /// </summary>
        /// <returns>FrameObject</returns>
        public FrameObject LeftBracket()
        {
            return CatchException(() =>
            {
                ExpUpdate expUpdate = OrderingChecker.LeftBracket();

                //完整運算式的刷新
                CompleteExpression = expUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定。subpanel強制給0
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = "0";
                
                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:RightBracket。輸入右括號
        /// </summary>
        /// <returns>FrameObject</returns>
        public FrameObject RightBracket()
        {
            return CatchException(() =>
            {
                FrameUpdate frameUpdate = OrderingChecker.RightBracket();

                //完整運算式的刷新
                CompleteExpression = frameUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定。
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = frameUpdate.Answer;
                
                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:Clear。輸入清除
        /// </summary>
        /// <returns></returns>
        public FrameObject Clear()
        {
            return CatchException(() =>
            {
                OrderingChecker.Clear();

                //完整運算式的刷新
                CompleteExpression = string.Empty;

                //panel, subpanel設定。
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = "0";
                
                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:ClearError。輸入清除錯誤
        /// </summary>
        /// <returns>FrameObject</returns>
        public FrameObject ClearError()
        {
            return CatchException(() =>
            {
                ExpUpdate expUpdate = OrderingChecker.ClearError();

                //完整運算式的刷新
                CompleteExpression = expUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定。
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = "0";
                
                return FrameObject;
            });
        }

        /// <summary>
        /// Feature:BackSpace。輸入返回鍵
        /// </summary>
        /// <returns>FrameObject</returns>
        public FrameObject BackSpace()
        {
            return CatchException(() =>
            {
                ExpUpdate expUpdate = OrderingChecker.BackSpace();

                //完整運算式的刷新
                CompleteExpression = expUpdate.Refresh(CompleteExpression);

                //panel, subpanel設定。
                FrameObject.SubPanel = CompleteExpression;
                FrameObject.Panel = expUpdate.Refresh(FrameObject.Panel);
                
                return FrameObject;
            });
        }
    }
}