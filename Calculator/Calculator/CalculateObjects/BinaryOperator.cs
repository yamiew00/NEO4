using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculateObjects
{
    public class BinaryOperator
    {
        public int Priority;
        public Func<decimal, decimal, decimal> Formula;

        public BinaryOperator(int priority, Func<decimal, decimal, decimal> formula)
        {
            Priority = priority;
            Formula = formula;
        }

        public BinaryOperator(int priority)
        {
            Priority = priority;
        }
    }
}
