using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    public class UnaryOperator
    {
        public char Name { get; set; }
        /// <summary>
        /// 運算規則
        /// </summary>
        public Func<decimal, decimal> Formula { get; private set; }

        public UnaryOperator(char name, Func<decimal, decimal> formula)
        {
            Name = name;
            Formula = formula;
        }
    }
}