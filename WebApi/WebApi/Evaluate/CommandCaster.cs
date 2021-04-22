using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.Setting;
using static WebApi.Setting.FeatureRule;

namespace WebApi.NewThing
{
    /// <summary>
    /// 功能執行判斷
    /// </summary>
    public class CommandCaster
    {
        /// <summary>
        /// 數字處理器
        /// </summary>
        private NumberMachine NumberMachine;

        /// <summary>
        /// 上一次執行成功的功能
        /// </summary>
        private Feature PreviousCast;

        /// <summary>
        /// 建構子
        /// </summary>
        public CommandCaster()
        {
            NumberMachine = new NumberMachine();
            PreviousCast = Feature.Null;
        }
        

        /// <summary>
        /// 檢查previousCast→currentCast的順序是否符合規範，否則拋出例外
        /// </summary>
        /// <typeparam name="T">定義要回傳的TResult</typeparam>
        /// <param name="currentCast">當前的功能</param>
        /// <param name="func">若順序正確，則要執行的func</param>
        /// <returns>TResult</returns>
        private T CheckOrder<T>(Feature currentCast, Func<T> func) 
        {
            if (FeatureRule.IsTheOrderingLegit(PreviousCast, currentCast))
            {
                return func.Invoke();
            }
            else
            {
                throw new OrderException(FeatureRule.INCORRECT_ORDER_MSG);
            }
        }
        
        //以下新的
        public NumberUpdate AddNumber(char number)
        {
            Feature CurrentCast = Feature.NUMBER;

            return CheckOrder<NumberUpdate>(CurrentCast, () =>
            {
                NumberUpdate successResponse;

                //等號後輸入數字
                if (PreviousCast == Feature.EQUAL)
                {
                    successResponse = NumberMachine.AddNumber(number);
                    successResponse.RemoveLength = Updates.REMOVE_ALL;
                    PreviousCast = CurrentCast;
                    return successResponse;
                }

                //backspace或clearerror之後的數字處理
                if ((PreviousCast == Feature.BACKSPACE || PreviousCast == Feature.CLEAR_ERROR) && NumberMachine.NumberField.Number == 0)
                {
                    successResponse = NumberMachine.AddNumber(number);
                    successResponse.RemoveLength += 1;
                    //執行成功時記錄下這次的Cast
                    PreviousCast = CurrentCast;
                    return successResponse;
                }

                successResponse = NumberMachine.AddNumber(number);
                //執行成功時記錄下這次的Cast
                PreviousCast = CurrentCast;

                return successResponse;
            });
        }

        /// <summary>
        /// 新增雙元運算子
        /// </summary>
        /// <param name="binaryName">雙元運算子</param>
        /// <returns>雙元運算子響應</returns>
        public BinaryUpdate AddBinary(char binaryName)
        {
            Feature CurrentCast = Feature.BINARY;

            return CheckOrder<BinaryUpdate>(CurrentCast, () =>
            {
                BinaryUpdate successResponse;

                if (PreviousCast == Feature.Null)
                {
                    NumberMachine.AddNumber('0');
                    successResponse = NumberMachine.AddBinary(binaryName);
                    //要補0
                    successResponse.UpdateString = $"0{successResponse.UpdateString}";
                }
                else if (PreviousCast == Feature.BINARY)
                {
                    successResponse = NumberMachine.ModifyBinary(binaryName);
                }
                else
                {
                    successResponse = NumberMachine.AddBinary(binaryName);
                }

                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.BINARY;
                return successResponse;
            });
        }

        

        /// <summary>
        /// 新增一個單元運算子 
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public UnaryUpdate AddUnary(char unary)
        {
            Feature CurrentCast = Feature.UNARY;

            return CheckOrder<UnaryUpdate>(CurrentCast, () =>
            {
                UnaryUpdate successResponse;

                //如果單元連按會有迭代的表現方式
                if (PreviousCast == Feature.UNARY)
                {
                    successResponse = NumberMachine.AddUnaryCombo(unary);
                }
                else
                {
                    //非迭代的case
                    successResponse = NumberMachine.AddUnary(unary);
                }

                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.UNARY;
                return successResponse;
            });
        }


        /// <summary>
        /// 等號事件
        /// </summary>
        /// <returns>等號響應</returns>
        public EqualUpdate Equal()
        {
            Feature CurrentCast = Feature.EQUAL;

            return CheckOrder<EqualUpdate>(CurrentCast, () =>
            {
                EqualUpdate successResponse;
                if (PreviousCast == Feature.Null)
                {
                    return new EqualUpdate("0", new Updates(removeLength: 0, updateString: "0="));
                }
                else if (PreviousCast == Feature.EQUAL)
                {
                    //這個怎麼辦
                }
                else if (PreviousCast == Feature.BINARY)
                {
                    System.Diagnostics.Debug.WriteLine("there1");
                    successResponse = NumberMachine.BinaryAndEqualCombo();
                    //執行成功時記錄下這次的Cast
                    PreviousCast = Feature.EQUAL;
                    return successResponse;
                }
                successResponse = NumberMachine.Equal();

                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.EQUAL;
                return successResponse;
            });
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        public Updates LeftBracket()
        {
            Feature CurrentCast = Feature.LEFT_BRACKET;
            
            return CheckOrder<Updates>(CurrentCast, () =>
            {
                var response = NumberMachine.LeftBracket();

                if (PreviousCast == Feature.EQUAL)
                {
                    response.RemoveLength = Updates.REMOVE_ALL;
                }
                
                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.LEFT_BRACKET;
                return response;
            });
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        public RightBracketUpdate RightBracket()
        {
            Feature CurrentCast = Feature.RIGHT_BRACKET;

            return CheckOrder<RightBracketUpdate>(CurrentCast, () =>
            {
                var response = NumberMachine.RightBracket();
                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.RIGHT_BRACKET;
                return response;
            });
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            Feature CurrentCast = Feature.CLEAR;

            if (FeatureRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                NumberMachine.Clear();
                //執行成功時記錄下這次的Cast
                //PreviousCast = Cast.CLEAR;
                PreviousCast = Feature.Null;
            }
            else
            {
                throw new OrderException(FeatureRule.INCORRECT_ORDER_MSG);
            }
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        /// <returns>ClearError響應</returns>
        public Updates ClearError()
        {
            Feature CurrentCast = Feature.CLEAR_ERROR;

            return CheckOrder<Updates>(CurrentCast, () =>
            {
                var successResponse = NumberMachine.ClearError();
                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.CLEAR_ERROR;
                return successResponse;
            });
        }


        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns>返回響應</returns>
        public Updates BackSpace()
        {
            Feature CurrentCast = Feature.BACKSPACE;

            return CheckOrder<Updates>(CurrentCast, () =>
            {
                var successResponse = NumberMachine.BackSpace();
                //執行成功時記錄下這次的Cast
                PreviousCast = Feature.BACKSPACE;
                return successResponse;
            });
        }
    }
}