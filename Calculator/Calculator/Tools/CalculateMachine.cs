using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.CalculateObject;
using Calculator.Tools.OperateObject;

namespace Calculator.Tools
{
    public class CalculateMachine
    {
        private static CalculateMachine calculateMachine;

        public static CalculateMachine getInstance()
        {
            if (calculateMachine == null)
            {
                calculateMachine = new CalculateMachine();
                return calculateMachine;
            }
            return calculateMachine;
        }

        public NumberField BaseNumber;
        public NumberField LastResult;
        public NumberField LastInput { get; set; }
        public IOperator OperateSelector { get; set; }
        //奇怪的做法
        public bool IsDealingEqual { get; set; }

        //傳入新數字，判斷要運算或只是存起來(因為是第一個數字)
        public NumberField Execute(NumberField number)
        {
            IsDealingEqual = false;

            ////判斷傳入的數字是否為可運算狀態
            if (number.CanNotOperated)
            {
                return LastResult;
            }

            if (LastResult == null)
            {
                return LastResult = number.EraseRedundantPoint();
            }
            else
            {
                LastInput = number.EraseRedundantPoint();
                return LastResult = Compute(LastResult, number);
            }
        }

        public string OperateExpression()
        {
            return LastResult.ToString() + OperateSelector.Mark();
        }


        public NumberField Equal(NumberField number)
        {
            ////判斷傳入的數字是否為可運算狀態
            if (number.CanNotOperated)
            {
                if (LastResult == null)
                {
                    LastResult = number.EraseRedundantPoint();

                }
                return LastResult = (OperateSelector == null) ?
                        BaseNumber = LastResult
                        : Compute(BaseNumber = LastResult, LastInput);
            }

            if (LastResult == null)
            {
                return LastResult = number.EraseRedundantPoint();
            }
            else
            {
                BaseNumber = LastResult;
                LastInput = number.EraseRedundantPoint();
                return LastResult = Compute(LastResult, number);
            }
        }

        public string EqualExpression()
        {
            return (OperateSelector == null) ?
                    LastResult.ToString() + "="
                    : BaseNumber.ToString()
                        + OperateSelector.Mark()
                        + LastInput.ToString()
                        + "=";
        }

        public void Clear()
        {
            BaseNumber = LastResult = LastInput = new NumberField();
            OperateSelector = null;
        }




        //唯一要留的
        private NumberField Compute(NumberField number1, NumberField number2)
        {
            //無限大在這裡處理好嗎
            var value = Operator.Compute(number1.Value, number2.Value);
            if (value == null)
            {
                Expression = $"{number1.ToString()} ÷ 0";
            }
            return new NumberField(value);
        }
        

        //新做法
        public NumberField LeftNumber;
        public NumberField RightNumber;

        //subPanel的顯示
        public string Expression { get; private set; }
        //panel的顯示
        public string currentNumber { get { return LeftNumber.ToString(); } } 

        //四則運算
        public IOperator Operator { get; set; }

        public void Operate(NumberField newNumber, IOperator nextOperator)
        {
            //共有4種case
            if (newNumber.isInput == false)
            {
                //如果等號後input, LeftNumber不變，並將RightNumber清空
                RightNumber = null;
                Operator = nextOperator;
            }
            else if (LeftNumber == null && Operator != null && RightNumber != null)
            {
                Console.WriteLine("case 1"); //未做
                LeftNumber = newNumber;
                Operator = nextOperator;
                RightNumber = null;
            }
            else if (LeftNumber == null && Operator == null && RightNumber == null)
            {
                //第一次輸進數字，不做運算 
                LeftNumber = newNumber;
                Operator = nextOperator;
            }
            else if (LeftNumber != null && Operator != null && RightNumber == null)
            {
                //左數 四則運算 右數
                LeftNumber = Compute(LeftNumber, newNumber);
                Operator = nextOperator;
            }

            //這個階段若LeftNumber.NaN 為null ，代表除到0了
            if (!LeftNumber.NaN)
            {
                Expression = $"{LeftNumber.ToString()} {nextOperator.Mark()}";
            }
            
            //無限大處理，後續還未做
            if (LeftNumber.NaN)
            {
                Expression += $"{Operator.Mark()}";
                //Refresh(); 
            }
        }

        public void EqualEvent(NumberField newNumber)
        {
            //共有五種case
            if ( LeftNumber == null && Operator != null && RightNumber != null)
            {
                Console.WriteLine("Equal case 1");
                Expression = LongExp(newNumber, RightNumber);
                LeftNumber = Compute(newNumber, RightNumber) ?? LeftNumber;         //是無限大的話值要暫時留著
            }
            else if (LeftNumber == null && Operator == null && RightNumber == null)
            {
                //第一次輸入
                //和下一種高度相似，可以合併
                Expression = ShortExp(newNumber);
                LeftNumber = newNumber;
            }
            else if (LeftNumber != null && Operator == null && RightNumber == null)
            {
                //無運算符，相當於第一次輸入
                Expression = ShortExp(newNumber);
                LeftNumber = newNumber;
            }
            else if (LeftNumber != null && Operator != null && RightNumber == null)
            {
                //兩數運算
                Expression = LongExp(LeftNumber, newNumber);
                LeftNumber = Compute(LeftNumber, newNumber) ?? LeftNumber;          //是無限大的話值要暫時留著
                RightNumber = newNumber;
            }
            else if (LeftNumber != null && Operator != null && RightNumber != null)
            {
                //等號連按，相當於重複計算LeftNumber與RightNumber，按當前的Operator做
                Expression = LongExp(LeftNumber, RightNumber);
                LeftNumber = Compute(LeftNumber, RightNumber) ?? LeftNumber;        //是無限大的話值要暫時留著
            }

            //無限大處理
            if (LeftNumber.NaN)
            {
                Expression = $"{LeftNumber.ToString()} ÷ ";
                Refresh();
            }
        }

        private void Refresh()
        {
            LeftNumber = null;
            Operator = null;
            RightNumber = null;
        }

        private string LongExp(NumberField Number1, NumberField Number2)
        {
            //等號符號的處理可以更好(?
            return Number1 + " " + Operator.Mark() + " " + Number2 + " =";
        }
        private string ShortExp(NumberField Number)
        {
            //等號符號的處理可以更好(?
            return Number + " =";
        }
    }
}
