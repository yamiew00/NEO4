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
    public class ComboController
    {
        private static ComboController _instance = new ComboController();
        public static ComboController Instance
        {
            get
            {
                return _instance;
            }
        }
        private ComboController()
        {
        }

        private Cast PreviousCast;

        public void Init()
        {
            PreviousCast = Cast.NUMBER;
            Newcontroller.Instance.Clear();
            Newcontroller.Instance.AddNumber('0');
        }


        public NumberResponse AddNumber(char number) 
        {
            Cast CurrentCast = Cast.NUMBER;
            //Cast null怎麼辦?暫時先當有number
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                //執行成功時記錄下這次的Cast
                PreviousCast = CurrentCast;
                return Newcontroller.Instance.AddNumber(number);
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public BinaryResponse AddBinary(char text)
        {
            Cast CurrentCast = Cast.BINARY;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                BinaryResponse response;

                if (PreviousCast == Cast.BINARY)
                {
                    response = Newcontroller.Instance.ModifyBinary(text);
                }
                else
                {
                    response = Newcontroller.Instance.AddBinary(text);
                }

                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.BINARY;
                return response;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public EqualResponse Equal()
        {
            Cast CurrentCast = Cast.EQUAL;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                var equalResponse = Newcontroller.Instance.Equal();

                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.EQUAL;
                return equalResponse;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public void LeftBracket()
        {
            Cast CurrentCast = Cast.LEFT_BRACKET;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                Newcontroller.Instance.LeftBracket();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.LEFT_BRACKET;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public void RightBracket()
        {
            Cast CurrentCast = Cast.RIGHT_BRACKET;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                Newcontroller.Instance.RightBracket();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.RIGHT_BRACKET;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }
        
        public void Clear()
        {
            Cast CurrentCast = Cast.CLEAR;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                Newcontroller.Instance.Clear();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.CLEAR;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public ClearErrorResponse ClearError()
        {
            Cast CurrentCast = Cast.CLEAR_ERROR;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                var response = Newcontroller.Instance.ClearError();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.CLEAR_ERROR;
                return response;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public BackSpaceResponse BackSpace()
        {
            Cast CurrentCast = Cast.BACKSPACE;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                var response = Newcontroller.Instance.BackSpace();
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.BACKSPACE;
                return response;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }

        public UnaryResponse AddUnary(char unary)
        {
            Cast CurrentCast = Cast.BACKSPACE;
            if (CastRule.IsTheOrderingLegit(PreviousCast, CurrentCast))
            {
                var response = Newcontroller.Instance.AddUnary(unary);
                //執行成功時記錄下這次的Cast
                PreviousCast = Cast.UNARY;
                return response;
            }
            else
            {
                throw new OrderException(CastRule.INCORRECT_ORDER_MSG);
            }
        }
    }
}