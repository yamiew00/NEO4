using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tools.OperateObject
{
    public interface IOperator
    {
        decimal Compute(decimal number1, decimal number2);
        string Mark();
    }
}
