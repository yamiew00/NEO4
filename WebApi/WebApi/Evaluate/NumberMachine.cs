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
    /// 數字處理器
    /// </summary>
    public class NumberMachine
    {
        /// <summary>
        /// 表示式控制器
        /// </summary>
        private ExpressionController ExpController;

        /// <summary>
        /// 數字物件
        /// </summary>
        private NumberField _NumberField;

        /// <summary>
        /// 數字物件
        /// </summary>
        public NumberField NumberField
        {
            get
            {
                return _NumberField;
            }

            set
            {
                _NumberField = value;
            }
        }

        /// <summary>
        /// 建構子
        /// </summary>
        public NumberMachine()
        {
            ExpController = new ExpressionController();
        }

        /// <summary>
        /// 新增一個位數
        /// </summary>
        /// <param name="number">一個位數</param>
        /// <returns>數字更新</returns>
        public NumberResponse AddNumber(char number)
        {
            //小數點case
            if (number.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (NumberField == null)
                {
                    NumberField = new NumberField();
                    NumberField.AddDigit(number);
                    return new NumberResponse(new Updates(removeLength: 0, updateString: "0."));
                }
                //若有值→判斷是否已為小數
                if (NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new NumberResponse(new Updates(removeLength: 0, updateString: string.Empty));
                }
                else
                {
                    //不是小數→更新小數點
                    NumberField.AddDigit(number);
                    return new NumberResponse(new Updates(removeLength: 0, updateString: "."));
                }
            }

            //防呆
            if (!char.IsNumber(number))
            {
                throw new Exception("NewControll.Add該輸入數字");
            }

            //正常的數字case
            if (NumberField == null)
            {
                NumberField = new NumberField(number);
            }
            else
            {
                NumberField.AddDigit(number);
            }
            return new NumberResponse(new Updates(removeLength: 0, updateString: number.ToString()));
        }
        
        /// <summary>
        /// 新增一個雙元運算子。成功執行後Number會變null。
        /// </summary>
        /// <param name="binary">雙元運算子</param>
        /// <returns>雙元運算子的響應</returns>
        public BinaryResponse AddBinary(char binary)
        {
            BinaryOperator binaryOperator = Operators.GetBinary(binary);

            if (NumberField == null)
            {
                ExpController.Add(binaryOperator);
            }
            else
            {
                ExpController.Add(NumberField.Number.Value);
                NumberField = null;
                ExpController.Add(binaryOperator);
            }
            return new BinaryResponse(new Updates(removeLength: 0, updateString: binary.ToString()));
        }

        /// <summary>
        /// 更改一個雙元運算子
        /// </summary>
        /// <param name="binary">更換的雙元運算子</param>
        /// <returns>雙元運算子響應</returns>
        public BinaryResponse ModifyBinary(char binary)
        {
            BinaryOperator binaryOperator = Operators.GetBinary(binary);
            if (NumberField != null)
            {
                throw new Exception("ModifyBinary時Number不能有值");
            }
            ExpController.Modify(binaryOperator);
            return new BinaryResponse(new Updates(removeLength: 1, updateString: binary.ToString()));
        }

        /// <summary>
        /// 等號事件
        /// </summary>
        /// <returns>等號響應</returns>
        public EqualResponse Equal()
        {
            //這裡的case應該還有更多
            if (NumberField != null)
            {
                ExpController.Add(NumberField.Number.Value);
                NumberField = null;
            }
            return new EqualResponse(new Updates(removeLength: 0, updateString: "="), answer: ExpController.GetResult());
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        public void LeftBracket()
        {
            ExpController.LeftBracket();
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        public void RightBracket()
        {
            if (NumberField != null)
            {
                ExpController.Add(NumberField.Number.Value);
            }
            
            ExpController.RightBracket();
            NumberField = null;
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            NumberField = null;
            ExpController.Clear();
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        /// <returns>ClearError響應</returns>
        public ClearErrorResponse ClearError()
        {
            if (NumberField == null)
            {
                return new ClearErrorResponse(new Updates(removeLength: 0, updateString: string.Empty));
            }
            int length = NumberField.Number.Value.ToString().Length;
            NumberField = new NumberField();
            
            return new ClearErrorResponse(new Updates(removeLength: length, updateString: "0"));
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns>返回事件響應</returns>
        public BackSpaceResponse BackSpace()
        {
            int RemoveLength = 0;
            if (NumberField == null)
            {
                return new BackSpaceResponse(new Updates(removeLength: RemoveLength, updateString: string.Empty));
            }

            RemoveLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            NumberField.ClearOneDigit();
            string updateString = NumberField.Number.ToString();
            if (NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new BackSpaceResponse(new Updates(RemoveLength, updateString));
        }

        /// <summary>
        /// 新增一個單元運算子
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public UnaryResponse AddUnary(char unary)
        {
            UnaryOperator unaryOperator = Operators.GetUnary(unary);
            var formula = unaryOperator.Formula;

            int removeLength = 0;
            if (NumberField == null)
            {
                return new UnaryResponse(new Updates(removeLength: removeLength, updateString: string.Empty));
            }

            removeLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                removeLength += 1;
            }

            NumberField.Number = formula(NumberField.Number.Value);

            string updateString = NumberField.Number.ToString();
            if (NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new UnaryResponse(new Updates(removeLength, updateString));
        }










        //以下新的

        /// <summary>
        /// 新增一個位數
        /// </summary>
        /// <param name="number">一個位數</param>
        /// <returns>數字更新</returns>
        public NumberUpdate NewAddNumber(char number)
        {
            //小數點case
            if (number.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (NumberField == null)
                {
                    NumberField = new NumberField();
                    NumberField.AddDigit(number);
                    //return new Updates(removeLength: 0, updateString: "0.");
                    return new NumberUpdate("0.", new Updates(removeLength: 0, updateString: "0."));
                }
                //若有值→判斷是否已為小數
                if (NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    //return new Updates(removeLength: 0, updateString: string.Empty);
                    return new NumberUpdate(NumberField.Number.ToString() + ".", new Updates(removeLength: 0, updateString: string.Empty));
                }
                else
                {
                    //不是小數→更新小數點
                    NumberField.AddDigit(number);
                    //return new Updates(removeLength: 0, updateString: ".");
                    return new NumberUpdate(NumberField.Number.ToString() + ".", new Updates(removeLength: 0, updateString: "."));
                }
            }

            //防呆
            if (!char.IsNumber(number))
            {
                throw new Exception("NewControll.Add該輸入數字");
            }

            //正常的數字case
            if (NumberField == null)
            {
                NumberField = new NumberField(number);
            }
            else
            {
                NumberField.AddDigit(number);
            }
            //return new Updates(removeLength: 0, updateString: number.ToString());
            return new NumberUpdate(NumberField.Number.ToString(), new Updates(removeLength: 0, updateString: number.ToString()));
        }

        /// <summary>
        /// 更改一個雙元運算子
        /// </summary>
        /// <param name="binary">更換的雙元運算子</param>
        /// <returns>雙元運算子響應</returns>
        public Updates NewModifyBinary(char binary)
        {
            BinaryOperator binaryOperator = Operators.GetBinary(binary);
            if (NumberField != null)
            {
                throw new Exception("ModifyBinary時Number不能有值");
            }
            ExpController.Modify(binaryOperator);
            return new Updates(removeLength: 1, updateString: binary.ToString());
        }

        /// <summary>
        /// 新增一個雙元運算子。成功執行後Number會變null。
        /// </summary>
        /// <param name="binary">雙元運算子</param>
        /// <returns>雙元運算子的響應</returns>
        public Updates NewAddBinary(char binary)
        {
            BinaryOperator binaryOperator = Operators.GetBinary(binary);

            if (NumberField == null)
            {
                ExpController.Add(binaryOperator);
            }
            else
            {
                ExpController.Add(NumberField.Number.Value);
                NumberField = null;
                ExpController.Add(binaryOperator);
            }
            return new Updates(removeLength: 0, updateString: binary.ToString());
        }

        private string TmpUnaryString;

        /// <summary>
        /// 新增一個單元運算子
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public UnaryUpdate NewAddUnary(char unary)
        {
            //tmp的處理
            TmpUnaryString = string.Empty;
            
            //必須case by case了
            UnaryOperator unaryOperator = Operators.GetUnary(unary);
            var formula = unaryOperator.Formula;

            int removeLength = 0;
            if (NumberField == null)
            {
                return new UnaryUpdate("0", new Updates(removeLength: removeLength, updateString: string.Empty));
            }

            removeLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                removeLength += 1;
            }

            //暫存
            var OriginNumberString = NumberField.Number.ToString();

            //計算後
            NumberField.Number = formula(NumberField.Number.Value);
            
            string updateString = string.Empty;

            //必須case by case處理
            if (unary == '√')
            {
                updateString = $"√({OriginNumberString})";
            }
            else if (unary == '±')
            {
                updateString = $"negate({OriginNumberString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }
            //return new Updates(removeLength, updateString));
            //記住這次的結果
            TmpUnaryString = updateString;
            return new UnaryUpdate(NumberField.Number.ToString(), new Updates(removeLength, updateString));
        }



        /// <summary>
        /// 新增一個單元運算子
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public UnaryUpdate NewAddUnaryCombo(char unary)
        {
            //必須case by case了
            UnaryOperator unaryOperator = Operators.GetUnary(unary);
            var formula = unaryOperator.Formula;

            int removeLength = TmpUnaryString.Length;

            //計算後
            NumberField.Number = formula(NumberField.Number.Value);

            //必須case by case處理
            if (unary == '√')
            {
                TmpUnaryString = $"√({TmpUnaryString})";
            }
            else if (unary == '±')
            {
                TmpUnaryString = $"negate({TmpUnaryString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }
            return new UnaryUpdate(NumberField.Number.ToString(), new Updates(removeLength, TmpUnaryString));
        }

        /// <summary>
        /// 等號事件
        /// </summary>
        /// <returns>等號響應</returns>
        public EqualUpdate NewEqual()
        {
            //這裡的case應該還有更多
            if (NumberField != null)
            {
                ExpController.Add(NumberField.Number.Value);
                NumberField = null;
            }
            //return new EqualResponse(new Updates(removeLength: 0, updateString: "="), answer: ExpController.GetResult());
            return new EqualUpdate(ExpController.GetResult().ToString(), new Updates(removeLength: 0, updateString: "="));
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        public Updates NewLeftBracket()
        {
            ExpController.LeftBracket();
            return new Updates(removeLength: 0, updateString: "(");
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        public Updates NewRightBracket()
        {
            if (NumberField != null)
            {
                ExpController.Add(NumberField.Number.Value);
            }

            ExpController.RightBracket();
            NumberField = null;

            return new Updates(removeLength: 0, updateString: ")");
        }


        /// <summary>
        /// Clear事件
        /// </summary>
        public void NewClear()
        {
            NumberField = null;
            ExpController.Clear();
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        /// <returns>ClearError響應</returns>
        public Updates NewClearError()
        {
            if (NumberField == null)
            {
                return new Updates(removeLength: 0, updateString: string.Empty);
            }
            int length = NumberField.Number.Value.ToString().Length;
            NumberField = new NumberField();

            return new Updates(removeLength: length, updateString: "0");
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns>返回事件響應</returns>
        public Updates NewBackSpace()
        {
            int RemoveLength = 0;
            if (NumberField == null)
            {
                return new Updates(removeLength: RemoveLength, updateString: string.Empty);
            }

            RemoveLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            NumberField.ClearOneDigit();
            string updateString = NumberField.Number.ToString();
            if (NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            //return new BackSpaceResponse(new Updates(RemoveLength, updateString));
            return new Updates(RemoveLength, updateString);
        }
    }
}