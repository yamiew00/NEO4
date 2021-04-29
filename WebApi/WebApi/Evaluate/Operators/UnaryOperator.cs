using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Evaluate.Operators
{
    /// <summary>
    /// 單元運算符
    /// </summary>
    public class UnaryOperator
    {
        /// <summary>
        /// 運算規則
        /// </summary>
        public Func<decimal, decimal> Formula { get; private set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public char Name { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="formula">運算規則</param>
        /// <param name="name">名稱</param>
        public UnaryOperator(Func<decimal, decimal> formula, char name)
        {
            Formula = formula;
            Name = name;
        }
    }
}