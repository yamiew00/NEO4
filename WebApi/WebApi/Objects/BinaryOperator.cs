using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    /// <summary>
    /// 雙元運算子
    /// </summary>
    public class BinaryOperator
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

        public BinaryOperator(int priority, char name, Func<decimal, decimal, decimal> formula)
        {
            Priority = priority;
            Name = name;
            Formula = formula;
        }
    }
}