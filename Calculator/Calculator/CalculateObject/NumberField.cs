using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculateObject
{
    public class NumberField
    {
        private readonly char COMMA = ',';
        public decimal Value { get; set; }

        private bool IsPositive = true;

        private int DegreeOfDecimal = 0;

        private bool DecimalPoint = false;

        //(不算敗因，但這個功能要整個砍掉)只有當這個數字是被計算過後的值，或者是由使用者輸入的值，才可以被運算。(初始生成的不能做運算)
        public bool CanNotOperated { get; private set;}

        public bool isInput { get; private set; }

        //新做法
        public bool NaN {  get; private set; }

        //建構子
        public NumberField()
        {
            Value = 0;
            CanNotOperated = true;
            isInput = false;
        }

        //建構子，輸入指定數值，傳入null代表這不是一個數(無限大)
        public NumberField(decimal? number)
        {
            if(number == null)
            {
                NaN = true;
                return;
            }
            Value = number ?? 0;
            //IsPositive 還沒做
            DegreeOfDecimal = getDigit(number ?? 0);
            DecimalPoint = (DegreeOfDecimal == 0) ? 
                false
                : (number.ToString().Split('.').Count() == 2) ? true : false;
            CanNotOperated = false;
            //isInput是true嗎
            isInput = true;
        }
        
        

        //輸入整數
        public void Input(string unitDigit)
        {
            //被輸入過
            isInput = true;

            int unit = 0;
            //如果非數字→先有小數點
            if(!int.TryParse(unitDigit, out unit))
            {
                if (unitDigit.Equals("."))
                {
                    DecimalPoint = true;
                }
                //更改為可運算
                CanNotOperated = false;
                return;
            }

            //限制輸入機制
            if(unit < 0 || unit > 9)
            {
                throw new Exception("只能輸入個位數");
            }

            //更改為可運算
            CanNotOperated = false;

            //整數或小數模式
            if (DecimalPoint)
            {
                decimal decimalUnitDigit = unit;
                int digit = DegreeOfDecimal ++;
                
                for (int i = 0; i < digit + 1; i++)
                {
                    decimalUnitDigit /= 10;
                }
                Value = Value + decimalUnitDigit;
            }
            else
            {
                Value = Value * 10 + unit;
            }
            
        }

        //判斷小數點後有幾個位數
        private int getDigit(decimal number)
        {
            int digit = 0;
            while (!IsInteger(number))
            {
                digit++;
                number *= 10;
            }
            return digit;
        }

        //判斷是否為整數
        private bool IsInteger(decimal number)
        {
            if((long)number == number)
            {
                return true;
            }
            return false;
        }

        //回傳所有小數的非零部分
        private string GetDecimal(decimal number)
        {
            var splitting = number.ToString().Split('.');
            if (splitting.Count() == 2)
            {
                return splitting[1];
            }
            else
            {
                return null;
            }
        }

        //回傳一個需要去除掉小數點的數字
        public NumberField EraseRedundantPoint()
        {
            if (DegreeOfDecimal == 0)
            {
                DecimalPoint = false;
            }
            return this;
        }

        //toString
        public override string ToString()
        {
            if (NaN)
            {
                return "無法除以零";
            }

            decimal tmp = Value;
            
            //stack
            Stack<string> stack = new Stack<string>();

            //小數部位
            if (DecimalPoint)
            {
                //非零的部分
                int nonZeroDigit = getDigit(Value);
                //不足則必須補零
                while (nonZeroDigit++ < DegreeOfDecimal)
                {
                    stack.Push("0");
                }
                //非零部分
                stack.Push(GetDecimal(Value));

                //小數點
                stack.Push(".");
                
            }
            

            //整數部位
            stack.Push(((long)tmp % 1000).ToString());
            while(tmp > 1000)
            {
                tmp /= 1000;
                stack.Push(((long)tmp % 1000).ToString() + COMMA);
            }

            //文字
            string str = "";
            while(stack.Count > 0)
            {
                var tmpStr = stack.Pop();
                str += tmpStr;
            }

            
            return str;
        }
    }
}
