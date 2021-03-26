using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tools.OperateObject
{
    public class Minus : IOperator
    {
        public decimal Compute(decimal number1, decimal number2)
        {
            return number1 - number2;
        }

        public string Mark()
        {
            return "-";
        }
    }
}
