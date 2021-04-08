using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.NewTrees
{
    public class NewBinary
    {
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; set; }

        public char Name { get; set; }
        /// <summary>
        /// 運算規則
        /// </summary>
        public Func<decimal, decimal, decimal> Formula { get; private set; }

        public NewBinary(int priority, char name, Func<decimal, decimal, decimal> formula)
        {
            Priority = priority;
            Name = name;
            Formula = formula;
        }
    }
}
