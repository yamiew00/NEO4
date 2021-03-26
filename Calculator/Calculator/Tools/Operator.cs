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
        public NumberField BaseNumber;
        public NumberField LastResult;
        public NumberField LastInput { get; set; }
        public IOperator OperateSelector { get; set; }
        //奇怪的做法
        public bool IsDealingEqual{get; set;}






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

        private NumberField Compute(NumberField number1, NumberField number2)
        {
            return new NumberField(OperateSelector.Compute(number1.Value, number2.Value));
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
    }
}
