using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculateObjects
{
    /// <summary>
    /// 特殊符號
    /// </summary>
    public class WildSymbol
    {
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="priority">優先度</param>
        public WildSymbol(int priority)
        {
            Priority = priority;
        }
    }
}
