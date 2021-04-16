using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.NewThing
{
    public class Number
    {
        public decimal? Value;
        public bool IsNumeric;
        private int Degree;

        public Number()
        {
            Value = 0;
            IsNumeric = false;
            Degree = 0;
        }

        public Number(char number)
        {
            Value = int.Parse(number.ToString());
            IsNumeric = false;
            Degree = 0;
        }
        
        //這有用嗎
        //public Number(char number)
        //{
        //    if (number.Equals('p'))
        //    {
        //        Value = 0;
        //        IsNumeric = true;
        //        return;
        //    }

        //    if (char.IsNumber(number))
        //    {
        //        Value = Convert.ToInt32(number);
        //        IsNumeric = false;
        //    }
        //    else
        //    {
        //        throw new Exception("非正規數字");
        //    }
        //}


        public void AddDigit(char number)
        {
            if (number.Equals('.'))
            {
                IsNumeric = true;
            }
            else if (char.IsNumber(number))
            {
                int digit = int.Parse(number.ToString());             
                if (IsNumeric)
                {
                    Value = Value + digit * (decimal)Math.Pow(0.1, Degree + 1);
                    Degree++;
                }
                else
                {
                    Value = 10 * Value + digit;
                }
            }
            else
            {
                throw new Exception("非正規數字");
            }
        }

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
                    var a = (int)(Value * (decimal)Math.Pow(10, Degree - 1));
                    Value =  a / (decimal)Math.Pow(10, Degree - 1);
                    Degree -= 1;
                }
            }
            else
            {
                Value = (int)(Value / 10);
            }
        }

        private bool IsInteger()
        {
            return (Value == (int)Value);
        }

        public bool IsEndWithPoint()
        {
            return IsNumeric && IsInteger();
        }
    }
}