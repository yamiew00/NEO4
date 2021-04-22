using System;
using WebApi.Evaluate;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.Setting;
using static WebApi.Setting.FeatureRule;

namespace WebApi.Frames
{
    /// <summary>
    /// 執行順序的判斷
    /// </summary>
    public class OrderingChecker
    {
        /// <summary>
        /// 數字處理器。回傳值仰賴此物件的回傳結果
        /// </summary>
        private NumberMachine NumberMachine;

        /// <summary>
        /// 上一次執行成功的功能
        /// </summary>
        private Feature PreviousFeature;

        /// <summary>
        /// 建構子
        /// </summary>
        public OrderingChecker()
        {
            NumberMachine = new NumberMachine();
            PreviousFeature = Feature.Null;
        }

        /// <summary>
        /// 檢查previousFeature→currentFeature的順序是否符合規範，否則拋出例外
        /// </summary>
        /// <typeparam name="T">定義要回傳的TResult</typeparam>
        /// <param name="currentCast">當前的功能</param>
        /// <param name="func">若順序正確，則要執行的func</param>
        /// <returns>TResult</returns>
        private T ThrowOrderException<T>(Feature currentCast, Func<T> func) 
        {
            if (FeatureRule.IsTheOrderingLegit(PreviousFeature, currentCast))
            {
                return func.Invoke();
            }
            else
            {
                throw new OrderException(FeatureRule.ORDER_EXCEPTION_MSG);
            }
        }
        
        /// <summary>
        /// Feature:Number
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>FrameUpdate</returns>
        public FrameUpdate AddNumber(char number)
        {
            Feature CurrentFeature = Feature.NUMBER;

            return ThrowOrderException<FrameUpdate>(CurrentFeature, () =>
            {
                FrameUpdate frameUpdate = NumberMachine.AddNumber(number);

                //等號後輸入數字
                if (PreviousFeature == Feature.EQUAL)
                {
                    frameUpdate.RemoveLength = ExpUpdate.REMOVE_ALL;

                    //執行成功時記錄下這次的Cast
                    PreviousFeature = CurrentFeature;
                    return frameUpdate;
                }

                //backspace或clearerror之後的數字處理
                if ((PreviousFeature == Feature.BACKSPACE || PreviousFeature == Feature.CLEAR_ERROR) && NumberMachine.NumberField.Number == 0)
                {
                    frameUpdate.RemoveLength += 1;

                    //執行成功時記錄下這次的Cast
                    PreviousFeature = CurrentFeature;
                    return frameUpdate;
                }

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            });
        }

        /// <summary>
        /// 新增雙元運算子
        /// </summary>
        /// <param name="binaryName">雙元運算子</param>
        /// <returns>雙元運算子響應</returns>
        public FrameUpdate AddBinary(char binaryName)
        {
            Feature CurrentFeature = Feature.BINARY;

            return ThrowOrderException<FrameUpdate>(CurrentFeature, () =>
            {
                FrameUpdate frameUpdate;

                if (PreviousFeature == Feature.Null)
                {
                    NumberMachine.AddNumber('0');
                    frameUpdate = NumberMachine.AddBinary(binaryName);
                    //要補0
                    frameUpdate.UpdateString = $"0{frameUpdate.UpdateString}";
                }
                else if (PreviousFeature == Feature.BINARY)
                {
                    frameUpdate = NumberMachine.ModifyBinary(binaryName);
                }
                else
                {
                    frameUpdate = NumberMachine.AddBinary(binaryName);
                }

                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.BINARY;
                return frameUpdate;
            });
        }

        /// <summary>
        /// 新增一個單元運算子 
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public FrameUpdate AddUnary(char unary)
        {
            Feature CurrentFeature = Feature.UNARY;

            return ThrowOrderException<FrameUpdate>(CurrentFeature, () =>
            {
                FrameUpdate frameUpdate;

                //如果單元連按會有迭代的表現方式
                if (PreviousFeature == Feature.UNARY)
                {
                    frameUpdate = NumberMachine.AddUnaryCombo(unary);
                }
                else
                {
                    //非迭代的case
                    frameUpdate = NumberMachine.AddUnary(unary);
                }

                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.UNARY;
                return frameUpdate;
            });
        }

        /// <summary>
        /// 等號事件
        /// </summary>
        /// <returns>等號響應</returns>
        public FrameUpdate Equal()
        {
            Feature CurrentFeature = Feature.EQUAL;

            return ThrowOrderException<FrameUpdate>(CurrentFeature, () =>
            {
                FrameUpdate frameUpdate;
                if (PreviousFeature == Feature.Null)
                {
                    return new FrameUpdate("0", new ExpUpdate(removeLength: 0, updateString: "0="));
                }
                else if (PreviousFeature == Feature.EQUAL)
                {
                    //這個怎麼辦
                }
                else if (PreviousFeature == Feature.BINARY)
                {
                    System.Diagnostics.Debug.WriteLine("there1");
                    frameUpdate = NumberMachine.BinaryAndEqualCombo();
                    //執行成功時記錄下這次的Cast
                    PreviousFeature = Feature.EQUAL;
                    return frameUpdate;
                }
                frameUpdate = NumberMachine.Equal();

                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.EQUAL;
                return frameUpdate;
            });
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        /// <returns>ExpUpdate</returns>
        public ExpUpdate LeftBracket()
        {
            Feature CurrentFeature = Feature.LEFT_BRACKET;
            
            return ThrowOrderException<ExpUpdate>(CurrentFeature, () =>
            {
                ExpUpdate expUpdate = NumberMachine.LeftBracket();

                if (PreviousFeature == Feature.EQUAL)
                {
                    expUpdate.RemoveLength = ExpUpdate.REMOVE_ALL;
                }
                
                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.LEFT_BRACKET;
                return expUpdate;
            });
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        /// <returns>FrameUpdate</returns>
        public FrameUpdate RightBracket()
        {
            Feature CurrentFeature = Feature.RIGHT_BRACKET;

            return ThrowOrderException<FrameUpdate>(CurrentFeature, () =>
            {
                FrameUpdate frameUpdate = NumberMachine.RightBracket();
                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.RIGHT_BRACKET;
                return frameUpdate;
            });
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            Feature CurrentFeature = Feature.CLEAR;

            if (FeatureRule.IsTheOrderingLegit(PreviousFeature, CurrentFeature))
            {
                NumberMachine.Clear();
                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.Null;
            }
            else
            {
                throw new OrderException(FeatureRule.ORDER_EXCEPTION_MSG);
            }
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        /// <returns>ClearError響應</returns>
        public ExpUpdate ClearError()
        {
            Feature CurrentFeature = Feature.CLEAR_ERROR;

            return ThrowOrderException<ExpUpdate>(CurrentFeature, () =>
            {
                ExpUpdate expUpdate = NumberMachine.ClearError();
                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.CLEAR_ERROR;
                return expUpdate;
            });
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns>返回響應</returns>
        public ExpUpdate BackSpace()
        {
            Feature CurrentFeature = Feature.BACKSPACE;

            return ThrowOrderException<ExpUpdate>(CurrentFeature, () =>
            {
                ExpUpdate expUpdate = NumberMachine.BackSpace();
                //執行成功時記錄下這次的Cast
                PreviousFeature = Feature.BACKSPACE;
                return expUpdate;
            });
        }
    }
}