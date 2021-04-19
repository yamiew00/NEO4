using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.Setting;
using static WebApi.Setting.CastRule;

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
        private NumberMachine InputMachine;

        /// <summary>
        /// 上一次執行成功的功能
        /// </summary>
        private Cast PreviousCast;

        /// <summary>
        /// 建構子
        /// </summary>
        public CommandCaster()
        {
            InputMachine = new NumberMachine();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            PreviousCast = Cast.NUMBER;
            InputMachine.Clear();
            InputMachine.AddNumber('0');
        }

        /// <summary>
        /// 檢查previousCast→currentCast的順序是否符合規範，否則拋出例外
        /// </summary>
        /// <typeparam name="T">定義要回傳的TResult</typeparam>
        /// <param name="currentCast">當前的功能</param>
        /// <param name="func">若順序正確，則要執行的func</param>
        /// <returns>TResult</returns>
        private T CheckOrder<T>(Cast currentCast, Func<T> func) 
        {
            if (CastRule.IsTheOrderingLegit(PreviousCast, currentCast))
            {
                return func.Invoke();
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        /// <summary>
        /// 檢查previousCast→currentCast的順序是否符合規範，否則拋出例外
        /// </summary>
        /// <param name="currentCast">當前的功能</param>
        /// <param name="action">若順序正確，則要執行的action</param>
        private void CheckOrder(Cast currentCast, Action action)
        {
            if (CastRule.IsTheOrderingLegit(PreviousCast, currentCast))
            {
                action.Invoke();
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        /// <summary>
        /// 新增數字
        /// </summary>
        /// <param name="number">數字</param>
        /// <returns>數字響應</returns>
        public NumberResponse AddNumber(char number) 
        {
            Cast CurrentCast = Cast.NUMBER;

            return CheckOrder<NumberResponse>(CurrentCast, () =>
            {
                NumberResponse successResponse;

                //等號後輸入數字
                if (PreviousCast == Cast.EQUAL)
                {
                    successResponse = InputMachine.AddNumber(number);
                    successResponse.SetStatus(999);
                    PreviousCast = CurrentCast;
                    return successResponse;
                }
                
                //backspace或clearerror之後的數字處理
                if ((PreviousCast == Cast.BACKSPACE || PreviousCast == Cast.CLEAR_ERROR) && InputMachine.NumberField.Number == 0)
                {
                    successResponse = InputMachine.AddNumber(number);
                    successResponse.Update.RemoveLength += 1;
                    //執行成功時記錄下這次的Cast
                    PreviousCast = CurrentCast;
                    return successResponse;
                }

                 successResponse = InputMachine.AddNumber(number);
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
        public BinaryResponse AddBinary(char binaryName)
        {
            Cast CurrentCast = Cast.BINARY;

            return CheckOrder<BinaryResponse>(CurrentCast, () =>
          {
              BinaryResponse successResponse;

              if (PreviousCast == Cast.BINARY)
              {
                  successResponse = InputMachine.ModifyBinary(binaryName);
              }
              else
              {
                  successResponse = InputMachine.AddBinary(binaryName);
              }

                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.BINARY;
              return successResponse;
          });
        }

        /// <summary>
        /// 等號事件
        /// </summary>
        /// <returns>等號響應</returns>
        public EqualResponse Equal()
        {
            Cast CurrentCast = Cast.EQUAL;

            return CheckOrder<EqualResponse>(CurrentCast, () =>
          {
              var successResponse = InputMachine.Equal();

              //執行成功時記錄下這次的Cast
              PreviousCast = Cast.EQUAL;
              return successResponse;
          });
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        public void LeftBracket()
        {
            Cast CurrentCast = Cast.LEFT_BRACKET;

            CheckOrder(CurrentCast, () =>
            {
                InputMachine.LeftBracket();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.LEFT_BRACKET;
            });
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        public void RightBracket()
        {
            Cast CurrentCast = Cast.RIGHT_BRACKET;

            CheckOrder(CurrentCast, () =>
            {
                InputMachine.RightBracket();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.RIGHT_BRACKET;
            });
        }
        
        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            Cast CurrentCast = Cast.CLEAR;

            CheckOrder(CurrentCast, () =>
            {
                InputMachine.Clear();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.CLEAR;
            });
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        /// <returns>ClearError響應</returns>
        public ClearErrorResponse ClearError()
        {
            Cast CurrentCast = Cast.CLEAR_ERROR;

            return CheckOrder<ClearErrorResponse>(CurrentCast, () =>
          {
              var successResponse = InputMachine.ClearError();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.CLEAR_ERROR;
              return successResponse;
          });
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns>返回響應</returns>
        public BackSpaceResponse BackSpace()
        {
            Cast CurrentCast = Cast.BACKSPACE;

            return CheckOrder<BackSpaceResponse>(CurrentCast, () =>
          {
              var successResponse = InputMachine.BackSpace();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.BACKSPACE;
              return successResponse;
          });
        }
        
        /// <summary>
        /// 新增一個單元運算子 
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public UnaryResponse AddUnary(char unary)
        {
            Cast CurrentCast = Cast.UNARY;

            return CheckOrder<UnaryResponse>(CurrentCast, () =>
          {
              var successResponse = InputMachine.AddUnary(unary);
              //執行成功時記錄下這次的Cast
              PreviousCast = Cast.UNARY;
              return successResponse;
          });
        }
    }
}