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
        
        /// <summary>
        /// 運算規則
        /// </summary>
        public Func<decimal, decimal, decimal> Formula { get; private set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="priority">優先度</param>
        /// <param name="formula">運算規則</param>
        public BinaryOperator(int priority, Func<decimal, decimal, decimal> formula)
        {
            Priority = priority;
            Formula = formula;
        }
    }
}