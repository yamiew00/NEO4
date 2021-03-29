using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tools.OperateObject
{
    public class Divide : IOperator
    {
        public decimal? Compute(decimal number1, decimal number2)
        {
            return (number2 == 0)? null : (number1 / number2) as decimal?;
        }

        public string Mark()
        {
            return "÷";
        }
    }
}
