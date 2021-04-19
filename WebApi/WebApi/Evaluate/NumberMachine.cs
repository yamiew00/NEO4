﻿using System;
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
                System.Diagnostics.Debug.WriteLine("value = " + NumberField.Number.Value);
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
            System.Diagnostics.Debug.WriteLine($"length = {length}");
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
    }
}