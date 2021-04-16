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
    public class CommandCaster
    {

        private InputMachine InputMachine;

        private Cast PreviousCast;

        public CommandCaster(){
            InputMachine = new InputMachine();
        }

        public void Init()
        {
            PreviousCast = Cast.NUMBER;
            InputMachine.Clear();
            InputMachine.AddNumber('0');
        }


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

        public NumberResponse AddNumber(char number) 
        {
            Cast CurrentCast = Cast.NUMBER;

            return CheckOrder<NumberResponse>(CurrentCast, () =>
            {
                var successResponse = InputMachine.AddNumber(number);
                //執行成功時記錄下這次的Cast
                PreviousCast = CurrentCast;
                return successResponse;
            });

        }

        public BinaryResponse AddBinary(char text)
        {
            Cast CurrentCast = Cast.BINARY;

            return CheckOrder<BinaryResponse>(CurrentCast, () =>
          {
              BinaryResponse successResponse;

              if (PreviousCast == Cast.BINARY)
              {
                  successResponse = InputMachine.ModifyBinary(text);
              }
              else
              {
                  successResponse = InputMachine.AddBinary(text);
              }

                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.BINARY;
              return successResponse;
          });
        }

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