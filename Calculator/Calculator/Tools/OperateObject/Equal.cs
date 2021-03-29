using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tools.OperateObject
{
    public class Equal : IOperator
    {
        public decimal? Compute(decimal number1, decimal number2)
        {
            throw new NotImplementedException();
        }

        public string Mark()
        {
            return "=";
        }
    }
}
