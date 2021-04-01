using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculateObjects
{
    /// <summary>
    /// 單元運算子
    /// </summary>
    public class UnaryOperator
    {
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 運算規則
        /// </summary>
        public Func<decimal, decimal> Formula { get; private set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="priority">優先度</param>
        /// <param name="formula">運算規則</param>
        public UnaryOperator(int priority, Func<decimal, decimal> formula)
        {
            Priority = priority;
            Formula = formula;
        }
    }
}
