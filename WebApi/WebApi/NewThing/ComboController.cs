using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using WebApi.Models.Response;

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
            Action = string.Empty;
        }

        private string Action;

        public string Number(char number)
        {
            Action = "Number";
            return Newcontroller.Instance.AddNumber(number);
        }

        public BinaryResponse AddBinary(char text)
        {
            BinaryResponse response;
            if (Action.Equals("Binary"))
            {
                response = Newcontroller.Instance.ModifyBinary(text);
            }
            else
            {
                response = Newcontroller.Instance.AddBinary(text);
            }

            //上述行動若失效的話，程式不應該走到這裡
            Action = "Binary";
            return response;
        }

        public decimal Equal()
        {
            Action = "Equal";
            return Newcontroller.Instance.Equal();
        }

        public void LeftBracket()
        {
            Action = "LeftBracket";
            Newcontroller.Instance.LeftBracket();
        }

        public void RightBracket()
        {
            Action = "RightBracket";
            Newcontroller.Instance.RightBracket();
        }


        public void Clear()
        {
            Action = "Clear";
            Newcontroller.Instance.Clear();
        }

        public int ClearError()
        {
            Action = "ClearError";
            return Newcontroller.Instance.ClearError();
        }

        public BackSpaceJson BackSpace()
        {
            Action = "BackSpace";
            return Newcontroller.Instance.BackSpace();
        }

        public UnaryResponseJson AddUnary(char unary)
        {
            Action = "Unary";
            return Newcontroller.Instance.AddUnary(unary);
        }
    }
}