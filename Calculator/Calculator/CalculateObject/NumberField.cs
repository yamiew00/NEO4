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
        /// <summary>
        /// 分隔用符號
        /// </summary>
        private readonly char COMMA = ',';

        /// <summary>
        /// 現有的值
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 小數位數
        /// </summary>
        private int DegreeOfDecimal;

        /// <summary>
        /// 是否有小數點
        /// </summary>
        private bool DecimalPoint;
        
        /// <summary>
        /// 判斷是否為使用者所輸入
        /// </summary>
        public bool isInput { get; private set; }

        /// <summary>
        /// 判斷是不是無限大(被除以零)
        /// </summary>
        public bool NaN {  get; private set; }

        //建構子
        public NumberField()
        {
            Value = 0;
            DegreeOfDecimal = 0;
            DecimalPoint = false;
            isInput = false;
            NaN = false;
        }

        //建構子，被視為自使用者輸入，輸入指定數值，傳入null代表這不是一個數(無限大)
        public NumberField(decimal? number)
        {
            if(number == null)
            {
                NaN = true;
                return;
            }
            Value = number ?? 0;
            DegreeOfDecimal = GetDigit(number ?? 0);
            DecimalPoint = (DegreeOfDecimal == 0) ? false : true;
            isInput = true;
        }

        //輸入整數，只能是0到9
        public void Input(string unitDigit)
        {
            //被輸入過
            isInput = true;

            decimal unit;
            if (!decimal.TryParse(unitDigit, out unit)){
                return;
            }
            
            //限制輸入
            if(unit < 0 || unit > 9)
            {
                throw new Exception("只能輸入個位數");
            }

            //整數或小數模式
            if (DecimalPoint)
            {
                int digit = DegreeOfDecimal ++;
                for (int i = 0; i < digit + 1; i++)
                {
                    unit /= 10;
                }
                Value = Value + unit;
            }
            else
            {
                Value = Value * 10 + unit;
            }
            
        }

        public void AddPoint()
        {
            isInput = true;
            DecimalPoint = true;
        }

        public void BackSpace()
        {
            if (!DecimalPoint)
            {
                Value = (long)(Value) / 10;
            }
            else
            {
                Value = deleteLast(Value);
                if (DegreeOfDecimal == 0)
                {
                    DecimalPoint = false;
                }
                else
                {
                    DegreeOfDecimal--;
                }
            }
        }

        //判斷小數點後有幾個位數
        private int GetDigit(decimal number)
        {
            int digit = 0;
            while (!IsInteger(number))
            {
                digit++;
                number *= 10;
            }
            return digit;
        }

        private int GetDigit(long number)
        {
            int digit = 1;
            while(number > 10)
            {
                number /= 10;
                digit++;
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
        
        
        //移除小數點後的最後一個位數
        private decimal deleteLast(decimal number)
        {
            int digit = GetDigit(number);
            if (digit == 0)
            {
                return number;
            }

            for (int i = 0; i < digit - 1; i++)
            {
                number *= 10;
            }

            number = Math.Floor(number);

            for (int i = 0; i < digit - 1; i++)
            {
                number /= 10;
            }
            return number;
        }

        //toString
        public override string ToString()
        {
            if (NaN)
            {
                return "無法除以零";
            }

            decimal abs = Math.Abs(Value);
            
            //stack
            Stack<string> stack = new Stack<string>();

            //小數部位
            if (DecimalPoint)
            {
                //非零的部分
                int nonZeroDigit = GetDigit(Value);
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

            stack.Push(((long)abs % 1000).ToString());
            int a = GetDigit((long)abs % 1000);
            while (abs >= 1000)
            {
                //補零
                for (int i = 0; i < 3 - a; i++)
                {
                    stack.Push("0");
                }

                abs /= 1000;
                stack.Push(((long)abs % 1000).ToString() + COMMA);
                a = GetDigit((long)abs % 1000);
            }

            //正負號
            if (Value < 0)
            {
                stack.Push("-");
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
