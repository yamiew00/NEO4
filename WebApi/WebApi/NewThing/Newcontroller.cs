using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.Objects;
using WebApi.Setting;

namespace WebApi.NewThing
{
    /// <summary>
    /// 方法回傳值一律改成response比較好?
    /// </summary>
    public class Newcontroller
    {
        private static Newcontroller _instance = new Newcontroller();
        public static Newcontroller Instance
        {
            get
            {
                return _instance;
            }
        }

        //這裡的實體之後還要處理成用戶獨立
        public ExpressionController expController;

        private Newcontroller()
        {
            expController = new ExpressionController();
        }

        public Number Number;
        
        
        public NumberResponse AddNumber(char number)
        {
            //小數點case
            if (number.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (Number == null)
                {
                    Number = new Number();
                    Number.AddDigit(number);
                    return new NumberResponse(new Updates(removeLength: 0, updateString: "0."));
                }
                //若有值→判斷是否已為小數
                if (Number.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new NumberResponse(new Updates(removeLength: 0, updateString: string.Empty));
                }
                else
                {
                    //不是小數→更新小數點
                    Number.AddDigit(number);
                    return new NumberResponse(new Updates(removeLength: 0, updateString: "."));
                }
            }

            //防呆
            if (!char.IsNumber(number))
            {
                throw new Exception("NewControll.Add該輸入數字");
            }

            //正常的數字case
            if (Number == null)
            {
                Number = new Number(number);
            }
            else
            {
                Number.AddDigit(number);
            }
            return new NumberResponse(new Updates(removeLength: 0, updateString: number.ToString()));
        }

        //binary combo要記得
        //Binary完，Number會變null
        public BinaryResponse AddBinary(char binary)
        {
            BinaryOperator binaryOperator = Operators.GetBinary(binary);

            if (Number == null)
            {
                //可能不完全錯，看是不是第一個
                //throw new Exception("Newcontroller.Binary順序錯誤");
                expController.Add(binaryOperator);
            }
            else
            {
                //.value.value取名超爛
                expController.Add(Number.Value.Value);
                Number = null;
                expController.Add(binaryOperator);
            }

            return new BinaryResponse(new Updates(removeLength: 0, updateString: binary.ToString()));
        }

        public BinaryResponse ModifyBinary(char binary)
        {
            BinaryOperator binaryOperator = Operators.GetBinary(binary);
            if (Number != null)
            {
                throw new Exception("ModifyBinary時Number不能有值");
            }
            expController.Modify(binaryOperator);
            return new BinaryResponse(new Updates(removeLength: 1, updateString: binary.ToString()));
        }

        public EqualResponse Equal()
        {
            //這裡的case應該還有更多
            if (Number != null)
            {
                expController.Add(Number.Value.Value);
                Number = null;
            }

            return new EqualResponse(new Updates(removeLength: 0, updateString: "="), answer: expController.GetResult());
        }

        public void LeftBracket()
        {
            expController.LeftBracket();
        }

        public void RightBracket()
        {
            //要防的呆應該有點多
            //防呆1
            if (Number != null)
            {
                expController.Add(Number.Value.Value);
                System.Diagnostics.Debug.WriteLine("value = " + Number.Value.Value);
            }
            
            expController.RightBracket();
            Number = null;
        }

        public void Clear()
        {
            Number = null;
            expController.Clear();
        }

        public ClearErrorResponse ClearError()
        {
            if (Number == null)
            {
                return new ClearErrorResponse(new Updates(removeLength: 0, updateString: string.Empty));
            }
            int length = Number.Value.Value.ToString().Length;
            System.Diagnostics.Debug.WriteLine($"length = {length}");
            Number = new Number();
            
            return new ClearErrorResponse(new Updates(removeLength: length, updateString: "0"));
        }

        public BackSpaceResponse BackSpace()
        {
            int RemoveLength = 0;
            if (Number == null)
            {
                return new BackSpaceResponse(new Updates(removeLength: RemoveLength, updateString: string.Empty));
            }

            RemoveLength = Number.Value.Value.ToString().Length;
            if (Number.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            Number.ClearOneDigit();
            string updateString = Number.Value.ToString();
            if (Number.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new BackSpaceResponse(new Updates(RemoveLength, updateString));
        }

        public UnaryResponse AddUnary(char unary)
        {
            UnaryOperator unaryOperator = Operators.GetUnary(unary);
            var formula = unaryOperator.Formula;

            int removeLength = 0;
            if (Number == null)
            {
                return new UnaryResponse(new Updates(removeLength: removeLength, updateString: string.Empty));
            }

            removeLength = Number.Value.Value.ToString().Length;
            if (Number.IsEndWithPoint())
            {
                removeLength += 1;
            }

            Number.Value = formula(Number.Value.Value);

            string updateString = Number.Value.ToString();
            if (Number.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new UnaryResponse(new Updates(removeLength, updateString));
        }
    }
}