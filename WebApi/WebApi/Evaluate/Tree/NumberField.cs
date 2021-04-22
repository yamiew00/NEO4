using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Exceptions;
using WebApi.Setting;

namespace WebApi.Evaluate.Tree
{
    /// <summary>
    /// 數字物件
    /// </summary>
    public class NumberField
    {
        /// <summary>
        /// 值
        /// </summary>
        public decimal? Number { get; set; }

        /// <summary>
        /// 有小數點的布林值(可能仍為整數)
        /// </summary>
        public bool IsNumeric;

        /// <summary>
        /// 小數點後有幾位
        /// </summary>
        private int Degree;

        /// <summary>
        /// 建構子
        /// </summary>
        public NumberField()
        {
            Number = 0;
            IsNumeric = false;
            Degree = 0;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="number">數字或小數點的char</param>
        public NumberField(char number)
        {
            Number = int.Parse(number.ToString());
            IsNumeric = false;
            Degree = 0;
        }
        
        /// <summary>
        /// 新增一個位數(或小數點)。
        /// </summary>
        /// <param name="number">一個位數</param>
        public void AddDigit(char number)
        {
            if (Number.HasValue && Number.Value.ToString().Length >= Global.MAX_DIGIT_LENGTH)
            {
                throw new TooLongDigitException("輸入位數過長");
            }
            else if (number.Equals('.'))
            {
                IsNumeric = true;
            }
            else if (char.IsNumber(number))
            {
                int digit = int.Parse(number.ToString());             
                if (IsNumeric)
                {
                    Number = Number + ( digit * (decimal)Math.Pow(0.1, Degree + 1));
                    Degree++;
                }
                else
                {
                    Number = (10 * Number) + digit;
                }
            }
            else
            {
                throw new Exception("非正規數字");
            }
        }

        /// <summary>
        /// 清除一個位數
        /// </summary>
        public void ClearOneDigit()
        {
            if (IsNumeric)
            {
                if (Degree == 0)
                {
                    IsNumeric = false;
                }
                else
                {
                    var a = (int)(Number * (decimal)Math.Pow(10, Degree - 1));
                    Number = a / (decimal)Math.Pow(10, Degree - 1);
                    Degree -= 1;
                }
            }
            else
            {
                Number = (int)(Number / 10);
            }
        }

        /// <summary>
        /// 判斷是否為整數
        /// </summary>
        /// <returns>是整數的布林值</returns>
        private bool IsInteger()
        {
            return (Number == (int)Number);
        }

        /// <summary>
        /// 是否以小數點為最後一位。
        /// </summary>
        /// <returns>是以小數點為最後一位的布林值</returns>
        public bool IsEndWithPoint()
        {
            return IsNumeric && IsInteger();
        }
    }
}