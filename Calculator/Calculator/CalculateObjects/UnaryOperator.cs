using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculateObjects
{
    public class UnaryOperator
    {
        public int Priority;
        public Func<decimal, decimal> Formula;

        public UnaryOperator(int priority, Func<decimal, decimal> formula)
        {
            Priority = priority;
            Formula = formula;
        }
    }
}
